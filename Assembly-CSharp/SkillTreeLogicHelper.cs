using System;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class SkillTreeLogicHelper
{
	// Token: 0x060028D2 RID: 10450 RVA: 0x000BF168 File Offset: 0x000BD368
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

	// Token: 0x060028D3 RID: 10451 RVA: 0x00016EC9 File Offset: 0x000150C9
	public static int GetCharonGoldStatBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount();
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x00016ED0 File Offset: 0x000150D0
	public static int GetCharonGoldRuneWeightBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount() * 2;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x00016ED0 File Offset: 0x000150D0
	public static int GetCharonGoldArmorBonus()
	{
		return SkillTreeLogicHelper.GetCharonGoldMilestoneCount() * 2;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x00016ED9 File Offset: 0x000150D9
	public static int GetVitalityAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Health_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x00016F0C File Offset: 0x0001510C
	public static int GetStrengthAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Attack_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x00016F3F File Offset: 0x0001513F
	public static int GetMagicAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Attack_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x00016F7B File Offset: 0x0001517B
	public static float GetInvulnTimeExtension()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Invuln_Time_Up).CurrentStatGain;
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x00016F89 File Offset: 0x00015189
	public static int GetEquipWeightAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equip_Up3).CurrentStatGain;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x00016FBF File Offset: 0x000151BF
	public static int GetRuneWeightAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Equip_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldRuneWeightBonus();
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x00016FFB File Offset: 0x000151FB
	public static int GetArmorAdds()
	{
		return 0 + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up2).CurrentStatGain + (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.Armor_Up3).CurrentStatGain + SkillTreeLogicHelper.GetCharonGoldArmorBonus();
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x00017037 File Offset: 0x00015237
	public static float GetDexterityAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add1).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dexterity_Add3).CurrentStatGain + (float)SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x0001706C File Offset: 0x0001526C
	public static float GetCritDamageAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Crit_Damage_Up).CurrentStatGain;
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x00017080 File Offset: 0x00015280
	public static float GetFocusAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up1).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Focus_Up3).CurrentStatGain + (float)SkillTreeLogicHelper.GetCharonGoldStatBonus();
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000170BE File Offset: 0x000152BE
	public static float GetMagicCritDamageAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Crit_Damage_Up).CurrentStatGain;
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000170D5 File Offset: 0x000152D5
	public static float GetAbilityCooldownMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Cooldown_Reduction_Up).CurrentStatGain;
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000170E6 File Offset: 0x000152E6
	public static float GetPotionMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Potion_Up).CurrentStatGain;
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000170F4 File Offset: 0x000152F4
	public static float GetArchitectCostMods()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Architect_Cost_Down).CurrentStatGain;
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x00017105 File Offset: 0x00015305
	public static bool IsClassUnlocked(ClassType classType)
	{
		return SkillTreeLogicHelper.GetClassLevel(classType) > 0;
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000BF1B0 File Offset: 0x000BD3B0
	public static int GetClassLevel(ClassType classType)
	{
		SkillTreeType skillTypeFromClass = SkillTreeLogicHelper.GetSkillTypeFromClass(classType);
		if (skillTypeFromClass == SkillTreeType.None)
		{
			return 0;
		}
		return SkillTreeManager.GetSkillTreeObj(skillTypeFromClass).Level;
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x000BF1D4 File Offset: 0x000BD3D4
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

	// Token: 0x060028E7 RID: 10471 RVA: 0x000BF300 File Offset: 0x000BD500
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

	// Token: 0x060028E8 RID: 10472 RVA: 0x000BF3FC File Offset: 0x000BD5FC
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

	// Token: 0x060028E9 RID: 10473 RVA: 0x00017110 File Offset: 0x00015310
	public static float GetConsecutiveDownstrikeDamageMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Down_Strike_Up).CurrentStatGain;
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x0001711E File Offset: 0x0001531E
	public static bool IsSmithyUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Smithy) > 0;
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x0001712A File Offset: 0x0001532A
	public static bool IsEnchantressUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Enchantress) > 0;
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x00017136 File Offset: 0x00015336
	public static bool IsArchitectUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Architect) > 0;
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x00017142 File Offset: 0x00015342
	public static bool IsLivingSafeUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Gold_Saved_Unlock) > 0;
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x00017151 File Offset: 0x00015351
	public static bool IsTotemUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Unlock_Totem) > 0;
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x00017160 File Offset: 0x00015360
	public static bool IsDummyUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Unlock_Dummy) > 0;
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x0001716F File Offset: 0x0001536F
	public static bool IsRandomizeChildrenUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Randomize_Children) > 0;
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x0001717E File Offset: 0x0001537E
	public static bool IsWeightCDReductionUnlocked()
	{
		return SkillTreeManager.GetSkillObjLevel(SkillTreeType.Weight_CD_Reduce) > 0;
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000BF4B0 File Offset: 0x000BD6B0
	public static float GetGoldGainMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_2).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_3).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_4).CurrentStatGain + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Gold_Gain_Up_5).CurrentStatGain + TraitManager.GetActualTraitGoldGain(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne) + TraitManager.GetActualTraitGoldGain(SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo);
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x0001718D File Offset: 0x0001538D
	public static float GetClassXPMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.XP_Up).CurrentStatGain;
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x0001719E File Offset: 0x0001539E
	public static float GetEquipmentOreMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equipment_Ore_Gain_Up).CurrentStatGain;
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000171AF File Offset: 0x000153AF
	public static float GetRuneOreMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Ore_Gain_Up).CurrentStatGain;
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x000171C0 File Offset: 0x000153C0
	public static float GetBossHPMPRegenMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Boss_Health_Restore).CurrentStatGain;
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x000171D1 File Offset: 0x000153D1
	public static float GetRelicCostMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Relic_Cost_Down).CurrentStatGain;
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x000171E2 File Offset: 0x000153E2
	public static float GetResolveAdds()
	{
		return 0f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Resolve_Up).CurrentStatGain;
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000171F9 File Offset: 0x000153F9
	public static float GetDashDamageMod()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Dash_Strike_Up).CurrentStatGain;
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x0001720A File Offset: 0x0001540A
	public static float GetCritChanceAdds()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Crit_Chance_Flat_Up).CurrentStatGain;
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x0001721B File Offset: 0x0001541B
	public static float GetMagicCritChanceAdds()
	{
		return SkillTreeManager.GetSkillTreeObj(SkillTreeType.Magic_Crit_Chance_Flat_Up).CurrentStatGain;
	}
}
