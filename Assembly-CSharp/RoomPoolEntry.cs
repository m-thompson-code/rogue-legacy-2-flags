using System;
using Rooms;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
[Serializable]
public class RoomPoolEntry
{
	// Token: 0x06004018 RID: 16408 RVA: 0x000E31D4 File Offset: 0x000E13D4
	public RoomPoolEntry(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x170015E7 RID: 5607
	// (get) Token: 0x06004019 RID: 16409 RVA: 0x000E31E3 File Offset: 0x000E13E3
	// (set) Token: 0x0600401A RID: 16410 RVA: 0x000E31EB File Offset: 0x000E13EB
	public BiomeType Biome
	{
		get
		{
			return this.m_biome;
		}
		private set
		{
			this.m_biome = value;
		}
	}

	// Token: 0x0400312A RID: 12586
	[SerializeField]
	[ReadOnly]
	private BiomeType m_biome;

	// Token: 0x0400312B RID: 12587
	public CompiledScene_ScriptableObject[] StandardRoomPool;

	// Token: 0x0400312C RID: 12588
	public MandatoryRoomEntry[] MandatoryRoomPool;

	// Token: 0x0400312D RID: 12589
	public RoomMetaData TransitionRoomMetaData;

	// Token: 0x0400312E RID: 12590
	public CompiledScene_ScriptableObject[] FairyRoomPoolOverride;

	// Token: 0x0400312F RID: 12591
	public CompiledScene_ScriptableObject[] TrapRoomPoolOverride;

	// Token: 0x04003130 RID: 12592
	public CompiledScene_ScriptableObject[] SpecialRoomProxies;
}
