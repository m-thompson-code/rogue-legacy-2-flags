using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class DownstrikeProjectile_RL : Projectile_RL
{
	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06002B13 RID: 11027 RVA: 0x00092009 File Offset: 0x00090209
	// (set) Token: 0x06002B14 RID: 11028 RVA: 0x00092010 File Offset: 0x00090210
	public static int ConsecutiveStrikes { get; set; }

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06002B15 RID: 11029 RVA: 0x00092018 File Offset: 0x00090218
	public override float ActualDamage
	{
		get
		{
			float num = 1f * SkillTreeLogicHelper.GetConsecutiveDownstrikeDamageMod();
			float num2;
			if (this.IgnoreDamageScale)
			{
				num2 = (this.Strength + this.Magic) * num;
			}
			else
			{
				num2 = this.Strength * this.StrengthScale + this.Magic * (this.MagicScale + num);
			}
			float num3 = 1f;
			num3 += Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.SpinKickDamage_Up);
			return num2 * num3;
		}
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x00092084 File Offset: 0x00090284
	protected override IEnumerator DestroyProjectileCoroutine(bool animate = false)
	{
		base.HitboxController.DisableAllCollisions = false;
		if (this.m_bounce == null)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			this.m_bounce = new Action<Projectile_RL, GameObject>(playerController.CharacterDownStrike.Bounce);
		}
		base.OnCollisionRelay.AddListener(this.m_bounce, false);
		yield return null;
		base.OnCollisionRelay.RemoveListener(this.m_bounce);
		base.HitboxController.DisableAllCollisions = true;
		yield return base.DestroyProjectileCoroutine(animate);
		yield break;
	}

	// Token: 0x04002318 RID: 8984
	private Action<Projectile_RL, GameObject> m_bounce;
}
