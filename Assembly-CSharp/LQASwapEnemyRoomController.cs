using System;
using UnityEngine;

// Token: 0x02000872 RID: 2162
public class LQASwapEnemyRoomController : MonoBehaviour
{
	// Token: 0x06004293 RID: 17043 RVA: 0x0010B130 File Offset: 0x00109330
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

	// Token: 0x06004294 RID: 17044 RVA: 0x0010B23C File Offset: 0x0010943C
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

	// Token: 0x06004295 RID: 17045 RVA: 0x0010B300 File Offset: 0x00109500
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

	// Token: 0x06004296 RID: 17046 RVA: 0x0010B38C File Offset: 0x0010958C
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

	// Token: 0x0400340C RID: 13324
	[SerializeField]
	private EnemySpawnController m_enemySpawner;

	// Token: 0x0400340D RID: 13325
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
