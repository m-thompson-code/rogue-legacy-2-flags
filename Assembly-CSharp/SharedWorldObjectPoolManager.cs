using System;

// Token: 0x020004F3 RID: 1267
public static class SharedWorldObjectPoolManager
{
	// Token: 0x060028CA RID: 10442 RVA: 0x00016E3F File Offset: 0x0001503F
	public static void DestroyBiomePools()
	{
		ProjectileManager.DestroyBiomePools();
		PropManager.DestroyPools();
		EnemyManager.DestroyPools();
		HazardManager.DestroyPools();
		DecoManager.DestroyPools();
		ChestManager.DestroyPools();
		BackgroundsPoolManager.DestroyPools();
		EffectTriggerAnimBehaviour.ClearNormalizedStartCounters();
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x00016E69 File Offset: 0x00015069
	public static void DestroyAllPools()
	{
		SharedWorldObjectPoolManager.DestroyBiomePools();
		TextPopupManager.DestroyPools();
		ItemDropManager.DestroyPools();
		ProjectileManager.DestroyOffscreenIconPool();
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x00016E7F File Offset: 0x0001507F
	public static void CreateBiomePools(BiomeType newBiome)
	{
		ProjectileManager.ClearProjectileQueue();
		ProjectileManager.CreateBiomePools(newBiome);
		PropManager.CreateBiomePools(newBiome);
		EnemyManager.CreateBiomePools(newBiome);
		HazardManager.CreateBiomePools(newBiome);
		DecoManager.CreateBiomePools(newBiome);
		ChestManager.CreateBiomePools(newBiome);
		BackgroundsPoolManager.CreateBackgroundPools(newBiome);
		ProjectileManager.CreatePoolsFromQueuedProjectiles();
	}
}
