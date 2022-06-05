using System;

// Token: 0x02000415 RID: 1045
[Serializable]
public class TunnelLibraryEntry
{
	// Token: 0x06002151 RID: 8529 RVA: 0x00011BDB File Offset: 0x0000FDDB
	public TunnelLibraryEntry(TunnelCategory category)
	{
		this.Category = category;
	}

	// Token: 0x04001E39 RID: 7737
	public TunnelCategory Category;

	// Token: 0x04001E3A RID: 7738
	public Tunnel Prefab;
}
