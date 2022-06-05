using System;
using TMPro;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class ChallengeOmniUIScoreboardEntry : MonoBehaviour
{
	// Token: 0x060030A7 RID: 12455 RVA: 0x000D0E40 File Offset: 0x000CF040
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

	// Token: 0x040027E4 RID: 10212
	[SerializeField]
	private ClassType m_scoreboardClassType;

	// Token: 0x040027E5 RID: 10213
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040027E6 RID: 10214
	[SerializeField]
	private TMP_Text m_valueText;

	// Token: 0x040027E7 RID: 10215
	[SerializeField]
	private bool m_isTotalScore;

	// Token: 0x040027E8 RID: 10216
	[SerializeField]
	private bool m_isTrophyEntry;
}
