using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200055C RID: 1372
public class SoulShopShop : MonoBehaviour, IDisplaySpeechBubble, IRootObj, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x1700125C RID: 4700
	// (get) Token: 0x06003268 RID: 12904 RVA: 0x000AAB32 File Offset: 0x000A8D32
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x06003269 RID: 12905 RVA: 0x000AAB3A File Offset: 0x000A8D3A
	// (set) Token: 0x0600326A RID: 12906 RVA: 0x000AAB42 File Offset: 0x000A8D42
	public BaseRoom Room { get; private set; }

	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x0600326B RID: 12907 RVA: 0x000AAB4B File Offset: 0x000A8D4B
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x1700125F RID: 4703
	// (get) Token: 0x0600326C RID: 12908 RVA: 0x000AAB53 File Offset: 0x000A8D53
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

	// Token: 0x0600326D RID: 12909 RVA: 0x000AAB60 File Offset: 0x000A8D60
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SoulShopDialogue_Intro);
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x000AABA0 File Offset: 0x000A8DA0
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayShopWindow = new Action(this.DisplayShopWindow);
		this.m_closeSoulShop = new Action(this.CloseSoulShop);
		this.m_closeSoulShopUnityEvent = new UnityAction(this.CloseSoulShop);
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x000AABEF File Offset: 0x000A8DEF
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x000AAC16 File Offset: 0x000A8E16
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x000AAC42 File Offset: 0x000A8E42
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		base.StartCoroutine(this.SetSpeechBubbleCoroutine());
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000AAC58 File Offset: 0x000A8E58
	private IEnumerator SetSpeechBubbleCoroutine()
	{
		yield return null;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.m_speechBubble.DisplayOffscreen = false;
		}
		else
		{
			this.m_speechBubble.DisplayOffscreen = true;
		}
		yield break;
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000AAC68 File Offset: 0x000A8E68
	public void OpenSoulShop()
	{
		this.m_interactable.SetIsInteractableActive(false);
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SoulShop_Unlocked))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SoulShop_Unlocked, true);
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OpenSoulShopCoroutine());
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x000AACB5 File Offset: 0x000A8EB5
	private IEnumerator OpenSoulShopCoroutine()
	{
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		yield return this.MovePlayerToNPC();
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
		}
		else
		{
			bool flag = false;
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SoulShopDialogue_Intro))
			{
				this.RunNPCIntroDialogue();
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
				{
					this.m_npcController.RunNextNPCDialogue(this.m_displayShopWindow);
				}
				else
				{
					this.DisplayShopWindow();
				}
			}
		}
		yield break;
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000AACC4 File Offset: 0x000A8EC4
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SoulShopDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_SOULSHOP_HOOD_1", "LOC_ID_SOULSHOP_MISC_OWNER_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayShopWindow);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x000AAD28 File Offset: 0x000A8F28
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_KERES_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_KERES_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_KERES_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_KERES_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeSoulShop);
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x000AADC7 File Offset: 0x000A8FC7
	private void DisplayShopWindow()
	{
		base.StartCoroutine(this.DisplayShopWindowCoroutine());
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x000AADD6 File Offset: 0x000A8FD6
	private IEnumerator DisplayShopWindowCoroutine()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.SoulShop))
		{
			WindowManager.LoadWindow(WindowID.SoulShop);
		}
		yield return null;
		WindowManager.GetWindowController(WindowID.SoulShop).WindowClosedEvent.AddListener(this.m_closeSoulShopUnityEvent);
		WindowManager.SetWindowIsOpen(WindowID.SoulShop, true);
		yield break;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x000AADE5 File Offset: 0x000A8FE5
	public void CloseSoulShop()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.SoulShop))
		{
			WindowManager.GetWindowController(WindowID.SoulShop).WindowClosedEvent.RemoveListener(this.m_closeSoulShopUnityEvent);
		}
		this.m_interactable.SetIsInteractableActive(true);
		this.m_npcController.SetNPCState(NPCState.Idle, false);
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000AAE20 File Offset: 0x000A9020
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x000AAE3B File Offset: 0x000A903B
	private IEnumerator MovePlayerToNPC()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (base.transform.lossyScale.x > 0f)
		{
			PlayerManager.GetPlayerController().SetFacing(false);
		}
		else
		{
			PlayerManager.GetPlayerController().SetFacing(true);
		}
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x000AAE52 File Offset: 0x000A9052
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400278F RID: 10127
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04002790 RID: 10128
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x04002791 RID: 10129
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04002792 RID: 10130
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04002793 RID: 10131
	private Interactable m_interactable;

	// Token: 0x04002794 RID: 10132
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04002795 RID: 10133
	private Action m_displayShopWindow;

	// Token: 0x04002796 RID: 10134
	private Action m_closeSoulShop;

	// Token: 0x04002797 RID: 10135
	private UnityAction m_closeSoulShopUnityEvent;
}
