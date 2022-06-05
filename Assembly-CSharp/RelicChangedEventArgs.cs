using System;

// Token: 0x020007EF RID: 2031
public class RelicChangedEventArgs : EventArgs
{
	// Token: 0x060043A0 RID: 17312 RVA: 0x000EC9E9 File Offset: 0x000EABE9
	public RelicChangedEventArgs(RelicType relicType)
	{
		this.Initialize(relicType);
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x000EC9F8 File Offset: 0x000EABF8
	public void Initialize(RelicType relicType)
	{
		this.RelicType = relicType;
	}

	// Token: 0x170016E3 RID: 5859
	// (get) Token: 0x060043A2 RID: 17314 RVA: 0x000ECA01 File Offset: 0x000EAC01
	// (set) Token: 0x060043A3 RID: 17315 RVA: 0x000ECA09 File Offset: 0x000EAC09
	public RelicType RelicType { get; private set; }
}
