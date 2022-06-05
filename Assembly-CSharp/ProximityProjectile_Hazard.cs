using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class ProximityProjectile_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001007 RID: 4103
	// (get) Token: 0x060028DE RID: 10462 RVA: 0x00087328 File Offset: 0x00085528
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"ProximityDashBoltProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x0008734C File Offset: 0x0008554C
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x00087371 File Offset: 0x00085571
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		(this.m_hbController.GetCollider(HitboxType.Terrain) as CircleCollider2D).radius = 5f;
		this.ResetHazard();
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x0008739B File Offset: 0x0008559B
	private IEnumerator FireProjectile()
	{
		this.m_state = ProximityProjectile_Hazard.ProximityMineState.Activated;
		base.Animator.SetBool("Active", true);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_voidMushroom_trigger", base.transform.position);
		this.m_waitYield.CreateNew(1.25f, false);
		yield return this.m_waitYield;
		float num = 0f;
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			num = CDGHelper.AngleBetweenPts(base.transform.position, PlayerManager.GetPlayerController().Midpoint);
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_voidMushroom_explode", base.transform.position);
		for (int i = 0; i < 360; i += 36)
		{
			ProjectileManager.FireProjectile(base.gameObject, "ProximityDashBoltProjectile", Vector2.zero, false, num + (float)i, 1f, false, true, true, true);
		}
		base.Animator.SetBool("Attacking", true);
		this.m_waitYield.CreateNew(0.1f, false);
		yield return this.m_waitYield;
		this.m_state = ProximityProjectile_Hazard.ProximityMineState.CoolingDown;
		base.Animator.SetBool("Attacking", false);
		this.m_waitYield.CreateNew(8.5f, false);
		yield return this.m_waitYield;
		this.ResetHazard();
		yield break;
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000873AA File Offset: 0x000855AA
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == ProximityProjectile_Hazard.ProximityMineState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000873C1 File Offset: 0x000855C1
	public override void ResetHazard()
	{
		this.m_state = ProximityProjectile_Hazard.ProximityMineState.Idle;
		base.Animator.SetBool("Active", false);
		base.Animator.SetBool("Attacking", false);
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000873F4 File Offset: 0x000855F4
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040021BC RID: 8636
	private const string PROJECTILE_NAME = "ProximityDashBoltProjectile";

	// Token: 0x040021BD RID: 8637
	private const string SFX_TRIGGER_NAME = "event:/SFX/Enemies/sfx_hazard_voidMushroom_trigger";

	// Token: 0x040021BE RID: 8638
	private const string SFX_EXPLODE_NAME = "event:/SFX/Enemies/sfx_hazard_voidMushroom_explode";

	// Token: 0x040021BF RID: 8639
	private ProximityProjectile_Hazard.ProximityMineState m_state;

	// Token: 0x040021C0 RID: 8640
	private WaitRL_Yield m_waitYield;

	// Token: 0x040021C1 RID: 8641
	private IHitboxController m_hbController;

	// Token: 0x040021C2 RID: 8642
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000C5B RID: 3163
	private enum ProximityMineState
	{
		// Token: 0x0400502A RID: 20522
		Idle,
		// Token: 0x0400502B RID: 20523
		Activated,
		// Token: 0x0400502C RID: 20524
		CoolingDown
	}
}
