using System;

// Token: 0x0200038B RID: 907
public class HeirloomObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021E1 RID: 8673 RVA: 0x0006B93B File Offset: 0x00069B3B
	public HeirloomObjectiveCompleteHUDEventArgs(HeirloomType heirloomType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Heirloom, displayDuration, null, null, null)
	{
		this.Initialize(heirloomType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x0006B955 File Offset: 0x00069B55
	public void Initialize(HeirloomType heirloomType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Heirloom, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.HeirloomType = heirloomType;
	}

	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x060021E3 RID: 8675 RVA: 0x0006B96B File Offset: 0x00069B6B
	// (set) Token: 0x060021E4 RID: 8676 RVA: 0x0006B973 File Offset: 0x00069B73
	public HeirloomType HeirloomType { get; private set; }
}
