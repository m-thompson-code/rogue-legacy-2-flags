using System;

// Token: 0x02000176 RID: 374
public class RicochetSpikes_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x00027D4C File Offset: 0x00025F4C
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

	// Token: 0x040010BF RID: 4287
	private Projectile_RL m_spike1;

	// Token: 0x040010C0 RID: 4288
	private Projectile_RL m_spike2;
}
