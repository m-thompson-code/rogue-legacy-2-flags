using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000662 RID: 1634
public class CreateMergeRoomInstances_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B2E RID: 15150 RVA: 0x000CB74B File Offset: 0x000C994B
	public IEnumerator Run(BiomeController biomeController)
	{
		List<int> list = new List<int>();
		foreach (GridPointManager gridPointManager in biomeController.GridPointManager.GridPointManagers)
		{
			if (gridPointManager.MergeWithGridPointManagers.Count > 0 && !list.Contains(gridPointManager.BiomeControllerIndex))
			{
				list.Add(gridPointManager.BiomeControllerIndex);
			}
		}
		using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				int biomeControllerIndex = enumerator2.Current;
				List<Room> connectedRooms = (from entry in biomeController.StandaloneRooms
				where entry.BiomeControllerIndex == biomeControllerIndex
				select entry).ToList<Room>();
				MergeRoomTools.MergeRooms(biomeController, connectedRooms).SetBiomeControllerIndex(biomeControllerIndex);
			}
			yield break;
		}
		yield break;
	}
}
