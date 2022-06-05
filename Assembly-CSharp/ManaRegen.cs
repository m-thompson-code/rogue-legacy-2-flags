using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class ManaRegen : MonoBehaviour
{
	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x06002244 RID: 8772 RVA: 0x000124D5 File Offset: 0x000106D5
	// (set) Token: 0x06002245 RID: 8773 RVA: 0x000124DD File Offset: 0x000106DD
	public bool IsManaRegenDelayed { get; private set; }

	// Token: 0x17000F0F RID: 3855
	// (get) Token: 0x06002246 RID: 8774 RVA: 0x000124E6 File Offset: 0x000106E6
	private ManaRegenType RegenType
	{
		get
		{
			return this.m_playerController.CharacterClass.ClassData.PassiveData.ManaRegenType;
		}
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000AA2FC File Offset: 0x000A84FC
	private void Awake()
	{
		this.m_playerController = base.GetComponent<PlayerController>();
		this.m_regenDelayWaitYield = new WaitRL_Yield(0f, false);
		this.m_onForceRegen = new Action<MonoBehaviour, EventArgs>(this.OnForceRegen);
		this.m_onPlayerHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHealthChange);
		this.m_onPlayerBlocked = new Action<MonoBehaviour, EventArgs>(this.OnPlayerBlocked);
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		this.m_onEnemyHit = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHit);
		this.m_onEnemyKilled = new Action<MonoBehaviour, EventArgs>(this.OnEnemyKilled);
		this.m_onItemCollected = new Action<MonoBehaviour, EventArgs>(this.OnItemCollected);
		this.m_onBreakableDestroyed = new Action<MonoBehaviour, EventArgs>(this.OnBreakableDestroyed);
		this.m_onManaChange = new Action<ManaChangeEventArgs>(this.OnManaChange);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000AA3C8 File Offset: 0x000A85C8
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerForceManaRegen, this.m_onForceRegen);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_onPlayerHealthChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerBlocked, this.m_onPlayerBlocked);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHit, this.m_onEnemyHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ItemCollected, this.m_onItemCollected);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BreakableDestroyed, this.m_onBreakableDestroyed);
		if (this.m_playerController && this.m_playerController.ManaChangeRelay != null)
		{
			this.m_playerController.ManaChangeRelay.AddListener(this.m_onManaChange, false);
		}
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000AA46C File Offset: 0x000A866C
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerForceManaRegen, this.m_onForceRegen);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onPlayerHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerBlocked, this.m_onPlayerBlocked);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ItemCollected, this.m_onItemCollected);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BreakableDestroyed, this.m_onBreakableDestroyed);
		if (this.m_playerController && this.m_playerController.ManaChangeRelay != null)
		{
			this.m_playerController.ManaChangeRelay.RemoveListener(this.m_onManaChange);
		}
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000AA510 File Offset: 0x000A8710
	private void OnForceRegen(object sender, EventArgs args)
	{
		ForceManaRegenEventArgs forceManaRegenEventArgs = args as ForceManaRegenEventArgs;
		this.RegenPlayerManaFlat((int)forceManaRegenEventArgs.RegenAmount, forceManaRegenEventArgs.UsePlayerRegenMods);
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x00012502 File Offset: 0x00010702
	private void OnManaChange(ManaChangeEventArgs args)
	{
		if (args.NewManaValue < args.PrevManaValue)
		{
			base.StartCoroutine(this.DelayManaRegen());
		}
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x0001251F File Offset: 0x0001071F
	private IEnumerator DelayManaRegen()
	{
		this.IsManaRegenDelayed = true;
		this.m_regenDelayWaitYield.CreateNew(2f, false);
		yield return this.m_regenDelayWaitYield;
		this.IsManaRegenDelayed = false;
		yield break;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000AA538 File Offset: 0x000A8738
	private void OnPlayerHealthChange(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnPlayerHit)
		{
			HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
			if (healthChangeEventArgs.NewHealthValue < healthChangeEventArgs.PrevHealthValue)
			{
				float num = EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ManaChargeInjury);
				this.RegenPlayerMana(0.1f + num, true);
			}
		}
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000AA57C File Offset: 0x000A877C
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		if (TraitManager.IsTraitActive(TraitType.ManaFromHurt))
		{
			CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
			this.RegenPlayerManaFlat((int)((float)PlayerManager.GetPlayerController().ActualMaxMana * 0.5f), false);
			EffectManager.PlayEffect(characterHitEventArgs.Attacker.gameObject, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x0001252E File Offset: 0x0001072E
	private void OnPlayerBlocked(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnPlayerBlock)
		{
			this.RegenPlayerMana(0.1f, true);
		}
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000AA5D8 File Offset: 0x000A87D8
	private void OnEnemyHit(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType != ManaRegenType.OnEnemyHit)
		{
			return;
		}
		if (TraitManager.IsTraitActive(TraitType.ManaFromHurt))
		{
			return;
		}
		CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
		EnemyController enemyController = characterHitEventArgs.Victim as EnemyController;
		if (!enemyController)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		bool flag = SaveManager.PlayerSaveData.CurrentCharacter.ClassType == ClassType.AstroClass;
		if (characterHitEventArgs.Attacker.gameObject.CompareTag("PlayerProjectile"))
		{
			if (enemyController.DisableHPMPBonuses || enemyController.TakesNoDamage)
			{
				enemyController.StatusEffectController.StopStatusEffect(StatusEffectType.Enemy_ManaBurn, true);
				return;
			}
			if (!this.m_enemyOnHitManaWasAdded)
			{
				this.m_enemyOnHitManaWasAdded = true;
				num += (int)(characterHitEventArgs.Attacker as Projectile_RL).ManaGainPerHit;
			}
			if (flag)
			{
				num2++;
			}
		}
		else if (flag && characterHitEventArgs.Attacker.gameObject.CompareTag("EnemyStatusEffect"))
		{
			num2++;
		}
		Projectile_RL projectile_RL = characterHitEventArgs.Attacker as Projectile_RL;
		bool flag2 = projectile_RL && projectile_RL.CastAbilityType == CastAbilityType.Talent;
		if (num + num2 > 0 && this.RegenPlayerManaFlat(num, !flag2) + this.RegenPlayerManaFlat(num2, false) > 0)
		{
			EffectManager.PlayEffect(enemyController.gameObject, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000AA718 File Offset: 0x000A8918
	private void OnEnemyKilled(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnEnemyKilled)
		{
			if (args != null)
			{
				EnemyDeathEventArgs enemyDeathEventArgs = args as EnemyDeathEventArgs;
				if (enemyDeathEventArgs != null && enemyDeathEventArgs.Victim.DisableHPMPBonuses)
				{
					return;
				}
			}
			this.RegenPlayerMana(0.05f, true);
		}
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x00012545 File Offset: 0x00010745
	private void OnItemCollected(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnGoldCollected && Economy_EV.GetItemDropValue((args as ItemCollectedEventArgs).Item.ItemDropType, false) > 0)
		{
			this.RegenPlayerMana(0.1f, true);
		}
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x00012575 File Offset: 0x00010775
	private void OnBreakableDestroyed(MonoBehaviour sender, EventArgs args)
	{
		if (TraitManager.IsTraitActive(TraitType.BreakPropsForMana))
		{
			this.RegenPlayerMana(0.01f, true);
		}
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000AA758 File Offset: 0x000A8958
	private void Update()
	{
		if (!TraitManager.IsTraitActive(TraitType.BonusMagicStrength))
		{
			return;
		}
		float num = (float)this.m_playerController.ActualMaxMana;
		float currentMana = this.m_playerController.CurrentMana;
		float num2 = (float)Mathf.CeilToInt(1f * num);
		if (currentMana < num2 && !this.IsManaRegenDelayed)
		{
			float num3 = (float)Mathf.CeilToInt(0.75f * num * Time.deltaTime);
			if (num3 + currentMana > num2)
			{
				this.m_playerController.SetMana(num2, false, true, false);
				return;
			}
			this.m_playerController.SetMana(num3, true, true, false);
		}
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000AA7E0 File Offset: 0x000A89E0
	private void RegenPlayerMana(float regenPercent, bool applyManaRegenMods)
	{
		float num = regenPercent * (float)this.m_playerController.ActualMaxMana;
		if (applyManaRegenMods)
		{
			num *= 1f + this.m_playerController.ManaRegenMod;
		}
		this.m_manaToAddThisUpdate += num;
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x0001258F File Offset: 0x0001078F
	private int RegenPlayerManaFlat(int regenFlat, bool applyManaRegenMods)
	{
		if (applyManaRegenMods)
		{
			regenFlat = Mathf.CeilToInt((float)regenFlat * (1f + this.m_playerController.ManaRegenMod));
		}
		this.m_manaToAddThisUpdate += (float)regenFlat;
		return regenFlat;
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000AA824 File Offset: 0x000A8A24
	private void LateUpdate()
	{
		if (this.m_playerController)
		{
			if (this.m_manaToAddThisUpdate > 0f)
			{
				bool flag = TraitManager.IsTraitActive(TraitType.NoManaCap);
				if (this.m_playerController.CurrentManaAsInt < this.m_playerController.ActualMaxMana || flag)
				{
					this.m_manaToAddThisUpdate = (float)Mathf.CeilToInt(this.m_manaToAddThisUpdate);
					this.m_playerController.SetMana(this.m_manaToAddThisUpdate, true, true, false);
					string text;
					if (this.m_playerController.CurrentManaAsInt >= this.m_playerController.ActualMaxMana && !flag)
					{
						text = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MAX_MANA_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					}
					else
					{
						text = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)this.m_manaToAddThisUpdate);
					}
					TextPopupManager.DisplayTextDefaultPos(TextPopupType.MPGained, text, this.m_playerController, true, true);
				}
				this.m_manaToAddThisUpdate = 0f;
			}
			this.m_enemyOnHitManaWasAdded = false;
		}
	}

	// Token: 0x04001EF4 RID: 7924
	private PlayerController m_playerController;

	// Token: 0x04001EF5 RID: 7925
	private WaitRL_Yield m_regenDelayWaitYield;

	// Token: 0x04001EF6 RID: 7926
	private float m_manaToAddThisUpdate;

	// Token: 0x04001EF7 RID: 7927
	private bool m_enemyOnHitManaWasAdded;

	// Token: 0x04001EF8 RID: 7928
	private Action<MonoBehaviour, EventArgs> m_onForceRegen;

	// Token: 0x04001EF9 RID: 7929
	private Action<MonoBehaviour, EventArgs> m_onPlayerHealthChange;

	// Token: 0x04001EFA RID: 7930
	private Action<MonoBehaviour, EventArgs> m_onPlayerBlocked;

	// Token: 0x04001EFB RID: 7931
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x04001EFC RID: 7932
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;

	// Token: 0x04001EFD RID: 7933
	private Action<MonoBehaviour, EventArgs> m_onEnemyKilled;

	// Token: 0x04001EFE RID: 7934
	private Action<MonoBehaviour, EventArgs> m_onItemCollected;

	// Token: 0x04001EFF RID: 7935
	private Action<MonoBehaviour, EventArgs> m_onBreakableDestroyed;

	// Token: 0x04001F00 RID: 7936
	private Action<ManaChangeEventArgs> m_onManaChange;
}
