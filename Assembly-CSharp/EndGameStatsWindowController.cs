using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200057C RID: 1404
public class EndGameStatsWindowController : WindowController
{
	// Token: 0x17001289 RID: 4745
	// (get) Token: 0x060033A0 RID: 13216 RVA: 0x000AEE03 File Offset: 0x000AD003
	public override WindowID ID
	{
		get
		{
			return WindowID.EndGameStats;
		}
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x000AEE07 File Offset: 0x000AD007
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x000AEE1C File Offset: 0x000AD01C
	public override void Initialize()
	{
		this.m_canvasGroup = this.m_windowCanvas.GetComponent<CanvasGroup>();
		base.Initialize();
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x000AEE6C File Offset: 0x000AD06C
	protected override void OnOpen()
	{
		this.UpdateAllStats();
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_playerModel.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		this.m_playerModel.Animator.SetBool("Victory", true);
		this.m_playerModel.Animator.Play("Victory", 0, 1f);
		this.m_biomeLightController.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x000AEEEF File Offset: 0x000AD0EF
	private IEnumerator OnOpenCoroutine()
	{
		this.m_canvasGroup.alpha = 0f;
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x000AEEFE File Offset: 0x000AD0FE
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.m_biomeLightController.SetActive(false);
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x000AEF1D File Offset: 0x000AD11D
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x000AEF4F File Offset: 0x000AD14F
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x000AEF81 File Offset: 0x000AD181
	private void OnCancelButtonDown(InputActionEventData data)
	{
		WindowManager.SetWindowIsOpen(WindowID.EndGameStats, false);
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x000AEF8C File Offset: 0x000AD18C
	private void UpdateAllStats()
	{
		this.m_bannerName.text = LocalizationManager.GetLocalizedPlayerName(SaveManager.PlayerSaveData.CurrentCharacter);
		this.m_levelText.text = SkillTreeManager.GetTotalSkillObjLevel().ToString();
		ClassData classData = ClassLibrary.GetClassData(SaveManager.PlayerSaveData.CurrentCharacter.ClassType);
		if (classData)
		{
			this.m_classText.text = LocalizationManager.GetString(classData.PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		this.UpdateTraits();
		bool flag = false;
		this.UpdateGear(EquipmentCategoryType.Weapon, ref flag);
		this.UpdateGear(EquipmentCategoryType.Head, ref flag);
		this.UpdateGear(EquipmentCategoryType.Chest, ref flag);
		this.UpdateGear(EquipmentCategoryType.Cape, ref flag);
		this.UpdateGear(EquipmentCategoryType.Trinket, ref flag);
		if (flag)
		{
			this.m_noGearText.SetActive(false);
		}
		else
		{
			this.m_noGearText.SetActive(true);
		}
		EndGameStatsStatsEntry[] statsEntries = this.m_statsEntries;
		for (int i = 0; i < statsEntries.Length; i++)
		{
			statsEntries[i].UpdateStat(true);
		}
		this.m_ratingEntry.UpdateStat(true);
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x000AF090 File Offset: 0x000AD290
	private void UpdateTraits()
	{
		bool flag = false;
		if (TraitLibrary.GetTraitData(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne) != null)
		{
			this.m_trait1Icon.transform.parent.gameObject.SetActive(true);
			this.m_trait1Icon.sprite = IconLibrary.GetTraitIcon(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne);
			flag = true;
		}
		else
		{
			this.m_trait1Icon.transform.parent.gameObject.SetActive(false);
		}
		if (TraitLibrary.GetTraitData(SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo) != null)
		{
			this.m_trait2Icon.transform.parent.gameObject.SetActive(true);
			this.m_trait2Icon.sprite = IconLibrary.GetTraitIcon(SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo);
			flag = true;
		}
		else
		{
			this.m_trait2Icon.transform.parent.gameObject.SetActive(false);
		}
		if (flag)
		{
			this.m_noTraitsText.SetActive(false);
			return;
		}
		this.m_noTraitsText.SetActive(true);
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x000AF1A8 File Offset: 0x000AD3A8
	private void UpdateGear(EquipmentCategoryType categoryType, ref bool gearFound)
	{
		Image image = null;
		switch (categoryType)
		{
		case EquipmentCategoryType.Weapon:
			image = this.m_weaponEquipmentIcon;
			break;
		case EquipmentCategoryType.Head:
			image = this.m_headEquipmentIcon;
			break;
		case EquipmentCategoryType.Chest:
			image = this.m_chestEquipmentIcon;
			break;
		case EquipmentCategoryType.Cape:
			image = this.m_capeEquipmentIcon;
			break;
		case EquipmentCategoryType.Trinket:
			image = this.m_trinketEquipmentIcon;
			break;
		}
		EquipmentObj equipped = EquipmentManager.GetEquipped(categoryType);
		if (equipped != null)
		{
			image.transform.parent.gameObject.SetActive(true);
			image.sprite = IconLibrary.GetEquipmentIcon(categoryType, equipped.EquipmentType);
			gearFound = true;
			return;
		}
		image.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x000AF24C File Offset: 0x000AD44C
	private void Update()
	{
		if (this.m_windowCanvas.gameObject.activeSelf)
		{
			float num = -5f * Time.unscaledDeltaTime;
			this.m_ray.transform.SetLocalEulerZ(this.m_ray.transform.localEulerAngles.z + num);
		}
	}

	// Token: 0x0400286B RID: 10347
	[SerializeField]
	private Image m_ray;

	// Token: 0x0400286C RID: 10348
	[SerializeField]
	private GameObject m_biomeLightController;

	// Token: 0x0400286D RID: 10349
	[Header("Banner")]
	[SerializeField]
	private TMP_Text m_bannerName;

	// Token: 0x0400286E RID: 10350
	[Header("Player")]
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x0400286F RID: 10351
	[SerializeField]
	private TMP_Text m_classText;

	// Token: 0x04002870 RID: 10352
	[SerializeField]
	private Image m_trait1Icon;

	// Token: 0x04002871 RID: 10353
	[SerializeField]
	private Image m_trait2Icon;

	// Token: 0x04002872 RID: 10354
	[SerializeField]
	private GameObject m_noTraitsText;

	// Token: 0x04002873 RID: 10355
	[SerializeField]
	private Image m_weaponEquipmentIcon;

	// Token: 0x04002874 RID: 10356
	[SerializeField]
	private Image m_headEquipmentIcon;

	// Token: 0x04002875 RID: 10357
	[SerializeField]
	private Image m_chestEquipmentIcon;

	// Token: 0x04002876 RID: 10358
	[SerializeField]
	private Image m_capeEquipmentIcon;

	// Token: 0x04002877 RID: 10359
	[SerializeField]
	private Image m_trinketEquipmentIcon;

	// Token: 0x04002878 RID: 10360
	[SerializeField]
	private GameObject m_noGearText;

	// Token: 0x04002879 RID: 10361
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x0400287A RID: 10362
	[Header("Stats")]
	[SerializeField]
	private EndGameStatsStatsEntry[] m_statsEntries;

	// Token: 0x0400287B RID: 10363
	[Header("Rating")]
	[SerializeField]
	private EndGameStatsStatsEntry m_ratingEntry;

	// Token: 0x0400287C RID: 10364
	private CanvasGroup m_canvasGroup;

	// Token: 0x0400287D RID: 10365
	private Action<InputActionEventData> m_onCancelButtonDown;
}
