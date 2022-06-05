using System;

// Token: 0x02000395 RID: 917
public class BlacksmithOmniUICategoryEntry : BaseOmniUICategoryEntry
{
	// Token: 0x17000E40 RID: 3648
	// (get) Token: 0x06002248 RID: 8776 RVA: 0x0006DCF5 File Offset: 0x0006BEF5
	// (set) Token: 0x06002249 RID: 8777 RVA: 0x0006DCFD File Offset: 0x0006BEFD
	public EquipmentCategoryType CategoryType { get; protected set; }

	// Token: 0x0600224A RID: 8778 RVA: 0x0006DD06 File Offset: 0x0006BF06
	public void Initialize(EquipmentCategoryType categoryType, int entryIndex, BlacksmithOmniUIWindowController windowController)
	{
		this.CategoryType = categoryType;
		this.Initialize(entryIndex, windowController);
		this.m_iconSprite.sprite = IconLibrary.GetEquipmentCategoryIcon(categoryType);
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x0006DD28 File Offset: 0x0006BF28
	public override void UpdateState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
		{
			if (equipmentType != EquipmentType.None)
			{
				EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(this.CategoryType, equipmentType);
				if (equipmentData && !equipmentData.Disabled)
				{
					if (EquipmentManager.GetFoundState(this.CategoryType, equipmentType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
					if (EquipmentManager.CanPurchaseEquipment(this.CategoryType, equipmentType, true))
					{
						flag2 = true;
						break;
					}
				}
			}
		}
		if (flag)
		{
			if (!this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(true);
			}
			if (this.m_upgradeSymbol.gameObject.activeSelf)
			{
				this.m_upgradeSymbol.gameObject.SetActive(false);
				return;
			}
		}
		else if (flag2)
		{
			if (this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(false);
			}
			if (!this.m_upgradeSymbol.gameObject.activeSelf)
			{
				this.m_upgradeSymbol.gameObject.SetActive(true);
				return;
			}
		}
		else
		{
			if (this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(false);
			}
			if (this.m_upgradeSymbol.gameObject.activeSelf)
			{
				this.m_upgradeSymbol.gameObject.SetActive(false);
			}
		}
	}
}
