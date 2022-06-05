using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class GenerateRoomSeeds_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005356 RID: 21334 RVA: 0x0002D4D9 File Offset: 0x0002B6D9
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
