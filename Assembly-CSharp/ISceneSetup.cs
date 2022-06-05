using System;

// Token: 0x020005CA RID: 1482
public interface ISceneSetup
{
	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x0600367B RID: 13947
	bool IsComplete { get; }

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600367C RID: 13948
	// (remove) Token: 0x0600367D RID: 13949
	event EventHandler<EventArgs> Complete;
}
