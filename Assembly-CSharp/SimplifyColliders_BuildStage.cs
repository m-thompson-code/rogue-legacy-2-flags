using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000672 RID: 1650
public class SimplifyColliders_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B6C RID: 15212 RVA: 0x000CC627 File Offset: 0x000CA827
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom room = enumerator.Current;
				BiomeCreatorTools.SimplifyAllFerr2DColliders(room, true, true);
			}
			yield break;
		}
		yield break;
	}
}
