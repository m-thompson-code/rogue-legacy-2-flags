using System;

// Token: 0x02000598 RID: 1432
public class ExplosiveEnemies_Trait : BaseTrait
{
	// Token: 0x17001207 RID: 4615
	// (get) Token: 0x06002D2A RID: 11562 RVA: 0x00018F06 File Offset: 0x00017106
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ExplosiveEnemies;
		}
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x00018F0D File Offset: 0x0001710D
	private void Start()
	{
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveEnemiesPotionProjectile");
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveEnemiesPotionExplosionProjectile");
	}
}
