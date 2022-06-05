using System;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public abstract class SceneSetup : MonoBehaviour, ISceneSetup
{
	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x0600367E RID: 13950
	// (set) Token: 0x0600367F RID: 13951
	public abstract bool IsComplete { get; protected set; }

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06003680 RID: 13952 RVA: 0x000BC310 File Offset: 0x000BA510
	// (remove) Token: 0x06003681 RID: 13953 RVA: 0x000BC348 File Offset: 0x000BA548
	public event EventHandler<EventArgs> Complete;

	// Token: 0x06003682 RID: 13954 RVA: 0x000BC37D File Offset: 0x000BA57D
	protected void OnComplete(EventArgs eventArgs)
	{
		if (this.Complete != null)
		{
			this.Complete(this, EventArgs.Empty);
		}
	}
}
