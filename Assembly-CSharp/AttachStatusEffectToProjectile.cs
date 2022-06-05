using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
[RequireComponent(typeof(Projectile_RL))]
public class AttachStatusEffectToProjectile : MonoBehaviour
{
	// Token: 0x06001DD8 RID: 7640 RVA: 0x0006215E File Offset: 0x0006035E
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile_RL>();
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x0006216C File Offset: 0x0006036C
	private void OnEnable()
	{
		foreach (AttachStatusEffectToProjectile.ProjectileStatusEffectEntry projectileStatusEffectEntry in this.m_statusEffectsToAttach)
		{
			float num = projectileStatusEffectEntry.Duration;
			if (projectileStatusEffectEntry.StatusEffect == StatusEffectType.Enemy_Burn && this.m_extendDurationWithWeaponsBurnAddRelic)
			{
				int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsBurnAdd).Level;
				if (level > 0)
				{
					if (num == 0f)
					{
						num = 3.05f;
					}
					num += 2f * (float)level;
				}
			}
			this.m_projectile.AttachStatusEffect(projectileStatusEffectEntry.StatusEffect, num);
		}
	}

	// Token: 0x04001B80 RID: 7040
	[SerializeField]
	private AttachStatusEffectToProjectile.ProjectileStatusEffectEntry[] m_statusEffectsToAttach;

	// Token: 0x04001B81 RID: 7041
	[SerializeField]
	private bool m_extendDurationWithWeaponsBurnAddRelic;

	// Token: 0x04001B82 RID: 7042
	private Projectile_RL m_projectile;

	// Token: 0x02000B84 RID: 2948
	[Serializable]
	private struct ProjectileStatusEffectEntry
	{
		// Token: 0x04004CE3 RID: 19683
		public StatusEffectType StatusEffect;

		// Token: 0x04004CE4 RID: 19684
		public float Duration;
	}
}
