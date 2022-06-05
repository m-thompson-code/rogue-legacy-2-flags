using System;
using Rewired;
using UnityEngine.Events;

// Token: 0x02000977 RID: 2423
public interface IOmniUIWindowController
{
	// Token: 0x1700199F RID: 6559
	// (get) Token: 0x060049E3 RID: 18915
	int SelectedCategoryIndex { get; }

	// Token: 0x170019A0 RID: 6560
	// (get) Token: 0x060049E4 RID: 18916
	int HighlightedCategoryIndex { get; }

	// Token: 0x170019A1 RID: 6561
	// (get) Token: 0x060049E5 RID: 18917
	int SelectedEntryIndex { get; }

	// Token: 0x170019A2 RID: 6562
	// (get) Token: 0x060049E6 RID: 18918
	bool IsInCategories { get; }

	// Token: 0x170019A3 RID: 6563
	// (get) Token: 0x060049E7 RID: 18919
	bool CanReset { get; }

	// Token: 0x170019A4 RID: 6564
	// (get) Token: 0x060049E8 RID: 18920
	Player RewiredPlayer { get; }

	// Token: 0x170019A5 RID: 6565
	// (get) Token: 0x060049E9 RID: 18921
	OmniUICommonSerializedFields CommonFields { get; }

	// Token: 0x060049EA RID: 18922
	void SetEntryNavigationEnabled(bool enable);

	// Token: 0x060049EB RID: 18923
	void SetKeyboardEnabled(bool enable);

	// Token: 0x060049EC RID: 18924
	void SetSelectedCategoryIndex(int index, bool playSFX);

	// Token: 0x060049ED RID: 18925
	void SetHighlightedCategoryIndex(int index, bool playSFX);

	// Token: 0x060049EE RID: 18926
	void SetHighlightedCategory(BaseOmniUICategoryEntry categoryEntry);

	// Token: 0x060049EF RID: 18927
	void SetSelectedEntryIndex(int index, bool playSFX, bool usingMouse);

	// Token: 0x170019A6 RID: 6566
	// (get) Token: 0x060049F0 RID: 18928
	UnityEvent SelectOptionEvent { get; }
}
