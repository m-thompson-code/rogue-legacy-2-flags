using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000673 RID: 1651
public class SpawnConditionalObjects_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B6E RID: 15214 RVA: 0x000CC63E File Offset: 0x000CA83E
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				foreach (ISimpleSpawnController simpleSpawnController in baseRoom.SpawnControllerManager.SimpleSpawnControllers_NoProps)
				{
					if (simpleSpawnController.gameObject.activeInHierarchy)
					{
						simpleSpawnController.Spawn();
					}
				}
			}
			yield break;
		}
		yield break;
	}
}
