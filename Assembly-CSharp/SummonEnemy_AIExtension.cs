using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028D RID: 653
public static class SummonEnemy_AIExtension
{
	// Token: 0x060012AA RID: 4778 RVA: 0x0008261C File Offset: 0x0008081C
	public static EnemyController SummonEnemy_NoYield(this BaseAIScript aiScript, EnemyType enemyType, EnemyRank rank, Vector2 spawnOffset, bool usesAbsPos, bool runSummonAnim)
	{
		SummonEnemyController component = aiScript.EnemyController.gameObject.GetComponent<SummonEnemyController>();
		if (component)
		{
			if (component.Contains(enemyType, rank))
			{
				EnemyController enemyController = EnemyManager.SummonEnemy(aiScript.EnemyController, enemyType, rank, spawnOffset, usesAbsPos, runSummonAnim, 1f, 1f);
				if (!runSummonAnim)
				{
					enemyController.gameObject.SetActive(true);
				}
				return enemyController;
			}
			Debug.Log(string.Concat(new string[]
			{
				"Could not summon enemy: ",
				enemyType.ToString(),
				" - ",
				rank.ToString(),
				". Enemy not found in SummonEnemyController."
			}));
		}
		else
		{
			Debug.Log(string.Concat(new string[]
			{
				"Could not summon enemy: ",
				enemyType.ToString(),
				" - ",
				rank.ToString(),
				". Enemy must have SummonEnemyController."
			}));
		}
		return null;
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x000097BD File Offset: 0x000079BD
	public static IEnumerator SummonEnemy(this BaseAIScript aiScript, EnemyType enemyType, EnemyRank rank, Vector2 spawnOffset, bool usesAbsPos)
	{
		SummonEnemyController component = aiScript.EnemyController.gameObject.GetComponent<SummonEnemyController>();
		if (component)
		{
			if (component.Contains(enemyType, rank))
			{
				EnemyController enemy = EnemyManager.SummonEnemy(aiScript.EnemyController, enemyType, rank, spawnOffset, usesAbsPos, false, 1f, 1f);
				yield return EnemyManager.RunSummonAnimCoroutine(enemy, 1f, false, 1f);
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"Could not summon enemy: ",
					enemyType.ToString(),
					" - ",
					rank.ToString(),
					". Enemy not found in SummonEnemyController."
				}));
			}
		}
		else
		{
			Debug.Log(string.Concat(new string[]
			{
				"Could not summon enemy: ",
				enemyType.ToString(),
				" - ",
				rank.ToString(),
				". Enemy must have SummonEnemyController."
			}));
		}
		yield break;
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00082714 File Offset: 0x00080914
	public static EnemyController SummonEnemy_NoYield(this BaseAIScript aiScript, EnemyType enemyType, EnemyRank rank, int roomSpawnPosIndex, bool runSummonAnim)
	{
		SpawnPositionController componentInChildren = aiScript.EnemyController.Room.gameObject.GetComponentInChildren<SpawnPositionController>();
		if (componentInChildren)
		{
			if (roomSpawnPosIndex == -1)
			{
				roomSpawnPosIndex = UnityEngine.Random.Range(0, componentInChildren.SpawnPositionArray.Length);
			}
			Vector3 spawnPosition = componentInChildren.GetSpawnPosition(roomSpawnPosIndex);
			return aiScript.SummonEnemy_NoYield(enemyType, rank, spawnPosition, true, runSummonAnim);
		}
		Debug.Log("Could not summon enemy via spawnPositionIndex. Room missing SpawnPositionController.");
		return null;
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x000097E9 File Offset: 0x000079E9
	public static IEnumerator SummonEnemy(this BaseAIScript aiScript, EnemyType enemyType, EnemyRank rank, int roomSpawnPosIndex)
	{
		SpawnPositionController componentInChildren = aiScript.EnemyController.Room.gameObject.GetComponentInChildren<SpawnPositionController>();
		if (componentInChildren)
		{
			if (roomSpawnPosIndex == -1)
			{
				roomSpawnPosIndex = UnityEngine.Random.Range(0, componentInChildren.SpawnPositionArray.Length);
			}
			Vector3 spawnPosition = componentInChildren.GetSpawnPosition(roomSpawnPosIndex);
			yield return aiScript.SummonEnemy(enemyType, rank, spawnPosition, true);
		}
		else
		{
			Debug.Log("Could not summon enemy via spawnPositionIndex. Room missing SpawnPositionController.");
		}
		yield break;
	}
}
