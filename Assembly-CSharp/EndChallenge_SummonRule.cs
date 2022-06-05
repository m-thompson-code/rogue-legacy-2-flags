using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x0200051F RID: 1311
[Serializable]
public class EndChallenge_SummonRule : BaseSummonRule
{
	// Token: 0x170011E2 RID: 4578
	// (get) Token: 0x0600307F RID: 12415 RVA: 0x000A5CE2 File Offset: 0x000A3EE2
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.EndChallenge;
		}
	}

	// Token: 0x170011E3 RID: 4579
	// (get) Token: 0x06003080 RID: 12416 RVA: 0x000A5CE9 File Offset: 0x000A3EE9
	public override Color BoxColor
	{
		get
		{
			return Color.red;
		}
	}

	// Token: 0x170011E4 RID: 4580
	// (get) Token: 0x06003081 RID: 12417 RVA: 0x000A5CF0 File Offset: 0x000A3EF0
	public override string RuleLabel
	{
		get
		{
			return "End Challenge";
		}
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000A5CF7 File Offset: 0x000A3EF7
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

	// Token: 0x04002686 RID: 9862
	[SerializeField]
	private ChallengeType m_challengeType;
}
