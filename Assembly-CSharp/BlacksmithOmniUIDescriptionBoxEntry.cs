using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000625 RID: 1573
public class BlacksmithOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<BlacksmithOmniUIDescriptionEventArgs, BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType>
{
	// Token: 0x0600305F RID: 12383 RVA: 0x0001A873 File Offset: 0x00018A73
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000CEBCC File Offset: 0x000CCDCC
	protected override void DisplayDescriptionBox(BlacksmithOmniUIDescriptionEventArgs args)
	{
		if (this.m_descriptionType != BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Unity && this.m_descriptionType != BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription1 && this.m_descriptionType != BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription2 && this.m_descriptionType != BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription3)
		{
			if (this.m_text1 != null)
			{
				this.m_text1.color = BaseOmniUIDescriptionBoxEntry<BlacksmithOmniUIDescriptionEventArgs, BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType>.OriginalColor;
			}
			if (this.m_text2 != null)
			{
				this.m_text2.color = BaseOmniUIDescriptionBoxEntry<BlacksmithOmniUIDescriptionEventArgs, BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType>.OriginalColor;
			}
		}
		if (this.m_textCanvasGroup != null)
		{
			this.m_textCanvasGroup.alpha = 0.35f;
		}
		EquipmentCategoryType categoryType = args.CategoryType;
		EquipmentType equipmentType = args.EquipmentType;
		EquipmentObj equipment = EquipmentManager.GetEquipment(categoryType, equipmentType);
		EquipmentObj equipped = EquipmentManager.GetEquipped(categoryType);
		EquipmentManager.IsEquipped(categoryType, equipmentType);
		EquipmentSetData equipmentSetData = EquipmentSetLibrary.GetEquipmentSetData(equipmentType);
		if (equipment == null)
		{
			return;
		}
		if (!this.m_previousEquipmentSetLevel.ContainsKey(equipmentType))
		{
			int value = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType);
			this.m_previousEquipmentSetLevel.Add(equipmentType, value);
		}
		switch (this.m_descriptionType)
		{
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Title:
		{
			this.m_icon.sprite = IconLibrary.GetEquipmentIcon(categoryType, equipmentType);
			string text = LocalizationManager.GetString(equipment.EquipmentData.Title, false, false);
			if (string.IsNullOrEmpty(text) || text.Contains("LOC_ID"))
			{
				text = Equipment_EV.GetFormattedEquipmentName(categoryType, equipmentType);
			}
			if (equipment.ClampedUpgradeLevel > 0)
			{
				text = text + " +" + equipment.ClampedUpgradeLevel.ToString();
			}
			this.m_titleText.text = text;
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription1:
			if (equipmentSetData != null)
			{
				int setRequirement = equipmentSetData.SetRequirement01;
				this.m_text1.text = string.Format(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_UNITY_REQUIREMENT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), setRequirement);
				if (EquipmentManager.Get_EquipmentSet_CurrentLevel(equipmentType) >= setRequirement)
				{
					string arg;
					if (CDGHelper.IsPercent(equipmentSetData.SetBonus01.StatGain))
					{
						arg = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), equipmentSetData.SetBonus01.StatGain * 100f);
					}
					else
					{
						arg = equipmentSetData.SetBonus01.StatGain.ToString();
					}
					if (EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType) >= setRequirement)
					{
						if (EquipmentManager.Get_EquipmentSet_LevelViewed(equipmentType) < 1)
						{
							EquipmentManager.Set_EquipmentSet_LevelViewed(equipmentType, 1, false);
							string newText = string.Format("<color=green>+{0}</color> ", arg);
							base.StartCoroutine(this.RunRevealUnityEffect(this.m_titleText, newText));
							this.FireUnityBonusEvents(equipmentType, setRequirement);
						}
						else
						{
							this.m_titleText.alpha = 1f;
							this.m_titleText.text = string.Format("<color=green>+{0}</color> ", arg);
						}
					}
					else
					{
						this.m_titleText.alpha = 0.4f;
						this.m_titleText.text = string.Format("+{0} ", arg);
					}
					TMP_Text titleText = this.m_titleText;
					titleText.text += LocalizationManager.GetString(EquipmentSetLibrary.GetEquipmentSetBonusLocID(equipmentSetData.SetBonus01.BonusType), false, false);
					return;
				}
				this.m_titleText.alpha = 1f;
				this.m_titleText.text = "???????";
				return;
			}
			break;
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription2:
			if (equipmentSetData != null)
			{
				int setRequirement2 = equipmentSetData.SetRequirement02;
				this.m_text1.text = string.Format(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_UNITY_REQUIREMENT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), setRequirement2);
				if (EquipmentManager.Get_EquipmentSet_CurrentLevel(equipmentType) >= setRequirement2)
				{
					string arg2;
					if (CDGHelper.IsPercent(equipmentSetData.SetBonus02.StatGain))
					{
						arg2 = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), equipmentSetData.SetBonus02.StatGain * 100f);
					}
					else
					{
						arg2 = equipmentSetData.SetBonus02.StatGain.ToString();
					}
					if (EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType) >= setRequirement2)
					{
						if (EquipmentManager.Get_EquipmentSet_LevelViewed(equipmentType) < 2)
						{
							EquipmentManager.Set_EquipmentSet_LevelViewed(equipmentType, 2, false);
							string newText2 = string.Format("<color=green>+{0}</color> ", arg2);
							base.StartCoroutine(this.RunRevealUnityEffect(this.m_titleText, newText2));
							this.FireUnityBonusEvents(equipmentType, setRequirement2);
						}
						else
						{
							this.m_titleText.alpha = 1f;
							this.m_titleText.text = string.Format("<color=green>+{0}</color> ", arg2);
						}
					}
					else
					{
						this.m_titleText.alpha = 0.4f;
						this.m_titleText.text = string.Format("+{0} ", arg2);
					}
					TMP_Text titleText2 = this.m_titleText;
					titleText2.text += LocalizationManager.GetString(EquipmentSetLibrary.GetEquipmentSetBonusLocID(equipmentSetData.SetBonus02.BonusType), false, false);
					return;
				}
				this.m_titleText.alpha = 1f;
				this.m_titleText.text = "???????";
				return;
			}
			break;
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.UnityDescription3:
			if (equipmentSetData != null)
			{
				int setRequirement3 = equipmentSetData.SetRequirement03;
				this.m_text1.text = string.Format(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_UNITY_REQUIREMENT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), setRequirement3);
				int num = EquipmentManager.Get_EquipmentSet_CurrentLevel(equipmentType);
				this.FireUnityBonusEvents(equipmentType, setRequirement3);
				if (num >= setRequirement3)
				{
					string arg3;
					if (CDGHelper.IsPercent(equipmentSetData.SetBonus03.StatGain))
					{
						arg3 = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), equipmentSetData.SetBonus03.StatGain * 100f);
					}
					else
					{
						arg3 = equipmentSetData.SetBonus03.StatGain.ToString();
					}
					if (EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType) >= setRequirement3)
					{
						if (EquipmentManager.Get_EquipmentSet_LevelViewed(equipmentType) < 3)
						{
							EquipmentManager.Set_EquipmentSet_LevelViewed(equipmentType, 3, false);
							string newText3 = string.Format("<color=green>+{0}</color> ", arg3);
							base.StartCoroutine(this.RunRevealUnityEffect(this.m_titleText, newText3));
							StoreAPIManager.GiveAchievement(AchievementType.UnlockHighestUnity, StoreType.All);
							this.FireUnityBonusEvents(equipmentType, setRequirement3);
						}
						else
						{
							this.m_titleText.alpha = 1f;
							this.m_titleText.text = string.Format("<color=green>+{0}</color> ", arg3);
						}
					}
					else
					{
						this.m_titleText.alpha = 0.4f;
						this.m_titleText.text = string.Format("+{0} ", arg3);
					}
					TMP_Text titleText3 = this.m_titleText;
					titleText3.text += LocalizationManager.GetString(EquipmentSetLibrary.GetEquipmentSetBonusLocID(equipmentSetData.SetBonus03.BonusType), false, false);
					return;
				}
				this.m_titleText.alpha = 1f;
				this.m_titleText.text = "???????";
			}
			break;
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Weight:
		{
			float playerVal = (float)(PlayerManager.IsInstantiated ? PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight : 0);
			float currentStatValue = equipment.GetCurrentStatValue(EquipmentStatType.Weight);
			float statValueAtLevel = equipment.GetStatValueAtLevel(EquipmentStatType.Weight, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Weight) : 0f;
			this.SetStatString(playerVal, currentStatValue, statValueAtLevel, equippedItemVal, true, false, args);
			this.m_text2.text = ((int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight)).ToString() + "/" + this.m_text2.text;
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Encumberance:
		{
			PlayerManager.GetPlayerController();
			int num2 = EquipmentManager.GetWeightLevel();
			num2 = Mathf.Clamp(num2, 0, Equipment_EV.CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL.Length - 1);
			string arg4 = Mathf.RoundToInt(Equipment_EV.RESOLVE_BONUS_PER_WEIGHT_LEVEL[num2] * 100f).ToString();
			string text2 = string.Format(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_BONUS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), arg4);
			switch (num2)
			{
			case 0:
				this.m_titleText.text = base.ColoredString(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Color.blue);
				this.m_text2.text = base.ColoredString(text2, Color.blue);
				return;
			case 1:
				this.m_titleText.text = base.ColoredString(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_2", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Color.black);
				this.m_text2.text = base.ColoredString(text2, Color.black);
				return;
			case 2:
			{
				Color color = new Color(0.39215687f, 0.078431375f, 0.8039216f, 1f);
				this.m_titleText.text = base.ColoredString(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_3", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), color);
				this.m_text2.text = base.ColoredString(text2, color);
				return;
			}
			case 3:
				this.m_titleText.text = base.ColoredString(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_4", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Color.red);
				this.m_text2.text = base.ColoredString(text2, Color.red);
				return;
			default:
				this.m_titleText.text = base.ColoredString(LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_ENCUMBRANCE_5", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Color.red);
				this.m_text2.text = base.ColoredString(text2, Color.red);
				return;
			}
			break;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Strength:
		{
			float playerVal2 = (float)(PlayerManager.IsInstantiated ? Mathf.RoundToInt(PlayerManager.GetPlayerController().ActualStrength) : 0);
			float currentStatValue2 = equipment.GetCurrentStatValue(EquipmentStatType.Strength);
			float statValueAtLevel2 = equipment.GetStatValueAtLevel(EquipmentStatType.Strength, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal2 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Strength) : 0f;
			this.SetStatString(playerVal2, currentStatValue2, statValueAtLevel2, equippedItemVal2, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Vitality:
		{
			float playerVal3 = (float)(PlayerManager.IsInstantiated ? PlayerManager.GetPlayerController().ActualVitality : 0);
			float currentStatValue3 = equipment.GetCurrentStatValue(EquipmentStatType.Vitality);
			float statValueAtLevel3 = equipment.GetStatValueAtLevel(EquipmentStatType.Vitality, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal3 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Vitality) : 0f;
			this.SetStatString(playerVal3, currentStatValue3, statValueAtLevel3, equippedItemVal3, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Magic:
		{
			float playerVal4 = (float)(PlayerManager.IsInstantiated ? Mathf.RoundToInt(PlayerManager.GetPlayerController().ActualMagic) : 0);
			float currentStatValue4 = equipment.GetCurrentStatValue(EquipmentStatType.Magic);
			float statValueAtLevel4 = equipment.GetStatValueAtLevel(EquipmentStatType.Magic, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal4 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Magic) : 0f;
			this.SetStatString(playerVal4, currentStatValue4, statValueAtLevel4, equippedItemVal4, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Armor:
		{
			float playerVal5 = (float)(PlayerManager.IsInstantiated ? PlayerManager.GetPlayerController().ActualArmor : 0);
			float currentStatValue5 = equipment.GetCurrentStatValue(EquipmentStatType.Armor);
			float statValueAtLevel5 = equipment.GetStatValueAtLevel(EquipmentStatType.Armor, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal5 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Armor) : 0f;
			this.SetStatString(playerVal5, currentStatValue5, statValueAtLevel5, equippedItemVal5, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Dexterity:
		{
			float playerVal6 = (float)(PlayerManager.IsInstantiated ? Mathf.RoundToInt(PlayerManager.GetPlayerController().ActualDexterity) : 0);
			float currentStatValue6 = equipment.GetCurrentStatValue(EquipmentStatType.Dexterity_Add);
			float statValueAtLevel6 = equipment.GetStatValueAtLevel(EquipmentStatType.Dexterity_Add, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal6 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Dexterity_Add) : 0f;
			this.SetStatString(playerVal6, currentStatValue6, statValueAtLevel6, equippedItemVal6, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Focus:
		{
			float playerVal7 = (float)(PlayerManager.IsInstantiated ? Mathf.RoundToInt(PlayerManager.GetPlayerController().ActualFocus) : 0);
			float currentStatValue7 = equipment.GetCurrentStatValue(EquipmentStatType.Focus_Add);
			float statValueAtLevel7 = equipment.GetStatValueAtLevel(EquipmentStatType.Focus_Add, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal7 = (equipped != null) ? equipped.GetCurrentStatValue(EquipmentStatType.Focus_Add) : 0f;
			this.SetStatString(playerVal7, currentStatValue7, statValueAtLevel7, equippedItemVal7, false, false, args);
			return;
		}
		case BlacksmithOmniUIDescriptionBoxEntry.BlacksmithOmniUIDescriptionBoxType.Unity:
		{
			float playerVal8 = (float)EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(args.EquipmentType);
			float currentStatValue8 = equipment.GetCurrentStatValue(EquipmentStatType.Unity);
			float statValueAtLevel8 = equipment.GetStatValueAtLevel(EquipmentStatType.Unity, equipment.ClampedUpgradeLevel + 1);
			float equippedItemVal8 = 0f;
			this.SetStatString(playerVal8, currentStatValue8, statValueAtLevel8, equippedItemVal8, false, false, args);
			string text3 = LocalizationManager.GetString("LOC_ID_EQUIP_STAT_TITLE_SET_BONUS_1", false, true);
			bool isFemale = false;
			text3 = LocalizationManager.GetFormatterGenderForcedString(text3, out isFemale);
			string @string = LocalizationManager.GetString(Equipment_EV.GetEquipmentTypeNameLocID(equipmentType), isFemale, true);
			this.m_titleText.text = string.Format(text3, @string);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000CF760 File Offset: 0x000CD960
	private void FireUnityBonusEvents(EquipmentType equipmentType, int requiredUnityLevel)
	{
		int num = EquipmentManager.Get_EquipmentSet_TotalEquippedLevel(equipmentType);
		if (this.m_previousEquipmentSetLevel[equipmentType] != num)
		{
			if (num >= requiredUnityLevel)
			{
				if (this.m_previousEquipmentSetLevel[equipmentType] < requiredUnityLevel && this.m_unityBonusOnUnityEvent != null)
				{
					this.m_unityBonusOnUnityEvent.Invoke();
				}
			}
			else if (this.m_previousEquipmentSetLevel[equipmentType] >= requiredUnityLevel && this.m_unityBonusOffUnityEvent != null)
			{
				this.m_unityBonusOffUnityEvent.Invoke();
			}
			this.m_previousEquipmentSetLevel[equipmentType] = num;
		}
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x0001A8A5 File Offset: 0x00018AA5
	private IEnumerator RunRevealUnityEffect(TMP_Text text, string newText)
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		RewiredMapController.SetMouseEnabled(false);
		text.alpha = 0f;
		text.text = newText;
		EffectManager.PlayEffect(base.gameObject, null, "EquipmentSetSparkle_Effect", Vector3.zero, 0.5f, EffectStopType.Gracefully, EffectTriggerDirection.None).transform.SetParent(text.transform.parent, false);
		yield return TweenManager.TweenTo_UnscaledTime(text, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		RewiredMapController.SetMouseEnabled(true);
		yield break;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000CF7DC File Offset: 0x000CD9DC
	private void SetStatString(float playerVal, float itemVal, float itemNextLevelVal, float equippedItemVal, bool lowerIsBetter, bool isPercent, BlacksmithOmniUIDescriptionEventArgs args)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(args.CategoryType);
		bool flag = false;
		if (equipped != null)
		{
			flag = (equipped.EquipmentType == args.EquipmentType);
		}
		float num = itemVal - equippedItemVal;
		float value = -itemVal;
		float num2 = itemNextLevelVal - itemVal;
		string text = isPercent ? base.PercentString(playerVal) : playerVal.ToCIString();
		if (!isPercent)
		{
			itemVal.ToCIString();
		}
		else
		{
			base.PercentString(itemVal);
		}
		switch (args.ButtonType)
		{
		case OmniUIButtonType.Purchasing:
			this.m_text1.text = base.ColoredValueString(itemVal, false, isPercent, lowerIsBetter, false);
			if (!flag && itemVal != 0f)
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num, true, isPercent, lowerIsBetter, false);
			}
			else
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num, true, isPercent, lowerIsBetter, true);
			}
			break;
		case OmniUIButtonType.Upgrading:
			this.m_text1.text = base.PlusSymbolString(itemVal, isPercent, true) + " " + base.ColoredValueString(num2, true, isPercent, lowerIsBetter, true);
			if (flag)
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num2, true, isPercent, lowerIsBetter, true);
			}
			else
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num + num2, true, isPercent, lowerIsBetter, true);
			}
			break;
		case OmniUIButtonType.Equipping:
			this.m_text1.text = base.PlusSymbolString(itemVal, isPercent, true);
			if (!flag && itemVal != 0f)
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num, true, isPercent, lowerIsBetter, false);
			}
			else
			{
				this.m_text2.text = text + " " + base.ColoredValueString(num, true, isPercent, lowerIsBetter, true);
			}
			break;
		case OmniUIButtonType.Unequipping:
			this.m_text1.text = base.PlusSymbolString(itemVal, isPercent, true);
			this.m_text2.text = text + " " + base.ColoredValueString(value, true, isPercent, lowerIsBetter, true);
			break;
		case OmniUIButtonType.CategorySelection:
			this.m_text2.text = text;
			break;
		}
		if (this.m_textCanvasGroup && itemVal != 0f)
		{
			this.m_textCanvasGroup.alpha = 1f;
		}
	}

	// Token: 0x040027AF RID: 10159
	[SerializeField]
	private UnityEvent m_unityBonusOnUnityEvent;

	// Token: 0x040027B0 RID: 10160
	[SerializeField]
	private UnityEvent m_unityBonusOffUnityEvent;

	// Token: 0x040027B1 RID: 10161
	private Dictionary<EquipmentType, int> m_previousEquipmentSetLevel = new Dictionary<EquipmentType, int>();

	// Token: 0x02000626 RID: 1574
	public enum BlacksmithOmniUIDescriptionBoxType
	{
		// Token: 0x040027B3 RID: 10163
		None,
		// Token: 0x040027B4 RID: 10164
		Title,
		// Token: 0x040027B5 RID: 10165
		UnityDescription1,
		// Token: 0x040027B6 RID: 10166
		UnityDescription2,
		// Token: 0x040027B7 RID: 10167
		UnityDescription3,
		// Token: 0x040027B8 RID: 10168
		Weight,
		// Token: 0x040027B9 RID: 10169
		Encumberance,
		// Token: 0x040027BA RID: 10170
		Strength,
		// Token: 0x040027BB RID: 10171
		Vitality,
		// Token: 0x040027BC RID: 10172
		Magic,
		// Token: 0x040027BD RID: 10173
		Armor,
		// Token: 0x040027BE RID: 10174
		Dexterity,
		// Token: 0x040027BF RID: 10175
		Focus,
		// Token: 0x040027C0 RID: 10176
		Unity
	}
}
