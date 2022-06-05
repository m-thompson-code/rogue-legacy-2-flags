using System;
using UnityEngine;

// Token: 0x02000809 RID: 2057
[Serializable]
public class ProjectileSubLayerEntry
{
	// Token: 0x0600440E RID: 17422 RVA: 0x000F0CD8 File Offset: 0x000EEED8
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

	// Token: 0x04003A26 RID: 14886
	public string Name;

	// Token: 0x04003A27 RID: 14887
	public int SubLayer;
}
