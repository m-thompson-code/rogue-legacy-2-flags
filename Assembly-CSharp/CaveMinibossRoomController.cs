using System;
using RLAudio;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public class CaveMinibossRoomController : MinibossRoomController
{
	// Token: 0x06002F87 RID: 12167 RVA: 0x000A2B9C File Offset: 0x000A0D9C
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Miniboss, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x000A2BD4 File Offset: 0x000A0DD4
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

	// Token: 0x06002F89 RID: 12169 RVA: 0x000A2D10 File Offset: 0x000A0F10
	protected override void OnBossDeath(object sender, EventArgs args)
	{
		base.OnBossDeath(sender, args);
		if (base.NumBossesDeadOrDying >= base.NumBosses)
		{
			MusicManager.StopMusic();
		}
	}
}
