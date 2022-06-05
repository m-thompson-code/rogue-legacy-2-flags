using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000966 RID: 2406
public class LineageWindowController : WindowController, IAudioEventEmitter, ILocalizable
{
	// Token: 0x1700197A RID: 6522
	// (get) Token: 0x06004933 RID: 18739 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x1700197B RID: 6523
	// (get) Token: 0x06004934 RID: 18740 RVA: 0x00028363 File Offset: 0x00026563
	public int CurrentSelectedCharacterIndex
	{
		get
		{
			return this.m_selectedCharacterIndex;
		}
	}

	// Token: 0x1700197C RID: 6524
	// (get) Token: 0x06004935 RID: 18741 RVA: 0x0011BD30 File Offset: 0x00119F30
	public int NumberOfSuccessors
	{
		get
		{
			int num = (int)SkillTreeManager.GetSkillTreeObj(SkillTreeType.More_Children).CurrentStatGain;
			return 3 + num;
		}
	}

	// Token: 0x1700197D RID: 6525
	// (get) Token: 0x06004936 RID: 18742 RVA: 0x00003E42 File Offset: 0x00002042
	public override WindowID ID
	{
		get
		{
			return WindowID.Lineage;
		}
	}

	// Token: 0x1700197E RID: 6526
	// (get) Token: 0x06004937 RID: 18743 RVA: 0x0002836B File Offset: 0x0002656B
	public Scene Scene
	{
		get
		{
			return base.gameObject.scene;
		}
	}

