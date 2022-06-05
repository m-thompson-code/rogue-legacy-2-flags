using System;
using System.Collections;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000384 RID: 900
public class MinimapHUDController : MonoBehaviour
{
	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x060021A9 RID: 8617 RVA: 0x0006A73B File Offset: 0x0006893B
	// (set) Token: 0x060021AA RID: 8618 RVA: 0x0006A742 File Offset: 0x00068942
	public static bool UpdateTextOnly { get; set; }

	// Token: 0x060021AB RID: 8619 RVA: 0x0006A74C File Offset: 0x0006894C
	private void Awake()
	{
		this.m_runeWaitYield = new WaitRL_Yield(0f, false);
		this.m_equipmentWaitYield = new WaitRL_Yield(0f, false);
		this.m_soulWaitYield = new WaitRL_Yield(0f, false);
		this.m_fadeOutHUDCollider.SetCanvasGroup(this.m_minimapCanvasGroup);
		this.m_onRuneChanged = new Action<MonoBehaviour, EventArgs>(this.OnRuneChanged);
		this.m_onEnterWorld = new Action<MonoBehaviour, EventArgs>(this.OnEnterWorld);
		this.m_onExitChallenge = new Action<MonoBehaviour, EventArgs>(this.OnExitChallenge);
		this.m_onGoldChanged = new Action<MonoBehaviour, EventArgs>(this.OnGoldChanged);
		this.m_onEquipmentOreChanged = new Action<MonoBehaviour, EventArgs>(this.OnEquipmentOreChanged);
		this.m_onRuneOreChanged = new Action<MonoBehaviour, EventArgs>(this.OnRuneOreChanged);
		this.m_onSoulChanged = new Action<MonoBehaviour, EventArgs>(this.OnSoulChanged);
		this.m_onNewGamePlusChange = new Action<MonoBehaviour, EventArgs>(this.OnNewGamePlusChange);
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x0006A830 File Offset: 0x00068A30
	private void OnEnable()
	{
		this.m_goldText.text = SaveManager.PlayerSaveData.GoldCollected.ToString();
		this.m_runeOreText.text = SaveManager.PlayerSaveData.GoldCollected.ToString();
		this.m_equipmentOreText.text = SaveManager.PlayerSaveData.GoldCollected.ToString();
		this.m_soulText.text = SaveManager.PlayerSaveData.GoldCollected.ToString();
		this.m_bankedGoldText.text = SaveManager.PlayerSaveData.GoldSaved.ToString();
		this.OnNewGamePlusChange(null, null);
		this.m_goldModText.gameObject.SetActive(false);
		this.m_runeCanvasGroup.gameObject.SetActive(false);
		this.m_runeCanvasGroup.alpha = 0f;
		this.m_runeDisplayComplete = true;
		this.m_equipmentCanvasGroup.gameObject.SetActive(false);
		this.m_equipmentCanvasGroup.alpha = 0f;
		this.m_equipmentDisplayComplete = true;
		this.m_soulCanvasGroup.gameObject.SetActive(false);
		this.m_soulCanvasGroup.alpha = 0f;
		this.m_soulDisplayComplete = true;
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.RuneEquippedLevelChanged, this.m_onRuneChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterWorld);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ChallengeNPC_ExitChallenge, this.m_onExitChallenge);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GoldChanged, this.m_onGoldChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EquipmentOreChanged, this.m_onEquipmentOreChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.RuneOreChanged, this.m_onRuneOreChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SoulChanged, this.m_onSoulChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.NGPlusChanged, this.m_onNewGamePlusChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.HouseRulesChanged, this.m_onNewGamePlusChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onEnterWorld);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_onNewGamePlusChange);
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x0006A9E0 File Offset: 0x00068BE0
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.RuneEquippedLevelChanged, this.m_onRuneChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ChallengeNPC_EnterChallenge, this.m_onEnterWorld);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ChallengeNPC_ExitChallenge, this.m_onExitChallenge);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GoldChanged, this.m_onGoldChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EquipmentOreChanged, this.m_onEquipmentOreChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.RuneOreChanged, this.m_onRuneOreChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SoulChanged, this.m_onSoulChanged);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.NGPlusChanged, this.m_onNewGamePlusChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.HouseRulesChanged, this.m_onNewGamePlusChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onEnterWorld);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_onNewGamePlusChange);
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x0006AA7B File Offset: 0x00068C7B
	private void OnExitChallenge(MonoBehaviour sender, EventArgs args)
	{
		this.m_runeCanvasGroup.gameObject.SetActive(true);
		this.m_equipmentCanvasGroup.gameObject.SetActive(true);
		this.m_soulCanvasGroup.gameObject.SetActive(true);
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x0006AAB0 File Offset: 0x00068CB0
	private void OnEnterWorld(MonoBehaviour sender, EventArgs args)
	{
		this.m_runeCanvasGroup.gameObject.SetActive(false);
		this.m_equipmentCanvasGroup.gameObject.SetActive(false);
		this.m_soulCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x0006AAE5 File Offset: 0x00068CE5
	private void OnRuneChanged(MonoBehaviour sender, EventArgs args)
	{
		if ((args as RuneEquippedLevelChangeEventArgs).RuneType == RuneType.GoldGain)
		{
			this.OnGoldChanged(null, null);
		}
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x0006AB00 File Offset: 0x00068D00
	private void OnGoldChanged(MonoBehaviour sender, EventArgs args)
	{
		this.m_goldText.text = SaveManager.PlayerSaveData.GoldCollected.ToString();
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (!currentPlayerRoom.IsNativeNull() && currentPlayerRoom.BiomeType == BiomeType.HubTown && SkillTreeManager.GetSkillObjLevel(SkillTreeType.Gold_Saved_Unlock) > 0)
		{
			if (!this.m_bankedGoldGO.gameObject.activeSelf)
			{
				this.m_bankedGoldGO.gameObject.SetActive(true);
			}
			this.m_bankedGoldText.text = SaveManager.PlayerSaveData.GoldSaved.ToString();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_bankedGoldGO.GetComponent<RectTransform>());
		}
		else if (this.m_bankedGoldGO.gameObject.activeSelf)
		{
			this.m_bankedGoldGO.gameObject.SetActive(false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_goldTextRectTransform);
		float goldGainMod = Economy_EV.GetGoldGainMod();
		float num = 1f;
		if (SaveManager.PlayerSaveData.CastleLockState != CastleLockState.NotLocked)
		{
			if (SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.World))
			{
				num = NPC_EV.GetArchitectGoldMod(-1);
			}
			else
			{
				num = NPC_EV.GetArchitectGoldMod((int)(SaveManager.PlayerSaveData.TimesCastleLocked + 1));
			}
		}
		int num2 = Mathf.RoundToInt(((1f + goldGainMod) * num - 1f) * 100f);
		if (num2 > 0)
		{
			if (!this.m_goldModText.gameObject.activeSelf)
			{
				this.m_goldModText.gameObject.SetActive(true);
			}
			this.m_goldModText.text = "+" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num2);
			Vector2 anchoredPosition = this.m_goldModTextRectTransform.anchoredPosition;
			anchoredPosition.x = this.m_goldTextRectTransform.anchoredPosition.x + this.m_goldTextRectTransform.sizeDelta.x + 20f;
			this.m_goldModTextRectTransform.anchoredPosition = anchoredPosition;
			return;
		}
		if (num2 < 0)
		{
			if (!this.m_goldModText.gameObject.activeSelf)
			{
				this.m_goldModText.gameObject.SetActive(true);
			}
			this.m_goldModText.text = "<color=red>" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num2) + "</color>";
			Vector2 anchoredPosition2 = this.m_goldModTextRectTransform.anchoredPosition;
			anchoredPosition2.x = this.m_goldTextRectTransform.anchoredPosition.x + this.m_goldTextRectTransform.sizeDelta.x + 20f;
			this.m_goldModTextRectTransform.anchoredPosition = anchoredPosition2;
			return;
		}
		if (this.m_goldModText.gameObject.activeSelf)
		{
			this.m_goldModText.gameObject.SetActive(false);
		}
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x0006AD90 File Offset: 0x00068F90
	private void OnRuneOreChanged(MonoBehaviour sender, EventArgs args)
	{
		this.m_runeOreText.text = SaveManager.PlayerSaveData.RuneOreCollected.ToString();
		if (MinimapHUDController.UpdateTextOnly)
		{
			if (this.m_runeCanvasGroup.isActiveAndEnabled)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_runeCanvasGroup.GetComponent<RectTransform>());
			}
			return;
		}
		if (this.m_runeDisplayComplete)
		{
			if (this.m_runeCoroutine != null)
			{
				if (this.m_runeCanvasGroup.gameObject.activeSelf)
				{
					this.m_runeCanvasGroup.gameObject.SetActive(false);
				}
				base.StopCoroutine(this.m_runeCoroutine);
			}
			if (this.m_runeTween != null)
			{
				this.m_runeTween.StopTweenWithConditionChecks(false, this.m_runeCanvasGroup, null);
			}
			if (!SaveManager.PlayerSaveData.InHubTown || ChallengeManager.IsInChallenge)
			{
				this.m_runeCoroutine = base.StartCoroutine(this.RuneOreDisplayCoroutine());
			}
			else
			{
				this.m_runeCanvasGroup.gameObject.SetActive(true);
				this.m_runeCanvasGroup.alpha = 1f;
			}
		}
		else
		{
			this.m_runeWaitYield.CreateNew(3f, false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_runeCanvasGroup.GetComponent<RectTransform>());
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x0006AEAA File Offset: 0x000690AA
	private IEnumerator RuneOreDisplayCoroutine()
	{
		this.m_runeDisplayComplete = false;
		this.m_runeCanvasGroup.gameObject.SetActive(true);
		this.m_runeCanvasGroup.alpha = 0f;
		TweenManager.TweenTo(this.m_runeCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.m_runeWaitYield.CreateNew(3f, false);
		yield return this.m_runeWaitYield;
		this.m_runeDisplayComplete = true;
		this.m_runeTween = TweenManager.TweenTo(this.m_runeCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return this.m_runeTween.TweenCoroutine;
		this.m_runeCanvasGroup.gameObject.SetActive(false);
		this.m_runeTween = null;
		yield break;
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x0006AEBC File Offset: 0x000690BC
	private void OnEquipmentOreChanged(MonoBehaviour sender, EventArgs args)
	{
		this.m_equipmentOreText.text = SaveManager.PlayerSaveData.EquipmentOreCollected.ToString();
		if (MinimapHUDController.UpdateTextOnly)
		{
			if (this.m_equipmentCanvasGroup.isActiveAndEnabled)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_equipmentCanvasGroup.GetComponent<RectTransform>());
			}
			return;
		}
		if (this.m_equipmentDisplayComplete)
		{
			if (this.m_equipmentCoroutine != null)
			{
				if (this.m_equipmentCanvasGroup.gameObject.activeSelf)
				{
					this.m_equipmentCanvasGroup.gameObject.SetActive(false);
				}
				base.StopCoroutine(this.m_equipmentCoroutine);
			}
			if (this.m_equipmentTween != null)
			{
				this.m_equipmentTween.StopTweenWithConditionChecks(false, this.m_equipmentCanvasGroup, null);
			}
			if (!SaveManager.PlayerSaveData.InHubTown || ChallengeManager.IsInChallenge)
			{
				this.m_equipmentCoroutine = base.StartCoroutine(this.EquipmentDisplayCoroutine());
			}
			else
			{
				this.m_equipmentCanvasGroup.gameObject.SetActive(true);
				this.m_equipmentCanvasGroup.alpha = 1f;
			}
		}
		else
		{
			this.m_equipmentWaitYield.CreateNew(3f, false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_equipmentCanvasGroup.GetComponent<RectTransform>());
	}

	// Token: 0x060021B5 RID: 8629 RVA: 0x0006AFD6 File Offset: 0x000691D6
	private IEnumerator EquipmentDisplayCoroutine()
	{
		this.m_equipmentDisplayComplete = false;
		this.m_equipmentCanvasGroup.gameObject.SetActive(true);
		this.m_equipmentCanvasGroup.alpha = 0f;
		TweenManager.TweenTo(this.m_equipmentCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.m_equipmentWaitYield.CreateNew(3f, false);
		yield return this.m_equipmentWaitYield;
		this.m_equipmentDisplayComplete = true;
		this.m_equipmentTween = TweenManager.TweenTo(this.m_equipmentCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return this.m_equipmentTween.TweenCoroutine;
		this.m_equipmentCanvasGroup.gameObject.SetActive(false);
		this.m_equipmentTween = null;
		yield break;
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x0006AFE8 File Offset: 0x000691E8
	private void OnSoulChanged(MonoBehaviour sender, EventArgs args)
	{
		int num = Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true) - SoulDrop.FakeSoulCounter_STATIC;
		bool flag = SaveManager.PlayerSaveData.InHubTown && (!this.m_soulCanvasGroup.gameObject.activeSelf || this.m_soulCanvasGroup.alpha <= 0f);
		if (this.m_previousSoulAmount == num && !flag)
		{
			return;
		}
		this.m_previousSoulAmount = num;
		this.m_soulText.text = num.ToString();
		if (MinimapHUDController.UpdateTextOnly)
		{
			if (this.m_soulCanvasGroup.isActiveAndEnabled)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_soulCanvasGroup.GetComponent<RectTransform>());
			}
			return;
		}
		if (this.m_soulDisplayComplete)
		{
			if (this.m_soulCoroutine != null)
			{
				if (this.m_soulCanvasGroup.gameObject.activeSelf)
				{
					this.m_soulCanvasGroup.gameObject.SetActive(false);
				}
				base.StopCoroutine(this.m_soulCoroutine);
			}
			if (this.m_soulTween != null)
			{
				this.m_soulTween.StopTweenWithConditionChecks(false, this.m_soulCanvasGroup, null);
			}
			if (!SaveManager.PlayerSaveData.InHubTown || ChallengeManager.IsInChallenge)
			{
				this.m_soulCoroutine = base.StartCoroutine(this.SoulDisplayCoroutine());
			}
			else
			{
				this.m_soulCanvasGroup.gameObject.SetActive(true);
				this.m_soulCanvasGroup.alpha = 1f;
			}
		}
		else
		{
			this.m_soulWaitYield.CreateNew(3f, false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_soulCanvasGroup.GetComponent<RectTransform>());
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x0006B15F File Offset: 0x0006935F
	private IEnumerator SoulDisplayCoroutine()
	{
		this.m_soulDisplayComplete = false;
		this.m_soulCanvasGroup.gameObject.SetActive(true);
		this.m_soulCanvasGroup.alpha = 0f;
		TweenManager.TweenTo(this.m_soulCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.m_soulWaitYield.CreateNew(3f, false);
		yield return this.m_soulWaitYield;
		this.m_soulDisplayComplete = true;
		this.m_soulTween = TweenManager.TweenTo(this.m_soulCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return this.m_soulTween.TweenCoroutine;
		this.m_soulCanvasGroup.gameObject.SetActive(false);
		this.m_soulTween = null;
		yield break;
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x0006B170 File Offset: 0x00069370
	private void OnNewGamePlusChange(MonoBehaviour sender, EventArgs args)
	{
		this.m_ngPlusText.text = "";
		if (SaveManager.PlayerSaveData.NewGamePlusLevel > 0)
		{
			this.m_ngPlusText.gameObject.SetActive(true);
			TMP_Text ngPlusText = this.m_ngPlusText;
			ngPlusText.text += string.Format(LocalizationManager.GetString("LOC_ID_MINIMAP_NG_TITLE_1", false, false), SaveManager.PlayerSaveData.NewGamePlusLevel);
		}
		int totalBurdenWeight = BurdenManager.GetTotalBurdenWeight();
		if (totalBurdenWeight > 0)
		{
			TMP_Text ngPlusText2 = this.m_ngPlusText;
			ngPlusText2.text += string.Format(LocalizationManager.GetString("LOC_ID_MINIMAP_BURDEN_TITLE_1", false, false), totalBurdenWeight);
		}
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			if (this.m_ngPlusText.text.Length > 0)
			{
				TMP_Text ngPlusText3 = this.m_ngPlusText;
				ngPlusText3.text += "\n";
			}
			if (SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay)
			{
				string @string = LocalizationManager.GetString("LOC_ID_ASSIST_MODE_DIFFICULTY_MINIMAP_DISPLAY_1", false, false);
				TMP_Text ngPlusText4 = this.m_ngPlusText;
				ngPlusText4.text += string.Format(@string, AssistMode_EV.GetDifficulty());
				return;
			}
			TMP_Text ngPlusText5 = this.m_ngPlusText;
			ngPlusText5.text += LocalizationManager.GetString("LOC_ID_MINIMAP_HOUSE_RULES_TITLE_1", false, false);
		}
	}

	// Token: 0x04001D1D RID: 7453
	private const float RESOURCE_DISPLAY_DURATION = 3f;

	// Token: 0x04001D1E RID: 7454
	[SerializeField]
	private TMP_Text m_goldText;

	// Token: 0x04001D1F RID: 7455
	[SerializeField]
	private RectTransform m_goldTextRectTransform;

	// Token: 0x04001D20 RID: 7456
	[SerializeField]
	private TMP_Text m_goldModText;

	// Token: 0x04001D21 RID: 7457
	[SerializeField]
	private RectTransform m_goldModTextRectTransform;

	// Token: 0x04001D22 RID: 7458
	[SerializeField]
	private TMP_Text m_runeOreText;

	// Token: 0x04001D23 RID: 7459
	[SerializeField]
	private TMP_Text m_equipmentOreText;

	// Token: 0x04001D24 RID: 7460
	[SerializeField]
	private TMP_Text m_soulText;

	// Token: 0x04001D25 RID: 7461
	[SerializeField]
	private CanvasGroup m_runeCanvasGroup;

	// Token: 0x04001D26 RID: 7462
	[SerializeField]
	private CanvasGroup m_equipmentCanvasGroup;

	// Token: 0x04001D27 RID: 7463
	[SerializeField]
	private CanvasGroup m_soulCanvasGroup;

	// Token: 0x04001D28 RID: 7464
	[SerializeField]
	private GameObject m_bankedGoldGO;

	// Token: 0x04001D29 RID: 7465
	[SerializeField]
	private TMP_Text m_bankedGoldText;

	// Token: 0x04001D2A RID: 7466
	[SerializeField]
	private TMP_Text m_ngPlusText;

	// Token: 0x04001D2B RID: 7467
	[SerializeField]
	private CanvasGroup m_minimapCanvasGroup;

	// Token: 0x04001D2C RID: 7468
	[SerializeField]
	private FadeOutHUDCollider m_fadeOutHUDCollider;

	// Token: 0x04001D2D RID: 7469
	private bool m_runeDisplayComplete;

	// Token: 0x04001D2E RID: 7470
	private bool m_equipmentDisplayComplete;

	// Token: 0x04001D2F RID: 7471
	private bool m_soulDisplayComplete;

	// Token: 0x04001D30 RID: 7472
	private Tween m_runeTween;

	// Token: 0x04001D31 RID: 7473
	private Tween m_equipmentTween;

	// Token: 0x04001D32 RID: 7474
	private Tween m_soulTween;

	// Token: 0x04001D33 RID: 7475
	private Coroutine m_runeCoroutine;

	// Token: 0x04001D34 RID: 7476
	private Coroutine m_equipmentCoroutine;

	// Token: 0x04001D35 RID: 7477
	private Coroutine m_soulCoroutine;

	// Token: 0x04001D36 RID: 7478
	private WaitRL_Yield m_runeWaitYield;

	// Token: 0x04001D37 RID: 7479
	private WaitRL_Yield m_equipmentWaitYield;

	// Token: 0x04001D38 RID: 7480
	private WaitRL_Yield m_soulWaitYield;

	// Token: 0x04001D39 RID: 7481
	private Action<MonoBehaviour, EventArgs> m_onRuneChanged;

	// Token: 0x04001D3A RID: 7482
	private Action<MonoBehaviour, EventArgs> m_onEnterWorld;

	// Token: 0x04001D3B RID: 7483
	private Action<MonoBehaviour, EventArgs> m_onExitChallenge;

	// Token: 0x04001D3C RID: 7484
	private Action<MonoBehaviour, EventArgs> m_onGoldChanged;

	// Token: 0x04001D3D RID: 7485
	private Action<MonoBehaviour, EventArgs> m_onEquipmentOreChanged;

	// Token: 0x04001D3E RID: 7486
	private Action<MonoBehaviour, EventArgs> m_onRuneOreChanged;

	// Token: 0x04001D3F RID: 7487
	private Action<MonoBehaviour, EventArgs> m_onSoulChanged;

	// Token: 0x04001D40 RID: 7488
	private Action<MonoBehaviour, EventArgs> m_onNewGamePlusChange;

	// Token: 0x04001D41 RID: 7489
	private int m_previousSoulAmount;
}
