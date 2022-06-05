using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public abstract class OmniUIIncrementButton<T> : OmniUIButton, IOmniUIIncrementButton
{
	// Token: 0x0600238D RID: 9101
	public abstract void InitializeIncrementList();

	// Token: 0x0600238E RID: 9102 RVA: 0x00073820 File Offset: 0x00071A20
	public override void OnConfirmButtonPressed()
	{
		if (this.m_incrementList == null || this.m_incrementList.Count <= 0)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		int selectedIndex = this.m_selectedIndex;
		if (this.m_isDecrementButton)
		{
			this.m_selectedIndex--;
		}
		else
		{
			this.m_selectedIndex++;
		}
		if (this.m_selectedIndex < 0)
		{
			this.m_selectedIndex = this.m_incrementList.Count - 1;
		}
		else if (this.m_selectedIndex >= this.m_incrementList.Count)
		{
			this.m_selectedIndex = 0;
		}
		if (this.m_selectedIndex != selectedIndex)
		{
			this.UpdateIncrementText();
		}
	}

	// Token: 0x0600238F RID: 9103
	protected abstract void UpdateIncrementText();

	// Token: 0x04001E3C RID: 7740
	[SerializeField]
	protected bool m_isDecrementButton;

	// Token: 0x04001E3D RID: 7741
	[SerializeField]
	protected TMP_Text m_levelText;

	// Token: 0x04001E3E RID: 7742
	protected List<T> m_incrementList;

	// Token: 0x04001E3F RID: 7743
	protected int m_selectedIndex;
}
