using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200062A RID: 1578
public class BlacksmithOmniUIEquipButton : OmniUIButton, IBlacksmithOmniUIButton
{
	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x06003079 RID: 12409 RVA: 0x0001A941 File Offset: 0x00018B41
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x0600307A RID: 12410 RVA: 0x0001A949 File Offset: 0x00018B49
	// (set) Token: 0x0600307B RID: 12411 RVA: 0x0001A951 File Offset: 0x00018B51
	public EquipmentCategoryType CategoryType { get; set; }

	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x0600307C RID: 12412 RVA: 0x0001A95A File Offset: 0x00018B5A
	// (set) Token: 0x0600307D RID: 12413 RVA: 0x0001A962 File Offset: 0x00018B62
	public EquipmentType EquipmentType { get; set; }

	// Token: 0x0600307E RID: 12414 RVA: 0x000CFE48 File Offset: 0x000CE048
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

	// Token: 0x0600307F RID: 12415 RVA: 0x000CFEA8 File Offset: 0x000CE0A8
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

	// Token: 0x06003080 RID: 12416 RVA: 0x000CFF6C File Offset: 0x000CE16C
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

	// Token: 0x06003081 RID: 12417 RVA: 0x000D0140 File Offset: 0x000CE340
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

	// Token: 0x06003082 RID: 12418 RVA: 0x000D01D8 File Offset: 0x000CE3D8
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

	// Token: 0x040027C9 RID: 10185
	public const float NOTCH_SHIFT_AMOUNT = 45f;

	// Token: 0x040027CA RID: 10186
	[SerializeField]
	private Image m_sliderOnSprite;

	// Token: 0x040027CB RID: 10187
	[SerializeField]
	private Image m_sliderOffSprite;

	// Token: 0x040027CC RID: 10188
	[SerializeField]
	private GameObject m_notchGameObj;

	// Token: 0x040027CD RID: 10189
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x040027CE RID: 10190
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
