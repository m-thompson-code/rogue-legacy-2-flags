using System;
using UnityEngine;

// Token: 0x020007AE RID: 1966
public class GameMessengerListenerRemovedEventArgs : EventArgs
{
	// Token: 0x06004230 RID: 16944 RVA: 0x000EBA07 File Offset: 0x000E9C07
	public GameMessengerListenerRemovedEventArgs(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.Method = method;
		this.FrameNumber = Time.frameCount;
	}

	// Token: 0x17001668 RID: 5736
	// (get) Token: 0x06004231 RID: 16945 RVA: 0x000EBA2F File Offset: 0x000E9C2F
	// (set) Token: 0x06004232 RID: 16946 RVA: 0x000EBA37 File Offset: 0x000E9C37
	public string Event { get; private set; }

	// Token: 0x17001669 RID: 5737
	// (get) Token: 0x06004233 RID: 16947 RVA: 0x000EBA40 File Offset: 0x000E9C40
	// (set) Token: 0x06004234 RID: 16948 RVA: 0x000EBA48 File Offset: 0x000E9C48
	public MonoBehaviour MonoBehaviour { get; private set; }

	// Token: 0x1700166A RID: 5738
	// (get) Token: 0x06004235 RID: 16949 RVA: 0x000EBA51 File Offset: 0x000E9C51
	// (set) Token: 0x06004236 RID: 16950 RVA: 0x000EBA59 File Offset: 0x000E9C59
	public string Method { get; private set; }

	// Token: 0x1700166B RID: 5739
	// (get) Token: 0x06004237 RID: 16951 RVA: 0x000EBA62 File Offset: 0x000E9C62
	// (set) Token: 0x06004238 RID: 16952 RVA: 0x000EBA6A File Offset: 0x000E9C6A
	public int FrameNumber { get; private set; }
}
