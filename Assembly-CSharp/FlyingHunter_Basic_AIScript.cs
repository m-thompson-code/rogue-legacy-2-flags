using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class FlyingHunter_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x000197BE File Offset: 0x000179BE
	protected virtual float TeleportHealthLossTrigger
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000197C5 File Offset: 0x000179C5
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingHunterBoltProjectile"
		};
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x000197DB File Offset: 0x000179DB
	protected virtual float WakeUpChaseDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x000197E2 File Offset: 0x000179E2
	protected virtual bool TeleportOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x000197E5 File Offset: 0x000179E5
	protected virtual Vector2 TeleportDistanceFromPlayer
	{
		get
		{
			return new Vector2(10f, 10f);
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x000197F6 File Offset: 0x000179F6
	protected virtual int TeleportAngleFromPlayer
	{
		get
		{
			return 60;
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x0600068D RID: 1677 RVA: 0x000197FA File Offset: 0x000179FA
	protected virtual int TeleportOddsToFlipSideOnHit
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x0600068E RID: 1678 RVA: 0x000197FE File Offset: 0x000179FE
	protected virtual bool TeleportSpawnProjectilesOnHit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x00019801 File Offset: 0x00017A01
	protected virtual float OnHitDelayDuration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06000690 RID: 1680 RVA: 0x00019808 File Offset: 0x00017A08
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001980F File Offset: 0x00017A0F
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00019818 File Offset: 0x00017A18
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

	// Token: 0x06000693 RID: 1683 RVA: 0x00019891 File Offset: 0x00017A91
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x000198B1 File Offset: 0x00017AB1
	private void OnDestroy()
	{
		if (base.EnemyController && this.TeleportOnHit)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.OnEnemyHit));
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000198EC File Offset: 0x00017AEC
	protected void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (args.DamageTaken > 0f && base.EnemyController.CurrentHealth <= this.m_requiredHPDropToTeleport)
		{
			this.m_requiredHPDropToTeleport = base.EnemyController.CurrentHealth - (float)base.EnemyController.ActualMaxHealth * this.TeleportHealthLossTrigger;
			base.LogicController.StopAllLogic(false);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "Teleport";
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0001995A File Offset: 0x00017B5A
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

	// Token: 0x06000697 RID: 1687 RVA: 0x00019969 File Offset: 0x00017B69
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.Animator.SetBool("Teleporting", false);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00019987 File Offset: 0x00017B87
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

	// Token: 0x06000699 RID: 1689 RVA: 0x00019996 File Offset: 0x00017B96
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

	// Token: 0x0600069A RID: 1690 RVA: 0x000199A5 File Offset: 0x00017BA5
	public override void ResetScript()
	{
		this.GoToSleep();
		this.m_chaseTarget = false;
		this.m_requiredHPDropToTeleport = 0f;
		base.ResetScript();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000199C8 File Offset: 0x00017BC8
	private void GoToSleep()
	{
		this.m_isSleeping = true;
		this.m_chaseTarget = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetVelocity(0f, 0f, false);
		base.Animator.SetBool("Awake", false);
		base.Animator.Play("Sleeping");
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00019A21 File Offset: 0x00017C21
	public override void OnEnemyActivated()
	{
		this.m_requiredHPDropToTeleport = base.EnemyController.CurrentHealth - (float)base.EnemyController.ActualMaxHealth * this.TeleportHealthLossTrigger;
		base.OnEnemyActivated();
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00019A4E File Offset: 0x00017C4E
	private void FixedUpdate()
	{
		if (base.IsInitialized && (base.LogicController.CurrentRangeState == LogicState.Close || base.LogicController.IsAggroed) && !this.m_chaseTarget)
		{
			this.m_chaseTarget = true;
		}
	}

	// Token: 0x04000ABB RID: 2747
	private bool m_isSleeping = true;

	// Token: 0x04000ABC RID: 2748
	private TrailRenderer m_trailRenderer;

	// Token: 0x04000ABD RID: 2749
	private const string TELEPORT_BEGIN_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_vanish";

	// Token: 0x04000ABE RID: 2750
	private const string TELEPORT_END_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_teleport_appear";

	// Token: 0x04000ABF RID: 2751
	private const string SHOOT_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_flyingHunter_shoot";

	// Token: 0x04000AC0 RID: 2752
	private bool m_chaseTarget;

	// Token: 0x04000AC1 RID: 2753
	private WaitRL_Yield m_teleportWaitYield;

	// Token: 0x04000AC2 RID: 2754
	private float m_requiredHPDropToTeleport;

	// Token: 0x04000AC3 RID: 2755
	private const string TELEPORT_STUNNED_ANIM = "Teleporting";

	// Token: 0x04000AC4 RID: 2756
	protected float m_teleportStunned_AnimSpeed = 1f;

	// Token: 0x04000AC5 RID: 2757
	protected float m_teleportStunned_Delay;

	// Token: 0x04000AC6 RID: 2758
	private const string BURST_TELL_INTRO = "BurstAttack_Tell_Intro";

	// Token: 0x04000AC7 RID: 2759
	private const string BURST_TELL_HOLD = "BurstAttack_Tell_Hold";

	// Token: 0x04000AC8 RID: 2760
	private const string BURST_ATTACK_INTRO = "BurstAttack_Attack_Intro";

	// Token: 0x04000AC9 RID: 2761
	private const string BURST_ATTACK_HOLD = "BurstAttack_Attack_Hold";

	// Token: 0x04000ACA RID: 2762
	private const string BURST_EXIT = "BurstAttack_Exit";

	// Token: 0x04000ACB RID: 2763
	private const string BURST_PROJECTILE = "FlyingHunterBoltProjectile";

	// Token: 0x04000ACC RID: 2764
	protected float m_burst_Tell_AnimationSpeed = 1f;

	// Token: 0x04000ACD RID: 2765
	protected float m_burst_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000ACE RID: 2766
	protected float m_burst_TellAndHold_Delay = 0.2f;

	// Token: 0x04000ACF RID: 2767
	protected float m_burst_AttackIntro_Delay;

	// Token: 0x04000AD0 RID: 2768
	protected float m_burst_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x04000AD1 RID: 2769
	protected float m_burst_AttackHold_Delay;

	// Token: 0x04000AD2 RID: 2770
	protected float m_burst_AttackHold_AnimationSpeed = 2f;

	// Token: 0x04000AD3 RID: 2771
	protected float m_burst_Exit_AnimationSpeed = 1f;

	// Token: 0x04000AD4 RID: 2772
	protected float m_burst_Exit_Delay;
}
