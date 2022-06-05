using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public class EnemyInvulnWindowStatusEffect : BaseStatusEffect
{
	// Token: 0x17001138 RID: 4408
	// (get) Token: 0x06002A9B RID: 10907 RVA: 0x00017D4C File Offset: 0x00015F4C
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyInvulnWindowStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001139 RID: 4409
	// (get) Token: 0x06002A9C RID: 10908 RVA: 0x00017D53 File Offset: 0x00015F53
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_InvulnWindow;
		}
	}

	// Token: 0x1700113A RID: 4410
	// (get) Token: 0x06002A9D RID: 10909 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x00017D5A File Offset: 0x00015F5A
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x00017D74 File Offset: 0x00015F74
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.InvulnWindow);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		this.m_countdownStarted = false;
		while (!this.m_countdownStarted)
		{
			yield return null;
		}
		float duration = Time.time + 5f;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.InvulnWindow, 5f);
		float fireDelay = Time.time + 1.15f;
		while (Time.time < duration)
		{
			if (Time.time >= fireDelay)
			{
				float num = CDGHelper.VectorToAngle(PlayerManager.GetPlayerController().Midpoint - this.m_charController.Midpoint);
				float num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				num += 40f;
				num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_INVULNWINDOW_PROJECTILE_OFFSET.y);
				ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectInvulnWindowProjectile", this.m_charController.Midpoint, false, num + num2, 1f, true, true, true, true);
				fireDelay = Time.time + 1.25f;
			}
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000C1EC8 File Offset: 0x000C00C8
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		this.m_statusEffectController.RemoveStatusEffectInvulnStack(this.StatusEffectType);
		(this.m_charController as EnemyController).DisableDeath = false;
		this.m_countdownStarted = false;
		if (!GameManager.IsApplicationClosing && this.m_charController && !this.m_charController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Invuln) && BaseStatusEffect.m_matBlockHelper_STATIC != null && this.m_charController.RendererArray != null)
		{
			foreach (Renderer renderer in this.m_charController.RendererArray)
			{
				if (renderer && renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
				{
					renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 0);
					renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				}
			}
		}
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000C1FF0 File Offset: 0x000C01F0
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (this.m_countdownStarted)
		{
			return;
		}
		float num = (float)this.m_charController.ActualMaxHealth * 0.01f;
		num = Mathf.Max(1f, num);
		if (this.m_charController.CurrentHealth <= num)
		{
			this.m_countdownStarted = true;
			(this.m_charController as EnemyController).DisableDeath = true;
			this.m_statusEffectController.AddStatusEffectInvulnStack(this.StatusEffectType);
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
			foreach (Renderer renderer in this.m_charController.RendererArray)
			{
				if (renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
				{
					renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 1);
					renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				}
			}
		}
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x00017D83 File Offset: 0x00015F83
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x0400247B RID: 9339
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectInvulnWindowProjectile"
	};

	// Token: 0x0400247C RID: 9340
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x0400247D RID: 9341
	private bool m_countdownStarted;
}
