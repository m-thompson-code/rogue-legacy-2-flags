using System;
using UnityEngine;

// Token: 0x02000BA0 RID: 2976
[Serializable]
public class SkillTreeObj
{
	// Token: 0x17001DE4 RID: 7652
	// (get) Token: 0x06005977 RID: 22903 RVA: 0x00030BC8 File Offset: 0x0002EDC8
	public int ClampedLevel
	{
		get
		{
			return Mathf.Clamp(this.Level, 0, this.MaxLevel);
		}
	}

	// Token: 0x17001DE5 RID: 7653
	// (get) Token: 0x06005978 RID: 22904 RVA: 0x00030BDC File Offset: 0x0002EDDC
	public float FirstLevelStatGain
	{
		get
		{
			return this.SkillTreeData.FirstLevelStatGain;
		}
	}

	// Token: 0x17001DE6 RID: 7654
	// (get) Token: 0x06005979 RID: 22905 RVA: 0x00030BE9 File Offset: 0x0002EDE9
	public float AdditionalLevelStatGain
	{
		get
		{
			return this.SkillTreeData.AdditionalLevelStatGain;
		}
	}

	// Token: 0x0600597A RID: 22906 RVA: 0x00030BF6 File Offset: 0x0002EDF6
	public float GetStatGainAtLevel(int level)
	{
		if (level <= 0)
		{
			return 0f;
		}
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		return this.FirstLevelStatGain + this.AdditionalLevelStatGain * (float)(level - 1);
	}

	// Token: 0x17001DE7 RID: 7655
	// (get) Token: 0x0600597B RID: 22907 RVA: 0x00030C23 File Offset: 0x0002EE23
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.Level);
		}
	}

	// Token: 0x17001DE8 RID: 7656
	// (get) Token: 0x0600597C RID: 22908 RVA: 0x00030C31 File Offset: 0x0002EE31
	public int GoldCost
	{
		get
		{
			return this.SkillTreeData.BaseCost + this.SkillTreeData.Appreciation * this.Level;
		}
	}

	// Token: 0x17001DE9 RID: 7657
	// (get) Token: 0x0600597D RID: 22909 RVA: 0x00153498 File Offset: 0x00151698
	public int GoldCostWithLevelAppreciation
	{
		get
		{
			int num = Mathf.Clamp(SkillTreeManager.GetTotalSkillObjLevel() - 20, 0, int.MaxValue);
			return (int)((double)this.GoldCost + Math.Floor((double)((float)num * 14f / 5f)) * 5.0);
		}
	}

	// Token: 0x0600597E RID: 22910 RVA: 0x001534E0 File Offset: 0x001516E0
	public int GetGoldCostWithAppreciationWhenAddingLevels(int levelsToAdd)
	{
		int level = this.Level;
		int num = 0;
		for (int i = 0; i < levelsToAdd; i++)
		{
			num += this.GoldCostWithLevelAppreciation;
			this.Level++;
			if (this.Level >= this.MaxLevel)
			{
				break;
			}
		}
		this.Level = level;
		return num;
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x00153530 File Offset: 0x00151730
	public int GetNumLevelsPurchaseableWithGold(int levelsToAdd, int goldAmount)
	{
		int level = this.Level;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < levelsToAdd; i++)
		{
			num += this.GoldCostWithLevelAppreciation;
			if (num <= goldAmount)
			{
				num2++;
			}
			this.Level++;
			if (this.Level >= this.MaxLevel)
			{
				break;
			}
		}
		this.Level = level;
		return num2;
	}

	// Token: 0x17001DEA RID: 7658
	// (get) Token: 0x06005980 RID: 22912 RVA: 0x00030C51 File Offset: 0x0002EE51
	public bool IsLevelLocked
	{
		get
		{
			return SkillTreeManager.IsInitialized && this.UnlockLevel > SkillTreeManager.GetTotalSkillObjLevel();
		}
	}

	// Token: 0x17001DEB RID: 7659
	// (get) Token: 0x06005981 RID: 22913 RVA: 0x00030C69 File Offset: 0x0002EE69
	public int UnlockLevel
	{
		get
		{
			return this.SkillTreeData.SkillUnlockLevel;
		}
	}

	// Token: 0x17001DEC RID: 7660
	// (get) Token: 0x06005982 RID: 22914 RVA: 0x00030C76 File Offset: 0x0002EE76
	public bool IsLocked
	{
		get
		{
			return this.SkillTreeData.SkillUnlockState == SkillUnlockState.Locked;
		}
	}

	// Token: 0x17001DED RID: 7661
	// (get) Token: 0x06005983 RID: 22915 RVA: 0x00030C86 File Offset: 0x0002EE86
	public bool IsSoulLocked
	{
		get
		{
			return this.MaxLevel <= 0;
		}
	}

	// Token: 0x17001DEE RID: 7662
	// (get) Token: 0x06005984 RID: 22916 RVA: 0x0015358C File Offset: 0x0015178C
	public int MaxLevel
	{
		get
		{
			int num = 0;
			SoulShopType soulShopType;
			if (!string.IsNullOrEmpty(this.SkillTreeData.SoulShopTag) && Enum.TryParse<SoulShopType>(this.SkillTreeData.SoulShopTag, out soulShopType) && Enum.IsDefined(typeof(SoulShopType), soulShopType))
			{
				SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(soulShopType);
				num = soulShopObj.CurrentEquippedLevel * this.SkillTreeData.AdditiveSoulShopLevels;
			}
			int num2 = this.SkillTreeData.MaxLevel + num;
			if (this.SkillTreeData.OverloadLevelCap > 0)
			{
				return Mathf.Min(this.SkillTreeData.OverloadLevelCap, num2);
			}
			return num2;
		}
	}

	// Token: 0x17001DEF RID: 7663
	// (get) Token: 0x06005985 RID: 22917 RVA: 0x00030C94 File Offset: 0x0002EE94
	public SkillTreeData SkillTreeData
	{
		get
		{
			return SkillTreeLibrary.GetSkillTreeData(this.SkillTreeType);
		}
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x00030CA1 File Offset: 0x0002EEA1
	public SkillTreeObj(SkillTreeType skillTreeType)
	{
		this.SkillTreeType = skillTreeType;
	}

	// Token: 0x040043E5 RID: 17381
	public SkillTreeType SkillTreeType;

	// Token: 0x040043E6 RID: 17382
	public int Level;
}
