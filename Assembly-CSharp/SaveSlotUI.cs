using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003DF RID: 991
public class SaveSlotUI : MonoBehaviour
{
	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06002488 RID: 9352 RVA: 0x00079C5C File Offset: 0x00077E5C
	// (set) Token: 0x06002489 RID: 9353 RVA: 0x00079C64 File Offset: 0x00077E64
	public byte SlotIndex { get; set; }

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x0600248A RID: 9354 RVA: 0x00079C6D File Offset: 0x00077E6D
	public Button Button
	{
		get
		{
			return this.m_button;
		}
	}

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x0600248B RID: 9355 RVA: 0x00079C75 File Offset: 0x00077E75
	// (set) Token: 0x0600248C RID: 9356 RVA: 0x00079C7D File Offset: 0x00077E7D
	public string PlayerData_FullFilePath { get; set; }

	// Token: 0x0600248D RID: 9357 RVA: 0x00079C86 File Offset: 0x00077E86
	protected virtual void Awake()
	{
		this.m_button = base.GetComponent<Button>();
	}

	// Token: 0x04001F13 RID: 7955
	private Button m_button;
}
