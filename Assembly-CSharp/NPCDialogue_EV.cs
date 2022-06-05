using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007B RID: 123
public static class NPCDialogue_EV
{
	// Token: 0x060001B9 RID: 441 RVA: 0x0004B71C File Offset: 0x0004991C
	public static int GetNPCDialogueCooldown(NPCType npcType)
	{
		switch (npcType)
		{
		case NPCType.Blacksmith:
			return 0;
		case NPCType.Enchantress:
			return 0;
		case NPCType.Architect:
			return 0;
		case NPCType.Charon:
			return 4;
		case NPCType.Dummy:
			return 0;
		case NPCType.PizzaGirl:
			return 3;
		case NPCType.Totem:
			return 0;
		case NPCType.LivingSafe:
			return 0;
		case NPCType.Dragon:
			return 0;
		}
		return 0;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0004B77C File Offset: 0x0004997C
	public static string GetNPCTitleLocID(NPCType npcType)
	{
		switch (npcType)
		{
		case NPCType.Blacksmith:
			return "LOC_ID_NAME_BLACKSMITH_1";
		case NPCType.Enchantress:
			return "LOC_ID_NAME_ENCHANTRESS_1";
		case NPCType.Architect:
			return "LOC_ID_NAME_ARCHITECT_1";
		case NPCType.Charon:
			return "LOC_ID_NAME_CHARON_1";
		case NPCType.Dummy:
			return "LOC_ID_NAME_DUMMY_1";
		case NPCType.PizzaGirl:
			return "LOC_ID_NAME_PIZZA_GIRL_1";
		case NPCType.Totem:
			if (!NPCDialogueManager.SageTotemNewNameRevealed())
			{
				return "LOC_ID_NAME_MASTERY_TOTEM_1";
			}
			return "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1";
		case NPCType.LivingSafe:
			return "LOC_ID_NAME_LIVING_SAFE_1";
		case NPCType.ChallengeHood:
			return "LOC_ID_NAME_GREEN_HOOD_1";
		case NPCType.NewGamePlusHood:
			return "LOC_ID_NAME_RED_HOOD_1";
		case NPCType.SoulShopHood:
			return "LOC_ID_NAME_SOULSHOP_HOOD_1";
		case NPCType.Johan:
			return "LOC_ID_NAME_J_NICKNAME_1";
		case NPCType.Dragon:
			return "LOC_ID_NAME_DRAGON_1";
		}
		return null;
	}

	// Token: 0x040003E2 RID: 994
	public static Vector2Int MAX_DIALOGUE_HUB_TOWN = new Vector2Int(2, 3);

	// Token: 0x040003E3 RID: 995
	public const int MAX_DIALOGUE_KINGDOM = 99;

	// Token: 0x040003E4 RID: 996
	public const int GLOBAL_DIALOGUE_CD = 0;

	// Token: 0x040003E5 RID: 997
	public const bool IGNORE_CHAR_DIALOGUE_CD_IN_TEXT_CHAINS = false;

	// Token: 0x040003E6 RID: 998
	public const bool IGNORE_GLOBAL_DIALOGUE_CD_IN_TEXT_CHAINS = true;

	// Token: 0x040003E7 RID: 999
	public static Dictionary<NPCType, NPCDialogueEntry[]> NPCDialogueTable = new Dictionary<NPCType, NPCDialogueEntry[]>
	{
		{
			NPCType.Blacksmith,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_SMELLS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ALL_MY_LIFE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_FORGE_JOKE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WORDS_AND_METALS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.TotemUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WHITE_SMITH_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_TALK_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOOKING_GOOD_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.ArchitectUnlocked,
					NPCDialogueCondition.EnchantressUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_REPAIR_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.EnchantressUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_DATE_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_INTERESTED_IN_BLACKSMITH_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_DELICIOUS_MEAL_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_DATE_WENT_WELL_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_HEADING_OUT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_APPRENTICE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_MASTER_SMITH_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_FORMING_CORE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_LIVING_SAFE_PIECES_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LivingSafeUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_COOKING_FOR_ENCHANTRESS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ARCHITECT_AGE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.ArchitectUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WINDOW_SHOPPING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_PIZZA_GIRL_SLEEP_OVER_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.PizzaGirlUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_STILL_LOOKING_FOR_APPRENTICE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_COME_FOR_DINNER_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_PIZZA_GIRL_CHARISMA_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_GIFT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_FEEL_SORRY_ENCHANTRESS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_BAD_BACK_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_STABILIZING_TOWER_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.DummyUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_HELP_WITH_APHANTASIA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_CHARONS_SCYTHE_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RESTLESS_LEG_FLARE_UP_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.CharonUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_LEARNING_HOW_TO_KNIT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_REASONS_FOR_KNITTING_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_MADE_SOME_SOUP_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_CHANGE_TO_BLANKET_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_HAPPY_DOCKS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOVELY_DAY_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WEDDING_RING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WHEN_TO_HOLD_WEDDING_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_ROCK_PAPER_SCISSORS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WONDER_WHAT_WORLD_IS_LIKE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_KEEP_BEING_GOOD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Enchantress,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_CAN_SEE_YOUR_SOUL_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_MORNING_TEA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_FATTAH_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RED_AETHER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_NO_MIST_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LivingSafeUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_TOTEM_DOES_NOT_EAT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.TotemUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOOKING_GOOD_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WHITE_SMITH_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.ArchitectUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_INTERESTED_IN_BLACKSMITH_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_REPAIR_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_DATE_WENT_WELL_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_DATE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_DUMMY_DOES_NOT_EAT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.DummyUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_SLEEP_APNEA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOVE_PILLOWS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RESTLESS_LEG_EXPLAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_APNEA_PILLOWS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_PIZZA_GIRL_CHARISMA_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_STILL_LOOKING_FOR_APPRENTICE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.PizzaGirlUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_BAD_BACK_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_ENCHANTRESS_GIFT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_NICKNAME_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_BABA_GANOUSH_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RESTLESS_LEG_FLARE_UP_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_HELP_WITH_APHANTASIA_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_MADE_SOME_SOUP_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_LEARNING_HOW_TO_KNIT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_WEIGHTED_BLANKET_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_CHANGE_TO_BLANKET_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOVELY_DAY_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_WEIGHTED_BLANKET_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RING_ORIGIN_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_BLACKSMITH_WEDDING_RING_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_ROCK_PAPER_SCISSORS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_RING_MEANING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LIKE_IT_HERE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_MORE_FOOD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_LOVE_YOU_EVERYDAY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Architect,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_ONCE_A_BARD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_TALE_OF_THE_FOX_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_LOCK_CASTLE_AGAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_TALE_OF_THE_TURTLE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_LUCK_AND_TALENT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_LOVES_PIZZA_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.PizzaGirlUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_RED_AETHER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_GOOD_DAY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_TALE_OF_THE_ELEPHANT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_LIVING_SAFE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LivingSafeUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_TALE_OF_THE_MOUSE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_NOT_MANY_STORIES_LEFT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_TALE_OF_THE_MONKEY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_POINT_OF_STORIES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_IS_THIS_AN_ANCHOR_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_BLACKSMITH_FIXED_GLASSES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_FATHER_TELLS_STORIES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_NO_MORE_STORIES_FOR_NOW_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_LONG_DAY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_ARCHITECT_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Charon,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_REPAY_THE_TOLL_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_CHEAT_DEATH_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_WELCOME_AGAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_ANOTHER_TRAVELLER_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.PizzaGirlUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_PIZZA_GIRL_AND_YOU_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_THANKS_FOR_LISTENING_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_PIZZA_GIRL_BARF_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PREGNANCY_SEA_SICK_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_PERSISTANCE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_NO_BARFING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_FAR_SHORES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_JOURNEY_TOGETHER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_ORZO_SOUP_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_ENCHANTRESS_MADE_SOME_SOUP_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_THANK_ENCHANTRESS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.EnchantressUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_SEE_YOU_AGAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_GOOD_TO_SEE_YOU_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_BLACKSMITH_BLADE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_PIZZA_GIRL_POOR_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_SAME_SHORES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_MORE_PIZZA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_JOURNEY_AGAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_CHARON_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Dummy,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_TANNING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_TENDRILS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_PIZZA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SPELLSWORD_FIGHTING_MASTER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_HI_STUTTER_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_WHATS_MY_NAME_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SUNDAYS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_LIVING_TOTEM_DATE_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_ALSO_LIKES_SUNDAYS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_WONDER_WHY_WEAPONS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SPELLSWORD_FIRST_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_DONT_SWEAT_THE_DETAILS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_PIZZA_GIRL_WEAPONS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_NOTICED_ARCHITECT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.PizzaGirlUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SECOND_DATE_WITH_TOTEM_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SECOND_SUCCESS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_DINNER_DATE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_HEAD_IN_CLOUDS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_SWEDISH_QUOTE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_NICKNAME_FOR_TOTEM_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_ARCHITECT_TALKING_TALES_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.ArchitectUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_WORKING_ON_STUTTER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_THINK_AND_ENUNCIATE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_MINDFUL_SPEAKING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_IRAD_BRAINS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SEE_THE_WORLD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_HELLO_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_THE_GREAT_GENERAL_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_LIVING_SAFE_MONEY_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LivingSafeUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_READING_EVERY_NIGHT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SPENT_THE_NIGHT_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_LOOKING_GOOD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.LivingSafe,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_INVESTMENTS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_BUILD_SKILL_TREE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_CHANGE_HIS_LOOK_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_USED_TO_BE_A_DAD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_HELPING_DUMMY_INVESTMENT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.DummyUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_USED_TO_TELL_STORIES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_LEAVING_FOR_MONEY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_WONT_SHOW_FACE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_WHY_CALLED_ARCHITECT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.ArchitectUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_WIFE_PASSED_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_BUYING_RED_AETHER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_LOOKING_WEALTHY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_GIVES_AWAY_ALL_THE_RED_AETHER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_ONLY_GOOD_JOB_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_CHANGE_IN_PERSPECTIVE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_LAZING_IN_THE_SUN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_GREET_HIS_SON_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_FRIEND_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_LIVING_SAFE_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Totem,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_CULTURAL_APPROPRIATION_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_ENJOYING_DOCKS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_FRENCH_QUOTE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_GERMAN_QUOTE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_CHINESE_QUOTE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_SPIRIT_WOOD_GREETING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_WHO_AM_I_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_WHATS_MY_NAME_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.DummyUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_INTERESTED_IN_DUMMY_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_HI_STUTTER_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_ALSO_LIKES_SUNDAYS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SUNDAYS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_DATE_SUCCESS_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_LIVING_TOTEM_DATE_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_DATE_FAILURE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_JAPAN_FOREVER_ALONE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_NOTICED_ARCHITECT_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_DONT_SWEAT_THE_DETAILS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed,
					NPCDialogueCondition.ArchitectUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_DINNER_DATE_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SECOND_DATE_WITH_TOTEM_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_GREAT_DATE_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_SECOND_SUCCESS_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_SWEDISH_QUOTE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_MY_NEW_NAME_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "LOC_ID_HUB_TOWN_DIALOGUE_DUMMY_NICKNAME_FOR_TOTEM_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_CONFIDENCE_RETURNED_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_LIKES_THE_DOCKS_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_PERFECT_NAME_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_BLACKSMITH_GIFT_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.BlacksmithUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_LUB_DUB_QUOTE_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_PERFECT_PARTNER_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_POSITIVE_REINFORCEMENT_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_QUINN_NAME_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_SUNNY_DAY_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_TOTEM_REWARD_1", "LOC_ID_NAME_MASTERY_TOTEM_NICKNAME_1", "", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.PizzaGirl,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_ENJOY_COMPANY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_FEEL_SOIL_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GARDENERS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GOOD_FEELING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_TAKING_SHIFTING_FOR_GRANTED_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_THANKS_FOR_LISTENING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_LONELY_RETURN_1", "", "LOC_ID_HUB_TOWN_DIALOGUE_CHARON_PIZZA_GIRL_AND_YOU_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LocIDPlayed
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PREGNANCY_SEA_SICK_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GATHERING_REGRETFUL_PEASANTS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GETTING_PIZZA_JOB_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PREGNANCY_FOOD_GENEROSITY_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.EnchantressUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PIZZA_FOOD_STONE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_OPENING_AN_ACCOUNT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.LivingSafeUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GIVE_ME_HOPE_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_ASK_TOTEM_QUIET_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.TotemUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PIZZA_LIFE_DIRECTION_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_BLACKSMITH_BELT_1", new NPCDialogueCondition[]
				{
					NPCDialogueCondition.BlacksmithUnlocked
				}),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_NICE_DAY_FOR_PIZZA_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_PIZZA_SHIFTING_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_STUPID_STORIES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_NAME_HISTORY_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_GLAD_TO_BE_STUCK_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_RIDE_AGAIN_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_REWARD_1", Array.Empty<NPCDialogueCondition>())
			}
		},
		{
			NPCType.Dragon,
			new NPCDialogueEntry[]
			{
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_BEATEN_BY_IRAD_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_CANNOT_GROW_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_NO_FLAMES_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_ORIGINAL_KEEPER_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_TUBALS_KILLERS_1", Array.Empty<NPCDialogueCondition>()),
				new NPCDialogueEntry("LOC_ID_DRAGON_NPC_REWARD_OF_SOULS_1", Array.Empty<NPCDialogueCondition>())
			}
		}
	};

	// Token: 0x040003E8 RID: 1000
	public static List<NPCType> NPCDialogueOrderList = new List<NPCType>
	{
		NPCType.Charon,
		NPCType.PizzaGirl,
		NPCType.Blacksmith,
		NPCType.Enchantress,
		NPCType.Dummy,
		NPCType.Totem,
		NPCType.Architect,
		NPCType.LivingSafe
	};

	// Token: 0x040003E9 RID: 1001
	public static string[] PIZZA_GIRL_HUBTOWN_GENERIC_DIALOGUES = new string[]
	{
		"LOC_ID_DIALOGUE_TELEPORTER_NPC_TOWN_TALK_REPEAT_1",
		"LOC_ID_DIALOGUE_TELEPORTER_NPC_TOWN_TALK_REPEAT_2",
		"LOC_ID_DIALOGUE_TELEPORTER_NPC_TOWN_TALK_REPEAT_3",
		"LOC_ID_DIALOGUE_TELEPORTER_NPC_TOWN_TALK_REPEAT_4"
	};

	// Token: 0x040003EA RID: 1002
	public static string[] NEW_GAME_PLUS_ACTIVATE_DIALOGUES = new string[]
	{
		"LOC_ID_NG_TEXT_CHANGING_DIMENSIONS_1",
		"LOC_ID_NG_TEXT_CHANGING_DIMENSIONS_2",
		"LOC_ID_NG_TEXT_CHANGING_DIMENSIONS_3",
		"LOC_ID_NG_TEXT_CHANGING_DIMENSIONS_4"
	};
}
