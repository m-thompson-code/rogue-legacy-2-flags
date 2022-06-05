using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200043F RID: 1087
public abstract class SelectionListOptionItem : BaseOptionItem
{
	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x060022FC RID: 8956 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool UsesLocID
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x060022FD RID: 8957 RVA: 0x000AC430 File Offset: 0x000AA630
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

	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x060022FE RID: 8958 RVA: 0x00004A8D File Offset: 0x00002C8D
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.SelectionList;
		}
	}

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x060022FF RID: 8959 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool PressAndHoldEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x00012BB3 File Offset: 0x00010DB3
	public override void Initialize()
	{
		base.Initialize();
		this.RefreshText(null, null);
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x00012BC3 File Offset: 0x00010DC3
	protected override void OnEnable()
	{
		base.OnEnable();
		this.RefreshText(null, null);
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000AC490 File Offset: 0x000AA690
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

	// Token: 0x06002303 RID: 8963 RVA: 0x000AC4FC File Offset: 0x000AA6FC
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

	// Token: 0x06002304 RID: 8964 RVA: 0x00012BD3 File Offset: 0x00010DD3
	public override void ActivateOption()
	{
		this.m_storedOnActivateIndex = this.m_selectedIndex;
		base.ActivateOption();
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x00012BE7 File Offset: 0x00010DE7
	public override void DeactivateOption(bool confirmOptionChange)
	{
		if (confirmOptionChange && this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
		base.DeactivateOption(confirmOptionChange);
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x00012C01 File Offset: 0x00010E01
	public override void CancelOptionChange()
	{
		this.m_selectedIndex = this.m_storedOnActivateIndex;
		if (!this.m_applyChangeOnlyOnExit)
		{
			this.InvokeValueChange();
		}
		this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
	}

	// Token: 0x06002307 RID: 8967
	public abstract void InvokeValueChange();

	// Token: 0x06002308 RID: 8968 RVA: 0x00012C2F File Offset: 0x00010E2F
	public override void RefreshText(object sender, EventArgs args)
	{
		if (base.IsInitialized)
		{
			this.m_incrementValueText.SetText(this.CurrentSelectionString, true);
		}
	}

	// Token: 0x04001F78 RID: 8056
	[SerializeField]
	protected UnityEvent m_selectionChangeEvent;

	// Token: 0x04001F79 RID: 8057
	protected string[] m_selectionLocIDArray;

	// Token: 0x04001F7A RID: 8058
	protected int m_selectedIndex;

	// Token: 0x04001F7B RID: 8059
	protected bool m_applyChangeOnlyOnExit = true;

	// Token: 0x04001F7C RID: 8060
	private int m_storedOnActivateIndex;
}
