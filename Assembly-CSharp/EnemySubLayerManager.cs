using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
[CreateAssetMenu(menuName = "Custom/Enemy Sub Layer Manager")]
public class EnemySubLayerManager : ScriptableObject
{
	// Token: 0x170016EA RID: 5866
	// (get) Token: 0x060043B9 RID: 17337 RVA: 0x000ECC2D File Offset: 0x000EAE2D
	private static EnemySubLayerManager Instance
	{
		get
		{
			if (Application.isPlaying)
			{
				if (EnemySubLayerManager.m_instance == null)
				{
					EnemySubLayerManager.m_instance = CDGResources.Load<EnemySubLayerManager>("Scriptable Objects/EnemySubLayerManager", "", true);
				}
				return EnemySubLayerManager.m_instance;
			}
			return EnemySubLayerManager.m_instance;
		}
	}

	// Token: 0x170016EB RID: 5867
	// (get) Token: 0x060043BA RID: 17338 RVA: 0x000ECC63 File Offset: 0x000EAE63
	// (set) Token: 0x060043BB RID: 17339 RVA: 0x000ECC6B File Offset: 0x000EAE6B
	public List<EnemySubLayerEntry> EnemySubLayers
	{
		get
		{
			return this.m_enemySubLayers;
		}
		private set
		{
			this.m_enemySubLayers = value;
		}
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x000ECC74 File Offset: 0x000EAE74
	public static int GetSubLayer(EnemyTypeAndRank enemy)
	{
		if (EnemySubLayerManager.Instance.m_subLayerTable == null)
		{
			EnemySubLayerManager.Instance.m_subLayerTable = new Dictionary<EnemyTypeAndRank, int>();
			for (int i = 0; i < EnemySubLayerManager.Instance.EnemySubLayers.Count; i++)
			{
				if (!EnemySubLayerManager.Instance.m_subLayerTable.ContainsKey(EnemySubLayerManager.Instance.EnemySubLayers[i].Enemy))
				{
					EnemySubLayerManager.Instance.m_subLayerTable.Add(EnemySubLayerManager.Instance.EnemySubLayers[i].Enemy, EnemySubLayerManager.Instance.EnemySubLayers[i].SubLayer);
				}
				else
				{
					Debug.LogFormat("<color=red>| EnemySubLayerManager | {0} {1} duplicate in Enemy Sub Layer list.</color>", new object[]
					{
						EnemySubLayerManager.Instance.EnemySubLayers[i].Enemy.Rank,
						EnemySubLayerManager.Instance.EnemySubLayers[i].Enemy.Type
					});
				}
			}
		}
		if (EnemySubLayerManager.Instance.m_subLayerTable.ContainsKey(enemy))
		{
			return CameraLayerUtility.DefaultEnemySubLayer + EnemySubLayerManager.Instance.m_subLayerTable[enemy];
		}
		return 0;
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x000ECDA0 File Offset: 0x000EAFA0
	public void PrintSizes()
	{
		for (int i = 0; i < EnemySubLayerManager.Instance.m_enemySubLayers.Count; i++)
		{
			EnemyController enemyPrefab = EnemyLibrary.GetEnemyPrefab(EnemySubLayerManager.Instance.m_enemySubLayers[i].Enemy.Type, EnemySubLayerManager.Instance.m_enemySubLayers[i].Enemy.Rank);
			Bounds bounds = EnemyUtility.GetBounds(enemyPrefab.gameObject);
			float enemyScale = EnemyUtility.GetEnemyScale(enemyPrefab);
			Debug.LogFormat("{0}: {1} {2} size = [{3}, {4}]", new object[]
			{
				i + 1,
				EnemySubLayerManager.Instance.m_enemySubLayers[i].Enemy.Rank,
				EnemySubLayerManager.Instance.m_enemySubLayers[i].Enemy.Type,
				bounds.size.x * enemyScale,
				bounds.size.y * enemyScale
			});
		}
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x000ECEA3 File Offset: 0x000EB0A3
	public void Reset()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.EnemySubLayers == null)
		{
			this.EnemySubLayers = new List<EnemySubLayerEntry>();
			return;
		}
		this.EnemySubLayers.Clear();
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x000ECECC File Offset: 0x000EB0CC
	public void UpdateSubLayers()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x040039E6 RID: 14822
	[SerializeField]
	private List<EnemySubLayerEntry> m_enemySubLayers;

	// Token: 0x040039E7 RID: 14823
	[SerializeField]
	private int m_subLayerDelta = 10;

	// Token: 0x040039E8 RID: 14824
	public const string RESOURCES_PATH = "Scriptable Objects/EnemySubLayerManager";

	// Token: 0x040039E9 RID: 14825
	public const string ASSETS_PATH = "Assets/Content/Scriptable Objects/EnemySubLayerManager.asset";

	// Token: 0x040039EA RID: 14826
	public const string ENEMY_PREFABS_RESOURCES_PATH = "Prefabs/Enemies";

	// Token: 0x040039EB RID: 14827
	public const string ENEMY_PREFABS_ASSETS_PATH = "Assets/Content/Prefabs/Enemies";

	// Token: 0x040039EC RID: 14828
	private static EnemySubLayerManager m_instance;

	// Token: 0x040039ED RID: 14829
	private EnemySizeComparer m_enemySizeComparer;

	// Token: 0x040039EE RID: 14830
	private Dictionary<EnemyTypeAndRank, int> m_subLayerTable;
}
