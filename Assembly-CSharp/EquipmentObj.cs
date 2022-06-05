using System;
using UnityEngine;

// Token: 0x02000396 RID: 918
[Serializable]
public class EquipmentObj
{
	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x06001E99 RID: 7833 RVA: 0x0000FFB2 File Offset: 0x0000E1B2
	public FoundState FoundState
	{
		get
		{
			if (this.EquipmentData.Disabled)
			{
				return FoundState.NotFound;
			}
			if (this.UpgradeLevel < -3)
			{
				return FoundState.NotFound;
			}
			if (this.UpgradeLevel > 0)
			{
				return FoundState.Purchased;
			}
			return (FoundState)this.UpgradeLevel;
		}
	}

	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x06001E9A RID: 7834 RVA: 0x0000FFE2 File Offset: 0x0000E1E2
	public int ClampedUpgradeLevel
	{
		get
		{
			return Mathf.Clamp(this.UpgradeLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0000FFF6 File Offset: 0x0000E1F6
	public bool IsMaxUpgradeLevel
	{
		get
		{
			return this.ClampedUpgradeLevel == this.MaxLevel;
		}
	}

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x06001E9C RID: 7836 RVA: 0x00010006 File Offset: 0x0000E206
	public bool HasMaxBlueprints
	{
		get
		{
			if (this.MaxLevel == 0)
			{
				return this.FoundState > FoundState.NotFound;
			}
			return this.UpgradeBlueprintsFound >= this.MaxLevel;
		}
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000A0704 File Offset: 0x0009E904
	public float GetStatValueAtLevel(EquipmentStatType statType, int level)
	{
		if (this.EquipmentData.Disabled)
		{
			return 0f;
		}
		if (level < 0)
		{
			return 0f;
		}
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		switch (statType)
		{
		case EquipmentStatType.Weight:
			return (float)(this.EquipmentData.BaseWeight + this.EquipmentData.ScalingWeight * level);
		case EquipmentStatType.Cooldown:
			return 0f;
		case EquipmentStatType.Strength:
			return (float)(this.EquipmentData.BaseStrengthDamage + this.EquipmentData.ScalingStrengthDamage * level);
		case EquipmentStatType.Vitality:
			return (float)(this.EquipmentData.BaseHealth + this.EquipmentData.ScalingHealth * level);
		case EquipmentStatType.Magic:
			return (float)(this.EquipmentData.BaseMagicDamage + this.EquipmentData.ScalingMagicDamage * level);
		case EquipmentStatType.Armor:
			return (float)(this.EquipmentData.BaseArmor + this.EquipmentData.ScalingArmor * level);
		case EquipmentStatType.Dexterity_Add:
			return this.EquipmentData.BaseStrengthCritChance + this.EquipmentData.ScalingStrengthCritChance * (float)level;
		case EquipmentStatType.CritDamage:
			return this.EquipmentData.BaseStrengthCritDamage + this.EquipmentData.ScalingStrengthCritDamage * (float)level;
		case EquipmentStatType.Focus_Add:
			return this.EquipmentData.BaseMagicCritChance + this.EquipmentData.ScalingMagicCritChance * (float)level;
		case EquipmentStatType.MagicCritDamage:
			return this.EquipmentData.BaseMagicCritDamage + this.EquipmentData.ScalingMagicCritDamage * (float)level;
		case EquipmentStatType.Unity:
			return (float)(this.EquipmentData.BaseEquipmentSetLevel + this.EquipmentData.ScalingEquipmentSetLevel * level);
		default:
			return 0f;
		}
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x0001002C File Offset: 0x0000E22C
	public float GetCurrentStatValue(EquipmentStatType statType)
	{
		if (this.EquipmentData.Disabled)
		{
			return 0f;
		}
		return this.GetStatValueAtLevel(statType, this.ClampedUpgradeLevel);
	}

	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000A0890 File Offset: 0x0009EA90
	public int MaxLevel
	{
		get
		{
			if (this.EquipmentData == null)
			{
				return 0;
			}
			int num = this.EquipmentData.MaximumLevel;
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.MaxEquipmentDrops);
			if (!soulShopObj.IsNativeNull())
			{
				num += soulShopObj.CurrentEquippedLevel;
			}
			return Mathf.Clamp(num, 0, Economy_EV.EQUIPMENT_LEVEL_GOLD_MOD.Length - 1);
		}
	}

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000A08E8 File Offset: 0x0009EAE8
	public int GoldCostToUpgrade
	{
		get
		{
			if (this.EquipmentData.Disabled)
			{
				return 0;
			}
			int num = this.EquipmentData.GoldCost;
			int num2 = this.UpgradeLevel;
			if (num2 >= 0)
			{
				num2++;
			}
			num2 = Mathf.Clamp(num2, 0, Economy_EV.EQUIPMENT_LEVEL_GOLD_MOD.Length - 1);
			num *= Economy_EV.EQUIPMENT_LEVEL_GOLD_MOD[num2];
			if (TraitManager.IsTraitActive(TraitType.LowerStorePrice))
			{
				num = (int)((float)num * 0.85f);
			}
			return num;
		}
	}

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000A0954 File Offset: 0x0009EB54
	public int OreCostToUpgrade
	{
		get
		{
			if (this.EquipmentData.Disabled)
			{
				return 0;
			}
			int oreCost = this.EquipmentData.OreCost;
			int num = this.UpgradeLevel;
			if (num >= 0)
			{
				num++;
			}
			num = Mathf.Clamp(num, 0, Economy_EV.EQUIPMENT_LEVEL_ORE_MOD.Length - 1);
			return oreCost * Economy_EV.EQUIPMENT_LEVEL_ORE_MOD[num];
		}
	}

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x0001004E File Offset: 0x0000E24E
	public int EquipmentSetLevel
	{
		get
		{
			if (this.EquipmentData.Disabled)
			{
				return 0;
			}
			return this.EquipmentData.BaseEquipmentSetLevel + this.EquipmentData.ScalingEquipmentSetLevel * this.ClampedUpgradeLevel;
		}
	}

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x0001007D File Offset: 0x0000E27D
	public EquipmentData EquipmentData
	{
		get
		{
			return EquipmentLibrary.GetEquipmentData(this.CategoryType, this.EquipmentType);
		}
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x00010090 File Offset: 0x0000E290
	public EquipmentObj(EquipmentCategoryType category, EquipmentType equipType)
	{
		this.CategoryType = category;
		this.EquipmentType = equipType;
	}

	// Token: 0x04001B5B RID: 7003
	public EquipmentCategoryType CategoryType;

	// Token: 0x04001B5C RID: 7004
	public EquipmentType EquipmentType;

	// Token: 0x04001B5D RID: 7005
	public int UpgradeLevel = -3;

	// Token: 0x04001B5E RID: 7006
	public int UpgradeBlueprintsFound;
}