	// Token: 0x06004938 RID: 18744 RVA: 0x0011BD54 File Offset: 0x00119F54
	private void Awake()
	{
		this.m_refreshText = new Action<object, EventArgs>(this.RefreshText);
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmHeirSelection = new Action(this.ConfirmHeirSelection);
		this.m_confirmQuitSelection = new Action(this.ConfirmQuitSelection);
		this.m_onConfirmHeirButtonPressed = new Action<InputActionEventData>(this.OnConfirmHeirButtonPressed);
		this.m_onViewLineageButtonPressed = new Action<InputActionEventData>(this.OnViewLineageButtonPressed);
		this.m_onRandomizeChildrenPressed = new Action<InputActionEventData>(this.OnRandomizeChildrenPressed);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalDirectionPressed = new Action<InputActionEventData>(this.OnHorizontalDirectionPressed);
		this.m_onHorizontalButtonReleased = new Action<InputActionEventData>(this.OnHorizontalButtonReleased);
		this.m_onVerticalButtonPressed = new Action<InputActionEventData>(this.OnVerticalButtonPressed);
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x0011BE28 File Offset: 0x0011A028
	private void Start()
	{
		Camera camera = Camera.main;
		if (CameraController.IsInstantiated)
		{
			camera = CameraController.GameCamera;
		}
		Vector3 position = camera.transform.position;
		position.x = this.m_lineageRoomController.EndingRoom.transform.position.x;
		position.y = this.m_lineageRoomController.EndingRoom.transform.position.y;
		position.x -= 3.7f;
		camera.transform.position = position;
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x0011BEB4 File Offset: 0x0011A0B4
	public override void Initialize()
	{
		this.m_characterDataArray = new CharacterData[this.NumberOfSuccessors];
		this.m_playerModels = new PlayerLookController[this.NumberOfSuccessors];
		this.m_goldFlags = new LineageGoldFlagController[this.NumberOfSuccessors];
		this.m_lineageEventArgs = new LineageHeirChangedEventArgs(null, false, false);
		base.Initialize();
	}

	// Token: 0x0600493B RID: 18747 RVA: 0x0011BF08 File Offset: 0x0011A108
	private void CreateRandomCharacters()
	{
		LineageRoomController component = this.m_lineageRoomController.EndingRoom.GetComponent<LineageRoomController>();
		Vector3 position = component.LineageModelPositionObject.transform.position;
		float num = 7.5f;
		float num2 = num / (float)Mathf.Clamp(this.NumberOfSuccessors - 1, 1, 999);
		float num3 = num / 2f;
		List<CharacterData> charsAlreadyChosen = new List<CharacterData>();
		for (int i = 0; i < this.m_characterDataArray.Length; i++)
		{
			if (!this.m_playerModels[i])
			{
				this.m_playerModels[i] = UnityEngine.Object.Instantiate<PlayerLookController>(this.m_playerModelPrefab, component.transform);
			}
			float x = position.x - num3 + num2 * (float)i;
			float y = position.y;
			float z = position.z - (float)i * 0.3f;
			Vector3 position2 = new Vector3(x, y, z);
			this.m_playerModels[i].transform.position = position2;
			bool forceRandomizeKit = i == 0 && SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ForceRandomizeKit).CurrentEquippedLevel > 0;
			this.m_characterDataArray[i] = CharacterCreator.GenerateRandomCharacter(charsAlreadyChosen, forceRandomizeKit);
			if (i == this.m_characterDataArray.Length - 1)
			{
				bool flag = false;
				SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ChooseYourClass);
				if (!soulShopObj.IsNativeNull() && soulShopObj.CurrentEquippedLevel > 0)
				{
					ClassType soulShopClassChosen = SaveManager.ModeSaveData.SoulShopClassChosen;
					if (soulShopClassChosen != ClassType.None)
					{
						CharacterCreator.GenerateClass(soulShopClassChosen, this.m_characterDataArray[i]);
						flag = true;
					}
				}
				if (flag)
				{
					if (this.m_characterDataArray[i].TraitOne == TraitType.RandomizeKit)
					{
						this.m_characterDataArray[i].TraitOne = this.m_characterDataArray[i].TraitTwo;
						this.m_characterDataArray[i].AntiqueOneOwned = this.m_characterDataArray[i].AntiqueTwoOwned;
						this.m_characterDataArray[i].TraitTwo = TraitType.None;
						this.m_characterDataArray[i].AntiqueTwoOwned = RelicType.None;
					}
					if (this.m_characterDataArray[i].TraitTwo == TraitType.RandomizeKit)
					{
						this.m_characterDataArray[i].TraitTwo = TraitType.None;
						this.m_characterDataArray[i].AntiqueTwoOwned = RelicType.None;
					}
				}
				SoulShopObj soulShopObj2 = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ChooseYourSpell);
				if (!soulShopObj2.IsNativeNull() && soulShopObj2.CurrentEquippedLevel > 0)
				{
					AbilityType soulShopSpellChosen = SaveManager.ModeSaveData.SoulShopSpellChosen;
					if (soulShopSpellChosen != AbilityType.None)
					{
						this.m_characterDataArray[i].Spell = soulShopSpellChosen;
						if (this.m_characterDataArray[i].ClassType == ClassType.MagicWandClass && CharacterCreator.GetAvailableTalents(ClassType.MagicWandClass).IndexOf(soulShopSpellChosen) != -1)
						{
							AbilityType[] availableSpells = CharacterCreator.GetAvailableSpells(ClassType.MagicWandClass);
							AbilityType abilityType = (availableSpells.Length != 0) ? availableSpells[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableSpells.Length)] : AbilityType.None;
							this.m_characterDataArray[i].Talent = abilityType;
							if (abilityType == AbilityType.None)
							{
								Debug.Log("<color=red>ERROR: Talent list was empty when attempting to change talent to something else due to locked spell.");
							}
						}
					}
				}
				if (flag)
				{
					if (!this.m_lockClassGO.activeSelf)
					{
						this.m_lockClassGO.SetActive(true);
					}
					this.m_lockClassGO.transform.SetParent(this.m_playerModels[i].transform, false);
					float num4 = 1.5f / this.m_playerModels[i].transform.lossyScale.x;
					this.m_lockClassGO.transform.localScale = new Vector3(num4, num4, num4);
					this.m_lockClassGO.transform.localPosition = new Vector3(0f, 2.25f, 1f);
				}
				else if (this.m_lockClassGO.activeSelf)
				{
					this.m_lockClassGO.SetActive(false);
				}
			}
			if (SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
			{
				EquipmentLoadout equipmentLoadout = SaveManager.EquipmentSaveData.GetEquipmentLoadout(this.m_characterDataArray[i].ClassType);
				if (equipmentLoadout != null)
				{
					equipmentLoadout.LoadLoadout(this.m_characterDataArray[i]);
				}
			}
			else
			{
				CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
				this.m_characterDataArray[i].EdgeEquipmentType = currentCharacter.EdgeEquipmentType;
				this.m_characterDataArray[i].CapeEquipmentType = currentCharacter.CapeEquipmentType;
				this.m_characterDataArray[i].HeadEquipmentType = currentCharacter.HeadEquipmentType;
				this.m_characterDataArray[i].ChestEquipmentType = currentCharacter.ChestEquipmentType;
				this.m_characterDataArray[i].TrinketEquipmentType = currentCharacter.TrinketEquipmentType;
			}
			this.m_playerModels[i].InitializeLook(this.m_characterDataArray[i]);
			Vector3 localScale = this.m_playerModels[i].transform.localScale;
			localScale.z = 0.5f;
			this.m_playerModels[i].transform.localScale = localScale;
			if (!this.m_goldFlags[i])
			{
				this.m_goldFlags[i] = UnityEngine.Object.Instantiate<LineageGoldFlagController>(this.m_goldFlagPrefab, component.transform);
				position2.y -= 1f;
				position2.x -= num3;
				this.m_goldFlags[i].transform.position = position2;
				base.StartCoroutine(this.RepositionGoldFlag(this.m_goldFlags[i].gameObject));
			}
			float goldGain = TraitManager.GetActualTraitGoldGain(this.m_characterDataArray[i].TraitOne) + TraitManager.GetActualTraitGoldGain(this.m_characterDataArray[i].TraitTwo);
			this.m_goldFlags[i].SetGoldGain(goldGain);
		}
		this.UpdateSelectedCharacter();
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x00028378 File Offset: 0x00026578
	private IEnumerator RepositionGoldFlag(GameObject goldFlag)
	{
		yield return null;
		goldFlag.transform.SetParent(this.m_goldFlagsGO.transform, true);
		Vector3 localPosition = goldFlag.transform.localPosition;
		localPosition.z = 0f;
		goldFlag.transform.localPosition = localPosition;
		goldFlag.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		yield break;
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x0002838E File Offset: 0x0002658E
	public void RunOpenTransition(float duration)
	{
		base.StartCoroutine(this.OnEnterTransitionCoroutine(duration));
	}

	// Token: 0x0600493E RID: 18750 RVA: 0x0002839E File Offset: 0x0002659E
	private IEnumerator OnEnterTransitionCoroutine(float duration)
	{
		if (!this.m_openTransitionRunning)
		{
			this.m_openTransitionRunning = true;
			AudioManager.PlayOneShot(this, this.m_enterSceneTransitionAudioEvent, default(Vector3));
			yield return TweenManager.TweenTo_UnscaledTime(Camera.main.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"position.x",
				this.m_lineageRoomController.EndingRoom.transform.position.x
			}).TweenCoroutine;
			this.m_lineageRoomController.EnableSpotlight(true);
			this.m_playerModels[this.CurrentSelectedCharacterIndex].Animator.SetBool("Victory", true);
			yield return TweenManager.TweenTo_UnscaledTime(this.m_windowCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			this.AddInputListeners();
			this.m_openTransitionRunning = false;
			this.m_openTransitionComplete = true;
		}
		yield break;
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x0011C448 File Offset: 0x0011A648
	protected override void OnOpen()
	{
		this.m_switchingWeapons = false;
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		int previousMasterSeed = SaveManager.PlayerSaveData.PreviousMasterSeed;
		int masterSeed = SaveManager.PlayerSaveData.MasterSeed;
		RNGSeedManager.SetMasterSeedOverride(Environment.TickCount);
		if (!AspectRatioManager.ForceEnable_16_9 && !AspectRatioManager.IsScreen_16_9_AspectRatio && AspectRatioManager.CurrentScreenAspectRatio < 1.7777778f)
		{
			AspectRatioManager.ForceEnable_16_9 = true;
			Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.ResolutionChanged, this, null);
		}
		RNGManager.Reset();
		this.m_storedSeed = RNGManager.GetSeed(RngID.Lineage);
		if (SaveManager.PlayerSaveData.LineageSeed == -1)
		{
			SaveManager.PlayerSaveData.LineageSeed = RNGManager.GetSeed(RngID.Lineage);
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		}
		else
		{
			SaveManager.PlayerSaveData.PreviousMasterSeed = previousMasterSeed;
			SaveManager.PlayerSaveData.MasterSeed = masterSeed;
			RNGManager.SetSeed(RngID.Lineage, SaveManager.PlayerSaveData.LineageSeed);
			for (int i = 0; i < SaveManager.PlayerSaveData.TimesRolledLineage; i++)
			{
				this.CreateRandomCharacters();
			}
		}
		this.CreateRandomCharacters();
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_selectedCharacterIndex = 0;
		this.m_currentPortraitRoomIndex = 0;
		this.m_isViewingPortraits = false;
		this.m_numRerolls = SkillTreeManager.GetSkillObjLevel(SkillTreeType.Randomize_Children);
		this.m_numRerolls -= SaveManager.PlayerSaveData.TimesRolledLineage;
		this.UpdateRerollHeirsNav();
		this.UpdateSelectedCharacter();
		this.m_windowCanvasGroup.alpha = 0f;
		this.m_lineageRoomController.EnableSpotlight(false);
		this.m_playerModels[this.CurrentSelectedCharacterIndex].Animator.SetBool("Victory", false);
		this.m_playerModels[this.CurrentSelectedCharacterIndex].Animator.Play("Idle", 0, 0f);
		TweenManager.RunFunction(0.5f, this, "RunOpenTransition", new object[]
		{
			1
		});
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.EnterLineageScreen, this, EventArgs.Empty);
	}

