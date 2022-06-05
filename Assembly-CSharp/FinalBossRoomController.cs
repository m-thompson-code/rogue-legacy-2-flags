using System;
using System.Collections;
using Cinemachine;
using FMOD.Studio;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020004FE RID: 1278
public class FinalBossRoomController : BossRoomController
{
	// Token: 0x170011C4 RID: 4548
	// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x000A3608 File Offset: 0x000A1808
	public override EnemySpawnController BossSpawnController
	{
		get
		{
			if (BurdenManager.IsBurdenActive(BurdenType.FinalBossUp))
			{
				return this.m_bossUpSpawnController;
			}
			return base.BossSpawnController;
		}
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000A3624 File Offset: 0x000A1824
	protected override void Awake()
	{
		base.Awake();
		this.m_colourShiftRoom = new Action<bool, float>(this.ColourShiftRoom);
		this.m_onBossIntroStopSpeedUp = new Action(this.OnBossIntroStopSpeedUp);
		this.m_onBossIntroStartSpeedUp = new Action(this.OnBossIntroStartSpeedUp);
		this.m_rootsArray = this.m_rootsGO.GetComponentsInChildren<Animator>();
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000A367E File Offset: 0x000A187E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_introSFXEventInstance_01.isValid())
		{
			this.m_introSFXEventInstance_01.release();
		}
		if (this.m_introSFXEventInstance_02.isValid())
		{
			this.m_introSFXEventInstance_02.release();
		}
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000A36B8 File Offset: 0x000A18B8
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_FINAL_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000A36F0 File Offset: 0x000A18F0
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (!WindowManager.GetIsWindowLoaded(WindowID.BossIntro))
		{
			WindowManager.LoadWindow(WindowID.BossIntro);
		}
		(WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController).StopSpeedUpRelay.AddListener(this.m_onBossIntroStopSpeedUp, false);
		(WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController).StartSpeedUpRelay.AddListener(this.m_onBossIntroStartSpeedUp, false);
		this.m_bossIntroStopSpeedUpRelayAdded = true;
		this.m_currentActiveEventInstance = default(EventInstance);
		if (!this.m_introSFXEventInstance_01.isValid())
		{
			this.m_introSFXEventInstance_01 = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_intro_sequence", base.transform);
		}
		if (!this.m_introSFXEventInstance_02.isValid())
		{
			this.m_introSFXEventInstance_02 = AudioUtility.GetEventInstance("event:/SFX/Enemies/Cain/sfx_cain_intro_sequence_02", base.transform);
		}
		if (BurdenManager.IsBurdenActive(BurdenType.FinalBossUp))
		{
			if (this.m_bossSpawnController.EnemyInstance)
			{
				this.m_bossSpawnController.EnemyInstance.gameObject.SetActive(false);
			}
		}
		else if (this.m_bossUpSpawnController.EnemyInstance)
		{
			this.m_bossUpSpawnController.EnemyInstance.gameObject.SetActive(false);
		}
		if (this.m_storedStartingCamPos == Vector3.zero)
		{
			this.m_storedStartingCamPos = this.m_bossStartCamPos.transform.position;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		if ((playerController.transform.localPosition.x < base.Boss.transform.localPosition.x && !playerController.IsFacingRight) || (playerController.transform.localPosition.x > base.Boss.transform.localPosition.x && playerController.IsFacingRight))
		{
			playerController.CharacterCorgi.Flip(false, true);
		}
		if (!SaveManager.PlayerSaveData.SpokenToFinalBoss)
		{
			base.StartCoroutine(this.DisableBoss());
			return;
		}
		base.StartCoroutine(this.SetupColourShiftRelayCoroutine());
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000A38C8 File Offset: 0x000A1AC8
	private IEnumerator DisableBoss()
	{
		while (!base.Boss.IsInitialized)
		{
			yield return null;
		}
		base.Boss.gameObject.SetActive(false);
		yield return this.SetupColourShiftRelayCoroutine();
		yield break;
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x000A38D7 File Offset: 0x000A1AD7
	private IEnumerator SetupColourShiftRelayCoroutine()
	{
		while (!base.Boss.IsInitialized)
		{
			yield return null;
		}
		FinalBoss_Basic_AIScript finalBoss_Basic_AIScript = base.Boss.LogicController.LogicScript as FinalBoss_Basic_AIScript;
		if (finalBoss_Basic_AIScript)
		{
			finalBoss_Basic_AIScript.ColourShiftRelay.AddListener(this.m_colourShiftRoom, false);
		}
		yield break;
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x000A38E8 File Offset: 0x000A1AE8
	protected override void OnDisable()
	{
		if (this.m_bossIntroStopSpeedUpRelayAdded)
		{
			this.m_bossIntroStopSpeedUpRelayAdded = false;
			(WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController).StopSpeedUpRelay.RemoveListener(this.m_onBossIntroStopSpeedUp);
			(WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController).StartSpeedUpRelay.RemoveListener(this.m_onBossIntroStartSpeedUp);
		}
		if (base.Boss && base.Boss.IsInitialized)
		{
			FinalBoss_Basic_AIScript finalBoss_Basic_AIScript = base.Boss.LogicController.LogicScript as FinalBoss_Basic_AIScript;
			if (finalBoss_Basic_AIScript)
			{
				finalBoss_Basic_AIScript.ColourShiftRelay.RemoveListener(this.m_colourShiftRoom);
			}
		}
		if (this.m_flameEffect && this.m_flameEffect.IsPlaying)
		{
			this.m_flameEffect.Stop(EffectStopType.Immediate);
		}
		this.m_flameEffect = null;
		base.OnDisable();
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x000A39BC File Offset: 0x000A1BBC
	private void ColourShiftRoom(bool shiftToWhite, float lerpSpeed)
	{
		if (base.Boss && !base.Boss.IsDead)
		{
			bool flag = base.Boss.EnemyRank > EnemyRank.Basic;
			if (shiftToWhite)
			{
				if (flag)
				{
					if (this.m_flameEffect && this.m_flameEffect.IsPlaying)
					{
						this.m_flameEffect.Stop(EffectStopType.Gracefully);
						this.m_flameEffect = null;
					}
					CameraController.LerpBGPostProcessToProfile(this.m_lightBGAdvancedProfile, lerpSpeed);
					CameraController.LerpFGPostProcessToProfile(this.m_lightFGAdvancedProfile, lerpSpeed);
				}
				else
				{
					CameraController.LerpBGPostProcessToProfile(this.m_lightBGProfile, lerpSpeed);
					CameraController.LerpFGPostProcessToProfile(this.m_lightFGProfile, lerpSpeed);
				}
				AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_arena_shift_darkToLight", CameraController.GameCamera.transform.position);
				return;
			}
			if (flag)
			{
				if (!this.m_flameEffect || !this.m_flameEffect.IsPlaying)
				{
					this.m_flameEffect = EffectManager.PlayEffect(base.gameObject, null, "CainPrime_Flames_Effect", base.transform.position, 99999f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				}
				CameraController.LerpBGPostProcessToProfile(this.m_darkBGAdvancedProfile, lerpSpeed);
				CameraController.LerpFGPostProcessToProfile(this.m_darkFGAdvancedProfile, lerpSpeed);
			}
			else
			{
				CameraController.LerpBGPostProcessToProfile(this.m_darkBGProfile, lerpSpeed);
				CameraController.LerpFGPostProcessToProfile(this.m_darkFGProfile, lerpSpeed);
			}
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_arena_shift_lightToDark", CameraController.GameCamera.transform.position);
		}
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x000A3B0A File Offset: 0x000A1D0A
	protected override IEnumerator StartIntro()
	{
		if (!SaveManager.PlayerSaveData.SpokenToFinalBoss)
		{
			Prop propInstance = base.Room.gameObject.FindObjectReference("CainProp", false, false).PropInstance;
			yield break;
		}
		if (base.Boss.EnemyRank > EnemyRank.Basic)
		{
			int trigger = Animator.StringToHash("Extend");
			Animator[] rootsArray = this.m_rootsArray;
			for (int i = 0; i < rootsArray.Length; i++)
			{
				rootsArray[i].SetTrigger(trigger);
			}
		}
		this.ColourShiftRoom(true, 0.75f);
		yield return base.StartIntro();
		yield break;
		IL_60:
		yield return null;
		if (!base.Boss.IsInitialized)
		{
			goto IL_60;
		}
		yield return null;
		base.Boss.gameObject.SetActive(true);
		if (!TraitManager.IsTraitActive(TraitType.EnemiesCensored))
		{
			goto IL_C4;
		}
		EnemiesCensored_Trait.ApplyCensoredEffect(base.Boss);
		IL_C4:
		if (!TraitManager.IsTraitActive(TraitType.CheerOnKills))
		{
			goto IL_EB;
		}
		(TraitManager.GetActiveTrait(TraitType.CheerOnKills) as CheerOnKills_Trait).ApplyOnEnemy(base.Boss, false);
		IL_EB:
		yield break;
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x000A3B19 File Offset: 0x000A1D19
	public void StartIntroManually()
	{
		base.StartCoroutine(this.StartIntro2());
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x000A3B28 File Offset: 0x000A1D28
	private IEnumerator StartIntro2()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_bossStartCamPos.transform.position = playerController.transform.position;
		RewiredMapController.SetIsInCutscene(true);
		if (global::AnimatorUtility.HasParameter(base.Boss.Animator, "Turn"))
		{
			base.Boss.Animator.ResetTrigger("Turn");
		}
		if ((base.Boss.transform.localPosition.x < playerController.transform.localPosition.x && !base.Boss.CharacterCorgi.IsFacingRight) || (base.Boss.transform.localPosition.x > playerController.transform.localPosition.x && base.Boss.CharacterCorgi.IsFacingRight))
		{
			base.Boss.CharacterCorgi.Flip(false, true);
		}
		yield return null;
		this.IntroStartRelay.Dispatch();
		while (!base.Boss.IsInitialized)
		{
			yield return null;
		}
		while (!base.Boss.LogicController.IsInitialized)
		{
			yield return null;
		}
		CainPropController cainProp = null;
		PropSpawnController propSpawnController = base.Room.gameObject.FindObjectReference("CainProp", false, false);
		cainProp = propSpawnController.PropInstance.GetComponent<CainPropController>();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, null);
		base.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_zoomIn");
		base.Boss.Room.CinemachineCamera.SetFollowTarget(this.m_bossStartCamPos.transform);
		if (!WindowManager.GetIsWindowLoaded(WindowID.BossIntro))
		{
			WindowManager.LoadWindow(WindowID.BossIntro);
		}
		BossIntroWindowController bossWindow = WindowManager.GetWindowController(WindowID.BossIntro) as BossIntroWindowController;
		bossWindow.SetEnemyType(base.Boss.EnemyType, base.Boss.EnemyRank);
		bossWindow.FadeInBarsAtStart = true;
		WindowManager.SetWindowIsOpen(WindowID.BossIntro, true);
		this.m_waitYield.CreateNew(1f, true);
		yield return this.m_waitYield;
		CinemachineBlendDefinition storedBlend = CameraController.CinemachineBrain.m_DefaultBlend;
		CameraController.CinemachineBrain.m_DefaultBlend = BossRoomController.m_followBlend;
		float zoomLevel = CameraController.ZoomLevel;
		if (zoomLevel != 1f)
		{
			base.VCamManager.SetLensSize(CameraController.GetVirtualCameraLensSize(zoomLevel));
		}
		base.Boss.Room.CinemachineCamera.SetIsActiveCamera(false);
		base.VCamManager.VirtualCamera.gameObject.SetActive(true);
		base.VCamManager.SetIsActiveCamera(true);
		base.VCamManager.SetFollowTarget(this.m_bossEndCamPos.transform);
		this.m_bossStartCamPos.transform.position = this.m_storedStartingCamPos;
		cainProp.Animator.Play("Eat");
		this.m_waitYield.CreateNew(0.55f, false);
		yield return this.m_waitYield;
		if (this.m_introSFXEventInstance_01.isValid())
		{
			AudioManager.Play(null, this.m_introSFXEventInstance_01);
			this.m_currentActiveEventInstance = this.m_introSFXEventInstance_01;
			this.m_introSFXStartTime = Time.time;
		}
		this.m_waitYield.CreateNew(3.45f, false);
		yield return this.m_waitYield;
		cainProp.Animator.Play("Poisoned");
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		cainProp.Animator.speed = 0f;
		bossWindow.StopSpeedUp();
		if (this.m_introSFXEventInstance_01.isValid())
		{
			AudioManager.Stop(this.m_introSFXEventInstance_01, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_currentActiveEventInstance = default(EventInstance);
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_CAIN_NAME_1", "LOC_ID_CAIN_ENDING_REFIGHT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		if (!WindowManager.GetIsWindowLoaded(WindowID.Dialogue))
		{
			WindowManager.LoadWindow(WindowID.Dialogue);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.24f, false);
		yield return this.m_waitYield;
		if (this.m_introSFXEventInstance_02.isValid())
		{
			AudioManager.Play(null, this.m_introSFXEventInstance_02);
			this.m_currentActiveEventInstance = this.m_introSFXEventInstance_02;
			this.m_introSFXStartTime = Time.time;
		}
		cainProp.Animator.speed = 1f;
		base.StartCoroutine(this.MovePlayerToEndPos());
		this.m_waitYield.CreateNew(0.26f, false);
		yield return this.m_waitYield;
		this.BossSpawnAnimRelay.Dispatch();
		base.Boss.gameObject.SetActive(true);
		if (TraitManager.IsTraitActive(TraitType.EnemiesCensored))
		{
			EnemiesCensored_Trait.ApplyCensoredEffect(base.Boss);
		}
		if (TraitManager.IsTraitActive(TraitType.CheerOnKills))
		{
			(TraitManager.GetActiveTrait(TraitType.CheerOnKills) as CheerOnKills_Trait).ApplyOnEnemy(base.Boss, false);
		}
		cainProp.gameObject.SetLayerRecursively(29, false);
		cainProp.Cape.gameObject.SetLayerRecursively(0, false);
		cainProp.Animator.Play("Transform");
		yield return base.Boss.LogicController.LogicScript.SpawnAnim();
		if (base.Boss.EnemyRank > EnemyRank.Basic)
		{
			int trigger = Animator.StringToHash("Extend");
			Animator[] rootsArray = this.m_rootsArray;
			for (int i = 0; i < rootsArray.Length; i++)
			{
				rootsArray[i].SetTrigger(trigger);
			}
		}
		this.ColourShiftRoom(true, 0.75f);
		cainProp.gameObject.SetActive(false);
		cainProp.gameObject.SetLayerRecursively(0, false);
		bossWindow.DisplayBossName = true;
		while (WindowManager.GetIsWindowOpen(WindowID.BossIntro))
		{
			yield return null;
		}
		base.Boss.Room.CinemachineCamera.VirtualCamera.gameObject.SetActive(true);
		base.Boss.Room.CinemachineCamera.SetIsActiveCamera(true);
		base.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_zoomOut");
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, new PlayerHUDVisibilityEventArgs(0.5f));
		base.Boss.LogicController.DisableLogicActivationByDistance = false;
		base.VCamManager.VirtualCamera.gameObject.SetActive(false);
		CameraController.CinemachineBrain.m_DefaultBlend = storedBlend;
		this.IntroCompleteRelay.Dispatch();
		SaveManager.PlayerSaveData.SpokenToFinalBoss = true;
		SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.Fruit, 0, false, false);
		if (this.m_introSFXEventInstance_02.isValid())
		{
			AudioManager.Stop(this.m_introSFXEventInstance_02, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_currentActiveEventInstance = default(EventInstance);
		RewiredMapController.SetIsInCutscene(false);
		yield break;
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x000A3B37 File Offset: 0x000A1D37
	private IEnumerator MovePlayerToEndPos()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 storedStartingCamPos = this.m_storedStartingCamPos;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(storedStartingCamPos, true);
		playerController.SetFacing(true);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x000A3B46 File Offset: 0x000A1D46
	private void OnBossIntroStartSpeedUp()
	{
		if (this.m_currentActiveEventInstance.isValid())
		{
			this.m_currentActiveEventInstance.setPaused(true);
		}
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x000A3B64 File Offset: 0x000A1D64
	private void OnBossIntroStopSpeedUp()
	{
		if (this.m_currentActiveEventInstance.isValid())
		{
			float num = Time.time - this.m_introSFXStartTime;
			this.m_currentActiveEventInstance.setTimelinePosition((int)(num * 1000f));
			this.m_currentActiveEventInstance.setPaused(false);
		}
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x000A3BAC File Offset: 0x000A1DAC
	public void SmashWindows()
	{
		foreach (PropSpawnController propSpawnController in this.m_windowsToSmashArray)
		{
			if (propSpawnController.PropInstance)
			{
				propSpawnController.PropInstance.Animators[0].SetTrigger("Smash");
				propSpawnController.PropInstance.Animators[0].Play("Smash", 0, 0.8f);
			}
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_death_windowsShatter", CameraController.GameCamera.transform.position);
	}

	// Token: 0x06002FD5 RID: 12245 RVA: 0x000A3C30 File Offset: 0x000A1E30
	public void ColourShiftToDefault()
	{
		CameraController.LerpBGPostProcessToProfile(this.m_defaultBGProfile, 2f);
		CameraController.LerpFGPostProcessToProfile(this.m_defaultFGProfile, 2f);
		if (base.Boss.EnemyRank > EnemyRank.Basic)
		{
			if (this.m_flameEffect && this.m_flameEffect.IsPlaying)
			{
				this.m_flameEffect.Stop(EffectStopType.Gracefully);
			}
			this.m_flameEffect = null;
			int trigger = Animator.StringToHash("Retract");
			Animator[] rootsArray = this.m_rootsArray;
			for (int i = 0; i < rootsArray.Length; i++)
			{
				rootsArray[i].SetTrigger(trigger);
			}
		}
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x000A3CC1 File Offset: 0x000A1EC1
	protected override IEnumerator StartOutro()
	{
		yield return base.StartOutro();
		Tunnel earthShiftTunnel = this.m_earthShiftTunnelSpawner.Tunnel;
		earthShiftTunnel.SetIsVisible(true);
		earthShiftTunnel.Interactable.ForceDisableInteractPrompt(false);
		if (base.Boss.transform.position.x > base.Room.Bounds.center.x)
		{
			float num = (earthShiftTunnel.transform.position.x - base.Room.Bounds.center.x) * 2f;
			Vector3 position = earthShiftTunnel.transform.position;
			position.x -= num;
			earthShiftTunnel.transform.position = position;
		}
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Cain/sfx_cain_defeated_doorAppear", earthShiftTunnel.transform.position);
		SpriteRenderer tunnelRenderer = earthShiftTunnel.GetComponentInChildren<SpriteRenderer>(true);
		Color tunnelColour = tunnelRenderer.color;
		tunnelColour.a = 0f;
		tunnelRenderer.color = tunnelColour;
		float fadeDuration = 1f;
		float delay = Time.time + fadeDuration;
		while (Time.time < delay)
		{
			float a = 1f - (delay - Time.time) / fadeDuration;
			tunnelColour.a = a;
			tunnelRenderer.color = tunnelColour;
			yield return null;
		}
		tunnelColour.a = 1f;
		tunnelRenderer.color = tunnelColour;
		earthShiftTunnel.SetIsLocked(false);
		yield break;
	}

	// Token: 0x06002FD7 RID: 12247 RVA: 0x000A3CD0 File Offset: 0x000A1ED0
	protected override void TeleportOut()
	{
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x000A3CD2 File Offset: 0x000A1ED2
	protected override void SetBossFlagDefeated()
	{
		base.SetBossFlagDefeated();
		SaveManager.PlayerSaveData.EndingSpawnRoom = EndingSpawnRoomType.Hallway;
	}

	// Token: 0x04002612 RID: 9746
	private const string TRANSITION_TO_LIGHT_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_arena_shift_darkToLight";

	// Token: 0x04002613 RID: 9747
	private const string TRANSITION_TO_DARK_SFX_NAME = "event:/SFX/Enemies/Cain/sfx_cain_arena_shift_lightToDark";

	// Token: 0x04002614 RID: 9748
	[SerializeField]
	private TunnelSpawnController m_earthShiftTunnelSpawner;

	// Token: 0x04002615 RID: 9749
	[SerializeField]
	private MobilePostProcessingProfile m_defaultBGProfile;

	// Token: 0x04002616 RID: 9750
	[SerializeField]
	private MobilePostProcessingProfile m_lightBGProfile;

	// Token: 0x04002617 RID: 9751
	[SerializeField]
	private MobilePostProcessingProfile m_darkBGProfile;

	// Token: 0x04002618 RID: 9752
	[SerializeField]
	private MobilePostProcessingProfile m_lightBGAdvancedProfile;

	// Token: 0x04002619 RID: 9753
	[SerializeField]
	private MobilePostProcessingProfile m_darkBGAdvancedProfile;

	// Token: 0x0400261A RID: 9754
	[SerializeField]
	private MobilePostProcessingProfile m_defaultFGProfile;

	// Token: 0x0400261B RID: 9755
	[SerializeField]
	private MobilePostProcessingProfile m_lightFGProfile;

	// Token: 0x0400261C RID: 9756
	[SerializeField]
	private MobilePostProcessingProfile m_darkFGProfile;

	// Token: 0x0400261D RID: 9757
	[SerializeField]
	private MobilePostProcessingProfile m_lightFGAdvancedProfile;

	// Token: 0x0400261E RID: 9758
	[SerializeField]
	private MobilePostProcessingProfile m_darkFGAdvancedProfile;

	// Token: 0x0400261F RID: 9759
	[SerializeField]
	private GameObject m_rootsGO;

	// Token: 0x04002620 RID: 9760
	[SerializeField]
	private PropSpawnController[] m_windowsToSmashArray;

	// Token: 0x04002621 RID: 9761
	[SerializeField]
	private EnemySpawnController m_bossUpSpawnController;

	// Token: 0x04002622 RID: 9762
	private Vector3 m_storedStartingCamPos = Vector3.zero;

	// Token: 0x04002623 RID: 9763
	private Action<bool, float> m_colourShiftRoom;

	// Token: 0x04002624 RID: 9764
	private BaseEffect m_flameEffect;

	// Token: 0x04002625 RID: 9765
	private Animator[] m_rootsArray;

	// Token: 0x04002626 RID: 9766
	private EventInstance m_introSFXEventInstance_01;

	// Token: 0x04002627 RID: 9767
	private EventInstance m_introSFXEventInstance_02;

	// Token: 0x04002628 RID: 9768
	private EventInstance m_currentActiveEventInstance;

	// Token: 0x04002629 RID: 9769
	private float m_introSFXStartTime;

	// Token: 0x0400262A RID: 9770
	private bool m_bossIntroStopSpeedUpRelayAdded;

	// Token: 0x0400262B RID: 9771
	private Action m_onBossIntroStopSpeedUp;

	// Token: 0x0400262C RID: 9772
	private Action m_onBossIntroStartSpeedUp;
}
