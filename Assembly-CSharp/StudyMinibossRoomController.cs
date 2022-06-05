using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class StudyMinibossRoomController : MinibossRoomController
{
	// Token: 0x060042D7 RID: 17111 RVA: 0x0010858C File Offset: 0x0010678C
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Miniboss, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x0010BD2C File Offset: 0x00109F2C
	protected override void TeleportOut()
	{
		BiomeController biomeController = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(BiomeType.Study);
		BaseRoom baseRoom = null;
		foreach (BaseRoom baseRoom2 in biomeController.Rooms)
		{
			if (baseRoom2.RoomType == RoomType.BossEntrance)
			{
				baseRoom = baseRoom2;
				break;
			}
		}
		if (baseRoom && base.Room.BiomeType != BiomeType.Stone)
		{
			if (base.TunnelSpawnController && base.TunnelSpawnController.Tunnel)
			{
				RewiredMapController.SetIsInCutscene(true);
				CutsceneManager.InitializeCutscene(this.m_bossSaveFlag, base.TunnelSpawnController.Tunnel.Destination);
				PortraitCutsceneController component = baseRoom.gameObject.GetComponent<PortraitCutsceneController>();
				base.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, component.TunnelSpawner.Tunnel);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
		else
		{
			if (base.TunnelSpawnController && base.TunnelSpawnController.Tunnel != null)
			{
				base.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x0002460C File Offset: 0x0002280C
	protected override void OnBossDeath(object sender, EventArgs args)
	{
		base.OnBossDeath(sender, args);
		if (base.NumBossesDeadOrDying >= base.NumBosses)
		{
			MusicManager.StopMusic();
		}
	}
}
