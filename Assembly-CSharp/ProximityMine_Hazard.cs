using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x02000733 RID: 1843
public class ProximityMine_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001515 RID: 5397
	// (get) Token: 0x0600386D RID: 14445 RVA: 0x0001EF80 File Offset: 0x0001D180
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"ProximityMineExplosionProjectile",
					"ProximityMineWarningProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x0001EFAC File Offset: 0x0001D1AC
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x000E84E0 File Offset: 0x000E66E0
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		if (pointHazardArgs != null)
		{
			this.m_explosionRadius = pointHazardArgs.Radius * 2f;
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
		}
		(this.m_hbController.GetCollider(HitboxType.Terrain) as CircleCollider2D).radius = 5f;
		this.ResetHazard();
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x0001EFD1 File Offset: 0x0001D1D1
	private IEnumerator FireProjectile()
	{
		this.m_state = ProximityMine_Hazard.ProximityMineState.Activated;
		base.Animator.SetBool("Active", true);
		this.m_triggerAudioEventEmitter.Play();
		this.m_warningProjectile = ProjectileManager.FireProjectile(base.gameObject, "ProximityMineWarningProjectile", Vector2.zero, false, 0f, 0f, false, true, true, true);
		this.m_warningProjectile.transform.localScale = new Vector3(this.m_explosionRadius, this.m_explosionRadius, this.m_warningProjectile.transform.localScale.z);
		this.m_waitYield.CreateNew(2.25f, false);
		yield return this.m_waitYield;
		this.m_warningProjectile.FlagForDestruction(null);
		this.m_warningProjectile = null;
		Projectile_RL projectile_RL = ProjectileManager.FireProjectile(base.gameObject, "ProximityMineExplosionProjectile", Vector2.zero, false, 0f, 0f, false, true, true, true);
		projectile_RL.transform.localScale = new Vector3(this.m_explosionRadius, this.m_explosionRadius, projectile_RL.transform.localScale.z);
		base.Animator.SetBool("Attacking", true);
		this.m_explodeAudioEventEmitter.Play();
		this.m_waitYield.CreateNew(projectile_RL.Lifespan, false);
		yield return this.m_waitYield;
		this.m_state = ProximityMine_Hazard.ProximityMineState.CoolingDown;
		base.Animator.SetBool("Attacking", false);
		this.m_waitYield.CreateNew(7.5f, false);
		yield return this.m_waitYield;
		this.ResetHazard();
		yield break;
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x0001EFE0 File Offset: 0x0001D1E0
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == ProximityMine_Hazard.ProximityMineState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x000E8544 File Offset: 0x000E6744
	public override void ResetHazard()
	{
		this.m_state = ProximityMine_Hazard.ProximityMineState.Idle;
		base.Animator.SetBool("Active", false);
		base.Animator.SetBool("Attacking", false);
		if (this.m_warningProjectile && this.m_warningProjectile.isActiveAndEnabled)
		{
			this.m_warningProjectile.gameObject.SetActive(false);
		}
		this.m_warningProjectile = null;
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x000E85AC File Offset: 0x000E67AC
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_warningProjectile && this.m_warningProjectile.isActiveAndEnabled)
		{
			this.m_warningProjectile.gameObject.SetActive(false);
		}
		this.m_warningProjectile = null;
		if (!GameManager.IsApplicationClosing && base.Animator)
		{
			base.Animator.WriteDefaultValues();
		}
	}

	// Token: 0x06003875 RID: 14453 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002D40 RID: 11584
	[SerializeField]
	private StudioEventEmitter m_triggerAudioEventEmitter;

	// Token: 0x04002D41 RID: 11585
	[SerializeField]
	private StudioEventEmitter m_explodeAudioEventEmitter;

	// Token: 0x04002D42 RID: 11586
	private const string EXPLOSION_PROJECTILE_NAME = "ProximityMineExplosionProjectile";

	// Token: 0x04002D43 RID: 11587
	private const string WARNING_PROJECTILE_NAME = "ProximityMineWarningProjectile";

	// Token: 0x04002D44 RID: 11588
	private float m_explosionRadius;

	// Token: 0x04002D45 RID: 11589
	private Projectile_RL m_warningProjectile;

	// Token: 0x04002D46 RID: 11590
	private IHitboxController m_hbController;

	// Token: 0x04002D47 RID: 11591
	private ProximityMine_Hazard.ProximityMineState m_state;

	// Token: 0x04002D48 RID: 11592
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002D49 RID: 11593
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000734 RID: 1844
	private enum ProximityMineState
	{
		// Token: 0x04002D4B RID: 11595
		Idle,
		// Token: 0x04002D4C RID: 11596
		Activated,
		// Token: 0x04002D4D RID: 11597
		CoolingDown
	}
}
