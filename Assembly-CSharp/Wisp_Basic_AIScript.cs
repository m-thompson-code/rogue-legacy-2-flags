using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class Wisp_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002253F File Offset: 0x0002073F
	public override bool ForceDeathAnimation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00022542 File Offset: 0x00020742
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FireballEnemyProjectile"
		};
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00022558 File Offset: 0x00020758
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
		this.m_idleLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_wisp_hit_idle_loop", base.transform);
		this.m_dashLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_wisp_hit_dash_loop", base.transform);
		AudioManager.PlayAttached(this, this.m_idleLoopEventInstance, base.gameObject);
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x000225BF File Offset: 0x000207BF
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			AudioManager.PlayAttached(this, this.m_idleLoopEventInstance, base.gameObject);
		}
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x000225DB File Offset: 0x000207DB
	protected override void OnDisable()
	{
		AudioManager.Stop(this.m_idleLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_dashLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x000225F5 File Offset: 0x000207F5
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00022606 File Offset: 0x00020806
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.2f, 0.4f);
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00022617 File Offset: 0x00020817
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.2f, 0.4f);
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00022628 File Offset: 0x00020828
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00022639 File Offset: 0x00020839
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0002264A File Offset: 0x0002084A
	protected override float IdleAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00022651 File Offset: 0x00020851
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00022658 File Offset: 0x00020858
	protected override float ForcedIdleDuration_IfReversingDirection
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00022660 File Offset: 0x00020860
	private void Update()
	{
		bool flag = base.EnemyController.FollowTarget && base.EnemyController.FlyingMovementType == FlyingMovementType.Towards;
		this.m_idleLoopEventInstance.setParameterByName("agroState", flag ? 1f : 0f, false);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x000226AD File Offset: 0x000208AD
	private void OnDestroy()
	{
		if (this.m_idleLoopEventInstance.isValid())
		{
			this.m_idleLoopEventInstance.release();
		}
		if (this.m_dashLoopEventInstance.isValid())
		{
			this.m_dashLoopEventInstance.release();
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000226E1 File Offset: 0x000208E1
	protected virtual float m_fireballSpeedMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06000B25 RID: 2853 RVA: 0x000226E8 File Offset: 0x000208E8
	protected virtual bool m_dropsFireballsWhileWalking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06000B26 RID: 2854 RVA: 0x000226EB File Offset: 0x000208EB
	protected virtual float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x000226F2 File Offset: 0x000208F2
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06000B28 RID: 2856 RVA: 0x000226F9 File Offset: 0x000208F9
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00022700 File Offset: 0x00020900
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00022707 File Offset: 0x00020907
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0002270E File Offset: 0x0002090E
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00022715 File Offset: 0x00020915
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0002271C File Offset: 0x0002091C
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00022723 File Offset: 0x00020923
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0002272A File Offset: 0x0002092A
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00022731 File Offset: 0x00020931
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00022738 File Offset: 0x00020938
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0002273F File Offset: 0x0002093F
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00022746 File Offset: 0x00020946
	protected virtual float m_dash_Attack_ForwardSpeedMod
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0002274D File Offset: 0x0002094D
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.275f;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00022754 File Offset: 0x00020954
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00022757 File Offset: 0x00020957
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0002275E File Offset: 0x0002095E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("Dash_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, this.m_dash_TellIntro_Delay, false);
		yield return this.Default_Animation("Dash_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellHold_Delay, false);
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		AudioManager.PlayAttached(this, this.m_dashLoopEventInstance, base.gameObject);
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = base.EnemyController.BaseSpeed * this.m_dash_Attack_ForwardSpeedMod;
		if (this.m_dropsFireballsDuringDashAttack)
		{
			yield return this.DropFireballDuringDash();
		}
		else if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimSpeed, this.m_dash_Exit_Delay, true);
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		AudioManager.Stop(this.m_dashLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0002276D File Offset: 0x0002096D
	private IEnumerator DropFireballDuringDash()
	{
		int numFireballs = (int)(this.m_dash_Attack_Duration / this.m_fireballDropDuringDashInterval);
		float remainingTime = this.m_dash_Attack_Duration - this.m_fireballDropDuringDashInterval * (float)numFireballs;
		int num;
		for (int i = 0; i < numFireballs; i = num + 1)
		{
			this.DropFireball();
			if (this.m_fireballDropDuringDashInterval > 0f)
			{
				yield return base.Wait(this.m_fireballDropDuringDashInterval, false);
			}
			num = i;
		}
		if (remainingTime > 0f)
		{
			yield return base.Wait(remainingTime, false);
		}
		yield break;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0002277C File Offset: 0x0002097C
	private void DropFireball()
	{
		float angle = 270f;
		if (base.EnemyController.EnemyRank == EnemyRank.Expert || base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			angle = CDGHelper.VectorToAngle(PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position);
		}
		this.FireProjectile("FireballEnemyProjectile", 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x000227F2 File Offset: 0x000209F2
	public override IEnumerator DeathAnim()
	{
		yield return base.DeathAnim();
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			Vector2 spawnOffset = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
			this.SummonEnemy_NoYield(EnemyType.Wisp, EnemyRank.Expert, spawnOffset, false, false);
			spawnOffset = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
			this.SummonEnemy_NoYield(EnemyType.Wisp, EnemyRank.Expert, spawnOffset, false, false);
		}
		yield break;
	}

	// Token: 0x04000FC6 RID: 4038
	private const string IDLE_LOOP_AUDIO_EVENT = "event:/SFX/Enemies/sfx_enemy_wisp_hit_idle_loop";

	// Token: 0x04000FC7 RID: 4039
	private const string DASH_LOOP_AUDIO_EVENT = "event:/SFX/Enemies/sfx_enemy_wisp_hit_dash_loop";

	// Token: 0x04000FC8 RID: 4040
	private const string AGRO_STATE_PARAM = "agroState";

	// Token: 0x04000FC9 RID: 4041
	private EventInstance m_idleLoopEventInstance;

	// Token: 0x04000FCA RID: 4042
	private EventInstance m_dashLoopEventInstance;

	// Token: 0x04000FCB RID: 4043
	protected const string FIREBALL_PROJECTILE = "FireballEnemyProjectile";

	// Token: 0x04000FCC RID: 4044
	protected const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x04000FCD RID: 4045
	protected const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x04000FCE RID: 4046
	protected const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x04000FCF RID: 4047
	protected const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x04000FD0 RID: 4048
	protected const string DASH_EXIT = "Dash_Exit";
}
