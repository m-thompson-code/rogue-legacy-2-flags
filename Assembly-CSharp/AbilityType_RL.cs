using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000BAE RID: 2990
public static class AbilityType_RL
{
	// Token: 0x17001E0F RID: 7695
	// (get) Token: 0x060059FB RID: 23035 RVA: 0x00031233 File Offset: 0x0002F433
	public static AbilityType[] TypeArray
	{
		get
		{
			if (AbilityType_RL.m_typeArray == null)
			{
				AbilityType_RL.m_typeArray = (Enum.GetValues(typeof(AbilityType)) as AbilityType[]);
			}
			return AbilityType_RL.m_typeArray;
		}
	}

	// Token: 0x17001E10 RID: 7696
	// (get) Token: 0x060059FC RID: 23036 RVA: 0x001549FC File Offset: 0x00152BFC
	public static AbilityType[] WeaponAbilityArray
	{
		get
		{
			if (AbilityType_RL.m_weaponTypeArray == null)
			{
				int num = AbilityType_RL.TypeArray.IndexOf(AbilityType.____WEAPON_ABILITIES____) + 1;
				int num2 = AbilityType_RL.TypeArray.IndexOf(AbilityType.____SPELL_ABILITIES____);
				int num3 = num2 - num;
				if (num > -1 && num2 > -1 && num3 > 0)
				{
					AbilityType_RL.m_weaponTypeArray = new AbilityType[num3];
					int num4 = 0;
					for (int i = num; i < num2; i++)
					{
						AbilityType_RL.m_weaponTypeArray[num4] = AbilityType_RL.TypeArray[i];
						num4++;
					}
				}
			}
			return AbilityType_RL.m_weaponTypeArray;
		}
	}

	// Token: 0x17001E11 RID: 7697
	// (get) Token: 0x060059FD RID: 23037 RVA: 0x00154A74 File Offset: 0x00152C74
	public static AbilityType[] ValidWeaponTypeArray
	{
		get
		{
			if (AbilityType_RL.m_validWeaponTypeArray == null)
			{
				AbilityType[] weaponAbilityArray = ClassLibrary.GetClassData(ClassType.CURIO_SHOPPE_CLASS).WeaponData.WeaponAbilityArray;
				AbilityType_RL.m_validWeaponTypeArray = new AbilityType[weaponAbilityArray.Length];
				Array.Copy(weaponAbilityArray, AbilityType_RL.m_validWeaponTypeArray, weaponAbilityArray.Length);
			}
			return AbilityType_RL.m_validWeaponTypeArray;
		}
	}

	// Token: 0x17001E12 RID: 7698
	// (get) Token: 0x060059FE RID: 23038 RVA: 0x00154AC0 File Offset: 0x00152CC0
	public static AbilityType[] SpellAbilityArray
	{
		get
		{
			if (AbilityType_RL.m_spellTypeArray == null)
			{
				int num = AbilityType_RL.TypeArray.IndexOf(AbilityType.____SPELL_ABILITIES____) + 1;
				int num2 = AbilityType_RL.TypeArray.IndexOf(AbilityType.____TALENT_ABILITIES____);
				int num3 = num2 - num;
				if (num > -1 && num2 > -1 && num3 > 0)
				{
					AbilityType_RL.m_spellTypeArray = new AbilityType[num3];
					int num4 = 0;
					for (int i = num; i < num2; i++)
					{
						AbilityType_RL.m_spellTypeArray[num4] = AbilityType_RL.TypeArray[i];
						num4++;
					}
				}
			}
			return AbilityType_RL.m_spellTypeArray;
		}
	}

	// Token: 0x17001E13 RID: 7699
	// (get) Token: 0x060059FF RID: 23039 RVA: 0x00154B3C File Offset: 0x00152D3C
	public static AbilityType[] ValidSpellTypeArray
	{
		get
		{
			if (AbilityType_RL.m_validSpellTypeArray == null)
			{
				AbilityType[] spellAbilityArray = ClassLibrary.GetClassData(ClassType.CURIO_SHOPPE_CLASS).SpellData.SpellAbilityArray;
				AbilityType_RL.m_validSpellTypeArray = new AbilityType[spellAbilityArray.Length];
				Array.Copy(spellAbilityArray, AbilityType_RL.m_validSpellTypeArray, spellAbilityArray.Length);
			}
			return AbilityType_RL.m_validSpellTypeArray;
		}
	}

