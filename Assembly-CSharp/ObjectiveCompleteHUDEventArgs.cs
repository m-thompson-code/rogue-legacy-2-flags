using System;

// Token: 0x02000388 RID: 904
public class ObjectiveCompleteHUDEventArgs : EventArgs
{
	// Token: 0x060021CB RID: 8651 RVA: 0x0006B809 File Offset: 0x00069A09
	public ObjectiveCompleteHUDEventArgs(ObjectiveCompleteHUDType hudType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		this.Initialize(hudType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x0006B81E File Offset: 0x00069A1E
	public void Initialize(ObjectiveCompleteHUDType hudType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		this.HUDType = hudType;
		this.DisplayDuration = displayDuration;
		this.TitleTextOverride = titleTextOverride;
		this.SubTitleTextOverride = subtitleTextOverride;
		this.DescriptionTextOverride = descriptionTextOverride;
	}

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x060021CD RID: 8653 RVA: 0x0006B845 File Offset: 0x00069A45
	// (set) Token: 0x060021CE RID: 8654 RVA: 0x0006B84D File Offset: 0x00069A4D
	public ObjectiveCompleteHUDType HUDType { get; private set; }

	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x060021CF RID: 8655 RVA: 0x0006B856 File Offset: 0x00069A56
	// (set) Token: 0x060021D0 RID: 8656 RVA: 0x0006B85E File Offset: 0x00069A5E
	public float DisplayDuration { get; private set; }

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x060021D1 RID: 8657 RVA: 0x0006B867 File Offset: 0x00069A67
	// (set) Token: 0x060021D2 RID: 8658 RVA: 0x0006B86F File Offset: 0x00069A6F
	public string TitleTextOverride { get; private set; }

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0006B878 File Offset: 0x00069A78
	// (set) Token: 0x060021D4 RID: 8660 RVA: 0x0006B880 File Offset: 0x00069A80
	public string SubTitleTextOverride { get; private set; }

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x060021D5 RID: 8661 RVA: 0x0006B889 File Offset: 0x00069A89
	// (set) Token: 0x060021D6 RID: 8662 RVA: 0x0006B891 File Offset: 0x00069A91
	public string DescriptionTextOverride { get; private set; }
}
