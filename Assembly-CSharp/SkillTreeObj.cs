using System;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[Serializable]
public class SkillTreeObj
{
	// Token: 0x170015EC RID: 5612
	// (get) Token: 0x0600403A RID: 16442 RVA: 0x000E3D5D File Offset: 0x000E1F5D
	public int ClampedLevel
	{
		get
		{
			return Mathf.Clamp(this.Level, 0, this.MaxLevel);
		}
	}

	// Token: 0x170015ED RID: 5613
	// (get) Token: 0x0600403B RID: 16443 RVA: 0x000E3D71 File Offset: 0x000E1F71
	public float FirstLevelStatGain
	{
		get
		{
			return this.SkillTreeData.FirstLevelStatGain;
		}
	}

	// Token: 0x170015EE RID: 5614
	// (get) Token: 0x0600403C RID: 16444 RVA: 0x000E3D7E File Offset: 0x000E1F7E
	public float AdditionalLevelStatGain
	{
		get
		{
			return this.SkillTreeData.AdditionalLevelStatGain;
		}
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x000E3D8B File Offset: 0x000E1F8B
	public float GetStatGainAtLevel(int level)
	{
		if (level <= 0)
		{
			return 0f;
		}
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		return this.FirstLevelStatGain + this.AdditionalLevelStatGain * (float)(level - 1);
	}

	// Token: 0x170015EF RID: 5615
	// (get) Token: 0x0600403E RID: 16446 RVA: 0x000E3DB8 File Offset: 0x000E1FB8
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.Level);
		}
	}

	// Token: 0x170015F0 RID: 5616
	// (get) Token: 0x0600403F RID: 16447 RVA: 0x000E3DC6 File Offset: 0x000E1FC6
	public int GoldCost
	{
		get
		{
			return this.SkillTreeData.BaseCost + this.SkillTreeData.Appreciation * this.Level;
		}
	}

	// Token: 0x170015F1 RID: 5617
	// (get) Token: 0x06004040 RID: 16448 RVA: 0x000E3DE8 File Offset: 0x000E1FE8
	public int GoldCostWithLevelAppreciation
	{
		get
		{
			int num = Mathf.Clamp(SkillTreeManager.GetTotalSkillObjLevel() - 20, 0, int.MaxValue);
			return (int)((double)this.GoldCost + Math.Floor((double)((float)num * 14f / 5f)) * 5.0);
		}
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x000E3E30 File Offset: 0x000E2030
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

	// Token: 0x06004042 RID: 16450 RVA: 0x000E3E80 File Offset: 0x000E2080
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

	// Token: 0x170015F2 RID: 5618
	// (get) Token: 0x06004043 RID: 16451 RVA: 0x000E3ED9 File Offset: 0x000E20D9
	public bool IsLevelLocked
	{
		get
		{
			return SkillTreeManager.IsInitialized && this.UnlockLevel > SkillTreeManager.GetTotalSkillObjLevel();
		}
	}

	// Token: 0x170015F3 RID: 5619
	// (get) Token: 0x06004044 RID: 16452 RVA: 0x000E3EF1 File Offset: 0x000E20F1
	public int UnlockLevel
	{
		get
		{
			return this.SkillTreeData.SkillUnlockLevel;
		}
	}

	// Token: 0x170015F4 RID: 5620
	// (get) Token: 0x06004045 RID: 16453 RVA: 0x000E3EFE File Offset: 0x000E20FE
	public bool IsLocked
	{
		get
		{
			return this.SkillTreeData.SkillUnlockState == SkillUnlockState.Locked;
		}
	}

	// Token: 0x170015F5 RID: 5621
	// (get) Token: 0x06004046 RID: 16454 RVA: 0x000E3F0E File Offset: 0x000E210E
	public bool IsSoulLocked
	{
		get
		{
			return this.MaxLevel <= 0;
		}
	}

	// Token: 0x170015F6 RID: 5622
	// (get) Token: 0x06004047 RID: 16455 RVA: 0x000E3F1C File Offset: 0x000E211C
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

	// Token: 0x170015F7 RID: 5623
	// (get) Token: 0x06004048 RID: 16456 RVA: 0x000E3FB6 File Offset: 0x000E21B6
	public SkillTreeData SkillTreeData
	{
		get
		{
			return SkillTreeLibrary.GetSkillTreeData(this.SkillTreeType);
		}
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x000E3FC3 File Offset: 0x000E21C3
	public SkillTreeObj(SkillTreeType skillTreeType)
	{
		this.SkillTreeType = skillTreeType;
	}

	// Token: 0x0400318A RID: 12682
	public SkillTreeType SkillTreeType;

	// Token: 0x0400318B RID: 12683
	public int Level;
}
