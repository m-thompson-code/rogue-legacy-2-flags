using System;
using UnityEngine;

// Token: 0x02000939 RID: 2361
public class EarthShiftTunnel : Tunnel
{
	// Token: 0x06004791 RID: 18321 RVA: 0x0011631C File Offset: 0x0011451C
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
