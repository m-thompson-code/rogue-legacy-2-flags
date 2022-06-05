using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200066A RID: 1642
public class GenerateRoomSeeds_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B5A RID: 15194 RVA: 0x000CC339 File Offset: 0x000CA539
	public IEnumerator Run(BiomeController biomeController)
	{
		if (biomeController.GridPointManager != null && biomeController.GridPointManager.GridPointManagers != null && biomeController.GridPointManager.GridPointManagers.Count > 0)
		{
			using (List<GridPointManager>.Enumerator enumerator = biomeController.GridPointManager.GridPointManagers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GridPointManager gridPointManager = enumerator.Current;
					int randomNumber = RNGManager.GetRandomNumber(RngID.Room_RNGSeed, "GenerateRoomSeeds_BuildStage.Run()", 0, int.MaxValue);
					gridPointManager.SetRoomSeed(randomNumber);
				}
				yield break;
			}
		}
		Debug.LogFormat("<color=red>[{0}] BiomeController ({1})'s GridPointManager Property is null or empty.</color>", new object[]
		{
			this,
			biomeController.Biome
		});
		yield break;
	}
}
