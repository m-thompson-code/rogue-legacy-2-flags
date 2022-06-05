using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000587 RID: 1415
public class GeneticistOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, GeneticistOmniUIEntry>
{
	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x060034E6 RID: 13542 RVA: 0x000B5F60 File Offset: 0x000B4160
	public override WindowID ID
	{
		get
		{
			return WindowID.Geneticist;
		}
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x000B5F64 File Offset: 0x000B4164
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x000B5F68 File Offset: 0x000B4168
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		Array typeArray = TraitType_RL.TypeArray;
		base.EntryArray = new GeneticistOmniUIEntry[typeArray.Length - 1];
		int num = 0;
		foreach (object obj in typeArray)
		{
			TraitType traitType = (TraitType)obj;
			if (traitType != TraitType.None && !(TraitLibrary.GetTraitData(traitType) == null))
			{
				base.EntryArray[num] = UnityEngine.Object.Instantiate<GeneticistOmniUIEntry>(this.m_entryPrefab);
				base.EntryArray[num].transform.SetParent(base.EntryLayoutGroup.transform);
				base.EntryArray[num].transform.localScale = Vector3.one;
				base.EntryArray[num].Initialize(traitType, this);
				base.EntryArray[num].SetEntryIndex(num);
				num++;
			}
		}
		base.EntryArray = base.EntryArray.Take(num).ToArray<GeneticistOmniUIEntry>();
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x000B608C File Offset: 0x000B428C
	protected override void UpdateScrollArrows(float scrollAmount)
	{
		base.UpdateScrollArrows(scrollAmount);
		if (base.TopScrollArrow.activeSelf || base.BottomScrollArrow.activeSelf)
		{
			float num = (float)base.ActiveEntryArray.Length * this.m_entryHeight;
			float num2 = Mathf.Clamp(1f - base.ScrollBar.value, 0f, 1f);
			float height = base.ContentViewport.rect.height;
			float num3 = height;
			float num4 = num3 - height;
			if (num2 > 0f)
			{
				num3 = height + (num - height) * num2;
				num4 = num3 - height;
			}
			int num5 = Mathf.CeilToInt(num4 / this.m_entryHeight);
			int num6 = Mathf.FloorToInt(num3 / this.m_entryHeight);
			if (base.TopScrollArrow.activeSelf)
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < num5; i++)
				{
					if (TraitManager.GetTraitFoundState(base.ActiveEntryArray[i].TraitType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
				else if (flag2)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, true);
				}
				else
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					if (TraitManager.GetTraitFoundState(base.ActiveEntryArray[j].TraitType) == FoundState.FoundButNotViewed)
					{
						flag3 = true;
						break;
					}
				}
				if (flag3)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
					return;
				}
				if (flag4)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, true);
					return;
				}
				this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
				this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
			}
		}
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x000B62BA File Offset: 0x000B44BA
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}
}
