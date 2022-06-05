using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000909 RID: 2313
public class EnchantressShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x170018DB RID: 6363
	// (get) Token: 0x06004638 RID: 17976 RVA: 0x0002698D File Offset: 0x00024B8D
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

	// Token: 0x170018DC RID: 6364
	// (get) Token: 0x06004639 RID: 17977 RVA: 0x0002699A File Offset: 0x00024B9A
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue() || this.HasUnseenRunes();
		}
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x001135F0 File Offset: 0x001117F0
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

	// Token: 0x0600463B RID: 17979 RVA: 0x0011367C File Offset: 0x0011187C
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

	// Token: 0x170018DD RID: 6365
	// (get) Token: 0x0600463C RID: 17980 RVA: 0x000269AC File Offset: 0x00024BAC
	public bool IsEnchantressOpen
	{
		get
		{
			return this.m_isEnchantressOpen;
		}
	}

	// Token: 0x170018DE RID: 6366
	// (get) Token: 0x0600463D RID: 17981 RVA: 0x000269B4 File Offset: 0x00024BB4
	// (set) Token: 0x0600463E RID: 17982 RVA: 0x000269BC File Offset: 0x00024BBC
	public BaseRoom Room { get; private set; }

	// Token: 0x170018DF RID: 6367
	// (get) Token: 0x0600463F RID: 17983 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x001136D8 File Offset: 0x001118D8
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayEnchantressWindow = new Action(this.DisplayEnchantressWindow);
		this.m_enchantressWindowClosed = new Action(this.EnchantressWindowClosed);
		this.m_enchantressWindowClosedUnityEvent = new UnityAction(this.EnchantressWindowClosed);
	}

	// Token: 0x06004641 RID: 17985 RVA: 0x000269C5 File Offset: 0x00024BC5
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x000269F1 File Offset: 0x00024BF1
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x000269FA File Offset: 0x00024BFA
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x00026A10 File Offset: 0x00024C10
	public void OpenEnchantress()
	{
		this.m_isEnchantressOpen = true;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenEnchantressCoroutine());
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x00026A32 File Offset: 0x00024C32
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

	// Token: 0x06004646 RID: 17990 RVA: 0x00113738 File Offset: 0x00111938
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

	// Token: 0x06004647 RID: 17991 RVA: 0x001137D0 File Offset: 0x001119D0
	private void RunEnchantressStackingDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.EnchantressDialogue_Stacking, true);
		DialogueManager.StartNewDialogue(this.m_enchantress, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_ENCHANTRESS_1", "LOC_ID_ENCHANTRESS_DIALOGUE_EXPLAINING_STACKING_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayEnchantressWindow);
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x00113834 File Offset: 0x00111A34
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

	// Token: 0x06004649 RID: 17993 RVA: 0x00026A41 File Offset: 0x00024C41
	private void DisplayEnchantressWindow()
	{
		base.StartCoroutine(this.DisplayEnchantressWindowCoroutine());
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x00026A50 File Offset: 0x00024C50
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

	// Token: 0x0600464B RID: 17995 RVA: 0x00026A5F File Offset: 0x00024C5F
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

	// Token: 0x0600464C RID: 17996 RVA: 0x001138D4 File Offset: 0x00111AD4
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

	// Token: 0x0600464E RID: 17998 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003635 RID: 13877
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04003636 RID: 13878
	[SerializeField]
	private NPCController m_enchantress;

	// Token: 0x04003637 RID: 13879
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04003638 RID: 13880
	private bool m_isEnchantressOpen;

	// Token: 0x04003639 RID: 13881
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400363A RID: 13882
	private Interactable m_interactable;

	// Token: 0x0400363B RID: 13883
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x0400363C RID: 13884
	private Action m_displayEnchantressWindow;

	// Token: 0x0400363D RID: 13885
	private Action m_enchantressWindowClosed;

	// Token: 0x0400363E RID: 13886
	private UnityAction m_enchantressWindowClosedUnityEvent;
}
