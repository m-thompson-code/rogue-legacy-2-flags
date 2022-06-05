using System;
using UnityEngine;

// Token: 0x0200023F RID: 575
[CreateAssetMenu(menuName = "Custom/Libraries/Projectile Library")]
public class ProjectileLibrary : ScriptableObject
{
	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06001710 RID: 5904 RVA: 0x00047E51 File Offset: 0x00046051
	public static Sprite DefaultOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_defaultOffscreenSprite;
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06001711 RID: 5905 RVA: 0x00047E5D File Offset: 0x0004605D
	public static Sprite VoidOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_voidOffscreenSprite;
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06001712 RID: 5906 RVA: 0x00047E69 File Offset: 0x00046069
	public static Sprite EnergyOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_energyOffscreenSprite;
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06001713 RID: 5907 RVA: 0x00047E75 File Offset: 0x00046075
	public static Sprite CurseOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_curseOffscreenSprite;
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06001714 RID: 5908 RVA: 0x00047E81 File Offset: 0x00046081
	// (set) Token: 0x06001715 RID: 5909 RVA: 0x00047E8D File Offset: 0x0004608D
	public static ProjectileEntry[] ProjectileEntryArray
	{
		get
		{
			return ProjectileLibrary.Instance.m_projectileEntryArray;
		}
		set
		{
			ProjectileLibrary.Instance.m_projectileEntryArray = value;
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06001716 RID: 5910 RVA: 0x00047E9A File Offset: 0x0004609A
	// (set) Token: 0x06001717 RID: 5911 RVA: 0x00047EA6 File Offset: 0x000460A6
	public static ProjectileBuffColourEntry[] ProjectileBuffColourArray
	{
		get
		{
			return ProjectileLibrary.Instance.m_buffColorArray;
		}
		set
		{
			ProjectileLibrary.Instance.m_buffColorArray = value;
		}
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06001718 RID: 5912 RVA: 0x00047EB4 File Offset: 0x000460B4
	public static ProjectileLibrary Instance
	{
		get
		{
			if (ProjectileLibrary.m_instance == null)
			{
				ProjectileLibrary.m_instance = CDGResources.Load<ProjectileLibrary>("Scriptable Objects/Libraries/ProjectileLibrary", "", true);
				if (Application.isPlaying)
				{
					ProjectileEntry[] projectileEntryArray = ProjectileLibrary.m_instance.m_projectileEntryArray;
					for (int i = 0; i < projectileEntryArray.Length; i++)
					{
						if (projectileEntryArray[i].ProjectilePrefab == null)
						{
							throw new Exception("Null projectile found in projectile library.");
						}
					}
				}
			}
			return ProjectileLibrary.m_instance;
		}
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x00047F24 File Offset: 0x00046124
	public static Projectile_RL GetProjectile(string projectileName)
	{
		foreach (ProjectileEntry projectileEntry in ProjectileLibrary.ProjectileEntryArray)
		{
			if (projectileEntry.ProjectilePrefab.name.Equals(projectileName, StringComparison.OrdinalIgnoreCase))
			{
				return projectileEntry.ProjectilePrefab;
			}
		}
		return null;
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x00047F68 File Offset: 0x00046168
	public static ProjectileEntry GetProjectileEntry(string projectileName)
	{
		foreach (ProjectileEntry projectileEntry in ProjectileLibrary.ProjectileEntryArray)
		{
			if (projectileEntry.ProjectilePrefab.name.Equals(projectileName, StringComparison.OrdinalIgnoreCase))
			{
				return projectileEntry;
			}
		}
		return null;
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x00047FA4 File Offset: 0x000461A4
	public static Color GetBuffColor(ProjectileBuffType buffType)
	{
		for (int i = 0; i < ProjectileLibrary.m_instance.m_buffColorArray.Length; i++)
		{
			if (ProjectileLibrary.m_instance.m_buffColorArray[i].ProjectileBuffType == buffType)
			{
				return ProjectileLibrary.m_instance.m_buffColorArray[i].Color;
			}
		}
		return Color.white;
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x00047FF4 File Offset: 0x000461F4
	public static byte GetBuffPriority(ProjectileBuffType buffType)
	{
		foreach (ProjectileBuffColourEntry projectileBuffColourEntry in ProjectileLibrary.m_instance.m_buffColorArray)
		{
			if (projectileBuffColourEntry.ProjectileBuffType == buffType)
			{
				return projectileBuffColourEntry.Priority;
			}
		}
		return 0;
	}

	// Token: 0x04001690 RID: 5776
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ProjectileLibrary";

	// Token: 0x04001691 RID: 5777
	[SerializeField]
	private Sprite m_defaultOffscreenSprite;

	// Token: 0x04001692 RID: 5778
	[SerializeField]
	private Sprite m_voidOffscreenSprite;

	// Token: 0x04001693 RID: 5779
	[SerializeField]
	private Sprite m_curseOffscreenSprite;

	// Token: 0x04001694 RID: 5780
	[SerializeField]
	private Sprite m_energyOffscreenSprite;

	// Token: 0x04001695 RID: 5781
	[SerializeField]
	private ProjectileBuffColourEntry[] m_buffColorArray;

	// Token: 0x04001696 RID: 5782
	[SerializeField]
	private ProjectileEntry[] m_projectileEntryArray;

	// Token: 0x04001697 RID: 5783
	private static ProjectileLibrary m_instance;
}
