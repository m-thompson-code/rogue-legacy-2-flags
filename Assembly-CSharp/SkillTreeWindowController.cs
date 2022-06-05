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

// Token: 0x02000593 RID: 1427
public class SkillTreeWindowController : WindowController, ILocalizable
{
	// Token: 0x170012E5 RID: 4837
	// (get) Token: 0x06003595 RID: 13717 RVA: 0x000BA298 File Offset: 0x000B8498
	public int SkillCount
	{
		get
		{
			return this.m_availableSkillsSet.Count;
		}
	}

	// Token: 0x170012E6 RID: 4838
	// (get) Token: 0x06003596 RID: 13718 RVA: 0x000BA2A5 File Offset: 0x000B84A5
	public HashSet<SkillTreeType> AvailableSkills
	{
		get
		{
			return this.m_availableSkillsSet;
		}
	}

	// Token: 0x170012E7 RID: 4839
	// (get) Token: 0x06003597 RID: 13719 RVA: 0x000BA2AD File Offset: 0x000B84AD
	public GameObject SkillTreeCastleParentObj
	{
		get
		{
			return this.m_castleParentObj;
		}
	}

	// Token: 0x170012E8 RID: 4840
	// (get) Token: 0x06003598 RID: 13720 RVA: 0x000BA2B5 File Offset: 0x000B84B5
	public override WindowID ID
	{
		get
		{
			return WindowID.SkillTree;
		}
	}

	// Token: 0x170012E9 RID: 4841
	// (get) Token: 0x06003599 RID: 13721 RVA: 0x000BA2B8 File Offset: 0x000B84B8
	// (set) Token: 0x0600359A RID: 13722 RVA: 0x000BA2C0 File Offset: 0x000B84C0
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

	// Token: 0x0600359B RID: 13723 RVA: 0x000BA2CC File Offset: 0x000B84CC
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

	// Token: 0x0600359C RID: 13724 RVA: 0x000BA36C File Offset: 0x000B856C
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

