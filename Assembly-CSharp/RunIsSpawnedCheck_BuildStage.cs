using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200066F RID: 1647
public class RunIsSpawnedCheck_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B64 RID: 15204 RVA: 0x000CC45D File Offset: 0x000CA65D
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
