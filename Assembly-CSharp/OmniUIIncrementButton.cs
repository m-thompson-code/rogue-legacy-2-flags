using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200065A RID: 1626
public abstract class OmniUIIncrementButton<T> : OmniUIButton, IOmniUIIncrementButton
{
	// Token: 0x060031AB RID: 12715
	public abstract void InitializeIncrementList();

	// Token: 0x060031AC RID: 12716 RVA: 0x000D3B24 File Offset: 0x000D1D24
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

	// Token: 0x060031AD RID: 12717
	protected abstract void UpdateIncrementText();

	// Token: 0x04002875 RID: 10357
	[SerializeField]
	protected bool m_isDecrementButton;

	// Token: 0x04002876 RID: 10358
	[SerializeField]
	protected TMP_Text m_levelText;

	// Token: 0x04002877 RID: 10359
	protected List<T> m_incrementList;

	// Token: 0x04002878 RID: 10360
	protected int m_selectedIndex;
}
