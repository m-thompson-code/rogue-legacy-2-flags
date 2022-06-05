using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Rewired;
using Rewired.Integration.UnityUI;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000992 RID: 2450
public class SkillTreeWindowController : WindowController, ILocalizable
{
	// Token: 0x17001A04 RID: 6660
	// (get) Token: 0x06004B7D RID: 19325 RVA: 0x00029605 File Offset: 0x00027805
	public int SkillCount
	{
		get
		{
			return this.m_availableSkillsSet.Count;
		}
	}

	// Token: 0x17001A05 RID: 6661
	// (get) Token: 0x06004B7E RID: 19326 RVA: 0x00029612 File Offset: 0x00027812
	public HashSet<SkillTreeType> AvailableSkills
	{
		get
		{
			return this.m_availableSkillsSet;
		}
	}

	// Token: 0x17001A06 RID: 6662
	// (get) Token: 0x06004B7F RID: 19327 RVA: 0x0002961A File Offset: 0x0002781A
	public GameObject SkillTreeCastleParentObj
	{
		get
		{
			return this.m_castleParentObj;
		}
	}

	// Token: 0x17001A07 RID: 6663
	// (get) Token: 0x06004B80 RID: 19328 RVA: 0x000047A4 File Offset: 0x000029A4
	public override WindowID ID
	{
		get
		{
			return WindowID.SkillTree;
		}
	}

