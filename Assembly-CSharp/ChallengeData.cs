using System;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
public class ChallengeData : ScriptableObject
{
	// Token: 0x04002F6D RID: 12141
	public ChallengeType ChallengeType;

	// Token: 0x04002F6E RID: 12142
	public ChallengeScoringType ScoringType;

	// Token: 0x04002F6F RID: 12143
	public ClassType ClassOverride;

	// Token: 0x04002F70 RID: 12144
	public float ParTime;

	// Token: 0x04002F71 RID: 12145
	public int GoldReq;

	// Token: 0x04002F72 RID: 12146
	public int SilverReq;

	// Token: 0x04002F73 RID: 12147
	public int BronzeReq;

	// Token: 0x04002F74 RID: 12148
	public int Reward;

	// Token: 0x04002F75 RID: 12149
	public int BaseHandicap;

	// Token: 0x04002F76 RID: 12150
	public float ScalingHandicap;

	// Token: 0x04002F77 RID: 12151
	public int MaxHandicap;

	// Token: 0x04002F78 RID: 12152
	public bool Disabled;

	// Token: 0x04002F79 RID: 12153
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002F7A RID: 12154
	public string Description;

	// Token: 0x04002F7B RID: 12155
	public string Controls;

	// Token: 0x04002F7C RID: 12156
	public string BossRequirementHint;

	// Token: 0x04002F7D RID: 12157
	public string Hint;
}
