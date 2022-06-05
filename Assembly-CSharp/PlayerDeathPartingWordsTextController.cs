using System;
using TMPro;
using UnityEngine;

// Token: 0x02000672 RID: 1650
public class PlayerDeathPartingWordsTextController : MonoBehaviour
{
	// Token: 0x17001361 RID: 4961
	// (get) Token: 0x06003242 RID: 12866 RVA: 0x0001B971 File Offset: 0x00019B71
	public TMP_Text Text
	{
		get
		{
			return this.m_partingWordsText;
		}
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x000D6D84 File Offset: 0x000D4F84
	public void UpdateMessage(bool isVictory)
	{
		if (!isVictory)
		{
			string text;
			if (this.m_partingWordsLocIDArray != null && this.m_partingWordsLocIDArray.Length != 0)
			{
				if (PlayerManager.GetCurrentPlayerRoom().SpecialRoomType == SpecialRoomType.Heirloom && (SaveManager.PlayerSaveData.CurrentCharacter.TraitOne == TraitType.CantAttack || SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo == TraitType.CantAttack) && (PlayerManager.GetCurrentPlayerRoom() as Room).GridPointManager.RoomMetaData.RoomPath.Contains("Levels Arena Challenge"))
				{
					text = LocalizationManager.GetString("LOC_ID_ARENA_MENU_ARENA_DEATH_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				}
				else
				{
					this.m_partingWordsIndex = UnityEngine.Random.Range(0, this.m_partingWordsLocIDArray.Length);
					text = LocalizationManager.GetString(this.m_partingWordsLocIDArray[this.m_partingWordsIndex], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				}
			}
			else
			{
				text = "A tip to help you out next time";
			}
			this.m_partingWordsText.text = text;
			return;
		}
		this.m_partingWordsText.text = LocalizationManager.GetString("LOC_ID_ENDING_VICTORY_DEATH_HINT_TEXT_OVERRIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x000D6E9C File Offset: 0x000D509C
	public void CycleUpdateMessage(bool cycleUp)
	{
		if (cycleUp)
		{
			this.m_partingWordsIndex++;
		}
		else
		{
			this.m_partingWordsIndex--;
		}
		if (this.m_partingWordsIndex < -1)
		{
			this.m_partingWordsIndex = this.m_partingWordsLocIDArray.Length - 1;
		}
		else if (this.m_partingWordsIndex >= this.m_partingWordsLocIDArray.Length)
		{
			this.m_partingWordsIndex = -1;
		}
		if (this.m_partingWordsIndex == -1)
		{
			this.m_partingWordsText.text = LocalizationManager.GetString("LOC_ID_ENDING_VICTORY_DEATH_HINT_TEXT_OVERRIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		this.m_partingWordsText.text = LocalizationManager.GetString(this.m_partingWordsLocIDArray[this.m_partingWordsIndex], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x04002900 RID: 10496
	[SerializeField]
	private TMP_Text m_partingWordsText;

	// Token: 0x04002901 RID: 10497
	private int m_partingWordsIndex;

	// Token: 0x04002902 RID: 10498
	private string[] m_partingWordsLocIDArray = new string[]
	{
		"LOC_ID_TUTORIAL_HINTS_BOSS_CD_1",
		"LOC_ID_TUTORIAL_HINTS_EQUIPMENT_1",
		"LOC_ID_TUTORIAL_HINTS_UPGRADING_EQUIPMENT_1",
		"LOC_ID_TUTORIAL_HINTS_UNITY_EXPLANATION_1",
		"LOC_ID_TUTORIAL_HINTS_FLAMEBARRIER_ROTATION_1",
		"LOC_ID_TUTORIAL_HINTS_SPINKICK_SIDE_RANGE_1",
		"LOC_ID_TUTORIAL_HINTS_MAGIC_DAMAGE_MEAT_1",
		"LOC_ID_TUTORIAL_HINTS_DASH_RECOVERY_1",
		"LOC_ID_TUTORIAL_HINTS_ENEMY_LEVELING_1",
		"LOC_ID_TUTORIAL_HINTS_DODGE_PRACTICE_1",
		"LOC_ID_TUTORIAL_HINTS_LOOK_FOR_CHESTS_1",
		"LOC_ID_TUTORIAL_HINTS_FINDING_RUNE_ORE_1",
		"LOC_ID_TUTORIAL_HINTS_TELEPORTER_INVINCIBILITY_1",
		"LOC_ID_TUTORIAL_HINTS_FINDING_EQUIPMENT_ORE_1",
		"LOC_ID_TUTORIAL_HINTS_BOSS_WEAKNESSES_1",
		"LOC_ID_TUTORIAL_HINTS_HEIRLOOM_EXPLANATIONS_1",
		"LOC_ID_TUTORIAL_HINTS_EXPLORE_1",
		"LOC_ID_TUTORIAL_HINTS_RESOLVE_LIGHT_1",
		"LOC_ID_TUTORIAL_HINTS_ARCHITECT_BOSSES_1",
		"LOC_ID_TUTORIAL_HINTS_HIGH_GOLD_MULTIPLIERS_1",
		"LOC_ID_TUTORIAL_HINTS_HOUSE_RULES_1",
		"LOC_ID_TUTORIAL_HINTS_RESOLVE_ECONOMY_1",
		"LOC_ID_TUTORIAL_HINTS_SWORD_DASH_ATTACK_1",
		"LOC_ID_TUTORIAL_HINTS_SWORD_BACK_STEP_1",
		"LOC_ID_TUTORIAL_HINTS_SHIELD_BLOCK_REMINDER_1",
		"LOC_ID_TUTORIAL_HINTS_SHIELD_HOLD_BLOCK_1",
		"LOC_ID_TUTORIAL_HINTS_SHIELD_PERFECT_BLOCK_1",
		"LOC_ID_TUTORIAL_HINTS_SHIELD_DASH_BLOCK_1",
		"LOC_ID_TUTORIAL_HINTS_SHIELD_BLOCK_VULNERABLE_1",
		"LOC_ID_TUTORIAL_HINTS_CHAKRAM_BOUNCE_1",
		"LOC_ID_TUTORIAL_HINTS_CHAKRAM_MULTIPLE_WEAPONS_1",
		"LOC_ID_TUTORIAL_HINTS_BOW_PERFECT_SHOT_1",
		"LOC_ID_TUTORIAL_HINTS_BOW_AIR_TIME_1",
		"LOC_ID_TUTORIAL_HINTS_BOW_ARC_1",
		"LOC_ID_TUTORIAL_HINTS_CREATE_PLATFORM_STATUS_EFFECT_1",
		"LOC_ID_TUTORIAL_HINTS_CREATE_PLATFORM_BLOCKS_PROJECTILES_1",
		"LOC_ID_TUTORIAL_HINTS_CREATE_PLATFORM_SIDE_WALLS_1"
	};
}
