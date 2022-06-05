using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
public class SuperHealer_Trait : BaseTrait
{
	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x06002E35 RID: 11829 RVA: 0x0001949A File Offset: 0x0001769A
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SuperHealer;
		}
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x000194A1 File Offset: 0x000176A1
	protected override void Awake()
	{
		base.Awake();
		this.m_playerController = PlayerManager.GetPlayerController();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		this.m_onHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnHealthChange);
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x000194D8 File Offset: 0x000176D8
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x000194F2 File Offset: 0x000176F2
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onHealthChange);
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x000C7A80 File Offset: 0x000C5C80
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

	// Token: 0x06002E3A RID: 11834 RVA: 0x000C7AE8 File Offset: 0x000C5CE8
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

	// Token: 0x06002E3B RID: 11835 RVA: 0x0001950C File Offset: 0x0001770C
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

	// Token: 0x040025FD RID: 9725
	private PlayerController m_playerController;

	// Token: 0x040025FE RID: 9726
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x040025FF RID: 9727
	private Action<MonoBehaviour, EventArgs> m_onHealthChange;

	// Token: 0x04002600 RID: 9728
	private float m_storedActualMaxHealth;
}
