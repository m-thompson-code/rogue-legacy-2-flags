using System;
using System.Collections;
using RLWorldCreation;

// Token: 0x02000AC1 RID: 2753
public class CreateMergeRooms_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052DF RID: 21215 RVA: 0x0002D190 File Offset: 0x0002B390
	public IEnumerator Run(BiomeController biomeController)
	{
		MergeRoomStrategy mergeRoomStrategy;
		if (biomeController.Biome == BiomeType.Tower)
		{
			mergeRoomStrategy = new MergeRoomStrategy_Tower();
		}
		else if (biomeController.Biome == BiomeType.Stone)
		{
			mergeRoomStrategy = new MergeRoomStrategy_Bridge();
		}
		else
		{
			mergeRoomStrategy = new MergeRoomStrategy();
		}
		yield return mergeRoomStrategy.Run(this, biomeController);
		yield break;
	}
}
