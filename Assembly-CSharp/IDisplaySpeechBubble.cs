using System;

// Token: 0x020005AE RID: 1454
public interface IDisplaySpeechBubble
{
	// Token: 0x1700131D RID: 4893
	// (get) Token: 0x06003633 RID: 13875
	bool ShouldDisplaySpeechBubble { get; }

	// Token: 0x1700131E RID: 4894
	// (get) Token: 0x06003634 RID: 13876
	SpeechBubbleType BubbleType { get; }
}
