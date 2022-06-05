using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005FB RID: 1531
public class GearCardEntry : MonoBehaviour
{
	// Token: 0x17001292 RID: 4754
	// (get) Token: 0x06002F2E RID: 12078 RVA: 0x00019D68 File Offset: 0x00017F68
	// (set) Token: 0x06002F2F RID: 12079 RVA: 0x00019D70 File Offset: 0x00017F70
	public GearCardEntry.GearCardEntryType GearCardType { get; private set; }

	// Token: 0x06002F30 RID: 12080 RVA: 0x000C9AB4 File Offset: 0x000C7CB4
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

	// Token: 0x06002F31 RID: 12081 RVA: 0x000C9B50 File Offset: 0x000C7D50
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

	// Token: 0x06002F32 RID: 12082 RVA: 0x000C9BE4 File Offset: 0x000C7DE4
	public void SetAsHeirloom(HeirloomType heirloomType)
	{
		HeirloomData heirloomData = HeirloomLibrary.GetHeirloomData(heirloomType);
		this.m_titleText.text = LocalizationManager.GetString(heirloomData.TitleLocID, false, false);
		this.m_descriptionText.text = LocalizationManager.GetString(heirloomData.DescriptionLocID, false, false);
		this.m_icon.sprite = IconLibrary.GetHeirloomSprite(heirloomType);
		this.GearCardType = GearCardEntry.GearCardEntryType.Heirloom;
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x000C9C40 File Offset: 0x000C7E40
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

	// Token: 0x06002F34 RID: 12084 RVA: 0x000C9D90 File Offset: 0x000C7F90
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

	// Token: 0x0400269D RID: 9885
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x0400269E RID: 9886
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x0400269F RID: 9887
	[SerializeField]
	private Image m_icon;

	// Token: 0x020005FC RID: 1532
	public enum GearCardEntryType
	{
		// Token: 0x040026A2 RID: 9890
		None,
		// Token: 0x040026A3 RID: 9891
		Equipment,
		// Token: 0x040026A4 RID: 9892
		Rune,
		// Token: 0x040026A5 RID: 9893
		Heirloom,
		// Token: 0x040026A6 RID: 9894
		Relic
	}
}
