using System;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public class MessengerListenerChangeEventArgs : EventArgs
{
	// Token: 0x06004226 RID: 16934 RVA: 0x000EB991 File Offset: 0x000E9B91
	public MessengerListenerChangeEventArgs(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Initialize(eventID, monoBehaviour, method, frameNumber);
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x000EB9A4 File Offset: 0x000E9BA4
	public void Initialize(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.Method = method;
		this.FrameNumber = frameNumber;
	}

	// Token: 0x17001664 RID: 5732
	// (get) Token: 0x06004228 RID: 16936 RVA: 0x000EB9C3 File Offset: 0x000E9BC3
	// (set) Token: 0x06004229 RID: 16937 RVA: 0x000EB9CB File Offset: 0x000E9BCB
	public string Event { get; private set; }

	// Token: 0x17001665 RID: 5733
	// (get) Token: 0x0600422A RID: 16938 RVA: 0x000EB9D4 File Offset: 0x000E9BD4
	// (set) Token: 0x0600422B RID: 16939 RVA: 0x000EB9DC File Offset: 0x000E9BDC
	public MonoBehaviour MonoBehaviour { get; private set; }

	// Token: 0x17001666 RID: 5734
	// (get) Token: 0x0600422C RID: 16940 RVA: 0x000EB9E5 File Offset: 0x000E9BE5
	// (set) Token: 0x0600422D RID: 16941 RVA: 0x000EB9ED File Offset: 0x000E9BED
	public string Method { get; private set; }

	// Token: 0x17001667 RID: 5735
	// (get) Token: 0x0600422E RID: 16942 RVA: 0x000EB9F6 File Offset: 0x000E9BF6
	// (set) Token: 0x0600422F RID: 16943 RVA: 0x000EB9FE File Offset: 0x000E9BFE
	public int FrameNumber { get; private set; }
}
