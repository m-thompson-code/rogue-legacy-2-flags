using System;

// Token: 0x02000C8C RID: 3212
public class LevelEditorWorldCreationCompleteEventArgs : EventArgs
{
	// Token: 0x06005C3D RID: 23613 RVA: 0x000329D1 File Offset: 0x00030BD1
	public LevelEditorWorldCreationCompleteEventArgs(BaseRoom builtRoom)
	{
		this.BuiltRoom = builtRoom;
	}

	// Token: 0x17001E93 RID: 7827
	// (get) Token: 0x06005C3E RID: 23614 RVA: 0x000329E0 File Offset: 0x00030BE0
	// (set) Token: 0x06005C3F RID: 23615 RVA: 0x000329E8 File Offset: 0x00030BE8
	public BaseRoom BuiltRoom { get; private set; }
}
