using System;

// Token: 0x020009DD RID: 2525
public interface ITurretSettingsConsumer
{
	// Token: 0x17001A83 RID: 6787
	// (get) Token: 0x06004CA8 RID: 19624
	float InitialFireDelay { get; }

	// Token: 0x17001A84 RID: 6788
	// (get) Token: 0x06004CA9 RID: 19625
	TurretLogicType LogicType { get; }

	// Token: 0x17001A85 RID: 6789
	// (get) Token: 0x06004CAA RID: 19626
	float LoopFireDelay { get; }

	// Token: 0x17001A86 RID: 6790
	// (get) Token: 0x06004CAB RID: 19627
	float ProjectileSpeedMod { get; }

	// Token: 0x17001A87 RID: 6791
	// (get) Token: 0x06004CAC RID: 19628
	bool UseHalfLoopDelay { get; }

	// Token: 0x06004CAD RID: 19629
	void SetInitialFireDelay(float value);

	// Token: 0x06004CAE RID: 19630
	void SetLogic(TurretLogicType logicType);

	// Token: 0x06004CAF RID: 19631
	void SetLoopFireDelay(float value);

	// Token: 0x06004CB0 RID: 19632
	void SetProjectileSpeedMod(float value);

	// Token: 0x06004CB1 RID: 19633
	void SetUseHalfLoopDelay(bool value);
}
