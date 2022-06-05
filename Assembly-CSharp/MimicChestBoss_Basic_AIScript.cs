using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class MimicChestBoss_Basic_AIScript : MimicChest_Basic_AIScript, IAudioEventEmitter
{
	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00006CD7 File Offset: 0x00004ED7
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.65f, 0.9f);
		}
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00006CE8 File Offset: 0x00004EE8
	protected override string JumpAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_jump";
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00006CEF File Offset: 0x00004EEF
	protected override string LandAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_land";
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00006CF6 File Offset: 0x00004EF6
	protected override string DeathHitAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_death_hit";
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float m_airborneKO_AnimationDelay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00003CEB File Offset: 0x00001EEB
	protected override float m_dashAttackSpeed
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00006CFD File Offset: 0x00004EFD
	protected override Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(5f, 5f);
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00006D0E File Offset: 0x00004F0E
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(21f, 29f);
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00006D1F File Offset: 0x00004F1F
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"MimicCoinProjectile",
			"MimicBossPotionProjectile",
			"MimicBossDashTrailProjectile",
			"MimicBossBounceBoltProjectile",
			"MimicBossJumpProjectile"
		};
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x00006D55 File Offset: 0x00004F55
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.LogicController.OverrideLogicDelay(0f);
		base.EnemyController.LockFlip = true;
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x00006D90 File Offset: 0x00004F90
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Intro()
	{
		base.EnemyController.AlwaysFacing = false;
		this.ToDo("INTRO");
		this.ChangeAnimationState("Neutral");
		yield return base.Wait(2f, false);
		this.ChangeAnimationState("Hint");
		yield return base.Wait(2f, false);
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Platform, true);
		base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Terrain, true);
		base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
		yield return this.Default_Attack_Cooldown(0.5f, 99999f);
		yield break;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00006D9F File Offset: 0x00004F9F
	public override IEnumerator SpawnAnim()
	{
		yield return this.Default_Animation("BossIntro_Open", 1f, 2f, true);
		yield return this.Default_Animation("BossIntro_ShakeAndClose", 1f, 1f, true);
		yield return this.Default_Animation("BossIntro_GrowlLoop", 1f, 1f, true);
		MusicManager.StopMusic();
		MusicManager.PlayMusic(SongID.StudyBossBGM_Boss5_Phase2, false, false);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, false);
		yield break;
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00006DAE File Offset: 0x00004FAE
	protected override string m_verticalShotProjectileName
	{
		get
		{
			return "MimicBossPotionProjectile";
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00006DB5 File Offset: 0x00004FB5
	protected override float m_dashAttack_Exit_AttackCD
	{
		get
		{
			return 7.5f;
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_spawnDashTrailProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float m_timeBetweenDashTrailProjectiles
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00006DBC File Offset: 0x00004FBC
	protected override void OnStartDashing()
	{
		if (!this.m_spawnDashTrailProjectiles)
		{
			return;
		}
		base.StartCoroutine(this.SpawnDashTrailProjectiles());
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00006DD4 File Offset: 0x00004FD4
	private IEnumerator SpawnDashTrailProjectiles()
	{
		float timeStart = Time.time;
		while (this.m_isDashAttacking)
		{
			if (Time.time - timeStart >= this.m_timeBetweenDashTrailProjectiles)
			{
				if (!base.IsPaused)
				{
					this.FireProjectile("MimicCoinProjectile", 3, true, 35f, 0.5f, true, true, true);
					if (base.EnemyController.IsGrounded)
					{
						if (this.m_shouldPlayGallop)
						{
							AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_mimicBoss_charge_gallop", base.gameObject);
						}
						this.m_shouldPlayGallop = !this.m_shouldPlayGallop;
					}
					else
					{
						this.m_shouldPlayGallop = true;
					}
				}
				timeStart = Time.time;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00006DE3 File Offset: 0x00004FE3
	protected override void OnStopDashing()
	{
		bool spawnDashTrailProjectiles = this.m_spawnDashTrailProjectiles;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00006DEC File Offset: 0x00004FEC
	protected override void OnDisable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.GetComponent<Interactable>().SetIsInteractableActive(false);
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		}
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00006E18 File Offset: 0x00005018
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.GetComponent<Interactable>().SetIsInteractableActive(false);
	}

	// Token: 0x04000DCB RID: 3531
	private const string COIN_PROJECTILE_NAME = "MimicCoinProjectile";

	// Token: 0x04000DCC RID: 3532
	private const string POTION_PROJECTILE_NAME = "MimicBossPotionProjectile";

	// Token: 0x04000DCD RID: 3533
	private const string DASH_TRAIL_PROJECTILE_NAME = "MimicBossDashTrailProjectile";

	// Token: 0x04000DCE RID: 3534
	private const string GALLOP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_mimicBoss_charge_gallop";

	// Token: 0x04000DCF RID: 3535
	protected const string INTRO_OPEN = "BossIntro_Open";

	// Token: 0x04000DD0 RID: 3536
	protected const string INTRO_CLOSE = "BossIntro_ShakeAndClose";

	// Token: 0x04000DD1 RID: 3537
	protected const string INTRO_GROWL = "BossIntro_GrowlLoop";

	// Token: 0x04000DD2 RID: 3538
	private bool m_shouldPlayGallop = true;
}
