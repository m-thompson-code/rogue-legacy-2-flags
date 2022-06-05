using System;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class Shout_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0000ABCF File Offset: 0x00008DCF
	public virtual Vector2 PushbackAmount
	{
		get
		{
			return this.m_pushbackAmount;
		}
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0008A6D4 File Offset: 0x000888D4
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

	// Token: 0x060015AB RID: 5547 RVA: 0x0008A7C8 File Offset: 0x000889C8
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (!this.m_abilityController.PlayerController.IsGrounded && this.PushbackAmount != Vector2.zero && this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x04001691 RID: 5777
	[SerializeField]
	private Vector2 m_pushbackAmount = Vector2.zero;

	// Token: 0x04001692 RID: 5778
	[HelpBox("OnShoutEvent WARNING: For Player abilities, set FollowTarget to OTHER to properly follow player.", HelpBoxMessageType.Warning)]
	public UnityEvent_GameObject OnShoutEvent;
}
