using System;

// Token: 0x02000185 RID: 389
public class ManaBomb_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06000DE2 RID: 3554 RVA: 0x0002AD0E File Offset: 0x00028F0E
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_abilityController.PlayerController.SetVelocityY(16f, false);
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
	}
}
