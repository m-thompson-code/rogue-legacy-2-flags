using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class ChangeFPSLimitOptionItem : SelectionListOptionItem
{
	// Token: 0x0600232D RID: 9005 RVA: 0x000AC964 File Offset: 0x000AAB64
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = System_EV.MAX_FPS_OPTIONS.IndexOf(SaveManager.ConfigData.FPSLimit);
			if (this.m_selectedIndex == -1)
			{
				this.m_selectedIndex = System_EV.MAX_FPS_OPTIONS.IndexOf(120);
			}
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000AC9C8 File Offset: 0x000AABC8
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[System_EV.MAX_FPS_OPTIONS.Length];
		for (int i = 0; i < this.m_selectionLocIDArray.Length; i++)
		{
			this.m_selectionLocIDArray[i] = System_EV.MAX_FPS_OPTIONS[i].ToString();
		}
		this.m_selectedIndex = System_EV.MAX_FPS_OPTIONS.IndexOf(SaveManager.ConfigData.FPSLimit);
		if (this.m_selectedIndex == -1)
		{
			this.m_selectedIndex = System_EV.MAX_FPS_OPTIONS.IndexOf(120);
		}
		base.Initialize();
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x00012E92 File Offset: 0x00011092
	public override void InvokeValueChange()
	{
		Debug.Log("Set Max FPS to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000ACA50 File Offset: 0x000AAC50
	public override void ConfirmOptionChange()
	{
		int num = System_EV.MAX_FPS_OPTIONS[this.m_selectedIndex];
		SaveManager.ConfigData.FPSLimit = num;
		Application.targetFrameRate = num;
	}
}
