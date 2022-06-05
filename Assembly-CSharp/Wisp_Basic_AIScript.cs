using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class Wisp_Basic_AIScript : BaseAIScript
{
	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06001144 RID: 4420 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool ForceDeathAnimation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0000907D File Offset: 0x0000727D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FireballEnemyProjectile"
		};
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x0007F438 File Offset: 0x0007D638
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
		this.m_idleLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_wisp_hit_idle_loop", base.transform);
		this.m_dashLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_wisp_hit_dash_loop", base.transform);
		AudioManager.PlayAttached(this, this.m_idleLoopEventInstance, base.gameObject);
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x00009093 File Offset: 0x00007293
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			AudioManager.PlayAttached(this, this.m_idleLoopEventInstance, base.gameObject);
		}
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000090AF File Offset: 0x000072AF
	protected override void OnDisable()
	{
		AudioManager.Stop(this.m_idleLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioManager.Stop(this.m_dashLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06001149 RID: 4425 RVA: 0x00003F6C File Offset: 0x0000216C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x0600114A RID: 4426 RVA: 0x000090C9 File Offset: 0x000072C9
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.2f, 0.4f);
		}
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x0600114B RID: 4427 RVA: 0x000090C9 File Offset: 0x000072C9
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.2f, 0.4f);
		}
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x0600114C RID: 4428 RVA: 0x00006C26 File Offset: 0x00004E26
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x0600114D RID: 4429 RVA: 0x00006C26 File Offset: 0x00004E26
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-4f, 4f);
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x0600114E RID: 4430 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float IdleAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x0600114F RID: 4431 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06001150 RID: 4432 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ForcedIdleDuration_IfReversingDirection
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x0007F4A0 File Offset: 0x0007D6A0
	private void Update()
	{
		bool flag = base.EnemyController.FollowTarget && base.EnemyController.FlyingMovementType == FlyingMovementType.Towards;
		this.m_idleLoopEventInstance.setParameterByName("agroState", flag ? 1f : 0f, false);
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x000090DA File Offset: 0x000072DA
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

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06001153 RID: 4435 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_fireballSpeedMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06001154 RID: 4436 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dropsFireballsWhileWalking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x06001155 RID: 4437 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x06001156 RID: 4438 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06001157 RID: 4439 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06001158 RID: 4440 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06001159 RID: 4441 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x0600115A RID: 4442 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x0600115B RID: 4443 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x0600115C RID: 4444 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x0600115D RID: 4445 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x0600115E RID: 4446 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06001160 RID: 4448 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06001161 RID: 4449 RVA: 0x000081A4 File Offset: 0x000063A4
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06001162 RID: 4450 RVA: 0x0000676B File Offset: 0x0000496B
	protected virtual float m_dash_Attack_ForwardSpeedMod
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06001163 RID: 4451 RVA: 0x000086AA File Offset: 0x000068AA
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.275f;
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06001164 RID: 4452 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06001165 RID: 4453 RVA: 0x00006772 File Offset: 0x00004972
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x0000910E File Offset: 0x0000730E
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

	// Token: 0x06001167 RID: 4455 RVA: 0x0000911D File Offset: 0x0000731D
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

	// Token: 0x06001168 RID: 4456 RVA: 0x0007F4F0 File Offset: 0x0007D6F0
	private void DropFireball()
	{
		float angle = 270f;
		if (base.EnemyController.EnemyRank == EnemyRank.Expert || base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			angle = CDGHelper.VectorToAngle(PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position);
		}
		this.FireProjectile("FireballEnemyProjectile", 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x0000912C File Offset: 0x0000732C
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

	// Token: 0x0400145F RID: 5215
	private const string IDLE_LOOP_AUDIO_EVENT = "event:/SFX/Enemies/sfx_enemy_wisp_hit_idle_loop";

	// Token: 0x04001460 RID: 5216
	private const string DASH_LOOP_AUDIO_EVENT = "event:/SFX/Enemies/sfx_enemy_wisp_hit_dash_loop";

	// Token: 0x04001461 RID: 5217
	private const string AGRO_STATE_PARAM = "agroState";

	// Token: 0x04001462 RID: 5218
	private EventInstance m_idleLoopEventInstance;

	// Token: 0x04001463 RID: 5219
	private EventInstance m_dashLoopEventInstance;

	// Token: 0x04001464 RID: 5220
	protected const string FIREBALL_PROJECTILE = "FireballEnemyProjectile";

	// Token: 0x04001465 RID: 5221
	protected const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x04001466 RID: 5222
	protected const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x04001467 RID: 5223
	protected const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x04001468 RID: 5224
	protected const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x04001469 RID: 5225
	protected const string DASH_EXIT = "Dash_Exit";
}
