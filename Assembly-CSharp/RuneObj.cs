using System;
using UnityEngine;

// Token: 0x020002C6 RID: 710
[Serializable]
public class RuneObj
{
	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06001C4F RID: 7247 RVA: 0x0005B8A7 File Offset: 0x00059AA7
	public FoundState FoundState
	{
		get
		{
			if (this.RuneData.Disabled)
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

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0005B8D7 File Offset: 0x00059AD7
	public int ClampedUpgradeLevel
	{
		get
		{
			return Mathf.Clamp(this.UpgradeLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06001C51 RID: 7249 RVA: 0x0005B8EB File Offset: 0x00059AEB
	public bool IsMaxEquippedLevel
	{
		get
		{
			return this.EquippedLevel == this.MaxLevel;
		}
	}

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06001C52 RID: 7250 RVA: 0x0005B8FB File Offset: 0x00059AFB
	public bool IsMaxUpgradeLevel
	{
		get
		{
			return this.ClampedUpgradeLevel == this.MaxLevel;
		}
	}

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06001C53 RID: 7251 RVA: 0x0005B90B File Offset: 0x00059B0B
	public bool HasMaxBlueprints
	{
		get
		{
			return this.UpgradeBlueprintsFound >= this.MaxLevel;
		}
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x0005B920 File Offset: 0x00059B20
	public float GetStatModTotal_1AtLevel(int level)
	{
		if (this.RuneData.Disabled)
		{
			return 0f;
		}
		if (level <= 0)
		{
			return 0f;
		}
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		return this.RuneData.StatMod01 + (float)(level - 1) * this.RuneData.ScalingStatMod01;
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x0005B978 File Offset: 0x00059B78
	public int GetWeightAtLevel(int level)
	{
		if (this.RuneData.Disabled)
		{
			return 0;
		}
		if (level <= 0)
		{
			return 0;
		}
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		return this.RuneData.BaseWeight + this.RuneData.ScalingWeight * (level - 1);
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06001C56 RID: 7254 RVA: 0x0005B9C4 File Offset: 0x00059BC4
	public float CurrentStatModTotal_1
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0f;
			}
			if (this.EquippedLevel <= 0)
			{
				return 0f;
			}
			return this.RuneData.StatMod01 + (float)(this.EquippedLevel - 1) * this.RuneData.ScalingStatMod01;
		}
	}

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x06001C57 RID: 7255 RVA: 0x0005BA14 File Offset: 0x00059C14
	public float StatModTotal_2
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0f;
			}
			if (this.EquippedLevel <= 0)
			{
				return 0f;
			}
			return this.RuneData.StatMod02 + (float)(this.EquippedLevel - 1) * this.RuneData.ScalingStatMod02;
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x06001C58 RID: 7256 RVA: 0x0005BA64 File Offset: 0x00059C64
	public float StatModTotal_3
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0f;
			}
			if (this.EquippedLevel <= 0)
			{
				return 0f;
			}
			return this.RuneData.StatMod03 + (float)(this.EquippedLevel - 1) * this.RuneData.ScalingStatMod03;
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x06001C59 RID: 7257 RVA: 0x0005BAB4 File Offset: 0x00059CB4
	public int CurrentWeight
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0;
			}
			if (this.EquippedLevel <= 0)
			{
				return 0;
			}
			return this.RuneData.BaseWeight + this.RuneData.ScalingWeight * (this.EquippedLevel - 1);
		}
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x06001C5A RID: 7258 RVA: 0x0005BAF0 File Offset: 0x00059CF0
	public int GoldCostToUpgrade
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0;
			}
			int num = this.RuneData.GoldCost;
			int num2 = this.ClampedUpgradeLevel;
			num2 = Mathf.Clamp(num2, 0, Economy_EV.RUNE_LEVEL_GOLD_MOD.Length - 1);
			num *= Economy_EV.RUNE_LEVEL_GOLD_MOD[num2];
			if (TraitManager.IsTraitActive(TraitType.LowerStorePrice))
			{
				num = (int)((float)num * 0.85f);
			}
			return num;
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06001C5B RID: 7259 RVA: 0x0005BB54 File Offset: 0x00059D54
	public int OreCostToUpgrade
	{
		get
		{
			if (this.RuneData.Disabled)
			{
				return 0;
			}
			int blackStoneCost = this.RuneData.BlackStoneCost;
			int num = this.ClampedUpgradeLevel;
			num = Mathf.Clamp(num, 0, Economy_EV.RUNE_LEVEL_ORE_MOD.Length - 1);
			return blackStoneCost * Economy_EV.RUNE_LEVEL_ORE_MOD[num];
		}
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06001C5C RID: 7260 RVA: 0x0005BB9C File Offset: 0x00059D9C
	public int MaxLevel
	{
		get
		{
			if (!this.RuneData)
			{
				return 0;
			}
			int num = this.RuneData.MaximumLevel;
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.MaxRuneDrops);
			if (!soulShopObj.IsNativeNull())
			{
				num += soulShopObj.CurrentEquippedLevel;
			}
			return num;
		}
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06001C5D RID: 7261 RVA: 0x0005BBE3 File Offset: 0x00059DE3
	public RuneData RuneData
	{
		get
		{
			return RuneLibrary.GetRuneData(this.RuneType);
		}
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x0005BBF0 File Offset: 0x00059DF0
	public RuneObj(RuneType runeType)
	{
		this.RuneType = runeType;
	}

	// Token: 0x0400199E RID: 6558
	public RuneType RuneType;

	// Token: 0x0400199F RID: 6559
	public int UpgradeLevel = -3;

	// Token: 0x040019A0 RID: 6560
	public int EquippedLevel;

	// Token: 0x040019A1 RID: 6561
	public int UpgradeBlueprintsFound;
}
