using System;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class DialogueDisplayOverride_Johan : DialogueDisplayOverride
{
	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x0600145F RID: 5215 RVA: 0x0003DE15 File Offset: 0x0003C015
	public bool SpawnIfFalse
	{
		get
		{
			return this.m_spawnIfFalse;
		}
	}

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x06001460 RID: 5216 RVA: 0x0003DE1D File Offset: 0x0003C01D
	public JohanPropController.Johan_SpawnCondition SpawnCondition
	{
		get
		{
			return this.m_spawnCondition;
		}
	}

	// Token: 0x04001426 RID: 5158
	[SerializeField]
	private JohanPropController.Johan_SpawnCondition m_spawnCondition;

	// Token: 0x04001427 RID: 5159
	[SerializeField]
	private bool m_spawnIfFalse;
}
