using System;

// Token: 0x020007DC RID: 2012
public class JukeboxOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x0600433C RID: 17212 RVA: 0x000EC5AB File Offset: 0x000EA7AB
	public JukeboxOmniUIDescriptionEventArgs(SongID songType, bool isPlaying)
	{
		this.Initialize(songType, isPlaying);
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x000EC5BB File Offset: 0x000EA7BB
	public void Initialize(SongID songType, bool isPlaying)
	{
		this.SongType = songType;
		this.IsPlaying = isPlaying;
	}

	// Token: 0x170016C4 RID: 5828
	// (get) Token: 0x0600433E RID: 17214 RVA: 0x000EC5CB File Offset: 0x000EA7CB
	// (set) Token: 0x0600433F RID: 17215 RVA: 0x000EC5D3 File Offset: 0x000EA7D3
	public SongID SongType { get; private set; }

	// Token: 0x170016C5 RID: 5829
	// (get) Token: 0x06004340 RID: 17216 RVA: 0x000EC5DC File Offset: 0x000EA7DC
	// (set) Token: 0x06004341 RID: 17217 RVA: 0x000EC5E4 File Offset: 0x000EA7E4
	public bool IsPlaying { get; private set; }
}
