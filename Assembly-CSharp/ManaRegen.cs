using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class ManaRegen : MonoBehaviour
{
	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x0600186D RID: 6253 RVA: 0x0004C586 File Offset: 0x0004A786
	// (set) Token: 0x0600186E RID: 6254 RVA: 0x0004C58E File Offset: 0x0004A78E
	public bool IsManaRegenDelayed { get; private set; }

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x0600186F RID: 6255 RVA: 0x0004C597 File Offset: 0x0004A797
	private ManaRegenType RegenType
	{
		get
		{
			return this.m_playerController.CharacterClass.ClassData.PassiveData.ManaRegenType;
		}
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0004C5B4 File Offset: 0x0004A7B4
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

	// Token: 0x06001871 RID: 6257 RVA: 0x0004C680 File Offset: 0x0004A880
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

	// Token: 0x06001872 RID: 6258 RVA: 0x0004C724 File Offset: 0x0004A924
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

	// Token: 0x06001873 RID: 6259 RVA: 0x0004C7C8 File Offset: 0x0004A9C8
	private void OnForceRegen(object sender, EventArgs args)
	{
		ForceManaRegenEventArgs forceManaRegenEventArgs = args as ForceManaRegenEventArgs;
		this.RegenPlayerManaFlat((int)forceManaRegenEventArgs.RegenAmount, forceManaRegenEventArgs.UsePlayerRegenMods);
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0004C7F0 File Offset: 0x0004A9F0
	private void OnManaChange(ManaChangeEventArgs args)
	{
		if (args.NewManaValue < args.PrevManaValue)
		{
			base.StartCoroutine(this.DelayManaRegen());
		}
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x0004C80D File Offset: 0x0004AA0D
	private IEnumerator DelayManaRegen()
	{
		this.IsManaRegenDelayed = true;
		this.m_regenDelayWaitYield.CreateNew(2f, false);
		yield return this.m_regenDelayWaitYield;
		this.IsManaRegenDelayed = false;
		yield break;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0004C81C File Offset: 0x0004AA1C
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

	// Token: 0x06001877 RID: 6263 RVA: 0x0004C860 File Offset: 0x0004AA60
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		if (TraitManager.IsTraitActive(TraitType.ManaFromHurt))
		{
			CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
			this.RegenPlayerManaFlat((int)((float)PlayerManager.GetPlayerController().ActualMaxMana * 0.5f), false);
			EffectManager.PlayEffect(characterHitEventArgs.Attacker.gameObject, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x0004C8BB File Offset: 0x0004AABB
	private void OnPlayerBlocked(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnPlayerBlock)
		{
			this.RegenPlayerMana(0.1f, true);
		}
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x0004C8D4 File Offset: 0x0004AAD4
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

	// Token: 0x0600187A RID: 6266 RVA: 0x0004CA14 File Offset: 0x0004AC14
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

	// Token: 0x0600187B RID: 6267 RVA: 0x0004CA51 File Offset: 0x0004AC51
	private void OnItemCollected(MonoBehaviour sender, EventArgs args)
	{
		if (this.RegenType == ManaRegenType.OnGoldCollected && Economy_EV.GetItemDropValue((args as ItemCollectedEventArgs).Item.ItemDropType, false) > 0)
		{
			this.RegenPlayerMana(0.1f, true);
		}
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x0004CA81 File Offset: 0x0004AC81
	private void OnBreakableDestroyed(MonoBehaviour sender, EventArgs args)
	{
		if (TraitManager.IsTraitActive(TraitType.BreakPropsForMana))
		{
			this.RegenPlayerMana(0.01f, true);
		}
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x0004CA9C File Offset: 0x0004AC9C
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

	// Token: 0x0600187E RID: 6270 RVA: 0x0004CB24 File Offset: 0x0004AD24
	private void RegenPlayerMana(float regenPercent, bool applyManaRegenMods)
	{
		float num = regenPercent * (float)this.m_playerController.ActualMaxMana;
		if (applyManaRegenMods)
		{
			num *= 1f + this.m_playerController.ManaRegenMod;
		}
		this.m_manaToAddThisUpdate += num;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x0004CB65 File Offset: 0x0004AD65
	private int RegenPlayerManaFlat(int regenFlat, bool applyManaRegenMods)
	{
		if (applyManaRegenMods)
		{
			regenFlat = Mathf.CeilToInt((float)regenFlat * (1f + this.m_playerController.ManaRegenMod));
		}
		this.m_manaToAddThisUpdate += (float)regenFlat;
		return regenFlat;
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0004CB98 File Offset: 0x0004AD98
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

	// Token: 0x040017C2 RID: 6082
	private PlayerController m_playerController;

	// Token: 0x040017C3 RID: 6083
	private WaitRL_Yield m_regenDelayWaitYield;

	// Token: 0x040017C4 RID: 6084
	private float m_manaToAddThisUpdate;

	// Token: 0x040017C5 RID: 6085
	private bool m_enemyOnHitManaWasAdded;

	// Token: 0x040017C6 RID: 6086
	private Action<MonoBehaviour, EventArgs> m_onForceRegen;

	// Token: 0x040017C7 RID: 6087
	private Action<MonoBehaviour, EventArgs> m_onPlayerHealthChange;

	// Token: 0x040017C8 RID: 6088
	private Action<MonoBehaviour, EventArgs> m_onPlayerBlocked;

	// Token: 0x040017C9 RID: 6089
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;

	// Token: 0x040017CA RID: 6090
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;

	// Token: 0x040017CB RID: 6091
	private Action<MonoBehaviour, EventArgs> m_onEnemyKilled;

	// Token: 0x040017CC RID: 6092
	private Action<MonoBehaviour, EventArgs> m_onItemCollected;

	// Token: 0x040017CD RID: 6093
	private Action<MonoBehaviour, EventArgs> m_onBreakableDestroyed;

	// Token: 0x040017CE RID: 6094
	private Action<ManaChangeEventArgs> m_onManaChange;
}
