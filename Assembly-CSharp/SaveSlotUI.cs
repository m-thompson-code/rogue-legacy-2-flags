using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200067F RID: 1663
public class SaveSlotUI : MonoBehaviour
{
	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x060032C4 RID: 12996 RVA: 0x0001BC22 File Offset: 0x00019E22
	// (set) Token: 0x060032C5 RID: 12997 RVA: 0x0001BC2A File Offset: 0x00019E2A
	public byte SlotIndex { get; set; }

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x060032C6 RID: 12998 RVA: 0x0001BC33 File Offset: 0x00019E33
	public Button Button
	{
		get
		{
			return this.m_button;
		}
	}

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x060032C7 RID: 12999 RVA: 0x0001BC3B File Offset: 0x00019E3B
	// (set) Token: 0x060032C8 RID: 13000 RVA: 0x0001BC43 File Offset: 0x00019E43
	public string PlayerData_FullFilePath { get; set; }

	// Token: 0x060032C9 RID: 13001 RVA: 0x0001BC4C File Offset: 0x00019E4C
	protected virtual void Awake()
	{
		this.m_button = base.GetComponent<Button>();
	}

	// Token: 0x0400298B RID: 10635
	private Button m_button;
}
