using System;

// Token: 0x02000615 RID: 1557
public class ObjectiveCompleteHUDEventArgs : EventArgs
{
	// Token: 0x06002FD7 RID: 12247 RVA: 0x0001A2DC File Offset: 0x000184DC
	public ObjectiveCompleteHUDEventArgs(ObjectiveCompleteHUDType hudType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		this.Initialize(hudType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x0001A2F1 File Offset: 0x000184F1
	public void Initialize(ObjectiveCompleteHUDType hudType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		this.HUDType = hudType;
		this.DisplayDuration = displayDuration;
		this.TitleTextOverride = titleTextOverride;
		this.SubTitleTextOverride = subtitleTextOverride;
		this.DescriptionTextOverride = descriptionTextOverride;
	}

	// Token: 0x170012B7 RID: 4791
	// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x0001A318 File Offset: 0x00018518
	// (set) Token: 0x06002FDA RID: 12250 RVA: 0x0001A320 File Offset: 0x00018520
	public ObjectiveCompleteHUDType HUDType { get; private set; }

	// Token: 0x170012B8 RID: 4792
	// (get) Token: 0x06002FDB RID: 12251 RVA: 0x0001A329 File Offset: 0x00018529
	// (set) Token: 0x06002FDC RID: 12252 RVA: 0x0001A331 File Offset: 0x00018531
	public float DisplayDuration { get; private set; }

	// Token: 0x170012B9 RID: 4793
	// (get) Token: 0x06002FDD RID: 12253 RVA: 0x0001A33A File Offset: 0x0001853A
	// (set) Token: 0x06002FDE RID: 12254 RVA: 0x0001A342 File Offset: 0x00018542
	public string TitleTextOverride { get; private set; }

	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06002FDF RID: 12255 RVA: 0x0001A34B File Offset: 0x0001854B
	// (set) Token: 0x06002FE0 RID: 12256 RVA: 0x0001A353 File Offset: 0x00018553
	public string SubTitleTextOverride { get; private set; }

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x0001A35C File Offset: 0x0001855C
	// (set) Token: 0x06002FE2 RID: 12258 RVA: 0x0001A364 File Offset: 0x00018564
	public string DescriptionTextOverride { get; private set; }
}
