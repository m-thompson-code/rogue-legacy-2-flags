using System;
using System.Collections;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class PlayerDeathRankTextController : MonoBehaviour
{
	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06002422 RID: 9250 RVA: 0x00077258 File Offset: 0x00075458
	public TMP_Text CounterText
	{
		get
		{
			return this.m_rankCounterText;
		}
	}

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06002423 RID: 9251 RVA: 0x00077260 File Offset: 0x00075460
	public TMP_Text Text
	{
		get
		{
			return this.m_rankText;
		}
	}

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x06002424 RID: 9252 RVA: 0x00077268 File Offset: 0x00075468
	// (set) Token: 0x06002425 RID: 9253 RVA: 0x00077270 File Offset: 0x00075470
	public bool IsComplete { get; private set; }

	// Token: 0x06002426 RID: 9254 RVA: 0x0007727C File Offset: 0x0007547C
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

	// Token: 0x06002427 RID: 9255 RVA: 0x000773AB File Offset: 0x000755AB
	public void StartTally()
	{
		this.IsComplete = false;
		this.m_tallyStarted = true;
		base.StartCoroutine(this.RankCoroutineV2());
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000773C8 File Offset: 0x000755C8
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

	// Token: 0x06002429 RID: 9257 RVA: 0x000773D8 File Offset: 0x000755D8
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

	// Token: 0x0600242A RID: 9258 RVA: 0x0007750C File Offset: 0x0007570C
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

	// Token: 0x0600242B RID: 9259 RVA: 0x000776B4 File Offset: 0x000758B4
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

	// Token: 0x0600242C RID: 9260 RVA: 0x000777E0 File Offset: 0x000759E0
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

	// Token: 0x0600242D RID: 9261 RVA: 0x00077834 File Offset: 0x00075A34
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

	// Token: 0x04001E9E RID: 7838
	[SerializeField]
	private TMP_Text m_rankText;

	// Token: 0x04001E9F RID: 7839
	[SerializeField]
	private TMP_Text m_rankCounterText;

	// Token: 0x04001EA0 RID: 7840
	private int m_currentRank;

	// Token: 0x04001EA1 RID: 7841
	private bool m_tallyStarted;

	// Token: 0x04001EA2 RID: 7842
	public Relay<int, int> XPGainedRelay = new Relay<int, int>();

	// Token: 0x04001EA3 RID: 7843
	public Relay<int> LevelGainedRelay = new Relay<int>();

	// Token: 0x04001EA4 RID: 7844
	public Relay XPGainStartRelay = new Relay();

	// Token: 0x04001EA5 RID: 7845
	public Relay XPGainCompleteRelay = new Relay();
}
