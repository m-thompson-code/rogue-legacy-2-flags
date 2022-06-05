using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009F2 RID: 2546
public abstract class BiomeRule : ScriptableObject
{
	// Token: 0x17001A96 RID: 6806
	// (get) Token: 0x06004CCA RID: 19658
	public abstract BiomeRuleExecutionTime ExecutionTime { get; }

	// Token: 0x06004CCB RID: 19659
	public abstract IEnumerator RunRule(BiomeType biome);

	// Token: 0x06004CCC RID: 19660
	public abstract void UndoRule(BiomeType biome);

	// Token: 0x06004CCD RID: 19661 RVA: 0x00029C08 File Offset: 0x00027E08
	protected virtual void OnDisable()
	{
		if (Application.isPlaying && !GameManager.IsApplicationClosing)
		{
			this.UndoRule(BiomeType.None);
		}
	}
}
