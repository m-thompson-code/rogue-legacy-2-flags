using System;

// Token: 0x020002DA RID: 730
public class SuperFart_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x060015CA RID: 5578 RVA: 0x0000ACDA File Offset: 0x00008EDA
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_abilityController.PlayerController.SetVelocityY(25f, false);
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
	}
}