	// Token: 0x17001A08 RID: 6664
	// (get) Token: 0x06004B81 RID: 19329 RVA: 0x00029622 File Offset: 0x00027822
	// (set) Token: 0x06004B82 RID: 19330 RVA: 0x0002962A File Offset: 0x0002782A
	public Animator CastleAnimator
	{
		get
		{
			return this.m_castleAnimator;
		}
		private set
		{
			this.m_castleAnimator = value;
		}
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x00127E2C File Offset: 0x0012602C
	private void Awake()
	{
		this.m_onHighlightedSkillChanged = new Action<MonoBehaviour, EventArgs>(this.OnHighlightedSkillChanged);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onSkillLevelChanged = new Action<MonoBehaviour, EventArgs>(this.OnSkillLevelChanged);
		this.m_cancelResetWarning = new Action(this.CancelResetWarning);
		this.m_onCancelButtonPressed = new Action<InputActionEventData>(this.OnCancelButtonPressed);
		this.m_onPurchaseButtonPressed = new Action<InputActionEventData>(this.OnPurchaseButtonPressed);
		this.m_onToggleCastleViewPressed = new Action<InputActionEventData>(this.OnToggleCastleViewPressed);
		this.m_onPurchaseMultipleButtonPressed = new Action<InputActionEventData>(this.OnPurchaseMultipleButtonPressed);
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x00127ECC File Offset: 0x001260CC
	public override void Initialize()
	{
		this.m_standaloneInputModule = UnityEngine.Object.FindObjectOfType<RewiredStandaloneInputModule>();
		this.m_skillTreeButtonDict = new Dictionary<SkillTreeType, SkillTreeSlot>();
		List<SkillTreeSlot> list = new List<SkillTreeSlot>();
		int num = 1;
		SkillTreeSlot[] componentsInChildren = base.GetComponentsInChildren<SkillTreeSlot>();
		this.m_availableSkillsSet = new HashSet<SkillTreeType>();
		foreach (SkillTreeSlot skillTreeSlot in componentsInChildren)
		{
			skillTreeSlot.Initialize(num, this.CastleAnimator);
			num++;
			if (!skillTreeSlot.HasData)
			{
				skillTreeSlot.gameObject.SetActive(false);
			}
			else
			{
				if (this.m_skillTreeButtonDict.ContainsKey(skillTreeSlot.SkillTreeType))
				{
					throw new Exception(string.Concat(new string[]
					{
						"Duplicate SkillTreeType: ",
						skillTreeSlot.SkillTreeType.ToString(),
						" found.  Offending indices are: ",
						this.m_skillTreeButtonDict[skillTreeSlot.SkillTreeType].SlotIndex.ToString(),
						" and ",
						num.ToString()
					}));
				}
				this.m_skillTreeButtonDict.Add(skillTreeSlot.SkillTreeType, skillTreeSlot);
				if (SkillTreeManager.GetSkillObjLevel(skillTreeSlot.SkillTreeType) > 0)
				{
					skillTreeSlot.gameObject.SetActive(true);
					list.Add(skillTreeSlot);
				}
				else
				{
					skillTreeSlot.gameObject.SetActive(false);
				}
				skillTreeSlot.UpdateSkillTreeType();
				if (skillTreeSlot.SkillTreeType != SkillTreeType.None)
				{
					this.m_availableSkillsSet.Add(skillTreeSlot.SkillTreeType);
				}
			}
		}
		foreach (SkillTreeSlot skillTreeSlot2 in list)
		{
			skillTreeSlot2.UnlockConnectedSkillSlots();
		}
		this.m_shopSignStartingPos = this.m_shopSignRectTransform.anchoredPosition;
		base.Initialize();
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x00029633 File Offset: 0x00027833
	protected override void OnFocus()
	{
		this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeIcon(this.m_selectedSkillTreeButton.SkillTreeType);
		this.m_selectedSkillTreeButton.gameObject.SetActive(true);
		this.m_selectedSkillTreeButton.Select(false);
		this.AddListeners();
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x00029673 File Offset: 0x00027873
	protected override void OnLostFocus()
	{
		this.RemoveListeners();
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnPause()
	{
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnUnpause()
	{
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x00128094 File Offset: 0x00126294
	private void SetTraitsVisible(bool visible)
	{
		foreach (BaseTrait baseTrait in TraitManager.ActiveTraitList)
		{
			baseTrait.SetPaused(!visible);
			if (baseTrait.PostProcessOverrideController != null)
			{
				if (visible)
				{
					baseTrait.PostProcessOverrideController.enabled = true;
				}
				else
				{
					baseTrait.PostProcessOverrideController.enabled = false;
				}
			}
		}
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x00128114 File Offset: 0x00126314
	public void EnableGardenTransitionState(bool enableTransitionState)
	{
		this.m_transitionStateEnabled = enableTransitionState;
		foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair in this.m_skillTreeButtonDict)
		{
			keyValuePair.Value.Button.interactable = !enableTransitionState;
		}
		this.m_descriptionBoxCanvasGroup.gameObject.SetActive(!enableTransitionState);
		this.m_navigationObj.SetActive(!enableTransitionState);
		this.m_levelText.gameObject.SetActive(!enableTransitionState);
		this.m_labourCostGO.SetActive(!enableTransitionState);
		this.m_shopSignRectTransform.gameObject.SetActive(!enableTransitionState);
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x001281D8 File Offset: 0x001263D8
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.StopAllStatusEffects();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HideEnemyHUD, null, null);
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.m_affordableItemsText.text = LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_SIGN_BUY_TOPSIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_affordableItemsText.text = LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_SIGN_BUY_DOCKS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_selectedSkillTreeButton = this.m_startingHighlightedButton;
		this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeIcon(this.m_selectedSkillTreeButton.SkillTreeType);
		this.m_selectedSkillTreeButton.gameObject.SetActive(true);
		this.m_selectedSkillTreeButton.Select(false);
		this.SetTraitsVisible(false);
		SkillTreeWindowController.CastleViewEnabled = false;
		foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair in this.m_skillTreeButtonDict)
		{
			keyValuePair.Value.Button.interactable = true;
		}
		this.m_skillTreeIconsCanvasGroup.alpha = 1f;
		this.m_descriptionBoxCanvasGroup.alpha = 1f;
		if (!this.m_transitionStateEnabled)
		{
			CinemachineVirtualCamera cinemachineVirtualCamera = CameraController.CinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
			cinemachineVirtualCamera.GetComponent<CinemachineConfiner_RL>().enabled = false;
			cinemachineVirtualCamera.Follow = null;
			Vector3 position = cinemachineVirtualCamera.transform.position;
			position.x -= 50f;
			position.y += 50f;
			cinemachineVirtualCamera.transform.position = position;
		}
		else
		{
			Vector3 position2 = CameraController.GameCamera.transform.position;
			position2.x = -100f;
			position2.y += 100f;
			CameraController.GameCamera.transform.position = position2;
			RewiredMapController.SetCurrentMapEnabled(false);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.SkillTree_Opened, this, null);
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = true;
		this.RefreshAllButtons(true);
		this.m_skillTreeIconsCanvasGroup.gameObject.SetActive(!this.m_transitionStateEnabled);
		this.CastleAnimator.Update(1f);
		this.CastleAnimator.Update(1f);
		base.StartCoroutine(this.ReparentSkillTreeToSky());
		this.m_shopSignDisplayed = false;
		Vector2 shopSignStartingPos = this.m_shopSignStartingPos;
		shopSignStartingPos.y -= 500f;
		this.m_shopSignRectTransform.anchoredPosition = shopSignStartingPos;
		this.UpdateShopSign();
		if (!this.m_transitionStateEnabled)
		{
			this.UpdateLabourCosts();
		}
		this.UpdateLevelText();
		if (this.HasAllSkills())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllSkills, StoreType.All);
		}
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x0012848C File Offset: 0x0012668C
	public bool HasAllSkills()
	{
		bool result = true;
		using (HashSet<SkillTreeType>.Enumerator enumerator = this.m_availableSkillsSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (SkillTreeManager.GetSkillObjLevel(enumerator.Current) <= 0)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x001284E8 File Offset: 0x001266E8
	private void StopAllStatusEffects()
	{
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
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x00128540 File Offset: 0x00126740
	private void UpdateLabourCosts()
	{
		if (SkillTreeManager.GetTotalSkillObjLevel() > 20)
		{
			if (!this.m_labourCostGO.activeSelf)
			{
				this.m_labourCostGO.SetActive(true);
			}
			int num = Mathf.Clamp(SkillTreeManager.GetTotalSkillObjLevel() - 20, 0, int.MaxValue);
			num = (int)(Math.Floor((double)((float)num * 14f / 5f)) * 5.0);
			this.m_labourCostText.text = num.ToString();
			return;
		}
		if (this.m_labourCostGO.activeSelf)
		{
			this.m_labourCostGO.SetActive(false);
		}
	}

	// Token: 0x06004B8F RID: 19343 RVA: 0x001285D0 File Offset: 0x001267D0
	private void UpdateLevelText()
	{
		int totalSkillObjLevel = SkillTreeManager.GetTotalSkillObjLevel();
		this.m_levelText.text = string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_CHARACTER_LEVEL_1", false, false), totalSkillObjLevel.ToString());
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x00128608 File Offset: 0x00126808
	private void UpdateShopSign()
	{
		if (this.m_transitionStateEnabled)
		{
			return;
		}
		bool flag = false;
		if (SkillTreeManager.GetSkillObjLevel(SkillTreeType.Smithy) > 0)
		{
			foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
			{
				if (equipmentCategoryType != EquipmentCategoryType.None)
				{
					foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
					{
						if (equipmentType != EquipmentType.None)
						{
							if (EquipmentManager.CanPurchaseEquipment(equipmentCategoryType, equipmentType, true))
							{
								flag = true;
								break;
							}
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
		}
		if (!flag && SkillTreeManager.GetSkillObjLevel(SkillTreeType.Enchantress) > 0)
		{
			foreach (RuneType runeType in RuneType_RL.TypeArray)
			{
				if (runeType != RuneType.None && RuneManager.CanPurchaseRune(runeType, true))
				{
					flag = true;
					break;
				}
			}
		}
		TweenManager.StopAllTweensContaining(this.m_shopSignRectTransform, false);
		if (flag && !this.m_shopSignDisplayed)
		{
			this.m_shopSignDisplayed = true;
			Vector2 shopSignStartingPos = this.m_shopSignStartingPos;
			shopSignStartingPos.y -= 500f;
			this.m_shopSignRectTransform.anchoredPosition = shopSignStartingPos;
			TweenManager.TweenTo(this.m_shopSignRectTransform, 0.25f, new EaseDelegate(Ease.Back.EaseOutSmall), new object[]
			{
				"anchoredPosition.y",
				this.m_shopSignStartingPos.y
			});
			return;
		}
		if (!flag && this.m_shopSignDisplayed)
		{
			this.m_shopSignDisplayed = false;
			Vector2 shopSignStartingPos2 = this.m_shopSignStartingPos;
			this.m_shopSignRectTransform.anchoredPosition = shopSignStartingPos2;
			TweenManager.TweenTo(this.m_shopSignRectTransform, 0.25f, new EaseDelegate(Ease.Back.EaseInSmall), new object[]
			{
				"anchoredPosition.y",
				this.m_shopSignStartingPos.y - 500f
			});
		}
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x0002967B File Offset: 0x0002787B
	private IEnumerator ReparentSkillTreeToSky()
	{
		this.m_skyIsReparenting = true;
		float startTime = Time.time;
		while (Time.time < startTime + 0.5f)
		{
			yield return null;
		}
		Sky sky = UnityEngine.Object.FindObjectOfType<Sky>();
		if (sky)
		{
			this.CastleAnimator.gameObject.transform.SetParent(sky.transform, true);
			Vector3 localScale = this.CastleAnimator.gameObject.transform.localScale;
			localScale.z = 1f;
			this.CastleAnimator.gameObject.transform.localScale = localScale;
			Vector3 localPosition = this.CastleAnimator.gameObject.transform.localPosition;
			localPosition.z += 1f;
			this.CastleAnimator.gameObject.transform.localPosition = localPosition;
		}
		else
		{
			Debug.Log("Failed to parent skilltree to sky.");
		}
		this.m_skyIsReparenting = false;
		yield break;
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x001287A4 File Offset: 0x001269A4
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.SetTraitsVisible(true);
		if (!this.m_transitionStateEnabled)
		{
			CinemachineVirtualCamera cinemachineVirtualCamera = CameraController.CinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
			cinemachineVirtualCamera.GetComponent<CinemachineConfiner_RL>().enabled = true;
			cinemachineVirtualCamera.Follow = PlayerManager.GetPlayerController().FollowTargetGO.transform;
		}
		this.CastleAnimator.gameObject.transform.localPosition = Vector3.zero;
		this.CastleAnimator.gameObject.transform.SetParent(this.SkillTreeCastleParentObj.transform, false);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.SkillTree_Closed, this, null);
		this.m_windowCanvas.gameObject.SetActive(false);
		if (this.m_currentAnimCoroutine != null)
		{
			base.StopCoroutine(this.m_currentAnimCoroutine);
		}
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = false;
		SaveManager.PlayerSaveData.UpdateCachedData();
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x00128878 File Offset: 0x00126A78
	private void AddListeners()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onPurchaseButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onToggleCastleViewPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onPurchaseMultipleButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x00128900 File Offset: 0x00126B00
	private void RemoveListeners()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onPurchaseButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onToggleCastleViewPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onPurchaseMultipleButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x00128988 File Offset: 0x00126B88
	private void OnHighlightedSkillChanged(MonoBehaviour sender, EventArgs args)
	{
		HighlightedSkillChangedEventArgs highlightedSkillChangedEventArgs = args as HighlightedSkillChangedEventArgs;
		if (highlightedSkillChangedEventArgs.SkillTreeType != SkillTreeType.None)
		{
			SkillTreeSlot skillTreeSlot = this.m_skillTreeButtonDict[highlightedSkillChangedEventArgs.SkillTreeType];
			if (this.m_selectedSkillTreeButton != skillTreeSlot)
			{
				this.m_selectedSkillTreeButton = skillTreeSlot;
				SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(highlightedSkillChangedEventArgs.SkillTreeType);
				if (skillTreeObj.IsSoulLocked)
				{
					this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeSoulLockedIcon();
					return;
				}
				if (skillTreeObj.IsLocked)
				{
					this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeLockedIcon();
					return;
				}
				this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeIcon(highlightedSkillChangedEventArgs.SkillTreeType);
			}
		}
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x00128A20 File Offset: 0x00126C20
	private void OnToggleCastleViewPressed(InputActionEventData eventData)
	{
		SkillTreeWindowController.CastleViewEnabled = !SkillTreeWindowController.CastleViewEnabled;
		if (!SkillTreeWindowController.CastleViewEnabled)
		{
			foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair in this.m_skillTreeButtonDict)
			{
				keyValuePair.Value.Button.interactable = true;
			}
			this.m_skillTreeIconsCanvasGroup.alpha = 1f;
			this.m_descriptionBoxCanvasGroup.alpha = 1f;
			this.m_selectedSkillTreeButton.Select(false);
			return;
		}
		foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair2 in this.m_skillTreeButtonDict)
		{
			keyValuePair2.Value.Button.interactable = false;
		}
		this.m_skillTreeIconsCanvasGroup.alpha = 0f;
		this.m_descriptionBoxCanvasGroup.alpha = 0f;
	}

	// Token: 0x06004B97 RID: 19351 RVA: 0x0002968A File Offset: 0x0002788A
	private void OnPurchaseButtonPressed(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (SkillTreeWindowController.CastleViewEnabled)
		{
			return;
		}
		this.m_selectedSkillTreeButton.PurchaseSkillUpgrade(1);
	}

	// Token: 0x06004B98 RID: 19352 RVA: 0x000296AB File Offset: 0x000278AB
	private void OnPurchaseMultipleButtonPressed(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (SkillTreeWindowController.CastleViewEnabled)
		{
			return;
		}
		this.m_selectedSkillTreeButton.PurchaseSkillUpgrade(5);
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x00128B2C File Offset: 0x00126D2C
	private void OnSkillLevelChanged(object sender, EventArgs args)
	{
		if (args == null)
		{
			return;
		}
		this.UpdateShopSign();
		this.UpdateLabourCosts();
		this.UpdateLevelText();
		SkillLevelChangedEventArgs skillLevelChangedEventArgs = args as SkillLevelChangedEventArgs;
		if (skillLevelChangedEventArgs.PrevLevel < skillLevelChangedEventArgs.NewLevel)
		{
			SkillTreeSlot skillTreeSlot = this.m_skillTreeButtonDict[skillLevelChangedEventArgs.SkillTreeType];
			skillTreeSlot.UnlockConnectedSkillSlots();
			this.RefreshAllButtons(false);
			if (!skillTreeSlot.HasAnimated)
			{
				if (SkillTreePopupLibrary.GetPopupData(skillLevelChangedEventArgs.SkillTreeType) != null)
				{
					if (this.m_currentAnimCoroutine != null)
					{
						base.StopCoroutine(this.m_currentAnimCoroutine);
					}
					this.m_currentAnimCoroutine = base.StartCoroutine(this.DisplaySkillTreePopUp(skillLevelChangedEventArgs.SkillTreeType, skillTreeSlot));
					return;
				}
				if (this.m_currentAnimCoroutine != null)
				{
					base.StopCoroutine(this.m_currentAnimCoroutine);
				}
				this.m_currentAnimCoroutine = base.StartCoroutine(this.AnimateBGImageCoroutine(skillTreeSlot));
				this.RunPurchaseAnim(skillTreeSlot);
				return;
			}
			else
			{
				this.RunPurchaseAnim(skillTreeSlot);
				base.StartCoroutine(this.UnlockLabourCostAnimCoroutine());
			}
		}
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x00128C0C File Offset: 0x00126E0C
	private void RunPurchaseAnim(SkillTreeSlot slot)
	{
		BaseEffect baseEffect = EffectManager.PlayEffect(slot.gameObject, null, "Purchase_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.transform.SetParent(slot.transform, false);
		baseEffect.transform.SetParent(null, true);
		if (this.m_purchaseTween != null)
		{
			this.m_purchaseTween.StopTweenWithConditionChecks(false, slot.transform, null);
		}
		Vector3 storedScale = slot.StoredScale;
		slot.transform.localScale = storedScale + new Vector3(0.1f, 0.1f, 0.1f);
		this.m_purchaseTween = TweenManager.TweenTo_UnscaledTime(slot.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			storedScale.x,
			"localScale.y",
			storedScale.y,
			"localScale.z",
			storedScale.z
		});
	}

	// Token: 0x06004B9B RID: 19355 RVA: 0x000296CC File Offset: 0x000278CC
	private IEnumerator AnimateBGImageCoroutine(SkillTreeSlot changedSkillSlot)
	{
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = false;
		if (!this.m_transitionStateEnabled)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
		}
		this.m_standaloneInputModule.allowMouseInput = false;
		if (changedSkillSlot.HasAnimParam)
		{
			yield return TweenManager.TweenTo(this.m_skillTreeIconsCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
		}
		yield return changedSkillSlot.AnimateBGImage();
		if (!this.m_transitionStateEnabled)
		{
			RewiredMapController.SetCurrentMapEnabled(true);
		}
		this.m_standaloneInputModule.allowMouseInput = true;
		if (changedSkillSlot.HasAnimParam)
		{
			yield return TweenManager.TweenTo(this.m_skillTreeIconsCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
		}
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = true;
		yield return this.UnlockLabourCostAnimCoroutine();
		yield break;
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x000296E2 File Offset: 0x000278E2
	private void OnCancelButtonPressed(InputActionEventData eventData)
	{
		if (!this.m_skyIsReparenting)
		{
			WindowManager.SetWindowIsOpen(WindowID.SkillTree, false, TransitionID.QuickSwipe);
		}
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x000296F4 File Offset: 0x000278F4
	private IEnumerator DisplaySkillTreePopUp(SkillTreeType skillTreeType, SkillTreeSlot changedSkillSlot)
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTreePopUp))
		{
			WindowManager.LoadWindow(WindowID.SkillTreePopUp);
		}
		if (!this.m_transitionStateEnabled)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
		}
		this.m_standaloneInputModule.allowMouseInput = false;
		this.RunPurchaseAnim(changedSkillSlot);
		float startTime = Time.time;
		while (Time.time < startTime + 0.5f)
		{
			yield return null;
		}
		this.m_standaloneInputModule.allowMouseInput = true;
		(WindowManager.GetWindowController(WindowID.SkillTreePopUp) as SkillTreePopUpWindowController).SetPopupType(skillTreeType);
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, true);
		while (WindowManager.ActiveWindow != this)
		{
			yield return null;
		}
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = false;
		if (!this.m_transitionStateEnabled)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
		}
		this.m_standaloneInputModule.allowMouseInput = false;
		if (changedSkillSlot.HasAnimParam)
		{
			yield return TweenManager.TweenTo(this.m_skillTreeIconsCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
		}
		yield return changedSkillSlot.AnimateBGImage();
		if (!this.m_transitionStateEnabled)
		{
			RewiredMapController.SetCurrentMapEnabled(true);
		}
		this.m_standaloneInputModule.allowMouseInput = true;
		if (changedSkillSlot.HasAnimParam)
		{
			yield return TweenManager.TweenTo(this.m_skillTreeIconsCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
		}
		EffectTriggerAnimBehaviour.DISABLE_GLOBALLY = true;
		yield return this.UnlockLabourCostAnimCoroutine();
		yield break;
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x00029711 File Offset: 0x00027911
	private IEnumerator UnlockLabourCostAnimCoroutine()
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.LabourCostsUnlocked) && SkillTreeManager.GetTotalSkillObjLevel() > 20)
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.LabourCostsUnlocked, true);
			if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTreePopUp))
			{
				WindowManager.LoadWindow(WindowID.SkillTreePopUp);
			}
			(WindowManager.GetWindowController(WindowID.SkillTreePopUp) as SkillTreePopUpWindowController).SetPopupType(SkillTreeType.LabourCosts_Unlocked);
			WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, true);
			while (WindowManager.ActiveWindow != this)
			{
				yield return null;
			}
		}
		this.UpdateLabourCosts();
		yield break;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x00128D0C File Offset: 0x00126F0C
	private void RunFarShoresResetWarning()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PlayFarShoresWarning))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PlayFarShoresWarning, false);
			if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
			{
				WindowManager.LoadWindow(WindowID.ConfirmMenu);
			}
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetButtonDelayTime(3f);
			confirmMenuWindowController.SetTitleText("Far Shores Content Reset", false);
			confirmMenuWindowController.SetDescriptionText("Our very first content release has arrived, and with it a ton of changes.  To keep things in check, all skills and gear have been reset. However, the gold spent has been refunded, and the discovery of all gear is preserved.\n\n<size=125%>SPEND YOUR GOLD!!!</size>", false);
			confirmMenuWindowController.SetNumberOfButtons(1);
			confirmMenuWindowController.SetOnCancelAction(this.m_cancelResetWarning);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("Aww nuts...", false);
			buttonAtIndex.SetOnClickAction(this.m_cancelResetWarning);
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x00128DB4 File Offset: 0x00126FB4
	private void RunArcaneHallowsResetWarning()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PlayArcaneHallowsWarning))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PlayArcaneHallowsWarning, false);
			if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
			{
				WindowManager.LoadWindow(WindowID.ConfirmMenu);
			}
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetButtonDelayTime(3f);
			confirmMenuWindowController.SetTitleText("Arcane Hallows Content Reset", false);
			confirmMenuWindowController.SetDescriptionText("The second content release is out, and once again the changes are massive.\n\nTo keep things in check, all skills have been reset. But fret not!  All gold spent on skills has also been refunded.\n\nEverything else has been preserved.", false);
			confirmMenuWindowController.SetNumberOfButtons(1);
			confirmMenuWindowController.SetOnCancelAction(this.m_cancelResetWarning);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("SPEND YOUR GOLD!!!", false);
			buttonAtIndex.SetOnClickAction(this.m_cancelResetWarning);
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x00128E5C File Offset: 0x0012705C
	private void RunDriftingWorldsResetWarning()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PlayDriftingWorldsWarning))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PlayDriftingWorldsWarning, false);
			if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
			{
				WindowManager.LoadWindow(WindowID.ConfirmMenu);
			}
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetButtonDelayTime(3f);
			confirmMenuWindowController.SetTitleText("Drifting Worlds Content Reset", false);
			confirmMenuWindowController.SetDescriptionText("A new content patch is out, which means once again equipped runes, gear, and all skills are reset. The majority of gold spent has been refunded, and the discovery of all gear and runes is preserved.", false);
			confirmMenuWindowController.SetNumberOfButtons(1);
			confirmMenuWindowController.SetOnCancelAction(this.m_cancelResetWarning);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("SPEND YOUR GOLD!!!", false);
			buttonAtIndex.SetOnClickAction(this.m_cancelResetWarning);
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x00128F04 File Offset: 0x00127104
	private void RunPizzaMundiResetWarning()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PlayPizzaMundiWarning))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PlayPizzaMundiWarning, false);
			if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
			{
				WindowManager.LoadWindow(WindowID.ConfirmMenu);
			}
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetButtonDelayTime(3f);
			confirmMenuWindowController.SetTitleText("Pizza Mundi Content Reset", false);
			confirmMenuWindowController.SetDescriptionText("!!! IMPORTANT !!! \nLeveling up Runes and Gear must now be unlocked in the new Soul Shop (this was our original vision for the game).\n\nALL EQUIPMENT HAS BEEN UNEQUIPPED BUT NOT LOST.", false);
			confirmMenuWindowController.SetNumberOfButtons(1);
			confirmMenuWindowController.SetOnCancelAction(this.m_cancelResetWarning);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("JUST GIVE ME PIZZA!", false);
			buttonAtIndex.SetOnClickAction(this.m_cancelResetWarning);
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x00128FAC File Offset: 0x001271AC
	private void RunDragonsVowResetWarning()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PlayDragonsVowWarning))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PlayDragonsVowWarning, false);
			if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
			{
				WindowManager.LoadWindow(WindowID.ConfirmMenu);
			}
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetButtonDelayTime(5f);
			confirmMenuWindowController.SetTitleText("Dragon's Vow Content Reset", false);
			confirmMenuWindowController.SetDescriptionText("This is a BIG balance patch, so all Gear and Runes have been unequipped.\n\nThe Skill Tree and Soul Shop have also been reset, with their resources refunded.\n\nDon't forget!  You can now purchase 5 skills at a time in the Skill Tree.", false);
			confirmMenuWindowController.SetNumberOfButtons(1);
			confirmMenuWindowController.SetOnCancelAction(this.m_cancelResetWarning);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("START EXPLORING!", false);
			buttonAtIndex.SetOnClickAction(this.m_cancelResetWarning);
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelResetWarning()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x00129054 File Offset: 0x00127254
	private void RefreshAllButtons(bool updateAnimParams)
	{
		foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair in this.m_skillTreeButtonDict)
		{
			if (keyValuePair.Value.isActiveAndEnabled && keyValuePair.Value.HasData)
			{
				keyValuePair.Value.UpdateSkillTreeType();
				keyValuePair.Value.RefreshSlotState(updateAnimParams);
				keyValuePair.Value.UpdateNavigationNodes();
			}
		}
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x001290E4 File Offset: 0x001272E4
	public void ForceUpdateSkillTreeAnimatorParams()
	{
		foreach (KeyValuePair<SkillTreeType, SkillTreeSlot> keyValuePair in this.m_skillTreeButtonDict)
		{
			if (keyValuePair.Value.HasData)
			{
				keyValuePair.Value.UpdateAnimatorParams();
			}
		}
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x0012914C File Offset: 0x0012734C
	public void RefreshText(object sender, EventArgs args)
	{
		this.UpdateLevelText();
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.m_affordableItemsText.text = LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_SIGN_BUY_TOPSIDE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		this.m_affordableItemsText.text = LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_SIGN_BUY_DOCKS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x040039B1 RID: 14769
	public const int MULTIPLE_PURCHASE_COUNT = 5;

	// Token: 0x040039B2 RID: 14770
	[SerializeField]
	private SkillTreeSlot m_startingHighlightedButton;

	// Token: 0x040039B3 RID: 14771
	[SerializeField]
	private Image m_infoPlateIcon;

	// Token: 0x040039B4 RID: 14772
	[SerializeField]
	private GameObject m_castleParentObj;

	// Token: 0x040039B5 RID: 14773
	[SerializeField]
	private Animator m_castleAnimator;

	// Token: 0x040039B6 RID: 14774
	[SerializeField]
	private CanvasGroup m_skillTreeIconsCanvasGroup;

	// Token: 0x040039B7 RID: 14775
	[SerializeField]
	private CanvasGroup m_descriptionBoxCanvasGroup;

	// Token: 0x040039B8 RID: 14776
	[SerializeField]
	private RectTransform m_shopSignRectTransform;

	// Token: 0x040039B9 RID: 14777
	[SerializeField]
	private GameObject m_labourCostGO;

	// Token: 0x040039BA RID: 14778
	[SerializeField]
	private TMP_Text m_labourCostText;

	// Token: 0x040039BB RID: 14779
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040039BC RID: 14780
	[SerializeField]
	private TMP_Text m_affordableItemsText;

	// Token: 0x040039BD RID: 14781
	[SerializeField]
	private GameObject m_navigationObj;

	// Token: 0x040039BE RID: 14782
	private SkillTreeSlot m_selectedSkillTreeButton;

	// Token: 0x040039BF RID: 14783
	private Dictionary<SkillTreeType, SkillTreeSlot> m_skillTreeButtonDict;

	// Token: 0x040039C0 RID: 14784
	private RewiredStandaloneInputModule m_standaloneInputModule;

	// Token: 0x040039C1 RID: 14785
	private Vector2 m_shopSignStartingPos;

	// Token: 0x040039C2 RID: 14786
	private bool m_shopSignDisplayed;

	// Token: 0x040039C3 RID: 14787
	private Coroutine m_currentAnimCoroutine;

	// Token: 0x040039C4 RID: 14788
	private bool m_skyIsReparenting;

	// Token: 0x040039C5 RID: 14789
	private bool m_transitionStateEnabled;

	// Token: 0x040039C6 RID: 14790
	private HashSet<SkillTreeType> m_availableSkillsSet;

	// Token: 0x040039C7 RID: 14791
	private Action<MonoBehaviour, EventArgs> m_onHighlightedSkillChanged;

	// Token: 0x040039C8 RID: 14792
	private Action<MonoBehaviour, EventArgs> m_onSkillLevelChanged;

	// Token: 0x040039C9 RID: 14793
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040039CA RID: 14794
	private Action m_cancelResetWarning;

	// Token: 0x040039CB RID: 14795
	private Action<InputActionEventData> m_onCancelButtonPressed;

	// Token: 0x040039CC RID: 14796
	private Action<InputActionEventData> m_onPurchaseButtonPressed;

	// Token: 0x040039CD RID: 14797
	private Action<InputActionEventData> m_onToggleCastleViewPressed;

	// Token: 0x040039CE RID: 14798
	private Action<InputActionEventData> m_onPurchaseMultipleButtonPressed;

	// Token: 0x040039CF RID: 14799
	public static bool CastleViewEnabled;

	// Token: 0x040039D0 RID: 14800
	private Tween m_purchaseTween;
}
