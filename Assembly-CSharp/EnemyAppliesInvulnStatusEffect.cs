using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class EnemyAppliesInvulnStatusEffect : BaseStatusEffect
{
	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x06002A02 RID: 10754 RVA: 0x00017907 File Offset: 0x00015B07
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyAppliesInvulnStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x06002A03 RID: 10755 RVA: 0x0001790E File Offset: 0x00015B0E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_AppliesInvuln;
		}
	}

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06002A04 RID: 10756 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x00017915 File Offset: 0x00015B15
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
		this.m_charController.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_Invuln, false);
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x00017935 File Offset: 0x00015B35
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

	// Token: 0x06002A07 RID: 10759 RVA: 0x000C0CA0 File Offset: 0x000BEEA0
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

	// Token: 0x06002A08 RID: 10760 RVA: 0x000C0DA0 File Offset: 0x000BEFA0
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

	// Token: 0x06002A09 RID: 10761 RVA: 0x000C0E10 File Offset: 0x000BF010
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

	// Token: 0x06002A0A RID: 10762 RVA: 0x000C0F0C File Offset: 0x000BF10C
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

	// Token: 0x04002436 RID: 9270
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectTeamInvincibilityProjectile"
	};

	// Token: 0x04002437 RID: 9271
	private Projectile_RL m_invulnRadiusProjectile;
}
