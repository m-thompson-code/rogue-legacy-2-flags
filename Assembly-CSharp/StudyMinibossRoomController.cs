using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class StudyMinibossRoomController : MinibossRoomController
{
	// Token: 0x0600302D RID: 12333 RVA: 0x000A4DE8 File Offset: 0x000A2FE8
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Miniboss, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000A4E20 File Offset: 0x000A3020
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

	// Token: 0x0600302F RID: 12335 RVA: 0x000A4F5C File Offset: 0x000A315C
	protected override void OnBossDeath(object sender, EventArgs args)
	{
		base.OnBossDeath(sender, args);
		if (base.NumBossesDeadOrDying >= base.NumBosses)
		{
			MusicManager.StopMusic();
		}
	}
}
