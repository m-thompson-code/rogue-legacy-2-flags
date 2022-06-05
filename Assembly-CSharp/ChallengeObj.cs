using System;
using System.Collections.Generic;

// Token: 0x02000347 RID: 839
[Serializable]
public class ChallengeObj
{
	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x06001B14 RID: 6932 RVA: 0x0000E0A6 File Offset: 0x0000C2A6
	public FoundState FoundState
	{
		get
		{
			if (this.ChallengeData.Disabled)
			{
				return FoundState.NotFound;
			}
			if (this.FoundLevel < -3)
			{
				return FoundState.NotFound;
			}
			return (FoundState)this.FoundLevel;
		}
	}

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0000E0CB File Offset: 0x0000C2CB
	public int MaxLevel
	{
		get
		{
			if (!this.ChallengeData)
			{
				return 0;
			}
			return this.ChallengeData.MaxHandicap;
		}
	}

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0000E0E7 File Offset: 0x0000C2E7
	public int MaxEquippableLevel
	{
		get
		{
			return this.UpgradeBlueprintsFound;
		}
	}

	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0000E0EF File Offset: 0x0000C2EF
	public bool HasMaxBlueprints
	{
		get
		{
			return this.UpgradeBlueprintsFound >= this.MaxLevel;
		}
	}

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0000E102 File Offset: 0x0000C302
	public ChallengeData ChallengeData
	{
		get
		{
			return ChallengeLibrary.GetChallengeData(this.ChallengeType);
		}
	}

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0000E10F File Offset: 0x0000C30F
	public ChallengeScoringType ScoringType
	{
		get
		{
			return this.ChallengeData.ScoringType;
		}
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x0000E11C File Offset: 0x0000C31C
	public ChallengeObj(ChallengeType challengeType)
	{
		this.ChallengeType = challengeType;
	}

	// Token: 0x0400192C RID: 6444
	public ChallengeType ChallengeType;

	// Token: 0x0400192D RID: 6445
	public int TotalHighScore;

	// Token: 0x0400192E RID: 6446
	public Dictionary<ClassType, int> ClassHighScores = new Dictionary<ClassType, int>();

	// Token: 0x0400192F RID: 6447
	public float BestTime = float.MaxValue;

	// Token: 0x04001930 RID: 6448
	public float BestTimeWithoutHandicaps = float.MaxValue;

	// Token: 0x04001931 RID: 6449
	public int FoundLevel = -3;

	// Token: 0x04001932 RID: 6450
	public int EquippedLevel;

	// Token: 0x04001933 RID: 6451
	public int UpgradeBlueprintsFound;
}
