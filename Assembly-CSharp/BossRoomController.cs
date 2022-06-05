using System;
using System.Collections;
using Cinemachine;
using FMOD.Studio;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000842 RID: 2114
public class BossRoomController : BaseSpecialRoomController, IAudioEventEmitter
{
	// Token: 0x1700177D RID: 6013
	// (get) Token: 0x06004144 RID: 16708 RVA: 0x00024177 File Offset: 0x00022377
	public int NumBossesDeadOrDying
	{
		get
		{
			return this.m_numBossesDeadOrDying;
		}
	}

	// Token: 0x1700177E RID: 6014
	// (get) Token: 0x06004145 RID: 16709 RVA: 0x0002417F File Offset: 0x0002237F
	public int NumBossesDead
	{
		get
		{
			return this.m_numBossesKilled;
		}
	}

	// Token: 0x1700177F RID: 6015
	// (get) Token: 0x06004146 RID: 16710 RVA: 0x00024187 File Offset: 0x00022387
	public int NumBosses
	{
		get
		{
			return this.m_numBosses;
		}
	}

	// Token: 0x17001780 RID: 6016
	// (get) Token: 0x06004147 RID: 16711 RVA: 0x0002418F File Offset: 0x0002238F
	public bool AllBossesDeadOrDying
	{
		get
		{
			return this.NumBossesDeadOrDying >= this.NumBosses;
		}
	}

	// Token: 0x17001781 RID: 6017
	// (get) Token: 0x06004148 RID: 16712 RVA: 0x000241A2 File Offset: 0x000223A2
	public bool AllBossesDead
	{
		get
		{
			return this.NumBossesDead >= this.NumBosses;
		}
	}

	// Token: 0x17001782 RID: 6018
	// (get) Token: 0x06004149 RID: 16713 RVA: 0x000241B5 File Offset: 0x000223B5
	public TunnelSpawnController TunnelSpawnController
	{
		get
		{
			return this.m_tunnelSpawnController;
		}
	}

	// Token: 0x17001783 RID: 6019
	// (get) Token: 0x0600414A RID: 16714 RVA: 0x000241BD File Offset: 0x000223BD
	public virtual EnemySpawnController BossSpawnController
	{
		get
		{
			return this.m_bossSpawnController;
		}
	}

	// Token: 0x17001784 RID: 6020
	// (get) Token: 0x0600414B RID: 16715 RVA: 0x000241C5 File Offset: 0x000223C5
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

	// Token: 0x17001785 RID: 6021
	// (get) Token: 0x0600414C RID: 16716 RVA: 0x000241E1 File Offset: 0x000223E1
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

	// Token: 0x17001786 RID: 6022
	// (get) Token: 0x0600414D RID: 16717 RVA: 0x000241FD File Offset: 0x000223FD
	public CinemachineVirtualCameraManager VCamManager
	{
		get
		{
			return this.m_vcamManager;
		}
	}

	// Token: 0x17001787 RID: 6023
	// (get) Token: 0x0600414E RID: 16718 RVA: 0x00024205 File Offset: 0x00022405
	// (set) Token: 0x0600414F RID: 16719 RVA: 0x0002420D File Offset: 0x0002240D
	public bool IsInitialized { get; protected set; }

	// Token: 0x17001788 RID: 6024
	// (get) Token: 0x06004150 RID: 16720 RVA: 0x00024216 File Offset: 0x00022416
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

	// Token: 0x17001789 RID: 6025
	// (get) Token: 0x06004151 RID: 16721 RVA: 0x00024243 File Offset: 0x00022443
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

	// Token: 0x06004152 RID: 16722 RVA: 0x001064E4 File Offset: 0x001046E4
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

	// Token: 0x06004153 RID: 16723 RVA: 0x00106580 File Offset: 0x00104780
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

