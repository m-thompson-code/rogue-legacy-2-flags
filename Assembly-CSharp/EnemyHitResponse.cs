using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class EnemyHitResponse : BaseCharacterHitResponse
{
	// Token: 0x0600154E RID: 5454 RVA: 0x00041417 File Offset: 0x0003F617
	protected override void Awake()
	{
		base.Awake();
		this.m_enemyController = (this.m_charController as EnemyController);
		this.m_hasInteractable = (this.m_enemyController.GetComponent<Interactable>() != null);
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00041447 File Offset: 0x0003F647
	protected override void HandleHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController != null && otherHBController.CollisionType == CollisionType.Hazard && this.m_enemyController.IsFlying)
		{
			return;
		}
		if (otherHBController.CollisionType == CollisionType.Player && this.m_hasInteractable)
		{
			return;
		}
		base.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x00041480 File Offset: 0x0003F680
	protected override float CharacterDamaged(IDamageObj damageObj, GameObject otherRootObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false)
	{
		Projectile_RL projectile_RL = damageObj as Projectile_RL;
		if (damageObj.gameObject.CompareTag("PlayerProjectile") && this.m_enemyController.CanIncrementRelicHitCounter)
		{
			if (!this.m_enemyController.TakesNoDamage && damageObj.ActualDamage > 0f)
			{
				RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.BonusDamageOnNextHit);
				if (relic.Level > 0 && relic.IntValue < 6)
				{
					relic.SetIntValue(1, true, true);
				}
			}
			RelicObj relic2 = SaveManager.PlayerSaveData.GetRelic(RelicType.DamageAuraOnHit);
			if (relic2.Level > 0)
			{
				PlayerController playerController = PlayerManager.GetPlayerController();
				if (projectile_RL.CastAbilityType == CastAbilityType.Weapon && !(projectile_RL is DownstrikeProjectile_RL) && playerController.DamageAuraProjectile != projectile_RL)
				{
					if (!playerController.DamageAuraProjectile || !playerController.DamageAuraProjectile.isActiveAndEnabled)
					{
						playerController.DamageAuraProjectile = ProjectileManager.FireProjectile(playerController.gameObject, "RelicDamageAuraOnHitProjectile", new Vector2(0f, 1f), true, 0f, 1f, false, true, true, true);
						playerController.DamageAuraProjectile.CastAbilityType = CastAbilityType.Talent;
						playerController.DamageAuraProjectile.transform.SetParent(playerController.transform);
					}
					else
					{
						playerController.DamageAuraProjectile.LifespanTimer = playerController.DamageAuraProjectile.Lifespan;
					}
					playerController.DamageAuraProjectile.LifespanTimer += 1.5f * (float)(relic2.Level - 1);
				}
			}
		}
		float num = base.CharacterDamaged(damageObj, otherRootObj, out critType, out damageBlocked, damageOverride, trueDamage);
		if (num <= 0f)
		{
			return 0f;
		}
		Vector2 absPos = this.m_enemyController.Midpoint;
		absPos.y += this.m_enemyController.CollisionBounds.height / 2f;
		if (!TraitManager.IsTraitActive(TraitType.NoEnemyHealthBar))
		{
			string text = (-num).ToString();
			if ((damageObj.gameObject.CompareTag("Player") || damageObj.gameObject.CompareTag("PlayerProjectile") || damageObj.gameObject.CompareTag("EnemyStatusEffect")) && TraitManager.IsTraitActive(TraitType.FakeSelfDamage))
			{
				float num2 = UnityEngine.Random.Range(Trait_EV.FAKE_SELF_DAMAGE_DAMAGE_DEALT_MOD.x, Trait_EV.FAKE_SELF_DAMAGE_DAMAGE_DEALT_MOD.y);
				text = ((int)(-num * num2)).ToString();
			}
			foreach (string str in BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER)
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
			if (!string.IsNullOrEmpty(damageObj.RelicDamageTypeString))
			{
				text += damageObj.RelicDamageTypeString;
			}
			TextPopupType popupType = TextPopupType.EnemyHit;
			if (critType == CriticalStrikeType.Regular)
			{
				popupType = TextPopupType.EnemyHit_Critical;
			}
			else if (critType == CriticalStrikeType.Guaranteed)
			{
				popupType = TextPopupType.EnemyHit_GuaranteedCritical;
			}
			else if (critType == CriticalStrikeType.Super)
			{
				popupType = TextPopupType.EnemyHit_SuperCritical;
			}
			TextPopupManager.DisplayTextAtAbsPos(popupType, text, absPos, null, TextAlignmentOptions.Center);
		}
		if (this.m_enemyController.CurrentHealth > 0f)
		{
			if (Time.time > EnemyHitResponse.m_timeSlowdownCDTimer && (otherRootObj.CompareTag("Player") || otherRootObj.CompareTag("PlayerProjectile")))
			{
				EffectManager.SetEffectParams("SlowTime_Effect", new object[]
				{
					"m_timeScaleValue",
					0.5f
				});
				EffectManager.PlayEffect(base.gameObject, this.m_charController.Animator, "SlowTime_Effect", Vector3.zero, 0.065f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			EnemyHitResponse.m_timeSlowdownCDTimer = Time.time + 0.55f;
		}
		if (!damageObj.IsDotDamage)
		{
			base.PlayDirectionalHitEffect(damageObj, otherRootObj);
			EffectManager.PlayHitEffectCamShake(0.2f);
		}
		PlayerController playerController2 = PlayerManager.GetPlayerController();
		float num3 = 0f;
		if (!otherRootObj.CompareTag("Hazard"))
		{
			Projectile_RL component = damageObj.gameObject.GetComponent<Projectile_RL>();
			if (component)
			{
				if (component.CompareTag("PlayerProjectile"))
				{
					if (this.m_enemyController.CurrentHealth <= 0f && component.CastAbilityType == CastAbilityType.Talent && playerController2.CastAbility.TalentAbilityType == AbilityType.KnockoutTalent)
					{
						this.m_enemyController.KilledByKnockout = true;
					}
					bool flag = component is DownstrikeProjectile_RL;
					if (component.CastAbilityType == CastAbilityType.Weapon)
					{
						if (!flag)
						{
							if (this.m_enemyController.IsVulnerableToLifeSteal && TraitManager.IsTraitActive(TraitType.Vampire) && this.m_enemyController.EnemyType != EnemyType.Dummy && this.m_enemyController.EnemyType != EnemyType.Eggplant)
							{
								num3 += num * 0.2f;
							}
							RelicObj relic3 = SaveManager.PlayerSaveData.GetRelic(RelicType.OnHitAreaDamage);
							int relicMaxStack = Relic_EV.GetRelicMaxStack(relic3.RelicType, relic3.Level);
							if (relic3.Level > 0 && relic3.IntValue >= relicMaxStack)
							{
								Projectile_RL projectile_RL2 = ProjectileManager.FireProjectile(component.Owner, "RelicOnHitAreaDamageProjectile", this.m_enemyController.Midpoint, true, 0f, 1f, true, true, true, true);
								projectile_RL2.CastAbilityType = CastAbilityType.Talent;
								projectile_RL2.MagicScale += 0.75f * (float)(relic3.Level - 1);
								projectile_RL2.ActualCritDamage = ProjectileManager.CalculateProjectileCritDamage(projectile_RL2, true);
								projectile_RL2.RelicDamageTypeString = projectile_RL2.RelicDamageTypeString + "[" + RelicType.OnHitAreaDamage.ToString() + "]";
								if (this.m_enemyController.CurrentHealth <= 0f)
								{
									EffectManager.PlayEffect(damageObj.gameObject, null, "RelicOnHitAreaDamage_Chains_Effect", this.m_enemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
								}
								relic3.SetIntValue(0, false, true);
								playerController2.StartOnHitAreaDamageTimer();
							}
						}
						if (playerController2.CharacterClass.ClassType == ClassType.MagicWandClass && !this.m_enemyController.DisableHPMPBonuses && !this.m_enemyController.TakesNoDamage)
						{
							this.m_enemyController.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_ManaBurn, 0f, component);
						}
					}
				}
				this.m_enemyController.LastDamageTakenType = component.DamageType;
			}
		}
		if (this.m_enemyController.CurrentHealth <= 0f && !this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_InvulnWindow))
		{
			if (!this.m_enemyController.DisableXPBonuses && SkillTreeLogicHelper.IsTotemUnlocked() && !ChallengeManager.IsInChallenge && !Mastery_EV.IsMaxMasteryRank(SaveManager.PlayerSaveData.CurrentCharacter.ClassType, 0, true))
			{
				int level = this.m_enemyController.Level;
				int num4 = Mastery_EV.CalculateRankV2(SaveManager.PlayerSaveData.GetClassXP(playerController2.CharacterClass.ClassType) + SaveManager.PlayerSaveData.RunAccumulatedXP);
				num4 = Mathf.Clamp(num4, 0, Mastery_EV.GetMaxMasteryRank());
				int num5 = 50 + level * 2;
				num5 = Mathf.Min(num5, Mastery_EV.GetMaxBaseXP(num4));
				float num6 = 1f;
				if (!this.m_enemyController.IsBoss)
				{
					switch (this.m_enemyController.EnemyRank)
					{
					case EnemyRank.Advanced:
						num6 += 0.25f;
						break;
					case EnemyRank.Expert:
						num6 += 1.5f;
						break;
					case EnemyRank.Miniboss:
						num6 += 4f;
						break;
					}
				}
				else
				{
					num6 += 14f;
				}
				float num7 = (float)Mathf.FloorToInt((float)num5 * num6);
				num6 = 1f;
				num6 += SkillTreeLogicHelper.GetClassXPMod();
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.NoGoldXPBonus).Level > 0)
				{
					num6 += (1f + Economy_EV.GetGoldGainMod()) * NPC_EV.GetArchitectGoldMod(-1);
					num6 = Mathf.Min(3f, num6);
				}
				int num8 = Mathf.FloorToInt(num7 * num6);
				playerController2.UpdateFrameAccumulatedXP((float)num8);
				playerController2.SetMasteryXP(num8, true);
			}
			if (!damageObj.gameObject.CompareTag("Player"))
			{
				if (this.m_enemyController.IsVulnerableToLifeSteal)
				{
					if (critType != CriticalStrikeType.None)
					{
						RelicObj relic4 = SaveManager.PlayerSaveData.GetRelic(RelicType.CritKillsHeal);
						num3 += (float)playerController2.ActualMaxHealth * ((float)relic4.Level * 0.06f);
					}
					float num9 = RuneLogicHelper.GetLifeStealPercent() * playerController2.GetActualStatValue(PlayerStat.Strength);
					num9 = Mathf.Clamp(num9, (float)RuneLogicHelper.GetNumLifeStealRunes(), 999f);
					num3 += (float)Mathf.CeilToInt(num9);
					float num10 = RuneLogicHelper.GetSoulStealPercent() * playerController2.GetActualStatValue(PlayerStat.Magic);
					num10 = Mathf.Clamp(num10, (float)RuneLogicHelper.GetNumSoulStealRunes(), 999f);
					num3 += (float)Mathf.CeilToInt(num10);
				}
				if (!this.m_enemyController.DisableHPMPBonuses && !this.m_enemyController.IsBoss && this.m_enemyController.EnemyRank != EnemyRank.Miniboss)
				{
					RelicObj relic5 = SaveManager.PlayerSaveData.GetRelic(RelicType.SpellKillMaxMana);
					if (relic5.Level > 0 && playerController2.CurrentHealthAsInt >= playerController2.ActualMaxHealth)
					{
						int num11 = relic5.Level * 200;
						if (relic5.IntValue < num11)
						{
							relic5.SetIntValue(5, true, true);
							if (relic5.IntValue > num11)
							{
								relic5.SetIntValue(num11, false, true);
							}
							float num12 = (float)playerController2.ActualMaxMana;
							playerController2.InitializeManaMods();
							float num13 = (float)playerController2.ActualMaxMana - num12;
							playerController2.SetMana(num13, true, true, false);
							absPos = playerController2.Midpoint;
							absPos.y += playerController2.CollisionBounds.height / 2f;
							string @string = LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MAX_MANA_UP_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
							TextPopupManager.DisplayTextAtAbsPos(TextPopupType.MPGained, string.Format(@string, num13), absPos, null, TextAlignmentOptions.Center);
						}
					}
					RelicObj relic6 = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeEnemyKill);
					if (relic6.Level > 0)
					{
						int num14 = 6;
						num14 -= relic6.Level - 1;
						if (relic6.IntValue >= num14)
						{
							relic6.SetIntValue(0, false, true);
						}
						else
						{
							relic6.SetIntValue(1, true, true);
						}
					}
					RelicObj relic7 = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeHitRegenerate);
					if (relic7.Level > 0)
					{
						int num15 = 6;
						num15 -= relic7.Level - 1;
						if (relic7.IntValue < num15)
						{
							relic7.SetIntValue(1, true, true);
						}
					}
					RelicObj relic8 = SaveManager.PlayerSaveData.GetRelic(RelicType.MagicDamageEnemyCount);
					int num16 = Mathf.RoundToInt(5f) * relic8.Level;
					if (relic8.Level > 0 && relic8.IntValue < num16)
					{
						relic8.SetIntValue(1, true, true);
						playerController2.InitializeMagicMods();
					}
				}
			}
		}
		if (num3 > 0f && !TraitManager.IsTraitActive(TraitType.MegaHealth))
		{
			int num17 = Mathf.CeilToInt(num3);
			if (num17 > 0)
			{
				playerController2.UpdateFrameAccumulatedLifeSteal((float)num17);
				playerController2.SetHealth((float)num17, true, true);
				absPos = playerController2.Midpoint;
				absPos.y += playerController2.CollisionBounds.height / 2f;
				EffectManager.PlayEffect(this.m_enemyController.gameObject, this.m_enemyController.Animator, "LifestealBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
		}
		return num;
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x00041F98 File Offset: 0x00040198
	private IEnumerator ResetStunAnimState()
	{
		this.m_enemyController.Animator.SetBool(base.StunnedAnimParamName, true);
		if (this.m_stunWaitYield == null)
		{
			this.m_stunWaitYield = new WaitRL_Yield(0f, false);
		}
		this.m_stunWaitYield.CreateNew(0.15f, false);
		yield return this.m_stunWaitYield;
		this.m_enemyController.Animator.SetBool(base.StunnedAnimParamName, false);
		yield break;
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00041FA8 File Offset: 0x000401A8
	protected override void CharacterKnockedBack(IDamageObj damageObj, GameObject otherRootObj)
	{
		Vector2 b = (damageObj == null) ? Vector2.one : damageObj.ExternalKnockbackMod;
		Vector2 internalKnockbackMod = this.m_enemyController.InternalKnockbackMod;
		Vector2 vector = Enemy_EV.ENEMY_BASE_KNOCKBACK_DISTANCE;
		vector *= internalKnockbackMod * b;
		if (TraitManager.IsTraitActive(TraitType.EnemyKnockedLow))
		{
			vector *= Trait_EV.ENEMY_KNOCKED_LOW_KNOCKBACK_MOD;
		}
		else if (TraitManager.IsTraitActive(TraitType.EnemyKnockedFar))
		{
			vector *= Trait_EV.ENEMY_KNOCKED_FAR_KNOCKBACK_MOD;
		}
		if (vector != Vector2.zero)
		{
			if (this.m_enemyController.IsFlying)
			{
				Vector2 vector2 = base.gameObject.transform.position - otherRootObj.transform.position;
				vector2.Normalize();
				if (this.m_enemyController.GetComponent<BounceCollision>())
				{
					float magnitude = this.m_enemyController.Velocity.magnitude;
					this.m_enemyController.SetVelocity(vector2.x * magnitude, vector2.y * magnitude, false);
					return;
				}
				if (this.m_enemyController.FlyingMovementType == FlyingMovementType.Override)
				{
					this.m_enemyController.FlyingMovementType = FlyingMovementType.Stop;
				}
				float magnitude2 = vector.magnitude;
				this.m_enemyController.FlightKnockbackAcceleration = vector2 * magnitude2;
				return;
			}
			else
			{
				this.PerformKnockbackPush(otherRootObj, vector);
			}
		}
	}

	// Token: 0x0400148D RID: 5261
	private EnemyController m_enemyController;

	// Token: 0x0400148E RID: 5262
	private static float m_timeSlowdownCDTimer;

	// Token: 0x0400148F RID: 5263
	private bool m_hasInteractable;

	// Token: 0x04001490 RID: 5264
	private WaitRL_Yield m_stunWaitYield;
}
