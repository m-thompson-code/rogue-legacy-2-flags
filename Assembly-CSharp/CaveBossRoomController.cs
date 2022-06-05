using System;

// Token: 0x0200084E RID: 2126
public class CaveBossRoomController : BossRoomController
{
	// Token: 0x060041A4 RID: 16804 RVA: 0x00108390 File Offset: 0x00106590
	protected override void OnBossHealthChange(object sender, EventArgs args)
	{
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs != null && (EnemyController)healthChangeEventArgs.HealthObj == base.Boss)
		{
			base.OnBossHealthChange(sender, args);
		}
	}
}
