using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200095D RID: 2397
public class EndGameStatsWindowController : WindowController
{
	// Token: 0x1700196E RID: 6510
	// (get) Token: 0x060048D5 RID: 18645 RVA: 0x00027F71 File Offset: 0x00026171
	public override WindowID ID
	{
		get
		{
			return WindowID.EndGameStats;
		}
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x00027F75 File Offset: 0x00026175
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x0011A54C File Offset: 0x0011874C
	public override void Initialize()
	{
		this.m_canvasGroup = this.m_windowCanvas.GetComponent<CanvasGroup>();
		base.Initialize();
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x0011A59C File Offset: 0x0011879C
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

	// Token: 0x060048D9 RID: 18649 RVA: 0x00027F89 File Offset: 0x00026189
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

	// Token: 0x060048DA RID: 18650 RVA: 0x00027F98 File Offset: 0x00026198
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.m_biomeLightController.SetActive(false);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x00027FB7 File Offset: 0x000261B7
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x00027FE9 File Offset: 0x000261E9
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x0002801B File Offset: 0x0002621B
	private void OnCancelButtonDown(InputActionEventData data)
	{
		WindowManager.SetWindowIsOpen(WindowID.EndGameStats, false);
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x0011A620 File Offset: 0x00118820
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

	// Token: 0x060048DF RID: 18655 RVA: 0x0011A724 File Offset: 0x00118924
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

	// Token: 0x060048E0 RID: 18656 RVA: 0x0011A83C File Offset: 0x00118A3C
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

	// Token: 0x060048E1 RID: 18657 RVA: 0x0011A8E0 File Offset: 0x00118AE0
	private void Update()
	{
		if (this.m_windowCanvas.gameObject.activeSelf)
		{
			float num = -5f * Time.unscaledDeltaTime;
			this.m_ray.transform.SetLocalEulerZ(this.m_ray.transform.localEulerAngles.z + num);
		}
	}

	// Token: 0x040037D1 RID: 14289
	[SerializeField]
	private Image m_ray;

	// Token: 0x040037D2 RID: 14290
	[SerializeField]
	private GameObject m_biomeLightController;

	// Token: 0x040037D3 RID: 14291
	[Header("Banner")]
	[SerializeField]
	private TMP_Text m_bannerName;

	// Token: 0x040037D4 RID: 14292
	[Header("Player")]
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040037D5 RID: 14293
	[SerializeField]
	private TMP_Text m_classText;

	// Token: 0x040037D6 RID: 14294
	[SerializeField]
	private Image m_trait1Icon;

	// Token: 0x040037D7 RID: 14295
	[SerializeField]
	private Image m_trait2Icon;

	// Token: 0x040037D8 RID: 14296
	[SerializeField]
	private GameObject m_noTraitsText;

	// Token: 0x040037D9 RID: 14297
	[SerializeField]
	private Image m_weaponEquipmentIcon;

	// Token: 0x040037DA RID: 14298
	[SerializeField]
	private Image m_headEquipmentIcon;

	// Token: 0x040037DB RID: 14299
	[SerializeField]
	private Image m_chestEquipmentIcon;

	// Token: 0x040037DC RID: 14300
	[SerializeField]
	private Image m_capeEquipmentIcon;

	// Token: 0x040037DD RID: 14301
	[SerializeField]
	private Image m_trinketEquipmentIcon;

	// Token: 0x040037DE RID: 14302
	[SerializeField]
	private GameObject m_noGearText;

	// Token: 0x040037DF RID: 14303
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x040037E0 RID: 14304
	[Header("Stats")]
	[SerializeField]
	private EndGameStatsStatsEntry[] m_statsEntries;

	// Token: 0x040037E1 RID: 14305
	[Header("Rating")]
	[SerializeField]
	private EndGameStatsStatsEntry m_ratingEntry;

	// Token: 0x040037E2 RID: 14306
	private CanvasGroup m_canvasGroup;

	// Token: 0x040037E3 RID: 14307
	private Action<InputActionEventData> m_onCancelButtonDown;
}
