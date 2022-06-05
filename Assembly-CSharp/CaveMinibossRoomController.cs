using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000851 RID: 2129
public class CaveMinibossRoomController : MinibossRoomController
{
	// Token: 0x060041AE RID: 16814 RVA: 0x0010858C File Offset: 0x0010678C
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Miniboss, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x060041AF RID: 16815 RVA: 0x001085C4 File Offset: 0x001067C4
	protected override void TeleportOut()
	{
		BiomeController biomeController = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(BiomeType.Cave);
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
				DragonCutsceneController component = baseRoom.gameObject.GetComponent<DragonCutsceneController>();
				base.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, component.TunnelSpawner.Tunnel);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
		else
		{
			if (base.TunnelSpawnController && base.TunnelSpawnController.Tunnel)
			{
				base.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
				return;
			}
			Debug.Log("Could not exit boss room, tunnel spawn controller is null.");
			return;
		}
	}

	// Token: 0x060041B0 RID: 16816 RVA: 0x0002460C File Offset: 0x0002280C
	protected override void OnBossDeath(object sender, EventArgs args)
	{
		base.OnBossDeath(sender, args);
		if (base.NumBossesDeadOrDying >= base.NumBosses)
		{
			MusicManager.StopMusic();
		}
	}
}
