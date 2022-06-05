using System;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class EarthShiftTunnel : Tunnel
{
	// Token: 0x060032BC RID: 12988 RVA: 0x000ABAC0 File Offset: 0x000A9CC0
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		Tunnel destination = base.Destination;
		Room room = null;
		BiomeController biomeController;
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		else
		{
			biomeController = WorldBuilder.GetBiomeController(BiomeType.Garden);
		}
		foreach (BaseRoom baseRoom in biomeController.Rooms)
		{
			Room room2 = (Room)baseRoom;
			EndingSpawnRoomTypeController component = room2.GetComponent<EndingSpawnRoomTypeController>();
			if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.Docks)
			{
				room = room2;
				break;
			}
		}
		if (room)
		{
			TunnelSpawnController tunnelSpawnController = room.gameObject.FindObjectReference("ManorTunnel", false, false);
			this.m_tempDestinationOverride = tunnelSpawnController.Tunnel;
			RewiredMapController.SetIsInCutscene(true);
			CutsceneManager.InitializeCutscene(PlayerSaveFlag.None, destination);
			RoomSaveController.DisableCutsceneSaving = true;
			base.OnPlayerInteractedWithTunnel(otherObj);
			return;
		}
		base.OnPlayerInteractedWithTunnel(otherObj);
	}
}
