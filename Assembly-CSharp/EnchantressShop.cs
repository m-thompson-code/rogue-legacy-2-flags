using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000553 RID: 1363
public class EnchantressShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x1700124A RID: 4682
	// (get) Token: 0x060031FF RID: 12799 RVA: 0x000A980E File Offset: 0x000A7A0E
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (this.HasEventDialogue())
			{
				return SpeechBubbleType.Dialogue;
			}
			return SpeechBubbleType.GearAvailable;
		}
	}

	// Token: 0x1700124B RID: 4683
	// (get) Token: 0x06003200 RID: 12800 RVA: 0x000A981B File Offset: 0x000A7A1B
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue() || this.HasUnseenRunes();
		}
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x000A9830 File Offset: 0x000A7A30
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnchantressDialogue_Intro))
		{
			return true;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnchantressDialogue_Stacking))
		{
			bool flag = false;
			foreach (RuneType runeType in RuneType_RL.TypeArray)
			{
				if (runeType != RuneType.None && RuneManager.GetUpgradeBlueprintsFound(runeType, true) > 1)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x000A98BC File Offset: 0x000A7ABC
	private bool HasUnseenRunes()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return false;
		}
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneData runeData = RuneLibrary.GetRuneData(runeType);
				if (runeData && !runeData.Disabled && RuneManager.GetFoundState(runeType) == FoundState.FoundButNotViewed)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x1700124C RID: 4684
	// (get) Token: 0x06003203 RID: 12803 RVA: 0x000A9918 File Offset: 0x000A7B18
	public bool IsEnchantressOpen
	{
		get
		{
			return this.m_isEnchantressOpen;
		}
	}

	// Token: 0x1700124D RID: 4685
	// (get) Token: 0x06003204 RID: 12804 RVA: 0x000A9920 File Offset: 0x000A7B20
	// (set) Token: 0x06003205 RID: 12805 RVA: 0x000A9928 File Offset: 0x000A7B28
	public BaseRoom Room { get; private set; }

	// Token: 0x1700124E RID: 4686
	// (get) Token: 0x06003206 RID: 12806 RVA: 0x000A9931 File Offset: 0x000A7B31
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x000A993C File Offset: 0x000A7B3C
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayEnchantressWindow = new Action(this.DisplayEnchantressWindow);
		this.m_enchantressWindowClosed = new Action(this.EnchantressWindowClosed);
		this.m_enchantressWindowClosedUnityEvent = new UnityAction(this.EnchantressWindowClosed);
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x000A999C File Offset: 0x000A7B9C
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x000A99C8 File Offset: 0x000A7BC8
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x000A99D1 File Offset: 0x000A7BD1
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x000A99E7 File Offset: 0x000A7BE7
	public void OpenEnchantress()
	{
		this.m_isEnchantressOpen = true;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenEnchantressCoroutine());
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000A9A09 File Offset: 0x000A7C09
	private IEnumerator OpenEnchantressCoroutine()
	{
		this.m_enchantress.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToEnchantress();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
		}
		else
		{
			bool flag = false;
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnchantressDialogue_Intro))
			{
				this.RunEnchantressIntroDialogue();
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnchantressDialogue_Stacking))
			{
				bool flag2 = false;
				foreach (RuneType runeType in RuneType_RL.TypeArray)
				{
					if (runeType != RuneType.None && RuneManager.GetUpgradeBlueprintsFound(runeType, true) > 1)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					this.RunEnchantressStackingDialogue();
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (NPCDialogueManager.CanSpeak(this.m_enchantress.NPCType))
				{
					this.m_enchantress.RunNextNPCDialogue(this.m_displayEnchantressWindow);
				}
				else
				{
					this.DisplayEnchantressWindow();
				}
			}
		}
		yield break;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x000A9A18 File Offset: 0x000A7C18
	private void RunEnchantressIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.EnchantressDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_enchantress, NPCState.AtAttention);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			DialogueManager.AddDialogue("LOC_ID_NAME_ENCHANTRESS_1", "LOC_ID_ENCHANTRESS_DIALOGUE_SPECIAL_INTRO_TOPSIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		else
		{
			DialogueManager.AddDialogue("LOC_ID_NAME_ENCHANTRESS_1", "LOC_ID_ENCHANTRESS_DIALOGUE_INTRODUCTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayEnchantressWindow);
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x000A9AB0 File Offset: 0x000A7CB0
	private void RunEnchantressStackingDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.EnchantressDialogue_Stacking, true);
		DialogueManager.StartNewDialogue(this.m_enchantress, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_ENCHANTRESS_1", "LOC_ID_ENCHANTRESS_DIALOGUE_EXPLAINING_STACKING_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayEnchantressWindow);
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x000A9B14 File Offset: 0x000A7D14
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_enchantress.IsBestFriend)
			{
				textLocID = "LOC_ID_ENCHANTRESS_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_ENCHANTRESS_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_enchantress.IsBestFriend)
		{
			textLocID = "LOC_ID_ENCHANTRESS_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_ENCHANTRESS_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_enchantress, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_enchantress.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_enchantressWindowClosed);
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x000A9BB3 File Offset: 0x000A7DB3
	private void DisplayEnchantressWindow()
	{
		base.StartCoroutine(this.DisplayEnchantressWindowCoroutine());
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x000A9BC2 File Offset: 0x000A7DC2
	private IEnumerator DisplayEnchantressWindowCoroutine()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Enchantress))
		{
			WindowManager.LoadWindow(WindowID.Enchantress);
		}
		yield return null;
		WindowManager.SetWindowIsOpen(WindowID.Enchantress, true);
		WindowManager.GetWindowController(WindowID.Enchantress).WindowClosedEvent.AddListener(this.m_enchantressWindowClosedUnityEvent);
		yield break;
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x000A9BD1 File Offset: 0x000A7DD1
	private IEnumerator MovePlayerToEnchantress()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			playerController.SetFacing(false);
		}
		else
		{
			playerController.SetFacing(true);
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x000A9BE0 File Offset: 0x000A7DE0
	private void EnchantressWindowClosed()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.Enchantress))
		{
			WindowManager.GetWindowController(WindowID.Enchantress).WindowClosedEvent.RemoveListener(this.m_enchantressWindowClosedUnityEvent);
		}
		this.m_isEnchantressOpen = false;
		this.m_interactable.SetIsInteractableActive(true);
		this.m_enchantress.SetNPCState(NPCState.Idle, false);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x000A9C35 File Offset: 0x000A7E35
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400275B RID: 10075
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400275C RID: 10076
	[SerializeField]
	private NPCController m_enchantress;

	// Token: 0x0400275D RID: 10077
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x0400275E RID: 10078
	private bool m_isEnchantressOpen;

	// Token: 0x0400275F RID: 10079
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002760 RID: 10080
	private Interactable m_interactable;

	// Token: 0x04002761 RID: 10081
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04002762 RID: 10082
	private Action m_displayEnchantressWindow;

	// Token: 0x04002763 RID: 10083
	private Action m_enchantressWindowClosed;

	// Token: 0x04002764 RID: 10084
	private UnityAction m_enchantressWindowClosedUnityEvent;
}
