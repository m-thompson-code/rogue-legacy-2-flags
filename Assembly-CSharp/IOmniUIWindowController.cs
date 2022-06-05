using System;
using Rewired;
using UnityEngine.Events;

// Token: 0x02000582 RID: 1410
public interface IOmniUIWindowController
{
	// Token: 0x17001294 RID: 4756
	// (get) Token: 0x06003438 RID: 13368
	int SelectedCategoryIndex { get; }

	// Token: 0x17001295 RID: 4757
	// (get) Token: 0x06003439 RID: 13369
	int HighlightedCategoryIndex { get; }

	// Token: 0x17001296 RID: 4758
	// (get) Token: 0x0600343A RID: 13370
	int SelectedEntryIndex { get; }

	// Token: 0x17001297 RID: 4759
	// (get) Token: 0x0600343B RID: 13371
	bool IsInCategories { get; }

	// Token: 0x17001298 RID: 4760
	// (get) Token: 0x0600343C RID: 13372
	bool CanReset { get; }

	// Token: 0x17001299 RID: 4761
	// (get) Token: 0x0600343D RID: 13373
	Player RewiredPlayer { get; }

	// Token: 0x1700129A RID: 4762
	// (get) Token: 0x0600343E RID: 13374
	OmniUICommonSerializedFields CommonFields { get; }

	// Token: 0x0600343F RID: 13375
	void SetEntryNavigationEnabled(bool enable);

	// Token: 0x06003440 RID: 13376
	void SetKeyboardEnabled(bool enable);

	// Token: 0x06003441 RID: 13377
	void SetSelectedCategoryIndex(int index, bool playSFX);

	// Token: 0x06003442 RID: 13378
	void SetHighlightedCategoryIndex(int index, bool playSFX);

	// Token: 0x06003443 RID: 13379
	void SetHighlightedCategory(BaseOmniUICategoryEntry categoryEntry);

	// Token: 0x06003444 RID: 13380
	void SetSelectedEntryIndex(int index, bool playSFX, bool usingMouse);

	// Token: 0x1700129B RID: 4763
	// (get) Token: 0x06003445 RID: 13381
	UnityEvent SelectOptionEvent { get; }
}
