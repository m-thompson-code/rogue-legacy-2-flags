using System;

// Token: 0x02000597 RID: 1431
public class ExplosiveChests_Trait : BaseTrait
{
	// Token: 0x17001206 RID: 4614
	// (get) Token: 0x06002D27 RID: 11559 RVA: 0x00018EDF File Offset: 0x000170DF
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ExplosiveChests;
		}
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x00018EE6 File Offset: 0x000170E6
	private void Start()
	{
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveChestsPotionProjectile");
		ProjectileManager.Instance.AddProjectileToPool("ExplosiveChestsPotionExplosionProjectile");
	}
}
