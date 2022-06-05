using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class EnemyAppliesInvulnStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x06001E63 RID: 7779 RVA: 0x00062AD5 File Offset: 0x00060CD5
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyAppliesInvulnStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x06001E64 RID: 7780 RVA: 0x00062ADC File Offset: 0x00060CDC
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_AppliesInvuln;
		}
	}

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06001E65 RID: 7781 RVA: 0x00062AE3 File Offset: 0x00060CE3
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x00062AEA File Offset: 0x00060CEA
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
		this.m_charController.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x00062B0A File Offset: 0x00060D0A
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.AppliesInvuln);
		EnemyController enemyController = this.m_charController as EnemyController;
		BaseRoom currentRoom = PlayerManager.GetCurrentPlayerRoom();
		if (!base.IsHidden)
		{
			this.CreateInvulnProjectileEffect();
		}
		while (Time.time < base.EndTime)
		{
			foreach (EnemySpawnController enemySpawnController in currentRoom.SpawnControllerManager.EnemySpawnControllers)
			{
				if (!enemySpawnController.IsDead && enemySpawnController.ShouldSpawn)
				{
					EnemyController enemyInstance = enemySpawnController.EnemyInstance;
					this.ApplyInvulnToEnemy(enemyInstance, enemyController);
				}
			}
			foreach (EnemyController enemy in EnemyManager.SummonedEnemyList)
			{
				this.ApplyInvulnToEnemy(enemy, enemyController);
			}
			yield return null;
		}
		EnemySpawnController[] enemySpawnControllers = currentRoom.SpawnControllerManager.EnemySpawnControllers;
		for (int i = 0; i < enemySpawnControllers.Length; i++)
		{
			EnemyController enemyInstance2 = enemySpawnControllers[i].EnemyInstance;
			if (enemyInstance2)
			{
				enemyInstance2.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
			}
		}
		foreach (EnemyController enemyController2 in EnemyManager.SummonedEnemyList)
		{
			enemyController2.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x00062B1C File Offset: 0x00060D1C
	private void CreateInvulnProjectileEffect()
	{
		if (this.m_invulnRadiusProjectile && !this.m_invulnRadiusProjectile.IsFreePoolObj && this.m_invulnRadiusProjectile.OwnerController == this.m_charController)
		{
			this.m_invulnRadiusProjectile.FlagForDestruction(null);
		}
		this.m_invulnRadiusProjectile = ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectTeamInvincibilityProjectile", this.m_charController.Midpoint - this.m_charController.transform.localPosition, false, 0f, 1f, false, true, true, true);
		float num = this.m_invulnRadiusProjectile.transform.localScale.x * 25f * 2f / this.m_invulnRadiusProjectile.transform.lossyScale.x;
		this.m_invulnRadiusProjectile.transform.localScale = new Vector3(num, num, this.m_invulnRadiusProjectile.transform.localScale.z);
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x00062C1C File Offset: 0x00060E1C
	private void ApplyInvulnToEnemy(EnemyController enemy, EnemyController sourceEnemyController)
	{
		if (!enemy || enemy.IsDead)
		{
			return;
		}
		if (enemy.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_AppliesInvuln))
		{
			return;
		}
		if (CDGHelper.DistanceBetweenPts(this.m_charController.Midpoint, enemy.Midpoint) <= 25f)
		{
			enemy.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_Invuln, 0f, sourceEnemyController);
		}
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x00062C8C File Offset: 0x00060E8C
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (this.m_invulnRadiusProjectile && !this.m_invulnRadiusProjectile.IsFreePoolObj && this.m_invulnRadiusProjectile.OwnerController == this.m_charController)
		{
			this.m_invulnRadiusProjectile.FlagForDestruction(null);
		}
		this.m_invulnRadiusProjectile = null;
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (currentPlayerRoom)
			{
				EnemySpawnController[] enemySpawnControllers = currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers;
				for (int i = 0; i < enemySpawnControllers.Length; i++)
				{
					EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
					if (enemyInstance)
					{
						enemyInstance.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
					}
				}
				foreach (EnemyController enemyController in EnemyManager.SummonedEnemyList)
				{
					enemyController.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
				}
			}
		}
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x00062D88 File Offset: 0x00060F88
	public override void SetIsHidden(bool hide)
	{
		base.SetIsHidden(hide);
		if (!base.IsPlaying)
		{
			return;
		}
		if (hide)
		{
			if (this.m_invulnRadiusProjectile && !this.m_invulnRadiusProjectile.IsFreePoolObj && this.m_invulnRadiusProjectile.OwnerController == this.m_charController)
			{
				this.m_invulnRadiusProjectile.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.CreateInvulnProjectileEffect();
		}
	}

	// Token: 0x04001BB2 RID: 7090
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectTeamInvincibilityProjectile"
	};

	// Token: 0x04001BB3 RID: 7091
	private Projectile_RL m_invulnRadiusProjectile;
}
