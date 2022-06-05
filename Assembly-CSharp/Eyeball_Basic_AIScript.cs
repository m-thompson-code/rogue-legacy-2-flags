using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class Eyeball_Basic_AIScript : BaseAIScript
{
	// Token: 0x060006DD RID: 1757 RVA: 0x00005640 File Offset: 0x00003840
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBounceBoltProjectile",
			"EyeballBoltProjectile",
			"EyeballBounceBoltProjectile",
			"EyeballBounceBoltProjectile"
		};
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float SingleShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060006DF RID: 1759 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float SingleShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0000566E File Offset: 0x0000386E
	protected virtual float SingleShot_TellHold_Duration
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float SingleShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float SingleShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float SingleShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float SingleShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float SingleShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float SingleShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float SingleShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float SingleShot_Exit_AttackCD
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float MultiShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x060006EA RID: 1770 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float MultiShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float MultiShot_TellHold_Duration
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float MultiShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float MultiShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float MultiShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float MultiShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float MultiShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float MultiShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float MultiShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float MultiShot_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int MultiShot_Attack_Amount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float MultiShot_AttackHold_DelayBetweenShots
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0000547D File Offset: 0x0000367D
	protected virtual float MultiShot_Attack_AngleSpread
	{
		get
		{
			return 75f;
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0005BE80 File Offset: 0x0005A080
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.Heading = Vector2.zero;
		this.UpdateEyeball();
		if (enemyController.IsBoss)
		{
			base.LogicController.DisableLogicActivationByDistance = true;
			this.m_faceForward = true;
		}
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0005BED4 File Offset: 0x0005A0D4
	public override void OnEnemyActivated()
	{
		base.OnEnemyActivated();
		if (base.EnemyController.TargetController && !base.EnemyController.IsBoss)
		{
			base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00005675 File Offset: 0x00003875
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootSingleFireball()
	{
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.SingleShot_TellIntro_AnimSpeed, "SingleShot_Tell_Hold", this.SingleShot_TellHold_AnimSpeed, this.SingleShot_TellHold_Duration);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.SingleShot_AttackIntro_AnimSpeed, this.SingleShot_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		float angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("EyeballBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.SingleShot_AttackHold_AnimSpeed, this.SingleShot_AttackHold_Delay, true);
		yield return this.Default_Animation("SingleShot_Exit", this.SingleShot_Exit_AnimSpeed, this.SingleShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.SingleShot_Exit_ForceIdle, this.SingleShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00005684 File Offset: 0x00003884
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootMultiFireball()
	{
		yield return this.Default_TellIntroAndLoop("ContinuousShot_Tell_Intro", this.MultiShot_TellIntro_AnimSpeed, "ContinuousShot_Tell_Hold", this.MultiShot_TellHold_AnimSpeed, this.MultiShot_TellHold_Duration);
		yield return this.Default_Animation("ContinuousShot_Attack_Intro", this.MultiShot_AttackIntro_AnimSpeed, this.MultiShot_AttackIntro_Delay, true);
		base.EnemyController.FollowTarget = false;
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		yield return this.ChangeAnimationState("ContinuousShot_Attack_Hold");
		this.SetAnimationSpeedMultiplier(this.MultiShot_AttackHold_AnimSpeed);
		int num;
		for (int i = 0; i < this.MultiShot_Attack_Amount; i = num + 1)
		{
			this.FireProjectile("EyeballBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("EyeballBoltProjectile", 1, false, fireAngle + this.MultiShot_Attack_AngleSpread, 1f, true, true, true);
				this.FireProjectile("EyeballBoltProjectile", 1, false, fireAngle - this.MultiShot_Attack_AngleSpread, 1f, true, true, true);
			}
			if (this.MultiShot_AttackHold_DelayBetweenShots > 0f)
			{
				yield return base.Wait(this.MultiShot_AttackHold_DelayBetweenShots, false);
			}
			num = i;
		}
		if (this.MultiShot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.MultiShot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("ContinuousShot_Exit", this.MultiShot_Exit_AnimSpeed, this.MultiShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.MultiShot_Exit_ForceIdle, this.MultiShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ExplodingShot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float ExplodingShot_TellHold_AnimSpeed
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x00005693 File Offset: 0x00003893
	protected virtual float ExplodingShot_TellHold_Duration
	{
		get
		{
			return 1.35f;
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ExplodingShot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ExplodingShot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ExplodingShot_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ExplodingShot_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float ExplodingShot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ExplodingShot_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float ExplodingShot_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x0000569A File Offset: 0x0000389A
	protected virtual float ExplodingShot_Exit_AttackCD
	{
		get
		{
			return 7f;
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x000056A1 File Offset: 0x000038A1
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ExplodingShot()
	{
		yield return this.Default_TellIntroAndLoop("Explosion_Tell_Intro", this.ExplodingShot_TellIntro_AnimSpeed, "Explosion_Tell_Hold", this.ExplodingShot_TellHold_AnimSpeed, this.ExplodingShot_TellHold_Duration);
		yield return this.Default_Animation("Explosion_Attack_Intro", this.ExplodingShot_AttackIntro_AnimSpeed, this.ExplodingShot_AttackIntro_Delay, true);
		float angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
			yield return base.Wait(0.45f, false);
			angle = CDGHelper.VectorToAngle(base.EnemyController.Heading);
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("EyeballBounceBoltProjectile", 1, false, angle, 1f, true, true, true);
		}
		yield return this.Default_Animation("Explosion_Attack_Hold", this.ExplodingShot_Exit_AnimSpeed, this.ExplodingShot_AttackHold_Delay, true);
		yield return this.Default_Animation("Explosion_Exit", this.ExplodingShot_AttackHold_AnimSpeed, this.ExplodingShot_Exit_Duration, true);
		base.EnemyController.FollowTarget = true;
		yield return this.Default_Attack_Cooldown(this.ExplodingShot_Exit_ForceIdle, this.ExplodingShot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x000056B0 File Offset: 0x000038B0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator ShootSpinningFireball()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x000056B8 File Offset: 0x000038B8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator ShootHomingFireball()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x000056C0 File Offset: 0x000038C0
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.EnemyController.Target)
		{
			this.UpdateEyeball();
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0005BF38 File Offset: 0x0005A138
	private void UpdateEyeball()
	{
		if (!this.m_faceForward)
		{
			base.EnemyController.Animator.SetFloat("LookDirectionX", base.EnemyController.HeadingX);
			base.EnemyController.Animator.SetFloat("LookDirectionY", base.EnemyController.HeadingY);
			return;
		}
		base.EnemyController.Animator.SetFloat("LookDirectionX", 0f);
		base.EnemyController.Animator.SetFloat("LookDirectionY", 0f);
	}

	// Token: 0x04000A4C RID: 2636
	protected const string EXPLODING_SHOT_TELL_INTRO = "Explosion_Tell_Intro";

	// Token: 0x04000A4D RID: 2637
	protected const string EXPLODING_SHOT_TELL_HOLD = "Explosion_Tell_Hold";

	// Token: 0x04000A4E RID: 2638
	protected const string EXPLODING_SHOT_ATTACK_INTRO = "Explosion_Attack_Intro";

	// Token: 0x04000A4F RID: 2639
	protected const string EXPLODING_SHOT_ATTACK_HOLD = "Explosion_Attack_Hold";

	// Token: 0x04000A50 RID: 2640
	protected const string EXPLODING_SHOT_EXIT = "Explosion_Exit";

	// Token: 0x04000A51 RID: 2641
	protected const string EXPLODING_SHOT_PROJECTILE = "EyeballBounceBoltProjectile";

	// Token: 0x04000A52 RID: 2642
	protected const string EXPLODING_SHOT_EXPERT_PROJECTILE = "EyeballBounceBoltProjectile";

	// Token: 0x04000A53 RID: 2643
	protected bool m_faceForward;
}
