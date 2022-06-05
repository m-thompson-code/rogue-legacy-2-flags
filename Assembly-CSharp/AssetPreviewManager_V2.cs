using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B5 RID: 1717
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Asset Preview Manager V2")]
public class AssetPreviewManager_V2 : ScriptableObject
{
	// Token: 0x17001598 RID: 5528
	// (get) Token: 0x06003F5A RID: 16218 RVA: 0x000E1EF8 File Offset: 0x000E00F8
	private static AssetPreviewManager_V2 Instance
	{
		get
		{
			if (!AssetPreviewManager_V2.m_instance)
			{
				AssetPreviewManager_V2.m_instance = CDGResources.Load<AssetPreviewManager_V2>("Scriptable Objects/Libraries/AssetPreviewManager_V2", "", true);
			}
			return AssetPreviewManager_V2.m_instance;
		}
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x000E1F20 File Offset: 0x000E0120
	public static GameObject GetEnemyPreviewEntry(EnemyType enemyType, EnemyRank enemyRank)
	{
		if (AssetPreviewManager_V2.m_enemyPreviewTable == null)
		{
			AssetPreviewManager_V2.CreateEnemyPreviewTable();
		}
		EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
		GameObject result;
		if (AssetPreviewManager_V2.m_enemyPreviewTable.TryGetValue(key, out result))
		{
			return result;
		}
		if (!AssetPreviewManager_V2.m_enemyPreviewTable.ContainsKey(key))
		{
			Debug.LogFormat("<color=red>| {0} | Doesn't contain an entry for Enemy ({1},{2})</color>", new object[]
			{
				AssetPreviewManager_V2.Instance,
				enemyRank,
				enemyType
			});
		}
		else if (AssetPreviewManager_V2.m_enemyPreviewTable[key] == null)
		{
			Debug.LogFormat("<color=red>| {0} | EntryObj field is null for Enemy ({1},{2})</color>", new object[]
			{
				AssetPreviewManager_V2.Instance,
				enemyRank,
				enemyType
			});
		}
		return AssetPreviewManager_V2.Instance.m_defaultPreviewEntry;
	}

	// Token: 0x06003F5C RID: 16220 RVA: 0x000E1FD4 File Offset: 0x000E01D4
	private static void CreateEnemyPreviewTable()
	{
		AssetPreviewManager_V2.m_enemyPreviewTable = new Dictionary<EnemyTypeAndRank, GameObject>();
		foreach (AssetPreviewManager_V2.EnemyPreviewEntry_V2 enemyPreviewEntry_V in AssetPreviewManager_V2.Instance.EnemyPreviewEntries)
		{
			foreach (AssetPreviewManager_V2.EnemyRankEntry_V2 enemyRankEntry_V in enemyPreviewEntry_V.RankEntries)
			{
				AssetPreviewManager_V2.m_enemyPreviewTable.Add(new EnemyTypeAndRank(enemyPreviewEntry_V.EnemyType, enemyRankEntry_V.Rank), enemyRankEntry_V.EntryObj);
			}
		}
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x000E204C File Offset: 0x000E024C
	public static GameObject GetChestPreviewEntry(ChestType chestType)
	{
		if (AssetPreviewManager_V2.m_chestPreviewTable == null)
		{
			AssetPreviewManager_V2.CreateChestPreviewTable();
		}
		GameObject result;
		if (AssetPreviewManager_V2.m_chestPreviewTable.TryGetValue(chestType, out result))
		{
			return result;
		}
		Debug.LogFormat("<color=red>| {0} | Doesn't contain an entry for Chest ({1})</color>", new object[]
		{
			AssetPreviewManager_V2.Instance,
			chestType
		});
		return AssetPreviewManager_V2.Instance.m_defaultPreviewEntry;
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x000E20A4 File Offset: 0x000E02A4
	private static void CreateChestPreviewTable()
	{
		AssetPreviewManager_V2.m_chestPreviewTable = new Dictionary<ChestType, GameObject>();
		foreach (AssetPreviewManager_V2.ChestPreviewEntry_V2 chestPreviewEntry_V in AssetPreviewManager_V2.Instance.ChestPreviewEntries)
		{
			AssetPreviewManager_V2.m_chestPreviewTable.Add(chestPreviewEntry_V.ChestType, chestPreviewEntry_V.EntryObj);
		}
	}

	// Token: 0x04002F0E RID: 12046
	public GameObject m_defaultPreviewEntry;

	// Token: 0x04002F0F RID: 12047
	[Space(10f)]
	public AssetPreviewManager_V2.EnemyPreviewEntry_V2[] EnemyPreviewEntries;

	// Token: 0x04002F10 RID: 12048
	public AssetPreviewManager_V2.ChestPreviewEntry_V2[] ChestPreviewEntries;

	// Token: 0x04002F11 RID: 12049
	[NonSerialized]
	public string EntryFolderPath = "Prefabs/Enemy Previews";

	// Token: 0x04002F12 RID: 12050
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AssetPreviewManager_V2";

	// Token: 0x04002F13 RID: 12051
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AssetPreviewManager_V2.asset";

	// Token: 0x04002F14 RID: 12052
	private static AssetPreviewManager_V2 m_instance;

	// Token: 0x04002F15 RID: 12053
	private static Dictionary<ChestType, GameObject> m_chestPreviewTable;

	// Token: 0x04002F16 RID: 12054
	private static Dictionary<EnemyTypeAndRank, GameObject> m_enemyPreviewTable;

	// Token: 0x02000E1C RID: 3612
	[Serializable]
	public class EnemyRankEntry_V2
	{
		// Token: 0x06006B81 RID: 27521 RVA: 0x001918EE File Offset: 0x0018FAEE
		public EnemyRankEntry_V2(EnemyRank rank)
		{
			this.Rank = rank;
		}

		// Token: 0x040056CC RID: 22220
		public EnemyRank Rank = EnemyRank.None;

		// Token: 0x040056CD RID: 22221
		public GameObject EntryObj;
	}

	// Token: 0x02000E1D RID: 3613
	[Serializable]
	public class EnemyPreviewEntry_V2
	{
		// Token: 0x06006B82 RID: 27522 RVA: 0x00191904 File Offset: 0x0018FB04
		public EnemyPreviewEntry_V2(EnemyType enemyType)
		{
			this.EnemyType = enemyType;
			this.RankEntries = new AssetPreviewManager_V2.EnemyRankEntry_V2[4];
			this.RankEntries[0] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Basic);
			this.RankEntries[1] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Advanced);
			this.RankEntries[2] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Expert);
			this.RankEntries[3] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Miniboss);
		}

		// Token: 0x040056CE RID: 22222
		public EnemyType EnemyType;

		// Token: 0x040056CF RID: 22223
		public AssetPreviewManager_V2.EnemyRankEntry_V2[] RankEntries;
	}

	// Token: 0x02000E1E RID: 3614
	[Serializable]
	public class ChestPreviewEntry_V2
	{
		// Token: 0x06006B83 RID: 27523 RVA: 0x00191962 File Offset: 0x0018FB62
		public ChestPreviewEntry_V2(ChestType chestType)
		{
			this.ChestType = chestType;
		}

		// Token: 0x040056D0 RID: 22224
		public ChestType ChestType;

		// Token: 0x040056D1 RID: 22225
		public GameObject EntryObj;
	}
}
