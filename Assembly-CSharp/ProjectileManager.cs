using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006A3 RID: 1699
public class ProjectileManager : MonoBehaviour
{
	// Token: 0x17001562 RID: 5474
	// (get) Token: 0x06003E20 RID: 15904 RVA: 0x000D97DC File Offset: 0x000D79DC
	public static bool IsInitialized
	{
		get
		{
			return ProjectileManager.m_isInitialized;
		}
	}

	// Token: 0x17001563 RID: 5475
	// (get) Token: 0x06003E21 RID: 15905 RVA: 0x000D97E3 File Offset: 0x000D79E3
	public static ProjectileManager Instance
	{
		get
		{
			if (!ProjectileManager.m_projectileManager)
			{
				ProjectileManager.m_projectileManager = CDGHelper.FindStaticInstance<ProjectileManager>(false);
			}
			return ProjectileManager.m_projectileManager;
		}
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x000D9804 File Offset: 0x000D7A04
	private void Awake()
	{
		this.m_createAbilityPool = new Action<MonoBehaviour, EventArgs>(this.CreateAbilityPool);
		this.m_disableExitRoomProjectiles = new Action<MonoBehaviour, EventArgs>(ProjectileManager.DisableExitRoomProjectiles);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChangeAbility, this.m_createAbilityPool);
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x000D9859 File Offset: 0x000D7A59
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (ProjectileManager.IsInitialized)
		{
			ProjectileManager.DisableAllProjectiles(false);
		}
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x000D9868 File Offset: 0x000D7A68
	private void OnDestroy()
	{
		if (!GameManager.IsApplicationClosing)
		{
			ProjectileManager.DestroyBiomePools();
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChangeAbility, this.m_createAbilityPool);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_disableExitRoomProjectiles);
			ProjectileManager.m_isInitialized = false;
			ProjectileManager.m_projectileManager = null;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x000D98B8 File Offset: 0x000D7AB8
	private void Initialize()
	{
		this.m_projectileDict = new Dictionary<string, GenericPool_RL<Projectile_RL>>(StringComparer.OrdinalIgnoreCase);
		this.m_offscreenIconPool = new GenericPool_RL<OffscreenIconObj>();
		this.m_offscreenIconPool.Initialize(this.m_offscreenIconPrefab, 50, false, true);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_disableExitRoomProjectiles);
		ProjectileManager.m_isInitialized = true;
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x000D9908 File Offset: 0x000D7B08
	public void CreateEnemyProjectilePools(List<EnemyController> enemyPrefabList)
	{
		foreach (EnemyController enemyController in enemyPrefabList)
		{
			BaseAIScript aiscript = EnemyClassLibrary.GetEnemyClassData(enemyController.EnemyType).GetAIScript(enemyController.EnemyRank);
			if (aiscript && aiscript.ProjectileNameArray != null)
			{
				foreach (string projectileName in aiscript.ProjectileNameArray)
				{
					this.QueueProjectileToPool(projectileName);
				}
			}
		}
		foreach (BaseStatusEffect baseStatusEffect in EnemyLibrary.GetEnemyPrefab(EnemyType.Skeleton, EnemyRank.Basic).GetComponentInChildren<StatusEffectController>().GetComponents<BaseStatusEffect>())
		{
			if (baseStatusEffect.ProjectileNameArray != null)
			{
				foreach (string projectileName2 in baseStatusEffect.ProjectileNameArray)
				{
					this.QueueProjectileToPool(projectileName2);
				}
			}
		}
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x000D99FC File Offset: 0x000D7BFC
	private void CreateAbilityPool(MonoBehaviour sender, EventArgs args)
	{
		ChangeAbilityEventArgs changeAbilityEventArgs = args as ChangeAbilityEventArgs;
		if (changeAbilityEventArgs.Ability != null)
		{
			BaseAbility_RL ability = AbilityLibrary.GetAbility(changeAbilityEventArgs.Ability.AbilityType);
			if (ability && ability.ProjectileNameArray != null)
			{
				foreach (string projectileName in ability.ProjectileNameArray)
				{
					this.AddProjectileToPool(projectileName);
				}
			}
		}
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x000D9A5C File Offset: 0x000D7C5C
	private void Internal_CreateBiomePools(BiomeType biomeType)
	{
		BiomeController biomeController;
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		else
		{
			biomeController = WorldBuilder.GetBiomeController(biomeType);
		}
		if (biomeController)
		{
			foreach (IHasProjectileNameArray hasProjectileNameArray in PlayerManager.GetPlayerController().GetComponentsInChildren<IHasProjectileNameArray>(true))
			{
				if (hasProjectileNameArray != null && hasProjectileNameArray.ProjectileNameArray != null)
				{
					foreach (string projectileName in hasProjectileNameArray.ProjectileNameArray)
					{
						this.QueueProjectileToPool(projectileName);
					}
				}
			}
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				ISpawnController[] spawnControllers = baseRoom.SpawnControllerManager.SpawnControllers;
				for (int i = 0; i < spawnControllers.Length; i++)
				{
					IHasProjectileNameArray hasProjectileNameArray2 = spawnControllers[i] as IHasProjectileNameArray;
					if (hasProjectileNameArray2 != null && hasProjectileNameArray2.ProjectileNameArray != null)
					{
						foreach (string projectileName2 in hasProjectileNameArray2.ProjectileNameArray)
						{
							this.QueueProjectileToPool(projectileName2);
						}
					}
				}
			}
			if (TraitManager.IsTraitActive(TraitType.ExplosiveChests))
			{
				this.QueueProjectileToPool("ExplosiveChestsPotionProjectile");
				this.QueueProjectileToPool("ExplosiveChestsPotionExplosionProjectile");
			}
			if (TraitManager.IsTraitActive(TraitType.ExplosiveEnemies))
			{
				this.QueueProjectileToPool("ExplosiveEnemiesPotionProjectile");
				this.QueueProjectileToPool("ExplosiveEnemiesPotionExplosionProjectile");
			}
		}
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x000D9BCC File Offset: 0x000D7DCC
	public void AddProjectileToPool(string projectileName)
	{
		if (!string.IsNullOrEmpty(projectileName) && !this.m_projectileDict.ContainsKey(projectileName))
		{
			ProjectileEntry projectileEntry = ProjectileLibrary.GetProjectileEntry(projectileName);
			if (projectileEntry != null)
			{
				this.m_projectileDict.Add(projectileName, this.CreatePool(projectileEntry.ProjectilePrefab, projectileEntry.PoolSize));
				foreach (IHasProjectileNameArray hasProjectileNameArray in projectileEntry.ProjectilePrefab.GetComponentsInChildren<IHasProjectileNameArray>())
				{
					if (hasProjectileNameArray.ProjectileNameArray != null)
					{
						foreach (string projectileName2 in hasProjectileNameArray.ProjectileNameArray)
						{
							this.AddProjectileToPool(projectileName2);
						}
					}
				}
			}
		}
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x000D9C6A File Offset: 0x000D7E6A
	public static void ClearProjectileQueue()
	{
		ProjectileManager.m_queueProjectileSet.Clear();
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x000D9C78 File Offset: 0x000D7E78
	private void QueueProjectileToPool(string projectileName)
	{
		if (!string.IsNullOrEmpty(projectileName) && !ProjectileManager.m_queueProjectileSet.Contains(projectileName))
		{
			ProjectileEntry projectileEntry = ProjectileLibrary.GetProjectileEntry(projectileName);
			if (projectileEntry != null)
			{
				ProjectileManager.m_queueProjectileSet.Add(projectileName);
				foreach (IHasProjectileNameArray hasProjectileNameArray in projectileEntry.ProjectilePrefab.GetComponentsInChildren<IHasProjectileNameArray>())
				{
					if (hasProjectileNameArray.ProjectileNameArray != null)
					{
						foreach (string projectileName2 in hasProjectileNameArray.ProjectileNameArray)
						{
							this.QueueProjectileToPool(projectileName2);
						}
					}
				}
			}
		}
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x000D9D00 File Offset: 0x000D7F00
	public static void CreatePoolsFromQueuedProjectiles()
	{
		ProjectileManager.Instance.CreatePoolsFromQueuedProjectiles_Internal();
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x000D9D0C File Offset: 0x000D7F0C
	private void CreatePoolsFromQueuedProjectiles_Internal()
	{
		List<string> list = this.m_projectileDict.Keys.ToList<string>();
		int num = 0;
		int num2 = 0;
		foreach (string text in ProjectileManager.m_queueProjectileSet)
		{
			ProjectileEntry projectileEntry = ProjectileLibrary.GetProjectileEntry(text);
			if (projectileEntry != null)
			{
				list.Remove(text);
				if (!this.m_projectileDict.ContainsKey(text))
				{
					num++;
					this.m_projectileDict.Add(text, this.CreatePool(projectileEntry.ProjectilePrefab, projectileEntry.PoolSize));
				}
				else
				{
					num2++;
				}
			}
		}
		foreach (string key in list)
		{
			if (this.m_projectileDict.ContainsKey(key))
			{
				this.m_projectileDict[key].DestroyPool();
				this.m_projectileDict.Remove(key);
			}
		}
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x000D9E24 File Offset: 0x000D8024
	private GenericPool_RL<Projectile_RL> CreatePool(Projectile_RL prefab, int poolSize)
	{
		if (!prefab)
		{
			return null;
		}
		GenericPool_RL<Projectile_RL> genericPool_RL = new GenericPool_RL<Projectile_RL>();
		genericPool_RL.Initialize(prefab, poolSize, false, true);
		return genericPool_RL;
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x000D9E40 File Offset: 0x000D8040
	private Projectile_RL FireProjectile_Internal(GameObject source, string projectileName, Vector2 positionOffset, bool matchFacing = true, float angleInDeg = 0f, float speedMod = 1f, bool useAbsPos = false, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		GenericPool_RL<Projectile_RL> genericPool_RL = null;
		if (!this.m_projectileDict.TryGetValue(projectileName, out genericPool_RL))
		{
			throw new Exception("Projectile: " + projectileName + " cannot be found in ProjectileManager.  Please make sure the prefab is added to the Projectile Library prefab. If pooling is on, ensure the projectile name is properly assigned to its respective ProjectileNameArray.");
		}
		BaseCharacterController component = source.GetRoot(false).GetComponent<BaseCharacterController>();
		Projectile_RL freeObj = genericPool_RL.GetFreeObj();
		if (freeObj.Animator)
		{
			freeObj.Animator.enabled = false;
		}
		freeObj.gameObject.SetActive(true);
		freeObj.MatchFacing = matchFacing;
		freeObj.Owner = source;
		float num = 1f;
		if (freeObj.CompareTag("EnemyProjectile") && !freeObj.IsWeighted)
		{
			speedMod += BurdenManager.GetBurdenStatGain(BurdenType.EnemyProjectiles);
			num += (float)BurdenManager.GetBurdenLevel(BurdenType.EnemyProjectiles) * 0.07000005f;
		}
		freeObj.Speed *= speedMod;
		freeObj.TurnSpeed *= num;
		if (freeObj.AudioEventEmitter)
		{
			freeObj.AudioEventEmitter.SetPlaySpawnAudio(playSpawnAudio);
			freeObj.AudioEventEmitter.SetPlayLifetimeAudio(playLifetimeAudio);
			freeObj.AudioEventEmitter.SetPlayDeathAudio(playDeathAudio);
		}
		if (component)
		{
			float d;
			if (freeObj.SnapToOwner)
			{
				d = 1f / component.BaseScaleToOffsetWith;
			}
			else
			{
				d = component.transform.localScale.x / component.BaseScaleToOffsetWith;
			}
			if (!useAbsPos)
			{
				positionOffset *= d;
			}
		}
		ProjectileManager.ApplyProjectileDamage(component, freeObj);
		bool flag = true;
		if (matchFacing && component)
		{
			flag = component.IsFacingRight;
		}
		if (freeObj.SnapToOwner && !useAbsPos)
		{
			if (component)
			{
				freeObj.transform.SetParent(component.Pivot.transform, false);
			}
			else
			{
				freeObj.transform.SetParent(source.transform, false);
			}
			freeObj.transform.localPosition = positionOffset;
			freeObj.transform.localEulerAngles = new Vector3(0f, 0f, angleInDeg);
			freeObj.RotationSpeed = -freeObj.RotationSpeed;
		}
		else
		{
			if (useAbsPos)
			{
				freeObj.transform.localPosition = positionOffset;
			}
			else
			{
				freeObj.transform.localPosition = ProjectileManager.DetermineSpawnPosition(source, positionOffset, flag);
			}
			if (!flag)
			{
				angleInDeg = 180f - angleInDeg;
				freeObj.Flip();
				freeObj.RotationSpeed = -freeObj.RotationSpeed;
			}
		}
		float num2 = 1f;
		if (freeObj.ScaleWithOwner)
		{
			if (component)
			{
				if (freeObj.SnapToOwner)
				{
					num2 = 1f / component.BaseScaleToOffsetWith;
				}
				else
				{
					num2 = source.transform.localScale.x / component.BaseScaleToOffsetWith;
				}
			}
			else if (!freeObj.SnapToOwner)
			{
				num2 = source.transform.localScale.x / 1f;
			}
		}
		else if (freeObj.SnapToOwner)
		{
			num2 = 1f / source.transform.localScale.x;
		}
		freeObj.transform.localScale = new Vector3(freeObj.transform.localScale.x * num2, freeObj.transform.localScale.y * num2, freeObj.transform.localScale.z);
		Vector2 heading = CDGHelper.AngleToVector((float)Mathf.RoundToInt(angleInDeg));
		freeObj.Heading = heading;
		if (freeObj.PivotFollowsOrientation)
		{
			float num3 = angleInDeg;
			if (freeObj.IsFlipped && !freeObj.SnapToOwner)
			{
				num3 = 180f + num3;
			}
			if (!freeObj.Pivot)
			{
				Vector3 localEulerAngles = freeObj.transform.localEulerAngles;
				localEulerAngles.z = num3;
				freeObj.transform.localEulerAngles = localEulerAngles;
			}
			else
			{
				Vector3 localEulerAngles2 = freeObj.Pivot.transform.localEulerAngles;
				localEulerAngles2.z = num3;
				freeObj.Pivot.transform.localEulerAngles = localEulerAngles2;
			}
		}
		freeObj.SetCorgiVelocity(freeObj.Heading * freeObj.Speed);
		freeObj.UpdateMovement();
		if (freeObj.Animator)
		{
			freeObj.Animator.enabled = true;
		}
		freeObj.Spawn();
		this.AddTraitEffects(freeObj);
		return freeObj;
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x000DA22C File Offset: 0x000D842C
	public static void ApplyProjectileDamage(BaseCharacterController characterController, Projectile_RL projectile)
	{
		projectile.MagicScale = projectile.ProjectileData.MagicScale;
		projectile.StrengthScale = projectile.ProjectileData.StrengthScale;
		if (!characterController)
		{
			return;
		}
		projectile.Magic = characterController.ActualMagic;
		projectile.Strength = characterController.ActualStrength;
		PlayerController playerController = characterController as PlayerController;
		if (!playerController)
		{
			projectile.ActualCritChance = characterController.ActualCritChance;
			projectile.ActualCritDamage = characterController.ActualCritDamage;
			return;
		}
		CastAbilityType lastCastAbilityTypeCasted = playerController.CastAbility.LastCastAbilityTypeCasted;
		bool flag = !playerController.CastAbility.GetAbility(lastCastAbilityTypeCasted, false).DealsNoDamage;
		if (projectile.DamageType == DamageType.Strength)
		{
			projectile.ActualCritChance = playerController.ActualCritChance;
			projectile.ActualCritDamage = ProjectileManager.CalculateProjectileCritDamage(projectile, false);
		}
		else
		{
			projectile.ActualCritChance = playerController.ActualMagicCritChance;
			projectile.ActualCritDamage = ProjectileManager.CalculateProjectileCritDamage(projectile, true);
		}
		if (projectile.DamageType == DamageType.Strength)
		{
			if (TraitManager.IsTraitActive(TraitType.BonusStrength))
			{
				projectile.DamageMod += 0.100000024f;
			}
			if (TraitManager.IsTraitActive(TraitType.BonusMagicStrength))
			{
				projectile.DamageMod += -0.5f;
			}
			if (TraitManager.IsTraitActive(TraitType.DamageBoost))
			{
				projectile.DamageMod += 0.5f;
			}
			AngryOnHit_Trait angryOnHit_Trait = TraitManager.GetActiveTrait(TraitType.AngryOnHit) as AngryOnHit_Trait;
			if (angryOnHit_Trait)
			{
				projectile.DamageMod += angryOnHit_Trait.StrengthMultiplier;
			}
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponSwap).Level * 0.07000005f;
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level * 1f;
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.LowResolveWeaponDamage).Level * 0.00999999f * (float)Mathf.Max(Mathf.RoundToInt((1f - playerController.ActualResolve) * 100f), 0);
			if (playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Suave))
			{
				int num = Mathf.RoundToInt(projectile.Magic * 0.15f);
				if (projectile.ActualDamage > 0f)
				{
					projectile.DamageMod += (float)num / projectile.ActualDamage;
				}
			}
			bool flag2 = projectile is DownstrikeProjectile_RL;
			if (flag2)
			{
				projectile.DamageMod += 0.4f * (float)SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickDamageBonus).Level;
			}
			if (lastCastAbilityTypeCasted == CastAbilityType.Weapon && !flag2 && projectile.BaseDamage > 0f)
			{
				int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsBurnAdd).Level;
				if (level > 0)
				{
					projectile.AttachStatusEffect(StatusEffectType.Enemy_Burn, 2f * (float)level);
					projectile.DamageMod -= 0f;
				}
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsPoisonAdd).Level > 0)
				{
					projectile.AttachStatusEffect(StatusEffectType.Enemy_Poison, 0f);
				}
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsComboAdd).Level > 0)
				{
					float duration = 0f;
					if (!projectile.StatusEffectTypes.IsNativeNull() && projectile.StatusEffectTypes.IndexOf(StatusEffectType.Player_Combo) != -1)
					{
						duration = float.MaxValue;
					}
					projectile.AttachStatusEffect(StatusEffectType.Player_Combo, duration);
				}
			}
		}
		else
		{
			if (TraitManager.IsTraitActive(TraitType.BonusMagicStrength))
			{
				projectile.DamageMod += 0f;
			}
			if (TraitManager.IsTraitActive(TraitType.MagicBoost))
			{
				projectile.DamageMod += 0.5f;
			}
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.MagicDamageEnemyCount);
			if (relic.Level > 0)
			{
				projectile.DamageMod += (float)relic.IntValue * 0.1f;
			}
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.BonusMana).Level * 0.08000004f;
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.SpellSwap).Level * 0.07000005f;
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.LowResolveMagicDamage).Level * 0.00999999f * (float)Mathf.Max(Mathf.RoundToInt((1f - playerController.ActualResolve) * 100f), 0);
			if (TraitManager.IsTraitActive(TraitType.ManaCostAndDamageUp) && projectile.CompareTag("PlayerProjectile"))
			{
				BaseAbility_RL ability = playerController.CastAbility.GetAbility(lastCastAbilityTypeCasted, false);
				if (ability && ability.BaseCost > 0)
				{
					projectile.DamageMod += 1f;
				}
			}
			if (lastCastAbilityTypeCasted == CastAbilityType.Spell)
			{
				int level2 = SaveManager.PlayerSaveData.GetRelic(RelicType.SpellsBurnAdd).Level;
				if (level2 > 0 && flag)
				{
					projectile.AttachStatusEffect(StatusEffectType.Enemy_Burn, 3.05f * (float)level2);
				}
				if (playerController.CurrentManaAsInt >= playerController.ActualMaxMana)
				{
					int level3 = SaveManager.PlayerSaveData.GetRelic(RelicType.MaxManaDamage).Level;
					if (level3 > 0)
					{
						projectile.DamageMod += 0.25f * (float)level3;
						projectile.RelicDamageTypeString = projectile.RelicDamageTypeString + "[" + RelicType.MaxManaDamage.ToString() + "]";
					}
				}
			}
		}
		if (playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Combo))
		{
			projectile.DamageMod += 0.02f * (float)playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Combo).TimesStacked;
		}
		if (playerController.IsInvincible)
		{
			int level4 = SaveManager.PlayerSaveData.GetRelic(RelicType.InvulnDamageBuff).Level;
			if (level4 > 0)
			{
				projectile.DamageMod += 1f * (float)level4;
				projectile.RelicDamageTypeString = projectile.RelicDamageTypeString + "[" + RelicType.InvulnDamageBuff.ToString() + "]";
			}
		}
		int level5 = SaveManager.PlayerSaveData.GetRelic(RelicType.BonusDamageCurse).Level;
		projectile.DamageMod += 1f * (float)level5;
		int level6 = SaveManager.PlayerSaveData.GetRelic(RelicType.RelicAmountDamageUp).Level;
		int totalUniqueRelics = SaveManager.PlayerSaveData.GetTotalUniqueRelics();
		projectile.DamageMod += 0.06f * (float)totalUniqueRelics * (float)level6;
		projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.AttackExhaust).Level * 0.5f;
		projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.TalentSwap).Level * 0f;
		projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.ReplacementRelic).Level * 0.1f;
		projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.DamageNoHitChallenge).Level * 1.5f;
		if ((float)playerController.CurrentHealthAsInt <= 0.5f * (float)playerController.ActualMaxHealth)
		{
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.LowHealthStatBonus).Level * 0.2f;
		}
		if ((float)playerController.CurrentHealthAsInt >= 0.5f * (float)playerController.ActualMaxHealth)
		{
			projectile.DamageMod += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.MaxHealthStatBonus).Level * 0.1f;
		}
		if (playerController.IsGrounded)
		{
			int level7 = SaveManager.PlayerSaveData.GetRelic(RelicType.GroundDamageBonus).Level;
			if (level7 > 0)
			{
				projectile.DamageMod += (float)level7 * 0.125f;
				projectile.RelicDamageTypeString = projectile.RelicDamageTypeString + "[" + RelicType.GroundDamageBonus.ToString() + "]";
			}
		}
		if (playerController.StatusEffectController.HasStatusEffect(StatusEffectType.Player_Dance))
		{
			float num2 = (float)playerController.StatusEffectController.GetStatusEffect(StatusEffectType.Player_Dance).TimesStacked * 0.15f;
			projectile.DamageMod += num2;
		}
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x000DAA48 File Offset: 0x000D8C48
	public static float CalculateProjectileCritDamage(Projectile_RL projectile, bool getMagicCritDmg)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		float result;
		if (!getMagicCritDmg)
		{
			float actualDexterity = playerController.ActualDexterity;
			float actualCritDamage = playerController.ActualCritDamage;
			float strengthScale = projectile.StrengthScale;
			result = (float)Mathf.RoundToInt(actualDexterity * strengthScale * actualCritDamage);
		}
		else
		{
			float actualFocus = playerController.ActualFocus;
			float actualMagicCritDamage = playerController.ActualMagicCritDamage;
			float magicScale = projectile.MagicScale;
			result = (float)Mathf.RoundToInt(actualFocus * magicScale * actualMagicCritDamage);
		}
		return result;
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x000DAAB4 File Offset: 0x000D8CB4
	private void AddTraitEffects(Projectile_RL projectile)
	{
		if (TraitManager.IsTraitActive(TraitType.ColorTrails) && projectile.Speed > 0f)
		{
			bool useOwnerCollisionPoint = projectile.UseOwnerCollisionPoint;
			projectile.UseOwnerCollisionPoint = false;
			BaseEffect baseEffect = EffectManager.PlayEffect(projectile.gameObject, projectile.Animator, "ColorTrails_Trait_Effect", projectile.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
			componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.time = 18f;
			componentInChildren.widthMultiplier = 1.5f;
			baseEffect.transform.SetParent(projectile.gameObject.transform, true);
			projectile.UseOwnerCollisionPoint = useOwnerCollisionPoint;
		}
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x000DAB9C File Offset: 0x000D8D9C
	public static Vector3 DetermineSpawnPosition(GameObject source, Vector2 spawnOffset, bool isFacingRight)
	{
		Vector3 result = Vector2.zero;
		if (isFacingRight)
		{
			result = source.transform.position + source.transform.rotation * spawnOffset;
		}
		else
		{
			result = source.transform.position - source.transform.rotation * new Vector2(spawnOffset.x, -spawnOffset.y);
		}
		return result;
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x000DAC1C File Offset: 0x000D8E1C
	public static Projectile_RL FireProjectile(GameObject source, string projectileName, Vector2 offset, bool matchFacing = true, float angleInDeg = 0f, float speedMod = 1f, bool useAbsPos = false, bool playSpawnAudio = true, bool playLifetimeAudio = true, bool playDeathAudio = true)
	{
		return ProjectileManager.Instance.FireProjectile_Internal(source, projectileName, offset, matchFacing, angleInDeg, speedMod, useAbsPos, playSpawnAudio, playLifetimeAudio, playDeathAudio);
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x000DAC44 File Offset: 0x000D8E44
	public static void DisableAllProjectiles(bool flagForDestruction)
	{
		foreach (KeyValuePair<string, GenericPool_RL<Projectile_RL>> keyValuePair in ProjectileManager.Instance.m_projectileDict)
		{
			if (flagForDestruction)
			{
				using (List<Projectile_RL>.Enumerator enumerator2 = keyValuePair.Value.ObjectList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Projectile_RL projectile_RL = enumerator2.Current;
						projectile_RL.FlagForDestruction(null);
					}
					continue;
				}
			}
			keyValuePair.Value.DisableAll();
		}
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x000DACEC File Offset: 0x000D8EEC
	public static void DisableAllProjectiles(bool flagForDestruction, params string[] tags)
	{
		foreach (KeyValuePair<string, GenericPool_RL<Projectile_RL>> keyValuePair in ProjectileManager.Instance.m_projectileDict)
		{
			foreach (string tag in tags)
			{
				if (keyValuePair.Value.GetFreeObj().CompareTag(tag))
				{
					if (flagForDestruction)
					{
						using (List<Projectile_RL>.Enumerator enumerator2 = keyValuePair.Value.ObjectList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Projectile_RL projectile_RL = enumerator2.Current;
								projectile_RL.FlagForDestruction(null);
							}
							goto IL_85;
						}
					}
					keyValuePair.Value.DisableAll();
				}
				IL_85:;
			}
		}
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x000DADC0 File Offset: 0x000D8FC0
	public static void DisableAllProjectiles(bool flagForDestruction, GameObject source, string projectileName)
	{
		GenericPool_RL<Projectile_RL> genericPool_RL;
		if (ProjectileManager.Instance.m_projectileDict.TryGetValue(projectileName, out genericPool_RL))
		{
			foreach (Projectile_RL projectile_RL in genericPool_RL.ObjectList)
			{
				if (projectile_RL.isActiveAndEnabled && !projectile_RL.IsFreePoolObj && projectile_RL.Owner == source)
				{
					if (flagForDestruction)
					{
						projectile_RL.FlagForDestruction(null);
					}
					else
					{
						projectile_RL.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x000DAE58 File Offset: 0x000D9058
	private static void DisableExitRoomProjectiles(object sender, EventArgs args)
	{
		foreach (KeyValuePair<string, GenericPool_RL<Projectile_RL>> keyValuePair in ProjectileManager.Instance.m_projectileDict)
		{
			foreach (Projectile_RL projectile_RL in keyValuePair.Value.ObjectList)
			{
				if (projectile_RL && projectile_RL.isActiveAndEnabled && projectile_RL.DestroyOnRoomChange)
				{
					projectile_RL.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06003E39 RID: 15929 RVA: 0x000DAF10 File Offset: 0x000D9110
	public static void DestroyBiomePools()
	{
		foreach (KeyValuePair<string, GenericPool_RL<Projectile_RL>> keyValuePair in ProjectileManager.Instance.m_projectileDict)
		{
			keyValuePair.Value.DestroyPool();
		}
		ProjectileManager.Instance.m_projectileDict.Clear();
	}

	// Token: 0x06003E3A RID: 15930 RVA: 0x000DAF7C File Offset: 0x000D917C
	public static void DestroyOffscreenIconPool()
	{
		ProjectileManager.Instance.m_offscreenIconPool.DestroyPool();
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x000DAF8D File Offset: 0x000D918D
	public static void CreateBiomePools(BiomeType biome)
	{
		ProjectileManager.Instance.Internal_CreateBiomePools(biome);
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x000DAF9A File Offset: 0x000D919A
	public static void AttachOffscreenIcon(IOffscreenObj offscreenObj, bool isEnemy)
	{
		OffscreenIconObj freeObj = ProjectileManager.Instance.m_offscreenIconPool.GetFreeObj();
		freeObj.gameObject.SetActive(true);
		freeObj.AttachOffscreenObj(offscreenObj, isEnemy);
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x000DAFBE File Offset: 0x000D91BE
	public static void Reset()
	{
		ProjectileManager.DisableAllProjectiles(false);
	}

	// Token: 0x04002E49 RID: 11849
	private const string PROJECTILEMANAGER_NAME = "ProjectileManager";

	// Token: 0x04002E4A RID: 11850
	private const string RESOURCE_PATH = "Prefabs/Managers/ProjectileManager";

	// Token: 0x04002E4B RID: 11851
	private const byte OFFSCREEN_ICON_POOLSIZE = 50;

	// Token: 0x04002E4C RID: 11852
	[SerializeField]
	private OffscreenIconObj m_offscreenIconPrefab;

	// Token: 0x04002E4D RID: 11853
	private Dictionary<string, GenericPool_RL<Projectile_RL>> m_projectileDict;

	// Token: 0x04002E4E RID: 11854
	private GenericPool_RL<OffscreenIconObj> m_offscreenIconPool;

	// Token: 0x04002E4F RID: 11855
	private Action<MonoBehaviour, EventArgs> m_createAbilityPool;

	// Token: 0x04002E50 RID: 11856
	private Action<MonoBehaviour, EventArgs> m_disableExitRoomProjectiles;

	// Token: 0x04002E51 RID: 11857
	private static bool m_isInitialized;

	// Token: 0x04002E52 RID: 11858
	private static ProjectileManager m_projectileManager = null;

	// Token: 0x04002E53 RID: 11859
	public static int ActiveProjectileCount = 0;

	// Token: 0x04002E54 RID: 11860
	private static HashSet<string> m_queueProjectileSet = new HashSet<string>();
}
