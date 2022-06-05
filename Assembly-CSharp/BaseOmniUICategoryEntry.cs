using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200061E RID: 1566
public abstract class BaseOmniUICategoryEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x0600300F RID: 12303 RVA: 0x0001A57E File Offset: 0x0001877E
	public IOmniUIWindowController WindowController
	{
		get
		{
			return this.m_windowController;
		}
	}

	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06003010 RID: 12304 RVA: 0x0001A586 File Offset: 0x00018786
	// (set) Token: 0x06003011 RID: 12305 RVA: 0x0001A58E File Offset: 0x0001878E
	public bool IsInitialized { get; private set; }

	// Token: 0x170012C7 RID: 4807
	// (get) Token: 0x06003012 RID: 12306 RVA: 0x0001A597 File Offset: 0x00018797
	// (set) Token: 0x06003013 RID: 12307 RVA: 0x0001A59F File Offset: 0x0001879F
	public int EntryIndex { get; private set; }

	// Token: 0x06003014 RID: 12308
	public abstract void UpdateState();

	// Token: 0x06003015 RID: 12309 RVA: 0x0001A5A8 File Offset: 0x000187A8
	protected virtual void Initialize(int entryIndex, IOmniUIWindowController windowController)
	{
		this.EntryIndex = entryIndex;
		this.m_windowController = windowController;
		this.IsInitialized = true;
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x0001A5BF File Offset: 0x000187BF
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (!this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x0001A5E4 File Offset: 0x000187E4
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x0001A609 File Offset: 0x00018809
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.m_windowController.SetHighlightedCategory(this);
		this.m_windowController.SetSelectedCategoryIndex(this.m_windowController.HighlightedCategoryIndex, true);
	}

	// Token: 0x0400277B RID: 10107
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x0400277C RID: 10108
	[SerializeField]
	protected Image m_newSymbol;

	// Token: 0x0400277D RID: 10109
	[SerializeField]
	protected Image m_upgradeSymbol;

	// Token: 0x0400277E RID: 10110
	[SerializeField]
	protected Image m_iconSprite;

	// Token: 0x0400277F RID: 10111
	private bool m_inputEnabled;

	// Token: 0x04002780 RID: 10112
	private IOmniUIWindowController m_windowController;
}
