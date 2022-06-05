using System;

// Token: 0x020007D4 RID: 2004
public class PlayerHUDVisibilityEventArgs : EventArgs
{
	// Token: 0x0600430C RID: 17164 RVA: 0x000EC399 File Offset: 0x000EA599
	public PlayerHUDVisibilityEventArgs(float tweenDuration)
	{
		this.Initialize(tweenDuration);
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x000EC3A8 File Offset: 0x000EA5A8
	public void Initialize(float tweenDuration)
	{
		this.TweenDuration = tweenDuration;
	}

	// Token: 0x170016B4 RID: 5812
	// (get) Token: 0x0600430E RID: 17166 RVA: 0x000EC3B1 File Offset: 0x000EA5B1
	// (set) Token: 0x0600430F RID: 17167 RVA: 0x000EC3B9 File Offset: 0x000EA5B9
	public float TweenDuration { get; private set; }
}
