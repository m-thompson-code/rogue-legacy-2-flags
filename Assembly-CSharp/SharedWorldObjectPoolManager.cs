using System;

// Token: 0x020002E9 RID: 745
public static class SharedWorldObjectPoolManager
{
	// Token: 0x06001D85 RID: 7557 RVA: 0x00061292 File Offset: 0x0005F492
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

	// Token: 0x06001D86 RID: 7558 RVA: 0x000612BC File Offset: 0x0005F4BC
	public static void DestroyAllPools()
	{
		SharedWorldObjectPoolManager.DestroyBiomePools();
		TextPopupManager.DestroyPools();
		ItemDropManager.DestroyPools();
		ProjectileManager.DestroyOffscreenIconPool();
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000612D2 File Offset: 0x0005F4D2
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
