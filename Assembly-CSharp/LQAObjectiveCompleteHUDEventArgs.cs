using System;

// Token: 0x02000616 RID: 1558
public class LQAObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x06002FE3 RID: 12259 RVA: 0x0001A36D File Offset: 0x0001856D
	public LQAObjectiveCompleteHUDEventArgs(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null, bool displayPlayer = false) : base(ObjectiveCompleteHUDType.LQA, 5f, null, null, null)
	{
		this.Initialize(displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride, displayPlayer);
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x0001A38B File Offset: 0x0001858B
	public void Initialize(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null, bool displayPlayer = false)
	{
		base.Initialize(ObjectiveCompleteHUDType.LQA, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.DisplayPlayer = displayPlayer;
	}

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x0001A3A1 File Offset: 0x000185A1
	// (set) Token: 0x06002FE6 RID: 12262 RVA: 0x0001A3A9 File Offset: 0x000185A9
	public bool DisplayPlayer { get; private set; }
}
