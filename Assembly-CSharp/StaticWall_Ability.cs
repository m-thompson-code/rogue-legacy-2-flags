using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D8 RID: 728
public class StaticWall_Ability : GenericSpell_Ability, ITalent, IAbility
{
	// Token: 0x060015BF RID: 5567 RVA: 0x0000AC8A File Offset: 0x00008E8A
	protected override void Awake()
	{
		base.Awake();
		this.m_resumeCooldown = new Action<Projectile_RL, GameObject>(this.ResumeCooldown);
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x0008A9BC File Offset: 0x00088BBC
	protected override void FireProjectile()
	{
		base.FireProjectile();
		base.StartCoroutine(this.FlashCoroutine());
		base.DecreaseCooldownWhenHit = false;
		base.DisplayPausedAbilityCooldown = true;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnDeathRelay.AddOnce(this.m_resumeCooldown, false);
		}
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x0000ACA4 File Offset: 0x00008EA4
	private IEnumerator FlashCoroutine()
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.Animator.SetBool("Flashing", false);
			float lifespan = Time.time + this.m_firedProjectile.Lifespan - 1f;
			while (Time.time < lifespan)
			{
				yield return null;
			}
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.Animator.SetBool("Flashing", true);
			}
		}
		yield break;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0000ACB3 File Offset: 0x00008EB3
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownWhenHit = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x0400169A RID: 5786
	private const float FLASH_KICK_IN_DURATION = 1f;

	// Token: 0x0400169B RID: 5787
	private Action<Projectile_RL, GameObject> m_resumeCooldown;
}
