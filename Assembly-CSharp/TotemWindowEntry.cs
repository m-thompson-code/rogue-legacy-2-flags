using System;
using TMPro;
using UnityEngine;

// Token: 0x0200099B RID: 2459
public class TotemWindowEntry : MonoBehaviour
{
	// Token: 0x06004BE3 RID: 19427 RVA: 0x00129BA0 File Offset: 0x00127DA0
	public void OnEnable()
	{
		if (this.m_entryType == TotemWindowEntry.TotemEntryType.Stat && ClassLibrary.GetClassData(this.m_classType) != null)
		{
			this.m_classUnlocked = SkillTreeLogicHelper.IsClassUnlocked(this.m_classType);
		}
		if (!this.m_isBonusType)
		{
			this.UpdateClassTitle();
			this.UpdateClassValue();
			return;
		}
		this.UpdateBonusTitle();
		this.UpdateBonusValue();
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x00129BFC File Offset: 0x00127DFC
	private void UpdateClassTitle()
	{
		if (this.m_entryType == TotemWindowEntry.TotemEntryType.Stat)
		{
			if (this.m_classUnlocked)
			{
				string text = ClassLibrary.GetClassData(this.m_classType).PassiveData.Title;
				text = LocalizationManager.GetString(text, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				this.m_titleText.text = text;
				return;
			}
			this.m_titleText.text = "???????";
		}
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x00129C64 File Offset: 0x00127E64
	private void UpdateClassValue()
	{
		if (this.m_entryType != TotemWindowEntry.TotemEntryType.Stat)
		{
			if (this.m_entryType == TotemWindowEntry.TotemEntryType.Footer)
			{
				int totalMasteryRank = Mastery_EV.GetTotalMasteryRank();
				this.m_valueText.text = totalMasteryRank.ToString();
				if (totalMasteryRank >= 150)
				{
					StoreAPIManager.GiveAchievement(AchievementType.UnlockHighMastery, StoreType.All);
				}
			}
			return;
		}
		if (!this.m_classUnlocked)
		{
			this.m_valueText.text = "???";
			return;
		}
		MasteryBonusType key = MasteryBonusType.None;
		if (!Mastery_EV.MasteryBonusTypeTable.TryGetValue(this.m_classType, out key))
		{
			this.m_valueText.text = key.ToString() + " NOT FOUND IN TYPE TABLE.";
			return;
		}
		float num = 0f;
		if (Mastery_EV.MasteryBonusAmountTable.TryGetValue(key, out num))
		{
			int classMasteryRank = SaveManager.PlayerSaveData.GetClassMasteryRank(this.m_classType);
			this.m_valueText.text = classMasteryRank.ToString();
			return;
		}
		this.m_valueText.text = key.ToString() + " NOT FOUND IN AMOUNT TABLE.";
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x00129D64 File Offset: 0x00127F64
	private void UpdateBonusTitle()
	{
		if (this.m_entryType == TotemWindowEntry.TotemEntryType.Stat)
		{
			if (this.m_classUnlocked)
			{
				MasteryBonusType key = MasteryBonusType.None;
				if (!Mastery_EV.MasteryBonusTypeTable.TryGetValue(this.m_classType, out key))
				{
					this.m_titleText.text = key.ToString() + " NOT FOUND IN TYPE TABLE.";
					return;
				}
				string locID;
				if (Mastery_EV.MasteryBonusLocIDTable.TryGetValue(key, out locID))
				{
					this.m_titleText.text = LocalizationManager.GetString(locID, false, false);
					return;
				}
				this.m_titleText.text = key.ToString() + " NOT FOUND IN LOC ID TABLE.";
				return;
			}
			else
			{
				this.m_titleText.text = "???????";
			}
		}
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x00129E14 File Offset: 0x00128014
	private void UpdateBonusValue()
	{
		if (this.m_entryType != TotemWindowEntry.TotemEntryType.Stat)
		{
			if (this.m_entryType == TotemWindowEntry.TotemEntryType.Footer)
			{
				float num = Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.RuneWeight_Up);
				if (num != (float)((int)num))
				{
					num = (float)Mathf.CeilToInt(num * 100f);
					this.m_valueText.text = "+" + num.ToString() + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num);
					return;
				}
				this.m_valueText.text = "+" + num.ToString();
			}
			return;
		}
		if (!this.m_classUnlocked)
		{
			this.m_valueText.text = "???";
			return;
		}
		MasteryBonusType key = MasteryBonusType.None;
		if (!Mastery_EV.MasteryBonusTypeTable.TryGetValue(this.m_classType, out key))
		{
			this.m_valueText.text = key.ToString() + " NOT FOUND IN TYPE TABLE.";
			return;
		}
		float num2 = 0f;
		if (!Mastery_EV.MasteryBonusAmountTable.TryGetValue(key, out num2))
		{
			this.m_valueText.text = key.ToString() + " NOT FOUND IN AMOUNT TABLE.";
			return;
		}
		float num3 = (float)SaveManager.PlayerSaveData.GetClassMasteryRank(this.m_classType) * num2;
		if (CDGHelper.IsPercent(num2))
		{
			num3 = (float)Mathf.CeilToInt(num3 * 100f);
			this.m_valueText.text = "+" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num3);
			return;
		}
		this.m_valueText.text = "+" + num3.ToString();
	}

	// Token: 0x040039F3 RID: 14835
	[SerializeField]
	private TotemWindowEntry.TotemEntryType m_entryType;

	// Token: 0x040039F4 RID: 14836
	[SerializeField]
	private ClassType m_classType;

	// Token: 0x040039F5 RID: 14837
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040039F6 RID: 14838
	[SerializeField]
	private TMP_Text m_valueText;

	// Token: 0x040039F7 RID: 14839
	[SerializeField]
	private bool m_isBonusType;

	// Token: 0x040039F8 RID: 14840
	private bool m_classUnlocked;

	// Token: 0x0200099C RID: 2460
	private enum TotemEntryType
	{
		// Token: 0x040039FA RID: 14842
		Stat,
		// Token: 0x040039FB RID: 14843
		Header,
		// Token: 0x040039FC RID: 14844
		Footer
	}
}
