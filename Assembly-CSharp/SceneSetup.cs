using System;
using UnityEngine;

// Token: 0x020009D3 RID: 2515
public abstract class SceneSetup : MonoBehaviour, ISceneSetup
{
	// Token: 0x17001A78 RID: 6776
	// (get) Token: 0x06004C90 RID: 19600
	// (set) Token: 0x06004C91 RID: 19601
	public abstract bool IsComplete { get; protected set; }

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06004C92 RID: 19602 RVA: 0x0012A240 File Offset: 0x00128440
	// (remove) Token: 0x06004C93 RID: 19603 RVA: 0x0012A278 File Offset: 0x00128478
	public event EventHandler<EventArgs> Complete;

	// Token: 0x06004C94 RID: 19604 RVA: 0x00029BED File Offset: 0x00027DED
	protected void OnComplete(EventArgs eventArgs)
	{
		if (this.Complete != null)
		{
			this.Complete(this, EventArgs.Empty);
		}
	}
}
