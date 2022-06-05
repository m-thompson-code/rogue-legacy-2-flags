using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000905 RID: 2309
public class DriftHouseInteriorController : MonoBehaviour
{
	// Token: 0x0600461F RID: 17951 RVA: 0x00112BE8 File Offset: 0x00110DE8
	private void Awake()
	{
		this.m_room = base.GetComponent<Room>();
		this.m_room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.ScarChallenges_Complete, true, 5f, null, null, null);
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x00026883 File Offset: 0x00024A83
	private void OnDestroy()
	{
		this.m_room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x00112C38 File Offset: 0x00110E38
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

	// Token: 0x06004622 RID: 17954 RVA: 0x00112D14 File Offset: 0x00110F14
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

	// Token: 0x06004623 RID: 17955 RVA: 0x000268A2 File Offset: 0x00024AA2
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

	// Token: 0x04003624 RID: 13860
	private Room m_room;

	// Token: 0x04003625 RID: 13861
	private float m_regenTick;

	// Token: 0x04003626 RID: 13862
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;
}
