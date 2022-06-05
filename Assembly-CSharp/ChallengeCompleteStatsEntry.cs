using System;
using TMPro;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public class ChallengeCompleteStatsEntry : MonoBehaviour
{
	// Token: 0x0600332E RID: 13102 RVA: 0x000ACF0C File Offset: 0x000AB10C
	public void UpdateStat(ChallengeTrophyRank goalRank)
	{
		string text = null;
		float num = ChallengeManager.CalculateActiveChallengeScore(this.m_statsEntryType);
		switch (this.m_statsEntryType)
		{
		case ChallengeCompleteStatsEntryType.BaseScore:
			this.m_score.text = 2500.ToString();
			return;
		case ChallengeCompleteStatsEntryType.HitsTaken:
			text = ChallengeManager.HitsTaken.ToString();
			break;
		case ChallengeCompleteStatsEntryType.Resolve:
			text = Mathf.RoundToInt(SaveManager.PlayerSaveData.GetTotalRelicResolveCost() * 100f).ToString();
			break;
		case ChallengeCompleteStatsEntryType.Timer:
			text = GlobalTimerHUDController.GetTimerString();
			break;
		case ChallengeCompleteStatsEntryType.ParTime:
		{
			float num2 = ChallengeManager.ActiveChallenge.ChallengeData.ParTime;
			if (ChallengeManager.ActiveChallenge.ScoringType == ChallengeScoringType.Platform)
			{
				if (goalRank == ChallengeTrophyRank.Bronze)
				{
					num2 += 10f;
				}
				else if (goalRank == ChallengeTrophyRank.Silver)
				{
					num2 += 3f;
				}
				else if (goalRank == ChallengeTrophyRank.Gold)
				{
					num2 += 0f;
				}
			}
			int num3 = (int)(num2 % 3600f / 60f);
			int num4 = (int)(num2 % 60f);
			int num5 = (int)((num2 - (float)((int)num2)) * 100f);
			text = string.Format(LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_PAR_TIME_1", false, false), num3, num4, num5);
			break;
		}
		case ChallengeCompleteStatsEntryType.HandicapMod:
			text = ChallengeManager.ActiveChallenge.EquippedLevel.ToString();
			break;
		case ChallengeCompleteStatsEntryType.FinalScore:
			this.m_score.text = num.ToString();
			return;
		case ChallengeCompleteStatsEntryType.Rating:
			this.m_amount.text = this.CalculateLetterGrade();
			this.m_score.text = this.CalculateRating();
			return;
		}
		if (this.m_amount != null)
		{
			this.m_amount.text = text;
		}
		if (this.m_score != null)
		{
			if (this.m_statsEntryType == ChallengeCompleteStatsEntryType.HandicapMod)
			{
				this.m_score.text = "x" + num.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
				return;
			}
			this.m_score.text = "+" + num.ToString();
		}
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x000AD107 File Offset: 0x000AB307
	private string CalculateLetterGrade()
	{
		return ChallengeManager.GetChallengeLetterGrade(ChallengeManager.ActiveChallenge.ChallengeType, ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.FinalScore));
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x000AD120 File Offset: 0x000AB320
	private string CalculateRating()
	{
		float num = ChallengeManager.CalculateActiveChallengeScore(ChallengeCompleteStatsEntryType.FinalScore);
		int num2 = ChallengeCompleteStatsEntry.m_ratingArray.Length;
		int num3 = Mathf.Clamp((int)(num / 50000f * (float)num2), 0, num2 - 1);
		return ChallengeCompleteStatsEntry.m_ratingArray[num3];
	}

	// Token: 0x040027F1 RID: 10225
	[SerializeField]
	private ChallengeCompleteStatsEntryType m_statsEntryType;

	// Token: 0x040027F2 RID: 10226
	[SerializeField]
	private TMP_Text m_amount;

	// Token: 0x040027F3 RID: 10227
	[SerializeField]
	private TMP_Text m_score;

	// Token: 0x040027F4 RID: 10228
	private static string[] m_ratingArray = new string[]
	{
		"Amateur Adventurer",
		"Enthusiastic Explorer",
		"Intrepid Traveler",
		"A True Rogue"
	};
}
