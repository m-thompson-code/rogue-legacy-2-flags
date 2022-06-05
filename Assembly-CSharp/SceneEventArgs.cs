using System;

// Token: 0x020007C1 RID: 1985
public class SceneEventArgs : EventArgs
{
	// Token: 0x0600429F RID: 17055 RVA: 0x000EBECC File Offset: 0x000EA0CC
	public SceneEventArgs(string sceneName)
	{
		this.SceneName = sceneName;
	}

	// Token: 0x1700168D RID: 5773
	// (get) Token: 0x060042A0 RID: 17056 RVA: 0x000EBEDB File Offset: 0x000EA0DB
	// (set) Token: 0x060042A1 RID: 17057 RVA: 0x000EBEE3 File Offset: 0x000EA0E3
	public string SceneName { get; private set; }
}
