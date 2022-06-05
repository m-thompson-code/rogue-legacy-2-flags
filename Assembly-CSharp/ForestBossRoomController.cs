using System;

// Token: 0x02000868 RID: 2152
public class ForestBossRoomController : BossRoomController
{
	// Token: 0x06004255 RID: 16981 RVA: 0x00024BC7 File Offset: 0x00022DC7
	protected override void Awake()
	{
		this.m_sentrySpawners = base.gameObject.GetComponentsInChildren<IHazardSpawnController>();
		base.Awake();
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x0010A85C File Offset: 0x00108A5C
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

	// Token: 0x040033EF RID: 13295
	private IHazardSpawnController[] m_sentrySpawners;
}
