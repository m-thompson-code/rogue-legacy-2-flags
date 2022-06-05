using System;
using UnityEngine;

// Token: 0x02000938 RID: 2360
public class EarthShiftEndHeirloomTunnel : Tunnel
{
	// Token: 0x0600478F RID: 18319 RVA: 0x00116238 File Offset: 0x00114438
	public override void ForceEnterTunnel(bool animate, Tunnel tempDestinationOverride = null)
	{
		BiomeController biomeController;
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		else
		{
			biomeController = WorldBuilder.GetBiomeController(BiomeType.Garden);
		}
		Room room = null;
		if (biomeController)
		{
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				Room room2 = (Room)baseRoom;
				EndingSpawnRoomTypeController component = room2.GetComponent<EndingSpawnRoomTypeController>();
				if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.EarthShiftEscapeStart)
				{
					room = room2;
					break;
				}
			}
			if (room)
			{
				Tunnel tunnel = room.gameObject.FindObjectReference("HeirloomTunnel", false, false).Tunnel;
				base.Destination = tunnel;
			}
		}
		if (!room)
		{
			Debug.Log("Could not find Heirloom Entrance room.");
		}
		base.ForceEnterTunnel(animate, tempDestinationOverride);
	}
}
