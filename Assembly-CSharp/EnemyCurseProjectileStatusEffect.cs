using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class EnemyCurseProjectileStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x06001E78 RID: 7800 RVA: 0x00062EC8 File Offset: 0x000610C8
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyCurseProjectileStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x06001E79 RID: 7801 RVA: 0x00062ECF File Offset: 0x000610CF
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Curse_Projectile;
		}
	}

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x06001E7A RID: 7802 RVA: 0x00062ED6 File Offset: 0x000610D6
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x00062EDD File Offset: 0x000610DD
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

	// Token: 0x04001BB6 RID: 7094
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectCurseProjectile"
	};

	// Token: 0x04001BB7 RID: 7095
	private float m_hitCooldownTimer;
}
