using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000275 RID: 629
public abstract class SelectionListOptionItem : BaseOptionItem
{
	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x0600190D RID: 6413 RVA: 0x0004EA87 File Offset: 0x0004CC87
	protected virtual bool UsesLocID
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x0600190E RID: 6414 RVA: 0x0004EA8C File Offset: 0x0004CC8C
	protected string CurrentSelectionString
	{
		get
		{
			if (this.m_selectionLocIDArray == null || this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_selectionLocIDArray.Length)
			{
				return string.Empty;
			}
			if (this.UsesLocID)
			{
				return LocalizationManager.GetString(this.m_selectionLocIDArray[this.m_selectedIndex], false, false);
			}
			return this.m_selectionLocIDArray[this.m_selectedIndex];
		}
	}

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x0600190F RID: 6415 RVA: 0x0004EAEA File Offset: 0x0004CCEA
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.SelectionList;
		}
	}

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x06001910 RID: 6416 RVA: 0x0004EAED File Offset: 0x0004CCED
	public override bool PressAndHoldEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x0004EAF0 File Offset: 0x0004CCF0
	public override void Initialize()
	{
		base.Initialize();
		this.RefreshText(null, null);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x0004EB00 File Offset: 0x0004CD00
	protected override void OnEnable()
	{
		base.OnEnable();
		this.RefreshText(null, null);
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x0004EB10 File Offset: 0x0004CD10
	public override void InvokeIncrement()
	{
		base.InvokeIncrement();
		this.m_selectedIndex++;
		if (this.m_selectedIndex >= this.m_selectionLocIDArray.Length)
		{
			this.m_selectedIndex = 0;
		}
		this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
		if (this.m_selectionChangeEvent != null)
		{
			this.m_selectionChangeEvent.Invoke();
		}
		if (!this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x0004EB7C File Offset: 0x0004CD7C
	public override void InvokeDecrement()
	{
		base.InvokeDecrement();
		this.m_selectedIndex--;
		if (this.m_selectedIndex < 0)
		{
			this.m_selectedIndex = this.m_selectionLocIDArray.Length - 1;
		}
		this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
		if (this.m_selectionChangeEvent != null)
		{
			this.m_selectionChangeEvent.Invoke();
		}
		if (!this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x0004EBE9 File Offset: 0x0004CDE9
	public override void ActivateOption()
	{
		this.m_storedOnActivateIndex = this.m_selectedIndex;
		base.ActivateOption();
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x0004EBFD File Offset: 0x0004CDFD
	public override void DeactivateOption(bool confirmOptionChange)
	{
		if (confirmOptionChange && this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
		base.DeactivateOption(confirmOptionChange);
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x0004EC17 File Offset: 0x0004CE17
	public override void CancelOptionChange()
	{
		this.m_selectedIndex = this.m_storedOnActivateIndex;
		if (!this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
		this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
	}

	// Token: 0x06001918 RID: 6424
	public abstract void InvokeValueChange();

	// Token: 0x06001919 RID: 6425 RVA: 0x0004EC45 File Offset: 0x0004CE45
	public override void RefreshText(object sender, EventArgs args)
	{
		if (base.IsInitialized)
		{
			this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
		}
	}

	// Token: 0x04001830 RID: 6192
	[SerializeField]
	protected UnityEvent m_selectionChangeEvent;

	// Token: 0x04001831 RID: 6193
	protected string[] m_selectionLocIDArray;

	// Token: 0x04001832 RID: 6194
	protected int m_selectedIndex;

	// Token: 0x04001833 RID: 6195
	protected bool m_applyChangeOnlyOnExit = true;

	// Token: 0x04001834 RID: 6196
	private int m_storedOnActivateIndex;
}
