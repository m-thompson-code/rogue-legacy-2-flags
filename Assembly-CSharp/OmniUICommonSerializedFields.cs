using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C0 RID: 960
public class OmniUICommonSerializedFields : MonoBehaviour
{
	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06002370 RID: 9072 RVA: 0x0007373F File Offset: 0x0007193F
	public CanvasGroup LetterBoxGroup
	{
		get
		{
			return this.m_letterBoxGroup;
		}
	}

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06002371 RID: 9073 RVA: 0x00073747 File Offset: 0x00071947
	public CanvasGroup MenuCanvasGroup
	{
		get
		{
			return this.m_menuCanvasGroup;
		}
	}

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06002372 RID: 9074 RVA: 0x0007374F File Offset: 0x0007194F
	public VerticalLayoutGroup CategoryEntryLayoutGroup
	{
		get
		{
			return this.m_categoryEntryLayoutGroup;
		}
	}

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06002373 RID: 9075 RVA: 0x00073757 File Offset: 0x00071957
	public VerticalLayoutGroup EntryLayoutGroup
	{
		get
		{
			return this.m_entryLayoutGroup;
		}
	}

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06002374 RID: 9076 RVA: 0x0007375F File Offset: 0x0007195F
	public GameObject SelectedCategoryIndicator
	{
		get
		{
			return this.m_selectedCategoryIndicator;
		}
	}

	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06002375 RID: 9077 RVA: 0x00073767 File Offset: 0x00071967
	public TMP_Text ChooseCategoryText
	{
		get
		{
			return this.m_chooseCategoryText;
		}
	}

	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06002376 RID: 9078 RVA: 0x0007376F File Offset: 0x0007196F
	public CanvasGroup BackgroundCanvasGroup
	{
		get
		{
			return this.m_bgCanvasGroup;
		}
	}

