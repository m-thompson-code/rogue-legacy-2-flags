using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class DriftHouseInteriorController : MonoBehaviour
{
	// Token: 0x060031ED RID: 12781 RVA: 0x000A8D80 File Offset: 0x000A6F80
	private void Awake()
	{
		this.m_room = base.GetComponent<Room>();
		this.m_room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.ScarChallenges_Complete, true, 5f, null, null, null);
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x000A8DD0 File Offset: 0x000A6FD0
	private void OnDestroy()
	{
		this.m_room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x000A8DF0 File Offset: 0x000A6FF0
	private void FixedUpdate()
	{
		if (Time.time < this.m_regenTick + 0.05f)
		{
			return;
		}
		this.m_regenTick = Time.time;
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			float currentMana = playerController.CurrentMana;
			float num = (float)playerController.ActualMaxMana;
			if (currentMana < num)
			{
				float num2 = 100f * Time.fixedDeltaTime;
				if (currentMana + num2 > num)
				{
					playerController.SetMana(num, false, true, false);
				}
				else
				{
					playerController.SetMana(100f * Time.fixedDeltaTime, true, true, false);
				}
			}
			if (playerController.CurrentHealth < (float)playerController.ActualMaxHealth)
			{
				playerController.SetHealth(100f * Time.fixedDeltaTime, true, true);
			}
			if (playerController.CurrentArmor < playerController.ActualArmor)
			{
				playerController.CurrentArmor += Mathf.CeilToInt(100f * Time.fixedDeltaTime);
				playerController.SetHealth(0f, true, true);
			}
		}
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x000A8ECC File Offset: 0x000A70CC
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.ResetHealth();
			playerController.ResetMana();
		}
		if (WindowManager.GetIsWindowLoaded(WindowID.Jukebox))
		{
			(WindowManager.GetWindowController(WindowID.Jukebox) as JukeboxOmniUIWindowController).JukeboxSpectrum.StopSpectrum();
		}
		Debug.Log("trophy count: " + ChallengeManager.GetTotalTrophyCount().ToString());
		base.StartCoroutine(this.ResetEggplantHealthCoroutine());
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.ScarChallenges_Complete) < InsightState.ResolvedButNotViewed && (SaveManager.ModeSaveData.HasBronzeSisyphusTrophy || SaveManager.ModeSaveData.HasSilverSisyphusTrophy || SaveManager.ModeSaveData.HasGoldSisyphusTrophy))
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.ScarChallenges_Complete, InsightState.ResolvedButNotViewed, false);
			this.m_insightEventArgs.Initialize(InsightType.ScarChallenges_Complete, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.ScarChallenges_Complete) < InsightState.DiscoveredButNotViewed && ChallengeManager.GetTotalTrophiesEarned(false) >= ChallengeManager.GetTotalTrophyCount() - 1)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.ScarChallenges_Complete, InsightState.DiscoveredButNotViewed, false);
			this.m_insightEventArgs.Initialize(InsightType.ScarChallenges_Complete, true, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x000A8FFF File Offset: 0x000A71FF
	private IEnumerator ResetEggplantHealthCoroutine()
	{
		yield return null;
		foreach (EnemySpawnController enemySpawnController in this.m_room.SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.EnemyInstance)
			{
				enemySpawnController.EnemyInstance.ResetHealth();
			}
		}
		yield break;
	}

	// Token: 0x0400274F RID: 10063
	private Room m_room;

	// Token: 0x04002750 RID: 10064
	private float m_regenTick;

	// Token: 0x04002751 RID: 10065
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;
}
