using System;
using System.Collections.Generic;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class DummyDialogueController : MonoBehaviour, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x170018D8 RID: 6360
	// (get) Token: 0x0600462B RID: 17963 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x170018D9 RID: 6361
	// (get) Token: 0x0600462C RID: 17964 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x170018DA RID: 6362
	// (get) Token: 0x0600462D RID: 17965 RVA: 0x000268C8 File Offset: 0x00024AC8
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x0600462E RID: 17966 RVA: 0x000268D0 File Offset: 0x00024AD0
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DummyDialogue_Intro);
	}

	// Token: 0x0600462F RID: 17967 RVA: 0x00112ECC File Offset: 0x001110CC
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

	// Token: 0x06004630 RID: 17968 RVA: 0x0002690D File Offset: 0x00024B0D
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

	// Token: 0x06004631 RID: 17969 RVA: 0x00112F2C File Offset: 0x0011112C
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

	// Token: 0x06004632 RID: 17970 RVA: 0x0002693F File Offset: 0x00024B3F
	private void CloseDummyTips()
	{
		if (this.m_npcController.CurrentState != NPCState.Idle)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x00113018 File Offset: 0x00111218
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

	// Token: 0x06004634 RID: 17972 RVA: 0x001130E0 File Offset: 0x001112E0
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

	// Token: 0x0400362A RID: 13866
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

	// Token: 0x0400362B RID: 13867
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x0400362C RID: 13868
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x0400362D RID: 13869
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x0400362E RID: 13870
	private Dictionary<ClassType, int> m_dialogueIndexDict = new Dictionary<ClassType, int>();

	// Token: 0x0400362F RID: 13871
	private NPCController m_npcController;

	// Token: 0x04003630 RID: 13872
	private Interactable m_interactable;

	// Token: 0x04003631 RID: 13873
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04003632 RID: 13874
	private Action m_closeDummyTips;

	// Token: 0x02000908 RID: 2312
	private class DummyDialogueEntry
	{
		// Token: 0x04003633 RID: 13875
		public string TitleLocID;

		// Token: 0x04003634 RID: 13876
		public string[] TextLocIDArray;
	}
}
