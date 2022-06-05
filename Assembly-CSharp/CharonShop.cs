using System;
using System.Collections;
using Cinemachine;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020008F9 RID: 2297
public class CharonShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x170018C2 RID: 6338
	// (get) Token: 0x060045C6 RID: 17862 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x170018C3 RID: 6339
	// (get) Token: 0x060045C7 RID: 17863 RVA: 0x00026519 File Offset: 0x00024719
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x170018C4 RID: 6340
	// (get) Token: 0x060045C8 RID: 17864 RVA: 0x00026521 File Offset: 0x00024721
	// (set) Token: 0x060045C9 RID: 17865 RVA: 0x00026529 File Offset: 0x00024729
	public BaseRoom Room { get; private set; }

	// Token: 0x170018C5 RID: 6341
	// (get) Token: 0x060045CA RID: 17866 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x001119E4 File Offset: 0x0010FBE4
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !this.m_endingSpeechBubblePlayed;
		}
		if (!this.m_useGardenLogic)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CharonDialogue_Intro);
		}
		return !this.m_hasSpoken;
	}

	// Token: 0x060045CC RID: 17868 RVA: 0x00111A40 File Offset: 0x0010FC40
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_runEnterCastleMenu = new Action(this.RunEnterCastleMenu);
		this.m_closeCharon = new Action(this.CloseCharon);
		this.m_cancelEnterCastleSelection = new Action(this.CancelEnterCastleSelection);
		this.m_confirmEnterCastleSelection = new Action(this.ConfirmEnterCastleSelection);
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x00026532 File Offset: 0x00024732
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x00026559 File Offset: 0x00024759
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x00026585 File Offset: 0x00024785
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		this.m_hasSpoken = false;
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x00026595 File Offset: 0x00024795
	public void OpenCharon()
	{
		this.m_interactable.SetIsInteractableActive(false);
		RewiredMapController.SetCurrentMapEnabled(false);
		base.StartCoroutine(this.OpenCharonCoroutine());
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x00111AB4 File Offset: 0x0010FCB4
	private void CloseCharon()
	{
		this.m_interactable.SetIsInteractableActive(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		this.m_charon.SetNPCState(NPCState.Idle, false);
		AudioManager.PlayOneShot(this, this.m_cancelEnterCastleAudioEvent, default(Vector3));
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x000265B6 File Offset: 0x000247B6
	private IEnumerator OpenCharonCoroutine()
	{
		this.m_charon.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		yield return this.MovePlayerToCharon();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.RunEndingDialogue();
		}
		else if (this.m_useGardenLogic)
		{
			this.RunGardenDialogue();
		}
		else if (this.HasEventDialogue())
		{
			this.RunCharonIntroDialogue();
		}
		else if (NPCDialogueManager.CanSpeak(this.m_charon.NPCType) && !this.m_useGardenLogic)
		{
			this.m_charon.RunNextNPCDialogue(this.m_runEnterCastleMenu);
		}
		else
		{
			this.RunEnterCastleMenu();
		}
		yield break;
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x00111AF8 File Offset: 0x0010FCF8
	private void RunCharonIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CharonDialogue_Intro, true);
		string textLocID = "LOC_ID_DIALOGUE_CHARON_FEE_TAKE_1";
		DialogueManager.StartNewDialogue(this.m_charon, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_DIALOGUE_CHARON_TITLE_MAIN_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_runEnterCastleMenu);
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x00111B5C File Offset: 0x0010FD5C
	private void RunGardenDialogue()
	{
		string textLocID = "LOC_ID_GARDEN_CHARON_DENYING_TRAVEL_1";
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated))
		{
			textLocID = "LOC_ID_GARDEN_CHARON_ACCEPTING_TRAVEL_1";
		}
		DialogueManager.StartNewDialogue(this.m_charon, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_DIALOGUE_CHARON_TITLE_MAIN_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated))
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_runEnterCastleMenu);
			return;
		}
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeCharon);
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x00111BE4 File Offset: 0x0010FDE4
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_charon.IsBestFriend)
			{
				textLocID = "LOC_ID_CHARON_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_CHARON_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_charon.IsBestFriend)
		{
			textLocID = "LOC_ID_CHARON_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_CHARON_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_charon, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_charon.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeCharon);
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x00111C84 File Offset: 0x0010FE84
	private void RunEnterCastleMenu()
	{
		this.InitializeEnterCastleConfirmMenu();
		AudioManager.PlayOneShot(this, this.m_charonWindowOpenedAudioEvent, default(Vector3));
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x00111CB4 File Offset: 0x0010FEB4
	private void InitializeEnterCastleConfirmMenu()
	{
		if (WindowManager.GetWindowController(WindowID.ConfirmMenu) == null)
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		string text = "LOC_ID_CHARON_UI_ENTER_CASTLE_TITLE_1";
		string text2 = "LOC_ID_CHARON_UI_ENTER_CASTLE_TEXT_1";
		if (this.m_useGardenLogic)
		{
			text = "LOC_ID_CHARON_UI_ENTER_DOCKS_TITLE_1";
			text2 = "LOC_ID_CHARON_UI_ENTER_DOCKS_TEXT_1";
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText(text, true);
		confirmMenuWindowController.SetDescriptionText(text2, true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelEnterCastleSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmEnterCastleSelection);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelEnterCastleSelection);
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x000265C5 File Offset: 0x000247C5
	private void CancelEnterCastleSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.CloseCharon();
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x00111D64 File Offset: 0x0010FF64
	private void ConfirmEnterCastleSelection()
	{
		AudioManager.PlayOneShot(this, this.m_chooseEnterCastleAudioEvent, default(Vector3));
		base.GetComponent<Interactable>().SetIsInteractableActive(false);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.StartCoroutine(this.LoadWorldCoroutine());
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x000265D5 File Offset: 0x000247D5
	private IEnumerator MovePlayerToCharon()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if ((playerController.transform.position.x < base.transform.position.x && !playerController.IsFacingRight) || (playerController.transform.position.x > base.transform.position.x && playerController.IsFacingRight))
		{
			playerController.CharacterCorgi.Flip(false, false);
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x060045DB RID: 17883 RVA: 0x000265E4 File Offset: 0x000247E4
	public IEnumerator LoadWorldCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		PlayerManager.GetPlayerController().ControllerCorgi.enabled = false;
		this.m_charon.SetNPCState(NPCState.Emote1, false);
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.GoldCollected > 0 && !this.m_useGardenLogic)
		{
			AudioManager.PlayOneShot(this, this.m_tollPaidAudioEvent, default(Vector3));
			SaveManager.PlayerSaveData.GiveMoneyToCharon(this);
		}
		this.m_charon.SetNPCState(NPCState.Emote2, false);
		Prop pizzaGirl = null;
		if (!this.m_useGardenLogic)
		{
			PropSpawnController propSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("PizzaGirl", false, false);
			if (propSpawnController)
			{
				pizzaGirl = propSpawnController.PropInstance;
			}
			if (pizzaGirl && NPCType_RL.IsNPCUnlocked(NPCType.PizzaGirl))
			{
				yield return this.PizzaGirlAnimCoroutine(pizzaGirl);
			}
		}
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		CinemachineVirtualCamera cinemachineVirtualCamera = CameraController.CinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
		if (cinemachineVirtualCamera)
		{
			cinemachineVirtualCamera.enabled = false;
			TweenManager.TweenBy(CameraController.GameCamera.transform, this.m_cameraShiftDuration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"localPosition.x",
				this.m_cameraShiftAmount
			});
		}
		GameObject gameObject = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("CharonBoat");
		if (gameObject)
		{
			AudioManager.PlayOneShotAttached(this, this.m_boatMoveAudioEvent, base.gameObject);
			PropSpawnController component = gameObject.GetComponent<PropSpawnController>();
			if (component.PropInstance)
			{
				this.m_boatBoatMoveTween = TweenManager.TweenBy(component.PropInstance.gameObject.transform, this.m_boatShiftDuration, new EaseDelegate(Ease.Quad.EaseIn), new object[]
				{
					"localPosition.x",
					this.m_boatShiftAmount
				});
				this.m_charonBoatMoveTween = TweenManager.TweenBy(this.m_charon.transform, this.m_boatShiftDuration, new EaseDelegate(Ease.Quad.EaseIn), new object[]
				{
					"localPosition.x",
					this.m_boatShiftAmount
				});
				this.m_playerBoatMoveTween = TweenManager.TweenBy(playerController.transform, this.m_boatShiftDuration, new EaseDelegate(Ease.Quad.EaseIn), new object[]
				{
					"localPosition.x",
					this.m_boatShiftAmount
				});
				if (pizzaGirl)
				{
					TweenManager.TweenBy(pizzaGirl.transform, this.m_boatShiftDuration, new EaseDelegate(Ease.Quad.EaseIn), new object[]
					{
						"position.x",
						this.m_boatShiftAmount
					});
				}
			}
		}
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (currentPlayerRoom)
		{
			foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
			{
				if (enemySpawnController.EnemyInstance)
				{
					enemySpawnController.EnemyInstance.StatusEffectController.StopAllStatusEffects(false);
				}
			}
		}
		this.m_waitYield.CreateNew(this.m_totalDelayBeforeLoading, false);
		yield return this.m_waitYield;
		playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		if (!this.m_useGardenLogic)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitHubTown, this, EventArgs.Empty);
			SceneLoader_RL.LoadScene(SceneID.World, TransitionID.CastleGate);
		}
		else
		{
			SceneLoader_RL.RunTransitionWithLogic(this.EnterDocksCoroutine(), TransitionID.FadeToBlackNoLoading, false);
		}
		yield break;
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x000265F3 File Offset: 0x000247F3
	private IEnumerator PizzaGirlAnimCoroutine(Prop pizzaGirl)
	{
		pizzaGirl.GetComponentInChildren<SpeechBubbleController>(true).SetSpeechBubbleEnabled(false);
		pizzaGirl.Animators[0].SetTrigger("Turn");
		pizzaGirl.transform.SetLocalScaleX(-1f);
		float delay = Time.time + 0.1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		pizzaGirl.Animators[0].SetTrigger("Jump_Tell");
		delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		pizzaGirl.Animators[0].SetTrigger("Jump");
		Coroutine ySpeedCoroutine = base.StartCoroutine(this.YSpeedCoroutine(pizzaGirl));
		pizzaGirl.CorgiController.ResetPermanentDisable();
		pizzaGirl.CorgiController.PermanentlyDisableUponTouchingPlatform = false;
		pizzaGirl.CorgiController.enabled = true;
		pizzaGirl.CorgiController.SetVerticalForce(15f);
		yield return TweenManager.TweenTo(pizzaGirl.transform, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.x",
			this.m_pizzaGirlPositionObj.transform.position.x
		}).TweenCoroutine;
		while (!pizzaGirl.CorgiController.State.IsGrounded)
		{
			yield return null;
		}
		pizzaGirl.CorgiController.SetForce(Vector2.zero);
		base.StopCoroutine(ySpeedCoroutine);
		pizzaGirl.Animators[0].SetTrigger("Land");
		pizzaGirl.CorgiController.enabled = false;
		yield break;
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x00026609 File Offset: 0x00024809
	private IEnumerator YSpeedCoroutine(Prop pizzaGirl)
	{
		for (;;)
		{
			pizzaGirl.Animators[0].SetFloat("ySpeed", pizzaGirl.CorgiController.Velocity.y);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x00026618 File Offset: 0x00024818
	private IEnumerator EnterDocksCoroutine()
	{
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		this.m_boatBoatMoveTween.StopTween(false);
		this.m_charonBoatMoveTween.StopTween(false);
		this.m_playerBoatMoveTween.StopTween(false);
		PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("DocksTunnel", false, false).Tunnel.ForceEnterTunnel(false, null);
		yield break;
	}

	// Token: 0x060045E0 RID: 17888 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040035DE RID: 13790
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040035DF RID: 13791
	[SerializeField]
	private GameObject m_pizzaGirlPositionObj;

	// Token: 0x040035E0 RID: 13792
	[SerializeField]
	private NPCController m_charon;

	// Token: 0x040035E1 RID: 13793
	[SerializeField]
	private bool m_useGardenLogic;

	// Token: 0x040035E2 RID: 13794
	[Header("Animation Timings")]
	[SerializeField]
	private float m_cameraShiftAmount = 2f;

	// Token: 0x040035E3 RID: 13795
	[SerializeField]
	private float m_cameraShiftDuration = 3f;

	// Token: 0x040035E4 RID: 13796
	[SerializeField]
	private float m_boatShiftAmount = 3f;

	// Token: 0x040035E5 RID: 13797
	[SerializeField]
	private float m_boatShiftDuration = 3f;

	// Token: 0x040035E6 RID: 13798
	[SerializeField]
	private float m_totalDelayBeforeLoading = 2f;

	// Token: 0x040035E7 RID: 13799
	[SerializeField]
	[EventRef]
	private string m_boatMoveAudioEvent;

	// Token: 0x040035E8 RID: 13800
	[SerializeField]
	[EventRef]
	private string m_charonWindowOpenedAudioEvent;

	// Token: 0x040035E9 RID: 13801
	[SerializeField]
	[EventRef]
	private string m_cancelEnterCastleAudioEvent;

	// Token: 0x040035EA RID: 13802
	[SerializeField]
	[EventRef]
	private string m_chooseEnterCastleAudioEvent;

	// Token: 0x040035EB RID: 13803
	[SerializeField]
	[EventRef]
	private string m_tollPaidAudioEvent;

	// Token: 0x040035EC RID: 13804
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040035ED RID: 13805
	private WaitRL_Yield m_waitYield;

	// Token: 0x040035EE RID: 13806
	private Interactable m_interactable;

	// Token: 0x040035EF RID: 13807
	private bool m_hasSpoken;

	// Token: 0x040035F0 RID: 13808
	private Tween m_playerBoatMoveTween;

	// Token: 0x040035F1 RID: 13809
	private Tween m_boatBoatMoveTween;

	// Token: 0x040035F2 RID: 13810
	private Tween m_charonBoatMoveTween;

	// Token: 0x040035F3 RID: 13811
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040035F4 RID: 13812
	private Action m_runEnterCastleMenu;

	// Token: 0x040035F5 RID: 13813
	private Action m_closeCharon;

	// Token: 0x040035F6 RID: 13814
	private Action m_cancelEnterCastleSelection;

	// Token: 0x040035F7 RID: 13815
	private Action m_confirmEnterCastleSelection;
}
