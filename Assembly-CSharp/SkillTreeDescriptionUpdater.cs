using System;
using TMPro;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
public class SkillTreeDescriptionUpdater : MonoBehaviour
{
	// Token: 0x0600402F RID: 16431 RVA: 0x000E3504 File Offset: 0x000E1704
	private void Awake()
	{
		this.m_onHighlightedSkillChanged = new Action<MonoBehaviour, EventArgs>(this.OnHighlightedSkillChanged);
		this.m_onSkillLevelChanged = new Action<MonoBehaviour, EventArgs>(this.OnSkillLevelChanged);
		this.m_onGoldChanged = new Action<MonoBehaviour, EventArgs>(this.OnGoldChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GoldChanged, this.m_onGoldChanged);
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x000E356E File Offset: 0x000E176E
	private void OnDestroy()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_HighlightedSkillChanged, this.m_onHighlightedSkillChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GoldChanged, this.m_onGoldChanged);
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x000E3598 File Offset: 0x000E1798
	private void OnHighlightedSkillChanged(MonoBehaviour sender, EventArgs args)
	{
		HighlightedSkillChangedEventArgs highlightedSkillChangedEventArgs = args as HighlightedSkillChangedEventArgs;
		this.UpdateText(highlightedSkillChangedEventArgs.SkillTreeType);
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x000E35B8 File Offset: 0x000E17B8
	private void OnSkillLevelChanged(MonoBehaviour sender, EventArgs args)
	{
		if (args == null)
		{
			return;
		}
		SkillLevelChangedEventArgs skillLevelChangedEventArgs = args as SkillLevelChangedEventArgs;
		this.UpdateText(skillLevelChangedEventArgs.SkillTreeType);
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x000E35DC File Offset: 0x000E17DC
	private void UpdateText(SkillTreeType skillType)
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(skillType);
		SkillTreeData skillTreeData = SkillTreeLibrary.GetSkillTreeData(skillType);
		if (this.DescriptionLocItem)
		{
			this.DescriptionLocItem.gameObject.SetActive(true);
		}
		if (this.CurrentValue)
		{
			this.CurrentValue.gameObject.SetActive(true);
		}
		bool isLocked = skillTreeObj.IsLocked;
		bool isSoulLocked = skillTreeObj.IsSoulLocked;
		bool isLevelLocked = skillTreeObj.IsLevelLocked;
		switch (this.DescriptionType)
		{
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Title:
			if (isLocked)
			{
				this.DescriptionLocItem.SetString("LOC_ID_SKILL_LOCKED_TITLE_1");
				return;
			}
			if (isSoulLocked)
			{
				this.DescriptionLocItem.SetString("LOC_ID_SKILL_SOUL_LOCKED_TITLE_1");
				return;
			}
			this.DescriptionLocItem.SetString(skillTreeData.Title);
			return;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Description:
			if (isLocked)
			{
				this.DescriptionLocItem.SetString("LOC_ID_SKILL_LOCKED_DESCRIPTION_1");
				return;
			}
			if (isSoulLocked)
			{
				string arg = "NULL";
				SoulShopType soulShopType;
				if (Enum.TryParse<SoulShopType>(skillTreeData.SoulShopTag, out soulShopType))
				{
					arg = LocalizationManager.GetString(SoulShopLibrary.GetSoulShopData(soulShopType).Title, false, false);
				}
				this.DescriptionLocItem.SetString("LOC_ID_SKILL_SOUL_LOCKED_DESCRIPTION_1");
				this.DescriptionLocItem.TextObj.text = string.Format(this.DescriptionLocItem.TextObj.text, arg);
				return;
			}
			this.DescriptionLocItem.SetString(skillTreeData.Description);
			if (skillTreeObj.SkillTreeType == SkillTreeType.Weight_CD_Reduce)
			{
				int num = Mathf.RoundToInt((0.2f + SkillTreeManager.GetSkillTreeObj(SkillTreeType.Weight_CD_Reduce).CurrentStatGain) * 100f);
				this.DescriptionLocItem.TextObj.text = string.Format(this.DescriptionLocItem.TextObj.text, num);
			}
			if (isLevelLocked)
			{
				TMP_Text textObj = this.DescriptionLocItem.TextObj;
				textObj.text = textObj.text + "\n\n" + string.Format(LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_LEVEL_WARNING_1", false, false), skillTreeObj.UnlockLevel);
				return;
			}
			break;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Stat:
		{
			if (isLocked || isSoulLocked)
			{
				this.DescriptionLocItem.gameObject.SetActive(false);
				this.CurrentValue.gameObject.SetActive(false);
				return;
			}
			bool flag = CDGHelper.IsPercent(skillTreeObj.SkillTreeData.FirstLevelStatGain);
			this.DescriptionLocItem.SetString(skillTreeData.StatTitle);
			float currentStatGain = skillTreeObj.CurrentStatGain;
			string text;
			if (!flag)
			{
				if (skillTreeObj.SkillTreeType == SkillTreeType.Gold_Saved_Cap_Up)
				{
					text = (currentStatGain + 1000f).ToString();
				}
				else
				{
					text = currentStatGain.ToString();
				}
			}
			else
			{
				text = this.PercentString(currentStatGain);
			}
			float value = skillTreeObj.GetStatGainAtLevel(skillTreeObj.Level + 1) - currentStatGain;
			if (skillTreeObj.Level < skillTreeObj.MaxLevel)
			{
				this.CurrentValue.text = text + " " + this.ColoredValueString(value, true, flag, false, false);
				return;
			}
			this.CurrentValue.text = text;
			return;
		}
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Level:
			if (!isLocked && !isSoulLocked)
			{
				this.CurrentValue.text = string.Format("{0} / {1}", skillTreeObj.ClampedLevel, skillTreeObj.MaxLevel);
				return;
			}
			this.DescriptionLocItem.gameObject.SetActive(false);
			this.CurrentValue.gameObject.SetActive(false);
			return;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Cost:
			if (isLocked || isSoulLocked)
			{
				this.CurrentValue.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
				return;
			}
			if (skillTreeObj.Level < skillTreeObj.MaxLevel)
			{
				this.CurrentValue.text = skillTreeObj.GoldCostWithLevelAppreciation.ToString();
				return;
			}
			this.CurrentValue.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
			return;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.GoldOwned:
			this.CurrentValue.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
			return;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.CostMultiple:
			if (skillTreeObj.MaxLevel <= 1)
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				if (isLocked || isSoulLocked)
				{
					this.CurrentValue.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
					return;
				}
				int levelsToAdd = Mathf.Clamp(5, 0, skillTreeObj.MaxLevel - skillTreeObj.Level);
				int numLevelsPurchaseableWithGold = skillTreeObj.GetNumLevelsPurchaseableWithGold(levelsToAdd, SaveManager.PlayerSaveData.GoldCollectedIncludingBank);
				if (numLevelsPurchaseableWithGold > 0)
				{
					this.CurrentValue.text = skillTreeObj.GetGoldCostWithAppreciationWhenAddingLevels(numLevelsPurchaseableWithGold).ToString();
					this.DescriptionLocItem.TextObj.text = string.Format(LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_COST_MULTIPLE_1", false, false), numLevelsPurchaseableWithGold);
					return;
				}
				this.CurrentValue.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
				this.DescriptionLocItem.TextObj.text = string.Format(LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_COST_MULTIPLE_1", false, false), 0);
				return;
			}
			break;
		case SkillTreeDescriptionUpdater.SkillTreeDescriptionType.Purchase:
			if (skillTreeObj.ClampedLevel >= skillTreeObj.MaxLevel)
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				this.CurrentValue.gameObject.SetActive(false);
				if (!isLocked && !isSoulLocked)
				{
					int levelsToAdd2 = Mathf.Clamp(5, 0, skillTreeObj.MaxLevel - skillTreeObj.Level);
					int numLevelsPurchaseableWithGold2 = skillTreeObj.GetNumLevelsPurchaseableWithGold(levelsToAdd2, SaveManager.PlayerSaveData.GoldCollectedIncludingBank);
					this.DescriptionLocItem.TextObj.text = string.Format(LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_PURCHASE_MULTIPLE_1", false, false), numLevelsPurchaseableWithGold2);
					return;
				}
				this.DescriptionLocItem.gameObject.SetActive(false);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x000E3B65 File Offset: 0x000E1D65
	private void OnGoldChanged(MonoBehaviour sender, EventArgs args)
	{
		if (this.DescriptionType == SkillTreeDescriptionUpdater.SkillTreeDescriptionType.GoldOwned)
		{
			this.CurrentValue.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
		}
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x000E3B85 File Offset: 0x000E1D85
	protected string ColoredString(string text, Color color)
	{
		return string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(color),
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x000E3BB8 File Offset: 0x000E1DB8
	protected string PercentString(float value)
	{
		value *= 100f;
		string text = value.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
		if (text[text.Length - 1] == '0')
		{
			text = Mathf.RoundToInt(value).ToString();
		}
		return string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), text);
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x000E3C14 File Offset: 0x000E1E14
	protected string PlusSymbolString(float value, bool isPercent, bool addSymbolToZero = true)
	{
		string text = isPercent ? this.PercentString(value) : value.ToString();
		if (addSymbolToZero)
		{
			if (value < 0f)
			{
				return text;
			}
			return "+" + text;
		}
		else
		{
			if (value <= 0f)
			{
				return text;
			}
			return "+" + text;
		}
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x000E3C64 File Offset: 0x000E1E64
	protected string ColoredValueString(float value, bool addBrackets, bool isPercent, bool lowerIsBetter, bool hideZero = true)
	{
		if (hideZero && value == 0f)
		{
			return "";
		}
		Color color = this.OriginalColor;
		if (value != 0f)
		{
			if ((value > 0f && !lowerIsBetter) || (value < 0f && lowerIsBetter))
			{
				color = this.BenefitColor;
			}
			else
			{
				color = this.DeficitColor;
			}
		}
		string text = isPercent ? this.PercentString(value) : value.ToString();
		if (value >= 0f)
		{
			text = this.PlusSymbolString(value, isPercent, true);
		}
		if (addBrackets)
		{
			text = "(" + text + ")";
		}
		return this.ColoredString(text, color);
	}

	// Token: 0x04003181 RID: 12673
	protected Color BenefitColor = new Color(0.05882353f, 0.50980395f, 0f);

	// Token: 0x04003182 RID: 12674
	protected Color DeficitColor = new Color(0.8392157f, 0f, 0f);

	// Token: 0x04003183 RID: 12675
	protected Color OriginalColor = new Color(0.3254902f, 0.24705882f, 0.27450982f);

	// Token: 0x04003184 RID: 12676
	public SkillTreeDescriptionUpdater.SkillTreeDescriptionType DescriptionType;

	// Token: 0x04003185 RID: 12677
	public LocalizationItem DescriptionLocItem;

	// Token: 0x04003186 RID: 12678
	public TMP_Text CurrentValue;

	// Token: 0x04003187 RID: 12679
	private Action<MonoBehaviour, EventArgs> m_onHighlightedSkillChanged;

	// Token: 0x04003188 RID: 12680
	private Action<MonoBehaviour, EventArgs> m_onSkillLevelChanged;

	// Token: 0x04003189 RID: 12681
	private Action<MonoBehaviour, EventArgs> m_onGoldChanged;

	// Token: 0x02000E22 RID: 3618
	public enum SkillTreeDescriptionType
	{
		// Token: 0x040056D6 RID: 22230
		Title,
		// Token: 0x040056D7 RID: 22231
		Description,
		// Token: 0x040056D8 RID: 22232
		Stat,
		// Token: 0x040056D9 RID: 22233
		Level,
		// Token: 0x040056DA RID: 22234
		Cost,
		// Token: 0x040056DB RID: 22235
		GoldOwned,
		// Token: 0x040056DC RID: 22236
		CostMultiple,
		// Token: 0x040056DD RID: 22237
		Purchase
	}
}
