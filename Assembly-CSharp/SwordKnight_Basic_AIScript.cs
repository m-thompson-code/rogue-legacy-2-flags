using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class SwordKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x060009E2 RID: 2530 RVA: 0x0001FC7A File Offset: 0x0001DE7A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SwordKnightSwordSlashProjectile",
			"SwordKnightBounceMinibossProjectile",
			"SwordKnightVoidProjectile",
			"SwordKnightVoidMinibossProjectile"
		};
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
	protected virtual float m_slash_TellIntroAndHold_Delay
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0001FCAF File Offset: 0x0001DEAF
	protected virtual float m_slash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0001FCB6 File Offset: 0x0001DEB6
	protected virtual float m_slash_Attack_Speed
	{
		get
		{
			return 13f;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0001FCBD File Offset: 0x0001DEBD
	protected virtual float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0001FCC4 File Offset: 0x0001DEC4
	protected virtual float m_cricket_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0001FCCB File Offset: 0x0001DECB
	protected virtual float m_cricket_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0001FCD2 File Offset: 0x0001DED2
	protected virtual int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001FCD5 File Offset: 0x0001DED5
	protected virtual float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 1.65f;
		}
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0001FCDC File Offset: 0x0001DEDC
	private void Awake()
	{
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x0001FCFE File Offset: 0x0001DEFE
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0001FD14 File Offset: 0x0001DF14
	private void OnPlayerHit(object sender, EventArgs args)
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.LogicController.CurrentLogicBlockName == "Idle" || base.LogicController.CurrentLogicBlockName == "WalkTowards" || base.LogicController.CurrentLogicBlockName == "WalkAway")
		{
			CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
			if (characterHitEventArgs != null && characterHitEventArgs.Attacker.Equals(base.EnemyController))
			{
				base.LogicController.StopAllLogic(true);
				base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "ChestBump";
			}
		}
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0001FDA8 File Offset: 0x0001DFA8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SlashAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		float dashSpeed = this.m_slash_Attack_Speed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		yield return this.Default_TellIntroAndLoop("Slash_Tell_Intro", this.m_slash_TellIntro_AnimationSpeed, "Slash_Tell_Hold", this.m_slash_TellHold_AnimationSpeed, this.m_slash_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Slash_Attack_Intro", this.m_slash_AttackIntro_AnimationSpeed, this.m_slash_AttackIntro_Delay, true);
		this.FireProjectile("SwordKnightSwordSlashProjectile", 0, true, 0f, 1f, true, true, true);
		yield return this.Default_Animation("Slash_Attack_Hold", this.m_slash_AttackHold_AnimationSpeed, 0f, false);
		if (this.m_slash_Attack_Duration > 0f)
		{
			base.SetVelocityX(dashSpeed, false);
			if (this.m_slash_Attack_Duration > 0f)
			{
				yield return base.Wait(this.m_slash_Attack_Duration, false);
			}
			base.SetVelocityX(0f, false);
		}
		if (this.m_slash_AttackHold_Delay - this.m_slash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_slash_AttackHold_Delay - this.m_slash_Attack_Duration, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Slash_Exit", this.m_slash_Exit_AnimationSpeed, this.m_slash_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_slash_Exit_ForceIdle, this.m_slash_AttackCD);
		yield break;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0001FDB7 File Offset: 0x0001DFB7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CricketAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("GroundShot_Tell_Intro", this.m_cricket_TellIntro_AnimationSpeed, "GroundShot_Tell_Hold", this.m_cricket_TellHold_AnimationSpeed, this.m_cricket_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("GroundShot_Attack_Intro", this.m_cricket_AttackIntro_AnimationSpeed, this.m_cricket_AttackIntro_Delay, true);
		yield return this.Default_Animation("GroundShot_Attack_Hold", this.m_cricket_AttackHold_AnimationSpeed, 0f, false);
		int num2;
		for (int i = 1; i < this.m_cricket_Attack_ProjectileAmount + 1; i = num2 + 1)
		{
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				int num = UnityEngine.Random.Range(0, 4);
				if (num == 0)
				{
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 6, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 5, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 4, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 3, true, 0f, 1f, true, true, true);
				}
				else if (num == 1)
				{
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 5, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 4, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 3, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 2, true, 0f, 1f, true, true, true);
				}
				else if (num == 2)
				{
					this.FireProjectile("SwordKnightVoidMinibossProjectile", 0, true, 0f, 1f, true, true, true);
				}
				else if (num == 3)
				{
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 6, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 4, true, 0f, 1f, true, true, true);
					this.FireProjectile("SwordKnightBounceMinibossProjectile", 2, true, 0f, 1f, true, true, true);
				}
			}
			else
			{
				this.FireProjectile("SwordKnightVoidProjectile", 0, true, 0f, 1f, true, true, true);
			}
			if (this.m_cricket_Attack_ProjectileAmount > 1 && this.m_cricket_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_cricket_Attack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return base.Wait(this.m_cricket_AttackHold_Delay, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("GroundShot_Exit", this.m_cricket_Exit_AnimationSpeed, this.m_cricket_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_cricket_Exit_ForceIdle, this.m_cricket_AttackCD);
		yield break;
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0001FDC6 File Offset: 0x0001DFC6
	protected virtual float m_chestBumpTellHoldDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0001FDCD File Offset: 0x0001DFCD
	protected virtual float m_chestBumpAttackHoldDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ChestBump()
	{
		base.EnemyController.LockFlip = true;
		base.EnemyController.SetVelocityX(0f, false);
		yield return this.Default_Animation("ChestBump_Tell_Intro", 10f, 0f, true);
		yield return this.Default_Animation("ChestBump_Tell_Hold", 10f, this.m_chestBumpTellHoldDelay, true);
		yield return this.Default_Animation("ChestBump_Attack_Intro", 1f, 0f, true);
		yield return this.Default_Animation("ChestBump_Attack_Hold", 1f, this.m_chestBumpAttackHoldDelay, true);
		yield return this.Default_Animation("ChestBump_Exit", 1f, 0f, true);
		yield return this.Default_Animation(BaseAIScript.NEUTRAL_STATE, 1f, 0f, true);
		base.EnemyController.LockFlip = false;
		yield break;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0001FDE3 File Offset: 0x0001DFE3
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.LockFlip = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000E87 RID: 3719
	protected float m_slash_TellIntro_AnimationSpeed = 0.5f;

	// Token: 0x04000E88 RID: 3720
	protected float m_slash_TellHold_AnimationSpeed = 0.5f;

	// Token: 0x04000E89 RID: 3721
	protected float m_slash_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x04000E8A RID: 3722
	protected float m_slash_AttackIntro_Delay;

	// Token: 0x04000E8B RID: 3723
	protected float m_slash_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E8C RID: 3724
	protected float m_slash_AttackHold_Delay = 0.5f;

	// Token: 0x04000E8D RID: 3725
	protected float m_slash_Exit_AnimationSpeed = 0.7f;

	// Token: 0x04000E8E RID: 3726
	protected float m_slash_Exit_Delay = 0.15f;

	// Token: 0x04000E8F RID: 3727
	protected float m_slash_AttackCD = 1f;

	// Token: 0x04000E90 RID: 3728
	protected float m_cricket_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000E91 RID: 3729
	protected float m_cricket_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x04000E92 RID: 3730
	protected float m_cricket_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000E93 RID: 3731
	protected float m_cricket_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E94 RID: 3732
	protected float m_cricket_AttackIntro_Delay;

	// Token: 0x04000E95 RID: 3733
	protected float m_cricket_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E96 RID: 3734
	protected float m_cricket_AttackHold_Delay = 0.25f;

	// Token: 0x04000E97 RID: 3735
	protected float m_cricket_Exit_AnimationSpeed = 0.45f;

	// Token: 0x04000E98 RID: 3736
	protected float m_cricket_Exit_Delay = 0.15f;

	// Token: 0x04000E99 RID: 3737
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x04000E9A RID: 3738
	private const string CHEST_BUMP_TELL_INTRO = "ChestBump_Tell_Intro";

	// Token: 0x04000E9B RID: 3739
	private const string CHEST_BUMP_TELL_HOLD = "ChestBump_Tell_Hold";

	// Token: 0x04000E9C RID: 3740
	private const string CHEST_BUMP_ATTACK_INTRO = "ChestBump_Attack_Intro";

	// Token: 0x04000E9D RID: 3741
	private const string CHEST_BUMP_ATTACK_HOLD = "ChestBump_Attack_Hold";

	// Token: 0x04000E9E RID: 3742
	private const string CHEST_BUMP_EXIT = "ChestBump_Exit";
}
