using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class DownstrikeProjectile_RL : Projectile_RL
{
	// Token: 0x170015DB RID: 5595
	// (get) Token: 0x06003B56 RID: 15190 RVA: 0x000208FF File Offset: 0x0001EAFF
	// (set) Token: 0x06003B57 RID: 15191 RVA: 0x00020906 File Offset: 0x0001EB06
	public static int ConsecutiveStrikes { get; set; }

	// Token: 0x170015DC RID: 5596
	// (get) Token: 0x06003B58 RID: 15192 RVA: 0x000F3AD4 File Offset: 0x000F1CD4
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

	// Token: 0x06003B59 RID: 15193 RVA: 0x0002090E File Offset: 0x0001EB0E
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

	// Token: 0x04002F28 RID: 12072
	private Action<Projectile_RL, GameObject> m_bounce;
}
