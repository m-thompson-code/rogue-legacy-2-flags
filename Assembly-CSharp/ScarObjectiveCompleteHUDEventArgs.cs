using System;

// Token: 0x0200038D RID: 909
public class ScarObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021EB RID: 8683 RVA: 0x0006B9D8 File Offset: 0x00069BD8
	public ScarObjectiveCompleteHUDEventArgs(ChallengeType challengeType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Scar, displayDuration, null, null, null)
	{
		this.Initialize(challengeType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x0006B9F2 File Offset: 0x00069BF2
	public void Initialize(ChallengeType challengeType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Scar, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.ChallengeType = challengeType;
	}

	// Token: 0x17000E33 RID: 3635
	// (get) Token: 0x060021ED RID: 8685 RVA: 0x0006BA08 File Offset: 0x00069C08
	// (set) Token: 0x060021EE RID: 8686 RVA: 0x0006BA10 File Offset: 0x00069C10
	public ChallengeType ChallengeType { get; private set; }
}
