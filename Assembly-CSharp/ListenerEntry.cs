using System;
using UnityEngine;

// Token: 0x02000A41 RID: 2625
[Serializable]
public class ListenerEntry
{
	// Token: 0x06004F27 RID: 20263 RVA: 0x0002B26C File Offset: 0x0002946C
	public ListenerEntry(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.MonoBehaviourName = monoBehaviour.ToString();
		this.Method = method;
		this.FrameNumber = Time.frameCount;
	}

	// Token: 0x04003C1E RID: 15390
	public string Event;

	// Token: 0x04003C1F RID: 15391
	public MonoBehaviour MonoBehaviour;

	// Token: 0x04003C20 RID: 15392
	public string MonoBehaviourName;

	// Token: 0x04003C21 RID: 15393
	public string Method;

	// Token: 0x04003C22 RID: 15394
	public int FrameNumber;
}
