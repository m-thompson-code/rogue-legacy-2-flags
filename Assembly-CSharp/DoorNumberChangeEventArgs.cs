using System;

// Token: 0x02000C56 RID: 3158
public class DoorNumberChangeEventArgs : EventArgs
{
	// Token: 0x06005B08 RID: 23304 RVA: 0x00031F30 File Offset: 0x00030130
	public DoorNumberChangeEventArgs(Door door, int originalNumber, int newNumber)
	{
		this.Door = door;
		this.OriginalNumber = originalNumber;
		this.NewNumber = newNumber;
	}

	// Token: 0x17001E51 RID: 7761
	// (get) Token: 0x06005B09 RID: 23305 RVA: 0x00031F4D File Offset: 0x0003014D
	// (set) Token: 0x06005B0A RID: 23306 RVA: 0x00031F55 File Offset: 0x00030155
	public Door Door { get; private set; }

	// Token: 0x17001E52 RID: 7762
	// (get) Token: 0x06005B0B RID: 23307 RVA: 0x00031F5E File Offset: 0x0003015E
	// (set) Token: 0x06005B0C RID: 23308 RVA: 0x00031F66 File Offset: 0x00030166
	public int OriginalNumber { get; private set; }

	// Token: 0x17001E53 RID: 7763
	// (get) Token: 0x06005B0D RID: 23309 RVA: 0x00031F6F File Offset: 0x0003016F
	// (set) Token: 0x06005B0E RID: 23310 RVA: 0x00031F77 File Offset: 0x00030177
	public int NewNumber { get; private set; }
}
