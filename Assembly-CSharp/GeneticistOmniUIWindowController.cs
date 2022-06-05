using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200097F RID: 2431
public class GeneticistOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, GeneticistOmniUIEntry>
{
	// Token: 0x170019DC RID: 6620
	// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x00028D77 File Offset: 0x00026F77
	public override WindowID ID
	{
		get
		{
			return WindowID.Geneticist;
		}
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x00122BE8 File Offset: 0x00120DE8
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

	// Token: 0x06004AA6 RID: 19110 RVA: 0x00122D0C File Offset: 0x00120F0C
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

	// Token: 0x06004AA7 RID: 19111 RVA: 0x00028C7A File Offset: 0x00026E7A
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}
}
