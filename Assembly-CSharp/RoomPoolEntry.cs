using System;
using Rooms;
using UnityEngine;

// Token: 0x02000B94 RID: 2964
[Serializable]
public class RoomPoolEntry
{
	// Token: 0x06005955 RID: 22869 RVA: 0x00030A93 File Offset: 0x0002EC93
	public RoomPoolEntry(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x17001DDF RID: 7647
	// (get) Token: 0x06005956 RID: 22870 RVA: 0x00030AA2 File Offset: 0x0002ECA2
	// (set) Token: 0x06005957 RID: 22871 RVA: 0x00030AAA File Offset: 0x0002ECAA
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

	// Token: 0x0400437C RID: 17276
	[SerializeField]
	[ReadOnly]
	private BiomeType m_biome;

	// Token: 0x0400437D RID: 17277
	public CompiledScene_ScriptableObject[] StandardRoomPool;

	// Token: 0x0400437E RID: 17278
	public MandatoryRoomEntry[] MandatoryRoomPool;

	// Token: 0x0400437F RID: 17279
	public RoomMetaData TransitionRoomMetaData;

	// Token: 0x04004380 RID: 17280
	public CompiledScene_ScriptableObject[] FairyRoomPoolOverride;

	// Token: 0x04004381 RID: 17281
	public CompiledScene_ScriptableObject[] TrapRoomPoolOverride;

	// Token: 0x04004382 RID: 17282
	public CompiledScene_ScriptableObject[] SpecialRoomProxies;
}
