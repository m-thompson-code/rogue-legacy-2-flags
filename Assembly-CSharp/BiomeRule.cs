using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public abstract class BiomeRule : ScriptableObject
{
	// Token: 0x17001369 RID: 4969
	// (get) Token: 0x060036B8 RID: 14008
	public abstract BiomeRuleExecutionTime ExecutionTime { get; }

	// Token: 0x060036B9 RID: 14009
	public abstract IEnumerator RunRule(BiomeType biome);

	// Token: 0x060036BA RID: 14010
	public abstract void UndoRule(BiomeType biome);

	// Token: 0x060036BB RID: 14011 RVA: 0x000BC3A0 File Offset: 0x000BA5A0
	protected virtual void OnDisable()
	{
		if (Application.isPlaying && !GameManager.IsApplicationClosing)
		{
			this.UndoRule(BiomeType.None);
		}
	}
}
