using System;

// Token: 0x0200069E RID: 1694
public class Breakable_SpellAndTalentOnly : Breakable
{
	// Token: 0x0600340E RID: 13326 RVA: 0x000DC348 File Offset: 0x000DA548
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
