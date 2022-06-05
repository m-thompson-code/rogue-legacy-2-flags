using System;
using UnityEngine;

// Token: 0x020003FC RID: 1020
[CreateAssetMenu(menuName = "Custom/Libraries/Projectile Library")]
public class ProjectileLibrary : ScriptableObject
{
	// Token: 0x17000E6F RID: 3695
	// (get) Token: 0x060020C3 RID: 8387 RVA: 0x00011613 File Offset: 0x0000F813
	public static Sprite DefaultOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_defaultOffscreenSprite;
		}
	}

	// Token: 0x17000E70 RID: 3696
	// (get) Token: 0x060020C4 RID: 8388 RVA: 0x0001161F File Offset: 0x0000F81F
	public static Sprite VoidOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_voidOffscreenSprite;
		}
	}

	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x060020C5 RID: 8389 RVA: 0x0001162B File Offset: 0x0000F82B
	public static Sprite EnergyOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_energyOffscreenSprite;
		}
	}

	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x060020C6 RID: 8390 RVA: 0x00011637 File Offset: 0x0000F837
	public static Sprite CurseOffscreenSprite
	{
		get
		{
			return ProjectileLibrary.Instance.m_curseOffscreenSprite;
		}
	}

	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x060020C7 RID: 8391 RVA: 0x00011643 File Offset: 0x0000F843
	// (set) Token: 0x060020C8 RID: 8392 RVA: 0x0001164F File Offset: 0x0000F84F
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

	// Token: 0x17000E74 RID: 3700
	// (get) Token: 0x060020C9 RID: 8393 RVA: 0x0001165C File Offset: 0x0000F85C
	// (set) Token: 0x060020CA RID: 8394 RVA: 0x00011668 File Offset: 0x0000F868
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

	// Token: 0x17000E75 RID: 3701
	// (get) Token: 0x060020CB RID: 8395 RVA: 0x000A5BFC File Offset: 0x000A3DFC
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

	// Token: 0x060020CC RID: 8396 RVA: 0x000A5C6C File Offset: 0x000A3E6C
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

	// Token: 0x060020CD RID: 8397 RVA: 0x000A5CB0 File Offset: 0x000A3EB0
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

	// Token: 0x060020CE RID: 8398 RVA: 0x000A5CEC File Offset: 0x000A3EEC
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

	// Token: 0x060020CF RID: 8399 RVA: 0x000A5D3C File Offset: 0x000A3F3C
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

	// Token: 0x04001DA8 RID: 7592
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ProjectileLibrary";

	// Token: 0x04001DA9 RID: 7593
	[SerializeField]
	private Sprite m_defaultOffscreenSprite;

	// Token: 0x04001DAA RID: 7594
	[SerializeField]
	private Sprite m_voidOffscreenSprite;

	// Token: 0x04001DAB RID: 7595
	[SerializeField]
	private Sprite m_curseOffscreenSprite;

	// Token: 0x04001DAC RID: 7596
	[SerializeField]
	private Sprite m_energyOffscreenSprite;

	// Token: 0x04001DAD RID: 7597
	[SerializeField]
	private ProjectileBuffColourEntry[] m_buffColorArray;

	// Token: 0x04001DAE RID: 7598
	[SerializeField]
	private ProjectileEntry[] m_projectileEntryArray;

	// Token: 0x04001DAF RID: 7599
	private static ProjectileLibrary m_instance;
}
