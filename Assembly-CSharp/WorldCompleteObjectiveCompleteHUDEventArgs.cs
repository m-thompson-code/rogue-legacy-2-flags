using System;

// Token: 0x0200061B RID: 1563
public class WorldCompleteObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FFB RID: 12283 RVA: 0x0001A4EC File Offset: 0x000186EC
	public WorldCompleteObjectiveCompleteHUDEventArgs(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.WorldComplete, displayDuration, null, null, null)
	{
		this.Initialize(displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x0001A504 File Offset: 0x00018704
	public void Initialize(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.WorldComplete, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}
}
