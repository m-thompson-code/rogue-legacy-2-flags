using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000670 RID: 1648
public class SetFerrEdges_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B66 RID: 15206 RVA: 0x000CC474 File Offset: 0x000CA674
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
