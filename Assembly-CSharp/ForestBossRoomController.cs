using System;

// Token: 0x02000500 RID: 1280
public class ForestBossRoomController : BossRoomController
{
	// Token: 0x06002FE1 RID: 12257 RVA: 0x000A3E18 File Offset: 0x000A2018
	protected override void Awake()
	{
		this.m_sentrySpawners = base.gameObject.GetComponentsInChildren<IHazardSpawnController>();
		base.Awake();
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x000A3E34 File Offset: 0x000A2034
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		IHazardSpawnController[] sentrySpawners = this.m_sentrySpawners;
		for (int i = 0; i < sentrySpawners.Length; i++)
		{
			Sentry_Hazard sentry_Hazard = sentrySpawners[i].Hazard as Sentry_Hazard;
			if (sentry_Hazard)
			{
				sentry_Hazard.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04002631 RID: 9777
	private IHazardSpawnController[] m_sentrySpawners;
}