	// Token: 0x06004154 RID: 16724 RVA: 0x001065D8 File Offset: 0x001047D8
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		BossRoomController.bossSaveId = (int)this.m_bossSaveFlag;
		if (MainMenuWindowController.splitStep <= 23 && this.m_bossSaveFlag == PlayerSaveFlag.FinalBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 24;
		}
		else if (MainMenuWindowController.splitStep <= 21 && this.m_bossSaveFlag == PlayerSaveFlag.GardenBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 22;
		}
		else if (MainMenuWindowController.splitStep <= 19 && this.m_bossSaveFlag == PlayerSaveFlag.CaveBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 20;
		}
		else if (MainMenuWindowController.splitStep <= 17 && this.m_bossSaveFlag == PlayerSaveFlag.TowerBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 18;
		}
		else if (MainMenuWindowController.splitStep <= 15 && this.m_bossSaveFlag == PlayerSaveFlag.StudyBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 16;
		}
		else if (MainMenuWindowController.splitStep <= 11 && this.m_bossSaveFlag == PlayerSaveFlag.ForestBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 12;
		}
		else if (MainMenuWindowController.splitStep <= 7 && this.m_bossSaveFlag == PlayerSaveFlag.BridgeBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 8;
		}
		else if (MainMenuWindowController.splitStep <= 3 && this.m_bossSaveFlag == PlayerSaveFlag.CastleBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 4;
		}
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (!this.IsInitialized)
		{
			this.Initialize();
		}
		this.AddListeners();
		base.StopAllCoroutines();
		base.StartCoroutine(this.StartIntro());
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x00024264 File Offset: 0x00022464
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerExitRoom(sender, eventArgs);
		this.m_numBossesKilled = 0;
		this.m_numBossesDeadOrDying = 0;
		this.RemoveListeners();
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00024282 File Offset: 0x00022482
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

	// Token: 0x06004157 RID: 16727 RVA: 0x000242B1 File Offset: 0x000224B1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_chestAudioEventInstance.isValid())
		{
			this.m_chestAudioEventInstance.release();
		}
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x000242D2 File Offset: 0x000224D2
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

	// Token: 0x06004159 RID: 16729 RVA: 0x00024309 File Offset: 0x00022509
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

	// Token: 0x0600415A RID: 16730 RVA: 0x0010671C File Offset: 0x0010491C
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

	// Token: 0x0600415B RID: 16731 RVA: 0x00106778 File Offset: 0x00104978
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

	// Token: 0x0600415C RID: 16732 RVA: 0x00024340 File Offset: 0x00022540
	protected virtual void OnModeShift(object sender, EventArgs args)
	{
		this.PhaseChangedRelay.Dispatch(1);
	}

	// Token: 0x0600415D RID: 16733 RVA: 0x0002434E File Offset: 0x0002254E
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

	// Token: 0x0600415E RID: 16734 RVA: 0x0002435D File Offset: 0x0002255D
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

	// Token: 0x0600415F RID: 16735 RVA: 0x001067EC File Offset: 0x001049EC
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

