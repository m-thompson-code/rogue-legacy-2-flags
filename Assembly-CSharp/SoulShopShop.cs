using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000923 RID: 2339
public class SoulShopShop : MonoBehaviour, IDisplaySpeechBubble, IRootObj, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x1700190F RID: 6415
	// (get) Token: 0x06004707 RID: 18183 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x17001910 RID: 6416
	// (get) Token: 0x06004708 RID: 18184 RVA: 0x00026F48 File Offset: 0x00025148
	// (set) Token: 0x06004709 RID: 18185 RVA: 0x00026F50 File Offset: 0x00025150
	public BaseRoom Room { get; private set; }

	// Token: 0x17001911 RID: 6417
	// (get) Token: 0x0600470A RID: 18186 RVA: 0x00026F59 File Offset: 0x00025159
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17001912 RID: 6418
	// (get) Token: 0x0600470B RID: 18187 RVA: 0x00026F61 File Offset: 0x00025161
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

	// Token: 0x0600470C RID: 18188 RVA: 0x00026F6E File Offset: 0x0002516E
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SoulShopDialogue_Intro);
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x00115104 File Offset: 0x00113304
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayShopWindow = new Action(this.DisplayShopWindow);
		this.m_closeSoulShop = new Action(this.CloseSoulShop);
		this.m_closeSoulShopUnityEvent = new UnityAction(this.CloseSoulShop);
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x00026FAD File Offset: 0x000251AD
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x00026FD4 File Offset: 0x000251D4
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00027000 File Offset: 0x00025200
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		base.StartCoroutine(this.SetSpeechBubbleCoroutine());
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00027016 File Offset: 0x00025216
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

	// Token: 0x06004712 RID: 18194 RVA: 0x00115154 File Offset: 0x00113354
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

	// Token: 0x06004713 RID: 18195 RVA: 0x00027025 File Offset: 0x00025225
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

	// Token: 0x06004714 RID: 18196 RVA: 0x001151A4 File Offset: 0x001133A4
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SoulShopDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_SOULSHOP_HOOD_1", "LOC_ID_SOULSHOP_MISC_OWNER_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayShopWindow);
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x00115208 File Offset: 0x00113408
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

	// Token: 0x06004716 RID: 18198 RVA: 0x00027034 File Offset: 0x00025234
	private void DisplayShopWindow()
	{
		base.StartCoroutine(this.DisplayShopWindowCoroutine());
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x00027043 File Offset: 0x00025243
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

	// Token: 0x06004718 RID: 18200 RVA: 0x00027052 File Offset: 0x00025252
	public void CloseSoulShop()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.SoulShop))
		{
			WindowManager.GetWindowController(WindowID.SoulShop).WindowClosedEvent.RemoveListener(this.m_closeSoulShopUnityEvent);
		}
		this.m_interactable.SetIsInteractableActive(true);
		this.m_npcController.SetNPCState(NPCState.Idle, false);
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x0002708D File Offset: 0x0002528D
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x000270A8 File Offset: 0x000252A8
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

	// Token: 0x0600471C RID: 18204 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400369F RID: 13983
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040036A0 RID: 13984
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x040036A1 RID: 13985
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x040036A2 RID: 13986
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040036A3 RID: 13987
	private Interactable m_interactable;

	// Token: 0x040036A4 RID: 13988
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040036A5 RID: 13989
	private Action m_displayShopWindow;

	// Token: 0x040036A6 RID: 13990
	private Action m_closeSoulShop;

	// Token: 0x040036A7 RID: 13991
	private UnityAction m_closeSoulShopUnityEvent;
}
