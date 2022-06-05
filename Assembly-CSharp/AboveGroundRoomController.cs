using System;
using System.Collections;
using Cinemachine;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class AboveGroundRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F05 RID: 12037 RVA: 0x000A0484 File Offset: 0x0009E684
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_followBlend.m_Time = 1f;
		this.m_followBlend.m_Style = CinemachineBlendDefinition.Style.Custom;
		this.m_followBlend.m_CustomCurve = this.m_trueEndBlend;
		this.m_newGamePlusConfirmComplete = new Action(this.NewGamePlusConfirmComplete);
		this.m_skillTreeWindowClosed = new Action<object, EventArgs>(this.SkillTreeWindowClosed);
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x000A04F9 File Offset: 0x0009E6F9
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000A0508 File Offset: 0x0009E708
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000A0517 File Offset: 0x0009E717
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ResetCharacter();
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterHubTown, this, null);
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000A0534 File Offset: 0x0009E734
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CurrentArmor = playerController.ActualArmor;
		playerController.ResetMana();
		playerController.ResetHealth();
		playerController.ResetAllAbilityAmmo();
		playerController.ResetAbilityCooldowns();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StopGlobalTimer, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HideGlobalTimer, null, null);
		this.m_hugPicRenderer.gameObject.SetActive(false);
		bool flag = !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding) && SaveManager.PlayerSaveData.TimesBeatenTraitor >= Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2 && BurdenManager.GetBurdenLevel(BurdenType.CastleBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.BridgeBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.ForestBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.StudyBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.TowerBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.CaveBossUp) > 0 && BurdenManager.GetBurdenLevel(BurdenType.FinalBossUp) > 0;
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding))
			{
				this.m_dragonSpawner.PropInstance.gameObject.SetActive(false);
				this.m_traitorSpawner.PropInstance.gameObject.SetActive(false);
			}
			if (flag)
			{
				this.m_trueEndTrigger.SetActive(true);
			}
			else
			{
				this.m_trueEndTrigger.SetActive(false);
			}
			this.m_paradeWall.gameObject.SetActive(false);
			this.m_skillTreeWall.gameObject.SetActive(true);
		}
		else
		{
			this.m_trueEndTrigger.SetActive(false);
			this.m_paradeSign.PropInstance.gameObject.SetActive(false);
			this.m_paradeWall.gameObject.SetActive(true);
			this.m_skillTreeWall.gameObject.SetActive(false);
			this.m_manorDoorProp.PropInstance.gameObject.SetActive(false);
			this.m_charonProp.PropInstance.transform.position = this.m_charonPostParadeSpawnPos.transform.position;
			this.m_charonProp.PropInstance.transform.SetLocalScaleX(Mathf.Abs(this.m_charonProp.PropInstance.transform.localScale.x));
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding))
			{
				this.m_dragonSpawner.PropInstance.gameObject.SetActive(false);
				this.m_traitorSpawner.PropInstance.gameObject.SetActive(false);
			}
		}
		base.StartCoroutine(this.FlipPlayer());
		base.StartCoroutine(this.UpdateHeartStateCoroutine());
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			base.StartCoroutine(this.GiveAchievementsCoroutine());
		}
		else
		{
			base.StartCoroutine(this.OpenSkillTreeCoroutine());
		}
		if (SaveManager.PlayerSaveData.NewGamePlusLevel > SaveManager.PlayerSaveData.HighestNGPlusBeaten)
		{
			SaveManager.PlayerSaveData.HighestNGPlusBeaten = SaveManager.PlayerSaveData.NewGamePlusLevel;
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		}
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x000A0818 File Offset: 0x0009EA18
	private IEnumerator OpenSkillTreeCoroutine()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		while (!base.Room.CinemachineCamera.IsActiveVirtualCamera)
		{
			yield return null;
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
		{
			WindowManager.LoadWindow(WindowID.SkillTree);
		}
		WindowManager.SetWindowIsOpen(WindowID.SkillTree, true);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_castle_open", default(Vector3));
		while (WindowManager.GetIsWindowOpen(WindowID.SkillTree))
		{
			yield return null;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenNGConfirmWindow))
		{
			yield return this.OpenNewGamePlusConfirmWindowCoroutine(2f);
		}
		yield break;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000A0827 File Offset: 0x0009EA27
	private IEnumerator GiveAchievementsCoroutine()
	{
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		StoreAPIManager.GiveAchievement(AchievementType.PrequelReveal, StoreType.All);
		bool flag = true;
		foreach (NPCType npctype in NPCType_RL.TypeArray)
		{
			if (npctype != NPCType.None && npctype != NPCType.Johan && npctype != NPCType.Jukebox && !NPCController.GetBestFriendState(npctype))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllFriends, StoreType.All);
		}
		yield break;
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x000A082F File Offset: 0x0009EA2F
	private IEnumerator UpdateHeartStateCoroutine()
	{
		yield return null;
		this.UpdateHeartState("Charon", false);
		this.UpdateHeartState("Architect", false);
		this.UpdateHeartState("OffshoreBank", false);
		this.UpdateHeartState("Dummy", true);
		this.UpdateHeartState("Totem", false);
		this.UpdateHeartState("Geras", false);
		this.UpdateHeartState("Elpis", false);
		this.UpdateHeartState("Keres", false);
		this.UpdateHeartState("Blacksmith", false);
		this.UpdateHeartState("Enchantress", false);
		this.UpdateHeartState("PizzaGirl", false);
		this.UpdateHeartState("Traitor", false);
		this.UpdateHeartState("Dragon", false);
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.m_traitorSpawner.PropInstance.GetComponent<NPCController>().ShowHeart();
		}
		yield break;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x000A0840 File Offset: 0x0009EA40
	private void UpdateHeartState(string objName, bool isEnemySpawner)
	{
		if (!isEnemySpawner)
		{
			PropSpawnController propSpawnController = base.Room.gameObject.FindObjectReference(objName, false, false);
			if (propSpawnController && propSpawnController.PropInstance)
			{
				NPCController component = propSpawnController.PropInstance.GetComponent<NPCController>();
				if (component)
				{
					component.UpdateHeartState();
					return;
				}
			}
		}
		else
		{
			EnemySpawnController enemySpawnController = base.Room.gameObject.FindObjectReference(objName, false, false);
			if (enemySpawnController && enemySpawnController.EnemyInstance)
			{
				NPCController component2 = enemySpawnController.EnemyInstance.GetComponent<NPCController>();
				if (component2)
				{
					component2.UpdateHeartState();
				}
			}
		}
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000A08D7 File Offset: 0x0009EAD7
	private IEnumerator FlipPlayer()
	{
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsFacingRight)
		{
			playerController.CharacterCorgi.Flip(true, true);
		}
		yield break;
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000A08DF File Offset: 0x0009EADF
	public void PlayTrueEnding()
	{
		base.StartCoroutine(this.TrueEndingCoroutine());
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000A08EE File Offset: 0x0009EAEE
	private IEnumerator TrueEndingCoroutine()
	{
		PlayerController player = PlayerManager.GetPlayerController();
		Prop dragon = this.m_dragonSpawner.PropInstance;
		Animator dragonAnimator = dragon.Animators[0];
		Prop dragonSilhouette = this.m_dragonSilhouetteSpawner.PropInstance;
		Prop traitor = this.m_traitorSpawner.PropInstance;
		Animator traitorAnimator = traitor.Animators[0];
		CorgiController_RL traitorCorgi = traitor.GetComponent<CorgiController_RL>();
		traitorCorgi.PermanentlyDisableUponTouchingPlatform = false;
		Prop pizzaGirl = this.m_pizzaGirlProp.PropInstance;
		CinemachineVirtualCameraManager cinemachineCamera = base.Room.CinemachineCamera;
		CinemachineBlendDefinition storedBlend = CameraController.CinemachineBrain.m_DefaultBlend;
		CameraController.CinemachineBrain.m_DefaultBlend = this.m_followBlend;
		Vector2 vector = Vector2.zero;
		RewiredMapController.SetIsInCutscene(true);
		RewiredMapController.SetCurrentMapEnabled(false);
		player.SetVelocity(0f, 0f, false);
		player.StopActiveAbilities(true);
		while (!player.IsGrounded)
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		base.Room.CinemachineCamera.SetIsActiveCamera(false);
		this.m_trueEndVcam.VirtualCamera.gameObject.SetActive(true);
		float zoomLevel = CameraController.ZoomLevel;
		if (zoomLevel != 1f)
		{
			this.m_trueEndVcam.SetLensSize(CameraController.GetVirtualCameraLensSize(zoomLevel));
		}
		this.m_trueEndVcam.SetIsActiveCamera(true);
		this.m_trueEndFollowObj.position = this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.SilhouetteCamPos);
		this.m_trueEndVcam.SetFollowTarget(this.m_trueEndFollowObj);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_trueEnding_01_dragonCall", this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.DragonCamPos));
		this.m_waitYield.CreateNew(2f, false);
		yield return this.m_waitYield;
		AudioManager.PlayOneShotAttached(null, "event:/UI/FrontEnd/ui_fe_trueEnding_02_dragonSwoop", dragonSilhouette.gameObject);
		TweenManager.TweenBy(dragonSilhouette.transform, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"position.y",
			100
		});
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		EffectManager.PlayEffect(player.gameObject, null, "CameraShakeVerySmall_Effect", Vector3.zero, 0.5f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		vector = this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.DragonCamPos);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_trueEnding_03_dragonApproach", vector);
		yield return TweenManager.TweenTo(this.m_trueEndFollowObj, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			vector.x,
			"position.y",
			vector.y
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		dragon.gameObject.SetActive(true);
		dragon.transform.SetPositionX(dragon.transform.position.x - 5f);
		dragon.transform.SetPositionY(dragon.transform.position.y + 30f);
		dragon.GetComponent<DragonPropController>().SetEndingCutsceneStateEnabled(true);
		dragonAnimator.enabled = true;
		dragonAnimator.SetBool("Idle", false);
		dragonAnimator.SetTrigger("Fall");
		AudioManager.PlayDelayedOneShot(null, "event:/UI/FrontEnd/ui_fe_trueEnding_04_dragonLand", this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.DragonCamPos), 1.5f);
		yield return TweenManager.TweenBy(dragon.transform, 2f, new EaseDelegate(Ease.Expo.EaseIn), new object[]
		{
			"position.x",
			5,
			"position.y",
			-30
		}).TweenCoroutine;
		dragonAnimator.SetBool("Idle", true);
		EffectManager.PlayEffect(player.gameObject, null, "CameraShakeSmall_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_waitYield.CreateNew(2f, false);
		yield return this.m_waitYield;
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_trueEnding_05_dragonSettle", this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.DragonCamPos));
		dragonAnimator.SetBool("OpenMouth", true);
		this.m_waitYield.CreateNew(2.5f, false);
		yield return this.m_waitYield;
		vector = this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.TraitorSpawnPosStart);
		AudioManager.PlayOneShotAttached(null, "event:/UI/FrontEnd/ui_fe_trueEnding_06_dragonSpit", traitor.gameObject);
		traitor.gameObject.SetActive(true);
		traitor.GetComponent<JohanPropController>().SetEndingCutsceneStateEnabled(true);
		traitorAnimator.enabled = true;
		traitor.transform.position = vector;
		traitorAnimator.Play("Idle", 0);
		traitorAnimator.Play("DodgeRoll_Hold", 1);
		traitorCorgi.SetVerticalForce(15f);
		EffectManager.PlayEffect(dragon.gameObject, dragonAnimator, "DragonSpit_Effect", vector, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		vector = this.GetTrueEndPosition(AboveGroundRoomController.TrueEndPos.TraitorSpawnPosEnd);
		TweenManager.TweenTo(traitor.transform, 3f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.x",
			vector.x
		});
		TweenManager.TweenTo(this.m_trueEndFollowObj, 3f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			vector.x,
			"position.y",
			vector.y
		});
		while (Mathf.Abs(traitor.transform.position.x - player.transform.position.x) > 3f)
		{
			yield return null;
		}
		base.StartCoroutine(this.PlayerJumpTraitorCoroutine());
		dragonAnimator.SetBool("OpenMouth", false);
		dragonAnimator.Play("Idle_Hold");
		traitorAnimator.Play("Empty", 1);
		traitorAnimator.Play("Death_4", 0, 1f);
		this.m_waitYield.CreateNew(2.5f, false);
		yield return this.m_waitYield;
		pizzaGirl.transform.SetLocalScaleX(pizzaGirl.transform.localScale.x * -1f);
		AudioManager.PlayOneShot(null, "event:/SFX/Interactables/sfx_pizzaGirl_farewell", pizzaGirl.transform.position);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		if (!WindowManager.GetIsWindowLoaded(WindowID.Dialogue))
		{
			WindowManager.LoadWindow(WindowID.Dialogue);
		}
		DialogueManager.StartNewDialogue(pizzaGirl.GetComponent<NPCController>(), NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_CUTSCENE_J_AND_Z_Z_SEES_JONAH_1", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		traitorCorgi.SetVerticalForce(5f);
		traitorAnimator.Play("JumpBlendTree");
		this.m_waitYield.CreateNew(0.1f, false);
		yield return this.m_waitYield;
		while (!traitorCorgi.State.IsGrounded)
		{
			yield return null;
		}
		traitorAnimator.Play("Idle");
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		DialogueManager.StartNewDialogue(traitor.GetComponent<NPCController>(), NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", "LOC_ID_CUTSCENE_J_AND_Z_JONAH_SAYS_SOMETHING_AFTER_GRINDING_1", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		this.m_hugPicRenderer.gameObject.SetActive(true);
		Color color = this.m_hugPicRenderer.color;
		color.a = 0f;
		this.m_hugPicRenderer.color = color;
		Vector3 position = this.m_hugPicRenderer.transform.position;
		position.y -= 1f;
		this.m_hugPicRenderer.transform.position = position;
		TweenManager.TweenBy(this.m_hugPicRenderer.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.y",
			1
		});
		yield return TweenManager.TweenTo(this.m_hugPicRenderer, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"color.a",
			1
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_CUTSCENE_J_AND_Z_Z_TALKS_HUG_1", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", "LOC_ID_CUTSCENE_J_AND_Z_J_TALKS_GOOP_1", false, DialogueWindowStyle.HorizontalLower, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_CUTSCENE_J_AND_Z_Z_TALKS_MISSED_YOU_1", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", "LOC_ID_CUTSCENE_J_AND_Z_J_TALKS_GRATEFUL_1", false, DialogueWindowStyle.HorizontalLower, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		pizzaGirl.transform.SetLocalScaleX(Mathf.Abs(pizzaGirl.transform.localScale.x));
		TweenManager.TweenBy(this.m_hugPicRenderer.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"position.y",
			-1
		});
		yield return TweenManager.TweenTo(this.m_hugPicRenderer, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"color.a",
			0
		}).TweenCoroutine;
		this.m_hugPicRenderer.gameObject.SetActive(false);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		this.m_trueEndVcam.VirtualCamera.gameObject.SetActive(false);
		this.m_trueEndVcam.SetIsActiveCamera(false);
		base.Room.CinemachineCamera.SetIsActiveCamera(true);
		this.m_waitYield.CreateNew(1f, false);
		yield return null;
		dragon.GetComponent<DragonPropController>().SetEndingCutsceneStateEnabled(false);
		traitor.GetComponent<JohanPropController>().SetEndingCutsceneStateEnabled(false);
		CameraController.CinemachineBrain.m_DefaultBlend = storedBlend;
		RewiredMapController.SetIsInCutscene(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SeenTrueEnding, true);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SeenTrueEnding_FirstTime, true);
		pizzaGirl.GetComponent<Interactable>().SpeechBubble.SetSpeechBubbleEnabled(true);
		yield break;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000A0900 File Offset: 0x0009EB00
	private Vector3 GetTrueEndPosition(AboveGroundRoomController.TrueEndPos posType)
	{
		if (posType < (AboveGroundRoomController.TrueEndPos)this.m_trueEndPosArrayTransform.childCount)
		{
			return this.m_trueEndPosArrayTransform.GetChild((int)posType).position;
		}
		return Vector3.zero;
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x000A0934 File Offset: 0x0009EB34
	private IEnumerator PlayerJumpTraitorCoroutine()
	{
		yield return PlayerMovementHelper.JumpPlayer(1.5f);
		PlayerManager.GetPlayerController().CharacterCorgi.Flip(true, false);
		yield break;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x000A093C File Offset: 0x0009EB3C
	private IEnumerator OpenNewGamePlusConfirmWindowCoroutine(float delay)
	{
		delay = Time.time + delay;
		while (Time.time < delay)
		{
			yield return null;
		}
		string text = (SaveManager.PlayerSaveData.TimesBeatenTraitor <= 1) ? "LOC_ID_COMPLETION_NG0_1" : "LOC_ID_COMPLETION_NG_PLUS_1";
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding_FirstTime))
		{
			text = "LOC_ID_COMPLETION_NG_FINAL_1";
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenuBig))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenuBig);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_COMPLETION_NG_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText(text, true);
		confirmMenuWindowController.SetOnCancelAction(this.m_newGamePlusConfirmComplete);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetButtonDelayTime(3f);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_NEWGAME_INFO_1", true);
		buttonAtIndex.SetOnClickAction(this.m_newGamePlusConfirmComplete);
		AudioManager.SetSFXPaused(true);
		AudioManager.PlayOneShot(null, "event:/Stingers/sting_challengComplete", default(Vector3));
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		yield break;
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x000A0952 File Offset: 0x0009EB52
	private void NewGamePlusConfirmComplete()
	{
		AudioManager.SetSFXPaused(false);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.SeenNGConfirmWindow, true);
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x000A0974 File Offset: 0x0009EB74
	private void FixedUpdate()
	{
		if (Time.time < this.m_regenTick + 0.05f)
		{
			return;
		}
		this.m_regenTick = Time.time;
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			float currentMana = playerController.CurrentMana;
			float num = (float)playerController.ActualMaxMana;
			if (currentMana < num)
			{
				float num2 = 100f * Time.fixedDeltaTime;
				if (currentMana + num2 > num)
				{
					playerController.SetMana(num, false, true, false);
				}
				else
				{
					playerController.SetMana(100f * Time.fixedDeltaTime, true, true, false);
				}
			}
			if (playerController.CurrentHealth < (float)playerController.ActualMaxHealth)
			{
				playerController.SetHealth(100f * Time.fixedDeltaTime, true, true);
			}
			if (playerController.CurrentArmor < playerController.ActualArmor)
			{
				playerController.CurrentArmor += Mathf.CeilToInt(100f * Time.fixedDeltaTime);
				playerController.SetHealth(0f, true, true);
			}
		}
	}

	// Token: 0x0400256E RID: 9582
	private const string SFX_DRAGON_CALL_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_01_dragonCall";

	// Token: 0x0400256F RID: 9583
	private const string SFX_DRAGON_SWOOP_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_02_dragonSwoop";

	// Token: 0x04002570 RID: 9584
	private const string SFX_DRAGON_APPROACH_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_03_dragonApproach";

	// Token: 0x04002571 RID: 9585
	private const string SFX_DRAGON_LAND_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_04_dragonLand";

	// Token: 0x04002572 RID: 9586
	private const string SFX_DRAGON_SETTLE_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_05_dragonSettle";

	// Token: 0x04002573 RID: 9587
	private const string SFX_DRAGON_SPIT_NAME = "event:/UI/FrontEnd/ui_fe_trueEnding_06_dragonSpit";

	// Token: 0x04002574 RID: 9588
	private const string SFX_PIZZA_GIRL_NOTICE_NAME = "event:/SFX/Interactables/sfx_pizzaGirl_farewell";

	// Token: 0x04002575 RID: 9589
	[SerializeField]
	private PropSpawnController m_paradeSign;

	// Token: 0x04002576 RID: 9590
	[SerializeField]
	private GameObject m_paradeWall;

	// Token: 0x04002577 RID: 9591
	[SerializeField]
	private GameObject m_skillTreeWall;

	// Token: 0x04002578 RID: 9592
	[SerializeField]
	private GameObject m_charonPostParadeSpawnPos;

	// Token: 0x04002579 RID: 9593
	[SerializeField]
	private PropSpawnController m_charonProp;

	// Token: 0x0400257A RID: 9594
	[SerializeField]
	private PropSpawnController m_manorDoorProp;

	// Token: 0x0400257B RID: 9595
	[Header("True Ending Props")]
	[SerializeField]
	private GameObject m_trueEndTrigger;

	// Token: 0x0400257C RID: 9596
	[SerializeField]
	private PropSpawnController m_traitorSpawner;

	// Token: 0x0400257D RID: 9597
	[SerializeField]
	private PropSpawnController m_dragonSilhouetteSpawner;

	// Token: 0x0400257E RID: 9598
	[SerializeField]
	private PropSpawnController m_dragonSpawner;

	// Token: 0x0400257F RID: 9599
	[SerializeField]
	private PropSpawnController m_pizzaGirlProp;

	// Token: 0x04002580 RID: 9600
	[SerializeField]
	private SpriteRenderer m_hugPicRenderer;

	// Token: 0x04002581 RID: 9601
	[SerializeField]
	private Transform m_trueEndFollowObj;

	// Token: 0x04002582 RID: 9602
	[SerializeField]
	private CinemachineVirtualCameraManager m_trueEndVcam;

	// Token: 0x04002583 RID: 9603
	[SerializeField]
	private AnimationCurve m_trueEndBlend;

	// Token: 0x04002584 RID: 9604
	[SerializeField]
	private Transform m_trueEndPosArrayTransform;

	// Token: 0x04002585 RID: 9605
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002586 RID: 9606
	private CinemachineBlendDefinition m_followBlend;

	// Token: 0x04002587 RID: 9607
	private Action m_newGamePlusConfirmComplete;

	// Token: 0x04002588 RID: 9608
	private Action<object, EventArgs> m_skillTreeWindowClosed;

	// Token: 0x04002589 RID: 9609
	private float m_regenTick;

	// Token: 0x02000CC6 RID: 3270
	private enum TrueEndPos
	{
		// Token: 0x040051DD RID: 20957
		SilhouetteCamPos,
		// Token: 0x040051DE RID: 20958
		DragonCamPos,
		// Token: 0x040051DF RID: 20959
		TraitorSpawnPosStart,
		// Token: 0x040051E0 RID: 20960
		TraitorSpawnPosEnd
	}
}
