using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class Heirloom_EV
{
	// Token: 0x06000181 RID: 385 RVA: 0x0000D16C File Offset: 0x0000B36C
	public static HeirloomDialogueEntry GetDialogueEntry(HeirloomType heirloomType)
	{
		HeirloomDialogueEntry result;
		if (Heirloom_EV.m_heirloomDialogueTable.TryGetValue(heirloomType, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000D18C File Offset: 0x0000B38C
	public static bool IsHeirloomLocked(HeirloomType heirloomType)
	{
		if (heirloomType != HeirloomType.UnlockBouncableDownstrike)
		{
			return heirloomType == HeirloomType.UnlockVoidDash && (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) <= 0 || SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) <= 0);
		}
		return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) <= 0;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000D1DD File Offset: 0x0000B3DD
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

	// Token: 0x04000381 RID: 897
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

	// Token: 0x04000382 RID: 898
	public static readonly Color CAVE_LANTERN_DARKNESS_COLOR_DIM = new Color(0.03f, 0.03f, 0.03f);

	// Token: 0x04000383 RID: 899
	public const float CAVE_LANTERN_DARKNESS_AMOUNT_DIM = 0.73f;

	// Token: 0x04000384 RID: 900
	public const float CAVE_LANTERN_DARKNESS_SOFTNESS_DIM = 0.13f;

	// Token: 0x04000385 RID: 901
	public static readonly Color CAVE_LANTERN_DARKNESS_COLOR_LIT = new Color(0.03f, 0.03f, 0.03f);

	// Token: 0x04000386 RID: 902
	public const float CAVE_LANTERN_DARKNESS_AMOUNT_LIT = 0.45f;

	// Token: 0x04000387 RID: 903
	public const float CAVE_LANTERN_DARKNESS_SOFTNESS_LIT = 0.31f;

	// Token: 0x04000388 RID: 904
	public const float CAVE_LANTERN_BURDEN_VIEW_MOD = 0.05f;

	// Token: 0x04000389 RID: 905
	public const float CAVE_LANTERN_BURDEN_VIEW_CAP = 0.25f;
}
