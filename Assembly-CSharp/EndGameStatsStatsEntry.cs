using System;
using TMPro;
using UnityEngine;

// Token: 0x0200095C RID: 2396
public class EndGameStatsStatsEntry : MonoBehaviour
{
	// Token: 0x060048CF RID: 18639 RVA: 0x0011A13C File Offset: 0x0011833C
	public void UpdateStat(bool animate)
	{
		string text = null;
		int num = this.CalculateScore(this.m_statsEntryType);
		switch (this.m_statsEntryType)
		{
		case EndGameStatsEntryType.HeirsLost:
		{
			int num2 = SaveManager.PlayerSaveData.TimesDied;
			text = num2.ToString();
			break;
		}
		case EndGameStatsEntryType.EnemiesDefeated:
		{
			int num2 = SaveManager.PlayerSaveData.EnemiesKilled;
			text = num2.ToString();
			break;
		}
		case EndGameStatsEntryType.GoldSpent:
		{
			int num2 = SaveManager.PlayerSaveData.GoldSpent;
			text = num2.ToString() + " g";
			break;
		}
		case EndGameStatsEntryType.TimePlayed:
			text = TimeSpan.FromSeconds((double)(SaveManager.PlayerSaveData.SecondsPlayed + GameTimer.TotalSessionAccumulatedTime)).ToString("hh\\:mm\\:ss");
			break;
		case EndGameStatsEntryType.FinalScore:
			this.m_score.text = num.ToString();
			return;
		case EndGameStatsEntryType.Rating:
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
			this.m_score.text = "+" + num.ToString();
		}
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x0011A274 File Offset: 0x00118474
	private int CalculateScore(EndGameStatsEntryType statType)
	{
		switch (statType)
		{
		case EndGameStatsEntryType.HeirsLost:
		{
			int num = 25000;
			int num2 = 625;
			int num3 = SaveManager.PlayerSaveData.TimesDied * num2;
			return Mathf.Clamp(num - num3, 1000, num);
		}
		case EndGameStatsEntryType.EnemiesDefeated:
		{
			int num4 = 25000;
			int num5 = (SaveManager.PlayerSaveData.TimesDied + 1) * 30;
			int enemiesKilled = SaveManager.PlayerSaveData.EnemiesKilled;
			int num6 = 150;
			int num7 = Mathf.Max(num5 - enemiesKilled, 0) * num6;
			return Mathf.Clamp(num4 - num7, 1000, num4);
		}
		case EndGameStatsEntryType.GoldSpent:
		{
			int num8 = 25000;
			int num9 = (int)((float)SaveManager.PlayerSaveData.GoldSpent * 0.5f);
			return Mathf.Clamp(num8 - num9, 1000, num8);
		}
		case EndGameStatsEntryType.TimePlayed:
		{
			int num10 = 25000;
			int num11 = (int)((SaveManager.PlayerSaveData.SecondsPlayed + GameTimer.TotalSessionAccumulatedTime) * 0.00027777778f * 1650f);
			return Mathf.Clamp(num10 - num11, 1000, num10);
		}
		case EndGameStatsEntryType.FinalScore:
		{
			int num12 = 0;
			foreach (object obj in Enum.GetValues(typeof(EndGameStatsEntryType)))
			{
				EndGameStatsEntryType endGameStatsEntryType = (EndGameStatsEntryType)obj;
				if (endGameStatsEntryType != statType)
				{
					num12 += this.CalculateScore(endGameStatsEntryType);
				}
			}
			return num12;
		}
		default:
			return 0;
		}
	}

	// Token: 0x060048D1 RID: 18641 RVA: 0x0011A3E4 File Offset: 0x001185E4
	private string CalculateLetterGrade()
	{
		int num = 100000;
		float num2 = (float)this.CalculateScore(EndGameStatsEntryType.FinalScore);
		int num3 = EndGameStatsStatsEntry.m_letterGradeArray.Length;
		int num4 = Mathf.Clamp((int)(num2 / (float)num * (float)num3), 0, num3 - 1);
		return EndGameStatsStatsEntry.m_letterGradeArray[num4];
	}

	// Token: 0x060048D2 RID: 18642 RVA: 0x0011A420 File Offset: 0x00118620
	private string CalculateRating()
	{
		int num = 100000;
		float num2 = (float)this.CalculateScore(EndGameStatsEntryType.FinalScore);
		int num3 = EndGameStatsStatsEntry.m_ratingArray.Length;
		int num4 = Mathf.Clamp((int)(num2 / (float)num * (float)num3), 0, num3 - 1);
		return EndGameStatsStatsEntry.m_ratingArray[num4];
	}

	// Token: 0x040037CC RID: 14284
	[SerializeField]
	private EndGameStatsEntryType m_statsEntryType;

	// Token: 0x040037CD RID: 14285
	[SerializeField]
	private TMP_Text m_amount;

	// Token: 0x040037CE RID: 14286
	[SerializeField]
	private TMP_Text m_score;

	// Token: 0x040037CF RID: 14287
	private static string[] m_letterGradeArray = new string[]
	{
		"C",
		"C",
		"C+",
		"C+",
		"C+",
		"C++",
		"C++",
		"C++",
		"B",
		"B",
		"B+",
		"B+",
		"B++",
		"B++",
		"A",
		"A+",
		"A++",
		"S",
		"S+",
		"S++"
	};

	// Token: 0x040037D0 RID: 14288
	private static string[] m_ratingArray = new string[]
	{
		"Amateur Adventurer",
		"Enthusiastic Explorer",
		"Intrepid Traveler",
		"A True Rogue"
	};
}
