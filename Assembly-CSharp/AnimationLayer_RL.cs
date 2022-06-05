using System;
using System.Collections.Generic;

// Token: 0x02000700 RID: 1792
public static class AnimationLayer_RL
{
	// Token: 0x060040CC RID: 16588 RVA: 0x000E5763 File Offset: 0x000E3963
	public static int GetSquashAndStretchLayerIndex(EnemyType enemyType)
	{
		if (AnimationLayer_RL.ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE.ContainsKey(enemyType))
		{
			return AnimationLayer_RL.ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE[enemyType];
		}
		return AnimationLayer_RL.DEFAULT_SQUASH_AND_STRETCH_LAYER_INDEX;
	}

	// Token: 0x04003287 RID: 12935
	private static int DEFAULT_SQUASH_AND_STRETCH_LAYER_INDEX = 1;

	// Token: 0x04003288 RID: 12936
	private static Dictionary<EnemyType, int> ENEMY_SQUASH_AND_STRETCH_LAYER_INDEX_TABLE = new Dictionary<EnemyType, int>
	{
		{
			EnemyType.Skeleton,
			2
		}
	};
}
