using System;
using UnityEngine;

// Token: 0x0200033E RID: 830
[Serializable]
public class BurdenObj
{
	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x0000DDDA File Offset: 0x0000BFDA
	// (set) Token: 0x06001AD5 RID: 6869 RVA: 0x0000DDFF File Offset: 0x0000BFFF
	public FoundState FoundState
	{
		get
		{
			if (this.BurdenData.Disabled)
			{
				return FoundState.NotFound;
			}
			if (this.m_foundState < FoundState.NotFound)
			{
				return FoundState.NotFound;
			}
			return this.m_foundState;
		}
		set
		{
			this.m_foundState = value;
		}
	}

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0000DE08 File Offset: 0x0000C008
	public int CurrentLevel
	{
		get
		{
			return Mathf.Clamp(this.m_currentLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x0000DE1C File Offset: 0x0000C01C
	public int MaxLevel
	{
		get
		{
			return this.BurdenData.MaxBurdenLevel;
		}
	}

	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x0000DE29 File Offset: 0x0000C029
	public int InitialBurdenWeight
	{
		get
		{
			return this.BurdenData.InitialBurdenCost;
		}
	}

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x0000DE36 File Offset: 0x0000C036
	public int CurrentBurdenWeight
	{
		get
		{
			return this.GetBurdenWeightAtLevel(this.CurrentLevel);
		}
	}

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0000DE44 File Offset: 0x0000C044
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.CurrentLevel);
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x0000DE52 File Offset: 0x0000C052
	public float GetStatGainAtLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		if (level <= 0)
		{
			return 0f;
		}
		return this.BurdenData.StatsGain * (float)level;
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x0000DE7B File Offset: 0x0000C07B
	public int GetBurdenWeightAtLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		if (level <= 0)
		{
			return 0;
		}
		return this.InitialBurdenWeight * level;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x0000DE9A File Offset: 0x0000C09A
	public void ForceLevel(int level)
	{
		this.m_currentLevel = level;
	}

	// Token: 0x17000CCE RID: 3278
	// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0000DEA3 File Offset: 0x0000C0A3
	public BurdenData BurdenData
	{
		get
		{
			return BurdenLibrary.GetBurdenData(this.BurdenType);
		}
	}

	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x06001ADF RID: 6879 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
	// (set) Token: 0x06001AE0 RID: 6880 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
	public BurdenType BurdenType { get; private set; }

	// Token: 0x06001AE1 RID: 6881 RVA: 0x0000DEC1 File Offset: 0x0000C0C1
	public void SetLevel(int value, bool additive, bool broadcast = true)
	{
		int currentLevel = this.m_currentLevel;
		if (additive)
		{
			this.m_currentLevel += value;
		}
		else
		{
			this.m_currentLevel = value;
		}
		this.m_currentLevel = Mathf.Clamp(this.m_currentLevel, 0, this.MaxLevel);
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x0000DEFE File Offset: 0x0000C0FE
	public BurdenObj(BurdenType burdenType)
	{
		this.BurdenType = burdenType;
	}

	// Token: 0x040018F7 RID: 6391
	private int m_currentLevel;

	// Token: 0x040018F8 RID: 6392
	private FoundState m_foundState = FoundState.FoundButNotViewed;
}
