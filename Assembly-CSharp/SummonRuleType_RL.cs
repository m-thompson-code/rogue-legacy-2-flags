using System;

// Token: 0x02000782 RID: 1922
public class SummonRuleType_RL
{
	// Token: 0x06004158 RID: 16728 RVA: 0x000E8838 File Offset: 0x000E6A38
	public static BaseSummonRule GetSummonRule(SummonRuleType ruleType)
	{
		BaseSummonRule result = null;
		if (ruleType <= SummonRuleType.TogglePlayerInvincibility)
		{
			if (ruleType <= SummonRuleType.WaitForXSeconds)
			{
				if (ruleType <= SummonRuleType.SetSummonPool)
				{
					if (ruleType != SummonRuleType.SummonEnemies)
					{
						if (ruleType != SummonRuleType.SetSpawnPoints)
						{
							if (ruleType == SummonRuleType.SetSummonPool)
							{
								result = new SetSummonPool_SummonRule();
							}
						}
						else
						{
							result = new SetSpawnPoints_SummonRule();
						}
					}
					else
					{
						result = new SummonEnemy_SummonRule();
					}
				}
				else if (ruleType <= SummonRuleType.SetSummonPoolDifficulty)
				{
					if (ruleType != SummonRuleType.SetSummonPoolLevelMod)
					{
						if (ruleType == SummonRuleType.SetSummonPoolDifficulty)
						{
							result = new SetSummonPoolDifficulty_SummonRule();
						}
					}
					else
					{
						result = new SetSummonPoolLevelMod_SummonRule();
					}
				}
				else if (ruleType != SummonRuleType.SaveCurrentEnemyHP)
				{
					if (ruleType == SummonRuleType.WaitForXSeconds)
					{
						result = new WaitForXSeconds_SummonRule();
					}
				}
				else
				{
					result = new SaveCurrentEnemyHP_SummonRule();
				}
			}
			else if (ruleType <= SummonRuleType.WaitUntilChestCollected)
			{
				if (ruleType != SummonRuleType.WaitUntilAllEnemiesDead)
				{
					if (ruleType != SummonRuleType.WaitUntilXRemaining)
					{
						if (ruleType == SummonRuleType.WaitUntilChestCollected)
						{
							result = new WaitUntilChestCollected_SummonRule();
						}
					}
					else
					{
						result = new WaitUntilXRemaining_SummonRule();
					}
				}
				else
				{
					result = new WaitUntilAllEnemiesDead_SummonRule();
				}
			}
			else if (ruleType <= SummonRuleType.WaitUntilModeShift)
			{
				if (ruleType != SummonRuleType.WaitUntilEnemyHP)
				{
					if (ruleType == SummonRuleType.WaitUntilModeShift)
					{
						result = new WaitUntilModeShift_SummonRule();
					}
				}
				else
				{
					result = new WaitUntilEnemyHP_SummonRule();
				}
			}
			else if (ruleType != SummonRuleType.TeleportPlayer)
			{
				if (ruleType == SummonRuleType.TogglePlayerInvincibility)
				{
					result = new TogglePlayerInvincibility_SummonRule();
				}
			}
			else
			{
				result = new TeleportPlayer_SummonRule();
			}
		}
		else if (ruleType <= SummonRuleType.SlowTime)
		{
			if (ruleType <= SummonRuleType.AwardHeirloom)
			{
				if (ruleType != SummonRuleType.StartArena)
				{
					if (ruleType != SummonRuleType.EndArena)
					{
						if (ruleType == SummonRuleType.AwardHeirloom)
						{
							result = new AwardHeirloom_SummonRule();
						}
					}
					else
					{
						result = new EndArena_SummonRule();
					}
				}
				else
				{
					result = new StartArena_SummonRule();
				}
			}
			else if (ruleType <= SummonRuleType.KillAllEnemies)
			{
				if (ruleType != SummonRuleType.SpawnChest)
				{
					if (ruleType == SummonRuleType.KillAllEnemies)
					{
						result = new KillAllEnemies_SummonRule();
					}
				}
				else
				{
					result = new SpawnChest_SummonRule();
				}
			}
			else if (ruleType != SummonRuleType.SetEnemiesDefeated)
			{
				if (ruleType == SummonRuleType.SlowTime)
				{
					result = new SlowTime_SummonRule();
				}
			}
			else
			{
				result = new SetEnemiesDefeated_SummonRule();
			}
		}
		else if (ruleType <= SummonRuleType.RunDialogue)
		{
			if (ruleType != SummonRuleType.PlayMusic)
			{
				if (ruleType != SummonRuleType.SetGlobalTimer)
				{
					if (ruleType == SummonRuleType.RunDialogue)
					{
						result = new RunDialogue_SummonRule();
					}
				}
				else
				{
					result = new SetGlobalTimer_SummonRule();
				}
			}
			else
			{
				result = new PlayMusic_SummonRule();
			}
		}
		else if (ruleType <= SummonRuleType.WaitForBroadcast)
		{
			if (ruleType != SummonRuleType.DisplayObjectiveComplete)
			{
				if (ruleType == SummonRuleType.WaitForBroadcast)
				{
					result = new WaitForBroadcast_SummonRule();
				}
			}
			else
			{
				result = new DisplayObjectiveComplete_SummonRule();
			}
		}
		else if (ruleType != SummonRuleType.EndChallenge)
		{
			if (ruleType == SummonRuleType.DebugTrace)
			{
				result = new DebugTrace_SummonRule();
			}
		}
		else
		{
			result = new EndChallenge_SummonRule();
		}
		return result;
	}
}
