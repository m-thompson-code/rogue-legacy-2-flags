using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class EnemyFlamerStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00063083 File Offset: 0x00061283
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyFlamerStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0006308A File Offset: 0x0006128A
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Flamer;
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x06001E8F RID: 7823 RVA: 0x00063091 File Offset: 0x00061291
	public override float StartingDurationOverride
	{
		get
		{
			return 2.1474836E+09f;
		}
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x00063098 File Offset: 0x00061298
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_intervalTimer = Time.time + 2.25f;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Flamer, 2.25f);
		while (Time.time < base.EndTime)
		{
			float num = CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, this.m_charController.Midpoint);
			if (Time.time > this.m_intervalTimer && num < 35f * CameraController.ZoomLevel)
			{
				if (!base.IsHidden)
				{
					yield return this.FireProjectileCoroutine();
				}
				this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Flamer, 2.25f);
				this.m_intervalTimer = Time.time + 2.25f;
			}
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x000630A7 File Offset: 0x000612A7
	private IEnumerator FireProjectileCoroutine()
	{
		int num3;
		for (int i = 0; i < 2; i = num3 + 1)
		{
			float num = CDGHelper.VectorToAngle(PlayerManager.GetPlayerController().Midpoint - this.m_charController.Midpoint);
			float num2 = UnityEngine.Random.Range(StatusEffect_EV.ENEMY_FLAME_PROJECTILE_OFFSET.x, StatusEffect_EV.ENEMY_FLAME_PROJECTILE_OFFSET.y);
			num += num2;
			ProjectileManager.FireProjectile(this.m_charController.gameObject, "StatusEffectFireballProjectile", this.m_charController.Midpoint, false, num, 1f, true, true, true, true);
			float delay = Time.time + 0.25f;
			while (Time.time < delay)
			{
				yield return null;
			}
			num3 = i;
		}
		yield break;
	}

	// Token: 0x04001BBF RID: 7103
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectFireballProjectile"
	};

	// Token: 0x04001BC0 RID: 7104
	private float m_intervalTimer;
}
