using System;

// Token: 0x0200074A RID: 1866
public class JournalType_RL
{
	// Token: 0x06004110 RID: 16656 RVA: 0x000E65C0 File Offset: 0x000E47C0
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

	// Token: 0x06004111 RID: 16657 RVA: 0x000E6631 File Offset: 0x000E4831
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

	// Token: 0x17001637 RID: 5687
	// (get) Token: 0x06004112 RID: 16658 RVA: 0x000E6670 File Offset: 0x000E4870
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

	// Token: 0x17001638 RID: 5688
	// (get) Token: 0x06004113 RID: 16659 RVA: 0x000E6697 File Offset: 0x000E4897
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

	// Token: 0x040034E6 RID: 13542
	private static JournalCategoryType[] m_categoryTypeArray;

	// Token: 0x040034E7 RID: 13543
	private static JournalCategoryType[] m_sortedCategoryTypeArray;
}
