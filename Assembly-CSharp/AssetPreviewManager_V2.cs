using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5D RID: 2909
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Asset Preview Manager V2")]
public class AssetPreviewManager_V2 : ScriptableObject
{
	// Token: 0x17001D90 RID: 7568
	// (get) Token: 0x0600588E RID: 22670 RVA: 0x000301ED File Offset: 0x0002E3ED
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

	// Token: 0x0600588F RID: 22671 RVA: 0x00152120 File Offset: 0x00150320
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

	// Token: 0x06005890 RID: 22672 RVA: 0x001521D4 File Offset: 0x001503D4
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

	// Token: 0x06005891 RID: 22673 RVA: 0x0015224C File Offset: 0x0015044C
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

	// Token: 0x06005892 RID: 22674 RVA: 0x001522A4 File Offset: 0x001504A4
	private static void CreateChestPreviewTable()
	{
		AssetPreviewManager_V2.m_chestPreviewTable = new Dictionary<ChestType, GameObject>();
		foreach (AssetPreviewManager_V2.ChestPreviewEntry_V2 chestPreviewEntry_V in AssetPreviewManager_V2.Instance.ChestPreviewEntries)
		{
			AssetPreviewManager_V2.m_chestPreviewTable.Add(chestPreviewEntry_V.ChestType, chestPreviewEntry_V.EntryObj);
		}
	}

	// Token: 0x04004157 RID: 16727
	public GameObject m_defaultPreviewEntry;

	// Token: 0x04004158 RID: 16728
	[Space(10f)]
	public AssetPreviewManager_V2.EnemyPreviewEntry_V2[] EnemyPreviewEntries;

	// Token: 0x04004159 RID: 16729
	public AssetPreviewManager_V2.ChestPreviewEntry_V2[] ChestPreviewEntries;

	// Token: 0x0400415A RID: 16730
	[NonSerialized]
	public string EntryFolderPath = "Prefabs/Enemy Previews";

	// Token: 0x0400415B RID: 16731
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AssetPreviewManager_V2";

	// Token: 0x0400415C RID: 16732
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AssetPreviewManager_V2.asset";

	// Token: 0x0400415D RID: 16733
	private static AssetPreviewManager_V2 m_instance;

	// Token: 0x0400415E RID: 16734
	private static Dictionary<ChestType, GameObject> m_chestPreviewTable;

	// Token: 0x0400415F RID: 16735
	private static Dictionary<EnemyTypeAndRank, GameObject> m_enemyPreviewTable;

	// Token: 0x02000B5E RID: 2910
	[Serializable]
	public class EnemyRankEntry_V2
	{
		// Token: 0x06005895 RID: 22677 RVA: 0x00030228 File Offset: 0x0002E428
		public EnemyRankEntry_V2(EnemyRank rank)
		{
			this.Rank = rank;
		}

		// Token: 0x04004160 RID: 16736
		public EnemyRank Rank = EnemyRank.None;

		// Token: 0x04004161 RID: 16737
		public GameObject EntryObj;
	}

	// Token: 0x02000B5F RID: 2911
	[Serializable]
	public class EnemyPreviewEntry_V2
	{
		// Token: 0x06005896 RID: 22678 RVA: 0x001522F0 File Offset: 0x001504F0
		public EnemyPreviewEntry_V2(EnemyType enemyType)
		{
			this.EnemyType = enemyType;
			this.RankEntries = new AssetPreviewManager_V2.EnemyRankEntry_V2[4];
			this.RankEntries[0] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Basic);
			this.RankEntries[1] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Advanced);
			this.RankEntries[2] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Expert);
			this.RankEntries[3] = new AssetPreviewManager_V2.EnemyRankEntry_V2(EnemyRank.Miniboss);
		}

		// Token: 0x04004162 RID: 16738
		public EnemyType EnemyType;

		// Token: 0x04004163 RID: 16739
		public AssetPreviewManager_V2.EnemyRankEntry_V2[] RankEntries;
	}

	// Token: 0x02000B60 RID: 2912
	[Serializable]
	public class ChestPreviewEntry_V2
	{
		// Token: 0x06005897 RID: 22679 RVA: 0x0003023E File Offset: 0x0002E43E
		public ChestPreviewEntry_V2(ChestType chestType)
		{
			this.ChestType = chestType;
		}

		// Token: 0x04004164 RID: 16740
		public ChestType ChestType;

		// Token: 0x04004165 RID: 16741
		public GameObject EntryObj;
	}
}
