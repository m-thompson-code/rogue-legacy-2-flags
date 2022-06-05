using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000161 RID: 353
public static class SummonEnemy_AIExtension
{
	// Token: 0x06000BD3 RID: 3027 RVA: 0x00023A34 File Offset: 0x00021C34
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

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00023B29 File Offset: 0x00021D29
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

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00023B58 File Offset: 0x00021D58
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

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00023BBC File Offset: 0x00021DBC
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
