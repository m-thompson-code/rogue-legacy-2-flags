using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000ADB RID: 2779
public class MergeTerrain_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005369 RID: 21353 RVA: 0x0002D546 File Offset: 0x0002B746
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
