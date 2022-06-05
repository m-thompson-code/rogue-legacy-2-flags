using System;

// Token: 0x0200061A RID: 1562
public class ScarObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FF7 RID: 12279 RVA: 0x0001A4AB File Offset: 0x000186AB
	public ScarObjectiveCompleteHUDEventArgs(ChallengeType challengeType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Scar, displayDuration, null, null, null)
	{
		this.Initialize(challengeType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x0001A4C5 File Offset: 0x000186C5
	public void Initialize(ChallengeType challengeType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Scar, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.ChallengeType = challengeType;
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x0001A4DB File Offset: 0x000186DB
	// (set) Token: 0x06002FFA RID: 12282 RVA: 0x0001A4E3 File Offset: 0x000186E3
	public ChallengeType ChallengeType { get; private set; }
}
