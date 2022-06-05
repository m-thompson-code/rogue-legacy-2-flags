using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class MimicChestBoss_Basic_AIScript : MimicChest_Basic_AIScript, IAudioEventEmitter
{
	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001A8A3 File Offset: 0x00018AA3
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06000782 RID: 1922 RVA: 0x0001A8B4 File Offset: 0x00018AB4
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.65f, 0.9f);
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001A8C5 File Offset: 0x00018AC5
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06000784 RID: 1924 RVA: 0x0001A8D6 File Offset: 0x00018AD6
	protected override string JumpAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_jump";
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001A8DD File Offset: 0x00018ADD
	protected override string LandAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_land";
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001A8E4 File Offset: 0x00018AE4
	protected override string DeathHitAudioEventPath
	{
		get
		{
			return "event:/SFX/Enemies/sfx_mimicBoss_death_hit";
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001A8EB File Offset: 0x00018AEB
	protected override float m_airborneKO_AnimationDelay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06000788 RID: 1928 RVA: 0x0001A8F2 File Offset: 0x00018AF2
	protected override float m_dashAttackSpeed
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001A8F9 File Offset: 0x00018AF9
	protected override Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(5f, 5f);
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x0001A90A File Offset: 0x00018B0A
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(21f, 29f);
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0001A91B File Offset: 0x00018B1B
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

	// Token: 0x0600078C RID: 1932 RVA: 0x0001A951 File Offset: 0x00018B51
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.LogicController.OverrideLogicDelay(0f);
		base.EnemyController.LockFlip = true;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0001A98C File Offset: 0x00018B8C
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

	// Token: 0x0600078E RID: 1934 RVA: 0x0001A99B File Offset: 0x00018B9B
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

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x0001A9AA File Offset: 0x00018BAA
	protected override string m_verticalShotProjectileName
	{
		get
		{
			return "MimicBossPotionProjectile";
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x0001A9B1 File Offset: 0x00018BB1
	protected override float m_dashAttack_Exit_AttackCD
	{
		get
		{
			return 7.5f;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x0001A9B8 File Offset: 0x00018BB8
	protected virtual bool m_spawnDashTrailProjectiles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001A9BB File Offset: 0x00018BBB
	protected virtual float m_timeBetweenDashTrailProjectiles
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0001A9C2 File Offset: 0x00018BC2
	protected override void OnStartDashing()
	{
		if (!this.m_spawnDashTrailProjectiles)
		{
			return;
		}
		base.StartCoroutine(this.SpawnDashTrailProjectiles());
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0001A9DA File Offset: 0x00018BDA
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

	// Token: 0x06000795 RID: 1941 RVA: 0x0001A9E9 File Offset: 0x00018BE9
	protected override void OnStopDashing()
	{
		bool spawnDashTrailProjectiles = this.m_spawnDashTrailProjectiles;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0001A9F2 File Offset: 0x00018BF2
	protected override void OnDisable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.GetComponent<Interactable>().SetIsInteractableActive(false);
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0001AA1E File Offset: 0x00018C1E
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		base.EnemyController.GetComponent<Interactable>().SetIsInteractableActive(false);
	}

	// Token: 0x04000B23 RID: 2851
	private const string COIN_PROJECTILE_NAME = "MimicCoinProjectile";

	// Token: 0x04000B24 RID: 2852
	private const string POTION_PROJECTILE_NAME = "MimicBossPotionProjectile";

	// Token: 0x04000B25 RID: 2853
	private const string DASH_TRAIL_PROJECTILE_NAME = "MimicBossDashTrailProjectile";

	// Token: 0x04000B26 RID: 2854
	private const string GALLOP_AUDIO_EVENT_PATH = "event:/SFX/Enemies/sfx_mimicBoss_charge_gallop";

	// Token: 0x04000B27 RID: 2855
	protected const string INTRO_OPEN = "BossIntro_Open";

	// Token: 0x04000B28 RID: 2856
	protected const string INTRO_CLOSE = "BossIntro_ShakeAndClose";

	// Token: 0x04000B29 RID: 2857
	protected const string INTRO_GROWL = "BossIntro_GrowlLoop";

	// Token: 0x04000B2A RID: 2858
	private bool m_shouldPlayGallop = true;
}
