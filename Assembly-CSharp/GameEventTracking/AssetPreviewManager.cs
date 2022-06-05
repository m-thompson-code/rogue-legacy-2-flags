using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DED RID: 3565
	[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Asset Preview Manager")]
	public class AssetPreviewManager : ScriptableObject
	{
		// Token: 0x17002054 RID: 8276
		// (get) Token: 0x0600642F RID: 25647 RVA: 0x000375C3 File Offset: 0x000357C3
		private static AssetPreviewManager Instance
		{
			get
			{
				if (AssetPreviewManager.m_instance == null)
				{
					AssetPreviewManager.m_instance = CDGResources.Load<AssetPreviewManager>("Scriptable Objects/Libraries/AssetPreviewManager", "", true);
				}
				return AssetPreviewManager.m_instance;
			}
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x00173AA0 File Offset: 0x00171CA0
		public static Sprite GetPreviewImage(ChestType chestType)
		{
			if (AssetPreviewManager.m_chestPreviewImageTable == null)
			{
				AssetPreviewManager.m_chestPreviewImageTable = new Dictionary<ChestType, Sprite>();
				foreach (ChestPreviewEntry chestPreviewEntry in AssetPreviewManager.Instance.ChestPreviewImages)
				{
					AssetPreviewManager.m_chestPreviewImageTable.Add(chestPreviewEntry.ChestType, chestPreviewEntry.PreviewImage);
				}
			}
			if (AssetPreviewManager.m_chestPreviewImageTable.ContainsKey(chestType))
			{
				return AssetPreviewManager.m_chestPreviewImageTable[chestType];
			}
			return null;
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x00173B34 File Offset: 0x00171D34
		public static Sprite GetPreviewImage(EnemyType enemyType, EnemyRank enemyRank)
		{
			if (AssetPreviewManager.m_enemyPreviewTable == null)
			{
				AssetPreviewManager.m_enemyPreviewTable = new Dictionary<EnemyTypeAndRank, Sprite>();
				foreach (EnemyPreviewEntry enemyPreviewEntry in AssetPreviewManager.Instance.EnemyPreviewImages)
				{
					foreach (EnemyRankEntry enemyRankEntry in enemyPreviewEntry.RankEntries)
					{
						AssetPreviewManager.m_enemyPreviewTable.Add(new EnemyTypeAndRank(enemyPreviewEntry.Type, enemyRankEntry.Rank), enemyRankEntry.PreviewImage);
					}
				}
			}
			EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
			if (AssetPreviewManager.m_enemyPreviewTable.ContainsKey(key) && AssetPreviewManager.m_enemyPreviewTable[key] != null)
			{
				return AssetPreviewManager.m_enemyPreviewTable[key];
			}
			if (!AssetPreviewManager.m_enemyPreviewTable.ContainsKey(key))
			{
				Debug.LogFormat("<color=red>| {0} | Doesn't contain an entry for Enemy ({1},{2})</color>", new object[]
				{
					AssetPreviewManager.Instance,
					enemyRank,
					enemyType
				});
			}
			else if (AssetPreviewManager.m_enemyPreviewTable[key] == null)
			{
				Debug.LogFormat("<color=red>| {0} | Sprite field is null for Enemy ({1},{2})</color>", new object[]
				{
					AssetPreviewManager.Instance,
					enemyRank,
					enemyType
				});
			}
			return AssetPreviewManager.Instance.m_defaultSprite;
		}

		// Token: 0x040051AD RID: 20909
		public List<EnemyPreviewEntry> EnemyPreviewImages;

		// Token: 0x040051AE RID: 20910
		public List<ChestPreviewEntry> ChestPreviewImages;

		// Token: 0x040051AF RID: 20911
		[SerializeField]
		private Sprite m_defaultSprite;

		// Token: 0x040051B0 RID: 20912
		public string ImageFolderPath = "Textures/UI/DeathScreen";

		// Token: 0x040051B1 RID: 20913
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AssetPreviewManager";

		// Token: 0x040051B2 RID: 20914
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AssetPreviewManager.asset";

		// Token: 0x040051B3 RID: 20915
		private static AssetPreviewManager m_instance;

		// Token: 0x040051B4 RID: 20916
		private static Dictionary<ChestType, Sprite> m_chestPreviewImageTable;

		// Token: 0x040051B5 RID: 20917
		private static Dictionary<EnemyTypeAndRank, Sprite> m_enemyPreviewTable;
	}
}
