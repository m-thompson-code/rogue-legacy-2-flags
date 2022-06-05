using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CD0 RID: 3280
[CreateAssetMenu(menuName = "Custom/Projectile Sub Layer Manager")]
public class ProjectileSubLayerManager : ScriptableObject
{
	// Token: 0x17001EEC RID: 7916
	// (get) Token: 0x06005D8F RID: 23951 RVA: 0x000337A7 File Offset: 0x000319A7
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

	// Token: 0x17001EED RID: 7917
	// (get) Token: 0x06005D90 RID: 23952 RVA: 0x000337DD File Offset: 0x000319DD
	// (set) Token: 0x06005D91 RID: 23953 RVA: 0x000337E5 File Offset: 0x000319E5
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

	// Token: 0x06005D92 RID: 23954 RVA: 0x0015E788 File Offset: 0x0015C988
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

	// Token: 0x06005D93 RID: 23955 RVA: 0x000337EE File Offset: 0x000319EE
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

	// Token: 0x06005D94 RID: 23956 RVA: 0x0003354D File Offset: 0x0003174D
	public void UpdateSubLayers()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x04004CE9 RID: 19689
	[SerializeField]
	private List<ProjectileSubLayerEntry> m_projectileSubLayers;

	// Token: 0x04004CEA RID: 19690
	[SerializeField]
	private int m_subLayerDelta = 10;

	// Token: 0x04004CEB RID: 19691
	public const string RESOURCES_PATH = "Scriptable Objects/ProjectileSubLayerManager";

	// Token: 0x04004CEC RID: 19692
	public const string ASSETS_PATH = "Assets/Content/Scriptable Objects/ProjectileSubLayerManager.asset";

	// Token: 0x04004CED RID: 19693
	public const string PREFABS_RESOURCES_PATH = "Prefabs/Projectiles";

	// Token: 0x04004CEE RID: 19694
	public const string PREFABS_ASSETS_PATH = "Assets/Content/Prefabs/Projectiles";

	// Token: 0x04004CEF RID: 19695
	private static ProjectileSubLayerManager m_instance;

	// Token: 0x04004CF0 RID: 19696
	private ProjectileSizeComparer m_comparer;

	// Token: 0x04004CF1 RID: 19697
	private Dictionary<string, int> m_subLayerTable;
}
