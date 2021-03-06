using System;
using System.Collections.Generic;

// Token: 0x0200006D RID: 109
public class Journal_EV
{
	// Token: 0x0600018A RID: 394 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
	public static JournalEntry[] GetJournalEntries(BiomeType biome, JournalType journalType)
	{
		return Journal_EV.GetJournalEntries(JournalType_RL.ConvertBiomeToJournalCategoryType(biome), journalType);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
	public static JournalEntry[] GetJournalEntries(JournalCategoryType journal, JournalType journalType)
	{
		JournalEntry[][] array;
		if (Journal_EV.m_journalEntryTable.TryGetValue(journal, out array) && array.GetLength(0) > (int)journalType)
		{
			return array[(int)journalType];
		}
		return null;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
	public static JournalEntry GetJournalEntry(BiomeType biome, JournalType journalType, int index)
	{
		return Journal_EV.GetJournalEntry(JournalType_RL.ConvertBiomeToJournalCategoryType(biome), journalType, index);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000E1F4 File Offset: 0x0000C3F4
	public static JournalEntry GetJournalEntry(JournalCategoryType journal, JournalType journalType, int index)
	{
		JournalEntry[] journalEntries = Journal_EV.GetJournalEntries(journal, journalType);
		if (journalEntries != null)
		{
			return journalEntries[index];
		}
		return default(JournalEntry);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000E21D File Offset: 0x0000C41D
	public static int GetNumJournals(BiomeType biome, JournalType journalType)
	{
		return Journal_EV.GetNumJournals(JournalType_RL.ConvertBiomeToJournalCategoryType(biome), journalType);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000E22C File Offset: 0x0000C42C
	public static int GetNumJournals(JournalCategoryType journal, JournalType journalType)
	{
		JournalEntry[] journalEntries = Journal_EV.GetJournalEntries(journal, journalType);
		if (journalEntries != null)
		{
			return journalEntries.Length;
		}
		return 0;
	}

	// Token: 0x0400039B RID: 923
	public const int HIGHEST_NUMBER_OF_ENTRIES = 20;

	// Token: 0x0400039C RID: 924
	private static Dictionary<JournalCategoryType, JournalEntry[][]> m_journalEntryTable = new Dictionary<JournalCategoryType, JournalEntry[][]>
	{
		{
			JournalCategoryType.Tutorial,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_TUTORIAL_FLAG_MARKER_1", "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_FLAG_MARKER_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_TUTORIAL_SERVANTS_SABOTAGE_1", "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_SERVANTS_SABOTAGE_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_TUTORIAL_TELEPORTER_TEST_1", "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_TELEPORTER_TEST_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_TUTORIAL_GROWING_ARMY_1", "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_GROWING_ARMY_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_TUTORIAL_SECRET_ENTRANCE_1", "LOC_ID_JOURNAL_DESCRIPTION_TUTORIAL_SECRET_ENTRANCE_1")
				}
			}
		},
		{
			JournalCategoryType.Castle,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_MAIN_REBEL_GOAL_1", "LOC_ID_JOURNAL_DESCRIPTION_MAIN_REBEL_GOAL_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_ROOT_HAZARDS_GROWING_1", "LOC_ID_JOURNAL_DESCRIPTION_ROOT_HAZARDS_GROWING_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_HEIRLOOM_DASH_TALK_1", "LOC_ID_JOURNAL_DESCRIPTION_HEIRLOOM_DASH_TALK_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_HEIRLOOM_MEMORY_TALK_1", "LOC_ID_JOURNAL_DESCRIPTION_HEIRLOOM_MEMORY_TALK_1")
				},
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_SPELLSWORD_FIGHTING_1", "LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_FIGHTING_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_SPELLSWORD_DUTIES_1", "LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_DUTIES_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_SPELLSWORD_DOUBLE_CROSSER_1", "LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_DOUBLE_CROSSER_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_SPELLSWORD_INSIGHT_CLUE_1", "LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_INSIGHT_CLUE_1")
				}
			}
		},
		{
			JournalCategoryType.Bridge,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_BRIDGE_PILING_BODIES_1", "LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_PILING_BODIES_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_BRIDGE_BODY_BURIAL_1", "LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_BODY_BURIAL_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_BRIDGE_BEASTS_1", "LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_BEASTS_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_BRIDGE_CLUE_1", "LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_CLUE_1")
				}
			}
		},
		{
			JournalCategoryType.Forest,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_FOREST_HIDDEN_DOUBLEJUMP_1", "LOC_ID_JOURNAL_DESCRIPTION_FOREST_HIDDEN_DOUBLEJUMP_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_FOREST_GIVE_UP_1", "LOC_ID_JOURNAL_DESCRIPTION_FOREST_GIVE_UP_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_FOREST_JOIN_1", "LOC_ID_JOURNAL_DESCRIPTION_FOREST_JOIN_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_FOREST_CONFIRM_1", "LOC_ID_JOURNAL_DESCRIPTION_FOREST_CONFIRM_1")
				},
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_DANCING_BOSS_GARDEN_1", "LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_GARDEN_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_DANCING_BOSS_BODIES_1", "LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_BODIES_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_DANCING_BOSS_STARVATION_1", "LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_STARVATION_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_DANCING_BOSS_TRAP_1", "LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_TRAP_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_DANCING_BOSS_CONSERVATORY_1", "LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_CONSERVATORY_1")
				}
			}
		},
		{
			JournalCategoryType.Study,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_ROOTS_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_ROOTS_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_NIBIRU_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_NIBIRU_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_CHANGING_STUDIES_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_CHANGING_STUDIES_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_CORDYCEP_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_CORDYCEP_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_DRASTIC_MEASURES_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_DRASTIC_MEASURES_1"),
					new JournalEntry("LOC_ID_JOURNAL_TITLE_STUDY_BOSS_MEMORY_CLUE_1", "LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_MEMORY_CLUE_1")
				},
				new JournalEntry[0]
			}
		},
		{
			JournalCategoryType.Tower,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_TEXT_SEEING_A_PSYCHIATRIST_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_TEXT_MY_MIND_IS_SHARP_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_TEXT_LIST_OF_WHAT_TO_DO_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_TEXT_DRAGON_SICK_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_TEXT_WHAT_IS_AN_ESTUARY_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_DESCRIPTION_TRUE_BELL_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MEMORY_DESCRIPTION_TOWER_BOSS_CLUE_1")
				},
				new JournalEntry[0]
			}
		},
		{
			JournalCategoryType.Cave,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_SETTING_UP_BASE_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_CANNOT_FIND_TUBAL_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_HORTICULTURE_NOTES_1"),
					new JournalEntry("LOC_ID_NAME_STUDY_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_ASH_CLOUDS_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_MISSING_WEAPONS_1"),
					new JournalEntry("LOC_ID_NAME_TOWER_BOSS_NOTES_1", "LOC_ID_MISC_JOURNAL_CAVE_HIDDEN_LOCATION_1")
				},
				new JournalEntry[0]
			}
		},
		{
			JournalCategoryType.PizzaGirl,
			new JournalEntry[][]
			{
				new JournalEntry[]
				{
					new JournalEntry("LOC_ID_JOURNAL_PIZZA_TITLE_1", "LOC_ID_JOURNAL_PIZZA_TEXT_1"),
					new JournalEntry("LOC_ID_JOURNAL_PIZZA_TITLE_2", "LOC_ID_JOURNAL_PIZZA_TEXT_2")
				}
			}
		}
	};
}
