using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000660 RID: 1632
public class CloseDoors_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B2A RID: 15146 RVA: 0x000CB70F File Offset: 0x000C990F
	public IEnumerator Run(BiomeController biomeController)
	{
		if (biomeController.StandaloneRooms != null)
		{
			using (List<Room>.Enumerator enumerator = biomeController.StandaloneRooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Room room = enumerator.Current;
					BiomeCreatorTools.CloseUnconnectedDoorsInRoom(room);
				}
				yield break;
			}
		}
		Debug.LogFormat("<color=red>| {0} | {1} Biome's Biome Controller's list of standalone Rooms is null.</color>", new object[]
		{
			this,
			biomeController.Biome
		});
		yield break;
	}
}
