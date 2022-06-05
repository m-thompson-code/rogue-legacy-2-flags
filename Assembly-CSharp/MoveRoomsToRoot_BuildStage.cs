using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000ADD RID: 2781
public class MoveRoomsToRoot_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005372 RID: 21362 RVA: 0x0002D586 File Offset: 0x0002B786
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				baseRoom.transform.SetParent(null, true);
			}
			yield break;
		}
		yield break;
	}
}
