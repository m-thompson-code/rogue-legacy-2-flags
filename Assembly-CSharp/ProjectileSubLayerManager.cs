using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000808 RID: 2056
[CreateAssetMenu(menuName = "Custom/Projectile Sub Layer Manager")]
public class ProjectileSubLayerManager : ScriptableObject
{
	// Token: 0x170016EE RID: 5870
	// (get) Token: 0x06004406 RID: 17414 RVA: 0x000F0BA5 File Offset: 0x000EEDA5
	private static ProjectileSubLayerManager Instance
	{
		get
		{
			if (Application.isPlaying)
			{
				if (ProjectileSubLayerManager.m_instance == null)
				{
					ProjectileSubLayerManager.m_instance = CDGResources.Load<ProjectileSubLayerManager>("Scriptable Objects/ProjectileSubLayerManager", "", true);
				}
				return ProjectileSubLayerManager.m_instance;
			}
			return ProjectileSubLayerManager.m_instance;
		}
	}

	// Token: 0x170016EF RID: 5871
	// (get) Token: 0x06004407 RID: 17415 RVA: 0x000F0BDB File Offset: 0x000EEDDB
	// (set) Token: 0x06004408 RID: 17416 RVA: 0x000F0BE3 File Offset: 0x000EEDE3
	public List<ProjectileSubLayerEntry> SubLayers
	{
		get
		{
			return this.m_projectileSubLayers;
		}
		private set
		{
			this.m_projectileSubLayers = value;
		}
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x000F0BEC File Offset: 0x000EEDEC
	public static int GetSubLayer(string projectileName)
	{
		if (ProjectileSubLayerManager.Instance.m_subLayerTable == null)
		{
			ProjectileSubLayerManager.Instance.m_subLayerTable = new Dictionary<string, int>(100);
			for (int i = 0; i < ProjectileSubLayerManager.Instance.SubLayers.Count; i++)
			{
				ProjectileSubLayerManager.Instance.m_subLayerTable.Add(ProjectileSubLayerManager.Instance.SubLayers[i].Name, ProjectileSubLayerManager.Instance.SubLayers[i].SubLayer);
			}
		}
		if (ProjectileSubLayerManager.Instance.m_subLayerTable.ContainsKey(projectileName))
		{
			return CameraLayerUtility.DefaultProjectileSubLayer + ProjectileSubLayerManager.Instance.m_subLayerTable[projectileName];
		}
		return 0;
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x000F0C93 File Offset: 0x000EEE93
	public void Reset()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.SubLayers == null)
		{
			this.SubLayers = new List<ProjectileSubLayerEntry>();
			return;
		}
		this.SubLayers.Clear();
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x000F0CBC File Offset: 0x000EEEBC
	public void UpdateSubLayers()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x04003A1D RID: 14877
	[SerializeField]
	private List<ProjectileSubLayerEntry> m_projectileSubLayers;

	// Token: 0x04003A1E RID: 14878
	[SerializeField]
	private int m_subLayerDelta = 10;

	// Token: 0x04003A1F RID: 14879
	public const string RESOURCES_PATH = "Scriptable Objects/ProjectileSubLayerManager";

	// Token: 0x04003A20 RID: 14880
	public const string ASSETS_PATH = "Assets/Content/Scriptable Objects/ProjectileSubLayerManager.asset";

	// Token: 0x04003A21 RID: 14881
	public const string PREFABS_RESOURCES_PATH = "Prefabs/Projectiles";

	// Token: 0x04003A22 RID: 14882
	public const string PREFABS_ASSETS_PATH = "Assets/Content/Prefabs/Projectiles";

	// Token: 0x04003A23 RID: 14883
	private static ProjectileSubLayerManager m_instance;

	// Token: 0x04003A24 RID: 14884
	private ProjectileSizeComparer m_comparer;

	// Token: 0x04003A25 RID: 14885
	private Dictionary<string, int> m_subLayerTable;
}
