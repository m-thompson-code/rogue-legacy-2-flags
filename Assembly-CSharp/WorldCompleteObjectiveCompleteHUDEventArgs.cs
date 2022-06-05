using System;

// Token: 0x0200038E RID: 910
public class WorldCompleteObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021EF RID: 8687 RVA: 0x0006BA19 File Offset: 0x00069C19
	public WorldCompleteObjectiveCompleteHUDEventArgs(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null) : base(ObjectiveCompleteHUDType.WorldComplete, displayDuration, null, null, null)
	{
		this.Initialize(displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x0006BA31 File Offset: 0x00069C31
	public void Initialize(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null)
	{
		base.Initialize(ObjectiveCompleteHUDType.WorldComplete, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
	}
}
