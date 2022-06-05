using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200044E RID: 1102
public class IceCrystal_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x0600289E RID: 10398 RVA: 0x00086355 File Offset: 0x00084555
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_initialScale = base.transform.localScale.x;
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x0008637F File Offset: 0x0008457F
	private IEnumerator Start()
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		this.m_weaponCollider = (this.m_hbController.GetCollider(HitboxType.Weapon) as CircleCollider2D);
		this.m_bodyCollider = (this.m_hbController.GetCollider(HitboxType.Body) as CircleCollider2D);
		if (!this.m_isGrowing)
		{
			base.StartCoroutine(this.InitialGrowIceCrystalCoroutine());
		}
		yield break;
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x0008638E File Offset: 0x0008458E
	private void OnEnable()
	{
		if (this.m_hbController.IsInitialized && !this.m_isGrowing)
		{
			base.StartCoroutine(this.InitialGrowIceCrystalCoroutine());
		}
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x000863B2 File Offset: 0x000845B2
	private IEnumerator InitialGrowIceCrystalCoroutine()
	{
		this.m_isPaused = false;
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, 1f);
		float delay = Time.time + 0.75f;
		while (Time.time < delay)
		{
			yield return null;
		}
		yield return this.GrowIceCrystalCoroutine();
		yield break;
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000863C1 File Offset: 0x000845C1
	private IEnumerator GrowIceCrystalCoroutine()
	{
		this.m_isGrowing = true;
		base.Animator.speed = 1f;
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, 1f);
		this.m_lifetimeEventEmitter.SetParameter("hazard_snowflake_active", 1f, true);
		bool weaponHitboxDisabled = true;
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		this.m_hbController.SetHitboxActiveState(HitboxType.Body, true);
		this.m_weaponCollider.radius = 3.5f;
		this.m_bodyCollider.radius = 3.5f;
		base.Animator.SetBool("Active", true);
		while (!base.Animator.GetCurrentAnimatorStateInfo(0).IsName("Grow"))
		{
			yield return null;
		}
		float growthDuration = this.m_initialScale / 0.3f;
		float totalGrowthDuration = Time.time + growthDuration;
		float speed = base.Animator.GetCurrentAnimatorStateInfo(0).length / growthDuration;
		base.Animator.speed = speed;
		float damageDelay = Time.time + 0.65f;
		while (Time.time < totalGrowthDuration)
		{
			float num = 1f - (totalGrowthDuration - Time.time) / growthDuration;
			float radius = 1.5f * num + 3.5f;
			this.m_weaponCollider.radius = radius;
			this.m_bodyCollider.radius = radius;
			if (Time.time >= damageDelay && weaponHitboxDisabled)
			{
				weaponHitboxDisabled = false;
				this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, true);
			}
			this.m_growEventEmitter.SetParameter("hazard_snowflake_size", num, false);
			yield return null;
		}
		this.m_isGrowing = false;
		yield break;
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x000863D0 File Offset: 0x000845D0
	public void PlayGrowSound()
	{
		this.m_growEventEmitter.Play();
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x000863DD File Offset: 0x000845DD
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x000863E6 File Offset: 0x000845E6
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		base.StopAllCoroutines();
		this.m_isGrowing = false;
		base.StartCoroutine(this.ShatterIceCrystalCoroutine());
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x00086402 File Offset: 0x00084602
	private IEnumerator ShatterIceCrystalCoroutine()
	{
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		this.m_hbController.SetHitboxActiveState(HitboxType.Body, false);
		base.Animator.SetBool("Active", false);
		this.m_breakEventEmitter.Play();
		this.m_lifetimeEventEmitter.SetParameter("hazard_snowflake_active", 0f, true);
		float regrowDelay = Time.time + 2.75f;
		while (Time.time < regrowDelay)
		{
			yield return null;
		}
		base.StartCoroutine(this.GrowIceCrystalCoroutine());
		yield break;
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x00086411 File Offset: 0x00084611
	public void SetIceCrystalPaused(bool pause)
	{
		if (this.m_isPaused == pause)
		{
			return;
		}
		this.m_isPaused = pause;
		if (pause)
		{
			this.BodyOnEnterHitResponse(null);
			base.StopAllCoroutines();
			return;
		}
		base.StartCoroutine(this.GrowIceCrystalCoroutine());
	}

	// Token: 0x060028A8 RID: 10408 RVA: 0x00086444 File Offset: 0x00084644
	public override void ResetHazard()
	{
		if (this.m_weaponCollider)
		{
			this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
			this.m_hbController.SetHitboxActiveState(HitboxType.Body, true);
			this.m_weaponCollider.radius = 3.5f;
			this.m_bodyCollider.radius = 3.5f;
		}
		base.Animator.SetBool("Active", false);
		base.transform.localScale = new Vector3(this.m_initialScale, this.m_initialScale, 1f);
		this.m_isGrowing = false;
		base.Animator.speed = 1f;
		this.m_hitEffects.SetActive(false);
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000864F5 File Offset: 0x000846F5
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002189 RID: 8585
	public const float ICE_CRYSTAL_MIN_RADIUS = 3.5f;

	// Token: 0x0400218A RID: 8586
	public const float ICE_CRYSTAL_MAX_RADIUS = 5f;

	// Token: 0x0400218B RID: 8587
	[SerializeField]
	public UnityEvent TriggeredEvent;

	// Token: 0x0400218C RID: 8588
	[SerializeField]
	private GameObject m_hitEffects;

	// Token: 0x0400218D RID: 8589
	[SerializeField]
	private StudioEventEmitter m_breakEventEmitter;

	// Token: 0x0400218E RID: 8590
	[SerializeField]
	private StudioEventEmitter m_lifetimeEventEmitter;

	// Token: 0x0400218F RID: 8591
	[SerializeField]
	private StudioEventEmitter m_growEventEmitter;

	// Token: 0x04002190 RID: 8592
	private IHitboxController m_hbController;

	// Token: 0x04002191 RID: 8593
	private CircleCollider2D m_weaponCollider;

	// Token: 0x04002192 RID: 8594
	private CircleCollider2D m_bodyCollider;

	// Token: 0x04002193 RID: 8595
	private bool m_isGrowing;

	// Token: 0x04002194 RID: 8596
	private float m_initialScale;

	// Token: 0x04002195 RID: 8597
	private bool m_isPaused;
}
