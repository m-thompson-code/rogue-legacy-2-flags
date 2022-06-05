using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class IterateThroughEnemyTypesInLevelEditor : MonoBehaviour
{
	// Token: 0x06003861 RID: 14433 RVA: 0x000C090C File Offset: 0x000BEB0C
	private void Awake()
	{
		if (!this.m_isOn)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x000C0920 File Offset: 0x000BEB20
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

	// Token: 0x06003863 RID: 14435 RVA: 0x000C0A14 File Offset: 0x000BEC14
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

	// Token: 0x06003864 RID: 14436 RVA: 0x000C0A48 File Offset: 0x000BEC48
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

	// Token: 0x06003865 RID: 14437 RVA: 0x000C0AF4 File Offset: 0x000BECF4
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

	// Token: 0x06003866 RID: 14438 RVA: 0x000C0B9F File Offset: 0x000BED9F
	private void ResetRoom()
	{
		this.m_onPlayManager.SetupWorld();
	}

	// Token: 0x04002B98 RID: 11160
	[SerializeField]
	private OnPlayManager m_onPlayManager;

	// Token: 0x04002B99 RID: 11161
	[SerializeField]
	private int m_overrideLevel = 1;

	// Token: 0x04002B9A RID: 11162
	[SerializeField]
	private bool m_isOn;

	// Token: 0x04002B9B RID: 11163
	[SerializeField]
	private KeyCode m_cycleType = KeyCode.LeftArrow;

	// Token: 0x04002B9C RID: 11164
	[SerializeField]
	private KeyCode m_cycleRank = KeyCode.RightArrow;

	// Token: 0x04002B9D RID: 11165
	private int m_nextEnemyTypeIndex;

	// Token: 0x04002B9E RID: 11166
	private int m_nextEnemyRankIndex;

	// Token: 0x04002B9F RID: 11167
	private List<EnemyType> m_enemyTypes = new List<EnemyType>();

	// Token: 0x04002BA0 RID: 11168
	private List<EnemyRank> m_enemyRanks = new List<EnemyRank>();

	// Token: 0x04002BA1 RID: 11169
	private EnemySpawnController[] m_enemySpawnControllers;

	// Token: 0x04002BA2 RID: 11170
	private EnemyType m_currentEnemyType;

	// Token: 0x04002BA3 RID: 11171
	private EnemyRank m_currentEnemyRank = EnemyRank.None;
}