	// Token: 0x17000E97 RID: 3735
	// (get) Token: 0x06002377 RID: 9079 RVA: 0x00073777 File Offset: 0x00071977
	public PlayerLookController PlayerModel
	{
		get
		{
			return this.m_playerModel;
		}
	}

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06002378 RID: 9080 RVA: 0x0007377F File Offset: 0x0007197F
	public GameObject PlayerSpawnPosObj
	{
		get
		{
			return this.m_playerSpawnPosObj;
		}
	}

	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06002379 RID: 9081 RVA: 0x00073787 File Offset: 0x00071987
	public GameObject NPCModel
	{
		get
		{
			return this.m_NPCModel;
		}
	}

	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x0007378F File Offset: 0x0007198F
	public GameObject NPCSpawnPosObj
	{
		get
		{
			return this.m_npcSpawnPosObj;
		}
	}

	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x0600237B RID: 9083 RVA: 0x00073797 File Offset: 0x00071997
	public NPCController NPCController
	{
		get
		{
			return this.m_npcController;
		}
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x0600237C RID: 9084 RVA: 0x0007379F File Offset: 0x0007199F
	public CanvasGroup DescriptionBox
	{
		get
		{
			return this.m_descriptionBox;
		}
	}

	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x0600237D RID: 9085 RVA: 0x000737A7 File Offset: 0x000719A7
	public CanvasGroup DescriptionBoxRaycastBlocker
	{
		get
		{
			return this.m_descriptionBoxRaycastBlocker;
		}
	}

	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x0600237E RID: 9086 RVA: 0x000737AF File Offset: 0x000719AF
	public CanvasGroup PurchaseBox
	{
		get
		{
			return this.m_purchaseBox;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x0600237F RID: 9087 RVA: 0x000737B7 File Offset: 0x000719B7
	public GameObject ResetTextbox
	{
		get
		{
			return this.m_resetTextbox;
		}
	}

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06002380 RID: 9088 RVA: 0x000737BF File Offset: 0x000719BF
	public ScrollBarInput_RL ScrollBarInput
	{
		get
		{
			return this.m_scrollBarInput;
		}
	}

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06002381 RID: 9089 RVA: 0x000737C7 File Offset: 0x000719C7
	public Scrollbar ScrollBar
	{
		get
		{
			return this.m_scrollBar;
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06002382 RID: 9090 RVA: 0x000737CF File Offset: 0x000719CF
	public ScrollRect ScrollRect
	{
		get
		{
			return this.m_scrollRect;
		}
	}

	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x06002383 RID: 9091 RVA: 0x000737D7 File Offset: 0x000719D7
	public RectTransform ContentViewport
	{
		get
		{
			return this.m_contentViewport;
		}
	}

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x06002384 RID: 9092 RVA: 0x000737DF File Offset: 0x000719DF
	public GameObject TopScrollArrow
	{
		get
		{
			return this.m_topScrollArrow;
		}
	}

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x06002385 RID: 9093 RVA: 0x000737E7 File Offset: 0x000719E7
	public Image TopScrollNewSymbol
	{
		get
		{
			return this.m_topScrollNewSymbol;
		}
	}

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x06002386 RID: 9094 RVA: 0x000737EF File Offset: 0x000719EF
	public Image TopScrollUpgradeSymbol
	{
		get
		{
			return this.m_topScrollUpgradeSymbol;
		}
	}

	// Token: 0x17000EA7 RID: 3751
	// (get) Token: 0x06002387 RID: 9095 RVA: 0x000737F7 File Offset: 0x000719F7
	public GameObject BottomScrollArrow
	{
		get
		{
			return this.m_bottomScrollArrow;
		}
	}

	// Token: 0x17000EA8 RID: 3752
	// (get) Token: 0x06002388 RID: 9096 RVA: 0x000737FF File Offset: 0x000719FF
	public Image BottomScrollNewSymbol
	{
		get
		{
			return this.m_bottomScrollNewSymbol;
		}
	}

	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x06002389 RID: 9097 RVA: 0x00073807 File Offset: 0x00071A07
	public Image BottomScrollUpgradeSymbol
	{
		get
		{
			return this.m_bottomScrollUpgradeSymbol;
		}
	}

	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x0600238A RID: 9098 RVA: 0x0007380F File Offset: 0x00071A0F
	public GameObject WarningMessageBox
	{
		get
		{
			return this.m_warningMessageBox;
		}
	}

	// Token: 0x04001E21 RID: 7713
	[Header("LetterBox")]
	[SerializeField]
	private CanvasGroup m_letterBoxGroup;

	// Token: 0x04001E22 RID: 7714
	[Header("Entries")]
	[SerializeField]
	protected CanvasGroup m_menuCanvasGroup;

	// Token: 0x04001E23 RID: 7715
	[SerializeField]
	private VerticalLayoutGroup m_categoryEntryLayoutGroup;

	// Token: 0x04001E24 RID: 7716
	[SerializeField]
	private VerticalLayoutGroup m_entryLayoutGroup;

	// Token: 0x04001E25 RID: 7717
	[SerializeField]
	private GameObject m_selectedCategoryIndicator;

	// Token: 0x04001E26 RID: 7718
	[SerializeField]
	private TMP_Text m_chooseCategoryText;

	// Token: 0x04001E27 RID: 7719
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04001E28 RID: 7720
	[Header("Models")]
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x04001E29 RID: 7721
	[SerializeField]
	private GameObject m_playerSpawnPosObj;

	// Token: 0x04001E2A RID: 7722
	[SerializeField]
	private GameObject m_NPCModel;

	// Token: 0x04001E2B RID: 7723
	[SerializeField]
	private GameObject m_npcSpawnPosObj;

	// Token: 0x04001E2C RID: 7724
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x04001E2D RID: 7725
	[Header("Description Boxes")]
	[SerializeField]
	protected CanvasGroup m_descriptionBox;

	// Token: 0x04001E2E RID: 7726
	[SerializeField]
	protected CanvasGroup m_descriptionBoxRaycastBlocker;

	// Token: 0x04001E2F RID: 7727
	[SerializeField]
	private CanvasGroup m_purchaseBox;

	// Token: 0x04001E30 RID: 7728
	[SerializeField]
	private GameObject m_resetTextbox;

	// Token: 0x04001E31 RID: 7729
	[Header("Scrollbar")]
	[SerializeField]
	private ScrollBarInput_RL m_scrollBarInput;

	// Token: 0x04001E32 RID: 7730
	[SerializeField]
	private Scrollbar m_scrollBar;

	// Token: 0x04001E33 RID: 7731
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x04001E34 RID: 7732
	[SerializeField]
	private RectTransform m_contentViewport;

	// Token: 0x04001E35 RID: 7733
	[SerializeField]
	private GameObject m_topScrollArrow;

	// Token: 0x04001E36 RID: 7734
	[SerializeField]
	private Image m_topScrollNewSymbol;

	// Token: 0x04001E37 RID: 7735
	[SerializeField]
	private Image m_topScrollUpgradeSymbol;

	// Token: 0x04001E38 RID: 7736
	[SerializeField]
	private GameObject m_bottomScrollArrow;

	// Token: 0x04001E39 RID: 7737
	[SerializeField]
	private Image m_bottomScrollNewSymbol;

	// Token: 0x04001E3A RID: 7738
	[SerializeField]
	private Image m_bottomScrollUpgradeSymbol;

	// Token: 0x04001E3B RID: 7739
	[SerializeField]
	private GameObject m_warningMessageBox;
}
