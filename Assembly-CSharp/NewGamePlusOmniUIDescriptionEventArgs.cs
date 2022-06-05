using System;

// Token: 0x02000CA5 RID: 3237
public class NewGamePlusOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CD5 RID: 23765 RVA: 0x0003306E File Offset: 0x0003126E
	public NewGamePlusOmniUIDescriptionEventArgs(BurdenType burdenType, OmniUIButtonType buttonType)
	{
		this.Initialize(burdenType, buttonType);
	}

	// Token: 0x06005CD6 RID: 23766 RVA: 0x0003307E File Offset: 0x0003127E
	public void Initialize(BurdenType burdenType, OmniUIButtonType buttonType)
	{
		this.BurdenType = burdenType;
		this.ButtonType = buttonType;
	}

	// Token: 0x17001EC7 RID: 7879
	// (get) Token: 0x06005CD7 RID: 23767 RVA: 0x0003308E File Offset: 0x0003128E
	// (set) Token: 0x06005CD8 RID: 23768 RVA: 0x00033096 File Offset: 0x00031296
	public BurdenType BurdenType { get; private set; }

	// Token: 0x17001EC8 RID: 7880
	// (get) Token: 0x06005CD9 RID: 23769 RVA: 0x0003309F File Offset: 0x0003129F
	// (set) Token: 0x06005CDA RID: 23770 RVA: 0x000330A7 File Offset: 0x000312A7
	public OmniUIButtonType ButtonType { get; private set; }
}
