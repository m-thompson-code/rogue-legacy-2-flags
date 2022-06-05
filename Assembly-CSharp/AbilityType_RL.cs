using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020006FB RID: 1787
public static class AbilityType_RL
{
	// Token: 0x17001613 RID: 5651
	// (get) Token: 0x060040B2 RID: 16562 RVA: 0x000E52EC File Offset: 0x000E34EC
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

	// Token: 0x17001614 RID: 5652
	// (get) Token: 0x060040B3 RID: 16563 RVA: 0x000E5314 File Offset: 0x000E3514
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

	// Token: 0x17001615 RID: 5653
	// (get) Token: 0x060040B4 RID: 16564 RVA: 0x000E538C File Offset: 0x000E358C
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

	// Token: 0x17001616 RID: 5654
	// (get) Token: 0x060040B5 RID: 16565 RVA: 0x000E53D8 File Offset: 0x000E35D8
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

	// Token: 0x17001617 RID: 5655
	// (get) Token: 0x060040B6 RID: 16566 RVA: 0x000E5454 File Offset: 0x000E3654
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

	// Token: 0x17001618 RID: 5656
	// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000E54A0 File Offset: 0x000E36A0
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

	// Token: 0x17001619 RID: 5657
	// (get) Token: 0x060040B8 RID: 16568 RVA: 0x000E551C File Offset: 0x000E371C
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

	// Token: 0x060040B9 RID: 16569 RVA: 0x000E5568 File Offset: 0x000E3768
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

	// Token: 0x060040BA RID: 16570 RVA: 0x000E55C4 File Offset: 0x000E37C4
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

	// Token: 0x060040BB RID: 16571 RVA: 0x000E5624 File Offset: 0x000E3824
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

	// Token: 0x060040BC RID: 16572 RVA: 0x000E5684 File Offset: 0x000E3884
	public static bool IsMageSpellOrTalent(AbilityType abilityType)
	{
		ClassData classData = ClassLibrary.GetClassData(ClassType.MagicWandClass);
		return classData.SpellData.SpellAbilityArray.Contains(abilityType) || classData.TalentData.TalentAbilityArray.Contains(abilityType);
	}

	// Token: 0x04003239 RID: 12857
	private static AbilityType[] m_typeArray;

	// Token: 0x0400323A RID: 12858
	private static AbilityType[] m_weaponTypeArray;

	// Token: 0x0400323B RID: 12859
	private static AbilityType[] m_spellTypeArray;

	// Token: 0x0400323C RID: 12860
	private static AbilityType[] m_talentTypeArray;

	// Token: 0x0400323D RID: 12861
	private static AbilityType[] m_validWeaponTypeArray;

	// Token: 0x0400323E RID: 12862
	private static AbilityType[] m_validSpellTypeArray;

	// Token: 0x0400323F RID: 12863
	private static AbilityType[] m_validTalentTypeArray;
}
