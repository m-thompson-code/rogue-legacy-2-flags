using System;

// Token: 0x020007E4 RID: 2020
public class SpecialRoomCompletedEventArgs : EventArgs
{
	// Token: 0x170016D1 RID: 5841
	// (get) Token: 0x06004366 RID: 17254 RVA: 0x000EC770 File Offset: 0x000EA970
	// (set) Token: 0x06004367 RID: 17255 RVA: 0x000EC778 File Offset: 0x000EA978
	public BaseSpecialRoomController SpecialRoomController { get; private set; }

	// Token: 0x06004368 RID: 17256 RVA: 0x000EC781 File Offset: 0x000EA981
	public SpecialRoomCompletedEventArgs(BaseSpecialRoomController specialRoomController)
	{
		this.Initialize(specialRoomController);
	}

	// Token: 0x06004369 RID: 17257 RVA: 0x000EC790 File Offset: 0x000EA990
	public void Initialize(BaseSpecialRoomController specialRoomController)
	{
		this.SpecialRoomController = specialRoomController;
	}
}
