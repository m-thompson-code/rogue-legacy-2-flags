using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000676 RID: 1654
public class PlayerHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x0600325C RID: 12892 RVA: 0x000D79EC File Offset: 0x000D5BEC
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

	// Token: 0x0600325D RID: 12893 RVA: 0x0001BA19 File Offset: 0x00019C19
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x000D7B08 File Offset: 0x000D5D08
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

	// Token: 0x0600325F RID: 12895 RVA: 0x0001BA2C File Offset: 0x00019C2C
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

	// Token: 0x06003260 RID: 12896 RVA: 0x000D7B6C File Offset: 0x000D5D6C
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

	// Token: 0x06003261 RID: 12897 RVA: 0x000D7C3C File Offset: 0x000D5E3C
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

	// Token: 0x06003262 RID: 12898 RVA: 0x000D7CFC File Offset: 0x000D5EFC
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

	// Token: 0x06003263 RID: 12899 RVA: 0x000D7E3C File Offset: 0x000D603C
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

	// Token: 0x06003264 RID: 12900 RVA: 0x000D7F28 File Offset: 0x000D6128
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

	// Token: 0x06003265 RID: 12901 RVA: 0x000D7FCC File Offset: 0x000D61CC
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

	// Token: 0x06003266 RID: 12902 RVA: 0x000D8048 File Offset: 0x000D6248
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

	// Token: 0x06003267 RID: 12903 RVA: 0x000D809C File Offset: 0x000D629C
	private void OnSkillLevelChanged(object sender, EventArgs eventArgs)
	{
		this.m_levelText.text = string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_CHARACTER_LEVEL_1", false, false), SkillTreeManager.GetTotalSkillObjLevel());
		Vector2 anchoredPosition = this.m_resolveTransform.anchoredPosition;
		anchoredPosition.x = this.m_levelText.preferredWidth;
		this.m_resolveTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x000D80FC File Offset: 0x000D62FC
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

	// Token: 0x06003269 RID: 12905 RVA: 0x000D8134 File Offset: 0x000D6334
	private void OnExhaustChange(object sender, EventArgs eventArgs)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.UpdateExhaust(playerController);
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x000D8150 File Offset: 0x000D6350
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

	// Token: 0x0600326B RID: 12907 RVA: 0x000D8220 File Offset: 0x000D6420
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

	// Token: 0x0600326C RID: 12908 RVA: 0x000D8350 File Offset: 0x000D6550
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

	// Token: 0x0600326D RID: 12909 RVA: 0x000D8594 File Offset: 0x000D6794
	private void OnManaChange(object sender, EventArgs eventArgs)
	{
		ManaChangeEventArgs manaChangeEventArgs = eventArgs as ManaChangeEventArgs;
		this.UpdateMana(manaChangeEventArgs.Player);
		this.UpdateSpellOrbs(PlayerManager.GetPlayerController());
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x0001BA3B File Offset: 0x00019C3B
	private void OnResolveChange(object sender, EventArgs eventArgs)
	{
		this.UpdateResolve(PlayerManager.GetPlayerController());
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x000D85C0 File Offset: 0x000D67C0
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

	// Token: 0x06003270 RID: 12912 RVA: 0x000D868C File Offset: 0x000D688C
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

	// Token: 0x06003271 RID: 12913 RVA: 0x000D880C File Offset: 0x000D6A0C
	private void UpdateSpellOrbs(PlayerController playerController)
	{
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000D881C File Offset: 0x000D6A1C
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

	// Token: 0x06003273 RID: 12915 RVA: 0x0001BA48 File Offset: 0x00019C48
	public void RefreshText(object sender, EventArgs args)
	{
		this.OnSkillLevelChanged(null, null);
	}

	// Token: 0x04002917 RID: 10519
	private const float MIN_HP_BAR_3_DIGITS_WIDTH = 320f;

	// Token: 0x04002918 RID: 10520
	private const float MIN_HP_BAR_2_DIGITS_WIDTH = 270f;

	// Token: 0x04002919 RID: 10521
	private const float MIN_MP_BAR_3_DIGITS_WIDTH = 280f;

	// Token: 0x0400291A RID: 10522
	private const float MIN_MP_BAR_2_DIGITS_WIDTH = 230f;

	// Token: 0x0400291B RID: 10523
	private const float MAX_HP_BAR_WIDTH = 1000f;

	// Token: 0x0400291C RID: 10524
	private const float MAX_MP_BAR_WIDTH = 800f;

	// Token: 0x0400291D RID: 10525
	private const float APPROXIMATE_MAX_HP = 1000f;

	// Token: 0x0400291E RID: 10526
	private const float APPROXIMATE_MAX_MP = 500f;

	// Token: 0x0400291F RID: 10527
	private const string HUD_FADE_TWEEN_ID = "HUDFadeTween";

	// Token: 0x04002920 RID: 10528
	[SerializeField]
	private CanvasGroup m_hudCanvasGroup;

	// Token: 0x04002921 RID: 10529
	[SerializeField]
	private CanvasGroup m_minimapCanvasGroup;

	// Token: 0x04002922 RID: 10530
	[SerializeField]
	private GameObject m_minimapFrameGO;

	// Token: 0x04002923 RID: 10531
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04002924 RID: 10532
	[SerializeField]
	private RectTransform m_hpBarPanelRectTransform;

	// Token: 0x04002925 RID: 10533
	[SerializeField]
	private Image m_hpBar;

	// Token: 0x04002926 RID: 10534
	[SerializeField]
	private RectTransform m_hpBarRectTransform;

	// Token: 0x04002927 RID: 10535
	[SerializeField]
	private Image m_hpExhaustBar;

	// Token: 0x04002928 RID: 10536
	[SerializeField]
	private RectTransform m_hpExhaustBarRectTransform;

	// Token: 0x04002929 RID: 10537
	[SerializeField]
	private TMP_Text m_hpText;

	// Token: 0x0400292A RID: 10538
	[SerializeField]
	private RectTransform m_mpBarRectTransform;

	// Token: 0x0400292B RID: 10539
	[SerializeField]
	private Image m_mpBar;

	// Token: 0x0400292C RID: 10540
	[SerializeField]
	private TMP_Text m_mpText;

	// Token: 0x0400292D RID: 10541
	[SerializeField]
	private RectTransform m_shieldIcon;

	// Token: 0x0400292E RID: 10542
	[SerializeField]
	private TMP_Text m_shieldText;

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	private GameObject m_spellOrbIcon;

	// Token: 0x04002930 RID: 10544
	[SerializeField]
	private TMP_Text m_spellOrbText;

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	private TMP_Text m_resolveText;

	// Token: 0x04002932 RID: 10546
	[SerializeField]
	private RectTransform m_resolveTransform;

	// Token: 0x04002933 RID: 10547
	[SerializeField]
	private FadeOutHUDCollider m_fadeOutHUDCollider;

	// Token: 0x04002934 RID: 10548
	private Vector3 m_spellOrbStoredPos;

	// Token: 0x04002935 RID: 10549
	private float m_storedMaxHP;

	// Token: 0x04002936 RID: 10550
	private float m_storedMaxMP;

	// Token: 0x04002937 RID: 10551
	private Tween m_playerHUDAlphaTween;

	// Token: 0x04002938 RID: 10552
	private Tween m_minimapHUDAlphaTween;

	// Token: 0x04002939 RID: 10553
	private TextPopupObj m_maxHealthTextPopup;

	// Token: 0x0400293A RID: 10554
	private int m_lastMaxHealthChange;

	// Token: 0x0400293B RID: 10555
	private float m_storedFadeOutHUDAlpha = 1f;

	// Token: 0x0400293C RID: 10556
	private float m_storedFadeOutMinimapHUDAlpha = 1f;

	// Token: 0x0400293D RID: 10557
	private BoxCollider2D m_fadeOutHPCollider;

	// Token: 0x0400293E RID: 10558
	private Action<MonoBehaviour, EventArgs> m_tweenInHUD;

	// Token: 0x0400293F RID: 10559
	private Action<MonoBehaviour, EventArgs> m_tweenOutHUD;

	// Token: 0x04002940 RID: 10560
	private Action<MonoBehaviour, EventArgs> m_onWorldCreated;

	// Token: 0x04002941 RID: 10561
	private Action<MonoBehaviour, EventArgs> m_onHealthChange;

	// Token: 0x04002942 RID: 10562
	private Action<MonoBehaviour, EventArgs> m_onExhaustChange;

	// Token: 0x04002943 RID: 10563
	private Action<MonoBehaviour, EventArgs> m_onMaxHealthChange;

	// Token: 0x04002944 RID: 10564
	private Action<MonoBehaviour, EventArgs> m_onManaChange;

	// Token: 0x04002945 RID: 10565
	private Action<MonoBehaviour, EventArgs> m_onResolveChange;

	// Token: 0x04002946 RID: 10566
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpen;

	// Token: 0x04002947 RID: 10567
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;

	// Token: 0x04002948 RID: 10568
	private Action<MonoBehaviour, EventArgs> m_onSkillLevelChanged;

	// Token: 0x04002949 RID: 10569
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400294A RID: 10570
	public static bool IgnoreHealthChangeEvents;

	// Token: 0x0400294B RID: 10571
	private Tween m_shieldTween;

	// Token: 0x0400294C RID: 10572
	private int m_storedCurrentArmor;
}
