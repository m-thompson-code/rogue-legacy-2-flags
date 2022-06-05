using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
[CreateAssetMenu(menuName = "Custom/Level Editor/Room Pool")]
public class RoomPool : ScriptableObject
{
	// Token: 0x170015E3 RID: 5603
	// (get) Token: 0x0600400E RID: 16398 RVA: 0x000E2F68 File Offset: 0x000E1168
	// (set) Token: 0x0600400F RID: 16399 RVA: 0x000E2F70 File Offset: 0x000E1170
	public List<RoomPoolEntry> BiomeRoomPools
	{
		get
		{
			return this.m_biomeRoomPools;
		}
		set
		{
			this.m_biomeRoomPools = value;
		}
	}

	// Token: 0x170015E4 RID: 5604
	// (get) Token: 0x06004010 RID: 16400 RVA: 0x000E2F79 File Offset: 0x000E1179
	public CompiledScene_ScriptableObject[] TrapRoomPool
	{
		get
		{
			return this.m_trapRoomPool;
		}
	}

	// Token: 0x170015E5 RID: 5605
	// (get) Token: 0x06004011 RID: 16401 RVA: 0x000E2F81 File Offset: 0x000E1181
	public CompiledScene_ScriptableObject[] FairyRoomPool
	{
		get
		{
			return this.m_fairyRoomPool;
		}
	}

	// Token: 0x170015E6 RID: 5606
	// (get) Token: 0x06004012 RID: 16402 RVA: 0x000E2F89 File Offset: 0x000E1189
	public CompiledScene_ScriptableObject[] SpecialRoomPool
	{
		get
		{
			return this.m_specialRoomPool;
		}
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x000E2F94 File Offset: 0x000E1194
	public CompiledScene_ScriptableObject[] GetCompiledScenes(BiomeType biomeType, RoomType roomType)
	{
		RoomPoolEntry roomPoolEntry = this.BiomeRoomPools.SingleOrDefault((RoomPoolEntry entry) => entry.Biome == biomeType);
		if (roomPoolEntry != null)
		{
			if (roomType <= RoomType.Fairy)
			{
				if (roomType == RoomType.Standard)
				{
					return roomPoolEntry.StandardRoomPool;
				}
				if (roomType == RoomType.Fairy)
				{
					if (roomPoolEntry.FairyRoomPoolOverride != null && roomPoolEntry.FairyRoomPoolOverride.Length != 0)
					{
						return roomPoolEntry.FairyRoomPoolOverride;
					}
					return this.m_fairyRoomPool;
				}
			}
			else if (roomType != RoomType.Trap)
			{
				if (roomType == RoomType.Bonus)
				{
					if (roomPoolEntry.SpecialRoomProxies != null && roomPoolEntry.SpecialRoomProxies.Length != 0)
					{
						return roomPoolEntry.SpecialRoomProxies;
					}
					return this.m_specialRoomPool;
				}
			}
			else
			{
				if (roomPoolEntry.TrapRoomPoolOverride != null && roomPoolEntry.TrapRoomPoolOverride.Length != 0)
				{
					return roomPoolEntry.TrapRoomPoolOverride;
				}
				return this.m_trapRoomPool;
			}
			return new CompiledScene_ScriptableObject[0];
		}
		return new CompiledScene_ScriptableObject[0];
	}

	// Token: 0x06004014 RID: 16404 RVA: 0x000E3058 File Offset: 0x000E1258
	public MandatoryRoomEntry[] GetMandatoryRoomEntries(BiomeType biomeType)
	{
		RoomPoolEntry roomPoolEntry = this.BiomeRoomPools.SingleOrDefault((RoomPoolEntry entry) => entry.Biome == biomeType);
		if (roomPoolEntry != null)
		{
			return roomPoolEntry.MandatoryRoomPool;
		}
		return new MandatoryRoomEntry[0];
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x000E309C File Offset: 0x000E129C
	public RoomMetaData GetTransitionRoom(BiomeType biomeType)
	{
		RoomPoolEntry roomPoolEntry = this.BiomeRoomPools.SingleOrDefault((RoomPoolEntry entry) => entry.Biome == biomeType);
		if (roomPoolEntry != null)
		{
			return roomPoolEntry.TransitionRoomMetaData;
		}
		return null;
	}

	// Token: 0x04003124 RID: 12580
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_trapRoomPool;

	// Token: 0x04003125 RID: 12581
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_fairyRoomPool;

	// Token: 0x04003126 RID: 12582
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_specialRoomPool;

	// Token: 0x04003127 RID: 12583
	[SerializeField]
	private List<RoomPoolEntry> m_biomeRoomPools = new List<RoomPoolEntry>
	{
		new RoomPoolEntry(BiomeType.Castle),
		new RoomPoolEntry(BiomeType.Cave),
		new RoomPoolEntry(BiomeType.Dragon),
		new RoomPoolEntry(BiomeType.Editor),
		new RoomPoolEntry(BiomeType.Forest),
		new RoomPoolEntry(BiomeType.Garden),
		new RoomPoolEntry(BiomeType.Lake),
		new RoomPoolEntry(BiomeType.Stone),
		new RoomPoolEntry(BiomeType.Study),
		new RoomPoolEntry(BiomeType.Sunken),
		new RoomPoolEntry(BiomeType.Tower),
		new RoomPoolEntry(BiomeType.TowerExterior),
		new RoomPoolEntry(BiomeType.Town)
	};

	// Token: 0x04003128 RID: 12584
	public static string RESOURCES_PATH = "Scriptable Objects/RoomPool";

	// Token: 0x04003129 RID: 12585
	public static string ASSET_PATH = "Assets/Content/" + RoomPool.RESOURCES_PATH + ".asset";
}
