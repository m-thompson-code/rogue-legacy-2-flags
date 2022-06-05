using System;
using System.Collections;
using Cinemachine;
using FMOD.Studio;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class BossRoomController : BaseSpecialRoomController, IAudioEventEmitter
{
	// Token: 0x170011A4 RID: 4516
	// (get) Token: 0x06002F42 RID: 12098 RVA: 0x000A17AD File Offset: 0x0009F9AD
	public int NumBossesDeadOrDying
	{
		get
		{
			return this.m_numBossesDeadOrDying;
		}
	}

	// Token: 0x170011A5 RID: 4517
	// (get) Token: 0x06002F43 RID: 12099 RVA: 0x000A17B5 File Offset: 0x0009F9B5
	public int NumBossesDead
	{
		get
		{
			return this.m_numBossesKilled;
		}
	}

	// Token: 0x170011A6 RID: 4518
	// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000A17BD File Offset: 0x0009F9BD
	public int NumBosses
	{
		get
		{
			return this.m_numBosses;
		}
	}

	// Token: 0x170011A7 RID: 4519
	// (get) Token: 0x06002F45 RID: 12101 RVA: 0x000A17C5 File Offset: 0x0009F9C5
	public bool AllBossesDeadOrDying
	{
		get
		{
			return this.NumBossesDeadOrDying >= this.NumBosses;
		}
	}

	// Token: 0x170011A8 RID: 4520
	// (get) Token: 0x06002F46 RID: 12102 RVA: 0x000A17D8 File Offset: 0x0009F9D8
	public bool AllBossesDead
	{
		get
		{
			return this.NumBossesDead >= this.NumBosses;
		}
	}

	// Token: 0x170011A9 RID: 4521
	// (get) Token: 0x06002F47 RID: 12103 RVA: 0x000A17EB File Offset: 0x0009F9EB
	public TunnelSpawnController TunnelSpawnController
	{
		get
		{
			return this.m_tunnelSpawnController;
		}
	}

	// Token: 0x170011AA RID: 4522
	// (get) Token: 0x06002F48 RID: 12104 RVA: 0x000A17F3 File Offset: 0x0009F9F3
	public virtual EnemySpawnController BossSpawnController
	{
		get
		{
			return this.m_bossSpawnController;
		}
	}

	// Token: 0x170011AB RID: 4523
	// (get) Token: 0x06002F49 RID: 12105 RVA: 0x000A17FB File Offset: 0x0009F9FB
	public EnemyController Boss
	{
		get
		{
			if (this.BossSpawnController)
			{
				return this.BossSpawnController.EnemyInstance;
			}
			return null;
		}
	}

	// Token: 0x170011AC RID: 4524
	// (get) Token: 0x06002F4A RID: 12106 RVA: 0x000A1817 File Offset: 0x0009FA17
	public ChestObj Chest
	{
		get
		{
			if (this.m_chestSpawnController)
			{
				return this.m_chestSpawnController.ChestInstance;
			}
			return null;
		}
	}

	// Token: 0x170011AD RID: 4525
	// (get) Token: 0x06002F4B RID: 12107 RVA: 0x000A1833 File Offset: 0x0009FA33
	public CinemachineVirtualCameraManager VCamManager
	{
		get
		{
			return this.m_vcamManager;
		}
	}

	// Token: 0x170011AE RID: 4526
	// (get) Token: 0x06002F4C RID: 12108 RVA: 0x000A183B File Offset: 0x0009FA3B
	// (set) Token: 0x06002F4D RID: 12109 RVA: 0x000A1843 File Offset: 0x0009FA43
	public bool IsInitialized { get; protected set; }

	// Token: 0x170011AF RID: 4527
	// (get) Token: 0x06002F4E RID: 12110 RVA: 0x000A184C File Offset: 0x0009FA4C
	public virtual float BossHealthAsPercentage
	{
		get
		{
			if (this.Boss)
			{
				return this.Boss.CurrentHealth / (float)this.Boss.ActualMaxHealth;
			}
			return 0f;
		}
	}

	// Token: 0x170011B0 RID: 4528
	// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000A1879 File Offset: 0x0009FA79
	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(this.m_description))
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x000A189C File Offset: 0x0009FA9C
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_bossDefeatedArgs = new BossObjectiveCompleteHUDEventArgs(EnemyType.None, EnemyRank.None, 5f, null, null, null);
		BossRoomController.m_followBlend.m_Time = 1f;
		BossRoomController.m_followBlend.m_Style = CinemachineBlendDefinition.Style.Custom;
		BossRoomController.m_followBlend.m_CustomCurve = this.m_cameraBlendCurve;
		this.m_onBossDeath = new Action<MonoBehaviour, EventArgs>(this.OnBossDeath);
		this.m_onBossHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnBossHealthChange);
		this.m_onModeShift = new Action<MonoBehaviour, EventArgs>(this.OnModeShift);
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x000A1938 File Offset: 0x0009FB38
	protected virtual void Initialize()
	{
		if (!this.BossSpawnController)
		{
			return;
		}
		if (!this.m_chestSpawnController)
		{
			return;
		}
		this.Chest.SetOpacity(0f);
		this.Chest.SetChestLockState(ChestLockState.Locked);
		this.Chest.gameObject.SetActive(false);
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x000A198E File Offset: 0x0009FB8E
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (!this.IsInitialized)
		{
			this.Initialize();
		}
		this.AddListeners();
		base.StopAllCoroutines();
		base.StartCoroutine(this.StartIntro());
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x000A19BF File Offset: 0x0009FBBF
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerExitRoom(sender, eventArgs);
		this.m_numBossesKilled = 0;
		this.m_numBossesDeadOrDying = 0;
		this.RemoveListeners();
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x000A19DD File Offset: 0x0009FBDD
	protected virtual void OnDisable()
	{
		if (this.m_chestAudioEventInstance.isValid())
		{
			AudioManager.Stop(this.m_chestAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_numBossesKilled = 0;
		this.m_numBossesDeadOrDying = 0;
		this.RemoveListeners();
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x000A1A0C File Offset: 0x0009FC0C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_chestAudioEventInstance.isValid())
		{
			this.m_chestAudioEventInstance.release();
		}
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x000A1A2D File Offset: 0x0009FC2D
	protected virtual void AddListeners()
	{
		if (!this.m_listenersAssigned)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onBossDeath);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHealthChange, this.m_onBossHealthChange);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyModeShift, this.m_onModeShift);
			this.m_listenersAssigned = true;
		}
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x000A1A64 File Offset: 0x0009FC64
	protected virtual void RemoveListeners()
	{
		if (this.m_listenersAssigned)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onBossDeath);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHealthChange, this.m_onBossHealthChange);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyModeShift, this.m_onModeShift);
			this.m_listenersAssigned = false;
		}
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x000A1A9C File Offset: 0x0009FC9C
	protected virtual void OnBossDeath(object sender, EventArgs args)
	{
		EnemyDeathEventArgs enemyDeathEventArgs = args as EnemyDeathEventArgs;
		if (enemyDeathEventArgs != null && enemyDeathEventArgs.Victim && enemyDeathEventArgs.Victim.IsBoss)
		{
			this.m_numBossesKilled++;
		}
		if (this.AllBossesDead)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.StartOutro());
		}
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x000A1AF8 File Offset: 0x0009FCF8
	protected virtual void OnBossHealthChange(object sender, EventArgs args)
	{
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs.PrevHealthValue > 0f && healthChangeEventArgs.NewHealthValue <= 0f)
		{
			this.m_numBossesDeadOrDying++;
			if (this.AllBossesDeadOrDying)
			{
				ProjectileManager.DisableAllProjectiles(true, new string[]
				{
					"EnemyProjectile"
				});
				PlayerManager.GetPlayerController().TakesNoDamage = true;
			}
		}
		this.BossTookDamageRelay.Dispatch(this.BossHealthAsPercentage);
	}

	// Token: 0x06002F5A RID: 12122 RVA: 0x000A1B6C File Offset: 0x0009FD6C
	protected virtual void OnModeShift(object sender, EventArgs args)
	{
		this.PhaseChangedRelay.Dispatch(1);
	}

	// Token: 0x06002F5B RID: 12123 RVA: 0x000A1B7A File Offset: 0x0009FD7A
	protected virtual IEnumerator StartIntro()
	{
		RewiredMapController.SetIsInCutscene(true);
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (global::AnimatorUtility.HasParameter(this.Boss.Animator, "Turn"))
		{
			this.Boss.Animator.ResetTrigger("Turn");
		}
		if ((this.Boss.transform.localPosition.x < playerController.transform.localPosition.x && !this.Boss.CharacterCorgi.IsFacingRight) || (this.Boss.transform.localPosition.x > playerController.transform.localPosition.x && this.Boss.CharacterCorgi.IsFacingRight))
		{
			this.Boss.CharacterCorgi.Flip(false, true);
		}
		if ((playerController.transform.localPosition.x < this.Boss.transform.localPosition.x && !playerController.IsFacingRight) || (playerController.transform.localPosition.x > this.Boss.transform.localPosition.x && playerController.IsFacingRight))
		{
			playerController.CharacterCorgi.Flip(false, true);
		}
		yield return null;
		this.IntroStartRelay.Dispatch();
		while (!this.Boss.IsInitialized)
		{
			yield return null;
		}
		while (!this.Boss.LogicController.IsInitialized)
		{
			yield return null;
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, null);
		this.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_zoomIn");
		CinemachineConfiner_RL confiner = this.Boss.Room.CinemachineCamera.GetComponent<CinemachineConfiner_RL>();
		confiner.enabled = false;
		this.Boss.Room.CinemachineCamera.SetFollowTarget(this.m_bossStartCamPos.transform);
		if (global::AnimatorUtility.HasState(this.Boss.Animator, "Intro_Idle"))
		{
			this.Boss.Animator.Play("Intro_Idle");
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.BossIntro))
		{
			WindowManager.LoadWindow(WindowID.BossIntro);
		}
		BossIntroWindowController bossWindow = WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController;
		bossWindow.SetEnemyType(this.Boss.EnemyType, this.Boss.EnemyRank);
		WindowManager.SetWindowIsOpen(WindowID.BossIntro, true);
		this.m_waitYield.CreateNew(1f, true);
		yield return this.m_waitYield;
		CinemachineBlendDefinition storedBlend = CameraController.CinemachineBrain.m_DefaultBlend;
		CameraController.CinemachineBrain.m_DefaultBlend = BossRoomController.m_followBlend;
		float zoomLevel = CameraController.ZoomLevel;
		if (zoomLevel != 1f)
		{
			this.VCamManager.SetLensSize(CameraController.GetVirtualCameraLensSize(zoomLevel));
		}
		this.Boss.Room.CinemachineCamera.SetIsActiveCamera(false);
		this.VCamManager.VirtualCamera.gameObject.SetActive(true);
		this.VCamManager.SetIsActiveCamera(true);
		this.VCamManager.SetFollowTarget(this.m_bossEndCamPos.transform);
		this.BossSpawnAnimRelay.Dispatch();
		yield return this.Boss.LogicController.LogicScript.SpawnAnim();
		bossWindow.DisplayBossName = true;
		while (WindowManager.GetIsWindowOpen(WindowID.BossIntro))
		{
			yield return null;
		}
		confiner.enabled = true;
		this.Boss.Room.CinemachineCamera.VirtualCamera.gameObject.SetActive(true);
		this.Boss.Room.CinemachineCamera.SetIsActiveCamera(true);
		this.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_zoomOut");
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, new PlayerHUDVisibilityEventArgs(0.5f));
		this.Boss.LogicController.DisableLogicActivationByDistance = false;
		this.VCamManager.VirtualCamera.gameObject.SetActive(false);
		CameraController.CinemachineBrain.m_DefaultBlend = storedBlend;
		this.IntroCompleteRelay.Dispatch();
		RewiredMapController.SetIsInCutscene(false);
		yield break;
	}

	// Token: 0x06002F5C RID: 12124 RVA: 0x000A1B89 File Offset: 0x0009FD89
	protected virtual IEnumerator StartOutro()
	{
		this.OutroStartRelay.Dispatch();
		this.m_waitYield.CreateNew(2f, false);
		yield return this.m_waitYield;
		float num = 4f;
		this.InitializeObjectiveCompleteArgs(num);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_bossDefeatedArgs);
		this.m_waitYield.CreateNew(num, false);
		yield return this.m_waitYield;
		float chestOpacity = 0f;
		this.Chest.gameObject.SetActive(true);
		this.Chest.transform.SetParent(base.transform);
		Vector3 position = this.Chest.transform.position;
		position.y += 1f;
		this.Chest.transform.position = position;
		this.Chest.Interactable.SetIsInteractableActive(false);
		TweenManager.TweenBy(this.Chest.transform, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.y",
			-1
		});
		this.m_chestAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Interactables/sfx_env_prop_bossChest_spawn_loop", this.Chest.transform);
		AudioManager.PlayAttached(this, this.m_chestAudioEventInstance, this.Chest.gameObject);
		while (chestOpacity < 1f)
		{
			this.Chest.SetOpacity(chestOpacity);
			chestOpacity += Time.deltaTime;
			yield return null;
		}
		AudioManager.Stop(this.m_chestAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Chest.Interactable.SetIsInteractableActive(true);
		this.Chest.SetChestLockState(ChestLockState.Unlocked);
		while (!this.Chest.IsOpen)
		{
			yield return null;
		}
		this.SetBossFlagDefeated();
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		float itemDropDuration = Time.time + 5f;
		while (ItemDropManager.HasActiveItemDrops && Time.time < itemDropDuration)
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		PlayerManager.GetPlayerController().TakesNoDamage = false;
		this.TeleportOut();
		this.OutroCompleteRelay.Dispatch();
		yield break;
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x000A1B98 File Offset: 0x0009FD98
	protected virtual void TeleportOut()
	{
		BiomeController biomeController = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(BiomeType.Castle);
		BaseRoom baseRoom = biomeController ? biomeController.TransitionRoom : null;
		if (baseRoom)
		{
			if (this.TunnelSpawnController && this.TunnelSpawnController.Tunnel)
			{
				RewiredMapController.SetIsInCutscene(true);
				CutsceneManager.InitializeCutscene(this.m_bossSaveFlag, this.TunnelSpawnController.Tunnel.Destination);
				PortraitCutsceneController component = baseRoom.gameObject.GetComponent<PortraitCutsceneController>();
				this.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, component.TunnelSpawner.Tunnel);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
		else
		{
			if (this.TunnelSpawnController && this.TunnelSpawnController.Tunnel)
			{
				this.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
	}

	// Token: 0x06002F5E RID: 12126 RVA: 0x000A1C85 File Offset: 0x0009FE85
	protected virtual void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		this.m_bossDefeatedArgs.Initialize(this.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, null, null, null);
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x000A1CA4 File Offset: 0x0009FEA4
	protected virtual void SetBossFlagDefeated()
	{
		this.BossDefeatedRelay.Dispatch();
		SaveManager.PlayerSaveData.SetFlag(this.m_bossSaveFlag, true);
		this.SetHighestBossNGBeaten(this.m_bossSaveFlag);
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Player, SavingType.FileOnly, true, null);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.GameMode, SavingType.FileOnly, true, null);
		saveBatch.End();
		BossEntranceRoomController.RunDoorCrumbleAnimation = true;
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x000A1D04 File Offset: 0x0009FF04
	private void SetHighestBossNGBeaten(PlayerSaveFlag saveFlag)
	{
		if (saveFlag <= PlayerSaveFlag.StudyBoss_Defeated)
		{
			if (saveFlag <= PlayerSaveFlag.ForestBoss_Defeated)
			{
				if (saveFlag != PlayerSaveFlag.CastleBoss_Defeated)
				{
					if (saveFlag != PlayerSaveFlag.ForestBoss_Defeated)
					{
						return;
					}
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ForestBoss_Defeated_FirstTime, true);
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Forest_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
					StoreAPIManager.GiveAchievement(AchievementType.BossForestDefeated, StoreType.All);
					if (BurdenManager.IsBurdenActive(BurdenType.ForestBossUp))
					{
						SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ForestBoss_Prime_Defeated_FirstTime, true);
						StoreAPIManager.GiveAchievement(AchievementType.BossForestAdvancedDefeated, StoreType.All);
						return;
					}
				}
				else
				{
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CastleBoss_Defeated_FirstTime, true);
					SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Castle_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
					StoreAPIManager.GiveAchievement(AchievementType.BossCastleDefeated, StoreType.All);
					if (BurdenManager.IsBurdenActive(BurdenType.CastleBossUp))
					{
						SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CastleBoss_Prime_Defeated_FirstTime, true);
						StoreAPIManager.GiveAchievement(AchievementType.BossCastleAdvancedDefeated, StoreType.All);
						return;
					}
				}
			}
			else if (saveFlag != PlayerSaveFlag.BridgeBoss_Defeated)
			{
				if (saveFlag != PlayerSaveFlag.StudyBoss_Defeated)
				{
					return;
				}
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.StudyBoss_Defeated_FirstTime, true);
				SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Study_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
				StoreAPIManager.GiveAchievement(AchievementType.BossStudyDefeated, StoreType.All);
				if (BurdenManager.IsBurdenActive(BurdenType.StudyBossUp))
				{
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.StudyBoss_Prime_Defeated_FirstTime, true);
					StoreAPIManager.GiveAchievement(AchievementType.BossStudyAdvancedDefeated, StoreType.All);
					return;
				}
			}
			else
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BridgeBoss_Defeated_FirstTime, true);
				SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Bridge_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
				StoreAPIManager.GiveAchievement(AchievementType.BossBridgeDefeated, StoreType.All);
				if (BurdenManager.IsBurdenActive(BurdenType.BridgeBossUp))
				{
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BridgeBoss_Prime_Defeated_FirstTime, true);
					StoreAPIManager.GiveAchievement(AchievementType.BossBridgeAdvancedDefeated, StoreType.All);
					return;
				}
			}
		}
		else if (saveFlag <= PlayerSaveFlag.CaveBoss_Defeated)
		{
			if (saveFlag != PlayerSaveFlag.TowerBoss_Defeated)
			{
				if (saveFlag != PlayerSaveFlag.CaveBoss_Defeated)
				{
					return;
				}
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveBoss_Defeated_FirstTime, true);
				SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Cave_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
				StoreAPIManager.GiveAchievement(AchievementType.BossCaveDefeated, StoreType.All);
				if (BurdenManager.IsBurdenActive(BurdenType.CaveBossUp))
				{
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveBoss_Prime_Defeated_FirstTime, true);
					StoreAPIManager.GiveAchievement(AchievementType.BossCaveAdvancedDefeated, StoreType.All);
					return;
				}
			}
			else
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TowerBoss_Defeated_FirstTime, true);
				SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Tower_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
				StoreAPIManager.GiveAchievement(AchievementType.BossTowerDefeated, StoreType.All);
				if (BurdenManager.IsBurdenActive(BurdenType.TowerBossUp))
				{
					SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TowerBoss_Prime_Defeated_FirstTime, true);
					StoreAPIManager.GiveAchievement(AchievementType.BossTowerAdvancedDefeated, StoreType.All);
					return;
				}
			}
		}
		else if (saveFlag != PlayerSaveFlag.FinalBoss_Defeated)
		{
			if (saveFlag != PlayerSaveFlag.GardenBoss_Defeated)
			{
				return;
			}
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.GardenBoss_Defeated_FirstTime, true);
			SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Garden_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
			StoreAPIManager.GiveAchievement(AchievementType.BossGardenDefeated, StoreType.All);
			if (BurdenManager.IsBurdenActive(BurdenType.GardenBossUp) || BurdenManager.IsBurdenActive(BurdenType.FinalBossUp))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.GardenBoss_Prime_Defeated_FirstTime, true);
				StoreAPIManager.GiveAchievement(AchievementType.BossGardenAdvancedDefeated, StoreType.All);
				return;
			}
		}
		else
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.FinalBoss_Defeated_FirstTime, true);
			SaveManager.ModeSaveData.SetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, BossID.Final_Boss, SaveManager.PlayerSaveData.NewGamePlusLevel, false);
			StoreAPIManager.GiveAchievement(AchievementType.BossFinalDefeated, StoreType.All);
			if (BurdenManager.IsBurdenActive(BurdenType.FinalBossUp))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.FinalBoss_Prime_Defeated_FirstTime, true);
				StoreAPIManager.GiveAchievement(AchievementType.BossFinalAdvancedDefeated, StoreType.All);
			}
		}
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x000A20A0 File Offset: 0x000A02A0
	protected void PlayAudio(string audioPath)
	{
		if (!string.IsNullOrEmpty(audioPath))
		{
			AudioManager.Play(this, audioPath, default(Vector3));
		}
	}

	// Token: 0x040025A6 RID: 9638
	protected const string CHEST_SPAWN_LOOP_AUDIO = "event:/SFX/Interactables/sfx_env_prop_bossChest_spawn_loop";

	// Token: 0x040025A7 RID: 9639
	protected const string CAMERA_PAN_TO_BOSS_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_zoomIn";

	// Token: 0x040025A8 RID: 9640
	protected const string CAMERA_PAN_TO_PLAYER_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_zoomOut";

	// Token: 0x040025A9 RID: 9641
	[SerializeField]
	protected PlayerSaveFlag m_bossSaveFlag;

	// Token: 0x040025AA RID: 9642
	[SerializeField]
	protected int m_numBosses;

	// Token: 0x040025AB RID: 9643
	[SerializeField]
	protected EnemySpawnController m_bossSpawnController;

	// Token: 0x040025AC RID: 9644
	[SerializeField]
	protected ChestSpawnController m_chestSpawnController;

	// Token: 0x040025AD RID: 9645
	[SerializeField]
	protected TunnelSpawnController m_tunnelSpawnController;

	// Token: 0x040025AE RID: 9646
	[Header("Intro Cutscene Variables")]
	[SerializeField]
	protected CinemachineVirtualCameraManager m_vcamManager;

	// Token: 0x040025AF RID: 9647
	[SerializeField]
	protected AnimationCurve m_cameraBlendCurve;

	// Token: 0x040025B0 RID: 9648
	[SerializeField]
	protected GameObject m_bossStartCamPos;

	// Token: 0x040025B1 RID: 9649
	[SerializeField]
	protected GameObject m_bossEndCamPos;

	// Token: 0x040025B2 RID: 9650
	protected int m_numBossesKilled;

	// Token: 0x040025B3 RID: 9651
	protected int m_numBossesDeadOrDying;

	// Token: 0x040025B4 RID: 9652
	protected bool m_listenersAssigned;

	// Token: 0x040025B5 RID: 9653
	protected static CinemachineBlendDefinition m_followBlend;

	// Token: 0x040025B6 RID: 9654
	protected BossObjectiveCompleteHUDEventArgs m_bossDefeatedArgs;

	// Token: 0x040025B7 RID: 9655
	protected WaitRL_Yield m_waitYield;

	// Token: 0x040025B8 RID: 9656
	public Relay IntroStartRelay = new Relay();

	// Token: 0x040025B9 RID: 9657
	public Relay IntroCompleteRelay = new Relay();

	// Token: 0x040025BA RID: 9658
	public Relay OutroStartRelay = new Relay();

	// Token: 0x040025BB RID: 9659
	public Relay OutroCompleteRelay = new Relay();

	// Token: 0x040025BC RID: 9660
	public Relay<float> BossTookDamageRelay = new Relay<float>();

	// Token: 0x040025BD RID: 9661
	public Relay BossDefeatedRelay = new Relay();

	// Token: 0x040025BE RID: 9662
	public Relay<int> PhaseChangedRelay = new Relay<int>();

	// Token: 0x040025BF RID: 9663
	public Relay BossSpawnAnimRelay = new Relay();

	// Token: 0x040025C0 RID: 9664
	private string m_description;

	// Token: 0x040025C1 RID: 9665
	protected EventInstance m_chestAudioEventInstance;

	// Token: 0x040025C2 RID: 9666
	private Action<MonoBehaviour, EventArgs> m_onBossDeath;

	// Token: 0x040025C3 RID: 9667
	private Action<MonoBehaviour, EventArgs> m_onBossHealthChange;

	// Token: 0x040025C4 RID: 9668
	private Action<MonoBehaviour, EventArgs> m_onModeShift;
}
