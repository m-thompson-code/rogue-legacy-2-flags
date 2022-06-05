using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
public class ExplosiveBreakable : Breakable
{
	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x0600365F RID: 13919 RVA: 0x0001DE6E File Offset: 0x0001C06E
	public override bool IsBroken
	{
		get
		{
			return base.IsBroken && !this.m_isExploding;
		}
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x0001DE83 File Offset: 0x0001C083
	private void OnEnable()
	{
		ProjectileManager.Instance.AddProjectileToPool(this.m_explosionProjectileName);
		ProjectileManager.Instance.AddProjectileToPool(this.m_warningProjectileName);
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x000E3D34 File Offset: 0x000E1F34
	private void OnDisable()
	{
		this.m_isExploding = false;
		if (this.m_warningProjectile && !this.m_warningProjectile.IsFreePoolObj && this.m_warningProjectile.Owner == base.gameObject)
		{
			this.m_warningProjectile.FlagForDestruction(null);
		}
		this.m_warningProjectile = null;
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x0001DEA5 File Offset: 0x0001C0A5
	protected override void TriggerCollision(IDamageObj damageObj)
	{
		if (!this.m_isExploding)
		{
			base.TriggerCollision(damageObj);
		}
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x0001DEB6 File Offset: 0x0001C0B6
	protected override void Break(IDamageObj damageObj)
	{
		base.StartCoroutine(this.ExplosionCoroutine(damageObj));
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x0001DEC6 File Offset: 0x0001C0C6
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

	// Token: 0x04002C21 RID: 11297
	[SerializeField]
	private string m_explosionProjectileName;

	// Token: 0x04002C22 RID: 11298
	[SerializeField]
	private string m_warningProjectileName;

	// Token: 0x04002C23 RID: 11299
	[SerializeField]
	private float m_explosionWarningDuration = 1f;

	// Token: 0x04002C24 RID: 11300
	private bool m_isExploding;

	// Token: 0x04002C25 RID: 11301
	private Projectile_RL m_warningProjectile;
}
