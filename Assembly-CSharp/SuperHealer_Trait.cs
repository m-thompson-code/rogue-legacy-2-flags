using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class SuperHealer_Trait : BaseTrait
{
	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x0600209C RID: 8348 RVA: 0x00066C46 File Offset: 0x00064E46
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SuperHealer;
		}
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x00066C4D File Offset: 0x00064E4D
	protected override void Awake()
	{
		base.Awake();
		this.m_playerController = PlayerManager.GetPlayerController();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		this.m_onHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnHealthChange);
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x00066C84 File Offset: 0x00064E84
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x00066C9E File Offset: 0x00064E9E
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x00066CB8 File Offset: 0x00064EB8
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		if (this.m_playerController.TakesNoDamage)
		{
			return;
		}
		if ((args as CharacterHitEventArgs).DamageTaken <= 0f)
		{
			return;
		}
		if (PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom().BiomeType == BiomeType.HubTown)
		{
			return;
		}
		SaveManager.PlayerSaveData.TemporaryMaxHealthMods -= 0.0625f;
		this.m_playerController.InitializeHealthMods();
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x00066D20 File Offset: 0x00064F20
	private void OnHealthChange(object sender, EventArgs args)
	{
		float num = (float)this.m_playerController.ActualMaxHealth;
		if (num != this.m_storedActualMaxHealth)
		{
			this.m_storedActualMaxHealth = num;
			base.StopAllCoroutines();
			base.StartCoroutine(this.DelayHealthRegen());
		}
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x00066D5D File Offset: 0x00064F5D
	private IEnumerator DelayHealthRegen()
	{
		float delay = Time.time + 2.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		while (this.m_playerController.CurrentHealthAsInt < this.m_playerController.ActualMaxHealth)
		{
			int num = Mathf.CeilToInt(0.05f * (float)this.m_playerController.ActualMaxHealth);
			this.m_playerController.SetHealth((float)num, true, true);
			string text = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num);
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.HPGained, text, this.m_playerController, true, true);
			float regenTick = Time.time + 0.25f;
			while (Time.time < regenTick)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001C63 RID: 7267
	private PlayerController m_playerController;

	// Token: 0x04001C64 RID: 7268
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x04001C65 RID: 7269
	private Action<MonoBehaviour, EventArgs> m_onHealthChange;

	// Token: 0x04001C66 RID: 7270
	private float m_storedActualMaxHealth;
}
