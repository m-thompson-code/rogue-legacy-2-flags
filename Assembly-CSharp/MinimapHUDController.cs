using System;
using System.Collections;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200060D RID: 1549
public class MinimapHUDController : MonoBehaviour
{
	// Token: 0x170012AB RID: 4779
	// (get) Token: 0x06002F9D RID: 12189 RVA: 0x0001A122 File Offset: 0x00018322
	// (set) Token: 0x06002F9E RID: 12190 RVA: 0x0001A129 File Offset: 0x00018329
	public static bool UpdateTextOnly { get; set; }

	// Token: 0x06002F9F RID: 12191 RVA: 0x000CB458 File Offset: 0x000C9658
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

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000CB53C File Offset: 0x000C973C
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

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000CB6EC File Offset: 0x000C98EC
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

	// Token: 0x06002FA2 RID: 12194 RVA: 0x0001A131 File Offset: 0x00018331
	private void OnExitChallenge(MonoBehaviour sender, EventArgs args)
	{
		this.m_runeCanvasGroup.gameObject.SetActive(true);
		this.m_equipmentCanvasGroup.gameObject.SetActive(true);
		this.m_soulCanvasGroup.gameObject.SetActive(true);
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x0001A166 File Offset: 0x00018366
	private void OnEnterWorld(MonoBehaviour sender, EventArgs args)
	{
		this.m_runeCanvasGroup.gameObject.SetActive(false);
		this.m_equipmentCanvasGroup.gameObject.SetActive(false);
		this.m_soulCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x0001A19B File Offset: 0x0001839B
	private void OnRuneChanged(MonoBehaviour sender, EventArgs args)
	{
		if ((args as RuneEquippedLevelChangeEventArgs).RuneType == RuneType.GoldGain)
		{
			this.OnGoldChanged(null, null);
		}
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000CB788 File Offset: 0x000C9988
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

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000CBA18 File Offset: 0x000C9C18
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

	// Token: 0x06002FA7 RID: 12199 RVA: 0x0001A1B4 File Offset: 0x000183B4
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

	// Token: 0x06002FA8 RID: 12200 RVA: 0x000CBB34 File Offset: 0x000C9D34
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

	// Token: 0x06002FA9 RID: 12201 RVA: 0x0001A1C3 File Offset: 0x000183C3
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

	// Token: 0x06002FAA RID: 12202 RVA: 0x000CBC50 File Offset: 0x000C9E50
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

	// Token: 0x06002FAB RID: 12203 RVA: 0x0001A1D2 File Offset: 0x000183D2
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

	// Token: 0x06002FAC RID: 12204 RVA: 0x000CBDC8 File Offset: 0x000C9FC8
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

	// Token: 0x04002706 RID: 9990
	private const float RESOURCE_DISPLAY_DURATION = 3f;

	// Token: 0x04002707 RID: 9991
	[SerializeField]
	private TMP_Text m_goldText;

	// Token: 0x04002708 RID: 9992
	[SerializeField]
	private RectTransform m_goldTextRectTransform;

	// Token: 0x04002709 RID: 9993
	[SerializeField]
	private TMP_Text m_goldModText;

	// Token: 0x0400270A RID: 9994
	[SerializeField]
	private RectTransform m_goldModTextRectTransform;

	// Token: 0x0400270B RID: 9995
	[SerializeField]
	private TMP_Text m_runeOreText;

	// Token: 0x0400270C RID: 9996
	[SerializeField]
	private TMP_Text m_equipmentOreText;

	// Token: 0x0400270D RID: 9997
	[SerializeField]
	private TMP_Text m_soulText;

	// Token: 0x0400270E RID: 9998
	[SerializeField]
	private CanvasGroup m_runeCanvasGroup;

	// Token: 0x0400270F RID: 9999
	[SerializeField]
	private CanvasGroup m_equipmentCanvasGroup;

	// Token: 0x04002710 RID: 10000
	[SerializeField]
	private CanvasGroup m_soulCanvasGroup;

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	private GameObject m_bankedGoldGO;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	private TMP_Text m_bankedGoldText;

	// Token: 0x04002713 RID: 10003
	[SerializeField]
	private TMP_Text m_ngPlusText;

	// Token: 0x04002714 RID: 10004
	[SerializeField]
	private CanvasGroup m_minimapCanvasGroup;

	// Token: 0x04002715 RID: 10005
	[SerializeField]
	private FadeOutHUDCollider m_fadeOutHUDCollider;

	// Token: 0x04002716 RID: 10006
	private bool m_runeDisplayComplete;

	// Token: 0x04002717 RID: 10007
	private bool m_equipmentDisplayComplete;

	// Token: 0x04002718 RID: 10008
	private bool m_soulDisplayComplete;

	// Token: 0x04002719 RID: 10009
	private Tween m_runeTween;

	// Token: 0x0400271A RID: 10010
	private Tween m_equipmentTween;

	// Token: 0x0400271B RID: 10011
	private Tween m_soulTween;

	// Token: 0x0400271C RID: 10012
	private Coroutine m_runeCoroutine;

	// Token: 0x0400271D RID: 10013
	private Coroutine m_equipmentCoroutine;

	// Token: 0x0400271E RID: 10014
	private Coroutine m_soulCoroutine;

	// Token: 0x0400271F RID: 10015
	private WaitRL_Yield m_runeWaitYield;

	// Token: 0x04002720 RID: 10016
	private WaitRL_Yield m_equipmentWaitYield;

	// Token: 0x04002721 RID: 10017
	private WaitRL_Yield m_soulWaitYield;

	// Token: 0x04002722 RID: 10018
	private Action<MonoBehaviour, EventArgs> m_onRuneChanged;

	// Token: 0x04002723 RID: 10019
	private Action<MonoBehaviour, EventArgs> m_onEnterWorld;

	// Token: 0x04002724 RID: 10020
	private Action<MonoBehaviour, EventArgs> m_onExitChallenge;

	// Token: 0x04002725 RID: 10021
	private Action<MonoBehaviour, EventArgs> m_onGoldChanged;

	// Token: 0x04002726 RID: 10022
	private Action<MonoBehaviour, EventArgs> m_onEquipmentOreChanged;

	// Token: 0x04002727 RID: 10023
	private Action<MonoBehaviour, EventArgs> m_onRuneOreChanged;

	// Token: 0x04002728 RID: 10024
	private Action<MonoBehaviour, EventArgs> m_onSoulChanged;

	// Token: 0x04002729 RID: 10025
	private Action<MonoBehaviour, EventArgs> m_onNewGamePlusChange;

	// Token: 0x0400272A RID: 10026
	private int m_previousSoulAmount;
}
