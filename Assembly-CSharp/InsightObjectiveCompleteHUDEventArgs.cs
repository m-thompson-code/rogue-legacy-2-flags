using System;

// Token: 0x0200038C RID: 908
public class InsightObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021E5 RID: 8677 RVA: 0x0006B97C File Offset: 0x00069B7C
	public InsightObjectiveCompleteHUDEventArgs(InsightType insightType, bool discovered, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Insight, displayDuration, null, null, null)
	{
		this.Initialize(insightType, discovered, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x0006B998 File Offset: 0x00069B98
	public void Initialize(InsightType insightType, bool discovered, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Insight, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.InsightType = insightType;
		this.Discovered = discovered;
	}

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0006B9B6 File Offset: 0x00069BB6
	// (set) Token: 0x060021E8 RID: 8680 RVA: 0x0006B9BE File Offset: 0x00069BBE
	public InsightType InsightType { get; private set; }

	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x060021E9 RID: 8681 RVA: 0x0006B9C7 File Offset: 0x00069BC7
	// (set) Token: 0x060021EA RID: 8682 RVA: 0x0006B9CF File Offset: 0x00069BCF
	public bool Discovered { get; private set; }
}
