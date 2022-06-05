using System;

// Token: 0x020007E0 RID: 2016
public class SoulShopOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06004352 RID: 17234 RVA: 0x000EC69A File Offset: 0x000EA89A
	public SoulShopOmniUIDescriptionEventArgs(SoulShopType soulShopType, OmniUIButtonType buttonType)
	{
		this.Initialize(soulShopType, buttonType);
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x000EC6AA File Offset: 0x000EA8AA
	public void Initialize(SoulShopType soulShopType, OmniUIButtonType buttonType)
	{
		this.SoulShopType = soulShopType;
		this.ButtonType = buttonType;
	}

	// Token: 0x170016CB RID: 5835
	// (get) Token: 0x06004354 RID: 17236 RVA: 0x000EC6BA File Offset: 0x000EA8BA
	// (set) Token: 0x06004355 RID: 17237 RVA: 0x000EC6C2 File Offset: 0x000EA8C2
	public SoulShopType SoulShopType { get; private set; }

	// Token: 0x170016CC RID: 5836
	// (get) Token: 0x06004356 RID: 17238 RVA: 0x000EC6CB File Offset: 0x000EA8CB
	// (set) Token: 0x06004357 RID: 17239 RVA: 0x000EC6D3 File Offset: 0x000EA8D3
	public OmniUIButtonType ButtonType { get; private set; }
}
