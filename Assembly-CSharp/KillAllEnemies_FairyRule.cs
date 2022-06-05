using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class KillAllEnemies_FairyRule : FairyRule
{
	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000842FE File Offset: 0x000824FE
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_KILL_ALL_ENEMIES_1";
		}
	}

	// Token: 0x17000FAE RID: 4014
	// (get) Token: 0x060027C3 RID: 10179 RVA: 0x00084305 File Offset: 0x00082505
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.KillAllEnemies;
		}
	}

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x060027C4 RID: 10180 RVA: 0x00084309 File Offset: 0x00082509
	public override bool LockChestAtStart
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x0008430C File Offset: 0x0008250C
	private void Awake()
	{
		this.m_checkEnemyDeathCount = new Action<MonoBehaviour, EventArgs>(this.CheckEnemyDeathCount);
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x00084320 File Offset: 0x00082520
	private void OnDestroy()
	{
		this.UnsubscribeFromEnemyDeathEvent();
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x00084328 File Offset: 0x00082528
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		if (base.State != FairyRoomState.Failed)
		{
			if (!this.PerformInitialEnemyDeathCount())
			{
				base.State = FairyRoomState.Running;
				this.m_isListeningForEnemyDeathEvent = true;
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_checkEnemyDeathCount);
				return;
			}
			base.SetIsPassed();
		}
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x0008435F File Offset: 0x0008255F
	public override void StopRule()
	{
		base.StopRule();
		this.UnsubscribeFromEnemyDeathEvent();
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x0008436D File Offset: 0x0008256D
	private void UnsubscribeFromEnemyDeathEvent()
	{
		if (this.m_isListeningForEnemyDeathEvent)
		{
			this.m_isListeningForEnemyDeathEvent = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_checkEnemyDeathCount);
		}
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x0008438C File Offset: 0x0008258C
	private bool PerformInitialEnemyDeathCount()
	{
		if (PlayerManager.IsInstantiated)
		{
			foreach (EnemySpawnController enemySpawnController in PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers)
			{
				if (!enemySpawnController.IsDead && !FairyRoomController.EnemyExceptionArray.Contains(enemySpawnController.Type))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000843E4 File Offset: 0x000825E4
	private void CheckEnemyDeathCount(object sender, EventArgs eventArgs)
	{
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			EnemyController victim = (eventArgs as EnemyDeathEventArgs).Victim;
			bool flag = true;
			foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
			{
				if (!(victim == null) && !(enemySpawnController.EnemyInstance == null) && !enemySpawnController.EnemyInstance.Equals(null) && !(victim.gameObject == enemySpawnController.EnemyInstance.gameObject) && !enemySpawnController.IsDead && !FairyRoomController.EnemyExceptionArray.Contains(enemySpawnController.Type))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				base.SetIsPassed();
				this.UnsubscribeFromEnemyDeathEvent();
			}
		}
	}

	// Token: 0x04002136 RID: 8502
	private bool m_isListeningForEnemyDeathEvent;

	// Token: 0x04002137 RID: 8503
	private Action<MonoBehaviour, EventArgs> m_checkEnemyDeathCount;
}
