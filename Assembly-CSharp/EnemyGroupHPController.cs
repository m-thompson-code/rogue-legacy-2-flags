using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F3 RID: 499
[RequireComponent(typeof(EnemyController))]
public class EnemyGroupHPController : MonoBehaviour
{
	// Token: 0x06001541 RID: 5441 RVA: 0x0004100C File Offset: 0x0003F20C
	private void Awake()
	{
		this.m_baseEnemy = base.GetComponent<EnemyController>();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
		this.m_onEnemyHPGain = new Action<object, HealthChangeEventArgs>(this.OnEnemyHPGain);
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x0004103E File Offset: 0x0003F23E
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

	// Token: 0x06001543 RID: 5443 RVA: 0x0004104D File Offset: 0x0003F24D
	private void OnEnable()
	{
		if (!this.m_isInitialized)
		{
			base.StartCoroutine(this.Initialize());
			return;
		}
		this.GroupEnemyHits();
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x0004106B File Offset: 0x0003F26B
	private void OnDisable()
	{
		this.StartAddingEnemyGroup();
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00041074 File Offset: 0x0003F274
	public void StartAddingEnemyGroup()
	{
		if (this.m_baseEnemy && this.m_baseEnemy.IsInitialized)
		{
			this.m_baseEnemy.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
			this.m_baseEnemy.HealthChangeRelay.RemoveListener(this.m_onEnemyHPGain);
		}
		this.m_enemyGroup.Clear();
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x000410D9 File Offset: 0x0003F2D9
	public void StopAddingEnemyGroup()
	{
		this.m_baseEnemy.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		this.m_baseEnemy.HealthChangeRelay.AddListener(this.m_onEnemyHPGain, false);
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x00041110 File Offset: 0x0003F310
	public void AddEnemyToGroup(EnemyController enemy)
	{
		if (!this.m_enemyGroup.Contains(enemy))
		{
			this.m_enemyGroup.Add(enemy);
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x0004112C File Offset: 0x0003F32C
	public void RemoveEnemyFromGroup(EnemyController enemy)
	{
		if (this.m_enemyGroup.Contains(enemy))
		{
			this.m_enemyGroup.Remove(enemy);
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x0004114C File Offset: 0x0003F34C
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

	// Token: 0x0600154A RID: 5450 RVA: 0x0004121C File Offset: 0x0003F41C
	private void OnEnemyHPGain(object sender, EventArgs args)
	{
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs != null && healthChangeEventArgs.PrevHealthValue < healthChangeEventArgs.NewHealthValue)
		{
			this.SyncHP();
		}
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00041248 File Offset: 0x0003F448
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

	// Token: 0x0600154C RID: 5452 RVA: 0x0004134C File Offset: 0x0003F54C
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

	// Token: 0x04001485 RID: 5253
	[SerializeField]
	private bool m_disableAutoGrouping;

	// Token: 0x04001486 RID: 5254
	[SerializeField]
	[ConditionalHide("m_disableAutoGrouping", true, true)]
	private bool m_autoGroupBySameEnemyType;

	// Token: 0x04001487 RID: 5255
	[SerializeField]
	[ConditionalHide("m_disableAutoGrouping", true, true)]
	private bool m_autoGroupBySameEnemyRank;

	// Token: 0x04001488 RID: 5256
	private List<EnemyController> m_enemyGroup = new List<EnemyController>();

	// Token: 0x04001489 RID: 5257
	private EnemyController m_baseEnemy;

	// Token: 0x0400148A RID: 5258
	private bool m_isInitialized;

	// Token: 0x0400148B RID: 5259
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x0400148C RID: 5260
	private Action<object, HealthChangeEventArgs> m_onEnemyHPGain;
}
