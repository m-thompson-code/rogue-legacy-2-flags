using System;

// Token: 0x02000CA6 RID: 3238
public class SoulShopOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CDB RID: 23771 RVA: 0x000330B0 File Offset: 0x000312B0
	public SoulShopOmniUIDescriptionEventArgs(SoulShopType soulShopType, OmniUIButtonType buttonType)
	{
		this.Initialize(soulShopType, buttonType);
	}

	// Token: 0x06005CDC RID: 23772 RVA: 0x000330C0 File Offset: 0x000312C0
	public void Initialize(SoulShopType soulShopType, OmniUIButtonType buttonType)
	{
		this.SoulShopType = soulShopType;
		this.ButtonType = buttonType;
	}

	// Token: 0x17001EC9 RID: 7881
	// (get) Token: 0x06005CDD RID: 23773 RVA: 0x000330D0 File Offset: 0x000312D0
	// (set) Token: 0x06005CDE RID: 23774 RVA: 0x000330D8 File Offset: 0x000312D8
	public SoulShopType SoulShopType { get; private set; }

	// Token: 0x17001ECA RID: 7882
	// (get) Token: 0x06005CDF RID: 23775 RVA: 0x000330E1 File Offset: 0x000312E1
	// (set) Token: 0x06005CE0 RID: 23776 RVA: 0x000330E9 File Offset: 0x000312E9
	public OmniUIButtonType ButtonType { get; private set; }
}
