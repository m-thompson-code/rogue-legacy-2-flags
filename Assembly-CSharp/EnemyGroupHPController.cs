using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000391 RID: 913
[RequireComponent(typeof(EnemyController))]
public class EnemyGroupHPController : MonoBehaviour
{
	// Token: 0x06001E7A RID: 7802 RVA: 0x0000FE1D File Offset: 0x0000E01D
	private void Awake()
	{
		this.m_baseEnemy = base.GetComponent<EnemyController>();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
		this.m_onEnemyHPGain = new Action<object, HealthChangeEventArgs>(this.OnEnemyHPGain);
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x0000FE4F File Offset: 0x0000E04F
	private IEnumerator Initialize()
	{
		while (!this.m_baseEnemy.IsInitialized)
		{
			yield return null;
		}
		this.m_isInitialized = true;
		this.GroupEnemyHits();
		yield break;
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x0000FE5E File Offset: 0x0000E05E
	private void OnEnable()
	{
		if (!this.m_isInitialized)
		{
			base.StartCoroutine(this.Initialize());
			return;
		}
		this.GroupEnemyHits();
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x0000FE7C File Offset: 0x0000E07C
	private void OnDisable()
	{
		this.StartAddingEnemyGroup();
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x0009F688 File Offset: 0x0009D888
	public void StartAddingEnemyGroup()
	{
		if (this.m_baseEnemy && this.m_baseEnemy.IsInitialized)
		{
			this.m_baseEnemy.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
			this.m_baseEnemy.HealthChangeRelay.RemoveListener(this.m_onEnemyHPGain);
		}
		this.m_enemyGroup.Clear();
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x0000FE84 File Offset: 0x0000E084
	public void StopAddingEnemyGroup()
	{
		this.m_baseEnemy.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		this.m_baseEnemy.HealthChangeRelay.AddListener(this.m_onEnemyHPGain, false);
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x0000FEBB File Offset: 0x0000E0BB
	public void AddEnemyToGroup(EnemyController enemy)
	{
		if (!this.m_enemyGroup.Contains(enemy))
		{
			this.m_enemyGroup.Add(enemy);
		}
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x0000FED7 File Offset: 0x0000E0D7
	public void RemoveEnemyFromGroup(EnemyController enemy)
	{
		if (this.m_enemyGroup.Contains(enemy))
		{
			this.m_enemyGroup.Remove(enemy);
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x0009F6F0 File Offset: 0x0009D8F0
	private void GroupEnemyHits()
	{
		if (!this.m_disableAutoGrouping)
		{
			this.StartAddingEnemyGroup();
			if (this.m_autoGroupBySameEnemyType || this.m_autoGroupBySameEnemyRank)
			{
				EnemySpawnController[] enemySpawnControllers = this.m_baseEnemy.Room.SpawnControllerManager.EnemySpawnControllers;
				for (int i = 0; i < enemySpawnControllers.Length; i++)
				{
					EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
					if (enemyInstance && (!this.m_autoGroupBySameEnemyType || enemyInstance.EnemyType == this.m_baseEnemy.EnemyType) && (!this.m_autoGroupBySameEnemyRank || enemyInstance.EnemyRank == this.m_baseEnemy.EnemyRank))
					{
						this.m_enemyGroup.Add(enemyInstance);
					}
				}
			}
			if (!this.m_enemyGroup.Contains(this.m_baseEnemy))
			{
				this.m_enemyGroup.Add(this.m_baseEnemy);
			}
		}
		this.StopAddingEnemyGroup();
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x0009F7C0 File Offset: 0x0009D9C0
	private void OnEnemyHPGain(object sender, EventArgs args)
	{
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs != null && healthChangeEventArgs.PrevHealthValue < healthChangeEventArgs.NewHealthValue)
		{
			this.SyncHP();
		}
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x0009F7EC File Offset: 0x0009D9EC
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (!this.m_baseEnemy.IsDead && args.DamageTaken > 0f)
		{
			foreach (EnemyController enemyController in this.m_enemyGroup)
			{
				if (!(enemyController == this.m_baseEnemy))
				{
					if (!enemyController.TakesNoDamage)
					{
						bool immuneToAllStatusEffects = enemyController.StatusEffectController.ImmuneToAllStatusEffects;
						enemyController.StatusEffectController.ImmuneToAllStatusEffects = true;
						StatusEffectController.DISABLE_ADDING_STATUS_EFFECTS = true;
						enemyController.CanIncrementRelicHitCounter = false;
						enemyController.CharacterHitResponse.StartHitResponse(args.Attacker.gameObject, args.Attacker, args.DamageTaken, true, false);
						enemyController.CanIncrementRelicHitCounter = true;
						enemyController.StatusEffectController.ImmuneToAllStatusEffects = immuneToAllStatusEffects;
						StatusEffectController.DISABLE_ADDING_STATUS_EFFECTS = false;
					}
					else
					{
						enemyController.SetHealth(-args.DamageTaken, true, true);
					}
				}
			}
			this.SyncHP();
		}
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x0009F8F0 File Offset: 0x0009DAF0
	private void SyncHP()
	{
		float num = 0f;
		foreach (EnemyController enemyController in this.m_enemyGroup)
		{
			if (!enemyController.IsDead && enemyController.CurrentHealth > num)
			{
				num = enemyController.CurrentHealth;
			}
		}
		foreach (EnemyController enemyController2 in this.m_enemyGroup)
		{
			if (!enemyController2.IsDead)
			{
				enemyController2.SetHealth(num, false, false);
			}
		}
	}

	// Token: 0x04001B3C RID: 6972
	[SerializeField]
	private bool m_disableAutoGrouping;

	// Token: 0x04001B3D RID: 6973
	[SerializeField]
	[ConditionalHide("m_disableAutoGrouping", true, true)]
	private bool m_autoGroupBySameEnemyType;

	// Token: 0x04001B3E RID: 6974
	[SerializeField]
	[ConditionalHide("m_disableAutoGrouping", true, true)]
	private bool m_autoGroupBySameEnemyRank;

	// Token: 0x04001B3F RID: 6975
	private List<EnemyController> m_enemyGroup = new List<EnemyController>();

	// Token: 0x04001B40 RID: 6976
	private EnemyController m_baseEnemy;

	// Token: 0x04001B41 RID: 6977
	private bool m_isInitialized;

	// Token: 0x04001B42 RID: 6978
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x04001B43 RID: 6979
	private Action<object, HealthChangeEventArgs> m_onEnemyHPGain;
}
