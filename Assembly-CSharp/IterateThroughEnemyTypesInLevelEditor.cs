using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class IterateThroughEnemyTypesInLevelEditor : MonoBehaviour
{
	// Token: 0x06004F05 RID: 20229 RVA: 0x0002B15E File Offset: 0x0002935E
	private void Awake()
	{
		if (!this.m_isOn)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x0012EC50 File Offset: 0x0012CE50
	private void Start()
	{
		foreach (object obj in Enum.GetValues(typeof(EnemyType)))
		{
			EnemyType enemyType = (EnemyType)obj;
			if (enemyType != EnemyType.None && enemyType != EnemyType.Any)
			{
				this.m_enemyTypes.Add(enemyType);
			}
		}
		this.m_currentEnemyType = this.m_enemyTypes[0];
		foreach (object obj2 in Enum.GetValues(typeof(EnemyRank)))
		{
			EnemyRank enemyRank = (EnemyRank)obj2;
			if (enemyRank != EnemyRank.Any && enemyRank != EnemyRank.None)
			{
				this.m_enemyRanks.Add(enemyRank);
			}
		}
		this.m_currentEnemyRank = this.m_enemyRanks[0];
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x0002B16F File Offset: 0x0002936F
	private void Update()
	{
		if (Input.GetKeyDown(this.m_cycleType))
		{
			this.CycleEnemyType();
			this.ResetRoom();
		}
		if (Input.GetKeyDown(this.m_cycleRank))
		{
			this.CycleEnemyRank();
			this.ResetRoom();
		}
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x0012ED44 File Offset: 0x0012CF44
	private void CycleEnemyRank()
	{
		if (this.m_enemySpawnControllers == null)
		{
			this.m_enemySpawnControllers = this.m_onPlayManager.BaseRoom.GetComponentsInChildren<EnemySpawnController>();
		}
		if (this.m_enemySpawnControllers.Length != 0)
		{
			this.m_currentEnemyRank = this.m_enemyRanks[this.m_nextEnemyRankIndex];
			foreach (EnemySpawnController enemySpawnController in this.m_enemySpawnControllers)
			{
				enemySpawnController.Override = true;
				enemySpawnController.Type = this.m_currentEnemyType;
				enemySpawnController.Rank = this.m_currentEnemyRank;
			}
			this.m_nextEnemyRankIndex++;
			if (this.m_nextEnemyRankIndex > this.m_enemyRanks.Count - 1)
			{
				this.m_nextEnemyRankIndex = 0;
			}
		}
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x0012EDF0 File Offset: 0x0012CFF0
	private void CycleEnemyType()
	{
		if (this.m_enemySpawnControllers == null)
		{
			this.m_enemySpawnControllers = this.m_onPlayManager.BaseRoom.GetComponentsInChildren<EnemySpawnController>();
		}
		if (this.m_enemySpawnControllers.Length != 0)
		{
			this.m_currentEnemyType = this.m_enemyTypes[this.m_nextEnemyTypeIndex];
			foreach (EnemySpawnController enemySpawnController in this.m_enemySpawnControllers)
			{
				enemySpawnController.Override = true;
				enemySpawnController.Type = this.m_currentEnemyType;
				enemySpawnController.Rank = this.m_currentEnemyRank;
			}
			this.m_nextEnemyTypeIndex++;
			if (this.m_nextEnemyTypeIndex > this.m_enemyTypes.Count - 1)
			{
				this.m_nextEnemyTypeIndex = 0;
			}
		}
	}

	// Token: 0x06004F0A RID: 20234 RVA: 0x0002B1A3 File Offset: 0x000293A3
	private void ResetRoom()
	{
		this.m_onPlayManager.SetupWorld();
	}

	// Token: 0x04003C05 RID: 15365
	[SerializeField]
	private OnPlayManager m_onPlayManager;

	// Token: 0x04003C06 RID: 15366
	[SerializeField]
	private int m_overrideLevel = 1;

	// Token: 0x04003C07 RID: 15367
	[SerializeField]
	private bool m_isOn;

	// Token: 0x04003C08 RID: 15368
	[SerializeField]
	private KeyCode m_cycleType = KeyCode.LeftArrow;

	// Token: 0x04003C09 RID: 15369
	[SerializeField]
	private KeyCode m_cycleRank = KeyCode.RightArrow;

	// Token: 0x04003C0A RID: 15370
	private int m_nextEnemyTypeIndex;

	// Token: 0x04003C0B RID: 15371
	private int m_nextEnemyRankIndex;

	// Token: 0x04003C0C RID: 15372
	private List<EnemyType> m_enemyTypes = new List<EnemyType>();

	// Token: 0x04003C0D RID: 15373
	private List<EnemyRank> m_enemyRanks = new List<EnemyRank>();

	// Token: 0x04003C0E RID: 15374
	private EnemySpawnController[] m_enemySpawnControllers;

	// Token: 0x04003C0F RID: 15375
	private EnemyType m_currentEnemyType;

	// Token: 0x04003C10 RID: 15376
	private EnemyRank m_currentEnemyRank = EnemyRank.None;
}
