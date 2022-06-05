using System;

// Token: 0x02000CB5 RID: 3253
public class RelicChangedEventArgs : EventArgs
{
	// Token: 0x06005D29 RID: 23849 RVA: 0x000333FF File Offset: 0x000315FF
	public RelicChangedEventArgs(RelicType relicType)
	{
		this.Initialize(relicType);
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x0003340E File Offset: 0x0003160E
	public void Initialize(RelicType relicType)
	{
		this.RelicType = relicType;
	}

	// Token: 0x17001EE1 RID: 7905
	// (get) Token: 0x06005D2B RID: 23851 RVA: 0x00033417 File Offset: 0x00031617
	// (set) Token: 0x06005D2C RID: 23852 RVA: 0x0003341F File Offset: 0x0003161F
	public RelicType RelicType { get; private set; }
}
