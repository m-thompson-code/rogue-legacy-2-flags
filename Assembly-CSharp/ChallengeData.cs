using System;
using UnityEngine;

// Token: 0x02000B72 RID: 2930
public class ChallengeData : ScriptableObject
{
	// Token: 0x040041BC RID: 16828
	public ChallengeType ChallengeType;

	// Token: 0x040041BD RID: 16829
	public ChallengeScoringType ScoringType;

	// Token: 0x040041BE RID: 16830
	public ClassType ClassOverride;

	// Token: 0x040041BF RID: 16831
	public float ParTime;

	// Token: 0x040041C0 RID: 16832
	public int GoldReq;

	// Token: 0x040041C1 RID: 16833
	public int SilverReq;

	// Token: 0x040041C2 RID: 16834
	public int BronzeReq;

	// Token: 0x040041C3 RID: 16835
	public int Reward;

	// Token: 0x040041C4 RID: 16836
	public int BaseHandicap;

	// Token: 0x040041C5 RID: 16837
	public float ScalingHandicap;

	// Token: 0x040041C6 RID: 16838
	public int MaxHandicap;

	// Token: 0x040041C7 RID: 16839
	public bool Disabled;

	// Token: 0x040041C8 RID: 16840
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040041C9 RID: 16841
	public string Description;

	// Token: 0x040041CA RID: 16842
	public string Controls;

	// Token: 0x040041CB RID: 16843
	public string BossRequirementHint;

	// Token: 0x040041CC RID: 16844
	public string Hint;
}
