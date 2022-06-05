using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CBC RID: 3260
[CreateAssetMenu(menuName = "Custom/Enemy Sub Layer Manager")]
public class EnemySubLayerManager : ScriptableObject
{
	// Token: 0x17001EE8 RID: 7912
	// (get) Token: 0x06005D42 RID: 23874 RVA: 0x000334DD File Offset: 0x000316DD
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

	// Token: 0x17001EE9 RID: 7913
	// (get) Token: 0x06005D43 RID: 23875 RVA: 0x00033513 File Offset: 0x00031713
	// (set) Token: 0x06005D44 RID: 23876 RVA: 0x0003351B File Offset: 0x0003171B
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

	// Token: 0x06005D45 RID: 23877 RVA: 0x0015AB84 File Offset: 0x00158D84
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

	// Token: 0x06005D46 RID: 23878 RVA: 0x0015ACB0 File Offset: 0x00158EB0
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

	// Token: 0x06005D47 RID: 23879 RVA: 0x00033524 File Offset: 0x00031724
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

	// Token: 0x06005D48 RID: 23880 RVA: 0x0003354D File Offset: 0x0003174D
	public void UpdateSubLayers()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x04004CAB RID: 19627
	[SerializeField]
	private List<EnemySubLayerEntry> m_enemySubLayers;

	// Token: 0x04004CAC RID: 19628
	[SerializeField]
	private int m_subLayerDelta = 10;

	// Token: 0x04004CAD RID: 19629
	public const string RESOURCES_PATH = "Scriptable Objects/EnemySubLayerManager";

	// Token: 0x04004CAE RID: 19630
	public const string ASSETS_PATH = "Assets/Content/Scriptable Objects/EnemySubLayerManager.asset";

	// Token: 0x04004CAF RID: 19631
	public const string ENEMY_PREFABS_RESOURCES_PATH = "Prefabs/Enemies";

	// Token: 0x04004CB0 RID: 19632
	public const string ENEMY_PREFABS_ASSETS_PATH = "Assets/Content/Prefabs/Enemies";

	// Token: 0x04004CB1 RID: 19633
	private static EnemySubLayerManager m_instance;

	// Token: 0x04004CB2 RID: 19634
	private EnemySizeComparer m_enemySizeComparer;

	// Token: 0x04004CB3 RID: 19635
	private Dictionary<EnemyTypeAndRank, int> m_subLayerTable;
}
