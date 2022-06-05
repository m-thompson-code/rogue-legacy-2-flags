using System;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class ChoosePrimaryDisplayOptionItem : SelectionListOptionItem
{
	// Token: 0x17000BF6 RID: 3062
	// (get) Token: 0x0600196C RID: 6508 RVA: 0x0004FBFA File Offset: 0x0004DDFA
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0004FC00 File Offset: 0x0004DE00
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = PlayerPrefs.GetInt("UnitySelectMonitor");
			if (this.m_selectedIndex >= this.m_selectionLocIDArray.Length)
			{
				this.m_selectedIndex = 0;
			}
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0004FC54 File Offset: 0x0004DE54
	public override void Initialize()
	{
		Display[] displays = Display.displays;
		this.m_selectionLocIDArray = new string[displays.Length];
		for (int i = 0; i < this.m_selectionLocIDArray.Length; i++)
		{
			this.m_selectionLocIDArray[i] = (i + 1).ToString();
		}
		this.m_selectedIndex = PlayerPrefs.GetInt("UnitySelectMonitor");
		if (this.m_selectedIndex >= this.m_selectionLocIDArray.Length)
		{
			this.m_selectedIndex = 0;
		}
		base.Initialize();
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x0004FCC8 File Offset: 0x0004DEC8
	public override void InvokeValueChange()
	{
		Debug.Log("Change Primary Display to monitor: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x0004FCE6 File Offset: 0x0004DEE6
	public override void ConfirmOptionChange()
	{
		PlayerPrefs.SetInt("UnitySelectMonitor", this.m_selectedIndex);
	}
}
