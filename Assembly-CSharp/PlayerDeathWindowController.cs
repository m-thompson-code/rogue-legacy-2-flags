using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using GameEventTracking;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200058F RID: 1423
public class PlayerDeathWindowController : WindowController, ILocalizable
{
	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x06003534 RID: 13620 RVA: 0x000B760B File Offset: 0x000B580B
	private bool RunVictoryDeathScreen
	{
		get
		{
			return SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround;
		}
	}

	// Token: 0x170012DD RID: 4829
	// (get) Token: 0x06003535 RID: 13621 RVA: 0x000B761B File Offset: 0x000B581B
	public override WindowID ID
	{
		get
		{
			return WindowID.PlayerDeath;
		}
	}

	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06003536 RID: 13622 RVA: 0x000B761F File Offset: 0x000B581F
	public Scene Scene
	{
		get
		{
			return base.gameObject.scene;
		}
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x000B762C File Offset: 0x000B582C
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onRestartInLineagePressed = new Action<InputActionEventData>(this.OnRestartInLineagePressed);
		this.m_moveMapHorizontal = new Action<InputActionEventData>(this.MoveMapHorizontal);
		this.m_moveMapVertical = new Action<InputActionEventData>(this.MoveMapVertical);
		this.m_zoomMapVertical = new Action<InputActionEventData>(this.ZoomMapVertical);
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x000B7694 File Offset: 0x000B5894
	public override void Initialize()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_reenactmentController.OnRoomTrackerTriggeredRelay.AddListener(new Action<RoomTrackerData>(this.OnRoomTrackerEventTriggered), false);
		this.m_reenactmentController.OnEnemyTrackerTriggeredRelay.AddListener(new Action<EnemyTrackerData>(this.OnEnemyTrackerEventTriggered), false);
		this.m_reenactmentController.OnChestTrackerTriggeredRelay.AddListener(new Action<ChestTrackerData>(this.OnChestTrackerEventTriggered), false);
		this.m_reenactmentController.OnItemTrackerTriggeredRelay.AddListener(new Action<ItemTrackerData>(this.OnItemTrackerEventTriggered), false);
		this.m_portraitRenderGO.SetActive(false);
		this.m_portrait.gameObject.SetLayerRecursively(5, true);
		base.Initialize();
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x000B7754 File Offset: 0x000B5954
	private void OnDestroy()
	{
		this.m_reenactmentController.OnRoomTrackerTriggeredRelay.RemoveListener(new Action<RoomTrackerData>(this.OnRoomTrackerEventTriggered));
		this.m_reenactmentController.OnEnemyTrackerTriggeredRelay.RemoveListener(new Action<EnemyTrackerData>(this.OnEnemyTrackerEventTriggered));
		this.m_reenactmentController.OnChestTrackerTriggeredRelay.RemoveListener(new Action<ChestTrackerData>(this.OnChestTrackerEventTriggered));
		this.m_reenactmentController.OnItemTrackerTriggeredRelay.RemoveListener(new Action<ItemTrackerData>(this.OnItemTrackerEventTriggered));
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x000B77D5 File Offset: 0x000B59D5
	public void StopDeathSnapshot()
	{
		this.m_deathSnapshot.Stop();
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x000B77E4 File Offset: 0x000B59E4
	protected override void OnOpen()
	{
		if (PlayerManager.IsInstantiated && !ChallengeManager.IsInChallenge)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (!currentPlayerRoom.IsNativeNull() && currentPlayerRoom.SaveController.CanSaveRoom)
			{
				RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(currentPlayerRoom.BiomeType, currentPlayerRoom.BiomeControllerIndex);
				currentPlayerRoom.SaveController.SaveRoomState(roomSaveData, true);
			}
		}
		if (!this.RunVictoryDeathScreen)
		{
			AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_death_transition_in", CameraController.GameCamera.transform.position);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, new PlayerHUDVisibilityEventArgs(0.5f));
		}
		else
		{
			if (PlayerManager.IsInstantiated && !PlayerManager.GetPlayerController().IsFacingRight)
			{
				PlayerManager.GetPlayerController().CharacterCorgi.Flip(false, false);
			}
			AudioManager.PlayOneShot(null, "event:/Stingers/sting_chestItem", CameraController.GameCamera.transform.position);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, new PlayerHUDVisibilityEventArgs(0f));
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StopGlobalTimer, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HideGlobalTimer, null, null);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_masteryUnlocked = SkillTreeLogicHelper.IsTotemUnlocked();
		CameraController.CinemachineBrain.enabled = false;
		foreach (BaseTrait baseTrait in TraitManager.ActiveTraitList)
		{
			baseTrait.DisableOnDeath();
		}
		bool runVictoryDeathScreen = this.RunVictoryDeathScreen;
		SaveManager.PlayerSaveData.IsDead = true;
		PlayerDeathWindowController.GlobalCharacterDeathResetLogic();
		SaveManager.PlayerSaveData.TimesDied++;
		if (this.RunVictoryDeathScreen)
		{
			SaveManager.PlayerSaveData.CurrentCharacter.IsVictory = true;
			if (PlayerManager.IsInstantiated)
			{
				RangeDamageBonusCurseIndicator componentInChildren = PlayerManager.GetPlayerController().Visuals.GetComponentInChildren<RangeDamageBonusCurseIndicator>();
				if (componentInChildren)
				{
					componentInChildren.DisableRangeEffect();
				}
			}
		}
		if (SaveManager.PlayerSaveData.GlobalNPCDialogueCD > 0)
		{
			SaveManager.PlayerSaveData.GlobalNPCDialogueCD--;
		}
		else
		{
			NPCDialogueManager.ReduceAllNPCDialogueCooldowns(-1);
		}
		SaveManager.PlayerSaveData.PopulatedNPCDialoguesList.Clear();
		if (SaveManager.PlayerSaveData.CastleLockState == CastleLockState.TemporaryLock)
		{
			SaveManager.PlayerSaveData.CastleLockState = CastleLockState.NotLocked;
		}
		if (!this.RunVictoryDeathScreen)
		{
			this.m_playTreeCutscene = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Play_Tree_DeathCutscene);
			this.m_playHestiaCutscene = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Play_Hestia_DeathCutscene);
			if (this.m_playTreeCutscene)
			{
				this.m_playHestiaCutscene = false;
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Hestia_DeathCutscene, false);
			}
			if (this.m_playTreeCutscene || this.m_playHestiaCutscene)
			{
				SaveManager.PlayerSaveData.TimesDiedSinceHestia = 0;
			}
			else
			{
				SaveManager.PlayerSaveData.TimesDiedSinceHestia++;
			}
		}
		else
		{
			this.m_playTreeCutscene = false;
			this.m_playHestiaCutscene = false;
		}
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		SaveManager.LineageSaveData.AddCharacterToLineage(currentCharacter);
		if (SaveManager.LineageSaveData.DuplicateNameCountDict.ContainsKey(currentCharacter.Name))
		{
			int num = SaveManager.LineageSaveData.DuplicateNameCountDict[currentCharacter.Name];
			SaveManager.LineageSaveData.DuplicateNameCountDict[currentCharacter.Name] = num + 1;
		}
		else
		{
			SaveManager.LineageSaveData.DuplicateNameCountDict.Add(currentCharacter.Name, 0);
		}
		SaveManager.SaveAllCurrentProfileGameData(SavingType.FileOnly, true, true);
		this.AddInputListeners();
		this.m_slainByController.UpdateMessage(this.RunVictoryDeathScreen);
		this.m_partingWordsController.UpdateMessage(this.RunVictoryDeathScreen);
		this.m_playerPositionObj.gameObject.SetActive(false);
		this.m_reenactmentCanvasGroup.alpha = 0f;
		this.m_line.SetAlpha(0f);
		this.m_slainByController.Text.alpha = 0f;
		this.m_partingWordsController.Text.alpha = 0f;
		this.m_rankController.Text.alpha = 0f;
		this.m_rankController.CounterText.alpha = 0f;
		this.m_portraitCanvasGroup.gameObject.SetActive(true);
		this.m_portraitCanvasGroup.alpha = 0f;
		this.m_enemiesKilledCounter = 0;
		this.m_enemiesDefeatedAmountText.text = this.m_enemiesKilledCounter.ToString();
		this.m_goldCollectedCounter = 0;
		this.m_goldCollectedAmountText.text = this.m_goldCollectedCounter.ToString();
		this.m_equipmentOreCollectedCounter = 0;
		this.m_equipmentOreCollectedAmountText.text = this.m_equipmentOreCollectedCounter.ToString();
		this.m_runeOreCollectedCounter = 0;
		this.m_runeOreCollectedAmountText.text = this.m_runeOreCollectedCounter.ToString();
		this.m_soulCollectedCounter = 0;
		this.m_soulCollectedAmountText.text = this.m_soulCollectedCounter.ToString();
		MapController.SetCameraFollowIsOn(false);
		this.m_deathPanelGO.SetActive(true);
		CameraController.SoloCam.Camera.orthographicSize = CameraController.GameCamera.orthographicSize;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerDeathWindow_Opened, this, null);
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_rankController.UpdateMessage();
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x000B7CBC File Offset: 0x000B5EBC
	public static void GlobalCharacterDeathResetLogic()
	{
		SaveManager.PlayerSaveData.InCastle = false;
		SaveManager.PlayerSaveData.ResetAllRelics(true);
		SaveManager.PlayerSaveData.TemporaryMaxHealthMods = 0f;
		SaveManager.PlayerSaveData.TimesRolledRelic = 0;
		SaveManager.PlayerSaveData.GoldilocksReceived = false;
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveTuningForkTriggered, false);
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x000B7D14 File Offset: 0x000B5F14
	private IEnumerator CreateTestTracker()
	{
		List<RoomTrackerData> roomTrackerList = GameEventTrackerManager.RoomEventTracker.RoomsEntered;
		List<EnemyTrackerData> enemyTrackerList = GameEventTrackerManager.EnemyEventTracker.EnemiesKilled;
		List<ItemTrackerData> itemTrackerList = GameEventTrackerManager.ItemEventTracker.ItemsCollected;
		List<ChestTrackerData> chestTrackerList = GameEventTrackerManager.ItemEventTracker.ChestsOpened;
		foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
		{
			BiomeController biomeController = keyValuePair.Value;
			if (biomeController.Rooms != null)
			{
				int num;
				for (int i = 0; i < biomeController.Rooms.Count; i = num + 1)
				{
					BaseRoom room = biomeController.Rooms[i];
					if (!ReenactmentController.IsTunnelRoom(room) && room.BiomeType != BiomeType.Garden)
					{
						roomTrackerList.Add(new RoomTrackerData(room.BiomeType, room.BiomeControllerIndex, default(Vector2), false));
						yield return null;
						foreach (ChestSpawnController chestSpawnController in room.SpawnControllerManager.ChestSpawnControllers)
						{
							if (chestSpawnController.ShouldSpawn && CDGHelper.RandomPlusMinus() > 0)
							{
								chestTrackerList.Add(new ChestTrackerData(chestSpawnController.ChestType, true, chestSpawnController.Room.BiomeType, chestSpawnController.Room.BiomeControllerIndex, chestSpawnController.ChestIndex));
								yield return null;
							}
						}
						ChestSpawnController[] array = null;
						foreach (EnemySpawnController enemySpawnController in room.SpawnControllerManager.EnemySpawnControllers)
						{
							if (CDGHelper.RandomPlusMinus() > 0)
							{
								itemTrackerList.Add(new ItemTrackerData(10f, ItemDropType.Coin));
								itemTrackerList.Add(new ItemTrackerData(10f, ItemDropType.EquipmentOre));
								itemTrackerList.Add(new ItemTrackerData(10f, ItemDropType.RuneOre));
								itemTrackerList.Add(new ItemTrackerData(100f, ItemDropType.Soul));
							}
							enemyTrackerList.Add(new EnemyTrackerData(room.BiomeType, room.BiomeControllerIndex, enemySpawnController.Type, enemySpawnController.Rank, enemySpawnController.EnemyIndex));
							enemyTrackerList.Add(new EnemyTrackerData(room.BiomeType, room.BiomeControllerIndex, enemySpawnController.Type, enemySpawnController.Rank, enemySpawnController.EnemyIndex));
							enemyTrackerList.Add(new EnemyTrackerData(room.BiomeType, room.BiomeControllerIndex, enemySpawnController.Type, enemySpawnController.Rank, enemySpawnController.EnemyIndex));
							yield return null;
						}
						EnemySpawnController[] array2 = null;
						room = null;
					}
					num = i;
				}
				biomeController = null;
			}
		}
		Dictionary<BiomeType, BiomeController>.Enumerator enumerator = default(Dictionary<BiomeType, BiomeController>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x000B7D1C File Offset: 0x000B5F1C
	private IEnumerator OnOpenCoroutine()
	{
		if (WorldBuilder.IsInstantiated)
		{
		}
		yield return new WaitForEndOfFrame();
		this.m_portraitRenderGO.SetActive(true);
		this.m_portrait.SetPortraitLook(SaveManager.PlayerSaveData.CurrentCharacter);
		bool fog = RenderSettings.fog;
		RenderSettings.fog = false;
		this.m_portraitRenderCamera.Render();
		this.m_portraitRenderGO.SetActive(false);
		RenderSettings.fog = fog;
		if (!this.RunVictoryDeathScreen)
		{
			yield return this.DeathAnimationCoroutine();
		}
		else
		{
			yield return this.VictoryAnimationCoroutine();
		}
		while (!this.m_reenactmentController.IsComplete)
		{
			yield return null;
		}
		this.m_goldCollectedCounter = SaveManager.PlayerSaveData.GoldCollected;
		this.m_goldCollectedAmountText.text = this.m_goldCollectedCounter.ToString();
		yield break;
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x000B7D2C File Offset: 0x000B5F2C
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		CameraController.CinemachineBrain.enabled = true;
		this.m_reenactmentController.Reset();
		CameraController.SoloCam.gameObject.SetActive(false);
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		this.RemoveInputListeners();
		this.m_windowCanvas.gameObject.SetActive(false);
		bool runVictoryDeathScreen = this.RunVictoryDeathScreen;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, this, new TraitChangedEventArgs(TraitType.None, TraitType.None));
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerDeathWindow_Closed, this, null);
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().Pivot.SetActive(false);
			PlayerManager.GetPlayerController().ControllerCorgi.enabled = false;
		}
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x000B7DDA File Offset: 0x000B5FDA
	private IEnumerator DeathAnimationCoroutine()
	{
		global::PlayerController player = PlayerManager.GetPlayerController();
		if (player)
		{
			player.Animator.SetBool("Stunned", true);
			player.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			Vector3 position = CameraController.GameCamera.transform.position;
			position.z = CameraController.SoloCam.transform.position.z;
			CameraController.SoloCam.transform.position = position;
			CameraController.SoloCam.gameObject.SetActive(true);
			CameraController.SoloCam.AddToCameraLayer(player.Visuals);
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			if (CameraController.GameCamera.orthographicSize != 9f)
			{
				TweenManager.TweenTo_UnscaledTime(CameraController.SoloCam.Camera, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
				{
					"orthographicSize",
					9f
				});
			}
			Vector3 vector = player.transform.position - this.m_playerPositionObj.position;
			vector += CameraController.GameCamera.transform.position;
			TweenManager.TweenTo_UnscaledTime(CameraController.SoloCam.transform, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"position.x",
				vector.x,
				"position.y",
				vector.y
			});
			if (!Mathf.Approximately(AspectRatioManager.CurrentGameAspectRatio, 1.7777778f) && AspectRatioManager.CurrentGameAspectRatio < 1.7777778f)
			{
				float d = AspectRatioManager.CurrentGameAspectRatio / 1.7777778f;
				Vector3 vector2 = player.transform.localScale * d;
				TweenManager.TweenTo_UnscaledTime(player.transform, 1f, new EaseDelegate(Ease.None), new object[]
				{
					"localScale.x",
					vector2.x,
					"localScale.y",
					vector2.y,
					"localScale.z",
					vector2.z
				});
			}
			this.m_bgCanvasGroup.alpha = 0f;
			this.m_bgCanvasGroup.gameObject.SetActive(true);
			Vector3 position2 = CameraController.GameCamera.transform.position;
			position2.z = this.m_bgCanvasGroup.transform.position.z;
			this.m_bgCanvasGroup.transform.position = position2;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, this.m_fadeToBlackTime, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			this.m_waitYield.CreateNew(0.25f, true);
			yield return this.m_waitYield;
			this.m_reenactmentController.Initialise(this.m_reenactmentGridLayout.constraintCount);
			player.Animator.SetBool("Stunned", false);
			if (SaveManager.PlayerSaveData.CurrentCharacter.IsRetired)
			{
				player.Animator.SetTrigger("Retire");
			}
			else
			{
				int num = Player_EV.PLAYER_DEATH_ANIM_OVERRIDE;
				if (num == -1 && (TraitManager.IsTraitActive(TraitType.Fart) || TraitManager.IsTraitActive(TraitType.SuperFart)))
				{
					num = 9;
				}
				if (SaveManager.PlayerSaveData.IsInHeirloom)
				{
					num = 8;
				}
				if (num > -1 && num < 12)
				{
					player.Animator.SetTrigger("Death" + num.ToString());
				}
				else
				{
					int num2 = UnityEngine.Random.Range(0, 12);
					while (num2 == 9 || num2 == 8)
					{
						num2 = UnityEngine.Random.Range(0, 12);
					}
					player.Animator.SetTrigger("Death" + num2.ToString());
				}
				if (num == 9)
				{
					TweenManager.RunFunction_UnscaledTime(2f, this, "RunFartEffect", Array.Empty<object>());
				}
			}
			if (player.CharacterClass.WeaponAbilityType != AbilityType.BoxingGloveWeapon && player.CharacterClass.WeaponAbilityType != AbilityType.ExplosiveHandsWeapon)
			{
				if (player.LookController.CurrentWeaponGeo)
				{
					player.LookController.CurrentWeaponGeo.gameObject.SetActive(false);
				}
				if (player.LookController.SecondaryWeaponGeo)
				{
					player.LookController.SecondaryWeaponGeo.gameObject.SetActive(false);
				}
			}
			if (this.m_deathRecapFadeInUnityEvent != null)
			{
				this.m_deathRecapFadeInUnityEvent.Invoke();
			}
			TweenManager.TweenTo_UnscaledTime(this.m_reenactmentCanvasGroup, 1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			yield return null;
			while (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
			{
				yield return null;
			}
			TweenManager.TweenTo_UnscaledTime(this.m_portraitCanvasGroup, 0.25f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
			{
				"alpha",
				1
			});
			if (this.m_portraitAppearedUnityEvent != null)
			{
				this.m_portraitAppearedUnityEvent.Invoke();
			}
			if (this.m_masteryUnlocked)
			{
				TweenManager.TweenTo_UnscaledTime(this.m_rankController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
				TweenManager.TweenTo_UnscaledTime(this.m_rankController.CounterText, 0.5f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
			}
			TweenManager.TweenTo_UnscaledTime(this.m_slainByController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			TweenManager.TweenTo_UnscaledTime(this.m_line, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"delay",
				0.25f,
				"color.a",
				1
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_partingWordsController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"delay",
				0.5f,
				"alpha",
				1
			}).TweenCoroutine;
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			this.m_reenactmentController.Run();
			if (this.m_masteryUnlocked)
			{
				this.m_rankController.StartTally();
			}
		}
		yield break;
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x000B7DE9 File Offset: 0x000B5FE9
	private IEnumerator VictoryAnimationCoroutine()
	{
		global::PlayerController player = PlayerManager.GetPlayerController();
		if (player)
		{
			player.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			Vector3 position = CameraController.GameCamera.transform.position;
			position.z = CameraController.SoloCam.transform.position.z;
			CameraController.SoloCam.transform.position = position;
			CameraController.SoloCam.gameObject.SetActive(true);
			CameraController.SoloCam.AddToCameraLayer(player.Visuals);
			if (CameraController.GameCamera.orthographicSize != 9f)
			{
				CameraController.SoloCam.Camera.orthographicSize = 9f;
			}
			Vector3 vector = player.transform.position - this.m_playerPositionObj.position;
			vector += CameraController.GameCamera.transform.position;
			Vector3 position2 = CameraController.SoloCam.transform.position;
			position2.x = vector.x;
			position2.y = vector.y;
			CameraController.SoloCam.transform.position = position2;
			if (!Mathf.Approximately(AspectRatioManager.CurrentGameAspectRatio, 1.7777778f) && AspectRatioManager.CurrentGameAspectRatio < 1.7777778f)
			{
				float d = AspectRatioManager.CurrentGameAspectRatio / 1.7777778f;
				Vector3 localScale = player.transform.localScale * d;
				player.transform.localScale = localScale;
			}
			this.m_bgCanvasGroup.alpha = 1f;
			this.m_bgCanvasGroup.gameObject.SetActive(true);
			Vector3 position3 = CameraController.GameCamera.transform.position;
			position3.z = this.m_bgCanvasGroup.transform.position.z;
			this.m_bgCanvasGroup.transform.position = position3;
			this.m_reenactmentController.Initialise(this.m_reenactmentGridLayout.constraintCount);
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			player.Animator.SetBool("Victory", true);
			if (this.m_deathRecapFadeInUnityEvent != null)
			{
				this.m_deathRecapFadeInUnityEvent.Invoke();
			}
			TweenManager.TweenTo_UnscaledTime(this.m_reenactmentCanvasGroup, 1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			yield return null;
			while (player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
			{
				yield return null;
			}
			if (this.m_masteryUnlocked)
			{
				TweenManager.TweenTo_UnscaledTime(this.m_rankController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
				TweenManager.TweenTo_UnscaledTime(this.m_rankController.CounterText, 0.5f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
			}
			TweenManager.TweenTo_UnscaledTime(this.m_slainByController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			TweenManager.TweenTo_UnscaledTime(this.m_line, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"delay",
				0.25f,
				"color.a",
				1
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_partingWordsController.Text, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"delay",
				0.5f,
				"alpha",
				1
			}).TweenCoroutine;
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			this.m_reenactmentController.Run();
			if (this.m_masteryUnlocked)
			{
				this.m_rankController.StartTally();
			}
		}
		yield break;
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x000B7DF8 File Offset: 0x000B5FF8
	private void RunFartEffect()
	{
		global::PlayerController playerController = PlayerManager.GetPlayerController();
		if (TraitManager.IsTraitActive(TraitType.Fart))
		{
			Vector3 vector = new Vector3(-0.25f, 1f, 0f);
			BaseEffect baseEffect = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "Death_Fart_Effect", playerController.transform.position + vector, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			if (!playerController.IsFacingRight)
			{
				Vector3 position = baseEffect.transform.position;
				position.x -= vector.x * 2f;
				baseEffect.transform.position = position;
				baseEffect.Flip();
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Spells/sfx_spell_fart_forDeath", baseEffect.transform.position);
			return;
		}
		Vector3 vector2 = new Vector3(-2f, 1f, 0f);
		Vector2 vector3 = new Vector2(1f, 1f);
		BaseEffect baseEffect2 = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "Death_SuperFart_Effect", playerController.transform.position + vector2, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		BaseEffect baseEffect3 = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "Death_SuperFartExplosion_Effect", playerController.transform.position + vector2, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect3.transform.localScale = new Vector3(3f, 3f, 1f);
		if (!playerController.IsFacingRight)
		{
			Vector3 position2 = baseEffect2.transform.position;
			position2.x -= vector2.x * 2f;
			baseEffect2.transform.position = position2;
			position2 = baseEffect3.transform.position;
			position2.x -= vector2.x * 2f;
			baseEffect3.transform.position = position2;
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Spells/sfx_spell_superfart_forDeath", baseEffect2.transform.position);
		TweenManager.TweenBy_UnscaledTime(playerController.Visuals.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.x",
			vector3.x * 7f
		});
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x000B8028 File Offset: 0x000B6228
	private void AddInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onRestartInLineagePressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.AddInputEventDelegate(this.m_moveMapHorizontal, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_moveMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical");
			base.RewiredPlayer.AddInputEventDelegate(this.m_zoomMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical_RStick");
		}
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x000B80A0 File Offset: 0x000B62A0
	private void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onRestartInLineagePressed);
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_moveMapHorizontal);
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_moveMapVertical);
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_zoomMapVertical);
		}
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x000B80F8 File Offset: 0x000B62F8
	private void MoveMapVertical(InputActionEventData inputActionEventData)
	{
		if (!this.m_reenactmentController.IsComplete)
		{
			return;
		}
		float num = inputActionEventData.GetAxis();
		if (num == 0f)
		{
			num = -inputActionEventData.GetAxisPrev();
		}
		Vector3 position = MapController.Camera.transform.position;
		float num2 = MapController.DeathMapCamera.orthographicSize / 6f;
		float num3 = num * num2 * 14f * Time.unscaledDeltaTime;
		position.y += num3;
		MapController.SetMapCameraPosition(position);
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x000B8170 File Offset: 0x000B6370
	private void MoveMapHorizontal(InputActionEventData inputActionEventData)
	{
		if (!this.m_reenactmentController.IsComplete)
		{
			return;
		}
		float num = inputActionEventData.GetAxis();
		if (num == 0f)
		{
			num = -inputActionEventData.GetAxisPrev();
		}
		Vector3 position = MapController.Camera.transform.position;
		float num2 = MapController.DeathMapCamera.orthographicSize / 6f;
		float num3 = num * num2 * 14f * Time.unscaledDeltaTime;
		position.x += num3;
		MapController.SetMapCameraPosition(position);
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x000B81E8 File Offset: 0x000B63E8
	private void ZoomMapVertical(InputActionEventData eventData)
	{
		if (!this.m_reenactmentController.IsComplete)
		{
			return;
		}
		float num = -eventData.GetAxis();
		float orthographicSize = Mathf.Clamp(MapController.DeathMapCamera.orthographicSize + num * 14f * Time.unscaledDeltaTime, 3f, 12f);
		MapController.DeathMapCamera.orthographicSize = orthographicSize;
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x000B8240 File Offset: 0x000B6440
	private void OnRestartInLineagePressed(InputActionEventData obj)
	{
		if (!this.m_reenactmentController.IsComplete || (!this.m_rankController.IsComplete && this.m_masteryUnlocked))
		{
			this.m_reenactmentController.FastForward();
			if (this.m_masteryUnlocked)
			{
				this.m_rankController.SkipTally();
			}
			return;
		}
		if (this.m_deathRecapFadeOutUnityEvent != null)
		{
			this.m_deathRecapFadeOutUnityEvent.Invoke();
		}
		if (this.RunVictoryDeathScreen)
		{
			SceneLoader_RL.LoadScene(SceneID.Parade, TransitionID.FadeToBlackWithLoading);
			return;
		}
		this.m_deathPanelGO.SetActive(false);
		string text = this.m_slainByController.Text.text;
		string text2 = this.m_partingWordsController.Text.text;
		string text3 = this.m_rankController.Text.text;
		string text4 = this.m_rankController.CounterText.text;
		if (this.m_playHestiaCutscene)
		{
			SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.HestiaDeath);
			return;
		}
		if (this.m_playTreeCutscene)
		{
			SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.TreeDeath);
			return;
		}
		(TransitionLibrary.GetTransitionInstance(TransitionID.PlayerDeathToLineage) as PlayerDeathToLineage_SceneTransition).SetData(this.m_portraitRT.texture as RenderTexture, this.m_playerPositionObj.anchoredPosition, text, text2, text3, text4);
		SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.PlayerDeathToLineage);
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x000B836A File Offset: 0x000B656A
	public void DeactivatePortrait()
	{
		this.m_portrait.gameObject.SetActive(false);
		this.m_portraitRenderCamera.gameObject.SetActive(false);
		this.m_portraitCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x000B839F File Offset: 0x000B659F
	private void OnEnemyTrackerEventTriggered(EnemyTrackerData enemyData)
	{
		this.m_enemiesKilledCounter++;
		this.m_enemiesDefeatedAmountText.text = this.m_enemiesKilledCounter.ToString();
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x000B83C8 File Offset: 0x000B65C8
	private void OnItemTrackerEventTriggered(ItemTrackerData itemData)
	{
		ItemDropType baseItemDropType = Economy_EV.GetBaseItemDropType(itemData.ItemDropType);
		if (baseItemDropType <= ItemDropType.EquipmentOre)
		{
			if (baseItemDropType == ItemDropType.Coin)
			{
				this.m_goldCollectedCounter += (int)itemData.Value;
				this.m_goldCollectedAmountText.text = this.m_goldCollectedCounter.ToString();
				return;
			}
			if (baseItemDropType != ItemDropType.EquipmentOre)
			{
				return;
			}
			this.m_equipmentOreCollectedCounter += (int)itemData.Value;
			this.m_equipmentOreCollectedAmountText.text = this.m_equipmentOreCollectedCounter.ToString();
			return;
		}
		else
		{
			if (baseItemDropType == ItemDropType.RuneOre)
			{
				this.m_runeOreCollectedCounter += (int)itemData.Value;
				this.m_runeOreCollectedAmountText.text = this.m_runeOreCollectedCounter.ToString();
				return;
			}
			if (baseItemDropType != ItemDropType.Soul)
			{
				return;
			}
			this.m_soulCollectedCounter += (int)itemData.Value;
			this.m_soulCollectedAmountText.text = this.m_soulCollectedCounter.ToString();
			return;
		}
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x000B84AF File Offset: 0x000B66AF
	private void OnChestTrackerEventTriggered(ChestTrackerData chestData)
	{
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x000B84B4 File Offset: 0x000B66B4
	private void OnRoomTrackerEventTriggered(RoomTrackerData roomData)
	{
		MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(roomData.Biome, roomData.BiomeControllerIndex);
		if (mapRoomEntry == null)
		{
			return;
		}
		bool flag = false;
		if (this.m_prevBiomeType != roomData.Biome && !roomData.ViaBiomeTransitionDoor)
		{
			flag = true;
		}
		this.m_prevBiomeType = roomData.Biome;
		if (roomData.WorldPosition != default(Vector2))
		{
			MapController.SetPlayerIconInRoom(roomData.Biome, roomData.BiomeControllerIndex, roomData.WorldPosition);
		}
		Vector3 playerIconPosition = MapController.PlayerIconPosition;
		if (MapController.DeathMapCamera.orthographicSize < 6f)
		{
			if (flag)
			{
				if (this.m_cameraZoomTween != null && this.m_cameraZoomTween.isActiveAndEnabled && this.m_cameraZoomTween.TweenedObj == MapController.DeathMapCamera && this.m_cameraZoomTween.ID == "CamZoom")
				{
					this.m_cameraZoomTween.StopTween(false);
				}
				MapController.DeathMapCamera.orthographicSize = 6f;
			}
			Vector3 vector = new Vector3(mapRoomEntry.AbsBounds.xMin, mapRoomEntry.AbsBounds.yMin, 1f);
			Vector3 vector2 = new Vector3(mapRoomEntry.AbsBounds.xMin, mapRoomEntry.AbsBounds.yMax, 1f);
			Vector3 vector3 = new Vector3(mapRoomEntry.AbsBounds.xMax, mapRoomEntry.AbsBounds.yMin, 1f);
			Vector3 vector4 = new Vector3(mapRoomEntry.AbsBounds.xMax, mapRoomEntry.AbsBounds.yMax, 1f);
			Vector3[] array = new Vector3[]
			{
				vector,
				vector2,
				vector3,
				vector4
			};
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector5 = array[i];
				if (vector5.x < this.m_camBoundsRect.x)
				{
					this.m_camBoundsRect.x = vector5.x;
				}
				if (vector5.x > this.m_camBoundsRect.w)
				{
					this.m_camBoundsRect.w = vector5.x;
				}
				if (vector5.y < this.m_camBoundsRect.y)
				{
					this.m_camBoundsRect.y = vector5.y;
				}
				if (vector5.y > this.m_camBoundsRect.z)
				{
					this.m_camBoundsRect.z = vector5.y;
				}
			}
			array[0] = new Vector3(this.m_camBoundsRect.x, this.m_camBoundsRect.y, MapController.DeathMapCamera.transform.position.z);
			array[1] = new Vector3(this.m_camBoundsRect.x, this.m_camBoundsRect.z, MapController.DeathMapCamera.transform.position.z);
			array[2] = new Vector3(this.m_camBoundsRect.w, this.m_camBoundsRect.y, MapController.DeathMapCamera.transform.position.z);
			array[3] = new Vector3(this.m_camBoundsRect.w, this.m_camBoundsRect.z, MapController.DeathMapCamera.transform.position.z);
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MinValue;
			float num4 = float.MaxValue;
			float num5 = 0.25f;
			num5 = Mathf.Clamp(num5, 0f, 0.5f);
			for (int j = 0; j < 4; j++)
			{
				Vector3 vector6 = MapController.DeathMapCamera.WorldToViewportPoint(array[j]);
				if (vector6.x < num)
				{
					num = vector6.x;
				}
				if (vector6.x > num2)
				{
					num2 = vector6.x;
				}
				if (vector6.y < num4)
				{
					num4 = vector6.y;
				}
				if (vector6.y > num3)
				{
					num3 = vector6.y;
				}
			}
			float num6 = 0f;
			if (num < num5)
			{
				float num7 = (num5 - num) / (1f - num5 * 2f);
				if (num7 > num6)
				{
					num6 = num7;
				}
			}
			if (num2 > 1f - num5)
			{
				float num8 = (num2 - (1f - num5)) / (1f - num5 * 2f);
				if (num8 > num6)
				{
					num6 = num8;
				}
			}
			if (num4 < num5)
			{
				float num9 = (num5 - num4) / (1f - num5 * 2f);
				if (num9 > num6)
				{
					num6 = num9;
				}
			}
			if (num3 > 1f - num5)
			{
				float num10 = (num3 - (1f - num5)) / (1f - num5 * 2f);
				if (num10 > num6)
				{
					num6 = num10;
				}
			}
			if (num6 > 0f && this.m_desiredZoomSize != num6)
			{
				if (this.m_cameraZoomTween != null && this.m_cameraZoomTween.isActiveAndEnabled && this.m_cameraZoomTween.TweenedObj == MapController.DeathMapCamera && this.m_cameraZoomTween.ID == "CamZoom")
				{
					this.m_cameraZoomTween.StopTween(false);
				}
				float num11 = Mathf.Min(6f, MapController.DeathMapCamera.orthographicSize * (1f + num6));
				if (!this.m_reenactmentController.IsFastforwarding)
				{
					this.m_cameraZoomTween = TweenManager.TweenTo_UnscaledTime(MapController.DeathMapCamera, 0.5f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
					{
						"orthographicSize",
						num11
					});
					this.m_cameraZoomTween.ID = "CamZoom";
				}
				else
				{
					MapController.DeathMapCamera.orthographicSize = num11;
				}
				this.m_desiredZoomSize = num6;
			}
		}
		if (this.m_desiredCamPos != playerIconPosition)
		{
			if (this.m_cameraMoveTween != null && this.m_cameraMoveTween.isActiveAndEnabled && this.m_cameraMoveTween.TweenedObj == MapController.Camera.transform && this.m_cameraMoveTween.ID == "CamMove")
			{
				this.m_cameraMoveTween.StopTween(false);
			}
			if (!this.m_reenactmentController.IsFastforwarding && !flag)
			{
				this.m_cameraMoveTween = TweenManager.TweenTo_UnscaledTime(MapController.Camera.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
				{
					"position.x",
					playerIconPosition.x,
					"position.y",
					playerIconPosition.y
				});
				this.m_cameraMoveTween.ID = "CamMove";
			}
			else
			{
				MapController.Camera.transform.position = new Vector3(playerIconPosition.x, playerIconPosition.y, 0f);
			}
			this.m_desiredCamPos = playerIconPosition;
		}
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x000B8B80 File Offset: 0x000B6D80
	protected override void OnFocus()
	{
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x000B8B82 File Offset: 0x000B6D82
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x000B8B84 File Offset: 0x000B6D84
	protected override void OnPause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x000B8B8B File Offset: 0x000B6D8B
	protected override void OnUnpause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x000B8B92 File Offset: 0x000B6D92
	public void RefreshText(object sender, EventArgs args)
	{
		GameEventTrackerManager.EnemyEventTracker.ForceRefreshText();
		this.m_partingWordsController.UpdateMessage(this.RunVictoryDeathScreen);
		this.m_slainByController.UpdateMessage(this.RunVictoryDeathScreen);
		this.m_rankController.UpdateMessage();
	}

	// Token: 0x04002965 RID: 10597
	private const bool RUN_EVENT_TRACKER_TEST = false;

	// Token: 0x04002966 RID: 10598
	[Space(10f)]
	[SerializeField]
	private ReenactmentController m_reenactmentController;

	// Token: 0x04002967 RID: 10599
	[SerializeField]
	private PlayerDeathSlainByTextController m_slainByController;

	// Token: 0x04002968 RID: 10600
	[SerializeField]
	private PlayerDeathPartingWordsTextController m_partingWordsController;

	// Token: 0x04002969 RID: 10601
	[SerializeField]
	private PlayerDeathRankTextController m_rankController;

	// Token: 0x0400296A RID: 10602
	[SerializeField]
	private GameObject m_deathPanelGO;

	// Token: 0x0400296B RID: 10603
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x0400296C RID: 10604
	[SerializeField]
	private float m_fadeToBlackTime = 1f;

	// Token: 0x0400296D RID: 10605
	[SerializeField]
	private TMP_Text m_enemiesDefeatedAmountText;

	// Token: 0x0400296E RID: 10606
	[SerializeField]
	private TMP_Text m_goldCollectedAmountText;

	// Token: 0x0400296F RID: 10607
	[SerializeField]
	private TMP_Text m_equipmentOreCollectedAmountText;

	// Token: 0x04002970 RID: 10608
	[SerializeField]
	private TMP_Text m_runeOreCollectedAmountText;

	// Token: 0x04002971 RID: 10609
	[SerializeField]
	private TMP_Text m_soulCollectedAmountText;

	// Token: 0x04002972 RID: 10610
	[SerializeField]
	private CanvasGroup m_reenactmentCanvasGroup;

	// Token: 0x04002973 RID: 10611
	[SerializeField]
	private GridLayoutGroup m_reenactmentGridLayout;

	// Token: 0x04002974 RID: 10612
	[SerializeField]
	private Image m_line;

	// Token: 0x04002975 RID: 10613
	[SerializeField]
	private RawImage m_portraitRT;

	// Token: 0x04002976 RID: 10614
	[SerializeField]
	private CanvasGroup m_portraitCanvasGroup;

	// Token: 0x04002977 RID: 10615
	[SerializeField]
	private LineagePortrait m_portrait;

	// Token: 0x04002978 RID: 10616
	[SerializeField]
	private Camera m_portraitRenderCamera;

	// Token: 0x04002979 RID: 10617
	[SerializeField]
	private GameObject m_portraitRenderGO;

	// Token: 0x0400297A RID: 10618
	[SerializeField]
	private RectTransform m_playerPositionObj;

	// Token: 0x0400297B RID: 10619
	[Space(10f)]
	[SerializeField]
	private UnityEvent m_portraitAppearedUnityEvent;

	// Token: 0x0400297C RID: 10620
	[SerializeField]
	private UnityEvent m_deathRecapFadeInUnityEvent;

	// Token: 0x0400297D RID: 10621
	[SerializeField]
	private UnityEvent m_deathRecapFadeOutUnityEvent;

	// Token: 0x0400297E RID: 10622
	[SerializeField]
	private StudioEventEmitter m_deathSnapshot;

	// Token: 0x0400297F RID: 10623
	private int m_enemiesKilledCounter;

	// Token: 0x04002980 RID: 10624
	private int m_goldCollectedCounter;

	// Token: 0x04002981 RID: 10625
	private int m_equipmentOreCollectedCounter;

	// Token: 0x04002982 RID: 10626
	private int m_runeOreCollectedCounter;

	// Token: 0x04002983 RID: 10627
	private int m_soulCollectedCounter;

	// Token: 0x04002984 RID: 10628
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002985 RID: 10629
	private bool m_masteryUnlocked;

	// Token: 0x04002986 RID: 10630
	private bool m_playTreeCutscene;

	// Token: 0x04002987 RID: 10631
	private bool m_playHestiaCutscene;

	// Token: 0x04002988 RID: 10632
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002989 RID: 10633
	private Action<InputActionEventData> m_onRestartInLineagePressed;

	// Token: 0x0400298A RID: 10634
	private Action<InputActionEventData> m_moveMapHorizontal;

	// Token: 0x0400298B RID: 10635
	private Action<InputActionEventData> m_moveMapVertical;

	// Token: 0x0400298C RID: 10636
	private Action<InputActionEventData> m_zoomMapVertical;

	// Token: 0x0400298D RID: 10637
	private Tween m_cameraZoomTween;

	// Token: 0x0400298E RID: 10638
	private Tween m_cameraMoveTween;

	// Token: 0x0400298F RID: 10639
	private float m_desiredZoomSize;

	// Token: 0x04002990 RID: 10640
	private Vector3 m_desiredCamPos;

	// Token: 0x04002991 RID: 10641
	private Vector4 m_camBoundsRect = new Vector4(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);

	// Token: 0x04002992 RID: 10642
	private BiomeType m_prevBiomeType = BiomeType.Castle;
}
