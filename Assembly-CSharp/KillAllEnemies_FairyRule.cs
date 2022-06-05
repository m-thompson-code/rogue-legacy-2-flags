using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000704 RID: 1796
public class KillAllEnemies_FairyRule : FairyRule
{
	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x060036D4 RID: 14036 RVA: 0x0001E294 File Offset: 0x0001C494
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_KILL_ALL_ENEMIES_1";
		}
	}

	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x060036D5 RID: 14037 RVA: 0x00017640 File Offset: 0x00015840
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.KillAllEnemies;
		}
	}

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x060036D6 RID: 14038 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool LockChestAtStart
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x0001E29B File Offset: 0x0001C49B
	private void Awake()
	{
		this.m_checkEnemyDeathCount = new Action<MonoBehaviour, EventArgs>(this.CheckEnemyDeathCount);
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x0001E2AF File Offset: 0x0001C4AF
	private void OnDestroy()
	{
		this.UnsubscribeFromEnemyDeathEvent();
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x0001E2B7 File Offset: 0x0001C4B7
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

	// Token: 0x060036DA RID: 14042 RVA: 0x0001E2EE File Offset: 0x0001C4EE
	public override void StopRule()
	{
		base.StopRule();
		this.UnsubscribeFromEnemyDeathEvent();
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x0001E2FC File Offset: 0x0001C4FC
	private void UnsubscribeFromEnemyDeathEvent()
	{
		if (this.m_isListeningForEnemyDeathEvent)
		{
			this.m_isListeningForEnemyDeathEvent = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_checkEnemyDeathCount);
		}
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x000E4DDC File Offset: 0x000E2FDC
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

	// Token: 0x060036DD RID: 14045 RVA: 0x000E4E34 File Offset: 0x000E3034
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

	// Token: 0x04002C67 RID: 11367
	private bool m_isListeningForEnemyDeathEvent;

	// Token: 0x04002C68 RID: 11368
	private Action<MonoBehaviour, EventArgs> m_checkEnemyDeathCount;
}
