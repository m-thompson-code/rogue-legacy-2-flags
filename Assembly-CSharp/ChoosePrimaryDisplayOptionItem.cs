using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class ChoosePrimaryDisplayOptionItem : SelectionListOptionItem
{
	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x0600235B RID: 9051 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000ACEEC File Offset: 0x000AB0EC
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

	// Token: 0x0600235D RID: 9053 RVA: 0x000ACF40 File Offset: 0x000AB140
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

	// Token: 0x0600235E RID: 9054 RVA: 0x000131E7 File Offset: 0x000113E7
	public override void InvokeValueChange()
	{
		Debug.Log("Change Primary Display to monitor: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x00013205 File Offset: 0x00011405
	public override void ConfirmOptionChange()
	{
		PlayerPrefs.SetInt("UnitySelectMonitor", this.m_selectedIndex);
	}
}
