using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000375 RID: 885
public class EnemyHUDController : MonoBehaviour
{
	// Token: 0x0600212B RID: 8491 RVA: 0x0006821D File Offset: 0x0006641D
	private static string GetGlobalLocIDOverride(EnemyHUDType hudType)
	{
		if (hudType == EnemyHUDType.Regular)
		{
			return EnemyHUDController.RegularEnemyLocIDOverride;
		}
		if (hudType == EnemyHUDType.Miniboss)
		{
			return EnemyHUDController.MinibossEnemyLocIDOverride;
		}
		if (hudType != EnemyHUDType.Boss)
		{
			return "";
		}
		return EnemyHUDController.BossEnemyLocIDOverride;
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x00068248 File Offset: 0x00066448
	private void Awake()
	{
		this.m_onEnemyHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHealthChange);
		this.m_onPause = new Action<MonoBehaviour, EventArgs>(this.OnPause);
		this.m_onUnpause = new Action<MonoBehaviour, EventArgs>(this.OnUnpause);
		this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHealthChange, this.m_onEnemyHealthChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PauseWindow_Opened, this.m_onPause);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PauseWindow_Closed, this.m_onUnpause);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.HideEnemyHUD, this.m_onPlayerDeath);
		this.m_waitYield = new WaitRL_Yield(1.5f, false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x00068308 File Offset: 0x00066508
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHealthChange, this.m_onEnemyHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Opened, this.m_onPause);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Closed, this.m_onUnpause);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.HideEnemyHUD, this.m_onPlayerDeath);
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x00068362 File Offset: 0x00066562
	private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
	{
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x00068378 File Offset: 0x00066578
	private void OnEnemyHealthChange(MonoBehaviour sender, EventArgs args)
	{
		if (TraitManager.IsTraitActive(TraitType.NoEnemyHealthBar))
		{
			base.StopAllCoroutines();
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.transform.parent.gameObject.activeInHierarchy)
		{
			return;
		}
		EnemyController enemyController = sender as EnemyController;
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs.NewHealthValue == healthChangeEventArgs.PrevHealthValue)
		{
			return;
		}
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		this.m_hpGainBar.gameObject.SetActive(false);
		this.m_hpLossBar.gameObject.SetActive(false);
		if (!false)
		{
			if (this.m_hudType == EnemyHUDType.Boss && !enemyController.IsBoss)
			{
				return;
			}
			if (this.m_hudType != EnemyHUDType.Boss && enemyController.IsBoss)
			{
				return;
			}
			if (!enemyController.IsBoss)
			{
				if (enemyController.EnemyRank == EnemyRank.Miniboss && this.m_hudType != EnemyHUDType.Miniboss)
				{
					return;
				}
				if (enemyController.EnemyRank != EnemyRank.Miniboss && this.m_hudType == EnemyHUDType.Miniboss)
				{
					return;
				}
			}
			string commanderText = null;
			if (enemyController.IsCommander)
			{
				commanderText = this.GetCommanderText(enemyController);
			}
			float newPercent = healthChangeEventArgs.NewHealthValue / (float)enemyController.ActualMaxHealth;
			float prevPercent = healthChangeEventArgs.PrevHealthValue / (float)enemyController.ActualMaxHealth;
			EnemyData enemyData = EnemyClassLibrary.GetEnemyClassData(enemyController.EnemyType).GetEnemyData(enemyController.EnemyRank);
			this.DisplayHealthBar(newPercent, prevPercent, enemyController.Level, enemyData.Title, enemyController.CurrentHealth <= 0f, commanderText);
		}
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000684DC File Offset: 0x000666DC
	private string GetCommanderText(EnemyController enemy)
	{
		string text = LocalizationManager.GetString("LOC_ID_INDEX_COMMANDER_BUFFS_COMMANDER_TITLE_1", false, false);
		foreach (StatusEffectType statusEffectType in StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY)
		{
			string locID;
			if (enemy.HasCommanderStatusEffect(statusEffectType) && StatusEffect_EV.STATUS_EFFECT_TITLE_LOC_IDS.TryGetValue(statusEffectType, out locID))
			{
				text = text + ", " + LocalizationManager.GetString(locID, false, false);
			}
		}
		return text.Replace(text, "(" + text + ")");
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x00068554 File Offset: 0x00066754
	private void DisplayHealthBar(float newPercent, float prevPercent, int enemyLevel, string locID, bool isKilled, string commanderText)
	{
		enemyLevel = Mathf.FloorToInt((float)enemyLevel * 2.5f);
		string globalLocIDOverride = EnemyHUDController.GetGlobalLocIDOverride(this.m_hudType);
		if (!string.IsNullOrEmpty(globalLocIDOverride))
		{
			locID = globalLocIDOverride;
		}
		base.gameObject.SetActive(true);
		this.m_canvasGroup.alpha = 1f;
		this.m_hpBar.fillAmount = newPercent;
		this.m_levelText.text = string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_CHARACTER_LEVEL_1", false, false), enemyLevel);
		this.m_enemyNameText.text = LocalizationManager.GetString(locID, false, false);
		if (!string.IsNullOrEmpty(commanderText))
		{
			if (!this.m_commanderText.gameObject.activeSelf)
			{
				this.m_commanderText.gameObject.SetActive(true);
			}
			this.m_commanderText.text = commanderText;
		}
		else if (this.m_commanderText.gameObject.activeSelf)
		{
			this.m_commanderText.gameObject.SetActive(false);
		}
		if (!isKilled)
		{
			base.StartCoroutine(this.HPGainLossAnimCoroutine(newPercent, prevPercent));
			base.StartCoroutine(this.TimedDisplayCoroutine());
			return;
		}
		base.StartCoroutine(this.BlinkDisplayCoroutine());
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x00068671 File Offset: 0x00066871
	private IEnumerator HPGainLossAnimCoroutine(float newFill, float prevFill)
	{
		if (prevFill > newFill)
		{
			this.m_hpLossBar.gameObject.SetActive(true);
			this.m_hpBar.fillAmount = newFill;
			this.m_hpLossBar.fillAmount = prevFill;
		}
		else
		{
			this.m_hpGainBar.gameObject.SetActive(true);
			this.m_hpBar.fillAmount = prevFill;
			this.m_hpGainBar.fillAmount = newFill;
		}
		float delay = Time.time + 0.1f;
		float fillAmount = newFill - prevFill;
		while (Time.time < delay)
		{
			yield return null;
		}
		delay = Time.time + 0.1f;
		float startTime = Time.time;
		while (Time.time < delay)
		{
			float num = (Time.time - startTime) / 0.1f;
			if (prevFill > newFill)
			{
				this.m_hpLossBar.fillAmount = prevFill + fillAmount * num;
			}
			else
			{
				this.m_hpBar.fillAmount = prevFill + fillAmount * num;
			}
			yield return null;
		}
		if (prevFill > newFill)
		{
			this.m_hpLossBar.fillAmount = newFill;
		}
		else
		{
			this.m_hpBar.fillAmount = newFill;
		}
		yield break;
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x0006868E File Offset: 0x0006688E
	private IEnumerator TimedDisplayCoroutine()
	{
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x0006869D File Offset: 0x0006689D
	private IEnumerator BlinkDisplayCoroutine()
	{
		int num;
		for (int counter = 0; counter < 4; counter = num + 1)
		{
			this.m_canvasGroup.alpha = 0.25f;
			this.m_waitYield.CreateNew(0.075f, false);
			yield return this.m_waitYield;
			this.m_canvasGroup.alpha = 1f;
			this.m_waitYield.CreateNew(0.15f, false);
			yield return this.m_waitYield;
			num = counter;
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000686AC File Offset: 0x000668AC
	private void OnPause(object sender, EventArgs eventArgs)
	{
		if (this.m_canvasGroup)
		{
			this.m_storedHUDAlpha = this.m_canvasGroup.alpha;
			this.m_canvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000686DC File Offset: 0x000668DC
	private void OnUnpause(object sender, EventArgs eventArgs)
	{
		if (this.m_canvasGroup)
		{
			this.m_canvasGroup.alpha = this.m_storedHUDAlpha;
		}
	}

	// Token: 0x04001CAE RID: 7342
	public static string RegularEnemyLocIDOverride = "";

	// Token: 0x04001CAF RID: 7343
	public static string MinibossEnemyLocIDOverride = "";

	// Token: 0x04001CB0 RID: 7344
	public static string BossEnemyLocIDOverride = "";

	// Token: 0x04001CB1 RID: 7345
	private const float DISPLAY_DURATION = 1.5f;

	// Token: 0x04001CB2 RID: 7346
	private const float HP_CHANGE_DURATION = 0.1f;

	// Token: 0x04001CB3 RID: 7347
	private const int NUM_BLINKS = 4;

	// Token: 0x04001CB4 RID: 7348
	private const float BLINK_DURATION = 0.15f;

	// Token: 0x04001CB5 RID: 7349
	[SerializeField]
	private EnemyHUDType m_hudType;

	// Token: 0x04001CB6 RID: 7350
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04001CB7 RID: 7351
	[SerializeField]
	private Image m_hpBar;

	// Token: 0x04001CB8 RID: 7352
	[SerializeField]
	private Image m_hpLossBar;

	// Token: 0x04001CB9 RID: 7353
	[SerializeField]
	private Image m_hpGainBar;

	// Token: 0x04001CBA RID: 7354
	[SerializeField]
	private TMP_Text m_enemyNameText;

	// Token: 0x04001CBB RID: 7355
	[SerializeField]
	private TMP_Text m_commanderText;

	// Token: 0x04001CBC RID: 7356
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001CBD RID: 7357
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001CBE RID: 7358
	private float m_storedHUDAlpha;

	// Token: 0x04001CBF RID: 7359
	private Action<MonoBehaviour, EventArgs> m_onEnemyHealthChange;

	// Token: 0x04001CC0 RID: 7360
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x04001CC1 RID: 7361
	private Action<MonoBehaviour, EventArgs> m_onPause;

	// Token: 0x04001CC2 RID: 7362
	private Action<MonoBehaviour, EventArgs> m_onUnpause;
}
