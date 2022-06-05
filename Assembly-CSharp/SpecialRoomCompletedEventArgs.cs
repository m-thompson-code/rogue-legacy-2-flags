using System;

// Token: 0x02000CAA RID: 3242
public class SpecialRoomCompletedEventArgs : EventArgs
{
	// Token: 0x17001ECF RID: 7887
	// (get) Token: 0x06005CEF RID: 23791 RVA: 0x00033186 File Offset: 0x00031386
	// (set) Token: 0x06005CF0 RID: 23792 RVA: 0x0003318E File Offset: 0x0003138E
	public BaseSpecialRoomController SpecialRoomController { get; private set; }

	// Token: 0x06005CF1 RID: 23793 RVA: 0x00033197 File Offset: 0x00031397
	public SpecialRoomCompletedEventArgs(BaseSpecialRoomController specialRoomController)
	{
		this.Initialize(specialRoomController);
	}

	// Token: 0x06005CF2 RID: 23794 RVA: 0x000331A6 File Offset: 0x000313A6
	public void Initialize(BaseSpecialRoomController specialRoomController)
	{
		this.SpecialRoomController = specialRoomController;
	}
}
