using System;
using UnityEngine;

// Token: 0x020004F7 RID: 1271
[Serializable]
public class SoulShopObj
{
	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06002902 RID: 10498 RVA: 0x0001726A File Offset: 0x0001546A
	public int CurrentOwnedLevel
	{
		get
		{
			return this.m_ownedLevel;
		}
	}

	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06002903 RID: 10499 RVA: 0x00017272 File Offset: 0x00015472
	public int CurrentEquippedLevel
	{
		get
		{
			return this.m_equippedLevel;
		}
	}

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06002904 RID: 10500 RVA: 0x000BF578 File Offset: 0x000BD778
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

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06002905 RID: 10501 RVA: 0x0001727A File Offset: 0x0001547A
	public int InitialCost
	{
		get
		{
			return this.SoulShopData.BaseCost;
		}
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x00017287 File Offset: 0x00015487
	public int CurrentCost(bool applyMaxLevelScalingCap)
	{
		return this.GetCostAtLevel(this.CurrentOwnedLevel + 1, applyMaxLevelScalingCap);
	}

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06002907 RID: 10503 RVA: 0x00017298 File Offset: 0x00015498
	// (set) Token: 0x06002908 RID: 10504 RVA: 0x000172A0 File Offset: 0x000154A0
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

	// Token: 0x06002909 RID: 10505 RVA: 0x000BF5C8 File Offset: 0x000BD7C8
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

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x0600290A RID: 10506 RVA: 0x000172A9 File Offset: 0x000154A9
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.CurrentEquippedLevel);
		}
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000BF614 File Offset: 0x000BD814
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

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x0600290C RID: 10508 RVA: 0x000172B7 File Offset: 0x000154B7
	public SoulShopData SoulShopData
	{
		get
		{
			return SoulShopLibrary.GetSoulShopData(this.SoulShopType);
		}
	}

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x0600290D RID: 10509 RVA: 0x000172C4 File Offset: 0x000154C4
	// (set) Token: 0x0600290E RID: 10510 RVA: 0x000172CC File Offset: 0x000154CC
	public SoulShopType SoulShopType { get; private set; }

	// Token: 0x0600290F RID: 10511 RVA: 0x000172D5 File Offset: 0x000154D5
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

	// Token: 0x06002910 RID: 10512 RVA: 0x000BF654 File Offset: 0x000BD854
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

	// Token: 0x06002911 RID: 10513 RVA: 0x00017312 File Offset: 0x00015512
	public SoulShopObj(SoulShopType soulShopType)
	{
		this.SoulShopType = soulShopType;
	}

	// Token: 0x040023C5 RID: 9157
	private int m_equippedLevel;

	// Token: 0x040023C6 RID: 9158
	private int m_ownedLevel;

	// Token: 0x040023C7 RID: 9159
	private bool m_wasViewed;
}
