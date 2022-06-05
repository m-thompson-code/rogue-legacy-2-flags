using System;

// Token: 0x02000C08 RID: 3080
public class JournalType_RL
{
	// Token: 0x06005A8D RID: 23181 RVA: 0x00155EBC File Offset: 0x001540BC
	public static JournalCategoryType ConvertBiomeToJournalCategoryType(BiomeType biome)
	{
		if (biome <= BiomeType.ForestBottom)
		{
			if (biome == BiomeType.Castle)
			{
				return JournalCategoryType.Castle;
			}
			if (biome == BiomeType.Cave)
			{
				return JournalCategoryType.Cave;
			}
			switch (biome)
			{
			case BiomeType.Forest:
			case BiomeType.ForestTop:
			case BiomeType.ForestBottom:
				return JournalCategoryType.Forest;
			}
		}
		else if (biome <= BiomeType.Study)
		{
			if (biome == BiomeType.Stone)
			{
				return JournalCategoryType.Bridge;
			}
			if (biome == BiomeType.Study)
			{
				return JournalCategoryType.Study;
			}
		}
		else if (biome == BiomeType.Tower || biome == BiomeType.TowerExterior)
		{
			return JournalCategoryType.Tower;
		}
		return JournalCategoryType.None;
	}

	// Token: 0x06005A8E RID: 23182 RVA: 0x000319AE File Offset: 0x0002FBAE
	public static BiomeType ConvertJournalCategoryTypeToBiome(JournalCategoryType journalCategoryType)
	{
		if (journalCategoryType <= JournalCategoryType.Study)
		{
			if (journalCategoryType == JournalCategoryType.Castle)
			{
				return BiomeType.Castle;
			}
			if (journalCategoryType == JournalCategoryType.Forest)
			{
				return BiomeType.Forest;
			}
			if (journalCategoryType == JournalCategoryType.Study)
			{
				return BiomeType.Study;
			}
		}
		else
		{
			if (journalCategoryType == JournalCategoryType.Bridge)
			{
				return BiomeType.Stone;
			}
			if (journalCategoryType == JournalCategoryType.Tower)
			{
				return BiomeType.Tower;
			}
			if (journalCategoryType == JournalCategoryType.Cave)
			{
				return BiomeType.Cave;
			}
		}
		return BiomeType.None;
	}

	// Token: 0x17001E33 RID: 7731
	// (get) Token: 0x06005A8F RID: 23183 RVA: 0x000319ED File Offset: 0x0002FBED
	public static JournalCategoryType[] CategoryTypeArray
	{
		get
		{
			if (JournalType_RL.m_categoryTypeArray == null)
			{
				JournalType_RL.m_categoryTypeArray = (Enum.GetValues(typeof(JournalCategoryType)) as JournalCategoryType[]);
			}
			return JournalType_RL.m_categoryTypeArray;
		}
	}

	// Token: 0x17001E34 RID: 7732
	// (get) Token: 0x06005A90 RID: 23184 RVA: 0x00031A14 File Offset: 0x0002FC14
	public static JournalCategoryType[] SortedCategoryTypeArray
	{
		get
		{
			if (JournalType_RL.m_sortedCategoryTypeArray == null)
			{
				JournalType_RL.m_sortedCategoryTypeArray = new JournalCategoryType[]
				{
					JournalCategoryType.None,
					JournalCategoryType.Castle,
					JournalCategoryType.Bridge,
					JournalCategoryType.Forest,
					JournalCategoryType.Study,
					JournalCategoryType.Tower,
					JournalCategoryType.Cave
				};
			}
			return JournalType_RL.m_sortedCategoryTypeArray;
		}
	}

	// Token: 0x04004762 RID: 18274
	private static JournalCategoryType[] m_categoryTypeArray;

	// Token: 0x04004763 RID: 18275
	private static JournalCategoryType[] m_sortedCategoryTypeArray;
}
