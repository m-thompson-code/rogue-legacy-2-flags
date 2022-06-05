using System;
using System.Collections.Generic;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class DummyDialogueController : MonoBehaviour, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17001247 RID: 4679
	// (get) Token: 0x060031F3 RID: 12787 RVA: 0x000A9016 File Offset: 0x000A7216
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17001248 RID: 4680
	// (get) Token: 0x060031F4 RID: 12788 RVA: 0x000A901E File Offset: 0x000A721E
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001249 RID: 4681
	// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000A9021 File Offset: 0x000A7221
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000A9029 File Offset: 0x000A7229
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DummyDialogue_Intro);
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000A9068 File Offset: 0x000A7268
	private void Awake()
	{
		foreach (ClassType classType in ClassType_RL.TypeArray)
		{
			if (classType != ClassType.None)
			{
				this.m_dialogueIndexDict.Add(classType, 0);
			}
		}
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_closeDummyTips = new Action(this.CloseDummyTips);
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000A90C7 File Offset: 0x000A72C7
	private void OnEnable()
	{
		this.m_endingSpeechBubblePlayed = false;
		if (PlayerManager.GetCurrentPlayerRoom().BiomeType == BiomeType.Garden)
		{
			this.m_speechBubble.DisplayOffscreen = false;
			return;
		}
		this.m_speechBubble.DisplayOffscreen = true;
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x000A90FC File Offset: 0x000A72FC
	public void ActivateDialogue()
	{
		if (PlayerManager.IsInstantiated)
		{
			this.m_interactable.SetIsInteractableActive(false);
			AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
			if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
			{
				this.RunEndingDialogue();
				return;
			}
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DummyDialogue_Intro))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DummyDialogue_Intro, true);
				DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
				DialogueManager.AddDialogue("LOC_ID_DUMMY_DIALOGUE_TITLE_1", "LOC_ID_DUMMY_DIALOGUE_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
				WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
				DialogueManager.AddDialogueCompleteEndHandler(this.m_closeDummyTips);
				return;
			}
			if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
			{
				this.m_npcController.RunNextNPCDialogue(this.m_closeDummyTips);
				return;
			}
			this.OpenDummyTips();
		}
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x000A91E7 File Offset: 0x000A73E7
	private void CloseDummyTips()
	{
		if (this.m_npcController.CurrentState != NPCState.Idle)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000A9224 File Offset: 0x000A7424
	private void OpenDummyTips()
	{
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		bool isFemale = SaveManager.PlayerSaveData.CurrentCharacter.IsFemale;
		DummyDialogueController.DummyDialogueEntry dummyDialogueEntry;
		if (DummyDialogueController.m_dialogueDict.TryGetValue(classType, out dummyDialogueEntry))
		{
			int num = Mathf.Clamp(this.m_dialogueIndexDict[classType], 0, dummyDialogueEntry.TextLocIDArray.Length - 1);
			string titleLocID = dummyDialogueEntry.TitleLocID;
			string textLocID = dummyDialogueEntry.TextLocIDArray[num];
			num++;
			if (num >= dummyDialogueEntry.TextLocIDArray.Length)
			{
				num = 0;
			}
			this.m_dialogueIndexDict[classType] = num;
			DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
			DialogueManager.AddDialogue(titleLocID, textLocID, isFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			DialogueManager.AddDialogueCompleteEndHandler(new Action(this.CloseDummyTips));
			return;
		}
		this.CloseDummyTips();
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000A92EC File Offset: 0x000A74EC
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_DUMMY_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_DUMMY_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_DUMMY_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_DUMMY_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeDummyTips);
	}

	// Token: 0x04002752 RID: 10066
	private static Dictionary<ClassType, DummyDialogueController.DummyDialogueEntry> m_dialogueDict = new Dictionary<ClassType, DummyDialogueController.DummyDialogueEntry>
	{
		{
			ClassType.SwordClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_KNIGHT_SWORD_MOVE_1",
					"LOC_ID_DUMMY_DIALOGUE_KNIGHT_SWORD_DASH_1",
					"LOC_ID_DUMMY_DIALOGUE_KNIGHT_BLOCK_PERFECT_1",
					"LOC_ID_DUMMY_DIALOGUE_KNIGHT_BLOCK_FREE_1"
				}
			}
		},
		{
			ClassType.BowClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_ARCHER_BOW_AIR_1",
					"LOC_ID_DUMMY_DIALOGUE_ARCHER_PLATFORM_SPORE_1",
					"LOC_ID_DUMMY_DIALOGUE_ARCHER_DASH_CANCEL_1",
					"LOC_ID_DUMMY_DIALOGUE_ARCHER_SPIN_KICK_1"
				}
			}
		},
		{
			ClassType.AxeClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_BARBARIAN_AXE_AIR_1",
					"LOC_ID_DUMMY_DIALOGUE_BARBARIAN_AXE_GROUND_1",
					"LOC_ID_DUMMY_DIALOGUE_BARBARIAN_SHOUT_FREEZE_1",
					"LOC_ID_DUMMY_DIALOGUE_BARBARIAN_AXE_AIR_CHIP_HIT_1"
				}
			}
		},
		{
			ClassType.MagicWandClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_MAGE_DOUBLE_HIT_1",
					"LOC_ID_DUMMY_DIALOGUE_MAGE_SPELL_VARIETY_1",
					"LOC_ID_DUMMY_DIALOGUE_MAGE_MIXING_1",
					"LOC_ID_DUMMY_DIALOGUE_MAGE_MANA_LEECH_TARGET_1"
				}
			}
		},
		{
			ClassType.SaberClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_SABER_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_SABER_BASIC_COMBO_1",
					"LOC_ID_DUMMY_DIALOGUE_SABER_ROLL_1",
					"LOC_ID_DUMMY_DIALOGUE_SABER_ROLL_COMBO_1"
				}
			}
		},
		{
			ClassType.LadleClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_COOK_ATTACK_REFLECT_1",
					"LOC_ID_DUMMY_DIALOGUE_COOK_HEAL_1",
					"LOC_ID_DUMMY_DIALOGUE_COOK_MANA_1"
				}
			}
		},
		{
			ClassType.SpearClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_SPEAR_DIRECTION_1",
					"LOC_ID_DUMMY_DIALOGUE_SPEAR_COUNTER_RESET_1",
					"LOC_ID_DUMMY_DIALOGUE_SPEAR_COUNTER_MANA_1",
					"LOC_ID_DUMMY_DIALOGUE_SPEAR_BOUNCE_1"
				}
			}
		},
		{
			ClassType.DualBladesClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_DUALBLADES_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_DUALBLADES_CRITSTRIKE_1",
					"LOC_ID_DUMMY_DIALOGUE_DUALBLADES_CLOAK_1",
					"LOC_ID_DUMMY_DIALOGUE_DUALBLADES_DASH_ATTACK_1",
					"LOC_ID_DUMMY_DIALOGUE_DUALBLADES_MANA_1"
				}
			}
		},
		{
			ClassType.GunClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_GUNSLINGER_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_GUNSLINGER_DASH_1",
					"LOC_ID_DUMMY_DIALOGUE_GUNSLINGER_AMMO_1",
					"LOC_ID_DUMMY_DIALOGUE_GUNSLINGER_AIR_1"
				}
			}
		},
		{
			ClassType.LanceClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_LANCE_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_LANCE_CONTROL_MOVEMENT_1",
					"LOC_ID_DUMMY_DIALOGUE_LANCE_MAGIC_AND_STRENGTH_1",
					"LOC_ID_DUMMY_DIALOGUE_LANCE_TALENT_FREE_CAST_1"
				}
			}
		},
		{
			ClassType.BoxingGloveClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_BOXER_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_BOXER_COMBO_1",
					"LOC_ID_DUMMY_DIALOGUE_BOXER_THROWING_1",
					"LOC_ID_DUMMY_DIALOGUE_BOXER_AIMING_1",
					"LOC_ID_DUMMY_DIALOGUE_BOXER_UPPERCUT_1"
				}
			}
		},
		{
			ClassType.KatanaClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_RONIN_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_RONIN_ANGLE_1",
					"LOC_ID_DUMMY_DIALOGUE_RONIN_CRIT_1",
					"LOC_ID_DUMMY_DIALOGUE_RONIN_TELESLICE_1",
					"LOC_ID_DUMMY_DIALOGUE_RONIN_DASH_CANCEL_1"
				}
			}
		},
		{
			ClassType.LuteClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_BARD_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_BARD_EXPLODING_NOTE_1",
					"LOC_ID_DUMMY_DIALOGUE_BARD_DANCE_1",
					"LOC_ID_DUMMY_DIALOGUE_BARD_DANCE_ADVANCED_1",
					"LOC_ID_DUMMY_DIALOGUE_BARD_CLIMB_1",
					"LOC_ID_DUMMY_DIALOGUE_BARD_CRESCENDO_1"
				}
			}
		},
		{
			ClassType.CannonClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_PIRATE_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_PIRATE_WEAKNESS_COMBO_1",
					"LOC_ID_DUMMY_DIALOGUE_PIRATE_DESTROY_PROJECTILES_1",
					"LOC_ID_DUMMY_DIALOGUE_PIRATE_BOAT_MOVEMENT_1"
				}
			}
		},
		{
			ClassType.AstroClass,
			new DummyDialogueController.DummyDialogueEntry
			{
				TitleLocID = "LOC_ID_NAME_DUMMY_1",
				TextLocIDArray = new string[]
				{
					"LOC_ID_DUMMY_DIALOGUE_ASTROMANCER_INTRO_1",
					"LOC_ID_DUMMY_DIALOGUE_ASTROMANCER_MULTIPLE_TARGETS_1",
					"LOC_ID_DUMMY_DIALOGUE_ASTROMANCER_PASSIVE_1",
					"LOC_ID_DUMMY_DIALOGUE_ASTROMANCER_COMET_1"
				}
			}
		}
	};

	// Token: 0x04002753 RID: 10067
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04002754 RID: 10068
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04002755 RID: 10069
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x04002756 RID: 10070
	private Dictionary<ClassType, int> m_dialogueIndexDict = new Dictionary<ClassType, int>();

	// Token: 0x04002757 RID: 10071
	private NPCController m_npcController;

	// Token: 0x04002758 RID: 10072
	private Interactable m_interactable;

	// Token: 0x04002759 RID: 10073
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x0400275A RID: 10074
	private Action m_closeDummyTips;

	// Token: 0x02000D2E RID: 3374
	private class DummyDialogueEntry
	{
		// Token: 0x0400536A RID: 21354
		public string TitleLocID;

		// Token: 0x0400536B RID: 21355
		public string[] TextLocIDArray;
	}
}
