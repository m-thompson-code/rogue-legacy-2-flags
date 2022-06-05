using System;

// Token: 0x02000C87 RID: 3207
public class SceneEventArgs : EventArgs
{
	// Token: 0x06005C28 RID: 23592 RVA: 0x000328E2 File Offset: 0x00030AE2
	public SceneEventArgs(string sceneName)
	{
		this.SceneName = sceneName;
	}

	// Token: 0x17001E8B RID: 7819
	// (get) Token: 0x06005C29 RID: 23593 RVA: 0x000328F1 File Offset: 0x00030AF1
	// (set) Token: 0x06005C2A RID: 23594 RVA: 0x000328F9 File Offset: 0x00030AF9
	public string SceneName { get; private set; }
}
