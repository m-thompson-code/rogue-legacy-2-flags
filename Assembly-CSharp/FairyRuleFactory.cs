using System;
using UnityEngine;

// Token: 0x02000701 RID: 1793
public static class FairyRuleFactory
{
	// Token: 0x060036C5 RID: 14021 RVA: 0x000E4C24 File Offset: 0x000E2E24
	public static FairyRule AddFairyRule(FairyRuleID fairyRuleID, GameObject gameObject)
	{
		FairyRule result = null;
		if (fairyRuleID <= FairyRuleID.KillAllEnemies)
		{
			if (fairyRuleID <= FairyRuleID.DontTakeDamage)
			{
				if (fairyRuleID == FairyRuleID.TimeLimit)
				{
					return gameObject.AddComponent<TimeLimit_FairyRule>();
				}
				if (fairyRuleID == FairyRuleID.DontGetHit)
				{
					return gameObject.AddComponent<DontGetHit_FairyRule>();
				}
				if (fairyRuleID == FairyRuleID.DontTakeDamage)
				{
					return gameObject.AddComponent<DontTakeDamage_FairyRule>();
				}
			}
			else
			{
				if (fairyRuleID == FairyRuleID.NoJumping)
				{
					return gameObject.AddComponent<NoJumping_FairyRule>();
				}
				if (fairyRuleID == FairyRuleID.NoDoubleJump)
				{
					return gameObject.AddComponent<NoDoubleJump_FairyRule>();
				}
				if (fairyRuleID == FairyRuleID.KillAllEnemies)
				{
					return gameObject.AddComponent<KillAllEnemies_FairyRule>();
				}
			}
		}
		else if (fairyRuleID <= FairyRuleID.NoAttacking)
		{
			if (fairyRuleID == FairyRuleID.HiddenChest)
			{
				return gameObject.AddComponent<HiddenChest_FairyRule>();
			}
			if (fairyRuleID == FairyRuleID.NoDash)
			{
				return gameObject.AddComponent<NoDash_FairyRule>();
			}
			if (fairyRuleID == FairyRuleID.NoAttacking)
			{
				return gameObject.AddComponent<NoAttacking_FairyRule>();
			}
		}
		else
		{
			if (fairyRuleID == FairyRuleID.Puzzle)
			{
				return gameObject.AddComponent<Puzzle_FairyRule>();
			}
			if (fairyRuleID == FairyRuleID.DestroyAllTargets)
			{
				return gameObject.AddComponent<DestroyAllTargets_FairyRule>();
			}
			if (fairyRuleID == FairyRuleID.ReachMe)
			{
				return gameObject.AddComponent<ReachMe_FairyRule>();
			}
		}
		if (fairyRuleID != FairyRuleID.None)
		{
			Debug.LogFormat("<color=red>[FairyRuleFactory] No case found for ({0})</color>", new object[]
			{
				fairyRuleID
			});
		}
		return result;
	}
}
