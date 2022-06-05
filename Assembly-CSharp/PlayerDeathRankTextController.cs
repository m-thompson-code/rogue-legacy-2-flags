using System;
using System.Collections;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x02000673 RID: 1651
public class PlayerDeathRankTextController : MonoBehaviour
{
	// Token: 0x17001362 RID: 4962
	// (get) Token: 0x06003246 RID: 12870 RVA: 0x0001B979 File Offset: 0x00019B79
	public TMP_Text CounterText
	{
		get
		{
			return this.m_rankCounterText;
		}
	}

	// Token: 0x17001363 RID: 4963
	// (get) Token: 0x06003247 RID: 12871 RVA: 0x0001B981 File Offset: 0x00019B81
	public TMP_Text Text
	{
		get
		{
			return this.m_rankText;
		}
	}

	// Token: 0x17001364 RID: 4964
	// (get) Token: 0x06003248 RID: 12872 RVA: 0x0001B989 File Offset: 0x00019B89
	// (set) Token: 0x06003249 RID: 12873 RVA: 0x0001B991 File Offset: 0x00019B91
	public bool IsComplete { get; private set; }

	// Token: 0x0600324A RID: 12874 RVA: 0x000D70BC File Offset: 0x000D52BC
	public void UpdateMessage()
	{
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		this.m_currentRank = SaveManager.PlayerSaveData.GetClassMasteryRank(classType);
		int classXP = SaveManager.PlayerSaveData.GetClassXP(classType);
		int num = Mastery_EV.XP_REQUIRED[this.m_currentRank];
		string arg = "NULL CLASS";
		ClassData classData = ClassLibrary.GetClassData(classType);
		if (classData != null)
		{
			arg = LocalizationManager.GetString(classData.PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		if (Mastery_EV.IsMaxMasteryRank(classType, 0, false))
		{
			int num2 = Mastery_EV.XP_REQUIRED[Mastery_EV.GetMaxMasteryRank()];
			this.m_rankCounterText.text = num2.ToString() + "/" + num2.ToString();
			this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_MAX_RANK_FORMATTER_1", false, false), arg, this.m_currentRank);
			return;
		}
		this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_RANK_FORMATTER_1", false, false), arg, this.m_currentRank);
		this.m_rankCounterText.text = classXP.ToString() + "/" + num.ToString();
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x0001B99A File Offset: 0x00019B9A
	public void StartTally()
	{
		this.IsComplete = false;
		this.m_tallyStarted = true;
		base.StartCoroutine(this.RankCoroutineV2());
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x0001B9B7 File Offset: 0x00019BB7
	private IEnumerator RankCoroutineV2()
	{
		this.IsComplete = false;
		if (Mastery_EV.IsMaxMasteryRank(SaveManager.PlayerSaveData.CurrentCharacter.ClassType, 0, false))
		{
			this.RankComplete();
			yield break;
		}
		this.XPGainStartRelay.Dispatch();
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		this.m_currentRank = SaveManager.PlayerSaveData.GetClassMasteryRank(classType);
		if (this.m_currentRank <= Mastery_EV.GetMaxMasteryRank())
		{
			float elapsedDuration = 0.016666668f;
			float startTime = Time.unscaledTime;
			int currentXP = SaveManager.PlayerSaveData.GetClassXP(classType);
			int endXP = currentXP + SaveManager.PlayerSaveData.RunAccumulatedXP;
			int xpRequired = Mastery_EV.XP_REQUIRED[this.m_currentRank];
			string className = "NULL CLASS";
			ClassData classData = ClassLibrary.GetClassData(classType);
			if (classData)
			{
				className = LocalizationManager.GetString(classData.PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_RANK_FORMATTER_1", false, false), className, this.m_currentRank);
			int num = currentXP;
			int xpTally = num;
			this.m_rankCounterText.text = num.ToString() + "/" + xpRequired.ToString();
			this.ApplyXPToSaveFile();
			while (xpTally < endXP)
			{
				startTime = Time.unscaledTime;
				int num2 = (int)(1000f * elapsedDuration);
				xpTally += num2;
				if (xpTally >= xpRequired)
				{
					if (currentXP + xpTally >= Mastery_EV.XP_REQUIRED[Mastery_EV.GetMaxMasteryRank()])
					{
						this.RankUpAnim(1);
						break;
					}
					this.m_currentRank++;
					xpRequired = Mastery_EV.XP_REQUIRED[this.m_currentRank];
					this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_RANK_FORMATTER_1", false, false), className, this.m_currentRank);
					this.RankUpAnim(1);
				}
				this.m_rankCounterText.text = xpTally.ToString() + "/" + xpRequired.ToString();
				this.XPGainedRelay.Dispatch(xpTally, xpRequired);
				while (Time.unscaledTime < startTime + elapsedDuration)
				{
					yield return null;
				}
			}
			className = null;
		}
		this.RankComplete();
		yield break;
	}

	// Token: 0x0600324D RID: 12877 RVA: 0x000D71EC File Offset: 0x000D53EC
	private void RankComplete()
	{
		this.IsComplete = true;
		this.m_tallyStarted = false;
		this.XPGainCompleteRelay.Dispatch();
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		int classMasteryRank = SaveManager.PlayerSaveData.GetClassMasteryRank(classType);
		int classXP = SaveManager.PlayerSaveData.GetClassXP(classType);
		string arg = "NULL CLASS";
		ClassData classData = ClassLibrary.GetClassData(classType);
		if (classData)
		{
			arg = LocalizationManager.GetString(classData.PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		if (Mastery_EV.IsMaxMasteryRank(classType, 0, true))
		{
			int num = Mastery_EV.XP_REQUIRED[Mastery_EV.GetMaxMasteryRank()];
			this.m_rankCounterText.text = num.ToString() + "/" + num.ToString();
			this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_MAX_RANK_FORMATTER_1", false, false), arg, classMasteryRank);
			return;
		}
		int num2 = Mastery_EV.XP_REQUIRED[classMasteryRank];
		this.m_rankCounterText.text = classXP.ToString() + "/" + num2.ToString();
		this.m_rankText.text = string.Format(LocalizationManager.GetString("LOC_ID_RANK_RANK_FORMATTER_1", false, false), arg, classMasteryRank);
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x000D7320 File Offset: 0x000D5520
	private void RankUpAnim(int numRanks)
	{
		Vector3 localScale = this.m_rankText.transform.localScale;
		this.m_rankText.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		TweenManager.TweenTo_UnscaledTime(this.m_rankText.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			localScale.x,
			"localScale.y",
			localScale.y,
			"localScale.z",
			localScale.z
		});
		localScale = this.m_rankCounterText.transform.localScale;
		this.m_rankCounterText.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		TweenManager.TweenTo_UnscaledTime(this.m_rankCounterText.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			localScale.x,
			"localScale.y",
			localScale.y,
			"localScale.z",
			localScale.z
		});
		if (numRanks > 0)
		{
			this.LevelGainedRelay.Dispatch(this.m_currentRank);
			EffectManager.PlayEffect(base.gameObject, null, "PlayerRankUp_Effect", this.m_rankText.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.DisplayRankUpText(numRanks);
		}
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x000D74C8 File Offset: 0x000D56C8
	private void DisplayRankUpText(int numRanks)
	{
		string text = LocalizationManager.GetString("LOC_ID_RANK_RANK_UP_1", false, false) + "\n";
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		MasteryBonusType key = MasteryBonusType.None;
		if (Mastery_EV.MasteryBonusTypeTable.TryGetValue(classType, out key))
		{
			string locID;
			if (Mastery_EV.MasteryBonusLocIDTable.TryGetValue(key, out locID))
			{
				text += LocalizationManager.GetString(locID, false, false);
			}
			else
			{
				text = text + key.ToString() + " NOT FOUND IN LOC ID TABLE.";
			}
			float num;
			if (Mastery_EV.MasteryBonusAmountTable.TryGetValue(key, out num))
			{
				num = (float)numRanks * num;
				if (num != (float)((int)num))
				{
					num = (float)Mathf.CeilToInt(num * 100f);
					text = text + " +" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num);
				}
				else
				{
					text = text + " +" + num.ToString();
				}
			}
		}
		else
		{
			text = text + key.ToString() + " NOT FOUND IN TYPE TABLE.";
		}
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerRankUp, text, this.m_rankText.transform.position, null, TextAlignmentOptions.Center);
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x000D75F4 File Offset: 0x000D57F4
	public void SkipTally()
	{
		if (!this.IsComplete && this.m_tallyStarted)
		{
			base.StopAllCoroutines();
			ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
			int classMasteryRank = SaveManager.PlayerSaveData.GetClassMasteryRank(classType);
			this.RankUpAnim(classMasteryRank - this.m_currentRank);
			this.RankComplete();
		}
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x000D7648 File Offset: 0x000D5848
	private void ApplyXPToSaveFile()
	{
		ClassType classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		SaveManager.PlayerSaveData.SetClassXP(classType, SaveManager.PlayerSaveData.RunAccumulatedXP, true, false, false);
		SaveManager.PlayerSaveData.RunAccumulatedXP = 0;
		if (Mastery_EV.GetTotalMasteryRank() >= 150)
		{
			StoreAPIManager.GiveAchievement(AchievementType.UnlockHighMastery, StoreType.All);
		}
	}

	// Token: 0x04002903 RID: 10499
	[SerializeField]
	private TMP_Text m_rankText;

	// Token: 0x04002904 RID: 10500
	[SerializeField]
	private TMP_Text m_rankCounterText;

	// Token: 0x04002905 RID: 10501
	private int m_currentRank;

	// Token: 0x04002906 RID: 10502
	private bool m_tallyStarted;

	// Token: 0x04002907 RID: 10503
	public Relay<int, int> XPGainedRelay = new Relay<int, int>();

	// Token: 0x04002908 RID: 10504
	public Relay<int> LevelGainedRelay = new Relay<int>();

	// Token: 0x04002909 RID: 10505
	public Relay XPGainStartRelay = new Relay();

	// Token: 0x0400290A RID: 10506
	public Relay XPGainCompleteRelay = new Relay();
}
