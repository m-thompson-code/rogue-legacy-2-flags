using System;

// Token: 0x02000704 RID: 1796
public class BiomeType_RL
{
	// Token: 0x17001621 RID: 5665
	// (get) Token: 0x060040CE RID: 16590 RVA: 0x000E579E File Offset: 0x000E399E
	public static BiomeType[] TypeArray
	{
		get
		{
			if (BiomeType_RL.m_typeArray == null)
			{
				BiomeType_RL.m_typeArray = (Enum.GetValues(typeof(BiomeType)) as BiomeType[]);
			}
			return BiomeType_RL.m_typeArray;
		}
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x000E57C5 File Offset: 0x000E39C5
	public static BiomeType GetGroupedBiomeType(BiomeType biomeType)
	{
		if (biomeType <= BiomeType.CaveBottom)
		{
			if (biomeType == BiomeType.CaveMiddle || biomeType == BiomeType.CaveBottom)
			{
				return BiomeType.Cave;
			}
		}
		else
		{
			if (biomeType == BiomeType.ForestTop || biomeType == BiomeType.ForestBottom)
			{
				return BiomeType.Forest;
			}
			if (biomeType == BiomeType.TowerExterior)
			{
				return BiomeType.Tower;
			}
		}
		return biomeType;
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x000E57F9 File Offset: 0x000E39F9
	public static bool IsValidBiome(BiomeType biomeType)
	{
		if (biomeType <= BiomeType.Lineage)
		{
			if (biomeType != BiomeType.None && biomeType != BiomeType.Editor && biomeType != BiomeType.Lineage)
			{
				return true;
			}
		}
		else if (biomeType != BiomeType.Spawn && biomeType != BiomeType.Special && biomeType != BiomeType.Any)
		{
			return true;
		}
		return false;
	}

	// Token: 0x040032BB RID: 12987
	private static BiomeType[] m_typeArray;
}
