using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class IcarusAchievementTrigger : MonoBehaviour
{
	// Token: 0x060015FF RID: 5631 RVA: 0x000449EC File Offset: 0x00042BEC
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.CompareTag("Player") || collision.CompareTag("Player_Dodging")) && SaveManager.PlayerSaveData.GetRelic(RelicType.FlightBonusCurse).Level > 0)
		{
			StoreAPIManager.GiveAchievement(AchievementType.Icarus, StoreType.All);
		}
	}
}
