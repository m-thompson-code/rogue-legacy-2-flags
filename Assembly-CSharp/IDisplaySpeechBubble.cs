using System;

// Token: 0x020009B6 RID: 2486
public interface IDisplaySpeechBubble
{
	// Token: 0x17001A4A RID: 6730
	// (get) Token: 0x06004C45 RID: 19525
	bool ShouldDisplaySpeechBubble { get; }

	// Token: 0x17001A4B RID: 6731
	// (get) Token: 0x06004C46 RID: 19526
	SpeechBubbleType BubbleType { get; }
}
