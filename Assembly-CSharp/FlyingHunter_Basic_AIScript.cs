using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class FlyingHunter_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float TeleportHealthLossTrigger
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0000658D File Offset: 0x0000478D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingHunterBoltProjectile"
		};
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float WakeUpChaseDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool TeleportOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x000065A3 File Offset: 0x000047A3
	protected virtual Vector2 TeleportDistanceFromPlayer
	{
		get
		{
			return new Vector2(10f, 10f);
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x000065B4 File Offset: 0x000047B4
	protected virtual int TeleportAngleFromPlayer
	{
		get
		{
			return 60;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x00006581 File Offset: 0x00004781
	protected virtual int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x0600098E RID: 2446 RVA: 0x00005FB8 File Offset: 0x000041B8
	protected virtual float OnHitDelayDuration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x000667DC File Offset: 0x000649DC
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		if (this.TeleportOnHit)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit), false);
		}
		this.m_teleportWaitYield = new WaitRL_Yield(0f, false);
		this.m_trailRenderer = base.EnemyController.GetComponentInChildren<TrailRenderer>();
		base.EnemyController.ResetToNeutralWhenUnculling = false;
		base.EnemyController.DisableDistanceThresholdCheck = true;
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000065B8 File Offset: 0x000047B8
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x000065D8 File Offset: 0x000047D8
	private void OnDestroy()
	{
		if (base.EnemyController && this.TeleportOnHit)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit));
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00066858 File Offset: 0x00064A58
	protected void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (args.DamageTaken > 0f && base.EnemyController.CurrentHealth <= this.m_requiredHPDropToTeleport)
		{
			this.m_requiredHPDropToTeleport = base.EnemyController.CurrentHealth - (float)base.EnemyController.ActualMaxHealth * this.TeleportHealthLossTrigger;
			base.LogicController.StopAllLogic(false);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Teleport";
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00006611 File Offset: 0x00004811
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[WanderLogic]
	[RestLogic]
	public IEnumerator Teleport()
	{
		float num = (float)UnityEngine.Random.Range(0, 359);
		float num2 = (float)UnityEngine.Random.Range(0, 100);
		num = (float)UnityEngine.Random.Range(0 - this.TeleportAngleFromPlayer, this.TeleportAngleFromPlayer);
		if (base.EnemyController.IsTargetToMyRight)
		{
			num += -180f;
		}
		if (num2 < (float)this.TeleportOddsToFlipSideOnHit)
		{
			num += -180f;
		}
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_vanish", base.EnemyController.transform.position);
		float num3 = UnityEngine.Random.Range(this.TeleportDistanceFromPlayer.x, this.TeleportDistanceFromPlayer.y);
		Vector3 vector = CDGHelper.AngleToVector(num) * num3;
		vector += PlayerManager.GetPlayerController().transform.position;
		vector.z = base.EnemyController.transform.position.z;
		base.EnemyController.transform.position = vector;
		base.EnemyController.UpdateBounds();
		base.EnemyController.ConstrainEnemyMovementToRoom();
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (CDGHelper.DistanceBetweenPts(base.EnemyController.transform.position, playerController.Midpoint) < num3 / 2f)
		{
			vector = base.EnemyController.transform.position;
			if (vector.y > base.EnemyController.Room.Bounds.center.y)
			{
				vector.y -= num3;
			}
			else
			{
				vector.y += num3;
			}
			base.EnemyController.transform.position = vector;
			base.EnemyController.UpdateBounds();
		}
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
		yield return null;
		base.SetVelocity(0f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_appear", base.gameObject);
		if (this.TeleportSpawnProjectilesOnHit)
		{
			AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_flyingHunter_shoot", base.gameObject);
		}
		base.EnemyController.Animator.SetBool("Teleporting", true);
		yield return this.Default_Animation("Teleporting", this.m_teleportStunned_AnimSpeed, this.m_teleportStunned_Delay, true);
		base.EnemyController.Animator.SetBool("Teleporting", false);
		if (this.TeleportSpawnProjectilesOnHit)
		{
			yield return this.Default_TellIntroAndLoop("BurstAttack_Tell_Intro", this.m_burst_Tell_AnimationSpeed, "BurstAttack_Tell_Hold", this.m_burst_TellHold_AnimationSpeed, this.m_burst_TellAndHold_Delay);
			yield return this.Default_Animation("BurstAttack_Attack_Intro", this.m_burst_AttackIntro_AnimationSpeed, this.m_burst_AttackIntro_Delay, true);
			int num4 = UnityEngine.Random.Range(0, 2);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Basic)
			{
				if (num4 == 0)
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 0f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 180f, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 90f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 270f, 1f, true, true, true);
				}
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced)
			{
				if (num4 == 0)
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 45f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 135f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -45f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -135f, 1f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 0f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 90f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 180f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 270f, 1f, true, true, true);
				}
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				if (num4 == 0)
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 30f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 60f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 120f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 150f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -30f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -60f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -120f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -150f, 1f, true, true, true);
				}
				else if (num4 == 1)
				{
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, -15f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 15f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 75f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 105f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 165f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 195f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 255f, 1f, true, true, true);
					this.FireProjectile("FlyingHunterBoltProjectile", 0, false, 285f, 1f, true, true, true);
				}
			}
			yield return this.Default_Animation("BurstAttack_Attack_Hold", this.m_burst_AttackHold_AnimationSpeed, this.m_burst_AttackHold_Delay, true);
			yield return this.Default_Animation("BurstAttack_Exit", this.m_burst_Exit_AnimationSpeed, this.m_burst_Exit_Delay, true);
		}
		yield return this.Idle(this.OnHitDelayDuration);
		yield return this.ChangeAnimationState("Neutral");
		yield break;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00006620 File Offset: 0x00004820
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.Animator.SetBool("Teleporting", false);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0000663E File Offset: 0x0000483E
	public override IEnumerator WalkTowards()
	{
		while (!this.m_chaseTarget)
		{
			yield return null;
		}
		if (this.m_isSleeping)
		{
			this.m_isSleeping = false;
			base.Animator.SetBool("Awake", true);
			yield return this.ChangeAnimationState("WakeUp");
			yield return base.Wait(this.WakeUpChaseDelay, false);
		}
		yield return base.WalkTowards();
		yield break;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0000664D File Offset: 0x0000484D
	public override IEnumerator Idle()
	{
		if (this.m_chaseTarget)
		{
			yield return this.WalkTowards();
		}
		else
		{
			yield return base.Idle();
		}
		yield break;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0000665C File Offset: 0x0000485C
	public override void ResetScript()
	{
		this.GoToSleep();
		this.m_chaseTarget = false;
		this.m_requiredHPDropToTeleport = 0f;
		base.ResetScript();
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000668C8 File Offset: 0x00064AC8
	private void GoToSleep()
	{
		this.m_isSleeping = true;
		this.m_chaseTarget = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetVelocity(0f, 0f, false);
		base.Animator.SetBool("Awake", false);
		base.Animator.Play("Sleeping");
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0000667C File Offset: 0x0000487C
	public override void OnEnemyActivated()
	{
		this.m_requiredHPDropToTeleport = base.EnemyController.CurrentHealth - (float)base.EnemyController.ActualMaxHealth * this.TeleportHealthLossTrigger;
		base.OnEnemyActivated();
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x000066A9 File Offset: 0x000048A9
	private void FixedUpdate()
	{
		if (base.IsInitialized && (base.LogicController.CurrentRangeState == LogicState.Close || base.LogicController.IsAggroed) && !this.m_chaseTarget)
		{
			this.m_chaseTarget = true;
		}
	}

	// Token: 0x04000D27 RID: 3367
	private bool m_isSleeping = true;

	// Token: 0x04000D28 RID: 3368
	private TrailRenderer m_trailRenderer;

	// Token: 0x04000D29 RID: 3369
	private const string TELEPORT_BEGIN_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_vanish";

	// Token: 0x04000D2A RID: 3370
	private const string TELEPORT_END_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_appear";

	// Token: 0x04000D2B RID: 3371
	private const string SHOOT_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_shoot";

	// Token: 0x04000D2C RID: 3372
	private bool m_chaseTarget;

	// Token: 0x04000D2D RID: 3373
	private WaitRL_Yield m_teleportWaitYield;

	// Token: 0x04000D2E RID: 3374
	private float m_requiredHPDropToTeleport;

	// Token: 0x04000D2F RID: 3375
	private const string TELEPORT_STUNNED_ANIM = "Teleporting";

	// Token: 0x04000D30 RID: 3376
	protected float m_teleportStunned_AnimSpeed = 1f;

	// Token: 0x04000D31 RID: 3377
	protected float m_teleportStunned_Delay;

	// Token: 0x04000D32 RID: 3378
	private const string BURST_TELL_INTRO = "BurstAttack_Tell_Intro";

	// Token: 0x04000D33 RID: 3379
	private const string BURST_TELL_HOLD = "BurstAttack_Tell_Hold";

	// Token: 0x04000D34 RID: 3380
	private const string BURST_ATTACK_INTRO = "BurstAttack_Attack_Intro";

	// Token: 0x04000D35 RID: 3381
	private const string BURST_ATTACK_HOLD = "BurstAttack_Attack_Hold";

	// Token: 0x04000D36 RID: 3382
	private const string BURST_EXIT = "BurstAttack_Exit";

	// Token: 0x04000D37 RID: 3383
	private const string BURST_PROJECTILE = "FlyingHunterBoltProjectile";

	// Token: 0x04000D38 RID: 3384
	protected float m_burst_Tell_AnimationSpeed = 1f;

	// Token: 0x04000D39 RID: 3385
	protected float m_burst_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000D3A RID: 3386
	protected float m_burst_TellAndHold_Delay = 0.2f;

	// Token: 0x04000D3B RID: 3387
	protected float m_burst_AttackIntro_Delay;

	// Token: 0x04000D3C RID: 3388
	protected float m_burst_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x04000D3D RID: 3389
	protected float m_burst_AttackHold_Delay;

	// Token: 0x04000D3E RID: 3390
	protected float m_burst_AttackHold_AnimationSpeed = 2f;

	// Token: 0x04000D3F RID: 3391
	protected float m_burst_Exit_AnimationSpeed = 1f;

	// Token: 0x04000D40 RID: 3392
	protected float m_burst_Exit_Delay;
}
