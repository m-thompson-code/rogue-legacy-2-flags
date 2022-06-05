using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000B90 RID: 2960
[CreateAssetMenu(menuName = "Custom/Level Editor/Room Pool")]
public class RoomPool : ScriptableObject
{
	// Token: 0x17001DDB RID: 7643
	// (get) Token: 0x06005945 RID: 22853 RVA: 0x00030A15 File Offset: 0x0002EC15
	// (set) Token: 0x06005946 RID: 22854 RVA: 0x00030A1D File Offset: 0x0002EC1D
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

	// Token: 0x17001DDC RID: 7644
	// (get) Token: 0x06005947 RID: 22855 RVA: 0x00030A26 File Offset: 0x0002EC26
	public CompiledScene_ScriptableObject[] TrapRoomPool
	{
		get
		{
			return this.m_trapRoomPool;
		}
	}

	// Token: 0x17001DDD RID: 7645
	// (get) Token: 0x06005948 RID: 22856 RVA: 0x00030A2E File Offset: 0x0002EC2E
	public CompiledScene_ScriptableObject[] FairyRoomPool
	{
		get
		{
			return this.m_fairyRoomPool;
		}
	}

	// Token: 0x17001DDE RID: 7646
	// (get) Token: 0x06005949 RID: 22857 RVA: 0x00030A36 File Offset: 0x0002EC36
	public CompiledScene_ScriptableObject[] SpecialRoomPool
	{
		get
		{
			return this.m_specialRoomPool;
		}
	}

	// Token: 0x0600594A RID: 22858 RVA: 0x001528E0 File Offset: 0x00150AE0
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

	// Token: 0x0600594B RID: 22859 RVA: 0x001529A4 File Offset: 0x00150BA4
	public MandatoryRoomEntry[] GetMandatoryRoomEntries(BiomeType biomeType)
	{
		RoomPoolEntry roomPoolEntry = this.BiomeRoomPools.SingleOrDefault((RoomPoolEntry entry) => entry.Biome == biomeType);
		if (roomPoolEntry != null)
		{
			return roomPoolEntry.MandatoryRoomPool;
		}
		return new MandatoryRoomEntry[0];
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x001529E8 File Offset: 0x00150BE8
	public RoomMetaData GetTransitionRoom(BiomeType biomeType)
	{
		RoomPoolEntry roomPoolEntry = this.BiomeRoomPools.SingleOrDefault((RoomPoolEntry entry) => entry.Biome == biomeType);
		if (roomPoolEntry != null)
		{
			return roomPoolEntry.TransitionRoomMetaData;
		}
		return null;
	}

	// Token: 0x04004373 RID: 17267
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_trapRoomPool;

	// Token: 0x04004374 RID: 17268
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_fairyRoomPool;

	// Token: 0x04004375 RID: 17269
	[SerializeField]
	private CompiledScene_ScriptableObject[] m_specialRoomPool;

	// Token: 0x04004376 RID: 17270
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

	// Token: 0x04004377 RID: 17271
	public static string RESOURCES_PATH = "Scriptable Objects/RoomPool";

	// Token: 0x04004378 RID: 17272
	public static string ASSET_PATH = "Assets/Content/" + RoomPool.RESOURCES_PATH + ".asset";
}
