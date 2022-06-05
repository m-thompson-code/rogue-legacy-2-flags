using System;

// Token: 0x020007D9 RID: 2009
public class EnchantressOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06004326 RID: 17190 RVA: 0x000EC4B1 File Offset: 0x000EA6B1
	public EnchantressOmniUIDescriptionEventArgs(RuneType runeType, OmniUIButtonType buttonType)
	{
		this.Initialize(runeType, buttonType);
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x000EC4C1 File Offset: 0x000EA6C1
	public void Initialize(RuneType runeType, OmniUIButtonType buttonType)
	{
		this.RuneType = runeType;
		this.ButtonType = buttonType;
	}

	// Token: 0x170016BC RID: 5820
	// (get) Token: 0x06004328 RID: 17192 RVA: 0x000EC4D1 File Offset: 0x000EA6D1
	// (set) Token: 0x06004329 RID: 17193 RVA: 0x000EC4D9 File Offset: 0x000EA6D9
	public RuneType RuneType { get; private set; }

	// Token: 0x170016BD RID: 5821
	// (get) Token: 0x0600432A RID: 17194 RVA: 0x000EC4E2 File Offset: 0x000EA6E2
	// (set) Token: 0x0600432B RID: 17195 RVA: 0x000EC4EA File Offset: 0x000EA6EA
	public OmniUIButtonType ButtonType { get; private set; }
}
