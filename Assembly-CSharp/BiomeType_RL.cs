using System;

// Token: 0x02000BB7 RID: 2999
public class BiomeType_RL
{
	// Token: 0x17001E1D RID: 7709
	// (get) Token: 0x06005A17 RID: 23063 RVA: 0x00031331 File Offset: 0x0002F531
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

	// Token: 0x06005A18 RID: 23064 RVA: 0x00031358 File Offset: 0x0002F558
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

	// Token: 0x06005A19 RID: 23065 RVA: 0x0003138C File Offset: 0x0002F58C
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

	// Token: 0x04004536 RID: 17718
	private static BiomeType[] m_typeArray;
}
