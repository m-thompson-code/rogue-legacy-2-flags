using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000897 RID: 2199
[Serializable]
public class EndChallenge_SummonRule : BaseSummonRule
{
	// Token: 0x170017FF RID: 6143
	// (get) Token: 0x06004353 RID: 17235 RVA: 0x0002537C File Offset: 0x0002357C
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.EndChallenge;
		}
	}

	// Token: 0x17001800 RID: 6144
	// (get) Token: 0x06004354 RID: 17236 RVA: 0x00025383 File Offset: 0x00023583
	public override Color BoxColor
	{
		get
		{
			return Color.red;
		}
	}

	// Token: 0x17001801 RID: 6145
	// (get) Token: 0x06004355 RID: 17237 RVA: 0x0002538A File Offset: 0x0002358A
	public override string RuleLabel
	{
		get
		{
			return "End Challenge";
		}
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x00025391 File Offset: 0x00023591
	public override IEnumerator RunSummonRule()
	{
		MusicManager.StopMusic();
		float delayTime = Time.time + 3f;
		while (Time.time < delayTime)
		{
			yield return null;
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.ChallengeComplete))
		{
			WindowManager.LoadWindow(WindowID.ChallengeComplete);
		}
		WindowManager.SetWindowIsOpen(WindowID.ChallengeComplete, true);
		while (WindowManager.GetIsWindowOpen(WindowID.ChallengeComplete))
		{
			yield return null;
		}
		base.SummonController.StopArena(true);
		base.IsRuleComplete = true;
		if (ChallengeManager.ActiveChallenge != null && ChallengeManager.ActiveChallenge.ChallengeType == ChallengeType.TutorialPurified)
		{
			ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(ChallengeType.TutorialPurified, false);
			if (challengeTrophyRank == ChallengeTrophyRank.Gold && !SaveManager.ModeSaveData.HasGoldSisyphusTrophy)
			{
				SaveManager.ModeSaveData.HasGoldSisyphusTrophy = true;
			}
			else if (challengeTrophyRank == ChallengeTrophyRank.Silver && !SaveManager.ModeSaveData.HasSilverSisyphusTrophy)
			{
				SaveManager.ModeSaveData.HasSilverSisyphusTrophy = true;
			}
			else if (challengeTrophyRank == ChallengeTrophyRank.Bronze && !SaveManager.ModeSaveData.HasBronzeSisyphusTrophy)
			{
				SaveManager.ModeSaveData.HasBronzeSisyphusTrophy = true;
			}
		}
		ChallengeManager.ReturnToDriftHouseWithTransition();
		yield break;
	}

	// Token: 0x0400347A RID: 13434
	[SerializeField]
	private ChallengeType m_challengeType;
}
