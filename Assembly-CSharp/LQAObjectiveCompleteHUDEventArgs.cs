using System;

// Token: 0x02000389 RID: 905
public class LQAObjectiveCompleteHUDEventArgs : ObjectiveCompleteHUDEventArgs
{
	// Token: 0x060021D7 RID: 8663 RVA: 0x0006B89A File Offset: 0x00069A9A
	public LQAObjectiveCompleteHUDEventArgs(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null, bool displayPlayer = false) : base(ObjectiveCompleteHUDType.LQA, 5f, null, null, null)
	{
		this.Initialize(displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride, displayPlayer);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x0006B8B8 File Offset: 0x00069AB8
	public void Initialize(float displayDuration = 5f, string titleTextOverride = null, string subtitleTextOverride = null, string descriptionTextOverride = null, bool displayPlayer = false)
	{
		base.Initialize(ObjectiveCompleteHUDType.LQA, displayDuration, titleTextOverride, subtitleTextOverride, descriptionTextOverride);
		this.DisplayPlayer = displayPlayer;
	}

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x060021D9 RID: 8665 RVA: 0x0006B8CE File Offset: 0x00069ACE
	// (set) Token: 0x060021DA RID: 8666 RVA: 0x0006B8D6 File Offset: 0x00069AD6
	public bool DisplayPlayer { get; private set; }
}
