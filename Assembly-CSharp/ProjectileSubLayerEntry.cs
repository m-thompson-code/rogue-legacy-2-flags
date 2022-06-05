using System;
using UnityEngine;

// Token: 0x02000CD1 RID: 3281
[Serializable]
public class ProjectileSubLayerEntry
{
	// Token: 0x06005D97 RID: 23959 RVA: 0x0015E830 File Offset: 0x0015CA30
	public ProjectileSubLayerEntry(Projectile_RL prefab, int subLayer)
	{
		string name = "";
		if (prefab != null)
		{
			if (prefab.ProjectileData != null)
			{
				name = prefab.name;
			}
			else
			{
				Debug.LogFormat("<color=purple>Projectile Prefab ({0}) has no Projectile Data set.</color>", new object[]
				{
					prefab
				});
			}
		}
		else
		{
			Debug.LogFormat("<color=purple>ProjectileSubLayerEntry Constructor was passed a null prefab argument</color>", Array.Empty<object>());
		}
		this.Name = name;
		this.SubLayer = subLayer;
	}

	// Token: 0x04004CF2 RID: 19698
	public string Name;

	// Token: 0x04004CF3 RID: 19699
	public int SubLayer;
}
