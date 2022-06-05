using System;

// Token: 0x02000647 RID: 1607
public interface IJukeboxOmniUIButton
{
	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x06003118 RID: 12568
	// (set) Token: 0x06003119 RID: 12569
	SongID SongType { get; set; }

	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x0600311A RID: 12570
	bool IsPlayingSong { get; }

	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x0600311B RID: 12571
	// (set) Token: 0x0600311C RID: 12572
	JukeboxOmniUIWindowController JukeboxWindowController { get; set; }

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x0600311D RID: 12573
	// (set) Token: 0x0600311E RID: 12574
	JukeboxOmniUIEntry JukeboxEntry { get; set; }
}
