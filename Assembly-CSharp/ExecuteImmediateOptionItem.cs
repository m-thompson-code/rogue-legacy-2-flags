using System;

// Token: 0x0200043A RID: 1082
public abstract class ExecuteImmediateOptionItem : BaseOptionItem
{
	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x060022D7 RID: 8919 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.ExecuteImmediate;
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void ConfirmOptionChange()
	{
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void CancelOptionChange()
	{
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void InvokeDecrement()
	{
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void InvokeIncrement()
	{
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x00012A46 File Offset: 0x00010C46
	public override void ActivateOption()
	{
		if (base.OptionItemActivated != null)
		{
			base.OptionItemActivated(this);
		}
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void DeactivateOption(bool confirmOptionChange)
	{
	}

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x060022DE RID: 8926 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool PressAndHoldEnabled
	{
		get
		{
			return false;
		}
	}
}
