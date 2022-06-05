using System;
using System.Collections;
using Cinemachine;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class CharonShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x060031B8 RID: 12728 RVA: 0x000A81E2 File Offset: 0x000A63E2
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000A81E5 File Offset: 0x000A63E5
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x060031BA RID: 12730 RVA: 0x000A81ED File Offset: 0x000A63ED
	// (set) Token: 0x060031BB RID: 12731 RVA: 0x000A81F5 File Offset: 0x000A63F5
	public BaseRoom Room { get; private set; }

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x060031BC RID: 12732 RVA: 0x000A81FE File Offset: 0x000A63FE
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x000A8208 File Offset: 0x000A6408
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

	// Token: 0x060031BE RID: 12734 RVA: 0x000A8264 File Offset: 0x000A6464
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_runEnterCastleMenu = new Action(this.RunEnterCastleMenu);
		this.m_closeCharon = new Action(this.CloseCharon);
		this.m_cancelEnterCastleSelection = new Action(this.CancelEnterCastleSelection);
		this.m_confirmEnterCastleSelection = new Action(this.ConfirmEnterCastleSelection);
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000A82D6 File Offset: 0x000A64D6
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000A82FD File Offset: 0x000A64FD
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000A8329 File Offset: 0x000A6529
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		this.m_hasSpoken = false;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x000A8339 File Offset: 0x000A6539
	public void OpenCharon()
	{
		this.m_interactable.SetIsInteractableActive(false);
		RewiredMapController.SetCurrentMapEnabled(false);
		base.StartCoroutine(this.OpenCharonCoroutine());
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x000A835C File Offset: 0x000A655C
	private void CloseCharon()
	{
		this.m_interactable.SetIsInteractableActive(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		this.m_charon.SetNPCState(NPCState.Idle, false);
		AudioManager.PlayOneShot(this, this.m_cancelEnterCastleAudioEvent, default(Vector3));
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000A839D File Offset: 0x000A659D
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

	// Token: 0x060031C5 RID: 12741 RVA: 0x000A83AC File Offset: 0x000A65AC
	private void RunCharonIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CharonDialogue_Intro, true);
		string textLocID = "LOC_ID_DIALOGUE_CHARON_FEE_TAKE_1";
		DialogueManager.StartNewDialogue(this.m_charon, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_DIALOGUE_CHARON_TITLE_MAIN_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_runEnterCastleMenu);
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x000A8410 File Offset: 0x000A6610
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

	// Token: 0x060031C7 RID: 12743 RVA: 0x000A8498 File Offset: 0x000A6698
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

	// Token: 0x060031C8 RID: 12744 RVA: 0x000A8538 File Offset: 0x000A6738
	private void RunEnterCastleMenu()
	{
		this.InitializeEnterCastleConfirmMenu();
		AudioManager.PlayOneShot(this, this.m_charonWindowOpenedAudioEvent, default(Vector3));
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x000A8568 File Offset: 0x000A6768
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

	// Token: 0x060031CA RID: 12746 RVA: 0x000A8615 File Offset: 0x000A6815
	private void CancelEnterCastleSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.CloseCharon();
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x000A8628 File Offset: 0x000A6828
	private void ConfirmEnterCastleSelection()
	{
		AudioManager.PlayOneShot(this, this.m_chooseEnterCastleAudioEvent, default(Vector3));
		base.GetComponent<Interactable>().SetIsInteractableActive(false);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.StartCoroutine(this.LoadWorldCoroutine());
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x000A866B File Offset: 0x000A686B
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

	// Token: 0x060031CD RID: 12749 RVA: 0x000A867A File Offset: 0x000A687A
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

	// Token: 0x060031CE RID: 12750 RVA: 0x000A8689 File Offset: 0x000A6889
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

	// Token: 0x060031CF RID: 12751 RVA: 0x000A869F File Offset: 0x000A689F
	private IEnumerator YSpeedCoroutine(Prop pizzaGirl)
	{
		for (;;)
		{
			pizzaGirl.Animators[0].SetFloat("ySpeed", pizzaGirl.CorgiController.Velocity.y);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x000A86AE File Offset: 0x000A68AE
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

	// Token: 0x060031D2 RID: 12754 RVA: 0x000A86FC File Offset: 0x000A68FC
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002721 RID: 10017
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04002722 RID: 10018
	[SerializeField]
	private GameObject m_pizzaGirlPositionObj;

	// Token: 0x04002723 RID: 10019
	[SerializeField]
	private NPCController m_charon;

	// Token: 0x04002724 RID: 10020
	[SerializeField]
	private bool m_useGardenLogic;

	// Token: 0x04002725 RID: 10021
	[Header("Animation Timings")]
	[SerializeField]
	private float m_cameraShiftAmount = 2f;

	// Token: 0x04002726 RID: 10022
	[SerializeField]
	private float m_cameraShiftDuration = 3f;

	// Token: 0x04002727 RID: 10023
	[SerializeField]
	private float m_boatShiftAmount = 3f;

	// Token: 0x04002728 RID: 10024
	[SerializeField]
	private float m_boatShiftDuration = 3f;

	// Token: 0x04002729 RID: 10025
	[SerializeField]
	private float m_totalDelayBeforeLoading = 2f;

	// Token: 0x0400272A RID: 10026
	[SerializeField]
	[EventRef]
	private string m_boatMoveAudioEvent;

	// Token: 0x0400272B RID: 10027
	[SerializeField]
	[EventRef]
	private string m_charonWindowOpenedAudioEvent;

	// Token: 0x0400272C RID: 10028
	[SerializeField]
	[EventRef]
	private string m_cancelEnterCastleAudioEvent;

	// Token: 0x0400272D RID: 10029
	[SerializeField]
	[EventRef]
	private string m_chooseEnterCastleAudioEvent;

	// Token: 0x0400272E RID: 10030
	[SerializeField]
	[EventRef]
	private string m_tollPaidAudioEvent;

	// Token: 0x0400272F RID: 10031
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04002730 RID: 10032
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002731 RID: 10033
	private Interactable m_interactable;

	// Token: 0x04002732 RID: 10034
	private bool m_hasSpoken;

	// Token: 0x04002733 RID: 10035
	private Tween m_playerBoatMoveTween;

	// Token: 0x04002734 RID: 10036
	private Tween m_boatBoatMoveTween;

	// Token: 0x04002735 RID: 10037
	private Tween m_charonBoatMoveTween;

	// Token: 0x04002736 RID: 10038
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04002737 RID: 10039
	private Action m_runEnterCastleMenu;

	// Token: 0x04002738 RID: 10040
	private Action m_closeCharon;

	// Token: 0x04002739 RID: 10041
	private Action m_cancelEnterCastleSelection;

	// Token: 0x0400273A RID: 10042
	private Action m_confirmEnterCastleSelection;
}
