using System;

// Token: 0x02000619 RID: 1561
public class InsightObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FF1 RID: 12273 RVA: 0x0001A44F File Offset: 0x0001864F
	public InsightObjectiveCompleteHUDEventArgs(InsightType insightType, bool discovered, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Insight, displayDuration, null, null, null)
	{
		this.Initialize(insightType, discovered, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x0001A46B File Offset: 0x0001866B
	public void Initialize(InsightType insightType, bool discovered, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Insight, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.InsightType = insightType;
		this.Discovered = discovered;
	}

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x0001A489 File Offset: 0x00018689
	// (set) Token: 0x06002FF4 RID: 12276 RVA: 0x0001A491 File Offset: 0x00018691
	public InsightType InsightType { get; private set; }

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x0001A49A File Offset: 0x0001869A
	// (set) Token: 0x06002FF6 RID: 12278 RVA: 0x0001A4A2 File Offset: 0x000186A2
	public bool Discovered { get; private set; }
}
