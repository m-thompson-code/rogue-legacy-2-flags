using System;

// Token: 0x0200018C RID: 396
public class SuperFart_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06000E2B RID: 3627 RVA: 0x0002B876 File Offset: 0x00029A76
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_abilityController.PlayerController.SetVelocityY(25f, false);
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
	}
}
