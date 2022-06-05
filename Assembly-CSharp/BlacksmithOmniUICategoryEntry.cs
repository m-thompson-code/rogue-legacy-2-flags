using System;

// Token: 0x02000624 RID: 1572
public class BlacksmithOmniUICategoryEntry : BaseOmniUICategoryEntry
{
	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x0600305A RID: 12378 RVA: 0x0001A838 File Offset: 0x00018A38
	// (set) Token: 0x0600305B RID: 12379 RVA: 0x0001A840 File Offset: 0x00018A40
	public EquipmentCategoryType CategoryType { get; protected set; }

	// Token: 0x0600305C RID: 12380 RVA: 0x0001A849 File Offset: 0x00018A49
	public void Initialize(EquipmentCategoryType categoryType, int entryIndex, BlacksmithOmniUIWindowController windowController)
	{
		this.CategoryType = categoryType;
		this.Initialize(entryIndex, windowController);
		this.m_iconSprite.sprite = IconLibrary.GetEquipmentCategoryIcon(categoryType);
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x000CEA74 File Offset: 0x000CCC74
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
