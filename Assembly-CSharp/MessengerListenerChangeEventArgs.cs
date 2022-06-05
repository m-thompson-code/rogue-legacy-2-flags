using System;
using UnityEngine;

// Token: 0x02000C73 RID: 3187
public class MessengerListenerChangeEventArgs : EventArgs
{
	// Token: 0x06005BAF RID: 23471 RVA: 0x000323A7 File Offset: 0x000305A7
	public MessengerListenerChangeEventArgs(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Initialize(eventID, monoBehaviour, method, frameNumber);
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x000323BA File Offset: 0x000305BA
	public void Initialize(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.Method = method;
		this.FrameNumber = frameNumber;
	}

	// Token: 0x17001E62 RID: 7778
	// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x000323D9 File Offset: 0x000305D9
	// (set) Token: 0x06005BB2 RID: 23474 RVA: 0x000323E1 File Offset: 0x000305E1
	public string Event { get; private set; }

	// Token: 0x17001E63 RID: 7779
	// (get) Token: 0x06005BB3 RID: 23475 RVA: 0x000323EA File Offset: 0x000305EA
	// (set) Token: 0x06005BB4 RID: 23476 RVA: 0x000323F2 File Offset: 0x000305F2
	public MonoBehaviour MonoBehaviour { get; private set; }

	// Token: 0x17001E64 RID: 7780
	// (get) Token: 0x06005BB5 RID: 23477 RVA: 0x000323FB File Offset: 0x000305FB
	// (set) Token: 0x06005BB6 RID: 23478 RVA: 0x00032403 File Offset: 0x00030603
	public string Method { get; private set; }

	// Token: 0x17001E65 RID: 7781
	// (get) Token: 0x06005BB7 RID: 23479 RVA: 0x0003240C File Offset: 0x0003060C
	// (set) Token: 0x06005BB8 RID: 23480 RVA: 0x00032414 File Offset: 0x00030614
	public int FrameNumber { get; private set; }
}
