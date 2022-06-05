using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000390 RID: 912
public abstract class BaseOmniUICategoryEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x060021FD RID: 8701 RVA: 0x0006C5BE File Offset: 0x0006A7BE
	public IOmniUIWindowController WindowController
	{
		get
		{
			return this.m_windowController;
		}
	}

	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x060021FE RID: 8702 RVA: 0x0006C5C6 File Offset: 0x0006A7C6
	// (set) Token: 0x060021FF RID: 8703 RVA: 0x0006C5CE File Offset: 0x0006A7CE
	public bool IsInitialized { get; private set; }

	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x06002200 RID: 8704 RVA: 0x0006C5D7 File Offset: 0x0006A7D7
	// (set) Token: 0x06002201 RID: 8705 RVA: 0x0006C5DF File Offset: 0x0006A7DF
	public int EntryIndex { get; private set; }

	// Token: 0x06002202 RID: 8706
	public abstract void UpdateState();

	// Token: 0x06002203 RID: 8707 RVA: 0x0006C5E8 File Offset: 0x0006A7E8
	protected virtual void Initialize(int entryIndex, IOmniUIWindowController windowController)
	{
		this.EntryIndex = entryIndex;
		this.m_windowController = windowController;
		this.IsInitialized = true;
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x0006C5FF File Offset: 0x0006A7FF
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (!this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x0006C624 File Offset: 0x0006A824
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (this.m_selectedSprite.gameObject.activeSelf)
		{
			this.m_selectedSprite.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x0006C649 File Offset: 0x0006A849
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x0006C64B File Offset: 0x0006A84B
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.m_windowController.SetHighlightedCategory(this);
		this.m_windowController.SetSelectedCategoryIndex(this.m_windowController.HighlightedCategoryIndex, true);
	}

	// Token: 0x04001D80 RID: 7552
	[SerializeField]
	protected Image m_selectedSprite;

	// Token: 0x04001D81 RID: 7553
	[SerializeField]
	protected Image m_newSymbol;

	// Token: 0x04001D82 RID: 7554
	[SerializeField]
	protected Image m_upgradeSymbol;

	// Token: 0x04001D83 RID: 7555
	[SerializeField]
	protected Image m_iconSprite;

	// Token: 0x04001D84 RID: 7556
	private bool m_inputEnabled;

	// Token: 0x04001D85 RID: 7557
	private IOmniUIWindowController m_windowController;
}
