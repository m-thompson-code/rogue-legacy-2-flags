using System;

// Token: 0x02000256 RID: 598
[Serializable]
public class TunnelLibraryEntry
{
	// Token: 0x06001798 RID: 6040 RVA: 0x00049680 File Offset: 0x00047880
	public TunnelLibraryEntry(TunnelCategory category)
	{
		this.Category = category;
	}

	// Token: 0x0400171D RID: 5917
	public TunnelCategory Category;

	// Token: 0x0400171E RID: 5918
	public Tunnel Prefab;
}
