using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008EF RID: 2287
public class BlacksmithShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x170018AA RID: 6314
	// (get) Token: 0x06004563 RID: 17763 RVA: 0x0002615C File Offset: 0x0002435C
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

	// Token: 0x170018AB RID: 6315
	// (get) Token: 0x06004564 RID: 17764 RVA: 0x00026169 File Offset: 0x00024369
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue() || this.HasUnseenGear();
		}
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x00110BB0 File Offset: 0x0010EDB0
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

	// Token: 0x06004566 RID: 17766 RVA: 0x00110CA8 File Offset: 0x0010EEA8
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

	// Token: 0x170018AC RID: 6316
	// (get) Token: 0x06004567 RID: 17767 RVA: 0x0002617B File Offset: 0x0002437B
	public bool IsBlacksmithOpen
	{
		get
		{
			return this.m_isBlacksmithOpen;
		}
	}

	// Token: 0x170018AD RID: 6317
	// (get) Token: 0x06004568 RID: 17768 RVA: 0x00026183 File Offset: 0x00024383
	// (set) Token: 0x06004569 RID: 17769 RVA: 0x0002618B File Offset: 0x0002438B
	public BaseRoom Room { get; private set; }

	// Token: 0x0600456A RID: 17770 RVA: 0x00026194 File Offset: 0x00024394
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x170018AE RID: 6318
	// (get) Token: 0x0600456B RID: 17771 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x00110D30 File Offset: 0x0010EF30
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayBlacksmithWindow = new Action(this.DisplayBlacksmithWindow);
		this.m_blacksmithWindowClosed = new Action(this.BlacksmithWindowClosed);
		this.m_blacksmithWindowClosedUnityEvent = new UnityAction(this.BlacksmithWindowClosed);
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x000261BB File Offset: 0x000243BB
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x00110D90 File Offset: 0x0010EF90
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		EndingSpawnRoomTypeController component = this.Room.GetComponent<EndingSpawnRoomTypeController>();
		if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
		{
			this.m_blacksmith.SetNPCState(NPCState.AtAttention, false);
		}
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x000261E7 File Offset: 0x000243E7
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x000261FD File Offset: 0x000243FD
	public void OpenBlacksmith()
	{
		this.m_isBlacksmithOpen = true;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenBlacksmithCoroutine());
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x0002621F File Offset: 0x0002441F
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

	// Token: 0x06004572 RID: 17778 RVA: 0x00110DD4 File Offset: 0x0010EFD4
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

	// Token: 0x06004573 RID: 17779 RVA: 0x00110E68 File Offset: 0x0010F068
	private void RunBlacksmithUpgradingDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BlacksmithDialogue_Upgrades, true);
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_EXPLAINING_UPGRADES_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayBlacksmithWindow);
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x00110EC8 File Offset: 0x0010F0C8
	private void RunBlacksmithEquipmentSetsDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BlacksmithDialogue_EquipmentSets, true);
		DialogueManager.StartNewDialogue(this.m_blacksmith, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_BLACKSMITH_1", "LOC_ID_BLACKSMITH_DIALOGUE_UNITY_HINT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayBlacksmithWindow);
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x00110F2C File Offset: 0x0010F12C
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

	// Token: 0x06004576 RID: 17782 RVA: 0x0002622E File Offset: 0x0002442E
	private void DisplayBlacksmithWindow()
	{
		base.StartCoroutine(this.DisplayBlacksmithCoroutine());
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x0002623D File Offset: 0x0002443D
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

	// Token: 0x06004578 RID: 17784 RVA: 0x0002624C File Offset: 0x0002444C
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

	// Token: 0x06004579 RID: 17785 RVA: 0x00110FCC File Offset: 0x0010F1CC
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

	// Token: 0x0600457B RID: 17787 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040035A7 RID: 13735
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040035A8 RID: 13736
	[SerializeField]
	private NPCController m_blacksmith;

	// Token: 0x040035A9 RID: 13737
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040035AA RID: 13738
	private bool m_isBlacksmithOpen;

	// Token: 0x040035AB RID: 13739
	private Interactable m_interactable;

	// Token: 0x040035AC RID: 13740
	private WaitRL_Yield m_waitYield;

	// Token: 0x040035AD RID: 13741
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040035AE RID: 13742
	private Action m_displayBlacksmithWindow;

	// Token: 0x040035AF RID: 13743
	private Action m_blacksmithWindowClosed;

	// Token: 0x040035B0 RID: 13744
	private UnityAction m_blacksmithWindowClosedUnityEvent;
}
