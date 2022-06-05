using System;

// Token: 0x02000339 RID: 825
public class ExplosiveEnemies_Trait : BaseTrait
{
	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x06002006 RID: 8198 RVA: 0x0006611D File Offset: 0x0006431D
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ExplosiveEnemies;
		}
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x00066124 File Offset: 0x00064324
	private void Start()
	{
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveEnemiesPotionProjectile");
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveEnemiesPotionExplosionProjectile");
	}
}
