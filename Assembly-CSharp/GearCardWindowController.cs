using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200095F RID: 2399
public class GearCardWindowController : WindowController, ILocalizable
{
	// Token: 0x17001971 RID: 6513
	// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00005D14 File Offset: 0x00003F14
	public override WindowID ID
	{
		get
		{
			return WindowID.GearCard;
		}
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x0002803C File Offset: 0x0002623C
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x0011A9CC File Offset: 0x00118BCC
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

	// Token: 0x060048EC RID: 18668 RVA: 0x00028076 File Offset: 0x00026276
	private GearCardEntry InstantiateGearCardEntry(Transform parentTransform, int siblingIndex)
	{
		GearCardEntry gearCardEntry = UnityEngine.Object.Instantiate<GearCardEntry>(this.m_gearCardEntryPrefab, parentTransform, false);
		gearCardEntry.gameObject.SetActive(false);
		gearCardEntry.transform.SetSiblingIndex(siblingIndex);
		return gearCardEntry;
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x0011AB8C File Offset: 0x00118D8C
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

	// Token: 0x060048EE RID: 18670 RVA: 0x0011AC90 File Offset: 0x00118E90
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

	// Token: 0x060048EF RID: 18671 RVA: 0x0011ACF8 File Offset: 0x00118EF8
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

	// Token: 0x060048F0 RID: 18672 RVA: 0x0011AE84 File Offset: 0x00119084
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

	// Token: 0x060048F1 RID: 18673 RVA: 0x0011AFBC File Offset: 0x001191BC
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

	// Token: 0x060048F2 RID: 18674 RVA: 0x0011B100 File Offset: 0x00119300
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

	// Token: 0x060048F3 RID: 18675 RVA: 0x0011B244 File Offset: 0x00119444
	private GearCardEntry[] ExpandGearCardArray(GearCardEntry[] arr, int size)
	{
		GearCardEntry[] array = new GearCardEntry[size];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = arr[i];
		}
		return array;
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x0011B270 File Offset: 0x00119470
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

	// Token: 0x060048F5 RID: 18677 RVA: 0x0002809D File Offset: 0x0002629D
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.UpdateArrays();
		this.SelectScrollBar(true);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x000280D7 File Offset: 0x000262D7
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

	// Token: 0x060048F7 RID: 18679 RVA: 0x000280E6 File Offset: 0x000262E6
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x00028106 File Offset: 0x00026306
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x0002810E File Offset: 0x0002630E
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x0011B310 File Offset: 0x00119510
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x0011B378 File Offset: 0x00119578
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060048FC RID: 18684 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x060048FD RID: 18685 RVA: 0x00028132 File Offset: 0x00026332
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x060048FE RID: 18686 RVA: 0x0002814B File Offset: 0x0002634B
	public void RefreshText(object sender, EventArgs args)
	{
		this.UpdateArrays();
	}

	// Token: 0x040037E7 RID: 14311
	private const int STARTING_ARRAY_SIZE = 5;

	// Token: 0x040037E8 RID: 14312
	[SerializeField]
	private GearCardEntry m_gearCardEntryPrefab;

	// Token: 0x040037E9 RID: 14313
	[SerializeField]
	private GameObject m_leftSideContentGO;

	// Token: 0x040037EA RID: 14314
	[SerializeField]
	private GameObject m_rightSideContentGO;

	// Token: 0x040037EB RID: 14315
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x040037EC RID: 14316
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x040037ED RID: 14317
	[SerializeField]
	private GameObject m_noEquipmentGO;

	// Token: 0x040037EE RID: 14318
	[SerializeField]
	private GameObject m_noUnityGO;

	// Token: 0x040037EF RID: 14319
	[SerializeField]
	private GameObject m_noRunesGO;

	// Token: 0x040037F0 RID: 14320
	[SerializeField]
	private GameObject m_noHeirloomsGO;

	// Token: 0x040037F1 RID: 14321
	[SerializeField]
	private GameObject m_noRelicsGO;

	// Token: 0x040037F2 RID: 14322
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x040037F3 RID: 14323
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x040037F4 RID: 14324
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x040037F5 RID: 14325
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x040037F6 RID: 14326
	private GearCardEntry[] m_equipmentEntryArray;

	// Token: 0x040037F7 RID: 14327
	private GearCardEntry[] m_unityEntryArray;

	// Token: 0x040037F8 RID: 14328
	private GearCardEntry[] m_runeEntryArray;

	// Token: 0x040037F9 RID: 14329
	private GearCardEntry[] m_heirloomEntryArray;

	// Token: 0x040037FA RID: 14330
	private GearCardEntry[] m_relicEntryArray;

	// Token: 0x040037FB RID: 14331
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x040037FC RID: 14332
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040037FD RID: 14333
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x040037FE RID: 14334
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x040037FF RID: 14335
	private static List<Vector2> m_activeUnityHelper = new List<Vector2>();

	// Token: 0x04003800 RID: 14336
	private static List<RuneType> m_activeRuneHelper = new List<RuneType>();

	// Token: 0x04003801 RID: 14337
	private static List<HeirloomType> m_activeHeirloomHelper = new List<HeirloomType>();

	// Token: 0x04003802 RID: 14338
	private static List<RelicType> m_activeRelicHelper = new List<RelicType>();
}
