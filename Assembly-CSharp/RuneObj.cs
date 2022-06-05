using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
[Serializable]
public class RuneObj
{
	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06002719 RID: 10009 RVA: 0x00015FE4 File Offset: 0x000141E4
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

	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x0600271A RID: 10010 RVA: 0x00016014 File Offset: 0x00014214
	public int ClampedUpgradeLevel
	{
		get
		{
			return Mathf.Clamp(this.UpgradeLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x0600271B RID: 10011 RVA: 0x00016028 File Offset: 0x00014228
	public bool IsMaxEquippedLevel
	{
		get
		{
			return this.EquippedLevel == this.MaxLevel;
		}
	}

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x0600271C RID: 10012 RVA: 0x00016038 File Offset: 0x00014238
	public bool IsMaxUpgradeLevel
	{
		get
		{
			return this.ClampedUpgradeLevel == this.MaxLevel;
		}
	}

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x0600271D RID: 10013 RVA: 0x00016048 File Offset: 0x00014248
	public bool HasMaxBlueprints
	{
		get
		{
			return this.UpgradeBlueprintsFound >= this.MaxLevel;
		}
	}

	// Token: 0x0600271E RID: 10014 RVA: 0x000B7F70 File Offset: 0x000B6170
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

	// Token: 0x0600271F RID: 10015 RVA: 0x000B7FC8 File Offset: 0x000B61C8
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

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06002720 RID: 10016 RVA: 0x000B8014 File Offset: 0x000B6214
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

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x06002721 RID: 10017 RVA: 0x000B8064 File Offset: 0x000B6264
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

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x06002722 RID: 10018 RVA: 0x000B80B4 File Offset: 0x000B62B4
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

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x06002723 RID: 10019 RVA: 0x0001605B File Offset: 0x0001425B
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

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06002724 RID: 10020 RVA: 0x000B8104 File Offset: 0x000B6304
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

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06002725 RID: 10021 RVA: 0x000B8168 File Offset: 0x000B6368
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

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06002726 RID: 10022 RVA: 0x000B81B0 File Offset: 0x000B63B0
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

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x06002727 RID: 10023 RVA: 0x00016097 File Offset: 0x00014297
	public RuneData RuneData
	{
		get
		{
			return RuneLibrary.GetRuneData(this.RuneType);
		}
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x000160A4 File Offset: 0x000142A4
	public RuneObj(RuneType runeType)
	{
		this.RuneType = runeType;
	}

	// Token: 0x040021A5 RID: 8613
	public RuneType RuneType;

	// Token: 0x040021A6 RID: 8614
	public int UpgradeLevel = -3;

	// Token: 0x040021A7 RID: 8615
	public int EquippedLevel;

	// Token: 0x040021A8 RID: 8616
	public int UpgradeBlueprintsFound;
}
