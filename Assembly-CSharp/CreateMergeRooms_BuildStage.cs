using System;
using System.Collections;
using RLWorldCreation;

// Token: 0x02000663 RID: 1635
public class CreateMergeRooms_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B30 RID: 15152 RVA: 0x000CB762 File Offset: 0x000C9962
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
