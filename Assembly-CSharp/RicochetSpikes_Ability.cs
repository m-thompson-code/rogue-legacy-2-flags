using System;

// Token: 0x020002B5 RID: 693
public class RicochetSpikes_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x06001460 RID: 5216 RVA: 0x00086DF8 File Offset: 0x00084FF8
	protected override void FireProjectile()
	{
		if (this.ProjectileName != null)
		{
			this.m_spike1 = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, 45f, 1f, false, true, true, true);
			this.m_spike2 = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, -45f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_spike1);
			this.m_abilityController.InitializeProjectile(this.m_spike2);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x04001611 RID: 5649
	private Projectile_RL m_spike1;

	// Token: 0x04001612 RID: 5650
	private Projectile_RL m_spike2;
}