	// Token: 0x17001E14 RID: 7700
	// (get) Token: 0x06005A00 RID: 23040 RVA: 0x00154B88 File Offset: 0x00152D88
	public static AbilityType[] TalentAbilityArray
	{
		get
		{
			if (AbilityType_RL.m_talentTypeArray == null)
			{
				int num = AbilityType_RL.TypeArray.IndexOf(AbilityType.____TALENT_ABILITIES____) + 1;
				int num2 = AbilityType_RL.TypeArray.IndexOf(AbilityType.____SPECIAL_ABILITIES____);
				int num3 = num2 - num;
				if (num > -1 && num2 > -1 && num3 > 0)
				{
					AbilityType_RL.m_talentTypeArray = new AbilityType[num3];
					int num4 = 0;
					for (int i = num; i < num2; i++)
					{
						AbilityType_RL.m_talentTypeArray[num4] = AbilityType_RL.TypeArray[i];
						num4++;
					}
				}
			}
			return AbilityType_RL.m_talentTypeArray;
		}
	}

	// Token: 0x17001E15 RID: 7701
	// (get) Token: 0x06005A01 RID: 23041 RVA: 0x00154C04 File Offset: 0x00152E04
	public static AbilityType[] ValidTalentTypeArray
	{
		get
		{
			if (AbilityType_RL.m_validTalentTypeArray == null)
			{
				AbilityType[] talentAbilityArray = ClassLibrary.GetClassData(ClassType.CURIO_SHOPPE_CLASS).TalentData.TalentAbilityArray;
				AbilityType_RL.m_validTalentTypeArray = new AbilityType[talentAbilityArray.Length];
				Array.Copy(talentAbilityArray, AbilityType_RL.m_validTalentTypeArray, talentAbilityArray.Length);
			}
			return AbilityType_RL.m_validTalentTypeArray;
		}
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x00154C50 File Offset: 0x00152E50
	public static List<AbilityType> GetWeaponAbilityList()
	{
		List<AbilityType> list = ((AbilityType[])Enum.GetValues(typeof(AbilityType))).ToList<AbilityType>();
		List<AbilityType> list2 = new List<AbilityType>();
		int num = list.IndexOf(AbilityType.____WEAPON_ABILITIES____) + 1;
		int num2 = list.IndexOf(AbilityType.____SPELL_ABILITIES____);
		for (int i = num; i < num2; i++)
		{
			list2.Add(list[i]);
		}
		return list2;
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x00154CAC File Offset: 0x00152EAC
	public static List<AbilityType> GetSpellAbilityList()
	{
		List<AbilityType> list = ((AbilityType[])Enum.GetValues(typeof(AbilityType))).ToList<AbilityType>();
		List<AbilityType> list2 = new List<AbilityType>();
		int num = list.IndexOf(AbilityType.____SPELL_ABILITIES____) + 1;
		int num2 = list.IndexOf(AbilityType.____TALENT_ABILITIES____);
		for (int i = num; i < num2; i++)
		{
			list2.Add(list[i]);
		}
		return list2;
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x00154D0C File Offset: 0x00152F0C
	public static List<AbilityType> GetTalentAbilityList()
	{
		List<AbilityType> list = ((AbilityType[])Enum.GetValues(typeof(AbilityType))).ToList<AbilityType>();
		List<AbilityType> list2 = new List<AbilityType>();
		int num = list.IndexOf(AbilityType.____TALENT_ABILITIES____) + 1;
		int num2 = list.IndexOf(AbilityType.____SPECIAL_ABILITIES____);
		for (int i = num; i < num2; i++)
		{
			list2.Add(list[i]);
		}
		return list2;
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x00154D6C File Offset: 0x00152F6C
	public static bool IsMageSpellOrTalent(AbilityType abilityType)
	{
		ClassData classData = ClassLibrary.GetClassData(ClassType.MagicWandClass);
		return classData.SpellData.SpellAbilityArray.Contains(abilityType) || classData.TalentData.TalentAbilityArray.Contains(abilityType);
	}

	// Token: 0x040044B4 RID: 17588
	private static AbilityType[] m_typeArray;

	// Token: 0x040044B5 RID: 17589
	private static AbilityType[] m_weaponTypeArray;

	// Token: 0x040044B6 RID: 17590
	private static AbilityType[] m_spellTypeArray;

	// Token: 0x040044B7 RID: 17591
	private static AbilityType[] m_talentTypeArray;

	// Token: 0x040044B8 RID: 17592
	private static AbilityType[] m_validWeaponTypeArray;

	// Token: 0x040044B9 RID: 17593
	private static AbilityType[] m_validSpellTypeArray;

	// Token: 0x040044BA RID: 17594
	private static AbilityType[] m_validTalentTypeArray;
}
