using System;

// Token: 0x020004F3 RID: 1267
public class CaveBossRoomController : BossRoomController
{
	// Token: 0x06002F7D RID: 12157 RVA: 0x000A291C File Offset: 0x000A0B1C
	protected override void OnBossHealthChange(object sender, EventArgs args)
	{
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs != null && (EnemyController)healthChangeEventArgs.HealthObj == base.Boss)
		{
			base.OnBossHealthChange(sender, args);
		}
	}
}
