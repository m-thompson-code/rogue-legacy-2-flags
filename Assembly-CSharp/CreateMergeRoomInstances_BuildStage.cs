using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000ABE RID: 2750
public class CreateMergeRoomInstances_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052D5 RID: 21205 RVA: 0x0002D15A File Offset: 0x0002B35A
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
