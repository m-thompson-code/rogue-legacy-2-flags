using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AE1 RID: 2785
public class SetFerrEdges_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005382 RID: 21378 RVA: 0x0002D5D2 File Offset: 0x0002B7D2
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				BiomeCreatorTools.SetOuterEdges(baseRoom.TerrainManager.Platforms, baseRoom.Bounds);
			}
			yield break;
		}
		yield break;
	}
}
