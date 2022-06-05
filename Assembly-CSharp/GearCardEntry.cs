using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000377 RID: 887
public class GearCardEntry : MonoBehaviour
{
	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06002147 RID: 8519 RVA: 0x00068B91 File Offset: 0x00066D91
	// (set) Token: 0x06002148 RID: 8520 RVA: 0x00068B99 File Offset: 0x00066D99
	public GearCardEntry.GearCardEntryType GearCardType { get; private set; }

	// Token: 0x06002149 RID: 8521 RVA: 0x00068BA4 File Offset: 0x00066DA4
	public void SetAsEquipment(EquipmentCategoryType category, EquipmentType equipType)
	{
		EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(category, equipType);
		string text = LocalizationManager.GetString(equipmentData.Title, false, false);
		if (string.IsNullOrEmpty(text) || text.Contains("LOC_ID"))
		{
			text = Equipment_EV.GetFormattedEquipmentName(category, equipType);
		}
		int upgradeLevel = EquipmentManager.GetUpgradeLevel(category, equipType);
		if (upgradeLevel > 0)
		{
			text = text + " +" + upgradeLevel.ToString();
		}
		this.m_titleText.text = text;
		this.m_descriptionText.text = LocalizationManager.GetString(equipmentData.Description, false, false);
		this.m_icon.sprite = IconLibrary.GetEquipmentIcon(category, equipType);
		this.GearCardType = GearCardEntry.GearCardEntryType.Equipment;
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x00068C40 File Offset: 0x00066E40
	public void SetAsRune(RuneType runeType)
	{
		RuneData runeData = RuneLibrary.GetRuneData(runeType);
		RuneObj rune = RuneManager.GetRune(runeType);
		this.m_titleText.text = LocalizationManager.GetString(runeData.Title, false, false);
		if (rune.EquippedLevel > 0)
		{
			TMP_Text titleText = this.m_titleText;
			titleText.text = titleText.text + " +" + rune.EquippedLevel.ToString();
		}
		this.m_descriptionText.text = LocalizationManager.GetString(runeData.Description, false, false);
		this.m_icon.sprite = IconLibrary.GetRuneIcon(runeType);
		this.GearCardType = GearCardEntry.GearCardEntryType.Rune;
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x00068CD4 File Offset: 0x00066ED4
	public void SetAsHeirloom(HeirloomType heirloomType)
	{
		HeirloomData heirloomData = HeirloomLibrary.GetHeirloomData(heirloomType);
		this.m_titleText.text = LocalizationManager.GetString(heirloomData.TitleLocID, false, false);
		this.m_descriptionText.text = LocalizationManager.GetString(heirloomData.DescriptionLocID, false, false);
		this.m_icon.sprite = IconLibrary.GetHeirloomSprite(heirloomType);
		this.GearCardType = GearCardEntry.GearCardEntryType.Heirloom;
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x00068D30 File Offset: 0x00066F30
	public void SetAsRelic(RelicType relicType)
	{
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
		this.m_titleText.text = LocalizationManager.GetString(relicData.Title, false, false);
		bool flag;
		this.m_descriptionText.text = LocalizationManager.GetString(relicData.Description, false, out flag, false);
		if (relic != null)
		{
			if (flag)
			{
				float value;
				float relicFormatString = Relic_EV.GetRelicFormatString(relicType, relic.Level, out value);
				this.m_descriptionText.text = string.Format(this.m_descriptionText.text, relicFormatString.ToCIString(), value.ToCIString());
			}
			if (relic.Level > 1)
			{
				TMP_Text titleText = this.m_titleText;
				titleText.text += string.Format(" (x{0})", relic.Level);
			}
			float costAmount = relicData.CostAmount;
			float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
			int num = Mathf.RoundToInt(Mathf.Clamp((costAmount - relicCostMod) * (float)relic.Level, 0f, float.MaxValue) * 100f);
			string arg = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), -num);
			TMP_Text titleText2 = this.m_titleText;
			titleText2.text += string.Format(" • <sprite=\"IconGlyph_Spritesheet_Texture\" name=\"Resolve_Icon\"> {0}", arg);
		}
		this.m_icon.sprite = IconLibrary.GetRelicSprite(relicType, false);
		this.GearCardType = GearCardEntry.GearCardEntryType.Relic;
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x00068E80 File Offset: 0x00067080
	public void SetAsUnity(EquipmentSetBonusType unityType, float bonusGain)
	{
		if (this.m_icon.transform.parent.gameObject.activeSelf)
		{
			this.m_icon.transform.parent.gameObject.SetActive(false);
		}
		if (this.m_descriptionText.gameObject.activeSelf)
		{
			this.m_descriptionText.gameObject.SetActive(false);
		}
		Vector3 localPosition = this.m_titleText.transform.parent.gameObject.transform.localPosition;
		localPosition.x = 40f;
		this.m_titleText.transform.parent.gameObject.transform.localPosition = localPosition;
		string arg;
		if (CDGHelper.IsPercent(bonusGain))
		{
			arg = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), bonusGain * 100f);
		}
		else
		{
			arg = bonusGain.ToString();
		}
		this.m_titleText.text = string.Format("+{0} ", arg);
		TMP_Text titleText = this.m_titleText;
		titleText.text += LocalizationManager.GetString(EquipmentSetLibrary.GetEquipmentSetBonusLocID(unityType), false, false);
	}

	// Token: 0x04001CD3 RID: 7379
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x04001CD4 RID: 7380
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001CD5 RID: 7381
	[SerializeField]
	private Image m_icon;

	// Token: 0x02000BFD RID: 3069
	public enum GearCardEntryType
	{
		// Token: 0x04004E60 RID: 20064
		None,
		// Token: 0x04004E61 RID: 20065
		Equipment,
		// Token: 0x04004E62 RID: 20066
		Rune,
		// Token: 0x04004E63 RID: 20067
		Heirloom,
		// Token: 0x04004E64 RID: 20068
		Relic
	}
}
