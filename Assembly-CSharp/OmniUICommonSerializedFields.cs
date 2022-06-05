using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000658 RID: 1624
public class OmniUICommonSerializedFields : MonoBehaviour
{
	// Token: 0x17001325 RID: 4901
	// (get) Token: 0x0600318E RID: 12686 RVA: 0x0001B393 File Offset: 0x00019593
	public CanvasGroup LetterBoxGroup
	{
		get
		{
			return this.m_letterBoxGroup;
		}
	}

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x0600318F RID: 12687 RVA: 0x0001B39B File Offset: 0x0001959B
	public CanvasGroup MenuCanvasGroup
	{
		get
		{
			return this.m_menuCanvasGroup;
		}
	}

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x06003190 RID: 12688 RVA: 0x0001B3A3 File Offset: 0x000195A3
	public VerticalLayoutGroup CategoryEntryLayoutGroup
	{
		get
		{
			return this.m_categoryEntryLayoutGroup;
		}
	}

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x06003191 RID: 12689 RVA: 0x0001B3AB File Offset: 0x000195AB
	public VerticalLayoutGroup EntryLayoutGroup
	{
		get
		{
			return this.m_entryLayoutGroup;
		}
	}

	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x06003192 RID: 12690 RVA: 0x0001B3B3 File Offset: 0x000195B3
	public GameObject SelectedCategoryIndicator
	{
		get
		{
			return this.m_selectedCategoryIndicator;
		}
	}

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x06003193 RID: 12691 RVA: 0x0001B3BB File Offset: 0x000195BB
	public TMP_Text ChooseCategoryText
	{
		get
		{
			return this.m_chooseCategoryText;
		}
	}

	// Token: 0x1700132B RID: 4907
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x0001B3C3 File Offset: 0x000195C3
	public CanvasGroup BackgroundCanvasGroup
	{
		get
		{
			return this.m_bgCanvasGroup;
		}
	}

	// Token: 0x1700132C RID: 4908
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x0001B3CB File Offset: 0x000195CB
	public PlayerLookController PlayerModel
	{
		get
		{
			return this.m_playerModel;
		}
	}

	// Token: 0x1700132D RID: 4909
	// (get) Token: 0x06003196 RID: 12694 RVA: 0x0001B3D3 File Offset: 0x000195D3
	public GameObject PlayerSpawnPosObj
	{
		get
		{
			return this.m_playerSpawnPosObj;
		}
	}

	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x06003197 RID: 12695 RVA: 0x0001B3DB File Offset: 0x000195DB
	public GameObject NPCModel
	{
		get
		{
			return this.m_NPCModel;
		}
	}

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x06003198 RID: 12696 RVA: 0x0001B3E3 File Offset: 0x000195E3
	public GameObject NPCSpawnPosObj
	{
		get
		{
			return this.m_npcSpawnPosObj;
		}
	}

	// Token: 0x17001330 RID: 4912
	// (get) Token: 0x06003199 RID: 12697 RVA: 0x0001B3EB File Offset: 0x000195EB
	public NPCController NPCController
	{
		get
		{
			return this.m_npcController;
		}
	}

	// Token: 0x17001331 RID: 4913
	// (get) Token: 0x0600319A RID: 12698 RVA: 0x0001B3F3 File Offset: 0x000195F3
	public CanvasGroup DescriptionBox
	{
		get
		{
			return this.m_descriptionBox;
		}
	}

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x0600319B RID: 12699 RVA: 0x0001B3FB File Offset: 0x000195FB
	public CanvasGroup DescriptionBoxRaycastBlocker
	{
		get
		{
			return this.m_descriptionBoxRaycastBlocker;
		}
	}

	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x0600319C RID: 12700 RVA: 0x0001B403 File Offset: 0x00019603
	public CanvasGroup PurchaseBox
	{
		get
		{
			return this.m_purchaseBox;
		}
	}

	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x0600319D RID: 12701 RVA: 0x0001B40B File Offset: 0x0001960B
	public GameObject ResetTextbox
	{
		get
		{
			return this.m_resetTextbox;
		}
	}

	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x0600319E RID: 12702 RVA: 0x0001B413 File Offset: 0x00019613
	public ScrollBarInput_RL ScrollBarInput
	{
		get
		{
			return this.m_scrollBarInput;
		}
	}

	// Token: 0x17001336 RID: 4918
	// (get) Token: 0x0600319F RID: 12703 RVA: 0x0001B41B File Offset: 0x0001961B
	public Scrollbar ScrollBar
	{
		get
		{
			return this.m_scrollBar;
		}
	}

	// Token: 0x17001337 RID: 4919
	// (get) Token: 0x060031A0 RID: 12704 RVA: 0x0001B423 File Offset: 0x00019623
	public ScrollRect ScrollRect
	{
		get
		{
			return this.m_scrollRect;
		}
	}

