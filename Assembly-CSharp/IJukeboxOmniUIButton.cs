using System;

// Token: 0x020003B2 RID: 946
public interface IJukeboxOmniUIButton
{
	// Token: 0x17000E70 RID: 3696
	// (get) Token: 0x06002300 RID: 8960
	// (set) Token: 0x06002301 RID: 8961
	SongID SongType { get; set; }

	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x06002302 RID: 8962
	bool IsPlayingSong { get; }

	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06002303 RID: 8963
	// (set) Token: 0x06002304 RID: 8964
	JukeboxOmniUIWindowController JukeboxWindowController { get; set; }

	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x06002305 RID: 8965
	// (set) Token: 0x06002306 RID: 8966
	JukeboxOmniUIEntry JukeboxEntry { get; set; }
}
