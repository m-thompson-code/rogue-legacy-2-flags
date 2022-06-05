using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class TowerBossRoomController : BossRoomController
{
	// Token: 0x06003033 RID: 12339 RVA: 0x000A50E8 File Offset: 0x000A32E8
	protected override void Awake()
	{
		base.Awake();
		this.m_runBossSpawnAnim = new Action(this.RunBossSpawnAnim);
		this.m_runIntroComplete = new Action(this.RunIntroComplete);
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x000A5114 File Offset: 0x000A3314
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

	// Token: 0x06003035 RID: 12341 RVA: 0x000A51D8 File Offset: 0x000A33D8
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

	// Token: 0x06003036 RID: 12342 RVA: 0x000A51E8 File Offset: 0x000A33E8
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

	// Token: 0x06003037 RID: 12343 RVA: 0x000A521C File Offset: 0x000A341C
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

	// Token: 0x06003038 RID: 12344 RVA: 0x000A5280 File Offset: 0x000A3480
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

	// Token: 0x04002654 RID: 9812
	[SerializeField]
	private EnemySpawnController[] m_bossSpawnControllers;

	// Token: 0x04002655 RID: 9813
	[SerializeField]
	private EnemySpawnController[] m_bossSpawnersToStartDisabled;

	// Token: 0x04002656 RID: 9814
	[SerializeField]
	private bool m_disableBossIntro;

	// Token: 0x04002657 RID: 9815
	[SerializeField]
	private bool m_disableActiveEyeballModeshift;

	// Token: 0x04002658 RID: 9816
	[SerializeField]
	private bool m_disableInactiveEyeballModeshift;

	// Token: 0x04002659 RID: 9817
	[SerializeField]
	private bool m_disableDeath;

	// Token: 0x0400265A RID: 9818
	private Action m_runBossSpawnAnim;

	// Token: 0x0400265B RID: 9819
	private Action m_runIntroComplete;
}
