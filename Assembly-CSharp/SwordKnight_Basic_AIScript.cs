using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class SwordKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000F3F RID: 3903 RVA: 0x000085B2 File Offset: 0x000067B2
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

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06000F40 RID: 3904 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_slash_TellIntroAndHold_Delay
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06000F41 RID: 3905 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_slash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06000F42 RID: 3906 RVA: 0x000085E0 File Offset: 0x000067E0
	protected virtual float m_slash_Attack_Speed
	{
		get
		{
			return 13f;
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected virtual float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_cricket_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00007483 File Offset: 0x00005683
	protected virtual float m_cricket_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06000F46 RID: 3910 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06000F47 RID: 3911 RVA: 0x000085E7 File Offset: 0x000067E7
	protected virtual float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 1.65f;
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000085EE File Offset: 0x000067EE
	private void Awake()
	{
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00008602 File Offset: 0x00006802
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00008610 File Offset: 0x00006810
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00077BE8 File Offset: 0x00075DE8
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

	// Token: 0x06000F4C RID: 3916 RVA: 0x00008624 File Offset: 0x00006824
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

	// Token: 0x06000F4D RID: 3917 RVA: 0x00008633 File Offset: 0x00006833
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

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_chestBumpTellHoldDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_chestBumpAttackHoldDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00008642 File Offset: 0x00006842
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

	// Token: 0x06000F51 RID: 3921 RVA: 0x00008651 File Offset: 0x00006851
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.LockFlip = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0400127A RID: 4730
	protected float m_slash_TellIntro_AnimationSpeed = 0.5f;

	// Token: 0x0400127B RID: 4731
	protected float m_slash_TellHold_AnimationSpeed = 0.5f;

	// Token: 0x0400127C RID: 4732
	protected float m_slash_AttackIntro_AnimationSpeed = 2f;

	// Token: 0x0400127D RID: 4733
	protected float m_slash_AttackIntro_Delay;

	// Token: 0x0400127E RID: 4734
	protected float m_slash_AttackHold_AnimationSpeed = 1f;

	// Token: 0x0400127F RID: 4735
	protected float m_slash_AttackHold_Delay = 0.5f;

	// Token: 0x04001280 RID: 4736
	protected float m_slash_Exit_AnimationSpeed = 0.7f;

	// Token: 0x04001281 RID: 4737
	protected float m_slash_Exit_Delay = 0.15f;

	// Token: 0x04001282 RID: 4738
	protected float m_slash_AttackCD = 1f;

	// Token: 0x04001283 RID: 4739
	protected float m_cricket_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04001284 RID: 4740
	protected float m_cricket_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x04001285 RID: 4741
	protected float m_cricket_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04001286 RID: 4742
	protected float m_cricket_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04001287 RID: 4743
	protected float m_cricket_AttackIntro_Delay;

	// Token: 0x04001288 RID: 4744
	protected float m_cricket_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04001289 RID: 4745
	protected float m_cricket_AttackHold_Delay = 0.25f;

	// Token: 0x0400128A RID: 4746
	protected float m_cricket_Exit_AnimationSpeed = 0.45f;

	// Token: 0x0400128B RID: 4747
	protected float m_cricket_Exit_Delay = 0.15f;

	// Token: 0x0400128C RID: 4748
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x0400128D RID: 4749
	private const string CHEST_BUMP_TELL_INTRO = "ChestBump_Tell_Intro";

	// Token: 0x0400128E RID: 4750
	private const string CHEST_BUMP_TELL_HOLD = "ChestBump_Tell_Hold";

	// Token: 0x0400128F RID: 4751
	private const string CHEST_BUMP_ATTACK_INTRO = "ChestBump_Attack_Intro";

	// Token: 0x04001290 RID: 4752
	private const string CHEST_BUMP_ATTACK_HOLD = "ChestBump_Attack_Hold";

	// Token: 0x04001291 RID: 4753
	private const string CHEST_BUMP_EXIT = "ChestBump_Exit";
}
