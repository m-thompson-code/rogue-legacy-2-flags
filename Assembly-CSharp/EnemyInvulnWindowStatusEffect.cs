using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class EnemyInvulnWindowStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x06001EAE RID: 7854 RVA: 0x0006336F File Offset: 0x0006156F
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyInvulnWindowStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D50 RID: 3408
	// (get) Token: 0x06001EAF RID: 7855 RVA: 0x00063376 File Offset: 0x00061576
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_InvulnWindow;
		}
	}

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x0006337D File Offset: 0x0006157D
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x00063384 File Offset: 0x00061584
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x0006339E File Offset: 0x0006159E
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

	// Token: 0x06001EB3 RID: 7859 RVA: 0x000633B0 File Offset: 0x000615B0
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

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000634D8 File Offset: 0x000616D8
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

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000635E0 File Offset: 0x000617E0
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04001BC9 RID: 7113
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectInvulnWindowProjectile"
	};

	// Token: 0x04001BCA RID: 7114
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x04001BCB RID: 7115
	private bool m_countdownStarted;
}
