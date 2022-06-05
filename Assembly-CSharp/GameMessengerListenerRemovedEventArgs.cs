using System;
using UnityEngine;

// Token: 0x02000C74 RID: 3188
public class GameMessengerListenerRemovedEventArgs : EventArgs
{
	// Token: 0x06005BB9 RID: 23481 RVA: 0x0003241D File Offset: 0x0003061D
	public GameMessengerListenerRemovedEventArgs(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.Method = method;
		this.FrameNumber = Time.frameCount;
	}

	// Token: 0x17001E66 RID: 7782
	// (get) Token: 0x06005BBA RID: 23482 RVA: 0x00032445 File Offset: 0x00030645
	// (set) Token: 0x06005BBB RID: 23483 RVA: 0x0003244D File Offset: 0x0003064D
	public string Event { get; private set; }

	// Token: 0x17001E67 RID: 7783
	// (get) Token: 0x06005BBC RID: 23484 RVA: 0x00032456 File Offset: 0x00030656
	// (set) Token: 0x06005BBD RID: 23485 RVA: 0x0003245E File Offset: 0x0003065E
	public MonoBehaviour MonoBehaviour { get; private set; }

	// Token: 0x17001E68 RID: 7784
	// (get) Token: 0x06005BBE RID: 23486 RVA: 0x00032467 File Offset: 0x00030667
	// (set) Token: 0x06005BBF RID: 23487 RVA: 0x0003246F File Offset: 0x0003066F
	public string Method { get; private set; }

	// Token: 0x17001E69 RID: 7785
	// (get) Token: 0x06005BC0 RID: 23488 RVA: 0x00032478 File Offset: 0x00030678
	// (set) Token: 0x06005BC1 RID: 23489 RVA: 0x00032480 File Offset: 0x00030680
	public int FrameNumber { get; private set; }
}
