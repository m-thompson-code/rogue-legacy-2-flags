using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ABA RID: 2746
public class CloseDoors_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052C5 RID: 21189 RVA: 0x0002D100 File Offset: 0x0002B300
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
