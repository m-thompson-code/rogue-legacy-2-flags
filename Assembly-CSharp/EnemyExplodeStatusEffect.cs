using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class EnemyExplodeStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x06001E82 RID: 7810 RVA: 0x00062F2E File Offset: 0x0006112E
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyExplodeStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x06001E83 RID: 7811 RVA: 0x00062F35 File Offset: 0x00061135
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Explode;
		}
	}

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x06001E84 RID: 7812 RVA: 0x00062F3C File Offset: 0x0006113C
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00062F43 File Offset: 0x00061143
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x00062F5D File Offset: 0x0006115D
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

	// Token: 0x06001E87 RID: 7815 RVA: 0x00062F6C File Offset: 0x0006116C
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (!this.m_isExploding && this.m_explodeCooldownTimer < Time.time)
		{
			this.m_explodeCoroutine = base.StartCoroutine(this.ExplodeCoroutine());
		}
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00062F95 File Offset: 0x00061195
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

	// Token: 0x06001E89 RID: 7817 RVA: 0x00062FA4 File Offset: 0x000611A4
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

	// Token: 0x06001E8A RID: 7818 RVA: 0x0006302D File Offset: 0x0006122D
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04001BB8 RID: 7096
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectExplosionProjectile",
		"StatusEffectExplosionWarningProjectile"
	};

	// Token: 0x04001BB9 RID: 7097
	private float m_explodeCooldownTimer;

	// Token: 0x04001BBA RID: 7098
	private bool m_warningExploded;

	// Token: 0x04001BBB RID: 7099
	private bool m_isExploding;

	// Token: 0x04001BBC RID: 7100
	private Projectile_RL m_warningProjectile;

	// Token: 0x04001BBD RID: 7101
	private Coroutine m_explodeCoroutine;

	// Token: 0x04001BBE RID: 7102
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