	// Token: 0x0600359D RID: 13725 RVA: 0x000BA534 File Offset: 0x000B8734
	protected override void OnFocus()
	{
		this.m_infoPlateIcon.sprite = IconLibrary.GetSkillTreeIcon(this.m_selectedSkillTreeButton.SkillTreeType);
		this.m_selectedSkillTreeButton.gameObject.SetActive(true);
		this.m_selectedSkillTreeButton.Select(false);
		this.AddListeners();
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x000BA574 File Offset: 0x000B8774
	protected override void OnLostFocus()
	{
		this.RemoveListeners();
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x000BA57C File Offset: 0x000B877C
	protected override void OnPause()
	{
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x000BA57E File Offset: 0x000B877E
	protected override void OnUnpause()
	{
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x000BA580 File Offset: 0x000B8780
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

	// Token: 0x060035A2 RID: 13730 RVA: 0x000BA600 File Offset: 0x000B8800
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

	// Token: 0x060035A3 RID: 13731 RVA: 0x000BA6C4 File Offset: 0x000B88C4
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

	// Token: 0x060035A4 RID: 13732 RVA: 0x000BA978 File Offset: 0x000B8B78
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

	// Token: 0x060035A5 RID: 13733 RVA: 0x000BA9D4 File Offset: 0x000B8BD4
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

	// Token: 0x060035A6 RID: 13734 RVA: 0x000BAA2C File Offset: 0x000B8C2C
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

	// Token: 0x060035A7 RID: 13735 RVA: 0x000BAABC File Offset: 0x000B8CBC
	private void UpdateLevelText()
	{
		int totalSkillObjLevel = SkillTreeManager.GetTotalSkillObjLevel();
		this.m_levelText.text = string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_CHARACTER_LEVEL_1", false, false), totalSkillObjLevel.ToString());
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x000BAAF4 File Offset: 0x000B8CF4
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

	// Token: 0x060035A9 RID: 13737 RVA: 0x000BAC90 File Offset: 0x000B8E90
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

	// Token: 0x060035AA RID: 13738 RVA: 0x000BACA0 File Offset: 0x000B8EA0
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

	// Token: 0x060035AB RID: 13739 RVA: 0x000BAD74 File Offset: 0x000B8F74
	private void AddListeners()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onPurchaseButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onToggleCastleViewPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onPurchaseMultipleButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000BADFC File Offset: 0x000B8FFC
	private void RemoveListeners()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onPurchaseButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onToggleCastleViewPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onPurchaseMultipleButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x000BAE84 File Offset: 0x000B9084
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

	// Token: 0x060035AE RID: 13742 RVA: 0x000BAF1C File Offset: 0x000B911C
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

	// Token: 0x060035AF RID: 13743 RVA: 0x000BB028 File Offset: 0x000B9228
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

	// Token: 0x060035B0 RID: 13744 RVA: 0x000BB049 File Offset: 0x000B9249
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

	// Token: 0x060035B1 RID: 13745 RVA: 0x000BB06C File Offset: 0x000B926C
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

	// Token: 0x060035B2 RID: 13746 RVA: 0x000BB14C File Offset: 0x000B934C
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

	// Token: 0x060035B3 RID: 13747 RVA: 0x000BB24B File Offset: 0x000B944B
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

	// Token: 0x060035B4 RID: 13748 RVA: 0x000BB261 File Offset: 0x000B9461
	private void OnCancelButtonPressed(InputActionEventData eventData)
	{
		if (!this.m_skyIsReparenting)
		{
			WindowManager.SetWindowIsOpen(WindowID.SkillTree, false, TransitionID.QuickSwipe);
		}
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x000BB273 File Offset: 0x000B9473
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

	// Token: 0x060035B6 RID: 13750 RVA: 0x000BB290 File Offset: 0x000B9490
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

	// Token: 0x060035B7 RID: 13751 RVA: 0x000BB2A0 File Offset: 0x000B94A0
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

	// Token: 0x060035B8 RID: 13752 RVA: 0x000BB348 File Offset: 0x000B9548
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

	// Token: 0x060035B9 RID: 13753 RVA: 0x000BB3F0 File Offset: 0x000B95F0
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

	// Token: 0x060035BA RID: 13754 RVA: 0x000BB498 File Offset: 0x000B9698
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

	// Token: 0x060035BB RID: 13755 RVA: 0x000BB540 File Offset: 0x000B9740
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

	// Token: 0x060035BC RID: 13756 RVA: 0x000BB5E8 File Offset: 0x000B97E8
	private void CancelResetWarning()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x000BB5F4 File Offset: 0x000B97F4
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

	// Token: 0x060035BE RID: 13758 RVA: 0x000BB684 File Offset: 0x000B9884
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

	// Token: 0x060035BF RID: 13759 RVA: 0x000BB6EC File Offset: 0x000B98EC
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

	// Token: 0x040029D5 RID: 10709
	public const int MULTIPLE_PURCHASE_COUNT = 5;

	// Token: 0x040029D6 RID: 10710
	[SerializeField]
	private SkillTreeSlot m_startingHighlightedButton;

	// Token: 0x040029D7 RID: 10711
	[SerializeField]
	private Image m_infoPlateIcon;

	// Token: 0x040029D8 RID: 10712
	[SerializeField]
	private GameObject m_castleParentObj;

	// Token: 0x040029D9 RID: 10713
	[SerializeField]
	private Animator m_castleAnimator;

	// Token: 0x040029DA RID: 10714
	[SerializeField]
	private CanvasGroup m_skillTreeIconsCanvasGroup;

	// Token: 0x040029DB RID: 10715
	[SerializeField]
	private CanvasGroup m_descriptionBoxCanvasGroup;

	// Token: 0x040029DC RID: 10716
	[SerializeField]
	private RectTransform m_shopSignRectTransform;

	// Token: 0x040029DD RID: 10717
	[SerializeField]
	private GameObject m_labourCostGO;

	// Token: 0x040029DE RID: 10718
	[SerializeField]
	private TMP_Text m_labourCostText;

	// Token: 0x040029DF RID: 10719
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040029E0 RID: 10720
	[SerializeField]
	private TMP_Text m_affordableItemsText;

	// Token: 0x040029E1 RID: 10721
	[SerializeField]
	private GameObject m_navigationObj;

	// Token: 0x040029E2 RID: 10722
	private SkillTreeSlot m_selectedSkillTreeButton;

	// Token: 0x040029E3 RID: 10723
	private Dictionary<SkillTreeType, SkillTreeSlot> m_skillTreeButtonDict;

	// Token: 0x040029E4 RID: 10724
	private RewiredStandaloneInputModule m_standaloneInputModule;

	// Token: 0x040029E5 RID: 10725
	private Vector2 m_shopSignStartingPos;

	// Token: 0x040029E6 RID: 10726
	private bool m_shopSignDisplayed;

	// Token: 0x040029E7 RID: 10727
	private Coroutine m_currentAnimCoroutine;

	// Token: 0x040029E8 RID: 10728
	private bool m_skyIsReparenting;

	// Token: 0x040029E9 RID: 10729
	private bool m_transitionStateEnabled;

	// Token: 0x040029EA RID: 10730
	private HashSet<SkillTreeType> m_availableSkillsSet;

	// Token: 0x040029EB RID: 10731
	private Action<MonoBehaviour, EventArgs> m_onHighlightedSkillChanged;

	// Token: 0x040029EC RID: 10732
	private Action<MonoBehaviour, EventArgs> m_onSkillLevelChanged;

	// Token: 0x040029ED RID: 10733
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040029EE RID: 10734
	private Action m_cancelResetWarning;

	// Token: 0x040029EF RID: 10735
	private Action<InputActionEventData> m_onCancelButtonPressed;

	// Token: 0x040029F0 RID: 10736
	private Action<InputActionEventData> m_onPurchaseButtonPressed;

	// Token: 0x040029F1 RID: 10737
	private Action<InputActionEventData> m_onToggleCastleViewPressed;

	// Token: 0x040029F2 RID: 10738
	private Action<InputActionEventData> m_onPurchaseMultipleButtonPressed;

	// Token: 0x040029F3 RID: 10739
	public static bool CastleViewEnabled;

	// Token: 0x040029F4 RID: 10740
	private Tween m_purchaseTween;
}
