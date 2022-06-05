using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class ExplosiveBreakable : Breakable
{
	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x0600275C RID: 10076 RVA: 0x00083044 File Offset: 0x00081244
	public override bool IsBroken
	{
		get
		{
			return base.IsBroken && !this.m_isExploding;
		}
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x00083059 File Offset: 0x00081259
	private void OnEnable()
	{
		ProjectileManager.Instance.AddProjectileToPool(this.m_explosionProjectileName);
		ProjectileManager.Instance.AddProjectileToPool(this.m_warningProjectileName);
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x0008307C File Offset: 0x0008127C
	private void OnDisable()
	{
		this.m_isExploding = false;
		if (this.m_warningProjectile && !this.m_warningProjectile.IsFreePoolObj && this.m_warningProjectile.Owner == base.gameObject)
		{
			this.m_warningProjectile.FlagForDestruction(null);
		}
		this.m_warningProjectile = null;
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x000830D5 File Offset: 0x000812D5
	protected override void TriggerCollision(IDamageObj damageObj)
	{
		if (!this.m_isExploding)
		{
			base.TriggerCollision(damageObj);
		}
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000830E6 File Offset: 0x000812E6
	protected override void Break(IDamageObj damageObj)
	{
		base.StartCoroutine(this.ExplosionCoroutine(damageObj));
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x000830F6 File Offset: 0x000812F6
	private IEnumerator ExplosionCoroutine(IDamageObj damageObj)
	{
		this.m_isExploding = true;
		base.BroadcastHitEvents(damageObj);
		Vector2 offsetPos = new Vector2(0f, this.m_hitboxController.GetCollider(HitboxType.Body).bounds.extents.y);
		if (!string.IsNullOrEmpty(this.m_warningProjectileName))
		{
			this.m_warningProjectile = ProjectileManager.FireProjectile(base.gameObject, this.m_warningProjectileName, offsetPos, false, 0f, 1f, false, true, true, true);
			this.m_warningProjectile.transform.SetParent(base.transform, true);
		}
		float blinkDuration = Time.time + this.m_explosionWarningDuration;
		float blinkInterval = 0.3f;
		while (Time.time < blinkDuration)
		{
			float blinkIntervalDuration = Time.time + blinkInterval;
			this.m_hitEffect.StartSingleBlinkEffect();
			while (Time.time < blinkIntervalDuration)
			{
				yield return null;
			}
			blinkInterval -= 0.05f;
			blinkInterval = Mathf.Max(blinkInterval, 0.15f);
		}
		ProjectileManager.FireProjectile(base.gameObject, this.m_explosionProjectileName, offsetPos, false, 0f, 1f, false, true, true, true);
		if (this.m_warningProjectile)
		{
			this.m_warningProjectile.FlagForDestruction(null);
		}
		this.m_warningProjectile = null;
		base.Break(damageObj);
		this.m_isExploding = false;
		yield break;
	}

	// Token: 0x040020FE RID: 8446
	[SerializeField]
	private string m_explosionProjectileName;

	// Token: 0x040020FF RID: 8447
	[SerializeField]
	private string m_warningProjectileName;

	// Token: 0x04002100 RID: 8448
	[SerializeField]
	private float m_explosionWarningDuration = 1f;

	// Token: 0x04002101 RID: 8449
	private bool m_isExploding;

	// Token: 0x04002102 RID: 8450
	private Projectile_RL m_warningProjectile;
}
