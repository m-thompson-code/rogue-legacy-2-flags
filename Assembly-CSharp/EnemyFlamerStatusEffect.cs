using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000521 RID: 1313
public class EnemyFlamerStatusEffect : BaseStatusEffect
{
	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06002A56 RID: 10838 RVA: 0x00017B2B File Offset: 0x00015D2B
	public override string[] ProjectileNameArray
	{
		get
		{
			return EnemyFlamerStatusEffect.m_projectileNameArray;
		}
	}

	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06002A57 RID: 10839 RVA: 0x00017B32 File Offset: 0x00015D32
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Flamer;
		}
	}

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06002A58 RID: 10840 RVA: 0x00017B39 File Offset: 0x00015D39
	public override float StartingDurationOverride
	{
		get
		{
			return 2.1474836E+09f;
		}
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x00017B40 File Offset: 0x00015D40
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

	// Token: 0x06002A5A RID: 10842 RVA: 0x00017B4F File Offset: 0x00015D4F
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

	// Token: 0x0400245B RID: 9307
	private static string[] m_projectileNameArray = new string[]
	{
		"StatusEffectFireballProjectile"
	};

	// Token: 0x0400245C RID: 9308
	private float m_intervalTimer;
}
