using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200066D RID: 1645
public class MergeTerrain_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B60 RID: 15200 RVA: 0x000CC42F File Offset: 0x000CA62F
	public IEnumerator Run(BiomeController biomeController)
	{
		Stopwatch timer = new Stopwatch();
		timer.Start();
		foreach (BaseRoom room in biomeController.Rooms)
		{
			RoomUtility.BuildAllFerr2DTerrains(room);
			if (timer.Elapsed.TotalMilliseconds >= 30.0)
			{
				yield return null;
				timer.Restart();
			}
		}
		List<BaseRoom>.Enumerator enumerator = default(List<BaseRoom>.Enumerator);
		yield break;
		yield break;
	}
}
