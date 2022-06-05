using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
[Serializable]
public class BurdenObj
{
	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x0600125E RID: 4702 RVA: 0x00035BA9 File Offset: 0x00033DA9
	// (set) Token: 0x0600125F RID: 4703 RVA: 0x00035BCE File Offset: 0x00033DCE
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

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06001260 RID: 4704 RVA: 0x00035BD7 File Offset: 0x00033DD7
	public int CurrentLevel
	{
		get
		{
			return Mathf.Clamp(this.m_currentLevel, 0, this.MaxLevel);
		}
	}

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x06001261 RID: 4705 RVA: 0x00035BEB File Offset: 0x00033DEB
	public int MaxLevel
	{
		get
		{
			return this.BurdenData.MaxBurdenLevel;
		}
	}

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06001262 RID: 4706 RVA: 0x00035BF8 File Offset: 0x00033DF8
	public int InitialBurdenWeight
	{
		get
		{
			return this.BurdenData.InitialBurdenCost;
		}
	}

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06001263 RID: 4707 RVA: 0x00035C05 File Offset: 0x00033E05
	public int CurrentBurdenWeight
	{
		get
		{
			return this.GetBurdenWeightAtLevel(this.CurrentLevel);
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x00035C13 File Offset: 0x00033E13
	public float CurrentStatGain
	{
		get
		{
			return this.GetStatGainAtLevel(this.CurrentLevel);
		}
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x00035C21 File Offset: 0x00033E21
	public float GetStatGainAtLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		if (level <= 0)
		{
			return 0f;
		}
		return this.BurdenData.StatsGain * (float)level;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00035C4A File Offset: 0x00033E4A
	public int GetBurdenWeightAtLevel(int level)
	{
		level = Mathf.Clamp(level, 0, this.MaxLevel);
		if (level <= 0)
		{
			return 0;
		}
		return this.InitialBurdenWeight * level;
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x00035C69 File Offset: 0x00033E69
	public void ForceLevel(int level)
	{
		this.m_currentLevel = level;
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06001268 RID: 4712 RVA: 0x00035C72 File Offset: 0x00033E72
	public BurdenData BurdenData
	{
		get
		{
			return BurdenLibrary.GetBurdenData(this.BurdenType);
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06001269 RID: 4713 RVA: 0x00035C7F File Offset: 0x00033E7F
	// (set) Token: 0x0600126A RID: 4714 RVA: 0x00035C87 File Offset: 0x00033E87
	public BurdenType BurdenType { get; private set; }

	// Token: 0x0600126B RID: 4715 RVA: 0x00035C90 File Offset: 0x00033E90
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

	// Token: 0x0600126C RID: 4716 RVA: 0x00035CCD File Offset: 0x00033ECD
	public BurdenObj(BurdenType burdenType)
	{
		this.BurdenType = burdenType;
	}

	// Token: 0x040012CD RID: 4813
	private int m_currentLevel;

	// Token: 0x040012CE RID: 4814
	private FoundState m_foundState = FoundState.FoundButNotViewed;
}
