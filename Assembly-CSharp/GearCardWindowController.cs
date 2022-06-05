using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200057D RID: 1405
public class GearCardWindowController : WindowController, ILocalizable
{
	// Token: 0x1700128A RID: 4746
	// (get) Token: 0x060033AE RID: 13230 RVA: 0x000AF2A6 File Offset: 0x000AD4A6
	public override WindowID ID
	{
		get
		{
			return WindowID.GearCard;
		}
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x000AF2AA File Offset: 0x000AD4AA
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x000AF2E4 File Offset: 0x000AD4E4
	public override void Initialize()
	{
		this.m_equipmentEntryArray = new GearCardEntry[5];
		this.m_unityEntryArray = new GearCardEntry[5];
		this.m_runeEntryArray = new GearCardEntry[5];
		this.m_heirloomEntryArray = new GearCardEntry[5];
		this.m_relicEntryArray = new GearCardEntry[5];
		int siblingIndex = this.m_noEquipmentGO.transform.GetSiblingIndex();
		for (int i = 0; i < 5; i++)
		{
			GearCardEntry gearCardEntry = this.InstantiateGearCardEntry(this.m_leftSideContentGO.transform, siblingIndex);
			gearCardEntry.SetAsEquipment(EquipmentCategoryType.Cape, EquipmentType.GEAR_ARMOR);
			this.m_equipmentEntryArray[i] = gearCardEntry;
		}
		int siblingIndex2 = this.m_noUnityGO.transform.GetSiblingIndex();
		for (int j = 0; j < 5; j++)
		{
			GearCardEntry gearCardEntry2 = this.InstantiateGearCardEntry(this.m_leftSideContentGO.transform, siblingIndex2);
			RectTransform component = gearCardEntry2.GetComponent<RectTransform>();
			Vector2 sizeDelta = component.sizeDelta;
			sizeDelta.y *= 0.2f;
			component.sizeDelta = sizeDelta;
			this.m_unityEntryArray[j] = gearCardEntry2;
		}
		int siblingIndex3 = this.m_noRunesGO.transform.GetSiblingIndex();
		for (int k = 0; k < 5; k++)
		{
			GearCardEntry gearCardEntry3 = this.InstantiateGearCardEntry(this.m_leftSideContentGO.transform, siblingIndex3);
			this.m_runeEntryArray[k] = gearCardEntry3;
		}
		int siblingIndex4 = this.m_noRelicsGO.transform.GetSiblingIndex();
		for (int l = 0; l < 5; l++)
		{
			GearCardEntry gearCardEntry4 = this.InstantiateGearCardEntry(this.m_rightSideContentGO.transform, siblingIndex4);
			this.m_relicEntryArray[l] = gearCardEntry4;
		}
		int siblingIndex5 = this.m_noHeirloomsGO.transform.GetSiblingIndex();
		for (int m = 0; m < 5; m++)
		{
			GearCardEntry gearCardEntry5 = this.InstantiateGearCardEntry(this.m_rightSideContentGO.transform, siblingIndex5);
			this.m_heirloomEntryArray[m] = gearCardEntry5;
		}
		base.Initialize();
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x000AF4A4 File Offset: 0x000AD6A4
	private GearCardEntry InstantiateGearCardEntry(Transform parentTransform, int siblingIndex)
	{
		GearCardEntry gearCardEntry = UnityEngine.Object.Instantiate<GearCardEntry>(this.m_gearCardEntryPrefab, parentTransform, false);
		gearCardEntry.gameObject.SetActive(false);
		gearCardEntry.transform.SetSiblingIndex(siblingIndex);
		return gearCardEntry;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x000AF4CC File Offset: 0x000AD6CC
	private void UpdateArrays()
	{
		this.UpdateEquipmentEntry(EquipmentCategoryType.Weapon, 0);
		this.UpdateEquipmentEntry(EquipmentCategoryType.Head, 1);
		this.UpdateEquipmentEntry(EquipmentCategoryType.Chest, 2);
		this.UpdateEquipmentEntry(EquipmentCategoryType.Cape, 3);
		this.UpdateEquipmentEntry(EquipmentCategoryType.Trinket, 4);
		bool flag = false;
		GearCardEntry[] equipmentEntryArray = this.m_equipmentEntryArray;
		for (int i = 0; i < equipmentEntryArray.Length; i++)
		{
			if (equipmentEntryArray[i].gameObject.activeSelf)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.m_noEquipmentGO.SetActive(false);
		}
		else
		{
			this.m_noEquipmentGO.SetActive(true);
		}
		if (this.UpdateUnityEntries())
		{
			this.m_noUnityGO.SetActive(false);
		}
		else
		{
			this.m_noUnityGO.SetActive(true);
		}
		if (this.UpdateRuneEntries_V2())
		{
			this.m_noRunesGO.SetActive(false);
		}
		else
		{
			this.m_noRunesGO.SetActive(true);
		}
		if (this.UpdateHeirloomEntries_V2())
		{
			this.m_noHeirloomsGO.SetActive(false);
		}
		else
		{
			this.m_noHeirloomsGO.SetActive(true);
		}
		if (this.UpdateRelicEntries_V2())
		{
			this.m_noRelicsGO.SetActive(false);
			return;
		}
		this.m_noRelicsGO.SetActive(true);
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x000AF5D0 File Offset: 0x000AD7D0
	private void UpdateEquipmentEntry(EquipmentCategoryType category, int arrayIndex)
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(category);
		GearCardEntry gearCardEntry = this.m_equipmentEntryArray[arrayIndex];
		if (equipped != null)
		{
			if (!gearCardEntry.gameObject.activeSelf)
			{
				gearCardEntry.gameObject.SetActive(true);
			}
			gearCardEntry.SetAsEquipment(equipped.CategoryType, equipped.EquipmentType);
			return;
		}
		if (gearCardEntry.gameObject.activeSelf)
		{
			gearCardEntry.gameObject.SetActive(false);
		}
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x000AF638 File Offset: 0x000AD838
	private bool UpdateUnityEntries()
	{
		GearCardWindowController.m_activeUnityHelper.Clear();
		foreach (EquipmentSetBonusType equipmentSetBonusType in EquipmentSetBonusType_RL.TypeArray)
		{
			if (equipmentSetBonusType != EquipmentSetBonusType.None)
			{
				float num = EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(equipmentSetBonusType);
				if (num > 0f)
				{
					GearCardWindowController.m_activeUnityHelper.Add(new Vector2((float)equipmentSetBonusType, num));
				}
			}
		}
		int count = GearCardWindowController.m_activeUnityHelper.Count;
		if (count > this.m_unityEntryArray.Length)
		{
			int num2 = this.m_unityEntryArray.Length;
			this.m_unityEntryArray = this.ExpandGearCardArray(this.m_unityEntryArray, count + 5);
			int siblingIndex = this.m_noUnityGO.transform.GetSiblingIndex();
			for (int j = num2; j < this.m_unityEntryArray.Length; j++)
			{
				GearCardEntry gearCardEntry = this.InstantiateGearCardEntry(this.m_leftSideContentGO.transform, siblingIndex);
				RectTransform component = gearCardEntry.GetComponent<RectTransform>();
				Vector2 sizeDelta = component.sizeDelta;
				sizeDelta.y *= 0.25f;
				component.sizeDelta = sizeDelta;
				this.m_unityEntryArray[j] = gearCardEntry;
			}
		}
		for (int k = 0; k < this.m_unityEntryArray.Length; k++)
		{
			GearCardEntry gearCardEntry2 = this.m_unityEntryArray[k];
			if (k < count)
			{
				gearCardEntry2.SetAsUnity((EquipmentSetBonusType)GearCardWindowController.m_activeUnityHelper[k].x, GearCardWindowController.m_activeUnityHelper[k].y);
				if (!gearCardEntry2.gameObject.activeSelf)
				{
					gearCardEntry2.gameObject.SetActive(true);
				}
			}
			else if (gearCardEntry2.gameObject.activeSelf)
			{
				gearCardEntry2.gameObject.SetActive(false);
			}
		}
		return count > 0;
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x000AF7C4 File Offset: 0x000AD9C4
	private bool UpdateRuneEntries_V2()
	{
		GearCardWindowController.m_activeRuneHelper.Clear();
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None && RuneManager.GetRuneEquippedLevel(runeType) > 0)
			{
				GearCardWindowController.m_activeRuneHelper.Add(runeType);
			}
		}
		int count = GearCardWindowController.m_activeRuneHelper.Count;
		if (count > this.m_runeEntryArray.Length)
		{
			int num = this.m_runeEntryArray.Length;
			this.m_runeEntryArray = this.ExpandGearCardArray(this.m_runeEntryArray, count + 5);
			int siblingIndex = this.m_noRunesGO.transform.GetSiblingIndex();
			for (int j = num; j < this.m_runeEntryArray.Length; j++)
			{
				GearCardEntry gearCardEntry = this.InstantiateGearCardEntry(this.m_leftSideContentGO.transform, siblingIndex);
				this.m_runeEntryArray[j] = gearCardEntry;
			}
		}
		for (int k = 0; k < this.m_runeEntryArray.Length; k++)
		{
			GearCardEntry gearCardEntry2 = this.m_runeEntryArray[k];
			if (k < count)
			{
				gearCardEntry2.SetAsRune(GearCardWindowController.m_activeRuneHelper[k]);
				if (!gearCardEntry2.gameObject.activeSelf)
				{
					gearCardEntry2.gameObject.SetActive(true);
				}
			}
			else if (gearCardEntry2.gameObject.activeSelf)
			{
				gearCardEntry2.gameObject.SetActive(false);
			}
		}
		return count > 0;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x000AF8FC File Offset: 0x000ADAFC
	private bool UpdateHeirloomEntries_V2()
	{
		GearCardWindowController.m_activeHeirloomHelper.Clear();
		foreach (HeirloomType heirloomType in HeirloomType_RL.TypeArray)
		{
			if (heirloomType != HeirloomType.None && SaveManager.PlayerSaveData.GetHeirloomLevel(heirloomType) > 0 && HeirloomLibrary.GetHeirloomData(heirloomType) != null)
			{
				GearCardWindowController.m_activeHeirloomHelper.Add(heirloomType);
			}
		}
		int count = GearCardWindowController.m_activeHeirloomHelper.Count;
		if (count > this.m_heirloomEntryArray.Length)
		{
			int num = this.m_heirloomEntryArray.Length;
			this.m_heirloomEntryArray = this.ExpandGearCardArray(this.m_heirloomEntryArray, count + 5);
			int siblingIndex = this.m_noHeirloomsGO.transform.GetSiblingIndex();
			for (int j = num; j < this.m_heirloomEntryArray.Length; j++)
			{
				GearCardEntry gearCardEntry = this.InstantiateGearCardEntry(this.m_rightSideContentGO.transform, siblingIndex);
				this.m_heirloomEntryArray[j] = gearCardEntry;
			}
		}
		for (int k = 0; k < this.m_heirloomEntryArray.Length; k++)
		{
			GearCardEntry gearCardEntry2 = this.m_heirloomEntryArray[k];
			if (k < count)
			{
				gearCardEntry2.SetAsHeirloom(GearCardWindowController.m_activeHeirloomHelper[k]);
				if (!gearCardEntry2.gameObject.activeSelf)
				{
					gearCardEntry2.gameObject.SetActive(true);
				}
			}
			else if (gearCardEntry2.gameObject.activeSelf)
			{
				gearCardEntry2.gameObject.SetActive(false);
			}
		}
		return count > 0;
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x000AFA40 File Offset: 0x000ADC40
	private bool UpdateRelicEntries_V2()
	{
		GearCardWindowController.m_activeRelicHelper.Clear();
		foreach (RelicType relicType in RelicType_RL.TypeArray)
		{
			if (relicType != RelicType.None && SaveManager.PlayerSaveData.GetRelic(relicType).Level > 0)
			{
				GearCardWindowController.m_activeRelicHelper.Add(relicType);
			}
		}
		int count = GearCardWindowController.m_activeRelicHelper.Count;
		if (count > this.m_relicEntryArray.Length)
		{
			int num = this.m_relicEntryArray.Length;
			this.m_relicEntryArray = this.ExpandGearCardArray(this.m_relicEntryArray, count + 5);
			int siblingIndex = this.m_noRelicsGO.transform.GetSiblingIndex();
			for (int j = num; j < this.m_relicEntryArray.Length; j++)
			{
				GearCardEntry gearCardEntry = this.InstantiateGearCardEntry(this.m_rightSideContentGO.transform, siblingIndex);
				this.m_relicEntryArray[j] = gearCardEntry;
			}
		}
		for (int k = 0; k < this.m_relicEntryArray.Length; k++)
		{
			GearCardEntry gearCardEntry2 = this.m_relicEntryArray[k];
			if (k < count)
			{
				gearCardEntry2.SetAsRelic(GearCardWindowController.m_activeRelicHelper[k]);
				if (!gearCardEntry2.gameObject.activeSelf)
				{
					gearCardEntry2.gameObject.SetActive(true);
				}
			}
			else if (gearCardEntry2.gameObject.activeSelf)
			{
				gearCardEntry2.gameObject.SetActive(false);
			}
		}
		return count > 0;
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x000AFB84 File Offset: 0x000ADD84
	private GearCardEntry[] ExpandGearCardArray(GearCardEntry[] arr, int size)
	{
		GearCardEntry[] array = new GearCardEntry[size];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = arr[i];
		}
		return array;
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x000AFBB0 File Offset: 0x000ADDB0
	protected void SelectScrollBar(bool selectLeft)
	{
		if (selectLeft)
		{
			this.m_selectedScrollBarInput = this.m_leftScrollBarInput;
			this.m_leftScrollArrow.gameObject.SetActive(true);
			this.m_rightScrollArrow.gameObject.SetActive(false);
			this.m_leftScrollBarInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
			this.m_rightScrollBarInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
			return;
		}
		this.m_selectedScrollBarInput = this.m_rightScrollBarInput;
		this.m_rightScrollArrow.gameObject.SetActive(true);
		this.m_leftScrollArrow.gameObject.SetActive(false);
		this.m_leftScrollBarInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
		this.m_rightScrollBarInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x000AFC4D File Offset: 0x000ADE4D
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.UpdateArrays();
		this.SelectScrollBar(true);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x000AFC87 File Offset: 0x000ADE87
	private IEnumerator RunOpenAnimation()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_leftCardCanvasGroup.alpha = 0f;
		this.m_rightCardCanvasGroup.alpha = 0f;
		float num = 50f;
		float duration = 0.15f;
		RectTransform component = this.m_leftCardCanvasGroup.GetComponent<RectTransform>();
		Vector3 v = component.anchoredPosition;
		v.y += num;
		component.anchoredPosition = v;
		TweenManager.TweenBy_UnscaledTime(component, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		TweenManager.TweenTo_UnscaledTime(this.m_leftCardCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		RectTransform component2 = this.m_rightCardCanvasGroup.GetComponent<RectTransform>();
		Vector3 v2 = component2.anchoredPosition;
		v2.y += num;
		component2.anchoredPosition = v2;
		TweenManager.TweenBy_UnscaledTime(component2, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_rightCardCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x000AFC96 File Offset: 0x000ADE96
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x000AFCB6 File Offset: 0x000ADEB6
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x000AFCBE File Offset: 0x000ADEBE
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x000AFCC8 File Offset: 0x000ADEC8
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x000AFD30 File Offset: 0x000ADF30
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x000AFD95 File Offset: 0x000ADF95
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x000AFDB1 File Offset: 0x000ADFB1
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x000AFDCA File Offset: 0x000ADFCA
	public void RefreshText(object sender, EventArgs args)
	{
		this.UpdateArrays();
	}

	// Token: 0x0400287E RID: 10366
	private const int STARTING_ARRAY_SIZE = 5;

	// Token: 0x0400287F RID: 10367
	[SerializeField]
	private GearCardEntry m_gearCardEntryPrefab;

	// Token: 0x04002880 RID: 10368
	[SerializeField]
	private GameObject m_leftSideContentGO;

	// Token: 0x04002881 RID: 10369
	[SerializeField]
	private GameObject m_rightSideContentGO;

	// Token: 0x04002882 RID: 10370
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x04002883 RID: 10371
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x04002884 RID: 10372
	[SerializeField]
	private GameObject m_noEquipmentGO;

	// Token: 0x04002885 RID: 10373
	[SerializeField]
	private GameObject m_noUnityGO;

	// Token: 0x04002886 RID: 10374
	[SerializeField]
	private GameObject m_noRunesGO;

	// Token: 0x04002887 RID: 10375
	[SerializeField]
	private GameObject m_noHeirloomsGO;

	// Token: 0x04002888 RID: 10376
	[SerializeField]
	private GameObject m_noRelicsGO;

	// Token: 0x04002889 RID: 10377
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x0400288A RID: 10378
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x0400288B RID: 10379
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x0400288C RID: 10380
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x0400288D RID: 10381
	private GearCardEntry[] m_equipmentEntryArray;

	// Token: 0x0400288E RID: 10382
	private GearCardEntry[] m_unityEntryArray;

	// Token: 0x0400288F RID: 10383
	private GearCardEntry[] m_runeEntryArray;

	// Token: 0x04002890 RID: 10384
	private GearCardEntry[] m_heirloomEntryArray;

	// Token: 0x04002891 RID: 10385
	private GearCardEntry[] m_relicEntryArray;

	// Token: 0x04002892 RID: 10386
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x04002893 RID: 10387
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002894 RID: 10388
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x04002895 RID: 10389
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x04002896 RID: 10390
	private static List<Vector2> m_activeUnityHelper = new List<Vector2>();

	// Token: 0x04002897 RID: 10391
	private static List<RuneType> m_activeRuneHelper = new List<RuneType>();

	// Token: 0x04002898 RID: 10392
	private static List<HeirloomType> m_activeHeirloomHelper = new List<HeirloomType>();

	// Token: 0x04002899 RID: 10393
	private static List<RelicType> m_activeRelicHelper = new List<RelicType>();
}
