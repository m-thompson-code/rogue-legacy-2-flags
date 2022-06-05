using System;
using UnityEngine;

// Token: 0x020002ED RID: 749
[Serializable]
public class SoulShopObj
{
	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x06001DBD RID: 7613 RVA: 0x00061DA3 File Offset: 0x0005FFA3
	public int CurrentOwnedLevel
	{
		get
		{
			return this.m_ownedLevel;
		}
	}

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x06001DBE RID: 7614 RVA: 0x00061DAB File Offset: 0x0005FFAB
	public int CurrentEquippedLevel
	{
		get
		{
			return this.m_equippedLevel;
		}
	}

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x06001DBF RID: 7615 RVA: 0x00061DB4 File Offset: 0x0005FFB4
	public int MaxLevel
	{
		get
		{
			int overloadMaxLevel = this.SoulShopData.OverloadMaxLevel;
			if (overloadMaxLevel <= 0)
			{
				return this.SoulShopData.MaxLevel;
			}
			if (SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.UnlockOverload).CurrentEquippedLevel > 0)
			{
				return overloadMaxLevel;
			}
			return this.SoulShopData.MaxLevel;
		}
	}

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x00061E01 File Offset: 0x00060001
	public int InitialCost
	{
		get
		{
			return this.SoulShopData.BaseCost;
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x00061E0E File Offset: 0x0006000E
	public int CurrentCost(bool applyMaxLevelScalingCap)
	{
		return this.GetCostAtLevel(this.CurrentOwnedLevel + 1, applyMaxLevelScalingCap);
	}

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x00061E1F File Offset: 0x0006001F
	// (set) Token: 0x06001DC3 RID: 7619 RVA: 0x00061E27 File Offset: 0x00060027
	public bool WasViewed
	{
		get
		{
			return this.m_wasViewed;
		}
		set
		{
			this.m_wasViewed = value;
		}
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00061E30 File Offset: 0x00060030
	public int GetCostAtLevel(int level, bool applyMaxLevelScalingCap)
	{
		if (!applyMaxLevelScalingCap)
		{
			level = Mathf.Clamp(level, 0, int.MaxValue);
		}
		else
		{
			level = Mathf.Clamp(level, 0, this.SoulShopData.MaxLevelScalingCap + 1);
		}
		return this.InitialCost + (level - 1) * this.SoulShopData.ScalingCost;
	}

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x00061E7C File Offset: 0x0006007C
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.CurrentEquippedLevel);
		}
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x00061E8C File Offset: 0x0006008C
	public float GetStatGainAtLevel(int level)
	{
		level = Mathf.Clamp(level, 0, int.MaxValue);
		if (level <= 0)
		{
			return 0f;
		}
		SoulShopData soulShopData = this.SoulShopData;
		return soulShopData.FirstLevelStatGain + (float)(level - 1) * soulShopData.AdditionalLevelStatGain;
	}

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x00061ECA File Offset: 0x000600CA
	public SoulShopData SoulShopData
	{
		get
		{
			return SoulShopLibrary.GetSoulShopData(this.SoulShopType);
		}
	}

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x00061ED7 File Offset: 0x000600D7
	// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x00061EDF File Offset: 0x000600DF
	public SoulShopType SoulShopType { get; private set; }

	// Token: 0x06001DCA RID: 7626 RVA: 0x00061EE8 File Offset: 0x000600E8
	public void SetEquippedLevel(int value, bool additive, bool broadcast = true)
	{
		int equippedLevel = this.m_equippedLevel;
		if (additive)
		{
			this.m_equippedLevel += value;
		}
		else
		{
			this.m_equippedLevel = value;
		}
		this.m_equippedLevel = Mathf.Clamp(this.m_equippedLevel, 0, this.CurrentOwnedLevel);
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x00061F28 File Offset: 0x00060128
	public void SetOwnedLevel(int value, bool additive, bool clampToMaxLevel, bool broadcast = true)
	{
		int ownedLevel = this.m_ownedLevel;
		if (additive)
		{
			this.m_ownedLevel += value;
		}
		else
		{
			this.m_ownedLevel = value;
		}
		if (clampToMaxLevel)
		{
			this.m_ownedLevel = Mathf.Clamp(this.m_ownedLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x00061F74 File Offset: 0x00060174
	public SoulShopObj(SoulShopType soulShopType)
	{
		this.SoulShopType = soulShopType;
	}

	// Token: 0x04001B7A RID: 7034
	private int m_equippedLevel;

	// Token: 0x04001B7B RID: 7035
	private int m_ownedLevel;

	// Token: 0x04001B7C RID: 7036
	private bool m_wasViewed;
}
