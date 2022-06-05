using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class ProximityMine_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001006 RID: 4102
	// (get) Token: 0x060028D5 RID: 10453 RVA: 0x00087170 File Offset: 0x00085370
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

	// Token: 0x060028D6 RID: 10454 RVA: 0x0008719C File Offset: 0x0008539C
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000871C4 File Offset: 0x000853C4
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

	// Token: 0x060028D8 RID: 10456 RVA: 0x00087226 File Offset: 0x00085426
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

	// Token: 0x060028D9 RID: 10457 RVA: 0x00087235 File Offset: 0x00085435
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == ProximityMine_Hazard.ProximityMineState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x0008724C File Offset: 0x0008544C
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

	// Token: 0x060028DB RID: 10459 RVA: 0x000872B4 File Offset: 0x000854B4
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

	// Token: 0x060028DD RID: 10461 RVA: 0x00087320 File Offset: 0x00085520
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040021B2 RID: 8626
	[SerializeField]
	private StudioEventEmitter m_triggerAudioEventEmitter;

	// Token: 0x040021B3 RID: 8627
	[SerializeField]
	private StudioEventEmitter m_explodeAudioEventEmitter;

	// Token: 0x040021B4 RID: 8628
	private const string EXPLOSION_PROJECTILE_NAME = "ProximityMineExplosionProjectile";

	// Token: 0x040021B5 RID: 8629
	private const string WARNING_PROJECTILE_NAME = "ProximityMineWarningProjectile";

	// Token: 0x040021B6 RID: 8630
	private float m_explosionRadius;

	// Token: 0x040021B7 RID: 8631
	private Projectile_RL m_warningProjectile;

	// Token: 0x040021B8 RID: 8632
	private IHitboxController m_hbController;

	// Token: 0x040021B9 RID: 8633
	private ProximityMine_Hazard.ProximityMineState m_state;

	// Token: 0x040021BA RID: 8634
	private WaitRL_Yield m_waitYield;

	// Token: 0x040021BB RID: 8635
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000C59 RID: 3161
	private enum ProximityMineState
	{
		// Token: 0x04005023 RID: 20515
		Idle,
		// Token: 0x04005024 RID: 20516
		Activated,
		// Token: 0x04005025 RID: 20517
		CoolingDown
	}
}
