using System;
using System.Collections;
using System.Collections.Generic;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class HeirloomStatuePropController : BaseSpecialPropController, IRoomConsumer
{
	// Token: 0x06002E1F RID: 11807 RVA: 0x0009BC4C File Offset: 0x00099E4C
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
		this.m_storedIconScale = this.m_iconSprite.transform.localScale;
		this.m_iconStartingY = this.m_iconSprite.transform.localPosition.y;
		base.gameObject.GetComponentsInChildren<ParticleSystem>(false, this.m_nonAllocParticleSystemList);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_challengeHeirloom = new Action(this.ChallengeHeirloom);
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x0009BCFC File Offset: 0x00099EFC
	protected override void DisableProp(bool firstTimeDisabled)
	{
		base.DisableProp(firstTimeDisabled);
		this.m_heirloomParticlesGO.SetActive(false);
		this.m_iconSprite.gameObject.SetActive(false);
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x0009BD24 File Offset: 0x00099F24
	protected override void InitializePooledPropOnEnter()
	{
		this.m_heirloomRoom = base.Room.gameObject.GetComponent<HeirloomRoomController>();
		if (this.m_heirloomRoom != null)
		{
			this.m_iconSprite.sprite = IconLibrary.GetHeirloomSprite(this.m_heirloomRoom.HeirloomType);
		}
		this.m_disableIconHoverAnim = false;
		this.m_iconSprite.transform.localScale = this.m_storedIconScale;
		this.m_heirloomParticlesGO.SetActive(true);
		this.m_iconSprite.gameObject.SetActive(true);
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x0009BDAA File Offset: 0x00099FAA
	public void TriggerHeirloom()
	{
		if (!this.m_heirloomRoom.IsFinalRoom)
		{
			this.InitializeConfirmMenu();
			base.StartCoroutine(this.TriggerHeirloomCoroutine());
			return;
		}
		base.StartCoroutine(this.TriggerHeirloomCompleteCoroutine());
	}

	// Token: 0x06002E23 RID: 11811 RVA: 0x0009BDDC File Offset: 0x00099FDC
	private void InitializeConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_HEIRLOOM_UI_CHALLENGE_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_HEIRLOOM_UI_CHALLENGE_TEXT_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_challengeHeirloom);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06002E24 RID: 11812 RVA: 0x0009BE6B File Offset: 0x0009A06B
	private IEnumerator TriggerHeirloomCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPosObj.transform.position;
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
		PlayerMovementHelper.ResumeAllMovementInput();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (this.m_heirloomRoom)
		{
			if (Heirloom_EV.IsHeirloomLocked(this.m_heirloomRoom.HeirloomType))
			{
				HeirloomDialogueEntry dialogueEntry = Heirloom_EV.GetDialogueEntry(this.m_heirloomRoom.HeirloomType);
				if (dialogueEntry != null)
				{
					ref HeirloomTextEntry ptr = dialogueEntry.HeirloomDialogue[0];
					string lockedHeirloomLocID = Heirloom_EV.GetLockedHeirloomLocID(this.m_heirloomRoom.HeirloomType, this.m_heirloomRoom.HeirloomLockedRepeatDialogue);
					this.m_heirloomRoom.HeirloomLockedRepeatDialogue = true;
					DialogueManager.StartNewDialogue(null, NPCState.Idle);
					DialogueManager.AddDialogue(ptr.DialogueTitleLocID, lockedHeirloomLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
					WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
					AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_heirloom_greeting", base.transform.position);
				}
			}
			else
			{
				HeirloomDialogueEntry dialogueEntry2 = Heirloom_EV.GetDialogueEntry(this.m_heirloomRoom.HeirloomType);
				if (dialogueEntry2 != null)
				{
					HeirloomTextEntry heirloomTextEntry;
					if (this.m_heirloomRoom.HeirloomDialogueIndex == 0)
					{
						heirloomTextEntry = dialogueEntry2.HeirloomDialogue[(int)this.m_heirloomRoom.HeirloomDialogueIndex];
						HeirloomRoomController heirloomRoom = this.m_heirloomRoom;
						byte heirloomDialogueIndex = heirloomRoom.HeirloomDialogueIndex;
						heirloomRoom.HeirloomDialogueIndex = heirloomDialogueIndex + 1;
					}
					else
					{
						if (dialogueEntry2.RandomizeRepeatDialogues)
						{
							this.m_heirloomRoom.HeirloomDialogueIndex = (byte)UnityEngine.Random.Range(1, dialogueEntry2.HeirloomDialogue.Length);
						}
						else
						{
							HeirloomRoomController heirloomRoom2 = this.m_heirloomRoom;
							byte heirloomDialogueIndex = heirloomRoom2.HeirloomDialogueIndex;
							heirloomRoom2.HeirloomDialogueIndex = heirloomDialogueIndex + 1;
						}
						this.m_heirloomRoom.HeirloomDialogueIndex = (byte)Mathf.Clamp((int)this.m_heirloomRoom.HeirloomDialogueIndex, 1, dialogueEntry2.HeirloomDialogue.Length - 1);
						heirloomTextEntry = dialogueEntry2.HeirloomDialogue[(int)this.m_heirloomRoom.HeirloomDialogueIndex];
					}
					DialogueManager.StartNewDialogue(null, NPCState.Idle);
					DialogueManager.AddDialogue(heirloomTextEntry.DialogueTitleLocID, heirloomTextEntry.DialogueTextLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
					WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
					AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_heirloom_greeting", base.transform.position);
				}
			}
		}
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		if (!Heirloom_EV.IsHeirloomLocked(this.m_heirloomRoom.HeirloomType))
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			this.m_waitYield.CreateNew(0.25f, false);
			yield return this.m_waitYield;
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
		yield break;
	}

	// Token: 0x06002E25 RID: 11813 RVA: 0x0009BE7A File Offset: 0x0009A07A
	private void CancelConfirmMenuSelection()
	{
		this.RunInsightDiscovered();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		RewiredMapController.SetCurrentMapEnabled(true);
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_heirloom_farewell", base.transform.position);
	}

	// Token: 0x06002E26 RID: 11814 RVA: 0x0009BEA7 File Offset: 0x0009A0A7
	private void ChallengeHeirloom()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.StartCoroutine(this.ChallengeHeirloomCoroutine());
	}

	// Token: 0x06002E27 RID: 11815 RVA: 0x0009BEBE File Offset: 0x0009A0BE
	private IEnumerator ChallengeHeirloomCoroutine()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_interactable.SetIsInteractableActive(false);
		this.m_storedFallMultiplier = playerController.FallMultiplierOverride;
		playerController.FallMultiplierOverride = 0.1f;
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(this.m_distortPosObj.transform.position);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = true;
		TweenManager.TweenTo(this.m_warpEffect, 1.5f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"DistortionAmount",
			0.025f
		});
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		yield return TweenManager.TweenBy(playerController.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.x",
			-0.25f
		}).TweenCoroutine;
		TweenManager.TweenBy(playerController.transform, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"localPosition.x",
			-0.5f,
			"localPosition.y",
			1
		});
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		ScreenDistortion_SceneTransition screenDistortion_SceneTransition = TransitionLibrary.GetTransitionInstance(TransitionID.ScreenDistortion) as ScreenDistortion_SceneTransition;
		screenDistortion_SceneTransition.SetDistortInPosition(this.m_distortPosObj);
		screenDistortion_SceneTransition.SetDistortOutPosition(playerController.gameObject);
		SceneLoader_RL.RunTransitionWithLogic(this.TeleportPlayer(), TransitionID.ScreenDistortion, false);
		yield break;
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x0009BECD File Offset: 0x0009A0CD
	private IEnumerator TeleportPlayer()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		TweenManager.StopAllTweensContaining(playerController.transform, false);
		playerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		playerController.SetFacing((base.SpecialRoomController as HeirloomRoomController).FaceRightAfterTeleport);
		playerController.Animator.SetBool("Victory", false);
		if (!this.m_heirloomRoom.IsFinalRoom)
		{
			playerController.CachedHealthOverride = playerController.CurrentHealth;
			playerController.CachedManaOverride = playerController.CurrentMana;
			if (!TraitManager.IsTraitActive(TraitType.MegaHealth))
			{
				playerController.SetHealth((float)playerController.ActualMaxHealth, false, true);
			}
			playerController.SetMana((float)playerController.ActualMaxMana, false, true, false);
		}
		if (!this.m_heirloomRoom.IsFinalRoom)
		{
			this.ApplyHeirloomAbility();
		}
		if (this.m_heirloomRoom.HeirloomTunnelSpawnController.Tunnel)
		{
			this.m_heirloomRoom.HeirloomTunnelSpawnController.Tunnel.ForceEnterTunnel(false, null);
		}
		else
		{
			Debug.Log("<color=red>Could not enter Heirloom. Tunnel not found on assigned tunnelSpawnController.</color>");
		}
		bool flag = false;
		if (!this.m_heirloomRoom.IsFinalRoom)
		{
			flag = this.RunInsightDiscovered();
		}
		float enterTime = Time.time;
		float delayDuration = 1.5f;
		if (flag)
		{
			delayDuration = 4f;
		}
		while (Time.time < enterTime + delayDuration)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x0009BEDC File Offset: 0x0009A0DC
	private void ApplyHeirloomAbility()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		SaveManager.PlayerSaveData.TemporaryHeirloomList.Add((base.SpecialRoomController as HeirloomRoomController).HeirloomType);
		playerController.InitializeAbilities();
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x0009BF07 File Offset: 0x0009A107
	private IEnumerator TriggerHeirloomCompleteCoroutine()
	{
		SaveManager.PlayerSaveData.SetHeirloomLevel(this.m_heirloomRoom.HeirloomType, 1, false, true);
		PlayerManager.GetPlayerController().InitializeAbilities();
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_interactable.SetIsInteractableActive(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPosObj.transform.position;
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
		PlayerMovementHelper.ResumeAllMovementInput();
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		HeirloomDialogueEntry dialogueEntry = Heirloom_EV.GetDialogueEntry(this.m_heirloomRoom.HeirloomType);
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue(dialogueEntry.HeirloomCompleteDialogue.DialogueTitleLocID, dialogueEntry.HeirloomCompleteDialogue.DialogueTextLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_heirloom_greeting", base.transform.position);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		playerController.Animator.SetBool("Victory", true);
		this.m_waitYield.CreateNew(0.75f, false);
		yield return this.m_waitYield;
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		HeirloomDrop heirloomDrop = new HeirloomDrop(this.m_heirloomRoom.HeirloomType);
		(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(heirloomDrop);
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		ScreenDistortion_SceneTransition screenDistortion_SceneTransition = TransitionLibrary.GetTransitionInstance(TransitionID.ScreenDistortion) as ScreenDistortion_SceneTransition;
		screenDistortion_SceneTransition.SetDistortInPosition(this.m_distortPosObj);
		screenDistortion_SceneTransition.SetDistortOutPosition(playerController.gameObject);
		SceneLoader_RL.RunTransitionWithLogic(this.TeleportPlayer(), TransitionID.ScreenDistortion, false);
		yield break;
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x0009BF18 File Offset: 0x0009A118
	private bool RunInsightDiscovered()
	{
		InsightType insightFromHeirloomType = InsightType_RL.GetInsightFromHeirloomType(this.m_heirloomRoom.HeirloomType);
		if (insightFromHeirloomType != InsightType.None)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(insightFromHeirloomType) < InsightState.DiscoveredButNotViewed)
			{
				this.m_insightEventArgs.Initialize(insightFromHeirloomType, true, 5f, null, null, null);
				SaveManager.PlayerSaveData.SetInsightState(insightFromHeirloomType, InsightState.DiscoveredButNotViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
				return true;
			}
		}
		else
		{
			Debug.Log("<color=yellow>Could not 'Insight Discovered' from Heirloom type: " + this.m_heirloomRoom.HeirloomType.ToString() + "</color>");
		}
		return false;
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x0009BFA8 File Offset: 0x0009A1A8
	protected void Update()
	{
		if (!this.m_disableIconHoverAnim)
		{
			Vector3 localPosition = this.m_iconSprite.transform.localPosition;
			localPosition.y = this.m_iconStartingY + Mathf.Sin(Time.time * 2f) * 0.1f;
			this.m_iconSprite.transform.localPosition = localPosition;
		}
	}

	// Token: 0x040024CA RID: 9418
	private const string SFX_GREETING_NAME = "event:/SFX/Interactables/sfx_heirloom_greeting";

	// Token: 0x040024CB RID: 9419
	private const string SFX_FAREWELL_NAME = "event:/SFX/Interactables/sfx_heirloom_farewell";

	// Token: 0x040024CC RID: 9420
	[SerializeField]
	private GameObject m_playerPosObj;

	// Token: 0x040024CD RID: 9421
	[SerializeField]
	private GameObject m_distortPosObj;

	// Token: 0x040024CE RID: 9422
	[SerializeField]
	private GameObject m_heirloomParticlesGO;

	// Token: 0x040024CF RID: 9423
	[SerializeField]
	private SpriteRenderer m_iconSprite;

	// Token: 0x040024D0 RID: 9424
	private WaitRL_Yield m_waitYield;

	// Token: 0x040024D1 RID: 9425
	private HeirloomWarp_Effect m_warpEffect;

	// Token: 0x040024D2 RID: 9426
	private float m_iconStartingY;

	// Token: 0x040024D3 RID: 9427
	private HeirloomRoomController m_heirloomRoom;

	// Token: 0x040024D4 RID: 9428
	private Vector3 m_storedIconScale;

	// Token: 0x040024D5 RID: 9429
	private bool m_disableIconHoverAnim;

	// Token: 0x040024D6 RID: 9430
	private List<ParticleSystem> m_nonAllocParticleSystemList = new List<ParticleSystem>();

	// Token: 0x040024D7 RID: 9431
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x040024D8 RID: 9432
	private float m_storedFallMultiplier;

	// Token: 0x040024D9 RID: 9433
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040024DA RID: 9434
	private Action m_challengeHeirloom;
}
