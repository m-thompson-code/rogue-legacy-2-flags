using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class KiStrike_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x06001539 RID: 5433 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x0600153A RID: 5434 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x0600153B RID: 5435 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x0600153C RID: 5436 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x0600153D RID: 5437 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x0600153E RID: 5438 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x0600153F RID: 5439 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06001540 RID: 5440 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06001541 RID: 5441 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06001542 RID: 5442 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x0000A99D File Offset: 0x00008B9D
	public override IEnumerator CastAbility()
	{
		if (this.m_disableGravity && this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00089AE0 File Offset: 0x00087CE0
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		if (this.m_disableGravity)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
			if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
			{
				this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
			}
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001671 RID: 5745
	[SerializeField]
	private bool m_disableGravity;
}
