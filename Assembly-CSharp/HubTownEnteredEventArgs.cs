using System;

// Token: 0x020007EA RID: 2026
public class HubTownEnteredEventArgs : EventArgs
{
	// Token: 0x06004384 RID: 17284 RVA: 0x000EC8B1 File Offset: 0x000EAAB1
	public HubTownEnteredEventArgs(HubTownController hubTownController)
	{
		this.Initialize(hubTownController);
	}

	// Token: 0x06004385 RID: 17285 RVA: 0x000EC8C0 File Offset: 0x000EAAC0
	public void Initialize(HubTownController hubTownController)
	{
		this.HubTownController = this.HubTownController;
	}

	// Token: 0x170016DA RID: 5850
	// (get) Token: 0x06004386 RID: 17286 RVA: 0x000EC8CE File Offset: 0x000EAACE
	// (set) Token: 0x06004387 RID: 17287 RVA: 0x000EC8D6 File Offset: 0x000EAAD6
	public HubTownController HubTownController { get; private set; }
}
