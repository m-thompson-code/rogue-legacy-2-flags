using System;
using System.Collections.Generic;

// Token: 0x02000BB3 RID: 2995
public static class AnimationLayer_RL
{
	// Token: 0x06005A15 RID: 23061 RVA: 0x000312F6 File Offset: 0x0002F4F6
	public static int GetSquashAndStretchLayerIndex(EnemyType enemyType)
	{
		if (AnimationLayer_RL.ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE.ContainsKey(enemyType))
		{
			return AnimationLayer_RL.ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE[enemyType];
		}
		return AnimationLayer_RL.DEFAULT_SQUASH_AND_STRETCH_LAYER_INDEX;
	}

	// Token: 0x04004502 RID: 17666
	private static int DEFAULT_SQUASH_AND_STRETCH_LAYER_INDEX = 1;

	// Token: 0x04004503 RID: 17667
	private static Dictionary<EnemyType, int> ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE = new Dictionary<EnemyType, int>
	{
		{
			EnemyType.Skeleton,
			2
		}
	};
}
