using System;
using UnityEngine;

// Token: 0x020004FB RID: 1275
[RequireComponent(typeof(Projectile_RL))]
public class AttachStatusEffectToProjectile : MonoBehaviour
{
	// Token: 0x06002923 RID: 10531 RVA: 0x00017370 File Offset: 0x00015570
	private void Awake()
	{
		this.m_projectile = base.GetComponent<Projectile_RL>();
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000BF8A4 File Offset: 0x000BDAA4
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

	// Token: 0x040023CE RID: 9166
	[SerializeField]
	private AttachStatusEffectToProjectile.ProjectileStatusEffectEntry[] m_statusEffectsToAttach;

	// Token: 0x040023CF RID: 9167
	[SerializeField]
	private bool m_extendDurationWithWeaponsBurnAddRelic;

	// Token: 0x040023D0 RID: 9168
	private Projectile_RL m_projectile;

	// Token: 0x020004FC RID: 1276
	[Serializable]
	private struct ProjectileStatusEffectEntry
	{
		// Token: 0x040023D1 RID: 9169
		public StatusEffectType StatusEffect;

		// Token: 0x040023D2 RID: 9170
		public float Duration;
	}
}
