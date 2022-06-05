using System;

// Token: 0x02000C9F RID: 3231
public class EnchantressOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CAF RID: 23727 RVA: 0x00032EC7 File Offset: 0x000310C7
	public EnchantressOmniUIDescriptionEventArgs(RuneType runeType, OmniUIButtonType buttonType)
	{
		this.Initialize(runeType, buttonType);
	}

	// Token: 0x06005CB0 RID: 23728 RVA: 0x00032ED7 File Offset: 0x000310D7
	public void Initialize(RuneType runeType, OmniUIButtonType buttonType)
	{
		this.RuneType = runeType;
		this.ButtonType = buttonType;
	}

	// Token: 0x17001EBA RID: 7866
	// (get) Token: 0x06005CB1 RID: 23729 RVA: 0x00032EE7 File Offset: 0x000310E7
	// (set) Token: 0x06005CB2 RID: 23730 RVA: 0x00032EEF File Offset: 0x000310EF
	public RuneType RuneType { get; private set; }

	// Token: 0x17001EBB RID: 7867
	// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x00032EF8 File Offset: 0x000310F8
	// (set) Token: 0x06005CB4 RID: 23732 RVA: 0x00032F00 File Offset: 0x00031100
	public OmniUIButtonType ButtonType { get; private set; }
}
