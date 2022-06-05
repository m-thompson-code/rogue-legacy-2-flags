using System;
using System.Collections.Generic;

// Token: 0x020001CD RID: 461
[Serializable]
public class ChallengeObj
{
	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06001296 RID: 4758 RVA: 0x00036B21 File Offset: 0x00034D21
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

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06001297 RID: 4759 RVA: 0x00036B46 File Offset: 0x00034D46
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

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06001298 RID: 4760 RVA: 0x00036B62 File Offset: 0x00034D62
	public int MaxEquippableLevel
	{
		get
		{
			return this.UpgradeBlueprintsFound;
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06001299 RID: 4761 RVA: 0x00036B6A File Offset: 0x00034D6A
	public bool HasMaxBlueprints
	{
		get
		{
			return this.UpgradeBlueprintsFound >= this.MaxLevel;
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x0600129A RID: 4762 RVA: 0x00036B7D File Offset: 0x00034D7D
	public ChallengeData ChallengeData
	{
		get
		{
			return ChallengeLibrary.GetChallengeData(this.ChallengeType);
		}
	}

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x0600129B RID: 4763 RVA: 0x00036B8A File Offset: 0x00034D8A
	public ChallengeScoringType ScoringType
	{
		get
		{
			return this.ChallengeData.ScoringType;
		}
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x00036B97 File Offset: 0x00034D97
	public ChallengeObj(ChallengeType challengeType)
	{
		this.ChallengeType = challengeType;
	}

	// Token: 0x040012F9 RID: 4857
	public ChallengeType ChallengeType;

	// Token: 0x040012FA RID: 4858
	public int TotalHighScore;

	// Token: 0x040012FB RID: 4859
	public Dictionary<ClassType, int> ClassHighScores = new Dictionary<ClassType, int>();

	// Token: 0x040012FC RID: 4860
	public float BestTime = float.MaxValue;

	// Token: 0x040012FD RID: 4861
	public float BestTimeWithoutHandicaps = float.MaxValue;

	// Token: 0x040012FE RID: 4862
	public int FoundLevel = -3;

	// Token: 0x040012FF RID: 4863
	public int EquippedLevel;

	// Token: 0x04001300 RID: 4864
	public int UpgradeBlueprintsFound;
}
