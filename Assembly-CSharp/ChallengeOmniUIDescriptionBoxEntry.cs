using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200039B RID: 923
public class ChallengeOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<ChallengeOmniUIDescriptionEventArgs, ChallengeOmniUIDescriptionBoxEntry.ChallengeOmniUIDescriptionBoxType>
{
	// Token: 0x0600226F RID: 8815 RVA: 0x0006F72C File Offset: 0x0006D92C
	protected override void DisplayDescriptionBox(ChallengeOmniUIDescriptionEventArgs args)
	{
		ChallengeType challengeType = args.ChallengeType;
		ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
		if (challenge == null)
		{
			return;
		}
		ChallengeData challengeData = challenge.ChallengeData;
		bool flag = challenge.FoundState > FoundState.NotFound;
		bool flag2 = Challenge_EV.ScarBossRequirementTable.ContainsKey(challengeType) && SaveManager.PlayerSaveData.GetFlag(Challenge_EV.ScarBossRequirementTable[challengeType]);
		switch (this.m_descriptionType)
		{
		case ChallengeOmniUIDescriptionBoxEntry.ChallengeOmniUIDescriptionBoxType.Title:
			if (flag)
			{
				if (this.m_iconGO && !this.m_iconGO.activeSelf)
				{
					this.m_iconGO.SetActive(true);
				}
				if (this.m_inactiveIconGO && this.m_inactiveIconGO.activeSelf)
				{
					this.m_inactiveIconGO.SetActive(false);
				}
				this.m_icon.sprite = IconLibrary.GetChallengeIcon(challengeType, ChallengeLibrary.ChallengeIconEntryType.Challenge);
			}
			else
			{
				if (this.m_iconGO && this.m_iconGO.activeSelf)
				{
					this.m_iconGO.SetActive(false);
				}
				if (this.m_inactiveIconGO && !this.m_inactiveIconGO.activeSelf)
				{
					this.m_inactiveIconGO.SetActive(true);
				}
				this.m_icon.sprite = IconLibrary.GetDefaultSprite();
			}
			if (this.m_frame)
			{
				switch (ChallengeManager.GetChallengeTrophyRank(challengeType, true))
				{
				case ChallengeTrophyRank.Bronze:
					this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Bronze);
					break;
				case ChallengeTrophyRank.Silver:
					this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Silver);
					break;
				case ChallengeTrophyRank.Gold:
					this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Gold);
					break;
				default:
					this.m_frame.sprite = IconLibrary.GetMiscIcon(MiscIconType.OmniUIEntry_Frame_Default);
					break;
				}
			}
			if (flag)
			{
				this.m_titleText.text = LocalizationManager.GetString(challengeData.Title, false, false);
				return;
			}
			this.m_titleText.text = "???";
			return;
		case ChallengeOmniUIDescriptionBoxEntry.ChallengeOmniUIDescriptionBoxType.Description:
			if (flag)
			{
				this.m_titleText.text = LocalizationManager.GetString(challengeData.Description, false, false);
				return;
			}
			if (flag2)
			{
				this.m_titleText.text = LocalizationManager.GetString(challengeData.Hint, false, false);
				return;
			}
			this.m_titleText.text = LocalizationManager.GetString(challengeData.BossRequirementHint, false, false);
			return;
		case ChallengeOmniUIDescriptionBoxEntry.ChallengeOmniUIDescriptionBoxType.Controls:
		{
			if (!flag)
			{
				this.m_titleText.text = "";
				return;
			}
			ClassType classType = challenge.ChallengeData.ClassOverride;
			if (classType == ClassType.None)
			{
				classType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
			}
			float num = (float)ChallengeManager.GetChallengeClassHighScore(challengeType, classType);
			string @string = LocalizationManager.GetString(ClassLibrary.GetClassData(classType).PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			float num2 = challengeData.ScalingHandicap * (float)challenge.EquippedLevel;
			if (challenge.ChallengeData.ScoringType == ChallengeScoringType.Battle)
			{
				num2 *= 100f;
			}
			string text = LocalizationManager.GetString("LOC_ID_CHALLENGE_UI_END_DEFAULT_TIME_1", false, false);
			float bestTimeWithoutHandicaps = challenge.BestTimeWithoutHandicaps;
			if (bestTimeWithoutHandicaps != 3.4028235E+38f)
			{
				text = this.ToTimeFormat(bestTimeWithoutHandicaps);
			}
			ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(challengeType, true);
			string locID = "";
			int challengeSoulsCollected = Souls_EV.GetChallengeSoulsCollected(challengeType);
			int num3 = 0;
			string locID2;
			int num4;
			string locID3;
			float elapsedSeconds;
			if (challengeTrophyRank == ChallengeTrophyRank.None)
			{
				locID2 = "LOC_ID_CHALLENGE_UI_BRONZE_REQ_1";
				num4 = challengeData.BronzeReq;
				locID3 = "LOC_ID_CHALLENGE_UI_BRONZE_TIME_1";
				elapsedSeconds = challengeData.ParTime + 10f;
				locID = "LOC_ID_CHALLENGE_UI_BRONZE_REWARD_1";
				num3 = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Bronze) - challengeSoulsCollected;
			}
			else if (challengeTrophyRank == ChallengeTrophyRank.Bronze)
			{
				locID2 = "LOC_ID_CHALLENGE_UI_SILVER_REQ_1";
				num4 = challengeData.SilverReq;
				locID3 = "LOC_ID_CHALLENGE_UI_SILVER_TIME_1";
				elapsedSeconds = challengeData.ParTime + 3f;
				locID = "LOC_ID_CHALLENGE_UI_SILVER_REWARD_1";
				num3 = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Silver) - challengeSoulsCollected;
			}
			else
			{
				locID2 = "LOC_ID_CHALLENGE_UI_GOLD_REQ_1";
				num4 = challengeData.GoldReq;
				locID3 = "LOC_ID_CHALLENGE_UI_GOLD_TIME_1";
				elapsedSeconds = challengeData.ParTime + 0f;
				if (challengeTrophyRank == ChallengeTrophyRank.Silver)
				{
					locID = "LOC_ID_CHALLENGE_UI_GOLD_REWARD_1";
					num3 = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Gold) - challengeSoulsCollected;
				}
				else if (challengeTrophyRank == ChallengeTrophyRank.Gold)
				{
					locID = "LOC_ID_CHALLENGE_UI_MAX_REWARD_1";
					num3 = Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeTrophyRank.Gold);
				}
			}
			string text2 = string.Format(LocalizationManager.GetString(locID, false, true), num3);
			string text3 = string.Format(LocalizationManager.GetString(locID3, false, true), this.ToTimeFormat(elapsedSeconds));
			if (challengeType == ChallengeType.Tutorial || challengeType == ChallengeType.TutorialPurified)
			{
				this.m_titleText.text = string.Format(LocalizationManager.GetString(challengeData.Controls, false, false), ChallengeManager.GetTotalTrophyCount() - 1);
				return;
			}
			this.m_titleText.text = string.Format(LocalizationManager.GetString(challengeData.Controls, false, false), new object[]
			{
				challengeData.BaseHandicap + 15,
				num2,
				LocalizationManager.GetString(locID2, false, false),
				num4,
				"",
				challenge.TotalHighScore,
				@string,
				num,
				ChallengeManager.GetChallengeLetterGrade(challengeType, num),
				text,
				text2,
				text3
			});
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x0006FC08 File Offset: 0x0006DE08
	private string ToTimeFormat(float elapsedSeconds)
	{
		int num = (int)(elapsedSeconds / 3600f);
		int num2 = (int)(elapsedSeconds % 3600f / 60f);
		int num3 = (int)(elapsedSeconds % 60f);
		int num4 = (int)((elapsedSeconds - (float)((int)elapsedSeconds)) * 100f);
		string result;
		if (num == 0)
		{
			result = string.Format("{0:D2}:{1:D2}:{2:D2}", num2, num3, num4);
		}
		else
		{
			result = string.Format("{0}:{1:D2}:{2:D2}:{3:D2}", new object[]
			{
				num,
				num2,
				num3,
				num4
			});
		}
		return result;
	}

	// Token: 0x04001DBD RID: 7613
	[SerializeField]
	private Image m_frame;

	// Token: 0x02000C0A RID: 3082
	public enum ChallengeOmniUIDescriptionBoxType
	{
		// Token: 0x04004EA9 RID: 20137
		None,
		// Token: 0x04004EAA RID: 20138
		Title,
		// Token: 0x04004EAB RID: 20139
		Description,
		// Token: 0x04004EAC RID: 20140
		Controls
	}
}
