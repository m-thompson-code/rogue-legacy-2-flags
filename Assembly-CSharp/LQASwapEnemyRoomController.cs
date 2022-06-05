using System;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class LQASwapEnemyRoomController : MonoBehaviour
{
	// Token: 0x06003001 RID: 12289 RVA: 0x000A4428 File Offset: 0x000A2628
	private void SwapEnemyType(bool swapUp)
	{
		int num = EnemyTypes_RL.TypeArray.IndexOf(this.m_enemySpawner.Type);
		if (num != -1)
		{
			int num2 = 0;
			if (swapUp)
			{
				do
				{
					num++;
					num2++;
					if (num >= EnemyTypes_RL.TypeArray.Length - 1)
					{
						num = 1;
					}
					bool flag = this.m_exclusionArray.IndexOf(EnemyTypes_RL.TypeArray[num]) != -1;
					if (EnemyLibrary.GetEnemyPrefab(this.m_enemySpawner.Type, this.m_enemySpawner.Rank) && !flag)
					{
						break;
					}
				}
				while (num2 < 100);
			}
			else
			{
				bool flag;
				do
				{
					num--;
					num2++;
					if (num <= 0)
					{
						num = EnemyTypes_RL.TypeArray.Length - 2;
					}
					flag = (this.m_exclusionArray.IndexOf(EnemyTypes_RL.TypeArray[num]) != -1);
				}
				while ((!EnemyLibrary.GetEnemyPrefab(this.m_enemySpawner.Type, this.m_enemySpawner.Rank) || flag) && num2 < 100);
			}
			this.m_enemySpawner.Type = EnemyTypes_RL.TypeArray[num];
			this.m_enemySpawner.Rank = EnemyRank.Basic;
			this.UpdateSpawnedEnemy();
		}
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x000A4534 File Offset: 0x000A2734
	private void SwapEnemyRank(bool swapUp)
	{
		int num = EnemyTypes_RL.RankArray.IndexOf(this.m_enemySpawner.Rank);
		if (num != -1)
		{
			int num2 = 0;
			if (swapUp)
			{
				do
				{
					num++;
					num2++;
					if (num >= EnemyTypes_RL.RankArray.Length - 2)
					{
						num = 0;
					}
					if (EnemyLibrary.GetEnemyPrefab(this.m_enemySpawner.Type, this.m_enemySpawner.Rank))
					{
						break;
					}
				}
				while (num2 < 100);
			}
			else
			{
				do
				{
					num--;
					num2++;
					if (num < 0)
					{
						num = EnemyTypes_RL.RankArray.Length - 3;
					}
				}
				while (!EnemyLibrary.GetEnemyPrefab(this.m_enemySpawner.Type, this.m_enemySpawner.Rank) && num2 < 100);
			}
			this.m_enemySpawner.Rank = EnemyTypes_RL.RankArray[num];
			this.UpdateSpawnedEnemy();
		}
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x000A45F8 File Offset: 0x000A27F8
	private void UpdateSpawnedEnemy()
	{
		if (EnemyLibrary.GetEnemyPrefab(this.m_enemySpawner.Type, this.m_enemySpawner.Rank))
		{
			if (this.m_enemySpawner.EnemyInstance && this.m_enemySpawner.EnemyInstance.gameObject.activeInHierarchy)
			{
				this.m_enemySpawner.EnemyInstance.gameObject.SetActive(false);
			}
			EnemyManager.CreateBiomePools(base.GetComponent<Room>().BiomeType);
			this.m_enemySpawner.InitializeEnemyInstance();
		}
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x000A4684 File Offset: 0x000A2884
	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if (Input.GetKeyDown(KeyCode.Equals))
			{
				this.SwapEnemyType(true);
				return;
			}
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				this.SwapEnemyType(false);
				return;
			}
			if (Input.GetKeyDown(KeyCode.RightBracket))
			{
				this.SwapEnemyRank(true);
				return;
			}
			if (Input.GetKeyDown(KeyCode.LeftBracket))
			{
				this.SwapEnemyRank(false);
			}
		}
	}

	// Token: 0x0400263F RID: 9791
	[SerializeField]
	private EnemySpawnController m_enemySpawner;

	// Token: 0x04002640 RID: 9792
	private EnemyType[] m_exclusionArray = new EnemyType[]
	{
		EnemyType.Eggplant,
		EnemyType.Ninja,
		EnemyType.Ghost,
		EnemyType.RogueKnight,
		EnemyType.JumpWallStick,
		EnemyType.StealthAssassin
	};
}
