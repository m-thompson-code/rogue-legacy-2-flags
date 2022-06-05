using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008B6 RID: 2230
	[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Asset Preview Manager")]
	public class AssetPreviewManager : ScriptableObject
	{
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x060048AF RID: 18607 RVA: 0x00104C7F File Offset: 0x00102E7F
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

		// Token: 0x060048B0 RID: 18608 RVA: 0x00104CA8 File Offset: 0x00102EA8
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

		// Token: 0x060048B1 RID: 18609 RVA: 0x00104D3C File Offset: 0x00102F3C
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

		// Token: 0x04003D53 RID: 15699
		public List<EnemyPreviewEntry> EnemyPreviewImages;

		// Token: 0x04003D54 RID: 15700
		public List<ChestPreviewEntry> ChestPreviewImages;

		// Token: 0x04003D55 RID: 15701
		[SerializeField]
		private Sprite m_defaultSprite;

		// Token: 0x04003D56 RID: 15702
		public string ImageFolderPath = "Textures/UI/DeathScreen";

		// Token: 0x04003D57 RID: 15703
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AssetPreviewManager";

		// Token: 0x04003D58 RID: 15704
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AssetPreviewManager.asset";

		// Token: 0x04003D59 RID: 15705
		private static AssetPreviewManager m_instance;

		// Token: 0x04003D5A RID: 15706
		private static Dictionary<ChestType, Sprite> m_chestPreviewImageTable;

		// Token: 0x04003D5B RID: 15707
		private static Dictionary<EnemyTypeAndRank, Sprite> m_enemyPreviewTable;
	}
}