	// Token: 0x06004160 RID: 16736 RVA: 0x0002436C File Offset: 0x0002256C
	protected virtual void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		this.m_bossDefeatedArgs.Initialize(this.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, null, null, null);
	}

	// Token: 0x06004161 RID: 16737 RVA: 0x001068DC File Offset: 0x00104ADC
	protected virtual void SetBossFlagDefeated()
	{
		BossRoomController.bossSaveId_finish = (int)this.m_bossSaveFlag;
		if (MainMenuWindowController.splitStep <= 24 && this.m_bossSaveFlag == PlayerSaveFlag.FinalBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 25;
		}
		else if (MainMenuWindowController.splitStep <= 22 && this.m_bossSaveFlag == PlayerSaveFlag.GardenBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 23;
		}
		else if (MainMenuWindowController.splitStep <= 20 && this.m_bossSaveFlag == PlayerSaveFlag.CaveBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 21;
		}
		else if (MainMenuWindowController.splitStep <= 18 && this.m_bossSaveFlag == PlayerSaveFlag.TowerBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 19;
		}
		else if (MainMenuWindowController.splitStep <= 16 && this.m_bossSaveFlag == PlayerSaveFlag.StudyBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 17;
		}
		else if (MainMenuWindowController.splitStep <= 12 && this.m_bossSaveFlag == PlayerSaveFlag.ForestBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 13;
		}
		else if (MainMenuWindowController.splitStep <= 8 && this.m_bossSaveFlag == PlayerSaveFlag.BridgeBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 9;
		}
		else if (MainMenuWindowController.splitStep <= 4 && this.m_bossSaveFlag == PlayerSaveFlag.CastleBoss_Defeated)
		{
			MainMenuWindowController.splitStep = 5;
		}
		this.BossDefeatedRelay.Dispatch();
		SaveManager.PlayerSaveData.SetFlag(this.m_bossSaveFlag, true);
		this.SetHighestBossNGBeaten(this.m_bossSaveFlag);
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Player, SavingType.FileOnly, true, null);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.GameMode, SavingType.FileOnly, true, null);
		saveBatch.End();
		BossEntranceRoomController.RunDoorCrumbleAnimation = true;
	}

	// Token: 0x06004162 RID: 16738 RVA: 0x00106A44 File Offset: 0x00104C44
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

	// Token: 0x06004163 RID: 16739 RVA: 0x00106DD4 File Offset: 0x00104FD4
	protected void PlayAudio(string audioPath)
	{
		if (!string.IsNullOrEmpty(audioPath))
		{
			AudioManager.Play(this, audioPath, default(Vector3));
		}
	}

	// Token: 0x04003310 RID: 13072
	protected const string CHEST_SPAWN_LOOP_AUDIO = "event:/SFX/Interactables/sfx_env_prop_bossChest_spawn_loop";

	// Token: 0x04003311 RID: 13073
	protected const string CAMERA_PAN_TO_BOSS_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_zoomIn";

	// Token: 0x04003312 RID: 13074
	protected const string CAMERA_PAN_TO_PLAYER_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_zoomOut";

	// Token: 0x04003313 RID: 13075
	[SerializeField]
	protected PlayerSaveFlag m_bossSaveFlag;

	// Token: 0x04003314 RID: 13076
	[SerializeField]
	protected int m_numBosses;

	// Token: 0x04003315 RID: 13077
	[SerializeField]
	protected EnemySpawnController m_bossSpawnController;

	// Token: 0x04003316 RID: 13078
	[SerializeField]
	protected ChestSpawnController m_chestSpawnController;

	// Token: 0x04003317 RID: 13079
	[SerializeField]
	protected TunnelSpawnController m_tunnelSpawnController;

	// Token: 0x04003318 RID: 13080
	[Header("Intro Cutscene Variables")]
	[SerializeField]
	protected CinemachineVirtualCameraManager m_vcamManager;

	// Token: 0x04003319 RID: 13081
	[SerializeField]
	protected AnimationCurve m_cameraBlendCurve;

	// Token: 0x0400331A RID: 13082
	[SerializeField]
	protected GameObject m_bossStartCamPos;

	// Token: 0x0400331B RID: 13083
	[SerializeField]
	protected GameObject m_bossEndCamPos;

	// Token: 0x0400331C RID: 13084
	protected int m_numBossesKilled;

	// Token: 0x0400331D RID: 13085
	protected int m_numBossesDeadOrDying;

	// Token: 0x0400331E RID: 13086
	protected bool m_listenersAssigned;

	// Token: 0x0400331F RID: 13087
	protected static CinemachineBlendDefinition m_followBlend;

	// Token: 0x04003320 RID: 13088
	protected BossObjectiveCompleteHUDEventArgs m_bossDefeatedArgs;

	// Token: 0x04003321 RID: 13089
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04003322 RID: 13090
	public Relay IntroStartRelay = new Relay();

	// Token: 0x04003323 RID: 13091
	public Relay IntroCompleteRelay = new Relay();

	// Token: 0x04003324 RID: 13092
	public Relay OutroStartRelay = new Relay();

	// Token: 0x04003325 RID: 13093
	public Relay OutroCompleteRelay = new Relay();

	// Token: 0x04003326 RID: 13094
	public Relay<float> BossTookDamageRelay = new Relay<float>();

	// Token: 0x04003327 RID: 13095
	public Relay BossDefeatedRelay = new Relay();

	// Token: 0x04003328 RID: 13096
	public Relay<int> PhaseChangedRelay = new Relay<int>();

	// Token: 0x04003329 RID: 13097
	public Relay BossSpawnAnimRelay = new Relay();

	// Token: 0x0400332A RID: 13098
	private string m_description;

	// Token: 0x0400332B RID: 13099
	protected EventInstance m_chestAudioEventInstance;

	// Token: 0x0400332C RID: 13100
	private Action<MonoBehaviour, EventArgs> m_onBossDeath;

	// Token: 0x0400332D RID: 13101
	private Action<MonoBehaviour, EventArgs> m_onBossHealthChange;

	// Token: 0x0400332E RID: 13102
	private Action<MonoBehaviour, EventArgs> m_onModeShift;

	// Token: 0x04003330 RID: 13104
	public static int bossSaveId = 456456456;

	// Token: 0x04003331 RID: 13105
	public static int bossSaveId_finish = 567567567;
}
