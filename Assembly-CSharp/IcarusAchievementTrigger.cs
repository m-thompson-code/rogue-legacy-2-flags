using System;
using UnityEngine;

// Token: 0x020003BE RID: 958
public class IcarusAchievementTrigger : MonoBehaviour
{
	// Token: 0x06001F8F RID: 8079 RVA: 0x000109C5 File Offset: 0x0000EBC5
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.CompareTag("Player") || collision.CompareTag("Player_Dodging")) && SaveManager.PlayerSaveData.GetRelic(RelicType.FlightBonusCurse).Level > 0)
		{
			StoreAPIManager.GiveAchievement(AchievementType.Icarus, StoreType.All);
		}
	}
}
