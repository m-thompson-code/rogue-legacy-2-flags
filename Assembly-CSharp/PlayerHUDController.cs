using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020003D9 RID: 985
public class PlayerHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x06002432 RID: 9266 RVA: 0x0007794C File Offset: 0x00075B4C
	private void Awake()
	{
		this.m_fadeOutHUDCollider.SetCanvasGroup(this.m_hudCanvasGroup);
		this.m_fadeOutHPCollider = this.m_fadeOutHUDCollider.GetComponents<BoxCollider2D>()[1];
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.m_tweenInHUD = new Action<MonoBehaviour, EventArgs>(this.TweenInHUD);
		this.m_tweenOutHUD = new Action<MonoBehaviour, EventArgs>(this.TweenOutHUD);
		this.m_onWorldCreated = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreated);
		this.m_onHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnHealthChange);
		this.m_onExhaustChange = new Action<MonoBehaviour, EventArgs>(this.OnExhaustChange);
		this.m_onMaxHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnMaxHealthChange);
		this.m_onManaChange = new Action<MonoBehaviour, EventArgs>(this.OnManaChange);
		this.m_onResolveChange = new Action<MonoBehaviour, EventArgs>(this.OnResolveChange);
		this.m_onSkillTreeOpen = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeOpen);
		this.m_onSkillTreeClosed = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeClosed);
		this.m_onSkillLevelChanged = new Action<MonoBehaviour, EventArgs>(this.OnSkillLevelChanged);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x00077A67 File Offset: 0x00075C67
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x00077A7C File Offset: 0x00075C7C
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (GameUtility.SceneHasRooms && SceneLoadingUtility.ActiveScene.name != SceneLoadingUtility.GetSceneName(SceneID.Lineage))
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x00077ADE File Offset: 0x00075CDE
	private IEnumerator Start()
	{
		this.m_spellOrbStoredPos = this.m_spellOrbIcon.transform.parent.localPosition;
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.UpdateHealth(playerController);
		this.UpdateMana(playerController);
		this.UpdateShield(playerController);
		this.UpdateSpellOrbs(playerController);
		this.UpdateResolve(playerController);
		this.UpdateExhaust(playerController);
		this.OnSkillLevelChanged(null, null);
		yield break;
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x00077AF0 File Offset: 0x00075CF0
	private void OnEnable()
	{
		this.m_spellOrbIcon.gameObject.SetActive(false);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.DisplayPlayerHUD, this.m_tweenInHUD);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.HidePlayerHUD, this.m_tweenOutHUD);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreated);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExhaustChange, this.m_onExhaustChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerMaxHealthChange, this.m_onMaxHealthChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerManaChange, this.m_onManaChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerResolveChanged, this.m_onResolveChange);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpen);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PauseWindow_Opened, this.m_onSkillTreeOpen);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PauseWindow_Closed, this.m_onSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x00077BC0 File Offset: 0x00075DC0
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.DisplayPlayerHUD, this.m_tweenInHUD);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.HidePlayerHUD, this.m_tweenOutHUD);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreated);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExhaustChange, this.m_onExhaustChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerMaxHealthChange, this.m_onMaxHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerManaChange, this.m_onManaChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerResolveChanged, this.m_onResolveChange);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpen);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Opened, this.m_onSkillTreeOpen);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Closed, this.m_onSkillTreeClosed);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillLevelChanged, this.m_onSkillLevelChanged);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x00077C80 File Offset: 0x00075E80
	private void TweenOutHUD(object sender, EventArgs args)
	{
		this.m_storedFadeOutHUDAlpha = 1f;
		if (this.m_hudCanvasGroup.alpha != 0f)
		{
			this.m_storedFadeOutHUDAlpha = this.m_hudCanvasGroup.alpha;
		}
		this.m_storedFadeOutMinimapHUDAlpha = 1f;
		if (this.m_minimapCanvasGroup.alpha != 0f)
		{
			this.m_storedFadeOutMinimapHUDAlpha = this.m_minimapCanvasGroup.alpha;
		}
		float duration = 0f;
		if (args != null)
		{
			PlayerHUDVisibilityEventArgs playerHUDVisibilityEventArgs = args as PlayerHUDVisibilityEventArgs;
			if (playerHUDVisibilityEventArgs != null)
			{
				duration = playerHUDVisibilityEventArgs.TweenDuration;
			}
		}
		if (this.m_playerHUDAlphaTween)
		{
			this.m_playerHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_hudCanvasGroup, null);
		}
		if (this.m_minimapHUDAlphaTween)
		{
			this.m_minimapHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_minimapCanvasGroup, null);
		}
		this.m_playerHUDAlphaTween = TweenManager.TweenTo_UnscaledTime(this.m_hudCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		this.m_playerHUDAlphaTween.ID = "HUDFadeTween";
		this.m_minimapHUDAlphaTween = TweenManager.TweenTo_UnscaledTime(this.m_minimapCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x00077DC0 File Offset: 0x00075FC0
	private void TweenInHUD(object sender, EventArgs args)
	{
		float duration = 0f;
		if (args != null)
		{
			PlayerHUDVisibilityEventArgs playerHUDVisibilityEventArgs = args as PlayerHUDVisibilityEventArgs;
			if (playerHUDVisibilityEventArgs != null)
			{
				duration = playerHUDVisibilityEventArgs.TweenDuration;
			}
		}
		if (this.m_playerHUDAlphaTween)
		{
			this.m_playerHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_hudCanvasGroup, null);
		}
		if (this.m_minimapHUDAlphaTween)
		{
			this.m_minimapHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_minimapCanvasGroup, null);
		}
		this.m_playerHUDAlphaTween = TweenManager.TweenTo_UnscaledTime(this.m_hudCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			this.m_storedFadeOutHUDAlpha
		});
		this.m_playerHUDAlphaTween.ID = "HUDFadeTween";
		this.m_minimapHUDAlphaTween = TweenManager.TweenTo_UnscaledTime(this.m_minimapCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			this.m_storedFadeOutMinimapHUDAlpha
		});
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x00077EAC File Offset: 0x000760AC
	private void OnWorldCreated(object sender, EventArgs args)
	{
		if (this.m_playerHUDAlphaTween)
		{
			this.m_playerHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_hudCanvasGroup, null);
		}
		if (this.m_minimapHUDAlphaTween)
		{
			this.m_minimapHUDAlphaTween.StopTweenWithConditionChecks(false, this.m_minimapCanvasGroup, null);
		}
		this.m_hudCanvasGroup.alpha = 1f;
		this.m_minimapCanvasGroup.alpha = 1f;
		this.m_minimapCanvasGroup.gameObject.SetActive(true);
		if (!TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			this.m_minimapFrameGO.SetActive(true);
			return;
		}
		this.m_minimapFrameGO.SetActive(false);
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x00077F50 File Offset: 0x00076150
	private void OnSkillTreeOpen(object sender, EventArgs eventArgs)
	{
		if (this.m_playerHUDAlphaTween && this.m_playerHUDAlphaTween.isActiveAndEnabled && this.m_playerHUDAlphaTween.ID == "HUDFadeTween")
		{
			this.m_playerHUDAlphaTween.Complete();
		}
		this.m_storedFadeOutHUDAlpha = this.m_hudCanvasGroup.alpha;
		this.m_hudCanvasGroup.alpha = 0f;
		this.m_minimapCanvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x00077FCC File Offset: 0x000761CC
	private void OnSkillTreeClosed(object sender, EventArgs eventArgs)
	{
		this.m_hudCanvasGroup.alpha = this.m_storedFadeOutHUDAlpha;
		this.m_minimapCanvasGroup.gameObject.SetActive(true);
		if (!TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			this.m_minimapFrameGO.SetActive(true);
			return;
		}
		this.m_minimapFrameGO.SetActive(false);
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x00078020 File Offset: 0x00076220
	private void OnSkillLevelChanged(object sender, EventArgs eventArgs)
	{
		this.m_levelText.text = string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_CHARACTER_LEVEL_1", false, false), SkillTreeManager.GetTotalSkillObjLevel());
		Vector2 anchoredPosition = this.m_resolveTransform.anchoredPosition;
		anchoredPosition.x = this.m_levelText.preferredWidth;
		this.m_resolveTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x00078080 File Offset: 0x00076280
	private void OnHealthChange(object sender, EventArgs eventArgs)
	{
		if (PlayerHUDController.IgnoreHealthChangeEvents)
		{
			return;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.UpdateHealth(playerController);
		this.UpdateShield(playerController);
		this.UpdateSpellOrbs(playerController);
		this.UpdateExhaust(playerController);
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x000780B8 File Offset: 0x000762B8
	private void OnExhaustChange(object sender, EventArgs eventArgs)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.UpdateExhaust(playerController);
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000780D4 File Offset: 0x000762D4
	private void UpdateExhaust(PlayerController playerController)
	{
		float num = (float)playerController.CurrentExhaust * 0.01f;
		Vector2 sizeDelta = this.m_hpBarRectTransform.sizeDelta;
		float num2 = this.m_hpBarPanelRectTransform.sizeDelta.x + sizeDelta.x;
		if (playerController.ActualMaxHealth > 0)
		{
			num2 *= playerController.CurrentHealth / (float)playerController.ActualMaxHealth;
		}
		sizeDelta.x = num2 - this.m_hpBarPanelRectTransform.sizeDelta.x;
		this.m_hpExhaustBarRectTransform.sizeDelta = sizeDelta;
		float num3 = (float)playerController.ActualMaxHealth * num;
		float num4 = 1f;
		if (playerController.CurrentHealth > 0f)
		{
			num4 = Mathf.Clamp((playerController.CurrentHealth - num3) / playerController.CurrentHealth, 0f, 1f);
		}
		this.m_hpExhaustBar.fillAmount = 1f - num4;
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000781A4 File Offset: 0x000763A4
	private void OnMaxHealthChange(object sender, EventArgs eventArgs)
	{
		MaxHealthChangeEventArgs maxHealthChangeEventArgs = eventArgs as MaxHealthChangeEventArgs;
		int num = (int)(maxHealthChangeEventArgs.NewMaxHealthValue - maxHealthChangeEventArgs.PrevMaxHealthValue);
		if (num != 0)
		{
			Vector2 absPos = new Vector2(this.m_hpBarPanelRectTransform.rect.width, -this.m_hpBarPanelRectTransform.sizeDelta.y);
			if (this.m_shieldIcon.gameObject.activeSelf)
			{
				absPos.x += this.m_shieldIcon.sizeDelta.x + 80f;
			}
			else
			{
				absPos.x += 80f;
			}
			if (this.m_maxHealthTextPopup && this.m_maxHealthTextPopup.gameObject.activeInHierarchy)
			{
				num = this.m_lastMaxHealthChange + num;
				this.m_maxHealthTextPopup.gameObject.SetActive(false);
			}
			string text = string.Format(LocalizationManager.GetString("LOC_ID_HEALING_ROOM_POP_UP_MAX_HEALTH_1", false, false), num);
			if (num < 0)
			{
				text = "<color=\"red\">" + text.Replace('+', ' ') + "</color>";
			}
			this.m_maxHealthTextPopup = TextPopupManager.DisplayTextAtAbsPos(TextPopupType.MaxHealthChange, text, absPos, this.m_hpBarPanelRectTransform.gameObject, TextAlignmentOptions.Right);
			this.m_lastMaxHealthChange = num;
		}
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x000782D4 File Offset: 0x000764D4
	private void UpdateHealth(PlayerController playerController)
	{
		bool flag = (this.m_storedCurrentArmor > 0 && playerController.CurrentArmor <= 0) || (this.m_storedCurrentArmor <= 0 && playerController.CurrentArmor > 0);
		if ((float)playerController.ActualMaxHealth != this.m_storedMaxHP || flag)
		{
			float num = ((float)playerController.ActualMaxHealth + 220f) / 1000f;
			float value = 1000f * num;
			float min = (playerController.ActualMaxHealth < 100) ? 270f : 320f;
			Vector2 sizeDelta = this.m_hpBarPanelRectTransform.sizeDelta;
			sizeDelta.x = Mathf.Clamp(value, min, 1000f);
			this.m_hpBarPanelRectTransform.sizeDelta = sizeDelta;
			this.m_storedMaxHP = (float)playerController.ActualMaxHealth;
			Vector2 anchoredPosition = this.m_shieldIcon.anchoredPosition;
			anchoredPosition.x = this.m_hpBarPanelRectTransform.anchoredPosition.x + this.m_hpBarPanelRectTransform.sizeDelta.x + 70f;
			this.m_shieldIcon.anchoredPosition = anchoredPosition;
			Vector2 offset = this.m_fadeOutHPCollider.offset;
			offset.x = this.m_hpBarPanelRectTransform.anchoredPosition.x + sizeDelta.x / 4f;
			if (playerController.CurrentArmor > 0)
			{
				float num2 = this.m_shieldIcon.sizeDelta.x / 2f;
				sizeDelta.x += num2;
				offset.x += num2;
			}
			this.m_fadeOutHPCollider.size = sizeDelta;
			this.m_fadeOutHPCollider.offset = offset;
		}
		float fillAmount = (float)playerController.CurrentHealthAsInt / (float)playerController.ActualMaxHealth;
		if (!TraitManager.IsTraitActive(TraitType.NoHealthBar))
		{
			this.m_hpBar.fillAmount = fillAmount;
		}
		else
		{
			this.m_hpBar.fillAmount = 0f;
		}
		if (TraitManager.IsTraitActive(TraitType.NoHealthBar))
		{
			if (this.m_hpText.gameObject.activeSelf)
			{
				this.m_hpText.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			if (!this.m_hpText.gameObject.activeSelf)
			{
				this.m_hpText.gameObject.SetActive(true);
			}
			this.m_hpText.text = string.Format("{0} / {1}", playerController.CurrentHealthAsInt, playerController.ActualMaxHealth);
		}
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x00078518 File Offset: 0x00076718
	private void OnManaChange(object sender, EventArgs eventArgs)
	{
		ManaChangeEventArgs manaChangeEventArgs = eventArgs as ManaChangeEventArgs;
		this.UpdateMana(manaChangeEventArgs.Player);
		this.UpdateSpellOrbs(PlayerManager.GetPlayerController());
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x00078543 File Offset: 0x00076743
	private void OnResolveChange(object sender, EventArgs eventArgs)
	{
		this.UpdateResolve(PlayerManager.GetPlayerController());
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x00078550 File Offset: 0x00076750
	private void UpdateMana(PlayerController player)
	{
		if (this.m_storedMaxMP != (float)player.ActualMaxMana)
		{
			float num = (float)player.ActualMaxMana / 500f;
			float value = 800f * num;
			float min = (player.ActualMaxMana < 100) ? 230f : 280f;
			Vector2 sizeDelta = this.m_mpBarRectTransform.sizeDelta;
			sizeDelta.x = Mathf.Clamp(value, min, 800f);
			this.m_mpBarRectTransform.sizeDelta = sizeDelta;
			this.m_storedMaxMP = (float)player.ActualMaxMana;
		}
		float fillAmount = player.CurrentMana / (float)player.ActualMaxMana;
		this.m_mpBar.fillAmount = fillAmount;
		this.m_mpText.text = string.Format("{0} / {1}", player.CurrentManaAsInt, player.ActualMaxMana);
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x0007861C File Offset: 0x0007681C
	private void UpdateShield(PlayerController playerController)
	{
		int currentArmor = playerController.CurrentArmor;
		if (currentArmor <= 0)
		{
			if (this.m_shieldIcon.gameObject.activeSelf)
			{
				this.m_shieldIcon.gameObject.SetActive(false);
				this.m_shieldText.gameObject.SetActive(false);
			}
		}
		else
		{
			if (!this.m_shieldIcon.gameObject.activeSelf)
			{
				this.m_shieldIcon.gameObject.SetActive(true);
			}
			if (this.m_storedCurrentArmor != currentArmor)
			{
				Vector3 vector = Vector3.one;
				Vector3 vector2 = vector;
				vector *= 1.2f;
				this.m_shieldIcon.transform.localScale = vector;
				if (this.m_shieldTween != null)
				{
					this.m_shieldTween.StopTweenWithConditionChecks(false, this.m_shieldIcon.transform, null);
				}
				this.m_shieldTween = TweenManager.TweenTo_UnscaledTime(this.m_shieldIcon.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"localScale.x",
					vector2.x,
					"localScale.y",
					vector2.y,
					"localScale.z",
					vector2.z
				});
				if (currentArmor > 0)
				{
					this.m_shieldText.gameObject.SetActive(true);
					this.m_shieldText.text = currentArmor.ToString();
				}
				else
				{
					this.m_shieldText.gameObject.SetActive(false);
				}
			}
		}
		this.m_storedCurrentArmor = currentArmor;
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x0007879C File Offset: 0x0007699C
	private void UpdateSpellOrbs(PlayerController playerController)
	{
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000787AC File Offset: 0x000769AC
	private void UpdateResolve(PlayerController playerController)
	{
		float actualResolve = playerController.ActualResolve;
		Color color = Color.white;
		if (actualResolve <= 0.5f)
		{
			color = Color.red;
		}
		else if (actualResolve <= 1f)
		{
			color = Color.yellow;
		}
		this.m_resolveText.color = color;
		this.m_resolveText.text = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), Mathf.RoundToInt(actualResolve * 100f));
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x0007881D File Offset: 0x00076A1D
	public void RefreshText(object sender, EventArgs args)
	{
		this.OnSkillLevelChanged(null, null);
	}

	// Token: 0x04001EA8 RID: 7848
	private const float MIN_HP_BAR_3_DIGITS_WIDTH = 320f;

	// Token: 0x04001EA9 RID: 7849
	private const float MIN_HP_BAR_2_DIGITS_WIDTH = 270f;

	// Token: 0x04001EAA RID: 7850
	private const float MIN_MP_BAR_3_DIGITS_WIDTH = 280f;

	// Token: 0x04001EAB RID: 7851
	private const float MIN_MP_BAR_2_DIGITS_WIDTH = 230f;

	// Token: 0x04001EAC RID: 7852
	private const float MAX_HP_BAR_WIDTH = 1000f;

	// Token: 0x04001EAD RID: 7853
	private const float MAX_MP_BAR_WIDTH = 800f;

	// Token: 0x04001EAE RID: 7854
	private const float APPROXIMATE_MAX_HP = 1000f;

	// Token: 0x04001EAF RID: 7855
	private const float APPROXIMATE_MAX_MP = 500f;

	// Token: 0x04001EB0 RID: 7856
	private const string HUD_FADE_TWEEN_ID = "HUDFadeTween";

	// Token: 0x04001EB1 RID: 7857
	[SerializeField]
	private CanvasGroup m_hudCanvasGroup;

	// Token: 0x04001EB2 RID: 7858
	[SerializeField]
	private CanvasGroup m_minimapCanvasGroup;

	// Token: 0x04001EB3 RID: 7859
	[SerializeField]
	private GameObject m_minimapFrameGO;

	// Token: 0x04001EB4 RID: 7860
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001EB5 RID: 7861
	[SerializeField]
	private RectTransform m_hpBarPanelRectTransform;

	// Token: 0x04001EB6 RID: 7862
	[SerializeField]
	private Image m_hpBar;

	// Token: 0x04001EB7 RID: 7863
	[SerializeField]
	private RectTransform m_hpBarRectTransform;

	// Token: 0x04001EB8 RID: 7864
	[SerializeField]
	private Image m_hpExhaustBar;

	// Token: 0x04001EB9 RID: 7865
	[SerializeField]
	private RectTransform m_hpExhaustBarRectTransform;

	// Token: 0x04001EBA RID: 7866
	[SerializeField]
	private TMP_Text m_hpText;

	// Token: 0x04001EBB RID: 7867
	[SerializeField]
	private RectTransform m_mpBarRectTransform;

	// Token: 0x04001EBC RID: 7868
	[SerializeField]
	private Image m_mpBar;

	// Token: 0x04001EBD RID: 7869
	[SerializeField]
	private TMP_Text m_mpText;

	// Token: 0x04001EBE RID: 7870
	[SerializeField]
	private RectTransform m_shieldIcon;

	// Token: 0x04001EBF RID: 7871
	[SerializeField]
	private TMP_Text m_shieldText;

	// Token: 0x04001EC0 RID: 7872
	[SerializeField]
	private GameObject m_spellOrbIcon;

	// Token: 0x04001EC1 RID: 7873
	[SerializeField]
	private TMP_Text m_spellOrbText;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	private TMP_Text m_resolveText;

	// Token: 0x04001EC3 RID: 7875
	[SerializeField]
	private RectTransform m_resolveTransform;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	private FadeOutHUDCollider m_fadeOutHUDCollider;

	// Token: 0x04001EC5 RID: 7877
	private Vector3 m_spellOrbStoredPos;

	// Token: 0x04001EC6 RID: 7878
	private float m_storedMaxHP;

	// Token: 0x04001EC7 RID: 7879
	private float m_storedMaxMP;

	// Token: 0x04001EC8 RID: 7880
	private Tween m_playerHUDAlphaTween;

	// Token: 0x04001EC9 RID: 7881
	private Tween m_minimapHUDAlphaTween;

	// Token: 0x04001ECA RID: 7882
	private TextPopupObj m_maxHealthTextPopup;

	// Token: 0x04001ECB RID: 7883
	private int m_lastMaxHealthChange;

	// Token: 0x04001ECC RID: 7884
	private float m_storedFadeOutHUDAlpha = 1f;

	// Token: 0x04001ECD RID: 7885
	private float m_storedFadeOutMinimapHUDAlpha = 1f;

	// Token: 0x04001ECE RID: 7886
	private BoxCollider2D m_fadeOutHPCollider;

	// Token: 0x04001ECF RID: 7887
	private Action<MonoBehaviour, EventArgs> m_tweenInHUD;

	// Token: 0x04001ED0 RID: 7888
	private Action<MonoBehaviour, EventArgs> m_tweenOutHUD;

	// Token: 0x04001ED1 RID: 7889
	private Action<MonoBehaviour, EventArgs> m_onWorldCreated;

	// Token: 0x04001ED2 RID: 7890
	private Action<MonoBehaviour, EventArgs> m_onHealthChange;

	// Token: 0x04001ED3 RID: 7891
	private Action<MonoBehaviour, EventArgs> m_onExhaustChange;

	// Token: 0x04001ED4 RID: 7892
	private Action<MonoBehaviour, EventArgs> m_onMaxHealthChange;

	// Token: 0x04001ED5 RID: 7893
	private Action<MonoBehaviour, EventArgs> m_onManaChange;

	// Token: 0x04001ED6 RID: 7894
	private Action<MonoBehaviour, EventArgs> m_onResolveChange;

	// Token: 0x04001ED7 RID: 7895
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpen;

	// Token: 0x04001ED8 RID: 7896
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;

	// Token: 0x04001ED9 RID: 7897
	private Action<MonoBehaviour, EventArgs> m_onSkillLevelChanged;

	// Token: 0x04001EDA RID: 7898
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04001EDB RID: 7899
	public static bool IgnoreHealthChangeEvents;

	// Token: 0x04001EDC RID: 7900
	private Tween m_shieldTween;

	// Token: 0x04001EDD RID: 7901
	private int m_storedCurrentArmor;
}
