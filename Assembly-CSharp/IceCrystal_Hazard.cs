using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000726 RID: 1830
public class IceCrystal_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x06003806 RID: 14342 RVA: 0x0001EBE9 File Offset: 0x0001CDE9
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_initialScale = base.transform.localScale.x;
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x0001EC13 File Offset: 0x0001CE13
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

	// Token: 0x06003808 RID: 14344 RVA: 0x0001EC22 File Offset: 0x0001CE22
	private void OnEnable()
	{
		if (this.m_hbController.IsInitialized && !this.m_isGrowing)
		{
			base.StartCoroutine(this.InitialGrowIceCrystalCoroutine());
		}
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0001EC46 File Offset: 0x0001CE46
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

	// Token: 0x0600380A RID: 14346 RVA: 0x0001EC55 File Offset: 0x0001CE55
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

	// Token: 0x0600380B RID: 14347 RVA: 0x0001EC64 File Offset: 0x0001CE64
	public void PlayGrowSound()
	{
		this.m_growEventEmitter.Play();
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x0001EC71 File Offset: 0x0001CE71
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0001EC7A File Offset: 0x0001CE7A
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		base.StopAllCoroutines();
		this.m_isGrowing = false;
		base.StartCoroutine(this.ShatterIceCrystalCoroutine());
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0001EC96 File Offset: 0x0001CE96
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

	// Token: 0x0600380F RID: 14351 RVA: 0x0001ECA5 File Offset: 0x0001CEA5
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

	// Token: 0x06003810 RID: 14352 RVA: 0x000E71F4 File Offset: 0x000E53F4
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

	// Token: 0x06003812 RID: 14354 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CF7 RID: 11511
	public const float ICE_CRYSTAL_MIN_RADIUS = 3.5f;

	// Token: 0x04002CF8 RID: 11512
	public const float ICE_CRYSTAL_MAX_RADIUS = 5f;

	// Token: 0x04002CF9 RID: 11513
	[SerializeField]
	public UnityEvent TriggeredEvent;

	// Token: 0x04002CFA RID: 11514
	[SerializeField]
	private GameObject m_hitEffects;

	// Token: 0x04002CFB RID: 11515
	[SerializeField]
	private StudioEventEmitter m_breakEventEmitter;

	// Token: 0x04002CFC RID: 11516
	[SerializeField]
	private StudioEventEmitter m_lifetimeEventEmitter;

	// Token: 0x04002CFD RID: 11517
	[SerializeField]
	private StudioEventEmitter m_growEventEmitter;

	// Token: 0x04002CFE RID: 11518
	private IHitboxController m_hbController;

	// Token: 0x04002CFF RID: 11519
	private CircleCollider2D m_weaponCollider;

	// Token: 0x04002D00 RID: 11520
	private CircleCollider2D m_bodyCollider;

	// Token: 0x04002D01 RID: 11521
	private bool m_isGrowing;

	// Token: 0x04002D02 RID: 11522
	private float m_initialScale;

	// Token: 0x04002D03 RID: 11523
	private bool m_isPaused;
}
