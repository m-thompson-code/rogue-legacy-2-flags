using System;
using System.Collections;

// Token: 0x02000252 RID: 594
public class TraitorBoss_Miniboss_AIScript : TraitorBoss_Basic_AIScript
{
	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x0600110C RID: 4364 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool is_Gregory
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00008F28 File Offset: 0x00007128
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CustomAxeCombo()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		base.Single_Enable_KnockbackDefense();
		yield return base.SetWeaponGeo(this.m_weaponGeoController.Axe, this.DEFAULT_WEAPON_SWAP_DELAY);
		yield return this.Default_TellIntroAndLoop("AxeGrounded_Tell_Intro", this.m_axe_TellIntro_AnimSpeed, "AxeGrounded_Tell_Hold", this.m_axe_TellHold_AnimSpeed, this.m_axe_TellIntroAndHold_Delay);
		yield return this.Default_Animation("AxeAirborne_Attack_Hold", this.m_axe_AttackHold_AnimSpeed, this.m_axe_AttackHold_Delay, false);
		this.m_axeSpinProjectile = this.FireProjectile("TraitorAxeSpinProjectile", 1, false, 0f, 1f, true, true, true);
		yield return base.Single_Jump(true, false, false);
		yield return base.Single_WaitUntilGrounded();
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return base.Single_Jump(true, false, false);
		yield return base.Single_WaitUntilGrounded();
		base.EnemyController.LockFlip = false;
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		if (this.is_Advanced)
		{
			base.EnemyController.LockFlip = false;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
			yield return base.Single_Jump(false, true, false);
			yield return base.Single_WaitUntilGrounded();
		}
		base.StopProjectile(ref this.m_axeSpinProjectile);
		base.Single_Disable_KnockbackDefense();
		yield return this.Default_Animation("AxeAirborne_Exit", this.m_axe_Exit_AnimSpeed, this.m_axe_Exit_Delay, true);
		base.Animator.SetTrigger("Change_Ability_Anim");
		base.EnemyController.LockFlip = false;
		base.HideAllWeaponGeos();
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_axe_Exit_IdleDuration, this.m_axe_AttackCD);
		yield break;
	}
}
