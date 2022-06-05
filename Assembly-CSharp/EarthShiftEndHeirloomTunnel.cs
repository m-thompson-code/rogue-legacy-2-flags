using System;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class EarthShiftEndHeirloomTunnel : Tunnel
{
	// Token: 0x060032BA RID: 12986 RVA: 0x000AB9D4 File Offset: 0x000A9BD4
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
