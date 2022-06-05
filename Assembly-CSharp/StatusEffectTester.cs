using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class StatusEffectTester : MonoBehaviour
{
	// Token: 0x06001F4B RID: 8011 RVA: 0x0006468A File Offset: 0x0006288A
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		this.StartStatusEffect();
		yield break;
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x0006469C File Offset: 0x0006289C
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

	// Token: 0x06001F4D RID: 8013 RVA: 0x00064760 File Offset: 0x00062960
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

	// Token: 0x04001C0A RID: 7178
	[SerializeField]
	private StatusEffectType m_statusEffectToAdd;

	// Token: 0x04001C0B RID: 7179
	[SerializeField]
	private bool m_applyOnlyToExpertEnemies;

	// Token: 0x04001C0C RID: 7180
	[SerializeField]
	private float m_duration = 5f;
}
