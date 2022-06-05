using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class Shout_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002B3F1 File Offset: 0x000295F1
	public virtual Vector2 PushbackAmount
	{
		get
		{
			return this.m_pushbackAmount;
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0002B3FC File Offset: 0x000295FC
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (!this.m_abilityController.PlayerController.IsGrounded && this.PushbackAmount != Vector2.zero)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			Vector2 pushbackAmount = this.PushbackAmount;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				pushbackAmount.x = -pushbackAmount.x;
			}
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			Vector2 pushbackAmount2 = this.PushbackAmount;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				pushbackAmount2.x = -pushbackAmount2.x;
			}
			this.m_abilityController.PlayerController.SetVelocity(pushbackAmount2.x, pushbackAmount2.y, false);
		}
		if (this.OnShoutEvent != null)
		{
			this.OnShoutEvent.Invoke(this.m_abilityController.PlayerController.gameObject);
		}
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0002B4F0 File Offset: 0x000296F0
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (!this.m_abilityController.PlayerController.IsGrounded && this.PushbackAmount != Vector2.zero && this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x0400110E RID: 4366
	[SerializeField]
	private Vector2 m_pushbackAmount = Vector2.zero;

	// Token: 0x0400110F RID: 4367
	[HelpBox("OnShoutEvent WARNING: For Player abilities, set FollowTarget to OTHER to properly follow player.", HelpBoxMessageType.Warning)]
	public UnityEvent_GameObject OnShoutEvent;
}
