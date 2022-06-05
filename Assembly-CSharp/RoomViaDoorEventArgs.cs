using System;

// Token: 0x020007B0 RID: 1968
public class RoomViaDoorEventArgs : EventArgs
{
	// Token: 0x0600423F RID: 16959 RVA: 0x000EBAB5 File Offset: 0x000E9CB5
	public RoomViaDoorEventArgs(BaseRoom room, Door viaDoor = null)
	{
		this.Initialise(room, viaDoor);
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x000EBAC5 File Offset: 0x000E9CC5
	public void Initialise(BaseRoom room, Door viaDoor = null)
	{
		this.Room = room;
		this.ViaDoor = viaDoor;
	}

	// Token: 0x1700166E RID: 5742
	// (get) Token: 0x06004241 RID: 16961 RVA: 0x000EBAD5 File Offset: 0x000E9CD5
	// (set) Token: 0x06004242 RID: 16962 RVA: 0x000EBADD File Offset: 0x000E9CDD
	public BaseRoom Room { get; private set; }

	// Token: 0x1700166F RID: 5743
	// (get) Token: 0x06004243 RID: 16963 RVA: 0x000EBAE6 File Offset: 0x000E9CE6
	// (set) Token: 0x06004244 RID: 16964 RVA: 0x000EBAEE File Offset: 0x000E9CEE
	public Door ViaDoor { get; private set; }
}
