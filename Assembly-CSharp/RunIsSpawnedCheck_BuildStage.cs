using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000ADF RID: 2783
public class RunIsSpawnedCheck_BuildStage : IBiomeBuildStage
{
	// Token: 0x0600537A RID: 21370 RVA: 0x0002D5AC File Offset: 0x0002B7AC
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				foreach (ISpawnController spawnController in baseRoom.SpawnControllerManager.SpawnControllers)
				{
					if (spawnController != null && spawnController.SpawnLogicController != null)
					{
						spawnController.SpawnLogicController.RunIsSpawnedCheck(SpawnScenarioCheckStage.PreMerge);
					}
				}
			}
			yield break;
		}
		yield break;
	}
}
