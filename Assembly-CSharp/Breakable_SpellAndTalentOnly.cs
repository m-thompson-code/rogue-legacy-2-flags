using System;

// Token: 0x020003F5 RID: 1013
public class Breakable_SpellAndTalentOnly : Breakable
{
	// Token: 0x060025A0 RID: 9632 RVA: 0x0007C660 File Offset: 0x0007A860
	protected override void TriggerCollision(IDamageObj damageObj)
	{
		if (damageObj.gameObject.CompareTag("PlayerProjectile"))
		{
			Projectile_RL projectile_RL = damageObj as Projectile_RL;
			if (projectile_RL != null)
			{
				bool flag = projectile_RL.CastAbilityType == CastAbilityType.Spell || projectile_RL.CastAbilityType == CastAbilityType.Talent;
				bool flag2 = projectile_RL.CastAbilityType == CastAbilityType.Weapon && TraitManager.IsTraitActive(TraitType.CantAttack);
				if (flag || flag2)
				{
					base.TriggerCollision(damageObj);
				}
			}
		}
	}
}
