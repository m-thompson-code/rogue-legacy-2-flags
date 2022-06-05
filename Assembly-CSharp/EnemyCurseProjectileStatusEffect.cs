using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051A RID: 1306
public class EnemyCurseProjectileStatusEffect : BaseStatusEffect
{
	// Token: 0x1700110F RID: 4367
	// (get) Token: 0x06002A29 RID: 10793 RVA: 0x000179CA File Offset: 0x00015BCA
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyCurseProjectileStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x06002A2A RID: 10794 RVA: 0x000179D1 File Offset: 0x00015BD1
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Curse_Projectile;
		}
	}

	// Token: 0x17001111 RID: 4369
	// (get) Token: 0x06002A2B RID: 10795 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000179D8 File Offset: 0x00015BD8
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_hitCooldownTimer = Time.time + 3.5f;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.CurseProjectile, 3.5f);
		while (Time.time < base.EndTime)
		{
			float num = CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, this.m_charController.Midpoint);
			if (Time.time > this.m_hitCooldownTimer && num < 35f * CameraController.ZoomLevel)
			{
				if (!base.IsHidden)
				{
					float angleInDeg = CDGHelper.VectorToAngle(this.m_charController.Midpoint - PlayerManager.GetPlayerController().Midpoint);
					ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectCurseProjectile", this.m_charController.Midpoint, false, angleInDeg, 1f, true, true, true, true);
				}
				this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.CurseProjectile, 3.5f);
				this.m_hitCooldownTimer = Time.time + 3.5f;
			}
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x04002445 RID: 9285
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectCurseProjectile"
	};

	// Token: 0x04002446 RID: 9286
	private float m_hitCooldownTimer;
}
