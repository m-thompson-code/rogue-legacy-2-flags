using System;
using TMPro;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class ChallengeOmniUIScoreboardEntry : MonoBehaviour
{
	// Token: 0x0600228F RID: 8847 RVA: 0x0007027C File Offset: 0x0006E47C
	public void UpdateEntry(ChallengeType challengeType)
	{
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge != null)
		{
			if (this.m_isTotalScore)
			{
				this.m_valueText.text = challenge.TotalHighScore.ToString();
				return;
			}
			if (this.m_isTrophyEntry)
			{
				ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(challengeType, true);
				int challengeSoulsRewarded = Souls_EV.GetChallengeSoulsRewarded(challengeType, challengeTrophyRank);
				string @string;
				int num;
				switch (challengeTrophyRank)
				{
				default:
					@string = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_BRONZE_REWARD_1", false, false);
					num = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Bronze) - challengeSoulsRewarded;
					break;
				case ChallengeTrophyRank.Bronze:
					@string = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_SILVER_REWARD_1", false, false);
					num = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Silver) - challengeSoulsRewarded;
					break;
				case ChallengeTrophyRank.Silver:
					@string = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_GOLD_REWARD_1", false, false);
					num = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Gold) - challengeSoulsRewarded;
					break;
				case ChallengeTrophyRank.Gold:
					@string = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_MAX_REWARD_1", false, false);
					num = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Gold);
					break;
				}
				this.m_titleText.text = string.Format(@string, num);
				return;
			}
			ClassData classData = ClassLibrary.GetClassData(this.m_scoreboardClassType);
			if (classData != null)
			{
				if (SkillTreeLogicHelper.IsClassUnlocked(this.m_scoreboardClassType))
				{
					this.m_titleText.text = LocalizationManager.GetString(classData.PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					this.m_valueText.text = ChallengeManager.GetChallengeClassHighScore(challengeType, this.m_scoreboardClassType).ToString();
					return;
				}
				this.m_titleText.text = "???????";
				this.m_valueText.text = "0";
			}
		}
	}

	// Token: 0x04001DCB RID: 7627
	[SerializeField]
	private ClassType m_scoreboardClassType;

	// Token: 0x04001DCC RID: 7628
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001DCD RID: 7629
	[SerializeField]
	private TMP_Text m_valueText;

	// Token: 0x04001DCE RID: 7630
	[SerializeField]
	private bool m_isTotalScore;

	// Token: 0x04001DCF RID: 7631
	[SerializeField]
	private bool m_isTrophyEntry;
}
