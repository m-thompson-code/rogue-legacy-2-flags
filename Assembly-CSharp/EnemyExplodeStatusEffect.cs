using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051E RID: 1310
public class EnemyExplodeStatusEffect : BaseStatusEffect
{
	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06002A3F RID: 10815 RVA: 0x00017A40 File Offset: 0x00015C40
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyExplodeStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06002A40 RID: 10816 RVA: 0x00017A47 File Offset: 0x00015C47
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Explode;
		}
	}

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06002A41 RID: 10817 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x00017A4E File Offset: 0x00015C4E
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x00017A68 File Offset: 0x00015C68
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_explodeCooldownTimer = 0f;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Explode);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x00017A77 File Offset: 0x00015C77
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (!this.m_isExploding && this.m_explodeCooldownTimer < Time.time)
		{
			this.m_explodeCoroutine = base.StartCoroutine(this.ExplodeCoroutine());
		}
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x00017AA0 File Offset: 0x00015CA0
	private IEnumerator ExplodeCoroutine()
	{
		this.m_isExploding = true;
		this.m_warningProjectile = ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectExplosionWarningProjectile", this.m_charController.Midpoint - this.m_charController.transform.localPosition, false, 0f, 1f, false, true, true, true);
		float num = this.m_warningProjectile.transform.localScale.x * 11f * 2f / this.m_warningProjectile.transform.lossyScale.x;
		this.m_warningProjectile.transform.localScale = new Vector3(num, num, this.m_warningProjectile.transform.localScale.z);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Explode, 1.25f);
		float warningDelay = Time.time + 1.25f;
		while (Time.time < warningDelay)
		{
			yield return null;
		}
		if (this.m_warningProjectile && !this.m_warningProjectile.IsFreePoolObj && this.m_warningProjectile.OwnerController == this.m_charController)
		{
			this.m_warningProjectile.FlagForDestruction(null);
		}
		Projectile_RL projectile_RL = ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectExplosionProjectile", this.m_charController.Midpoint - this.m_charController.transform.localPosition, false, 0f, 1f, false, true, true, true);
		num = projectile_RL.transform.localScale.x * 11f * 2f / projectile_RL.transform.lossyScale.x;
		projectile_RL.transform.localScale = new Vector3(num, num, projectile_RL.transform.localScale.z);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Explode);
		this.m_explodeCooldownTimer = Time.time + 4.25f;
		this.m_isExploding = false;
		yield break;
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000C1484 File Offset: 0x000BF684
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		this.m_isExploding = false;
		if (this.m_explodeCoroutine != null)
		{
			base.StopCoroutine(this.m_explodeCoroutine);
		}
		if (this.m_warningProjectile && !this.m_warningProjectile.IsFreePoolObj && this.m_warningProjectile.OwnerController == this.m_charController)
		{
			this.m_warningProjectile.FlagForDestruction(null);
		}
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x00017AAF File Offset: 0x00015CAF
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x0400244D RID: 9293
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectExplosionProjectile",
		"StatusEffectExplosionWarningProjectile"
	};

	// Token: 0x0400244E RID: 9294
	private float m_explodeCooldownTimer;

	// Token: 0x0400244F RID: 9295
	private bool m_warningExploded;

	// Token: 0x04002450 RID: 9296
	private bool m_isExploding;

	// Token: 0x04002451 RID: 9297
	private Projectile_RL m_warningProjectile;

	// Token: 0x04002452 RID: 9298
	private Coroutine m_explodeCoroutine;

	// Token: 0x04002453 RID: 9299
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
