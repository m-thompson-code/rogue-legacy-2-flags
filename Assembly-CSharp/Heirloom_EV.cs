using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class Heirloom_EV
{
	// Token: 0x06000195 RID: 405 RVA: 0x000497B4 File Offset: 0x000479B4
	public static HeirloomDialogueEntry GetDialogueEntry(HeirloomType heirloomType)
	{
		HeirloomDialogueEntry result;
		if (Heirloom_EV.m_heirloomDialogueTable.TryGetValue(heirloomType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x000497D4 File Offset: 0x000479D4
	public static bool IsHeirloomLocked(HeirloomType heirloomType)
	{
		if (heirloomType != HeirloomType.UnlockBouncableDownstrike)
		{
			return heirloomType == HeirloomType.UnlockVoidDash && (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) <= 0 || SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) <= 0);
		}
		return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) <= 0;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00003A4E File Offset: 0x00001C4E
	public static string GetLockedHeirloomLocID(HeirloomType heirloomType, bool isRepeat)
	{
		if (heirloomType != HeirloomType.UnlockBouncableDownstrike)
		{
			if (heirloomType != HeirloomType.UnlockVoidDash)
			{
				return null;
			}
			return "LOC_ID_HEIRLOOM_TEXT_VOID_DASH_NO_ABILITY_1";
		}
		else
		{
			if (!isRepeat)
			{
				return "LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_NO_ABILITY_1";
			}
			return "LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_NO_ABILITY_REPEAT_1";
		}
	}

	// Token: 0x040003A2 RID: 930
	private static Dictionary<HeirloomType, HeirloomDialogueEntry> m_heirloomDialogueTable = new Dictionary<HeirloomType, HeirloomDialogueEntry>
	{
		{
			HeirloomType.UnlockAirDash,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DASH_INTRO_1", "LOC_ID_HEIRLOOM_TEXT_DASH_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DASH_INTRO_REPEAT_1", "LOC_ID_HEIRLOOM_TEXT_DASH_INTRO_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DASH_END_1", "LOC_ID_HEIRLOOM_TEXT_DASH_END_1"),
				RandomizeRepeatDialogues = true
			}
		},
		{
			HeirloomType.UnlockMemory,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_MEMORY_INTRO_1", "LOC_ID_HEIRLOOM_TEXT_MEMORY_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_MEMORY_INTRO_REPEAT_1", "LOC_ID_HEIRLOOM_TEXT_MEMORY_INTRO_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_MEMORY_END_1", "LOC_ID_HEIRLOOM_TEXT_MEMORY_END_1"),
				RandomizeRepeatDialogues = true
			}
		},
		{
			HeirloomType.UnlockBouncableDownstrike,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_SPIN_KICK_INTRO_1", "LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_SPIN_KICK_INTRO_REPEAT_1", "LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_INTRO_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_SPIN_KICK_END_1", "LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_END_1"),
				RandomizeRepeatDialogues = true
			}
		},
		{
			HeirloomType.UnlockDoubleJump,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DOUBLE_JUMP_INTRO_1", "LOC_ID_HEIRLOOM_TEXT_DOUBLE_JUMP_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DOUBLE_JUMP_REPEAT_1", "LOC_ID_HEIRLOOM_TEXT_DOUBLE_JUMP_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_DOUBLE_JUMP_END_1", "LOC_ID_HEIRLOOM_TEXT_DOUBLE_JUMP_END_1"),
				RandomizeRepeatDialogues = true
			}
		},
		{
			HeirloomType.UnlockVoidDash,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_VOID_DASH_INTRO_1", "LOC_ID_HEIRLOOM_TEXT_VOID_DASH_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_VOID_DASH_INTRO_REPEAT_1", "LOC_ID_HEIRLOOM_TEXT_VOID_DASH_INTRO_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_HEIRLOOM_TITLE_VOID_DASH_END_1", "LOC_ID_HEIRLOOM_TEXT_VOID_DASH_END_1"),
				RandomizeRepeatDialogues = true
			}
		},
		{
			HeirloomType.UnlockEarthShift,
			new HeirloomDialogueEntry
			{
				HeirloomDialogue = new HeirloomTextEntry[]
				{
					new HeirloomTextEntry("LOC_ID_NAME_EARTH_SHIFT_NAME_1", "LOC_ID_HEIRLOOM_TEXT_EARTH_SHIFT_INTRO_1"),
					new HeirloomTextEntry("LOC_ID_NAME_EARTH_SHIFT_NAME_1", "LOC_ID_HEIRLOOM_TEXT_EARTH_SHIFT_INTRO_REPEAT_1")
				},
				HeirloomCompleteDialogue = new HeirloomTextEntry("LOC_ID_NAME_EARTH_SHIFT_NAME_1", "LOC_ID_HEIRLOOM_TEXT_EARTH_SHIFT_END_1"),
				RandomizeRepeatDialogues = true
			}
		}
	};

	// Token: 0x040003A3 RID: 931
	public static readonly Color CAVE_LANTERN_DARKNESS_COLOR_DIM = new Color(0.03f, 0.03f, 0.03f);

	// Token: 0x040003A4 RID: 932
	public const float CAVE_LANTERN_DARKNESS_AMOUNT_DIM = 0.73f;

	// Token: 0x040003A5 RID: 933
	public const float CAVE_LANTERN_DARKNESS_SOFTNESS_DIM = 0.13f;

	// Token: 0x040003A6 RID: 934
	public static readonly Color CAVE_LANTERN_DARKNESS_COLOR_LIT = new Color(0.03f, 0.03f, 0.03f);

	// Token: 0x040003A7 RID: 935
	public const float CAVE_LANTERN_DARKNESS_AMOUNT_LIT = 0.45f;

	// Token: 0x040003A8 RID: 936
	public const float CAVE_LANTERN_DARKNESS_SOFTNESS_LIT = 0.31f;

	// Token: 0x040003A9 RID: 937
	public const float CAVE_LANTERN_BURDEN_VIEW_MOD = 0.05f;

	// Token: 0x040003AA RID: 938
	public const float CAVE_LANTERN_BURDEN_VIEW_CAP = 0.25f;
}
