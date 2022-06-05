using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000399 RID: 921
public class BlacksmithOmniUIEquipButton : OmniUIButton, IBlacksmithOmniUIButton
{
	// Token: 0x17000E46 RID: 3654
	// (get) Token: 0x06002261 RID: 8801 RVA: 0x0006F0C3 File Offset: 0x0006D2C3
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x06002262 RID: 8802 RVA: 0x0006F0CB File Offset: 0x0006D2CB
	// (set) Token: 0x06002263 RID: 8803 RVA: 0x0006F0D3 File Offset: 0x0006D2D3
	public EquipmentCategoryType CategoryType { get; set; }

	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x06002264 RID: 8804 RVA: 0x0006F0DC File Offset: 0x0006D2DC
	// (set) Token: 0x06002265 RID: 8805 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
	public EquipmentType EquipmentType { get; set; }

	// Token: 0x06002266 RID: 8806 RVA: 0x0006F0F0 File Offset: 0x0006D2F0
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = OmniUIButtonType.Equipping;
		if (EquipmentManager.IsEquipped(this.CategoryType, this.EquipmentType))
		{
			buttonType = OmniUIButtonType.Unequipping;
		}
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new BlacksmithOmniUIDescriptionEventArgs(this.CategoryType, this.EquipmentType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.CategoryType, this.EquipmentType, buttonType);
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x0006F150 File Offset: 0x0006D350
	public override void OnConfirmButtonPressed()
	{
		base.OnConfirmButtonPressed();
		if (!EquipmentManager.IsEquipped(this.CategoryType, this.EquipmentType))
		{
			if (!EquipmentManager.CanEquip(this.CategoryType, this.EquipmentType, true))
			{
				base.StartCoroutine(this.ShakeAnimCoroutine());
				return;
			}
			if (BlacksmithOmniUIEquipButton.SetEquipped(this.CategoryType, this.EquipmentType))
			{
				this.InitializeButtonEventArgs();
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
				this.RunOnConfirmPressedAnimation();
				return;
			}
		}
		else if (BlacksmithOmniUIEquipButton.SetEquipped(this.CategoryType, EquipmentType.None))
		{
			this.InitializeButtonEventArgs();
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
			this.RunOnConfirmPressedAnimation();
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x0006F214 File Offset: 0x0006D414
	public static bool SetEquipped(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(category);
		int num = (equipped != null) ? EquipmentManager.Get_EquipmentSet_CurrentUnityTier(equipped.EquipmentType) : 0;
		int num2 = EquipmentManager.Get_EquipmentSet_CurrentUnityTier(equipType);
		bool result = EquipmentManager.SetEquipped(category, equipType, true);
		EquipmentObj equipped2 = EquipmentManager.GetEquipped(category);
		int num3 = EquipmentManager.Get_EquipmentSet_CurrentUnityTier(equipType);
		int num4 = (equipped != null) ? EquipmentManager.Get_EquipmentSet_CurrentUnityTier(equipped.EquipmentType) : 0;
		if (equipped != null && num4 < num)
		{
			Vector3 unityDownTextPosition = (WindowManager.GetWindowController(WindowID.Blacksmith) as BlacksmithOmniUIWindowController).UnityDownTextPosition;
			string text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_UNITY_DOWN_1", false, false);
			bool isFemale = false;
			text = LocalizationManager.GetFormatterGenderForcedString(text, out isFemale);
			string @string = LocalizationManager.GetString(equipped.EquipmentData.Title, isFemale, false);
			if (string.IsNullOrEmpty(@string) || @string.Contains("LOC_ID"))
			{
				@string = LocalizationManager.GetString(Equipment_EV.GetEquipmentTypeNameLocID(equipped.EquipmentType), isFemale, true);
			}
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.UnityLevelDown, string.Format(text, @string), unityDownTextPosition, null, TextAlignmentOptions.Center);
			Debug.Log("Unity level has lowered for equipment type: " + equipped.EquipmentType.ToString());
			return result;
		}
		if (equipped2 != null && num3 > num2)
		{
			Vector3 unityDownTextPosition2 = (WindowManager.GetWindowController(WindowID.Blacksmith) as BlacksmithOmniUIWindowController).UnityDownTextPosition;
			string text2 = LocalizationManager.GetString("LOC_ID_BLACKSMITH_UNITY_UP_1", false, false);
			bool isFemale2 = false;
			text2 = LocalizationManager.GetFormatterGenderForcedString(text2, out isFemale2);
			string string2 = LocalizationManager.GetString(equipped2.EquipmentData.Title, isFemale2, false);
			if (string.IsNullOrEmpty(string2) || string2.Contains("LOC_ID"))
			{
				string2 = LocalizationManager.GetString(Equipment_EV.GetEquipmentTypeNameLocID(equipped2.EquipmentType), isFemale2, true);
			}
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.UnityLevelDown, string.Format(text2, string2), unityDownTextPosition2, null, TextAlignmentOptions.Center);
			Debug.Log("Unity level has raised for equipment type: " + equipped2.EquipmentType.ToString());
		}
		return result;
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x0006F3E8 File Offset: 0x0006D5E8
	public override void UpdateState()
	{
		if (EquipmentManager.GetFoundState(this.CategoryType, this.EquipmentType) != FoundState.Purchased)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.m_canvasGroup.alpha = 1f;
			this.IsButtonActive = true;
		}
		EquipmentObj equipped = EquipmentManager.GetEquipped(this.CategoryType);
		if (equipped != null && equipped.EquipmentType == this.EquipmentType)
		{
			this.SetIconEquippedState(true);
			return;
		}
		this.SetIconEquippedState(false);
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x0006F480 File Offset: 0x0006D680
	private void SetIconEquippedState(bool equipped)
	{
		if (equipped)
		{
			if (!this.m_sliderOnSprite.gameObject.activeSelf)
			{
				this.m_sliderOnSprite.gameObject.SetActive(true);
			}
			if (this.m_sliderOffSprite.gameObject.activeSelf)
			{
				this.m_sliderOffSprite.gameObject.SetActive(false);
			}
			Vector3 localPosition = this.m_notchGameObj.transform.localPosition;
			localPosition.x = 45f;
			this.m_notchGameObj.transform.localPosition = localPosition;
			return;
		}
		if (this.m_sliderOnSprite.gameObject.activeSelf)
		{
			this.m_sliderOnSprite.gameObject.SetActive(false);
		}
		if (!this.m_sliderOffSprite.gameObject.activeInHierarchy)
		{
			this.m_sliderOffSprite.gameObject.SetActive(true);
		}
		Vector3 localPosition2 = this.m_notchGameObj.transform.localPosition;
		localPosition2.x = 0f;
		this.m_notchGameObj.transform.localPosition = localPosition2;
	}

	// Token: 0x04001DB5 RID: 7605
	public const float NOTCH_SHIFT_AMOUNT = 45f;

	// Token: 0x04001DB6 RID: 7606
	[SerializeField]
	private Image m_sliderOnSprite;

	// Token: 0x04001DB7 RID: 7607
	[SerializeField]
	private Image m_sliderOffSprite;

	// Token: 0x04001DB8 RID: 7608
	[SerializeField]
	private GameObject m_notchGameObj;

	// Token: 0x04001DB9 RID: 7609
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001DBA RID: 7610
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
