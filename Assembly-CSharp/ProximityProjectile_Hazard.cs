using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000736 RID: 1846
public class ProximityProjectile_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17001518 RID: 5400
	// (get) Token: 0x0600387C RID: 14460 RVA: 0x0001F00E File Offset: 0x0001D20E
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

	// Token: 0x0600387D RID: 14461 RVA: 0x0001F032 File Offset: 0x0001D232
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0001F057 File Offset: 0x0001D257
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		(this.m_hbController.GetCollider(HitboxType.Terrain) as CircleCollider2D).radius = 5f;
		this.ResetHazard();
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0001F081 File Offset: 0x0001D281
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

	// Token: 0x06003880 RID: 14464 RVA: 0x0001F090 File Offset: 0x0001D290
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_state == ProximityProjectile_Hazard.ProximityMineState.Idle)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x0001F0A7 File Offset: 0x0001D2A7
	public override void ResetHazard()
	{
		this.m_state = ProximityProjectile_Hazard.ProximityMineState.Idle;
		base.Animator.SetBool("Active", false);
		base.Animator.SetBool("Attacking", false);
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002D51 RID: 11601
	private const string PROJECTILE_NAME = "ProximityDashBoltProjectile";

	// Token: 0x04002D52 RID: 11602
	private const string SFX_TRIGGER_NAME = "event:/SFX/Enemies/sfx_hazard_voidMushroom_trigger";

	// Token: 0x04002D53 RID: 11603
	private const string SFX_EXPLODE_NAME = "event:/SFX/Enemies/sfx_hazard_voidMushroom_explode";

	// Token: 0x04002D54 RID: 11604
	private ProximityProjectile_Hazard.ProximityMineState m_state;

	// Token: 0x04002D55 RID: 11605
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002D56 RID: 11606
	private IHitboxController m_hbController;

	// Token: 0x04002D57 RID: 11607
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x02000737 RID: 1847
	private enum ProximityMineState
	{
		// Token: 0x04002D59 RID: 11609
		Idle,
		// Token: 0x04002D5A RID: 11610
		Activated,
		// Token: 0x04002D5B RID: 11611
		CoolingDown
	}
}