	// Token: 0x06004940 RID: 18752 RVA: 0x0011C614 File Offset: 0x0011A814
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_openTransitionRunning = false;
		this.m_openTransitionComplete = false;
		if (AspectRatioManager.ForceEnable_16_9)
		{
			AspectRatioManager.ForceEnable_16_9 = false;
			Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.ResolutionChanged, this, null);
		}
		RNGManager.SetSeed(RngID.Lineage, this.m_storedSeed);
		this.RemoveInputListeners();
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x0011C678 File Offset: 0x0011A878
	private void AddInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmHeirButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onViewLineageButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onRandomizeChildrenPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalDirectionPressed, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalDirectionPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalButtonReleased, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustReleased, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalButtonPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
			this.m_inputListenersAdded = true;
		}
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x0011C78C File Offset: 0x0011A98C
	private void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmHeirButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onViewLineageButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onRandomizeChildrenPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalDirectionPressed, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalDirectionPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalButtonReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalButtonReleased, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustReleased, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Vertical");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalButtonPressed, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Vertical");
			this.m_inputListenersAdded = false;
		}
	}

	// Token: 0x06004943 RID: 18755 RVA: 0x000283B4 File Offset: 0x000265B4
	protected override void OnFocus()
	{
		if (this.m_openTransitionComplete && !this.m_inputListenersAdded)
		{
			this.AddInputListeners();
		}
	}

	// Token: 0x06004944 RID: 18756 RVA: 0x000283CC File Offset: 0x000265CC
	protected override void OnLostFocus()
	{
		if (this.m_openTransitionComplete && this.m_inputListenersAdded)
		{
			this.RemoveInputListeners();
		}
	}

	// Token: 0x06004945 RID: 18757 RVA: 0x0011C8A0 File Offset: 0x0011AAA0
	private void UpdateRerollHeirsNav()
	{
		if (this.m_numRerolls > 0)
		{
			if (!this.m_rerollHeirsNavObj.gameObject.activeSelf)
			{
				this.m_rerollHeirsNavObj.gameObject.SetActive(true);
			}
			this.m_rerollHeirsText.text = string.Format(LocalizationManager.GetString("LOC_ID_LINEAGE_REROLL_HEIRS_1", false, false), this.m_numRerolls);
			return;
		}
		if (this.m_rerollHeirsNavObj.gameObject.activeSelf)
		{
			this.m_rerollHeirsNavObj.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x0011C924 File Offset: 0x0011AB24
	private void OnRandomizeChildrenPressed(InputActionEventData data)
	{
		if (!this.m_isViewingPortraits && this.m_numRerolls > 0)
		{
			this.m_numRerolls--;
			AudioManager.PlayOneShot(this, this.m_rerollAudioEvent, default(Vector3));
			AmbientSoundController.Instance.StopOnTransition = false;
			SceneLoader_RL.RunTransitionWithLogic(delegate()
			{
				this.CreateRandomCharacters();
			}, TransitionID.QuickSwipe, false);
			AmbientSoundController.Instance.StopOnTransition = true;
			SaveManager.PlayerSaveData.TimesRolledLineage++;
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
			this.UpdateRerollHeirsNav();
		}
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x000283E4 File Offset: 0x000265E4
	private void OnCancelButtonDown(InputActionEventData data)
	{
		if (this.m_isViewingPortraits)
		{
			if (!this.m_isShiftingPortraits)
			{
				this.TogglePortraitViewing();
				return;
			}
		}
		else
		{
			this.InitializeExitConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x0011C9B0 File Offset: 0x0011ABB0
	private void OnHorizontalDirectionPressed(InputActionEventData data)
	{
		bool flag = data.GetAxis() > 0f;
		if (!this.m_isViewingPortraits)
		{
			if (data.GetButtonDown() || data.GetNegativeButtonDown())
			{
				int selectedCharacterIndex = this.m_selectedCharacterIndex;
				if (flag)
				{
					this.m_selectedCharacterIndex++;
				}
				else
				{
					this.m_selectedCharacterIndex--;
				}
				if (this.m_selectedCharacterIndex > this.NumberOfSuccessors - 1)
				{
					this.m_selectedCharacterIndex = 0;
				}
				else if (this.m_selectedCharacterIndex < 0)
				{
					this.m_selectedCharacterIndex = this.NumberOfSuccessors - 1;
				}
				if (selectedCharacterIndex != this.m_selectedCharacterIndex)
				{
					this.UpdateSelectedCharacter();
					AudioManager.PlayOneShot(this, this.m_heirSelectionChangeAudioEvent, default(Vector3));
					return;
				}
			}
		}
		else if (!this.m_isShiftingPortraits)
		{
			this.ShiftPortraits(flag);
		}
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x0002840B File Offset: 0x0002660B
	private void OnHorizontalButtonReleased(InputActionEventData data)
	{
		this.m_portraitsViewed = 0;
	}

	// Token: 0x0600494A RID: 18762 RVA: 0x0011CA78 File Offset: 0x0011AC78
	private void OnVerticalButtonPressed(InputActionEventData data)
	{
		if (this.m_isViewingPortraits)
		{
			return;
		}
		if (this.m_switchingWeapons)
		{
			return;
		}
		CharacterData characterData = this.m_characterDataArray[this.m_selectedCharacterIndex];
		if (this.IsVariantAbilityUnlocked(characterData))
		{
			ClassType classType = characterData.ClassType;
			if (classType <= ClassType.MagicWandClass)
			{
				if (classType != ClassType.SwordClass)
				{
					if (classType != ClassType.AxeClass)
					{
						if (classType == ClassType.MagicWandClass)
						{
							if (characterData.Weapon != AbilityType.ScytheWeapon)
							{
								characterData.Weapon = AbilityType.ScytheWeapon;
							}
							else
							{
								characterData.Weapon = AbilityType.MagicWandWeapon;
							}
						}
					}
					else if (characterData.Weapon != AbilityType.AxeSpinnerWeapon)
					{
						characterData.Weapon = AbilityType.AxeSpinnerWeapon;
					}
					else
					{
						characterData.Weapon = AbilityType.AxeWeapon;
					}
				}
				else if (characterData.Weapon != AbilityType.ChakramWeapon)
				{
					characterData.Weapon = AbilityType.ChakramWeapon;
				}
				else
				{
					characterData.Weapon = AbilityType.SwordWeapon;
				}
			}
			else if (classType <= ClassType.LadleClass)
			{
				if (classType != ClassType.BowClass)
				{
					if (classType == ClassType.LadleClass)
					{
						if (characterData.Weapon != AbilityType.SpoonsWeapon)
						{
							characterData.Weapon = AbilityType.SpoonsWeapon;
						}
						else
						{
							characterData.Weapon = AbilityType.FryingPanWeapon;
						}
					}
				}
				else if (characterData.Weapon != AbilityType.GroundBowWeapon)
				{
					characterData.Weapon = AbilityType.GroundBowWeapon;
				}
				else
				{
					characterData.Weapon = AbilityType.BowWeapon;
				}
			}
			else if (classType != ClassType.BoxingGloveClass)
			{
				if (classType == ClassType.LuteClass)
				{
					if (characterData.Weapon != AbilityType.KineticBowWeapon)
					{
						characterData.Weapon = AbilityType.KineticBowWeapon;
					}
					else
					{
						characterData.Weapon = AbilityType.LuteWeapon;
					}
				}
			}
			else if (characterData.Weapon != AbilityType.ExplosiveHandsWeapon)
			{
				characterData.Weapon = AbilityType.ExplosiveHandsWeapon;
			}
			else
			{
				characterData.Weapon = AbilityType.BoxingGloveWeapon;
			}
			this.m_playerModels[this.m_selectedCharacterIndex].InitializeLook(this.m_characterDataArray[this.m_selectedCharacterIndex]);
			this.UpdateSelectedCharacter();
			base.StartCoroutine(this.SwitchWeaponAnimCoroutine(this.m_playerModels[this.m_selectedCharacterIndex]));
		}
	}

	// Token: 0x0600494B RID: 18763 RVA: 0x00028414 File Offset: 0x00026614
	private IEnumerator SwitchWeaponAnimCoroutine(PlayerLookController lookController)
	{
		this.m_switchingWeapons = true;
		RewiredMapController.SetCurrentMapEnabled(false);
		if (LineageWindowController.m_matBlock == null)
		{
			LineageWindowController.m_matBlock = new MaterialPropertyBlock();
		}
		Component component = EffectManager.PlayEffect(lookController.gameObject, lookController.Animator, "LineageScreenWeaponSwap_Effect", lookController.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		float num = lookController.transform.localScale.x / 1.4f;
		component.transform.localScale = new Vector3(num, num, num);
		AudioManager.PlayOneShot(this, this.m_weaponChangeAudioEvent, default(Vector3));
		if (lookController.CurrentWeaponGeo)
		{
			lookController.CurrentWeaponGeo.GetPropertyBlock(LineageWindowController.m_matBlock);
			Color storedColor = LineageWindowController.m_matBlock.GetColor(ShaderID_RL._AddColor);
			LineageWindowController.m_matBlock.SetColor(ShaderID_RL._AddColor, new Color(1f, 1f, 1f, 1f));
			lookController.CurrentWeaponGeo.SetPropertyBlock(LineageWindowController.m_matBlock);
			float delay = Time.unscaledTime + 0.05f;
			while (Time.unscaledTime < delay)
			{
				yield return null;
			}
			lookController.CurrentWeaponGeo.GetPropertyBlock(LineageWindowController.m_matBlock);
			LineageWindowController.m_matBlock.SetColor(ShaderID_RL._AddColor, storedColor);
			lookController.CurrentWeaponGeo.SetPropertyBlock(LineageWindowController.m_matBlock);
			storedColor = default(Color);
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		this.m_switchingWeapons = false;
		yield break;
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x0011CC30 File Offset: 0x0011AE30
	private bool IsVariantAbilityUnlocked(CharacterData charData)
	{
		if (charData.TraitOne == TraitType.RandomizeKit || charData.TraitTwo == TraitType.RandomizeKit)
		{
			return false;
		}
		ClassType classType = charData.ClassType;
		SoulShopObj soulShopObj = null;
		bool flag = false;
		if (classType <= ClassType.MagicWandClass)
		{
			if (classType != ClassType.SwordClass)
			{
				if (classType != ClassType.AxeClass)
				{
					if (classType == ClassType.MagicWandClass)
					{
						soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.WandVariant);
						flag = true;
					}
				}
				else
				{
					soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.AxeVariant);
					flag = true;
				}
			}
			else
			{
				soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.SwordVariant);
				flag = true;
			}
		}
		else if (classType <= ClassType.LadleClass)
		{
			if (classType != ClassType.BowClass)
			{
				if (classType == ClassType.LadleClass)
				{
					soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.LadleVariant);
					flag = true;
				}
			}
			else
			{
				soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ArcherVariant);
				flag = true;
			}
		}
		else if (classType != ClassType.BoxingGloveClass)
		{
			if (classType == ClassType.LuteClass)
			{
				soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.LuteVariant);
				flag = true;
			}
		}
		else
		{
			soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.BoxerVariant);
			flag = true;
		}
		return flag && !soulShopObj.IsNativeNull() && soulShopObj.CurrentEquippedLevel > 0;
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x0011CD54 File Offset: 0x0011AF54
	private void OnConfirmHeirButtonPressed(InputActionEventData data)
	{
		if (this.m_isViewingPortraits)
		{
			this.TogglePortraitViewing();
			return;
		}
		this.InitializeSelectHeirConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		AudioManager.PlayOneShot(this, this.m_heirChosenAudioEvent, default(Vector3));
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x0011CD94 File Offset: 0x0011AF94
	private void InitializeSelectHeirConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_LINEAGE_CHOOSE_HEIR_MENU_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_LINEAGE_CHOOSE_HEIR_MENU_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmHeirSelection);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x0011CE14 File Offset: 0x0011B014
	private void InitializeExitConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_LINEAGE_QUIT_MENU_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_LINEAGE_QUIT_MENU_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmQuitSelection);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x0011CE94 File Offset: 0x0011B094
	private void ConfirmHeirSelection()
	{
		AudioManager.PlayOneShot(this, this.m_transitionToTownAudioEvent, default(Vector3));
		LineageWindowController.CharacterLoadedFromLineage = true;
		CharacterData characterData = this.m_characterDataArray[this.m_selectedCharacterIndex];
		characterData.CopyEquipment(SaveManager.PlayerSaveData.CurrentCharacter);
		SaveManager.PlayerSaveData.CurrentCharacter = characterData;
		SaveManager.PlayerSaveData.IsDead = false;
		SaveManager.PlayerSaveData.LineageSeed = -1;
		SaveManager.PlayerSaveData.TimesRolledLineage = 0;
		NPCDialogueManager.PopulateNPCDialogues(true);
		TraitManager.SetTraitSeenState(characterData.TraitOne, TraitSeenState.SeenTwice, false);
		TraitManager.SetTraitSeenState(characterData.TraitTwo, TraitSeenState.SeenTwice, false);
		SaveManager.PlayerSaveData.SetSpellSeenState(characterData.Spell, true);
		if (characterData.ClassType == ClassType.MagicWandClass && AbilityType_RL.IsMageSpellOrTalent(characterData.Talent))
		{
			SaveManager.PlayerSaveData.SetSpellSeenState(characterData.Talent, true);
		}
		if (SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
		{
			EquipmentLoadout equipmentLoadout = SaveManager.EquipmentSaveData.GetEquipmentLoadout(SaveManager.PlayerSaveData.CurrentCharacter.ClassType);
			if (equipmentLoadout != null)
			{
				equipmentLoadout.LoadLoadout(SaveManager.PlayerSaveData.CurrentCharacter);
			}
			EquipmentLoadout.VerifyLoadoutWeight();
		}
		if (SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
		{
			RuneLoadout runeLoadout = SaveManager.EquipmentSaveData.GetRuneLoadout(SaveManager.PlayerSaveData.CurrentCharacter.ClassType);
			if (runeLoadout != null)
			{
				runeLoadout.LoadLoadout();
			}
		}
		SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.Lineage_ConfirmHeir, this, EventArgs.Empty);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.Hallway)
		{
			WorldBuilder.FirstBiomeOverride = BiomeType.Garden;
			SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
			return;
		}
		WorldBuilder.FirstBiomeOverride = BiomeType.None;
		SceneLoader_RL.LoadScene(SceneID.Town, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x0002842A File Offset: 0x0002662A
	private void ConfirmQuitSelection()
	{
		SceneLoader_RL.LoadScene(SceneID.MainMenu, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x00028433 File Offset: 0x00026633
	private void OnViewLineageButtonPressed(InputActionEventData data)
	{
		this.TogglePortraitViewing();
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x0002843B File Offset: 0x0002663B
	public void TogglePortraitViewing()
	{
		base.StartCoroutine(this.TogglePortraitAnimCoroutine(!this.m_isViewingPortraits));
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x00028453 File Offset: 0x00026653
	private IEnumerator TogglePortraitAnimCoroutine(bool toggleIn)
	{
		this.m_isViewingPortraits = toggleIn;
		RewiredMapController.SetCurrentMapEnabled(false);
		if (toggleIn)
		{
			TweenManager.TweenTo(this.m_bannerAndDescripCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			this.m_lineageRoomController.EnableSpotlight(false);
			this.m_playerModels[this.CurrentSelectedCharacterIndex].Animator.SetBool("Victory", false);
			this.m_currentPortraitRoomIndex = LineageRoomController.NumPortraits - 1;
			LineagePortrait portraitAtIndex = this.m_lineageRoomController.GetPortraitAtIndex(this.m_currentPortraitRoomIndex);
			LineageRoomController.ResetRoomIndex(this.m_lineageRoomController, true);
			AudioManager.PlayOneShot(this, this.m_viewPortraitsAudioEvent, default(Vector3));
			yield return TweenManager.TweenTo(Camera.main.transform, 0.5f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"position.x",
				portraitAtIndex.transform.position.x + this.m_portraitXOffset
			}).TweenCoroutine;
		}
		else
		{
			AudioManager.PlayOneShot(this, this.m_portraitEndAudioEvent, default(Vector3));
			float duration = Mathf.Clamp((float)(LineageRoomController.NumPortraits - this.m_currentPortraitRoomIndex) * 0.05f, 0.5f, float.MaxValue);
			yield return TweenManager.TweenTo(Camera.main.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"position.x",
				this.m_lineageRoomController.EndingRoom.transform.position.x
			}).TweenCoroutine;
			TweenManager.TweenTo(this.m_bannerAndDescripCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			this.m_lineageRoomController.EnableSpotlight(true);
			this.m_playerModels[this.CurrentSelectedCharacterIndex].Animator.SetBool("Victory", true);
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x0011D010 File Offset: 0x0011B210
	private void ShiftPortraits(bool moveRight)
	{
		int currentPortraitRoomIndex = this.m_currentPortraitRoomIndex;
		if (moveRight)
		{
			this.m_currentPortraitRoomIndex++;
		}
		else
		{
			this.m_currentPortraitRoomIndex--;
		}
		if (this.m_currentPortraitRoomIndex > LineageRoomController.NumPortraits - 1)
		{
			this.m_currentPortraitRoomIndex = LineageRoomController.NumPortraits - 1;
		}
		if (this.m_currentPortraitRoomIndex < 0)
		{
			this.m_currentPortraitRoomIndex = 0;
		}
		if (currentPortraitRoomIndex != this.m_currentPortraitRoomIndex)
		{
			LineagePortrait portraitAtIndex = this.m_lineageRoomController.GetPortraitAtIndex(this.m_currentPortraitRoomIndex);
			if (portraitAtIndex)
			{
				base.StartCoroutine(this.ShiftToPortraitCoroutine(portraitAtIndex));
				return;
			}
			Debug.LogWarning("<color=yellow>Could not navigate to portrait at index: " + this.m_currentPortraitRoomIndex.ToString() + ". Portrait could not be found.</color>");
		}
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x00028469 File Offset: 0x00026669
	private IEnumerator ShiftToPortraitCoroutine(LineagePortrait portrait)
	{
		this.m_isShiftingPortraits = true;
		int num = 5;
		float num2 = (float)this.m_portraitsViewed / (float)num;
		float duration = Mathf.Clamp(0.5f - 0.4f * num2, 0.1f, 0.5f);
		this.m_portraitsViewed = Mathf.Clamp(this.m_portraitsViewed + 1, 0, num);
		AudioManager.PlayOneShot(this, this.m_scrollPortraitAudioEvent, default(Vector3));
		yield return TweenManager.TweenTo(Camera.main.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			portrait.transform.position.x + this.m_portraitXOffset
		}).TweenCoroutine;
		this.m_isShiftingPortraits = false;
		yield break;
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x0011D0C0 File Offset: 0x0011B2C0
	private void Update()
	{
		if (this.m_triggerObject.gameObject)
		{
			Camera camera;
			if (CameraController.IsInstantiated)
			{
				camera = CameraController.GameCamera;
			}
			else
			{
				camera = Camera.main;
			}
			this.m_triggerObject.gameObject.transform.position = camera.transform.position;
		}
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x0011D114 File Offset: 0x0011B314
	private Vector2Int GetRoomIndexByCamPosition()
	{
		float num = Mathf.Abs(Camera.main.gameObject.transform.position.x);
		float num2 = 32f;
		num -= num2;
		if (num < 0f)
		{
			num = 0f;
		}
		int x = (int)(num / num2);
		int y = Mathf.CeilToInt(num / num2);
		return new Vector2Int(x, y);
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x0011D16C File Offset: 0x0011B36C
	private void UpdateSelectedCharacter()
	{
		bool classLocked = this.m_selectedCharacterIndex == this.m_characterDataArray.Length - 1 && SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ChooseYourClass).CurrentEquippedLevel > 0;
		bool spellLocked = this.m_selectedCharacterIndex == this.m_characterDataArray.Length - 1 && SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.ChooseYourSpell).CurrentEquippedLevel > 0;
		this.m_lineageEventArgs.Initialize(this.m_characterDataArray[this.m_selectedCharacterIndex], classLocked, spellLocked);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.Lineage_SelectedNewHeir, this, this.m_lineageEventArgs);
		for (int i = 0; i < this.NumberOfSuccessors; i++)
		{
			if (i == this.m_selectedCharacterIndex)
			{
				this.m_playerModels[i].Animator.SetBool("Victory", true);
			}
			else
			{
				this.m_playerModels[i].Animator.SetBool("Victory", false);
			}
		}
		if (this.IsVariantAbilityUnlocked(this.m_characterDataArray[this.m_selectedCharacterIndex]))
		{
			if (!this.m_pressUpGO.activeSelf)
			{
				this.m_pressUpGO.SetActive(true);
			}
			this.m_pressUpGO.transform.SetParent(this.m_playerModels[this.m_selectedCharacterIndex].transform, false);
			float num = 1f / this.m_playerModels[this.m_selectedCharacterIndex].transform.lossyScale.x;
			this.m_pressUpGO.transform.localScale = new Vector3(num, num, num);
			this.m_pressUpGO.transform.localPosition = new Vector3(0f, 2.75f, 1f);
			if (!this.m_upArrowAnimating)
			{
				this.m_upArrowAnimating = true;
				base.StartCoroutine(this.AnimateUpArrowCoroutine());
				return;
			}
		}
		else if (this.m_pressUpGO.activeSelf)
		{
			this.m_pressUpGO.SetActive(false);
		}
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x0002847F File Offset: 0x0002667F
	private IEnumerator AnimateUpArrowCoroutine()
	{
		float arrowYPos = this.m_pressUpGO.transform.localPosition.y;
		for (;;)
		{
			this.m_pressUpGO.transform.SetLocalPositionY(arrowYPos + 0.05f);
			yield return TweenManager.TweenTo_UnscaledTime(this.m_pressUpGO.transform, 0.5f, new EaseDelegate(Ease.Back.EaseOut), new object[]
			{
				"localPosition.y",
				arrowYPos
			}).TweenCoroutine;
		}
		yield break;
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x0002848E File Offset: 0x0002668E
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_rerollHeirsText.text = string.Format(LocalizationManager.GetString("LOC_ID_LINEAGE_REROLL_HEIRS_1", false, false), this.m_numRerolls);
	}

	// Token: 0x04003824 RID: 14372
	private const int STARTING_NUMBER_OF_SUCCESSORS = 3;

	// Token: 0x04003825 RID: 14373
	private const float PLAYER_PLATFORM_WIDTH = 7.5f;

	// Token: 0x04003826 RID: 14374
	[SerializeField]
	private CanvasGroup m_windowCanvasGroup;

	// Token: 0x04003827 RID: 14375
	[SerializeField]
	private CanvasGroup m_bannerAndDescripCanvasGroup;

	// Token: 0x04003828 RID: 14376
	[SerializeField]
	private PlayerLookController m_playerModelPrefab;

	// Token: 0x04003829 RID: 14377
	[SerializeField]
	private LineageGoldFlagController m_goldFlagPrefab;

	// Token: 0x0400382A RID: 14378
	[SerializeField]
	private GameObject m_goldFlagsGO;

	// Token: 0x0400382B RID: 14379
	[SerializeField]
	private LineageRoomController m_lineageRoomController;

	// Token: 0x0400382C RID: 14380
	[SerializeField]
	private GameObject m_triggerObject;

	// Token: 0x0400382D RID: 14381
	[SerializeField]
	private float m_portraitXOffset;

	// Token: 0x0400382E RID: 14382
	[SerializeField]
	private GameObject m_rerollHeirsNavObj;

	// Token: 0x0400382F RID: 14383
	[SerializeField]
	private TMP_Text m_rerollHeirsText;

	// Token: 0x04003830 RID: 14384
	[SerializeField]
	private GameObject m_lockClassGO;

	// Token: 0x04003831 RID: 14385
	[SerializeField]
	private GameObject m_pressUpGO;

	// Token: 0x04003832 RID: 14386
	[SerializeField]
	[EventRef]
	private string m_enterSceneTransitionAudioEvent;

	// Token: 0x04003833 RID: 14387
	[SerializeField]
	[EventRef]
	private string m_heirSelectionChangeAudioEvent;

	// Token: 0x04003834 RID: 14388
	[SerializeField]
	[EventRef]
	private string m_heirChosenAudioEvent;

	// Token: 0x04003835 RID: 14389
	[SerializeField]
	[EventRef]
	private string m_transitionToTownAudioEvent;

	// Token: 0x04003836 RID: 14390
	[SerializeField]
	[EventRef]
	private string m_viewPortraitsAudioEvent;

	// Token: 0x04003837 RID: 14391
	[SerializeField]
	[EventRef]
	private string m_scrollPortraitAudioEvent;

	// Token: 0x04003838 RID: 14392
	[SerializeField]
	[EventRef]
	private string m_portraitEndAudioEvent;

	// Token: 0x04003839 RID: 14393
	[SerializeField]
	[EventRef]
	private string m_rerollAudioEvent;

	// Token: 0x0400383A RID: 14394
	[SerializeField]
	[EventRef]
	private string m_weaponChangeAudioEvent;

	// Token: 0x0400383B RID: 14395
	public static bool CharacterLoadedFromLineage;

	// Token: 0x0400383C RID: 14396
	private CharacterData[] m_characterDataArray;

	// Token: 0x0400383D RID: 14397
	private PlayerLookController[] m_playerModels;

	// Token: 0x0400383E RID: 14398
	private LineageGoldFlagController[] m_goldFlags;

	// Token: 0x0400383F RID: 14399
	private LineageHeirChangedEventArgs m_lineageEventArgs;

	// Token: 0x04003840 RID: 14400
	private bool m_isViewingPortraits;

	// Token: 0x04003841 RID: 14401
	private int m_selectedCharacterIndex;

	// Token: 0x04003842 RID: 14402
	private int m_currentPortraitRoomIndex;

	// Token: 0x04003843 RID: 14403
	private bool m_openTransitionRunning;

	// Token: 0x04003844 RID: 14404
	private bool m_openTransitionComplete;

	// Token: 0x04003845 RID: 14405
	private bool m_isShiftingPortraits;

	// Token: 0x04003846 RID: 14406
	private bool m_inputListenersAdded;

	// Token: 0x04003847 RID: 14407
	private int m_numRerolls;

	// Token: 0x04003848 RID: 14408
	private int m_storedSeed;

	// Token: 0x04003849 RID: 14409
	private int m_portraitsViewed;

	// Token: 0x0400384A RID: 14410
	private bool m_upArrowAnimating;

	// Token: 0x0400384B RID: 14411
	private bool m_switchingWeapons;

	// Token: 0x0400384C RID: 14412
	private Action<object, EventArgs> m_refreshText;

	// Token: 0x0400384D RID: 14413
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x0400384E RID: 14414
	private Action m_confirmHeirSelection;

	// Token: 0x0400384F RID: 14415
	private Action m_confirmQuitSelection;

	// Token: 0x04003850 RID: 14416
	private Action<InputActionEventData> m_onConfirmHeirButtonPressed;

	// Token: 0x04003851 RID: 14417
	private Action<InputActionEventData> m_onViewLineageButtonPressed;

	// Token: 0x04003852 RID: 14418
	private Action<InputActionEventData> m_onRandomizeChildrenPressed;

	// Token: 0x04003853 RID: 14419
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x04003854 RID: 14420
	private Action<InputActionEventData> m_onHorizontalDirectionPressed;

	// Token: 0x04003855 RID: 14421
	private Action<InputActionEventData> m_onHorizontalButtonReleased;

	// Token: 0x04003856 RID: 14422
	private Action<InputActionEventData> m_onVerticalButtonPressed;

	// Token: 0x04003857 RID: 14423
	private static MaterialPropertyBlock m_matBlock;
}
