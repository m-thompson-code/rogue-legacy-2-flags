using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AE5 RID: 2789
public class SimplifyColliders_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005395 RID: 21397 RVA: 0x0002D63F File Offset: 0x0002B83F
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
