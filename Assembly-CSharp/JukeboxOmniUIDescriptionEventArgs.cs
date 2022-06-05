using System;

// Token: 0x02000CA2 RID: 3234
public class JukeboxOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CC5 RID: 23749 RVA: 0x00032FC1 File Offset: 0x000311C1
	public JukeboxOmniUIDescriptionEventArgs(SongID songType, bool isPlaying)
	{
		this.Initialize(songType, isPlaying);
	}

	// Token: 0x06005CC6 RID: 23750 RVA: 0x00032FD1 File Offset: 0x000311D1
	public void Initialize(SongID songType, bool isPlaying)
	{
		this.SongType = songType;
		this.IsPlaying = isPlaying;
	}

	// Token: 0x17001EC2 RID: 7874
	// (get) Token: 0x06005CC7 RID: 23751 RVA: 0x00032FE1 File Offset: 0x000311E1
	// (set) Token: 0x06005CC8 RID: 23752 RVA: 0x00032FE9 File Offset: 0x000311E9
	public SongID SongType { get; private set; }

	// Token: 0x17001EC3 RID: 7875
	// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x00032FF2 File Offset: 0x000311F2
	// (set) Token: 0x06005CCA RID: 23754 RVA: 0x00032FFA File Offset: 0x000311FA
	public bool IsPlaying { get; private set; }
}
