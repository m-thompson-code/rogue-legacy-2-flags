using System;

// Token: 0x02000338 RID: 824
public class ExplosiveChests_Trait : BaseTrait
{
	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06002003 RID: 8195 RVA: 0x000660EE File Offset: 0x000642EE
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ExplosiveChests;
		}
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000660F5 File Offset: 0x000642F5
	private void Start()
	{
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveChestsPotionProjectile");
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveChestsPotionExplosionProjectile");
	}
}
