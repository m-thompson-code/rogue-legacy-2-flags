using System;

// Token: 0x020002CE RID: 718
public class ManaBomb_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06001563 RID: 5475 RVA: 0x0000AA32 File Offset: 0x00008C32
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_abilityController.PlayerController.SetVelocityY(16f, false);
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
	}
}
