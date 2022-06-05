using System;
using TMPro;
using UnityEngine;

// Token: 0x02000596 RID: 1430
public class TotemWindowEntry : MonoBehaviour
{
	// Token: 0x060035D7 RID: 13783 RVA: 0x000BBAB4 File Offset: 0x000B9CB4
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

	// Token: 0x060035D8 RID: 13784 RVA: 0x000BBB10 File Offset: 0x000B9D10
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

	// Token: 0x060035D9 RID: 13785 RVA: 0x000BBB78 File Offset: 0x000B9D78
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

	// Token: 0x060035DA RID: 13786 RVA: 0x000BBC78 File Offset: 0x000B9E78
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

	// Token: 0x060035DB RID: 13787 RVA: 0x000BBD28 File Offset: 0x000B9F28
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

	// Token: 0x04002A00 RID: 10752
	[SerializeField]
	private TotemWindowEntry.TotemEntryType m_entryType;

	// Token: 0x04002A01 RID: 10753
	[SerializeField]
	private ClassType m_classType;

	// Token: 0x04002A02 RID: 10754
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04002A03 RID: 10755
	[SerializeField]
	private TMP_Text m_valueText;

	// Token: 0x04002A04 RID: 10756
	[SerializeField]
	private bool m_isBonusType;

	// Token: 0x04002A05 RID: 10757
	private bool m_classUnlocked;

	// Token: 0x02000D7B RID: 3451
	private enum TotemEntryType
	{
		// Token: 0x0400547C RID: 21628
		Stat,
		// Token: 0x0400547D RID: 21629
		Header,
		// Token: 0x0400547E RID: 21630
		Footer
	}
}
