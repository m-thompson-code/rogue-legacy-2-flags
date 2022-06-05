using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class DialogueDisplayOverride_Johan : DialogueDisplayOverride
{
	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x06001D80 RID: 7552 RVA: 0x0000F399 File Offset: 0x0000D599
	public bool SpawnIfFalse
	{
		get
		{
			return this.m_spawnIfFalse;
		}
	}

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0000F3A1 File Offset: 0x0000D5A1
	public JohanPropController.Johan_SpawnCondition SpawnCondition
	{
		get
		{
			return this.m_spawnCondition;
		}
	}

	// Token: 0x04001ACD RID: 6861
	[SerializeField]
	private JohanPropController.Johan_SpawnCondition m_spawnCondition;

	// Token: 0x04001ACE RID: 6862
	[SerializeField]
	private bool m_spawnIfFalse;
}
