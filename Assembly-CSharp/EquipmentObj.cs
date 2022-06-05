using System;
using UnityEngine;

// Token: 0x020001F6 RID: 502
[Serializable]
public class EquipmentObj
{
	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06001554 RID: 5460 RVA: 0x000420F3 File Offset: 0x000402F3
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

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x06001555 RID: 5461 RVA: 0x00042123 File Offset: 0x00040323
	public int ClampedUpgradeLevel
	{
		get
		{
			return Mathf.Clamp(this.UpgradeLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x06001556 RID: 5462 RVA: 0x00042137 File Offset: 0x00040337
	public bool IsMaxUpgradeLevel
	{
		get
		{
			return this.ClampedUpgradeLevel == this.MaxLevel;
		}
	}

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x06001557 RID: 5463 RVA: 0x00042147 File Offset: 0x00040347
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

	// Token: 0x06001558 RID: 5464 RVA: 0x00042170 File Offset: 0x00040370
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

	// Token: 0x06001559 RID: 5465 RVA: 0x000422FB File Offset: 0x000404FB
	public float GetCurrentStatValue(EquipmentStatType statType)
	{
		if (this.EquipmentData.Disabled)
		{
			return 0f;
		}
		return this.GetStatValueAtLevel(statType, this.ClampedUpgradeLevel);
	}

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x0600155A RID: 5466 RVA: 0x00042320 File Offset: 0x00040520
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

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x0600155B RID: 5467 RVA: 0x00042378 File Offset: 0x00040578
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

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x0600155C RID: 5468 RVA: 0x000423E4 File Offset: 0x000405E4
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

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x0600155D RID: 5469 RVA: 0x00042433 File Offset: 0x00040633
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

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x0600155E RID: 5470 RVA: 0x00042462 File Offset: 0x00040662
	public EquipmentData EquipmentData
	{
		get
		{
			return EquipmentLibrary.GetEquipmentData(this.CategoryType, this.EquipmentType);
		}
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00042475 File Offset: 0x00040675
	public EquipmentObj(EquipmentCategoryType category, EquipmentType equipType)
	{
		this.CategoryType = category;
		this.EquipmentType = equipType;
	}

	// Token: 0x0400149E RID: 5278
	public EquipmentCategoryType CategoryType;

	// Token: 0x0400149F RID: 5279
	public EquipmentType EquipmentType;

	// Token: 0x040014A0 RID: 5280
	public int UpgradeLevel = -3;

	// Token: 0x040014A1 RID: 5281
	public int UpgradeBlueprintsFound;
}
