using System;

// Token: 0x02000618 RID: 1560
public class HeirloomObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FED RID: 12269 RVA: 0x0001A40E File Offset: 0x0001860E
	public HeirloomObjectiveCompleteHUDEventArgs(HeirloomType heirloomType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.Heirloom, displayDuration, null, null, null)
	{
		this.Initialize(heirloomType, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x0001A428 File Offset: 0x00018628
	public void Initialize(HeirloomType heirloomType, float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.Heirloom, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.HeirloomType = heirloomType;
	}

	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06002FEF RID: 12271 RVA: 0x0001A43E File Offset: 0x0001863E
	// (set) Token: 0x06002FF0 RID: 12272 RVA: 0x0001A446 File Offset: 0x00018646
	public HeirloomType HeirloomType { get; private set; }
}
