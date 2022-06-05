using System;

// Token: 0x020007C6 RID: 1990
public class LevelEditorWorldCreationCompleteEventArgs : EventArgs
{
	// Token: 0x060042B4 RID: 17076 RVA: 0x000EBFBB File Offset: 0x000EA1BB
	public LevelEditorWorldCreationCompleteEventArgs(BaseRoom builtRoom)
	{
		this.BuiltRoom = builtRoom;
	}

	// Token: 0x17001695 RID: 5781
	// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000EBFCA File Offset: 0x000EA1CA
	// (set) Token: 0x060042B6 RID: 17078 RVA: 0x000EBFD2 File Offset: 0x000EA1D2
	public BaseRoom BuiltRoom { get; private set; }
}
