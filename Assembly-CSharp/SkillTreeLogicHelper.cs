using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class SkillTreeLogicHelper
{
	// Token: 0x06001D8D RID: 7565 RVA: 0x000615D8 File Offset: 0x0005F7D8
	public static int GetCharonGoldMilestoneCount()
	{
		int num = 0;
		if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Charon_Gold_Stat_Bonus) > 0)
		{
			int maxCharonBonusLevel = SkillTree_EV.GetMaxCharonBonusLevel();
			int num2 = 0;
			while (num2 < maxCharonBonusLevel && SaveManager.PlayerSaveData.GoldAcceptedByCharon >= SkillTree_EV.CHARON_GOLD_STAT_BONUS_MILESTONES[num2])
			{
				num++;
				num2++;
			}
		}
		return num;
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0006161E File Offset: 0x0005F81E
	public static int GetCharonGoldStatBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount();
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x00061625 File Offset: 0x0005F825
	public static int GetCharonGoldRuneWeightBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount() * 2;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0006162E File Offset: 0x0005F82E
	public static int GetCharonGoldArmorBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount() * 2;
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x00061637 File Offset: 0x0005F837
	public static int GetVitalityAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0006166A File Offset: 0x0005F86A
	public static int GetStrengthAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x0006169D File Offset: 0x0005F89D
	public static int GetMagicAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x000616D9 File Offset: 0x0005F8D9
	public static float GetInvulnTimeExtension()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Invuln_Time_Up).CurrentStatGain;
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x000616E7 File Offset: 0x0005F8E7
	public static int GetEquipWeightAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up3).CurrentStatGain;
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x0006171D File Offset: 0x0005F91D
	public static int GetRuneWeightAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldRuneWeightBonus();
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x00061759 File Offset: 0x0005F959
	public static int GetArmorAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldArmorBonus();
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x00061795 File Offset: 0x0005F995
	public static float GetDexterityAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add1).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add3).CurrentStatGain + (float)SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000617CA File Offset: 0x0005F9CA
	public static float GetCritDamageAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Crit_Damage_Up).CurrentStatGain;
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000617DE File Offset: 0x0005F9DE
	public static float GetFocusAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up1).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up3).CurrentStatGain + (float)SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x0006181C File Offset: 0x0005FA1C
	public static float GetMagicCritDamageAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Crit_Damage_Up).CurrentStatGain;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x00061833 File Offset: 0x0005FA33
	public static float GetAbilityCooldownMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Cooldown_Reduction_Up).CurrentStatGain;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x00061844 File Offset: 0x0005FA44
	public static float GetPotionMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Potion_Up).CurrentStatGain;
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x00061852 File Offset: 0x0005FA52
	public static float GetArchitectCostMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Architect_Cost_Down).CurrentStatGain;
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x00061863 File Offset: 0x0005FA63
	public static bool IsClassUnlocked(ClassType classType)
	{
		return SkillTreeLogicHelper.GetClassLevel(classType) > 0;
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x00061870 File Offset: 0x0005FA70
	public static int GetClassLevel(ClassType classType)
	{
		SkillTreeType skillTypeFromClass = SkillTreeLogicHelper.GetSkillTypeFromClass(classType);
		if (skillTypeFromClass == SkillTreeType.None)
		{
			return 0;
		}
		return SkillTreeManager.GetSkillTreeObj(skillTypeFromClass).Level;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x00061894 File Offset: 0x0005FA94
	public static SkillTreeType GetSkillTypeFromClass(ClassType classType)
	{
		if (classType <= ClassType.SaberClass)
		{
			if (classType <= ClassType.MagicWandClass)
			{
				if (classType == ClassType.SwordClass)
				{
					return SkillTreeType.Sword_Class_Unlock;
				}
				if (classType == ClassType.AxeClass)
				{
					return SkillTreeType.Axe_Class_Unlock;
				}
				if (classType == ClassType.MagicWandClass)
				{
					return SkillTreeType.Wand_Class_Unlock;
				}
			}
			else if (classType <= ClassType.SpearClass)
			{
				if (classType == ClassType.DualBladesClass)
				{
					return SkillTreeType.DualBlades_Class_Unlock;
				}
				if (classType == ClassType.SpearClass)
				{
					return SkillTreeType.Spear_Class_Unlock;
				}
			}
			else
			{
				if (classType == ClassType.BowClass)
				{
					return SkillTreeType.Bow_Class_Unlock;
				}
				if (classType == ClassType.SaberClass)
				{
					return SkillTreeType.Saber_Class_Unlock;
				}
			}
		}
		else if (classType <= ClassType.GunClass)
		{
			if (classType <= ClassType.BoxingGloveClass)
			{
				if (classType == ClassType.LadleClass)
				{
					return SkillTreeType.Ladle_Class_Unlock;
				}
				if (classType == ClassType.BoxingGloveClass)
				{
					return SkillTreeType.BoxingGlove_Class_Unlock;
				}
			}
			else
			{
				if (classType == ClassType.LanceClass)
				{
					return SkillTreeType.Lancer_Class_Unlock;
				}
				if (classType == ClassType.GunClass)
				{
					return SkillTreeType.Gun_Class_Unlock;
				}
			}
		}
		else if (classType <= ClassType.CannonClass)
		{
			if (classType == ClassType.KatanaClass)
			{
				return SkillTreeType.Samurai_Class_Unlock;
			}
			if (classType == ClassType.CannonClass)
			{
				return SkillTreeType.Pirate_Class_Unlock;
			}
		}
		else
		{
			if (classType == ClassType.LuteClass)
			{
				return SkillTreeType.Music_Class_Unlock;
			}
			if (classType == ClassType.AstroClass)
			{
				return SkillTreeType.Astro_Class_Unlock;
			}
		}
		return SkillTreeType.None;
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000619C0 File Offset: 0x0005FBC0
	public static ClassType GetClassTypeFromSkill(SkillTreeType skillType)
	{
		if (skillType <= SkillTreeType.Axe_Class_Unlock)
		{
			if (skillType <= SkillTreeType.Saber_Class_Unlock)
			{
				if (skillType == SkillTreeType.BoxingGlove_Class_Unlock)
				{
					return ClassType.BoxingGloveClass;
				}
				if (skillType == SkillTreeType.Saber_Class_Unlock)
				{
					return ClassType.SaberClass;
				}
			}
			else
			{
				if (skillType == SkillTreeType.DualBlades_Class_Unlock)
				{
					return ClassType.DualBladesClass;
				}
				if (skillType == SkillTreeType.Ladle_Class_Unlock)
				{
					return ClassType.LadleClass;
				}
				if (skillType == SkillTreeType.Axe_Class_Unlock)
				{
					return ClassType.AxeClass;
				}
			}
		}
		else if (skillType <= SkillTreeType.Spear_Class_Unlock)
		{
			if (skillType == SkillTreeType.Wand_Class_Unlock)
			{
				return ClassType.MagicWandClass;
			}
			if (skillType == SkillTreeType.Bow_Class_Unlock)
			{
				return ClassType.BowClass;
			}
			if (skillType == SkillTreeType.Spear_Class_Unlock)
			{
				return ClassType.SpearClass;
			}
		}
		else
		{
			switch (skillType)
			{
			case SkillTreeType.Gun_Class_Unlock:
				return ClassType.GunClass;
			case (SkillTreeType)491:
				break;
			case SkillTreeType.Samurai_Class_Unlock:
				return ClassType.KatanaClass;
			case SkillTreeType.Music_Class_Unlock:
				return ClassType.LuteClass;
			case SkillTreeType.Pirate_Class_Unlock:
				return ClassType.CannonClass;
			case SkillTreeType.Astro_Class_Unlock:
				return ClassType.AstroClass;
			default:
				if (skillType == SkillTreeType.Knight_Upgrade)
				{
					return ClassType.SwordClass;
				}
				if (skillType == SkillTreeType.Lancer_Class_Unlock)
				{
					return ClassType.LanceClass;
				}
				break;
			}
		}
		return ClassType.None;
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x00061ABC File Offset: 0x0005FCBC
	public static DeathDefiedType IsDeathDefied()
	{
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_Unity).Level > 0)
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_UnityUsed).SetLevel(1, true, true);
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife_Unity).SetLevel(-1, true, true);
			return DeathDefiedType.ExtraLife_Unity;
		}
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife).Level > 0)
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLife).SetLevel(-1, true, true);
			SaveManager.PlayerSaveData.GetRelic(RelicType.ExtraLifeUsed).SetLevel(1, true, true);
			return DeathDefiedType.ExtraLife;
		}
		float currentStatGain = SkillTreeManager.GetSkillTreeObj(SkillTreeType.Death_Dodge).CurrentStatGain;
		if (UnityEngine.Random.Range(0f, 1f) <= currentStatGain)
		{
			return DeathDefiedType.Death_Dodge;
		}
		if (ChallengeManager.IsInChallenge)
		{
			return DeathDefiedType.FailedChallenge;
		}
		return DeathDefiedType.None;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x00061B70 File Offset: 0x0005FD70
	public static float GetConsecutiveDownstrikeDamageMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Down_Strike_Up).CurrentStatGain;
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x00061B7E File Offset: 0x0005FD7E
	public static bool IsSmithyUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Smithy) > 0;
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x00061B8A File Offset: 0x0005FD8A
	public static bool IsEnchantressUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Enchantress) > 0;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x00061B96 File Offset: 0x0005FD96
	public static bool IsArchitectUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Architect) > 0;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x00061BA2 File Offset: 0x0005FDA2
	public static bool IsLivingSafeUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Gold_Saved_Unlock) > 0;
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x00061BB1 File Offset: 0x0005FDB1
	public static bool IsTotemUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Unlock_Totem) > 0;
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x00061BC0 File Offset: 0x0005FDC0
	public static bool IsDummyUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Unlock_Dummy) > 0;
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x00061BCF File Offset: 0x0005FDCF
	public static bool IsRandomizeChildrenUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Randomize_Children) > 0;
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x00061BDE File Offset: 0x0005FDDE
	public static bool IsWeightCDReductionUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Weight_CD_Reduce) > 0;
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x00061BF0 File Offset: 0x0005FDF0
	public static float GetGoldGainMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_3).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_4).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_5).CurrentStatGain + TraitManager.GetActualTraitGoldGain(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne) + TraitManager.GetActualTraitGoldGain(SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x00061C76 File Offset: 0x0005FE76
	public static float GetClassXPMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.XP_Up).CurrentStatGain;
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x00061C87 File Offset: 0x0005FE87
	public static float GetEquipmentOreMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equipment_Ore_Gain_Up).CurrentStatGain;
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x00061C98 File Offset: 0x0005FE98
	public static float GetRuneOreMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Ore_Gain_Up).CurrentStatGain;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x00061CA9 File Offset: 0x0005FEA9
	public static float GetBossHPMPRegenMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Boss_Health_Restore).CurrentStatGain;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x00061CBA File Offset: 0x0005FEBA
	public static float GetRelicCostMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Relic_Cost_Down).CurrentStatGain;
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x00061CCB File Offset: 0x0005FECB
	public static float GetResolveAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Resolve_Up).CurrentStatGain;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x00061CE2 File Offset: 0x0005FEE2
	public static float GetDashDamageMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dash_Strike_Up).CurrentStatGain;
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x00061CF3 File Offset: 0x0005FEF3
	public static float GetCritChanceAdds()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Crit_Chance_Flat_Up).CurrentStatGain;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x00061D04 File Offset: 0x0005FF04
	public static float GetMagicCritChanceAdds()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Crit_Chance_Flat_Up).CurrentStatGain;
	}
}
