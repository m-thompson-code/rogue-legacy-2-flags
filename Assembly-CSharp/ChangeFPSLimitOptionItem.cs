using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class ChangeFPSLimitOptionItem : SelectionListOptionItem
{
	// Token: 0x0600193E RID: 6462 RVA: 0x0004F2C4 File Offset: 0x0004D4C4
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

	// Token: 0x0600193F RID: 6463 RVA: 0x0004F328 File Offset: 0x0004D528
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

	// Token: 0x06001940 RID: 6464 RVA: 0x0004F3AD File Offset: 0x0004D5AD
	public override void InvokeValueChange()
	{
		Debug.Log("Set Max FPS to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x0004F3C4 File Offset: 0x0004D5C4
	public override void ConfirmOptionChange()
	{
		int num = System_EV.MAX_FPS_OPTIONS[this.m_selectedIndex];
		SaveManager.ConfigData.FPSLimit = num;
		Application.targetFrameRate = num;
	}
}
