using System;
using System.Collections;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class PlayerHitResponse : BaseCharacterHitResponse
{
	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06001B40 RID: 6976 RVA: 0x0005643A File Offset: 0x0005463A
	public override bool TakesDamageWhileStunned
	{
		get
		{
			return TraitManager.IsTraitActive(TraitType.NoImmunityWindow) || base.TakesDamageWhileStunned;
		}
	}

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00056450 File Offset: 0x00054650
	public override bool TriggerInvincibilityAfterStun
	{
		get
		{
			return !TraitManager.IsTraitActive(TraitType.NoImmunityWindow) && base.TriggerInvincibilityAfterStun;
		}
	}

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x06001B42 RID: 6978 RVA: 0x00056466 File Offset: 0x00054666
	public override bool IsInvincible
	{
		get
		{
			return base.IsInvincible || !RewiredMapController.IsMapEnabled(GameInputMode.Game);
		}
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06001B43 RID: 6979 RVA: 0x0005647C File Offset: 0x0005467C
	public bool CanRecoverFromStun
	{
		get
		{
			return Time.time > this.m_stunStartTime + 0.15f;
		}
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x00056491 File Offset: 0x00054691
	protected override void Awake()
	{
		base.Awake();
		this.m_playerBlockedEventArgs = new PlayerBlockedEventArgs(null);
		this.m_playerController = (this.m_charController as PlayerController);
		this.m_challengeDeathYield = new WaitRL_Yield(1.5f, true);
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x000564C8 File Offset: 0x000546C8
	public override void StartHitResponse(GameObject otherRootGameObj, IDamageObj damageObj, float damageOverride = -1f, bool trueDamage = false, bool fireEvents = true)
	{
		if (this.m_playerController.IsSpearSpinning)
		{
			Projectile_RL projectile_RL = damageObj as Projectile_RL;
			if (projectile_RL && projectile_RL.CompareTag("EnemyProjectile") && (projectile_RL.CollisionFlags & ProjectileCollisionFlag.ReflectWeak) != ProjectileCollisionFlag.None)
			{
				return;
			}
		}
		base.StartHitResponse(otherRootGameObj, damageObj, damageOverride, trueDamage, fireEvents);
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x00056518 File Offset: 0x00054718
	protected override void HandleHitResponse(IHitboxController otherHBController)
	{
		if (this.m_playerController.CharacterDownStrike.IsTriggeringBounce)
		{
			return;
		}
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0 && this.m_playerController.CharacterDash.IsDashing)
		{
			if (CollisionType_RL.IsProjectile(otherHBController.RootGameObject))
			{
				Projectile_RL component = otherHBController.RootGameObject.GetComponent<Projectile_RL>();
				if (component && (component.CollisionFlags & ProjectileCollisionFlag.VoidDashable) != ProjectileCollisionFlag.None)
				{
					return;
				}
			}
			else if (otherHBController.RootGameObject.CompareTag("Breakable"))
			{
				if (otherHBController.RootGameObject.GetComponent<Breakable_VoidDashOnly>())
				{
					return;
				}
			}
			else if (otherHBController.RootGameObject.CompareTag("Enemy") && otherHBController.RootGameObject.GetComponent<EnemyVoidDashable>())
			{
				return;
			}
		}
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.NoSpikeDamage).Level > 0 && otherHBController.RootGameObject.CompareTag("Hazard") && otherHBController.RootGameObject.GetComponent<TallSpike_Hazard>())
		{
			return;
		}
		if (((SaveManager.PlayerSaveData.EnableHouseRules && SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage) || this.m_playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_NoContactDamage)) && otherHBController.RootGameObject.CompareTag("Enemy"))
		{
			EnemyController component2 = otherHBController.RootGameObject.GetComponent<EnemyController>();
			if (component2 && !component2.AlwaysDealsContactDamage && !component2.AttackingWithContactDamage)
			{
				otherHBController.RemoveFromRepeatHitChecks(this.m_playerController.gameObject);
				return;
			}
		}
		base.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x0005668C File Offset: 0x0005488C
	protected override float CharacterDamaged(IDamageObj damageObj, GameObject otherRootObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false)
	{
		this.m_blinkOnHit = true;
		bool flag = false;
		float num = base.CharacterDamaged(damageObj, otherRootObj, out critType, out damageBlocked, damageOverride, trueDamage);
		if (num > 0f)
		{
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.BonusDamageOnNextHit);
			if (relic.Level > 0)
			{
				relic.SetIntValue(0, false, true);
			}
			RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.DamageNoHitChallenge);
			if (relic2.Level > 0 && (!SaveManager.PlayerSaveData.InHubTown || ChallengeManager.IsInChallenge))
			{
				SaveManager.PlayerSaveData.GetRelic(RelicType.DamageNoHitChallengeUsed).SetLevel(relic2.Level, true, true);
				relic2.SetLevel(0, false, true);
				flag = true;
			}
			RelicObj relic3 = SaveManager.PlayerSaveData.GetRelic(RelicType.NoAttackDamageBonus);
			if (relic3.Level > 0)
			{
				relic3.SetIntValue(0, false, true);
				this.m_playerController.StartNoAttackDamageBonusTimer();
			}
			RelicObj relic4 = SaveManager.PlayerSaveData.GetRelic(RelicType.MagicDamageEnemyCount);
			if (relic4.Level > 0)
			{
				relic4.SetIntValue(0, false, true);
			}
		}
		if (this.m_playerController.CurrentHealth <= 0f)
		{
			return num;
		}
		if (num > 0f && TraitManager.IsTraitActive(TraitType.DisarmOnHurt))
		{
			this.m_playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_Disarmed, 2f, null);
		}
		if (num > 0f && ChallengeManager.IsInChallenge)
		{
			ChallengeManager.HitsTaken++;
		}
		if (this.m_playerController.IsBlocking)
		{
			this.m_playerBlockedEventArgs.Initialize(damageObj);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerBlocked, this, this.m_playerBlockedEventArgs);
			if (Time.time < this.m_playerController.BlockStartTime + 0.135f)
			{
				base.BlinkPulseEffect.StartSingleBlinkEffect(Color.yellow);
			}
			else
			{
				base.BlinkPulseEffect.StartSingleBlinkEffect(Color.white);
			}
			this.m_blinkOnHit = false;
		}
		Vector2 vector = this.m_playerController.Midpoint;
		vector.y += this.m_playerController.CollisionBounds.height / 2f;
		string text = "";
		if (TraitManager.IsTraitActive(TraitType.NoHealthBar))
		{
			text = "???";
		}
		else if (TraitManager.IsTraitActive(TraitType.FakeSelfDamage))
		{
			if (num > 0f || damageBlocked > 0f)
			{
				float num2 = UnityEngine.Random.Range(Trait_EV.FAKE_SELF_DAMAGE_DAMAGE_TAKEN_MOD.x, Trait_EV.FAKE_SELF_DAMAGE_DAMAGE_TAKEN_MOD.y);
				text = ((int)(-num * num2)).ToString();
			}
		}
		else if (num > 0f || damageBlocked > 0f)
		{
			string text2 = "";
			if (num > 0f)
			{
				text2 += (-num).ToString();
			}
			if (damageBlocked > 0f)
			{
				if (string.IsNullOrEmpty(text2))
				{
					text2 = text2 + "<size=75%><color=grey>(" + (-damageBlocked).ToString() + ")</color></size>";
				}
				else
				{
					text2 = text2 + " <size=75%><color=grey>(" + (-damageBlocked).ToString() + ")</color></size>";
				}
			}
			text = text2;
		}
		foreach (string str in BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER)
		{
			if (text.Length <= 0)
			{
				text = text + "[" + str + "]";
			}
			else
			{
				text = text + " [" + str + "]";
			}
		}
		TextPopupType popupType = TextPopupType.PlayerHit;
		if (num > 0f)
		{
			if (critType == CriticalStrikeType.Regular)
			{
				popupType = TextPopupType.PlayerHit_Critical;
			}
			else if (critType == CriticalStrikeType.Guaranteed)
			{
				popupType = TextPopupType.PlayerHit_GuaranteedCritical;
			}
			else if (critType == CriticalStrikeType.Super)
			{
				popupType = TextPopupType.PlayerHit_SuperCritical;
			}
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			TextPopupManager.DisplayTextAtAbsPos(popupType, text, vector, null, TextAlignmentOptions.Center);
		}
		if (num > 0f)
		{
			if (flag)
			{
				TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_RELIC_UI_RELIC_BROKEN_1", StringGenderType.Male, vector + Vector2.up * 1.5f, 0f);
			}
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.ManaRestoreOnHurt).Level;
			if (level > 0)
			{
				float num3 = (float)(this.m_playerController.ActualMaxMana * 2);
				float num4 = Mathf.Clamp((float)(100 * level), 0f, num3 - (float)this.m_playerController.CurrentManaAsInt);
				float num5 = Mathf.Min(this.m_playerController.CurrentMana + num4, num3);
				num5 = (float)Mathf.CeilToInt(num5);
				this.m_playerController.SetMana(num5, false, true, true);
				string text3;
				if (num4 <= 0f)
				{
					text3 = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MAX_MANA_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				}
				else
				{
					text3 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num4);
				}
				text3 += "  [ManaRestoreOnHurt]";
				TextPopupManager.DisplayText(TextPopupType.MPGained, text3, this.m_playerController.gameObject, vector + Vector2.down, true, true);
			}
		}
		if (!damageObj.IsDotDamage && !this.m_playerController.IsBlocking)
		{
			EffectManager.SetEffectParams("SlowTime_Effect", new object[]
			{
				"m_timeScaleValue",
				0.1f
			});
			EffectManager.PlayEffect(base.gameObject, this.m_playerController.Animator, "SlowTime_Effect", Vector3.zero, 0.5f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		if (!this.m_playerController.IsBlocking)
		{
			BaseEffect baseEffect = EffectManager.PlayEffect(CameraController.ForegroundOrthoCam.gameObject, null, "ScreenDamageEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			baseEffect.transform.SetParent(CameraController.ForegroundOrthoCam.transform, false);
			Vector3 localScale = Vector3.one * CameraController.ZoomLevel * 2f;
			localScale.x *= AspectRatioManager.CurrentGameAspectRatio / 1.7777778f;
			baseEffect.transform.localScale = localScale;
			baseEffect.DisableDestroyOnRoomChange = true;
		}
		if (otherRootObj.CompareTag("Enemy"))
		{
			EnemyController component = otherRootObj.GetComponent<EnemyController>();
			if (component)
			{
				if (num > 0f)
				{
					float num6 = 0f;
					int num7 = RuneManager.GetRuneEquippedLevel(RuneType.ReturnDamage);
					num7 += Mathf.RoundToInt(EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.ReturnDamage));
					num6 += RuneLogicHelper.GetDamageReturnPercent(num7) * this.m_playerController.GetActualStatValue(PlayerStat.Vitality);
					if (num6 > 0f)
					{
						if (TraitManager.IsTraitActive(TraitType.SkillCritsOnly))
						{
							num6 = 0f;
						}
						component.CharacterHitResponse.StartHitResponse(this.m_charController.gameObject, this.m_playerController, num6, false, true);
					}
				}
				float num8 = 0f;
				bool flag2 = false;
				if (component.IsSummoned)
				{
					EnemyController enemyController = component.Summoner as EnemyController;
					if (enemyController && enemyController.EnemyType == EnemyType.CaveBoss)
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					num8 += num * BurdenManager.GetBurdenStatGain(BurdenType.EnemyLifesteal);
					if (num8 > 0f)
					{
						int num9 = Mathf.CeilToInt(num8);
						if (num9 > 0 && component.CurrentHealthAsInt > 0)
						{
							vector = component.Midpoint;
							vector.y += component.CollisionBounds.height / 2f;
							string text4 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", false, false), num9);
							TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, text4, vector, null, TextAlignmentOptions.Center);
							component.SetHealth((float)num9, true, true);
							((BurstEffect)EffectManager.PlayEffect(this.m_playerController.gameObject, null, "LifestealBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None)).DestinationOverride = component.transform;
						}
					}
				}
				if (component.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Disarm))
				{
					this.m_playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_Disarmed, 0f, damageObj);
				}
			}
		}
		if (this.m_playerController.IsMushroomBig)
		{
			this.m_playerController.SetMushroomBig(false, true);
		}
		return num;
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x00056E58 File Offset: 0x00055058
	protected override void CharacterStunned(IDamageObj damageObj, GameObject otherRootObj)
	{
		this.m_playerController.StopActiveAbilities(false);
		this.m_stunStartTime = Time.time;
		base.CharacterStunned(damageObj, otherRootObj);
		if (TraitManager.IsTraitActive(TraitType.NoImmunityWindow))
		{
			base.SetInvincibleTime(0.1f, true, false);
		}
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x00056E94 File Offset: 0x00055094
	protected override void CharacterKnockedBack(IDamageObj damageObj, GameObject otherRootObj)
	{
		if (this.m_playerController.CurrentlyInRoom != null)
		{
			float num = 1f;
			if (TraitManager.IsTraitActive(TraitType.FakeSelfDamage))
			{
				num = 2.5f;
			}
			if (!SaveManager.ConfigData.DisableSlowdownOnHit)
			{
				EffectManager.SetEffectParams("CameraShake_Effect_Template", new object[]
				{
					"m_shakeSpeed",
					10f * num,
					"m_shakeAmplitude",
					10.5f * num
				});
			}
			else
			{
				EffectManager.SetEffectParams("CameraShake_Effect_Template", new object[]
				{
					"m_shakeSpeed",
					5f * num,
					"m_shakeAmplitude",
					5.5f * num
				});
			}
			EffectManager.PlayEffect(this.m_playerController.CurrentlyInRoom.gameObject, null, "CameraShake_Effect_Template", Vector3.zero, 0.3f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		Vector2 vector = Player_EV.PLAYER_BASE_KNOCKBACK_DISTANCE;
		Vector2 b = (damageObj == null) ? Vector2.one : damageObj.ExternalKnockbackMod;
		Vector2 internalKnockbackMod = this.m_playerController.InternalKnockbackMod;
		vector *= internalKnockbackMod * b;
		if (TraitManager.IsTraitActive(TraitType.PlayerKnockedLow))
		{
			vector *= Trait_EV.PLAYER_KNOCKED_LOW_KNOCKBACK_MOD;
		}
		else if (TraitManager.IsTraitActive(TraitType.PlayerKnockedFar))
		{
			vector *= Trait_EV.PLAYER_KNOCKED_FAR_KNOCKBACK_MOD;
		}
		if (vector != Vector2.zero)
		{
			this.PerformKnockbackPush(otherRootObj, vector);
		}
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x00056FF4 File Offset: 0x000551F4
	protected override void CharacterKilled(IDamageObj damageObj, GameObject otherRootObj)
	{
		DeathDefiedType deathDefiedType = SkillTreeLogicHelper.IsDeathDefied();
		TextPopupManager.DisableAllTextPopups();
		switch (deathDefiedType)
		{
		case DeathDefiedType.Death_Dodge:
		case DeathDefiedType.ExtraLife:
		case DeathDefiedType.ExtraLife_Unity:
			foreach (AnimatorControllerParameter animatorControllerParameter in this.m_playerController.Animator.parameters)
			{
				AnimatorControllerParameterType type = animatorControllerParameter.type;
				if (type != AnimatorControllerParameterType.Bool)
				{
					if (type == AnimatorControllerParameterType.Trigger)
					{
						this.m_playerController.Animator.ResetTrigger(animatorControllerParameter.name);
					}
				}
				else
				{
					this.m_playerController.Animator.SetBool(animatorControllerParameter.name, false);
				}
			}
			this.m_playerController.StopActiveAbilities(true);
			WindowManager.SetWindowIsOpen(WindowID.DeathDefy, true);
			(WindowManager.GetWindowController(WindowID.DeathDefy) as DeathDefyWindowController).DeathDefiedType = deathDefiedType;
			this.CharacterStunned(damageObj, otherRootObj);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerFakedDeath, null, null);
			if (this.m_playerController.UseOverrideDefaultTint && this.m_playerController.DefaultTintOverrideColor == Color.black && !TraitManager.IsTraitActive(TraitType.SmallHitbox))
			{
				base.StartCoroutine(this.FakeDeathTintCoroutine());
				return;
			}
			break;
		case DeathDefiedType.FailedChallenge:
			this.m_playerController.StopActiveAbilities(true);
			this.m_playerController.ControllerCorgi.GravityActive(false);
			this.m_playerController.ControllerCorgi.SetForce(Vector2.zero);
			base.SetInvincibleTime(9999f, false, false);
			RewiredMapController.SetCurrentMapEnabled(false);
			base.StartCoroutine(this.ChallengeDeathSlowdownCoroutine());
			return;
		default:
			if (this.m_playerController.UseOverrideDefaultTint && this.m_playerController.DefaultTintOverrideColor == Color.black && !TraitManager.IsTraitActive(TraitType.SmallHitbox))
			{
				EffectManager.SetEffectParams("RemoveTint_Effect", new object[]
				{
					"m_useUnscaledTime",
					true
				});
				EffectManager.PlayEffect(this.m_playerController.gameObject, this.m_playerController.Animator, "RemoveTint_Effect", Vector3.zero, 0.25f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			base.CharacterKilled(damageObj, otherRootObj);
			break;
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000571E7 File Offset: 0x000553E7
	private IEnumerator FakeDeathTintCoroutine()
	{
		EffectManager.SetEffectParams("RemoveTint_Effect", new object[]
		{
			"m_useUnscaledTime",
			true
		});
		EffectManager.PlayEffect(this.m_playerController.gameObject, this.m_playerController.Animator, "RemoveTint_Effect", Vector3.zero, 0.25f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		while (WindowManager.GetIsWindowOpen(WindowID.DeathDefy))
		{
			yield return null;
		}
		EffectManager.SetEffectParams("AddBlackTint_Effect", new object[]
		{
			"m_useUnscaledTime",
			true
		});
		EffectManager.PlayEffect(this.m_playerController.gameObject, this.m_playerController.Animator, "AddBlackTint_Effect", Vector3.zero, 0.25f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		yield break;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000571F6 File Offset: 0x000553F6
	private IEnumerator ChallengeDeathSlowdownCoroutine()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 0.25f);
		yield return this.m_challengeDeathYield;
		RLTimeScale.Reset();
		ChallengeManager.ReturnToDriftHouseWithTransition();
		yield break;
	}

	// Token: 0x04001902 RID: 6402
	private PlayerController m_playerController;

	// Token: 0x04001903 RID: 6403
	private PlayerBlockedEventArgs m_playerBlockedEventArgs;

	// Token: 0x04001904 RID: 6404
	private float m_stunStartTime;

	// Token: 0x04001905 RID: 6405
	private WaitRL_Yield m_challengeDeathYield;
}
