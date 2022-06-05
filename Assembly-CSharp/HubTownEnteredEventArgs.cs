using System;

// Token: 0x02000CB0 RID: 3248
public class HubTownEnteredEventArgs : EventArgs
{
	// Token: 0x06005D0D RID: 23821 RVA: 0x000332C7 File Offset: 0x000314C7
	public HubTownEnteredEventArgs(HubTownController hubTownController)
	{
		this.Initialize(hubTownController);
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x000332D6 File Offset: 0x000314D6
	public void Initialize(HubTownController hubTownController)
	{
		this.HubTownController = this.HubTownController;
	}

	// Token: 0x17001ED8 RID: 7896
	// (get) Token: 0x06005D0F RID: 23823 RVA: 0x000332E4 File Offset: 0x000314E4
	// (set) Token: 0x06005D10 RID: 23824 RVA: 0x000332EC File Offset: 0x000314EC
	public HubTownController HubTownController { get; private set; }
}
