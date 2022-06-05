using System;

// Token: 0x02000C9A RID: 3226
public class PlayerHUDVisibilityEventArgs : EventArgs
{
	// Token: 0x06005C95 RID: 23701 RVA: 0x00032DAF File Offset: 0x00030FAF
	public PlayerHUDVisibilityEventArgs(float tweenDuration)
	{
		this.Initialize(tweenDuration);
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x00032DBE File Offset: 0x00030FBE
	public void Initialize(float tweenDuration)
	{
		this.TweenDuration = tweenDuration;
	}

	// Token: 0x17001EB2 RID: 7858
	// (get) Token: 0x06005C97 RID: 23703 RVA: 0x00032DC7 File Offset: 0x00030FC7
	// (set) Token: 0x06005C98 RID: 23704 RVA: 0x00032DCF File Offset: 0x00030FCF
	public float TweenDuration { get; private set; }
}
