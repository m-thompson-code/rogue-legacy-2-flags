using System;

// Token: 0x020005D5 RID: 1493
public interface ITurretSettingsConsumer
{
	// Token: 0x17001356 RID: 4950
	// (get) Token: 0x06003696 RID: 13974
	float InitialFireDelay { get; }

	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x06003697 RID: 13975
	TurretLogicType LogicType { get; }

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x06003698 RID: 13976
	float LoopFireDelay { get; }

	// Token: 0x17001359 RID: 4953
	// (get) Token: 0x06003699 RID: 13977
	float ProjectileSpeedMod { get; }

	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x0600369A RID: 13978
	bool UseHalfLoopDelay { get; }

	// Token: 0x0600369B RID: 13979
	void SetInitialFireDelay(float value);

	// Token: 0x0600369C RID: 13980
	void SetLogic(TurretLogicType logicType);

	// Token: 0x0600369D RID: 13981
	void SetLoopFireDelay(float value);

	// Token: 0x0600369E RID: 13982
	void SetProjectileSpeedMod(float value);

	// Token: 0x0600369F RID: 13983
	void SetUseHalfLoopDelay(bool value);
}
