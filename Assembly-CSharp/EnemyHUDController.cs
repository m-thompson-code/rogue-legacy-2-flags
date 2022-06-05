using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005F6 RID: 1526
public class EnemyHUDController : MonoBehaviour
{
	// Token: 0x06002F00 RID: 12032 RVA: 0x00019BB9 File Offset: 0x00017DB9
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

	// Token: 0x06002F01 RID: 12033 RVA: 0x000C8FB0 File Offset: 0x000C71B0
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

	// Token: 0x06002F02 RID: 12034 RVA: 0x000C9070 File Offset: 0x000C7270
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHealthChange, this.m_onEnemyHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Opened, this.m_onPause);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PauseWindow_Closed, this.m_onUnpause);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.HideEnemyHUD, this.m_onPlayerDeath);
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x00019BE3 File Offset: 0x00017DE3
	private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
	{
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000C90CC File Offset: 0x000C72CC
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

	// Token: 0x06002F05 RID: 12037 RVA: 0x000C9230 File Offset: 0x000C7430
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

	// Token: 0x06002F06 RID: 12038 RVA: 0x000C92A8 File Offset: 0x000C74A8
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

	// Token: 0x06002F07 RID: 12039 RVA: 0x00019BF7 File Offset: 0x00017DF7
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

	// Token: 0x06002F08 RID: 12040 RVA: 0x00019C14 File Offset: 0x00017E14
	private IEnumerator TimedDisplayCoroutine()
	{
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x00019C23 File Offset: 0x00017E23
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

	// Token: 0x06002F0A RID: 12042 RVA: 0x00019C32 File Offset: 0x00017E32
	private void OnPause(object sender, EventArgs eventArgs)
	{
		if (this.m_canvasGroup)
		{
			this.m_storedHUDAlpha = this.m_canvasGroup.alpha;
			this.m_canvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x00019C62 File Offset: 0x00017E62
	private void OnUnpause(object sender, EventArgs eventArgs)
	{
		if (this.m_canvasGroup)
		{
			this.m_canvasGroup.alpha = this.m_storedHUDAlpha;
		}
	}

	// Token: 0x04002669 RID: 9833
	public static string RegularEnemyLocIDOverride = "";

	// Token: 0x0400266A RID: 9834
	public static string MinibossEnemyLocIDOverride = "";

	// Token: 0x0400266B RID: 9835
	public static string BossEnemyLocIDOverride = "";

	// Token: 0x0400266C RID: 9836
	private const float DISPLAY_DURATION = 1.5f;

	// Token: 0x0400266D RID: 9837
	private const float HP_CHANGE_DURATION = 0.1f;

	// Token: 0x0400266E RID: 9838
	private const int NUM_BLINKS = 4;

	// Token: 0x0400266F RID: 9839
	private const float BLINK_DURATION = 0.15f;

	// Token: 0x04002670 RID: 9840
	[SerializeField]
	private EnemyHUDType m_hudType;

	// Token: 0x04002671 RID: 9841
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04002672 RID: 9842
	[SerializeField]
	private Image m_hpBar;

	// Token: 0x04002673 RID: 9843
	[SerializeField]
	private Image m_hpLossBar;

	// Token: 0x04002674 RID: 9844
	[SerializeField]
	private Image m_hpGainBar;

	// Token: 0x04002675 RID: 9845
	[SerializeField]
	private TMP_Text m_enemyNameText;

	// Token: 0x04002676 RID: 9846
	[SerializeField]
	private TMP_Text m_commanderText;

	// Token: 0x04002677 RID: 9847
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04002678 RID: 9848
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002679 RID: 9849
	private float m_storedHUDAlpha;

	// Token: 0x0400267A RID: 9850
	private Action<MonoBehaviour, EventArgs> m_onEnemyHealthChange;

	// Token: 0x0400267B RID: 9851
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x0400267C RID: 9852
	private Action<MonoBehaviour, EventArgs> m_onPause;

	// Token: 0x0400267D RID: 9853
	private Action<MonoBehaviour, EventArgs> m_onUnpause;
}
