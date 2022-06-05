using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AE7 RID: 2791
public class SpawnConditionalObjects_BuildStage : IBiomeBuildStage
{
	// Token: 0x0600539D RID: 21405 RVA: 0x0002D665 File Offset: 0x0002B865
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
