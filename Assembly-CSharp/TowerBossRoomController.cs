using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class TowerBossRoomController : BossRoomController
{
	// Token: 0x060042DD RID: 17117 RVA: 0x00024FE2 File Offset: 0x000231E2
	protected override void Awake()
	{
		base.Awake();
		this.m_runBossSpawnAnim = new Action(this.RunBossSpawnAnim);
		this.m_runIntroComplete = new Action(this.RunIntroComplete);
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x0010BFC4 File Offset: 0x0010A1C4
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		EnemySpawnController[] bossSpawnControllers = this.m_bossSpawnControllers;
		for (int i = 0; i < bossSpawnControllers.Length; i++)
		{
			EnemyController enemyInstance = bossSpawnControllers[i].EnemyInstance;
			if (enemyInstance)
			{
				EnemyGroupHPController component = enemyInstance.GetComponent<EnemyGroupHPController>();
				component.StartAddingEnemyGroup();
				foreach (EnemySpawnController enemySpawnController in this.m_bossSpawnControllers)
				{
					EnemyController enemyInstance2 = enemySpawnController.EnemyInstance;
					if (enemySpawnController)
					{
						component.AddEnemyToGroup(enemyInstance2);
					}
				}
				component.StopAddingEnemyGroup();
				if ((enemyInstance.EnemyType == EnemyType.EyeballBoss_Left || enemyInstance.EnemyType == EnemyType.EyeballBoss_Right) && enemyInstance.EnemyRank == EnemyRank.Miniboss)
				{
					enemyInstance.TakesNoDamage = true;
				}
				enemyInstance.DisableDeath = this.m_disableDeath;
			}
		}
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x0002500E File Offset: 0x0002320E
	protected override IEnumerator StartIntro()
	{
		base.Boss.LockFlip = true;
		this.BossSpawnAnimRelay.AddOnce(this.m_runBossSpawnAnim, false);
		this.IntroCompleteRelay.AddOnce(this.m_runIntroComplete, false);
		foreach (EnemySpawnController enemySpawnController in this.m_bossSpawnControllers)
		{
			EnemyController enemyInstance = enemySpawnController.EnemyInstance;
			if (enemyInstance)
			{
				if (!this.DisableSpawner(enemySpawnController))
				{
					enemyInstance.Animator.SetBool("Centered", false);
					enemyInstance.Animator.Play("Intro_Idle");
					(enemyInstance.LogicController.LogicScript as EyeballBoss_Basic_AIScript).StartsDisabled = false;
					(enemyInstance.LogicController.LogicScript as EyeballBoss_Basic_AIScript).DisableModeshift = this.m_disableActiveEyeballModeshift;
					enemyInstance.TakesNoDamage = false;
				}
				else
				{
					(enemyInstance.LogicController.LogicScript as EyeballBoss_Basic_AIScript).StartsDisabled = true;
					(enemyInstance.LogicController.LogicScript as EyeballBoss_Basic_AIScript).DisableModeshift = this.m_disableInactiveEyeballModeshift;
				}
				enemyInstance.LogicController.DisableLogicActivationByDistance = true;
			}
		}
		if (!this.m_disableBossIntro)
		{
			yield return base.StartIntro();
		}
		this.RunIntroComplete();
		yield break;
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x0010C088 File Offset: 0x0010A288
	private bool DisableSpawner(EnemySpawnController bossSpawner)
	{
		foreach (EnemySpawnController y in this.m_bossSpawnersToStartDisabled)
		{
			if (bossSpawner == y)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x0010C0BC File Offset: 0x0010A2BC
	private void RunBossSpawnAnim()
	{
		foreach (EnemySpawnController enemySpawnController in this.m_bossSpawnControllers)
		{
			if (!(enemySpawnController == this.BossSpawnController) && !this.DisableSpawner(enemySpawnController))
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance)
				{
					base.StartCoroutine(enemyInstance.LogicController.LogicScript.SpawnAnim());
				}
			}
		}
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x0010C120 File Offset: 0x0010A320
	private void RunIntroComplete()
	{
		foreach (EnemySpawnController enemySpawnController in this.m_bossSpawnControllers)
		{
			if (!(enemySpawnController == this.BossSpawnController) && !this.DisableSpawner(enemySpawnController))
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance)
				{
					enemyInstance.LogicController.DisableLogicActivationByDistance = false;
				}
			}
		}
	}

	// Token: 0x04003431 RID: 13361
	[SerializeField]
	private EnemySpawnController[] m_bossSpawnControllers;

	// Token: 0x04003432 RID: 13362
	[SerializeField]
	private EnemySpawnController[] m_bossSpawnersToStartDisabled;

	// Token: 0x04003433 RID: 13363
	[SerializeField]
	private bool m_disableBossIntro;

	// Token: 0x04003434 RID: 13364
	[SerializeField]
	private bool m_disableActiveEyeballModeshift;

	// Token: 0x04003435 RID: 13365
	[SerializeField]
	private bool m_disableInactiveEyeballModeshift;

	// Token: 0x04003436 RID: 13366
	[SerializeField]
	private bool m_disableDeath;

	// Token: 0x04003437 RID: 13367
	private Action m_runBossSpawnAnim;

	// Token: 0x04003438 RID: 13368
	private Action m_runIntroComplete;
}
