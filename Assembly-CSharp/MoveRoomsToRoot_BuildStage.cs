using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200066E RID: 1646
public class MoveRoomsToRoot_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B62 RID: 15202 RVA: 0x000CC446 File Offset: 0x000CA646
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