	// Token: 0x17001338 RID: 4920
	// (get) Token: 0x060031A1 RID: 12705 RVA: 0x0001B42B File Offset: 0x0001962B
	public RectTransform ContentViewport
	{
		get
		{
			return this.m_contentViewport;
		}
	}

	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x060031A2 RID: 12706 RVA: 0x0001B433 File Offset: 0x00019633
	public GameObject TopScrollArrow
	{
		get
		{
			return this.m_topScrollArrow;
		}
	}

	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x060031A3 RID: 12707 RVA: 0x0001B43B File Offset: 0x0001963B
	public Image TopScrollNewSymbol
	{
		get
		{
			return this.m_topScrollNewSymbol;
		}
	}

	// Token: 0x1700133B RID: 4923
	// (get) Token: 0x060031A4 RID: 12708 RVA: 0x0001B443 File Offset: 0x00019643
	public Image TopScrollUpgradeSymbol
	{
		get
		{
			return this.m_topScrollUpgradeSymbol;
		}
	}

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x060031A5 RID: 12709 RVA: 0x0001B44B File Offset: 0x0001964B
	public GameObject BottomScrollArrow
	{
		get
		{
			return this.m_bottomScrollArrow;
		}
	}

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x060031A6 RID: 12710 RVA: 0x0001B453 File Offset: 0x00019653
	public Image BottomScrollNewSymbol
	{
		get
		{
			return this.m_bottomScrollNewSymbol;
		}
	}

	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x060031A7 RID: 12711 RVA: 0x0001B45B File Offset: 0x0001965B
	public Image BottomScrollUpgradeSymbol
	{
		get
		{
			return this.m_bottomScrollUpgradeSymbol;
		}
	}

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x060031A8 RID: 12712 RVA: 0x0001B463 File Offset: 0x00019663
	public GameObject WarningMessageBox
	{
		get
		{
			return this.m_warningMessageBox;
		}
	}

	// Token: 0x0400285A RID: 10330
	[Header("LetterBox")]
	[SerializeField]
	private CanvasGroup m_letterBoxGroup;

	// Token: 0x0400285B RID: 10331
	[Header("Entries")]
	[SerializeField]
	protected CanvasGroup m_menuCanvasGroup;

	// Token: 0x0400285C RID: 10332
	[SerializeField]
	private VerticalLayoutGroup m_categoryEntryLayoutGroup;

	// Token: 0x0400285D RID: 10333
	[SerializeField]
	private VerticalLayoutGroup m_entryLayoutGroup;

	// Token: 0x0400285E RID: 10334
	[SerializeField]
	private GameObject m_selectedCategoryIndicator;

	// Token: 0x0400285F RID: 10335
	[SerializeField]
	private TMP_Text m_chooseCategoryText;

	// Token: 0x04002860 RID: 10336
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04002861 RID: 10337
	[Header("Models")]
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x04002862 RID: 10338
	[SerializeField]
	private GameObject m_playerSpawnPosObj;

	// Token: 0x04002863 RID: 10339
	[SerializeField]
	private GameObject m_NPCModel;

	// Token: 0x04002864 RID: 10340
	[SerializeField]
	private GameObject m_npcSpawnPosObj;

	// Token: 0x04002865 RID: 10341
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x04002866 RID: 10342
	[Header("Description Boxes")]
	[SerializeField]
	protected CanvasGroup m_descriptionBox;

	// Token: 0x04002867 RID: 10343
	[SerializeField]
	protected CanvasGroup m_descriptionBoxRaycastBlocker;

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	private CanvasGroup m_purchaseBox;

	// Token: 0x04002869 RID: 10345
	[SerializeField]
	private GameObject m_resetTextbox;

	// Token: 0x0400286A RID: 10346
	[Header("Scrollbar")]
	[SerializeField]
	private ScrollBarInput_RL m_scrollBarInput;

	// Token: 0x0400286B RID: 10347
	[SerializeField]
	private Scrollbar m_scrollBar;

	// Token: 0x0400286C RID: 10348
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x0400286D RID: 10349
	[SerializeField]
	private RectTransform m_contentViewport;

	// Token: 0x0400286E RID: 10350
	[SerializeField]
	private GameObject m_topScrollArrow;

	// Token: 0x0400286F RID: 10351
	[SerializeField]
	private Image m_topScrollNewSymbol;

	// Token: 0x04002870 RID: 10352
	[SerializeField]
	private Image m_topScrollUpgradeSymbol;

	// Token: 0x04002871 RID: 10353
	[SerializeField]
	private GameObject m_bottomScrollArrow;

	// Token: 0x04002872 RID: 10354
	[SerializeField]
	private Image m_bottomScrollNewSymbol;

	// Token: 0x04002873 RID: 10355
	[SerializeField]
	private Image m_bottomScrollUpgradeSymbol;

	// Token: 0x04002874 RID: 10356
	[SerializeField]
	private GameObject m_warningMessageBox;
}
