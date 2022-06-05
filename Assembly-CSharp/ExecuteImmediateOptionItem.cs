using System;

// Token: 0x02000271 RID: 625
public abstract class ExecuteImmediateOptionItem : BaseOptionItem
{
	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x060018E8 RID: 6376 RVA: 0x0004E3BC File Offset: 0x0004C5BC
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.ExecuteImmediate;
		}
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x0004E3BF File Offset: 0x0004C5BF
	public override void ConfirmOptionChange()
	{
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0004E3C1 File Offset: 0x0004C5C1
	public override void CancelOptionChange()
	{
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x0004E3C3 File Offset: 0x0004C5C3
	public override void InvokeDecrement()
	{
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x0004E3C5 File Offset: 0x0004C5C5
	public override void InvokeIncrement()
	{
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0004E3C7 File Offset: 0x0004C5C7
	public override void ActivateOption()
	{
		if (base.OptionItemActivated != null)
		{
			base.OptionItemActivated(this);
		}
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0004E3DD File Offset: 0x0004C5DD
	public override void DeactivateOption(bool confirmOptionChange)
	{
	}

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x060018EF RID: 6383 RVA: 0x0004E3DF File Offset: 0x0004C5DF
	public override bool PressAndHoldEnabled
	{
		get
		{
			return false;
		}
	}
}
