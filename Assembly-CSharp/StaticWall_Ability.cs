using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class StaticWall_Ability : GenericSpell_Ability, ITalent, IAbility
{
	// Token: 0x06000E26 RID: 3622 RVA: 0x0002B7E0 File Offset: 0x000299E0
	protected override void Awake()
	{
		base.Awake();
		this.m_resumeCooldown = new Action<Projectile_RL, GameObject>(this.ResumeCooldown);
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0002B7FC File Offset: 0x000299FC
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

	// Token: 0x06000E28 RID: 3624 RVA: 0x0002B84F File Offset: 0x00029A4F
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

	// Token: 0x06000E29 RID: 3625 RVA: 0x0002B85E File Offset: 0x00029A5E
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		base.DecreaseCooldownWhenHit = true;
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x04001117 RID: 4375
	private const float FLASH_KICK_IN_DURATION = 1f;

	// Token: 0x04001118 RID: 4376
	private Action<Projectile_RL, GameObject> m_resumeCooldown;
}
