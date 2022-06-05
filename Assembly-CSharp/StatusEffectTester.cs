using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class StatusEffectTester : MonoBehaviour
{
	// Token: 0x06002BC8 RID: 11208 RVA: 0x000185D1 File Offset: 0x000167D1
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		this.StartStatusEffect();
		yield break;
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000C4868 File Offset: 0x000C2A68
	public void StartStatusEffect()
	{
		if (this.m_statusEffectToAdd == StatusEffectType.None)
		{
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		EnemyController caster = null;
		foreach (EnemySpawnController enemySpawnController in PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn && enemySpawnController.EnemyInstance)
			{
				EnemyController component = enemySpawnController.EnemyInstance.gameObject.GetComponent<EnemyController>();
				if (component && !component.IsDead && (component.EnemyRank == EnemyRank.Expert || !this.m_applyOnlyToExpertEnemies))
				{
					caster = component;
					component.StatusEffectController.StartStatusEffect(this.m_statusEffectToAdd, this.m_duration, playerController);
				}
			}
		}
		playerController.StatusEffectController.StartStatusEffect(this.m_statusEffectToAdd, this.m_duration, caster);
	}

	// Token: 0x06002BCA RID: 11210 RVA: 0x000C492C File Offset: 0x000C2B2C
	public void StopStatusEffect()
	{
		if (this.m_statusEffectToAdd == StatusEffectType.None)
		{
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		foreach (EnemySpawnController enemySpawnController in PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn && enemySpawnController.EnemyInstance != null)
			{
				EnemyController component = enemySpawnController.EnemyInstance.gameObject.GetComponent<EnemyController>();
				if (component)
				{
					component.StatusEffectController.StopStatusEffect(this.m_statusEffectToAdd, true);
				}
			}
		}
		playerController.StatusEffectController.StopStatusEffect(this.m_statusEffectToAdd, true);
	}

	// Token: 0x04002514 RID: 9492
	[SerializeField]
	private StatusEffectType m_statusEffectToAdd;

	// Token: 0x04002515 RID: 9493
	[SerializeField]
	private bool m_applyOnlyToExpertEnemies;

	// Token: 0x04002516 RID: 9494
	[SerializeField]
	private float m_duration = 5f;
}
