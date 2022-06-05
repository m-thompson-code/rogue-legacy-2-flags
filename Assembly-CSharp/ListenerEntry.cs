using System;
using UnityEngine;

// Token: 0x0200061E RID: 1566
[Serializable]
public class ListenerEntry
{
	// Token: 0x06003883 RID: 14467 RVA: 0x000C10EB File Offset: 0x000BF2EB
	public ListenerEntry(string eventID, MonoBehaviour monoBehaviour, string method, int frameNumber)
	{
		this.Event = eventID;
		this.MonoBehaviour = monoBehaviour;
		this.MonoBehaviourName = monoBehaviour.ToString();
		this.Method = method;
		this.FrameNumber = Time.frameCount;
	}

	// Token: 0x04002BB1 RID: 11185
	public string Event;

	// Token: 0x04002BB2 RID: 11186
	public MonoBehaviour MonoBehaviour;

	// Token: 0x04002BB3 RID: 11187
	public string MonoBehaviourName;

	// Token: 0x04002BB4 RID: 11188
	public string Method;

	// Token: 0x04002BB5 RID: 11189
	public int FrameNumber;
}
