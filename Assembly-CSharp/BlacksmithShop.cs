using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000548 RID: 1352
public class BlacksmithShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x17001235 RID: 4661
	// (get) Token: 0x0600317F RID: 12671 RVA: 0x000A76AC File Offset: 0x000A58AC
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

	// Token: 0x17001236 RID: 4662
	// (get) Token: 0x06003180 RID: 12672 RVA: 0x000A76B9 File Offset: 0x000A58B9
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue() || this.HasUnseenGear();
		}
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x000A76CC File Offset: 0x000A58CC
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_Intro))
		{
			return true;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_Upgrades))
		{
			bool flag = false;
			foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
			{
				if (equipmentCategoryType != EquipmentCategoryType.None)
				{
					foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
					{
						if (equipmentType != EquipmentType.None && EquipmentManager.GetUpgradeBlueprintsFound(equipmentCategoryType, equipmentType, true) >= 1)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				return true;
			}
		}
		else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_EquipmentSets))
		{
			bool flag2 = false;
			foreach (EquipmentType equipmentType2 in EquipmentType_RL.TypeArray)
			{
				if (equipmentType2 != EquipmentType.None && EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType2) >= 3)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x000A77C4 File Offset: 0x000A59C4
	private bool HasUnseenGear()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return false;
		}
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
				{
					if (equipmentType != EquipmentType.None)
					{
						EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(equipmentCategoryType, equipmentType);
						if (equipmentData && !equipmentData.Disabled && EquipmentManager.GetFoundState(equipmentCategoryType, equipmentType) == FoundState.FoundButNotViewed)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x17001237 RID: 4663
	// (get) Token: 0x06003183 RID: 12675 RVA: 0x000A7849 File Offset: 0x000A5A49
	public bool IsBlacksmithOpen
	{
		get
		{
			return this.m_isBlacksmithOpen;
		}
	}

	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06003184 RID: 12676 RVA: 0x000A7851 File Offset: 0x000A5A51
	// (set) Token: 0x06003185 RID: 12677 RVA: 0x000A7859 File Offset: 0x000A5A59
	public BaseRoom Room { get; private set; }

	// Token: 0x06003186 RID: 12678 RVA: 0x000A7862 File Offset: 0x000A5A62
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06003187 RID: 12679 RVA: 0x000A7889 File Offset: 0x000A5A89
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x000A7894 File Offset: 0x000A5A94
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayBlacksmithWindow = new Action(this.DisplayBlacksmithWindow);
		this.m_blacksmithWindowClosed = new Action(this.BlacksmithWindowClosed);
		this.m_blacksmithWindowClosedUnityEvent = new UnityAction(this.BlacksmithWindowClosed);
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000A78F4 File Offset: 0x000A5AF4
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x000A7920 File Offset: 0x000A5B20
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		EndingSpawnRoomTypeController component = this.Room.GetComponent<EndingSpawnRoomTypeController>();
		if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
		{
			this.m_blacksmith.SetNPCState(NPCState.AtAttention, false);
		}
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x000A7964 File Offset: 0x000A5B64
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x000A797A File Offset: 0x000A5B7A
	public void OpenBlacksmith()
	{
		this.m_isBlacksmithOpen = true;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenBlacksmithCoroutine());
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x000A799C File Offset: 0x000A5B9C
	private IEnumerator OpenBlacksmithCoroutine()
	{
		this.m_blacksmith.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToBlacksmith();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
		}
		else
		{
			bool flag = false;
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_Intro))
			{
				this.RunBlacksmithIntroDialogue();
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_Upgrades))
			{
				bool flag2 = false;
				foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
				{
					if (equipmentCategoryType != EquipmentCategoryType.None)
					{
						foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
						{
							if (equipmentType != EquipmentType.None && EquipmentManager.GetUpgradeBlueprintsFound(equipmentCategoryType, equipmentType, true) >= 1)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
				}
				if (flag2)
				{
					this.RunBlacksmithUpgradingDialogue();
				}
				else
				{
					flag = true;
				}
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BlacksmithDialogue_EquipmentSets))
			{
				bool flag3 = false;
				foreach (EquipmentType equipmentType2 in EquipmentType_RL.TypeArray)
				{
					if (equipmentType2 != EquipmentType.None && EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType2) >= 3)
					{
						flag3 = true;
						break;
					}
				}
				if (flag3)
				{
					this.RunBlacksmithEquipmentSetsDialogue();
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
				if (NPCDialogueManager.CanSpeak(this.m_blacksmith.NPCType))
				{
					this.m_blacksmith.RunNextNPCDialogue(this.m_displayBlacksmithWindow);
				}
				else
				{
					this.DisplayBlacksmithWindow();
				}
			}
		}
		yield break;
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x000A79AC File Offset: 0x000A5BAC
	private void RunBlacksmithIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BlacksmithDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.AtAttention);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_SPECIAL_INTRO_TOPSIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		else
		{
			DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_INTRODUCTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayBlacksmithWindow);
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x000A7A40 File Offset: 0x000A5C40
	private void RunBlacksmithUpgradingDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BlacksmithDialogue_Upgrades, true);
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_EXPLAINING_UPGRADES_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayBlacksmithWindow);
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x000A7AA0 File Offset: 0x000A5CA0
	private void RunBlacksmithEquipmentSetsDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BlacksmithDialogue_EquipmentSets, true);
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_UNITY_HINT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayBlacksmithWindow);
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x000A7B04 File Offset: 0x000A5D04
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_blacksmith.IsBestFriend)
			{
				textLocID = "LOC_ID_BLACKSMITH_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_BLACKSMITH_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_blacksmith.IsBestFriend)
		{
			textLocID = "LOC_ID_BLACKSMITH_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_BLACKSMITH_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_blacksmith.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_blacksmithWindowClosed);
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x000A7BA3 File Offset: 0x000A5DA3
	private void DisplayBlacksmithWindow()
	{
		base.StartCoroutine(this.DisplayBlacksmithCoroutine());
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x000A7BB2 File Offset: 0x000A5DB2
	private IEnumerator DisplayBlacksmithCoroutine()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Blacksmith))
		{
			WindowManager.LoadWindow(WindowID.Blacksmith);
		}
		yield return null;
		WindowManager.SetWindowIsOpen(WindowID.Blacksmith, true);
		WindowManager.GetWindowController(WindowID.Blacksmith).WindowClosedEvent.AddListener(this.m_blacksmithWindowClosedUnityEvent);
		yield break;
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000A7BC1 File Offset: 0x000A5DC1
	private IEnumerator MovePlayerToBlacksmith()
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

	// Token: 0x06003195 RID: 12693 RVA: 0x000A7BD0 File Offset: 0x000A5DD0
	private void BlacksmithWindowClosed()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.Blacksmith))
		{
			WindowManager.GetWindowController(WindowID.Blacksmith).WindowClosedEvent.RemoveListener(this.m_blacksmithWindowClosedUnityEvent);
		}
		this.m_isBlacksmithOpen = false;
		this.m_interactable.SetIsInteractableActive(true);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.m_blacksmith.SetNPCState(NPCState.AtAttention, false);
			return;
		}
		this.m_blacksmith.SetNPCState(NPCState.Idle, false);
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x000A7C3F File Offset: 0x000A5E3F
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002701 RID: 9985
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04002702 RID: 9986
	[SerializeField]
	private NPCController m_blacksmith;

	// Token: 0x04002703 RID: 9987
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04002704 RID: 9988
	private bool m_isBlacksmithOpen;

	// Token: 0x04002705 RID: 9989
	private Interactable m_interactable;

	// Token: 0x04002706 RID: 9990
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002707 RID: 9991
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04002708 RID: 9992
	private Action m_displayBlacksmithWindow;

	// Token: 0x04002709 RID: 9993
	private Action m_blacksmithWindowClosed;

	// Token: 0x0400270A RID: 9994
	private UnityAction m_blacksmithWindowClosedUnityEvent;
}
