using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using RLAudio;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006A6 RID: 1702
public class ChestObj : MonoBehaviour, IRoomConsumer, IRootObj, IGenericPoolObj, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x170013E6 RID: 5094
	// (get) Token: 0x06003444 RID: 13380 RVA: 0x0001CB26 File Offset: 0x0001AD26
	// (set) Token: 0x06003445 RID: 13381 RVA: 0x0001CB2E File Offset: 0x0001AD2E
	public bool IsFreePoolObj { get; set; }

	// Token: 0x170013E7 RID: 5095
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x0001CB37 File Offset: 0x0001AD37
	// (set) Token: 0x06003447 RID: 13383 RVA: 0x0001CB3F File Offset: 0x0001AD3F
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x170013E8 RID: 5096
	// (get) Token: 0x06003448 RID: 13384 RVA: 0x0001CB48 File Offset: 0x0001AD48
	// (set) Token: 0x06003449 RID: 13385 RVA: 0x0001CB50 File Offset: 0x0001AD50
	public SpecialItemType SpecialItemOverride { get; set; }

	// Token: 0x170013E9 RID: 5097
	// (get) Token: 0x0600344A RID: 13386 RVA: 0x0001CB59 File Offset: 0x0001AD59
	// (set) Token: 0x0600344B RID: 13387 RVA: 0x0001CB61 File Offset: 0x0001AD61
	public BossID BossID { get; private set; }

	// Token: 0x170013EA RID: 5098
	// (get) Token: 0x0600344C RID: 13388 RVA: 0x0001CB6A File Offset: 0x0001AD6A
	// (set) Token: 0x0600344D RID: 13389 RVA: 0x0001CB72 File Offset: 0x0001AD72
	public int Gold { get; private set; }

	// Token: 0x170013EB RID: 5099
	// (get) Token: 0x0600344E RID: 13390 RVA: 0x0001CB7B File Offset: 0x0001AD7B
	// (set) Token: 0x0600344F RID: 13391 RVA: 0x0001CB83 File Offset: 0x0001AD83
	public bool IsInitialised { get; private set; }

	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x06003450 RID: 13392 RVA: 0x0001CB8C File Offset: 0x0001AD8C
	// (set) Token: 0x06003451 RID: 13393 RVA: 0x0001CB94 File Offset: 0x0001AD94
	public ChestLockState LockState { get; private set; }

	// Token: 0x170013ED RID: 5101
	// (get) Token: 0x06003452 RID: 13394 RVA: 0x0001CB9D File Offset: 0x0001AD9D
	public bool IsLocked
	{
		get
		{
			return this.LockState == ChestLockState.Locked || this.LockState == ChestLockState.Failed;
		}
	}

	// Token: 0x170013EE RID: 5102
	// (get) Token: 0x06003453 RID: 13395 RVA: 0x0001CBB3 File Offset: 0x0001ADB3
	// (set) Token: 0x06003454 RID: 13396 RVA: 0x0001CBBB File Offset: 0x0001ADBB
	public int Level { get; private set; }

	// Token: 0x170013EF RID: 5103
	// (get) Token: 0x06003455 RID: 13397 RVA: 0x0001CBC4 File Offset: 0x0001ADC4
	// (set) Token: 0x06003456 RID: 13398 RVA: 0x0001CBCC File Offset: 0x0001ADCC
	public ChestType ChestType { get; private set; }

	// Token: 0x170013F0 RID: 5104
	// (get) Token: 0x06003457 RID: 13399 RVA: 0x0001CBD5 File Offset: 0x0001ADD5
	// (set) Token: 0x06003458 RID: 13400 RVA: 0x0001CBDD File Offset: 0x0001ADDD
	public bool IsOpen { get; private set; }

	// Token: 0x170013F1 RID: 5105
	// (get) Token: 0x06003459 RID: 13401 RVA: 0x0001CBE6 File Offset: 0x0001ADE6
	// (set) Token: 0x0600345A RID: 13402 RVA: 0x0001CBEE File Offset: 0x0001ADEE
	public BaseRoom Room { get; private set; }

	// Token: 0x170013F2 RID: 5106
	// (get) Token: 0x0600345B RID: 13403 RVA: 0x0001CBF7 File Offset: 0x0001ADF7
	// (set) Token: 0x0600345C RID: 13404 RVA: 0x0001CBFF File Offset: 0x0001ADFF
	public Interactable Interactable { get; private set; }

	// Token: 0x170013F3 RID: 5107
	// (get) Token: 0x0600345D RID: 13405 RVA: 0x0001CC08 File Offset: 0x0001AE08
	// (set) Token: 0x0600345E RID: 13406 RVA: 0x0001CC10 File Offset: 0x0001AE10
	public int ChestIndex { get; private set; }

	// Token: 0x170013F4 RID: 5108
	// (get) Token: 0x0600345F RID: 13407 RVA: 0x0001CC19 File Offset: 0x0001AE19
	public IRelayLink OnOpenedRelay
	{
		get
		{
			return this.m_onOpenedRelay.link;
		}
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x000DC864 File Offset: 0x000DAA64
	private void Awake()
	{
		this.m_animator = base.GetComponentInChildren<Animator>();
		this.Interactable = base.GetComponent<Interactable>();
		this.m_mesh = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_disableInputYield = new WaitUntil(() => RewiredMapController.IsMapEnabled(GameInputMode.Game));
		this.m_chestArgs = new ChestOpenedEventArgs(this, SpecialItemType.None, 0, null);
		this.m_matPropertyBlock = new MaterialPropertyBlock();
		this.IsAwakeCalled = true;
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x0001CC26 File Offset: 0x0001AE26
	public void SetChestIndex(int index)
	{
		this.ChestIndex = index;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x0001CC2F File Offset: 0x0001AE2F
	private void OnDestroy()
	{
		this.Interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnChestInteractedWith));
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x0001CC4D File Offset: 0x0001AE4D
	private void OnChestInteractedWith(GameObject otherObj)
	{
		if (!this.IsOpen)
		{
			this.OpenChest();
		}
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x000DC8F4 File Offset: 0x000DAAF4
	public void ForceOpenChest()
	{
		this.m_ambientAudioEvent.Stop();
		this.m_animator.SetBool("AlreadyOpened", true);
		this.m_animator.SetTrigger("Open");
		this.m_animator.Update(2f);
		this.m_animator.Update(2f);
		this.Interactable.SetIsInteractableActive(false);
		this.m_hitboxController.GetCollider(HitboxType.Terrain).enabled = false;
		this.IsOpen = true;
	}

	// Token: 0x06003465 RID: 13413 RVA: 0x000DC974 File Offset: 0x000DAB74
	private void OpenChest()
	{
		if (this.IsLocked)
		{
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.FreeFairyChest);
			if (relic.Level <= 0)
			{
				this.m_animator.SetTrigger("LockRattle");
				AudioManager.PlayOneShot(null, "event:/UI/InGame/ui_ig_locked", base.transform.position);
				return;
			}
			this.m_animator.SetBool("FairyFailed", false);
			relic.SetLevel(-1, true, true);
			SaveManager.PlayerSaveData.GetRelic(RelicType.FreeFairyChestUsed).SetLevel(1, true, true);
		}
		this.m_onOpenedRelay.Dispatch();
		this.m_openedAudioEvent.Play();
		this.m_ambientAudioEvent.Stop();
		this.Interactable.SetIsInteractableActive(false);
		this.m_hitboxController.GetCollider(HitboxType.Terrain).enabled = false;
		this.IsOpen = true;
		if (this.BossID != BossID.None)
		{
			this.m_previousBossNGLevel = SaveManager.ModeSaveData.GetHighestNGBossBeaten(SaveManager.PlayerSaveData.GameModeType, this.BossID);
		}
		else
		{
			this.m_previousBossNGLevel = -1;
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OpenChestAnimCoroutine());
	}

	// Token: 0x06003466 RID: 13414 RVA: 0x000DCA84 File Offset: 0x000DAC84
	public void SetSparkleParticleOpacity(float opacity)
	{
		opacity = Mathf.Clamp(1f - opacity, 0f, 1f);
		if (this.m_currentParticleSystemOpacity == opacity)
		{
			return;
		}
		if (!this.m_particleSystem)
		{
			this.m_particleSystem = base.gameObject.GetComponentInChildren<ParticleSystem>();
		}
		if (this.m_particleSystem)
		{
			Renderer component = this.m_particleSystem.GetComponent<Renderer>();
			if (component)
			{
				if (component.material.HasProperty(ShaderID_RL._MultiplyColor))
				{
					Color color = component.material.GetColor(ShaderID_RL._MultiplyColor);
					color.a = opacity;
					component.material.SetColor(ShaderID_RL._MultiplyColor, color);
				}
				else
				{
					Debug.LogFormat("| {0} | Sparkle Renderer's Material does not have a property called <b>{1}</b>.</color>", new object[]
					{
						this,
						ShaderID_RL._MultiplyColor
					});
				}
			}
		}
		else
		{
			Debug.Log("No particle system");
		}
		this.m_currentParticleSystemOpacity = opacity;
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x0001CC5D File Offset: 0x0001AE5D
	private IEnumerator OpenChestAnimCoroutine()
	{
		SpecialItemType rewardType = this.GetSpecialItemTypeToDrop();
		ISpecialItemDrop specialItemDropObj = this.CalculateSpecialItemDropObj(rewardType);
		if (this.ChestType != ChestType.Boss && this.ChestType != ChestType.Black && rewardType != SpecialItemType.Gold && rewardType != SpecialItemType.Ore && rewardType != SpecialItemType.Soul && specialItemDropObj == null)
		{
			if (this.ChestType == ChestType.Fairy)
			{
				rewardType = SpecialItemType.Ore;
			}
			else
			{
				rewardType = SpecialItemType.Gold;
			}
		}
		this.m_dropPosition = base.transform.position;
		this.m_dropPosition.y = this.m_dropPosition.y + 1f;
		this.m_animator.SetTrigger("Open");
		float architectGoldMod = NPC_EV.GetArchitectGoldMod(-1);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.NoGoldXPBonus).Level;
		if (rewardType == SpecialItemType.Gold && this.ChestType != ChestType.Boss && this.ChestType != ChestType.Black && (architectGoldMod <= 0f || level > 0))
		{
			this.m_animator.SetBool("Empty", true);
			EffectManager.PlayEffect(base.gameObject, this.m_animator, "ChestOpenMoths_Effect", this.m_dropPosition, 5f, EffectStopType.Immediate, EffectTriggerDirection.None);
		}
		else if (specialItemDropObj != null)
		{
			EffectManager.PlayEffect(base.gameObject, this.m_animator, "ChestOpenUnscaledTime_Effect", base.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		else
		{
			EffectManager.PlayEffect(base.gameObject, this.m_animator, "ChestOpen_Effect", base.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (specialItemDropObj != null)
		{
			playerController.SetVelocity(0f, 0f, false);
			playerController.StopActiveAbilities(true);
			playerController.CharacterHitResponse.SetInvincibleTime(999f, false, false);
			RewiredMapController.SetMapEnabled(GameInputMode.Game, false);
		}
		string treasureChestOpenState = "TreasureChest_Open_Animation";
		while (!this.m_animator.GetCurrentAnimatorStateInfo(0).IsName(treasureChestOpenState))
		{
			yield return null;
		}
		if (this.ChestType != ChestType.Boss && TraitManager.IsTraitActive(TraitType.ExplosiveChests))
		{
			float angleInDeg = UnityEngine.Random.Range(Trait_EV.EXPLOSIVE_CHEST_PROJECTILE_ANGLE.x, Trait_EV.EXPLOSIVE_CHEST_PROJECTILE_ANGLE.y);
			ProjectileManager.FireProjectile(base.gameObject, "ExplosiveChestsPotionProjectile", base.transform.position, false, angleInDeg, 1f, true, true, true, true);
		}
		int level2 = SaveManager.PlayerSaveData.GetRelic(RelicType.ChestHealthRestore).Level;
		float num = (float)(Mathf.CeilToInt(playerController.ActualMagic * 1f) * level2);
		if (num > 0f)
		{
			playerController.SetHealth(num, true, true);
			Vector2 absPos = playerController.Midpoint;
			absPos.y += playerController.CollisionBounds.height / 2f;
			string text = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, text, absPos, null, TextAlignmentOptions.Center);
		}
		if (specialItemDropObj == null)
		{
			if (this.ChestType == ChestType.Boss)
			{
				this.DropRewardFromBossChest();
			}
			else if (this.ChestType == ChestType.Black)
			{
				this.DropRegularRewardFromBlackChest(rewardType, null, this.Level);
			}
			else
			{
				if (this.ChestType == ChestType.Gold)
				{
					int amount = Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.EquipmentOre, this.Level) * 2f);
					int amount2 = Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.RuneOre, this.Level) * 0.5f);
					ItemDropManager.DropItem(ItemDropType.EquipmentOre, amount, this.m_dropPosition, true, true, true);
					ItemDropManager.DropItem(ItemDropType.RuneOre, amount2, this.m_dropPosition, true, true, true);
				}
				if (this.ChestType == ChestType.Silver)
				{
					int amount3 = Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.EquipmentOre, this.Level) * 0.6f);
					ItemDropManager.DropItem(ItemDropType.EquipmentOre, amount3, this.m_dropPosition, false, false, true);
				}
				this.DropRewardFromRegularChest(rewardType, null, this.Level);
			}
		}
		else
		{
			yield return null;
			GameManager.SetIsPaused(true);
			AnimatorUpdateMode storedPlayerUpdateMode = playerController.Animator.updateMode;
			AnimatorUpdateMode storedChestUpdateMode = this.m_animator.updateMode;
			playerController.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			this.m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			playerController.CharacterMove.SetHorizontalMove(0f);
			playerController.Animator.SetBool("Victory", true);
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			if (this.ChestType == ChestType.Boss)
			{
				this.DropRewardFromBossChest();
			}
			else if (this.ChestType == ChestType.Black)
			{
				this.DropRegularRewardFromBlackChest(rewardType, specialItemDropObj, this.Level);
			}
			else
			{
				this.DropRewardFromRegularChest(rewardType, specialItemDropObj, this.Level);
			}
			yield return this.m_disableInputYield;
			RewiredMapController.SetMapEnabled(GameInputMode.Game, false);
			GameManager.SetIsPaused(true);
			if (this.ChestType == ChestType.Gold)
			{
				int amount4 = Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.EquipmentOre, this.Level) * 2f);
				int amount5 = Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.RuneOre, this.Level) * 0.5f);
				ItemDropManager.DropItem(ItemDropType.EquipmentOre, amount4, this.m_dropPosition, true, true, true);
				ItemDropManager.DropItem(ItemDropType.RuneOre, amount5, this.m_dropPosition, true, true, true);
			}
			else if (this.ChestType == ChestType.Silver)
			{
				ItemDropManager.DropItem(ItemDropType.EquipmentOre, Mathf.CeilToInt((float)this.GetOreDropAmount(ItemDropType.EquipmentOre, this.Level) * 0.6f), this.m_dropPosition, false, false, true);
			}
			else if (this.ChestType == ChestType.Fairy)
			{
				ItemDropManager.DropItem(ItemDropType.RuneOre, this.GetOreDropAmount(ItemDropType.RuneOre, this.Level), this.m_dropPosition, true, true, true);
			}
			playerController.Animator.SetBool("Victory", false);
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			playerController.CharacterHitResponse.SetInvincibleTime(1f, false, false);
			RewiredMapController.SetMapEnabled(GameInputMode.Game, true);
			GameManager.SetIsPaused(false);
			playerController.Animator.updateMode = storedPlayerUpdateMode;
			this.m_animator.updateMode = storedChestUpdateMode;
		}
		yield break;
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x000DCB64 File Offset: 0x000DAD64
	private void DropRewardFromRegularChest(SpecialItemType itemDropType, ISpecialItemDrop specialItemDrop, int chestLevel)
	{
		int num = 0;
		this.m_specialItemDropsList.Clear();
		bool forceMagnetize = this.ChestType == ChestType.Fairy || this.ChestType == ChestType.Gold;
		if (itemDropType <= SpecialItemType.Gold)
		{
			if (itemDropType == SpecialItemType.None)
			{
				goto IL_104;
			}
			if (itemDropType == SpecialItemType.Gold)
			{
				ItemDropManager.DropGold(this.Gold, this.m_dropPosition, TraitManager.IsTraitActive(TraitType.ItemsGoFlying) || this.ChestType == ChestType.Gold, forceMagnetize, true);
				num = this.Gold;
				goto IL_104;
			}
		}
		else
		{
			if (itemDropType == SpecialItemType.Ore)
			{
				ItemDropType itemDropType2 = (this.ChestType == ChestType.Fairy) ? ItemDropType.RuneOre : ItemDropType.EquipmentOre;
				num = this.GetOreDropAmount(itemDropType2, chestLevel);
				ItemDropManager.DropItem(itemDropType2, num, this.m_dropPosition, true, forceMagnetize, true);
				goto IL_104;
			}
			if (itemDropType == SpecialItemType.Soul)
			{
				ItemDropManager.DropItem(ItemDropType.Soul, 200, this.m_dropPosition, true, true, true);
				goto IL_104;
			}
		}
		if (specialItemDrop != null)
		{
			this.m_specialItemDropsList.Add(specialItemDrop);
		}
		foreach (ISpecialItemDrop specialItemDrop2 in this.m_specialItemDropsList)
		{
			ItemDropManager.DropSpecialItem(specialItemDrop2, true);
		}
		IL_104:
		this.m_chestArgs.Initialize(this, itemDropType, num, this.m_specialItemDropsList);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ChestOpened, this, this.m_chestArgs);
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x000DCCA8 File Offset: 0x000DAEA8
	private int GetOreDropAmount(ItemDropType oreDropType, int chestLevel)
	{
		float num = (float)((oreDropType == ItemDropType.RuneOre) ? 170 : 190);
		float num2 = (oreDropType == ItemDropType.RuneOre) ? 2.25f : 5.1f;
		return (int)(num + (float)chestLevel * num2);
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x000DCCE0 File Offset: 0x000DAEE0
	private void DropRewardFromBossChest()
	{
		if (this.BossID != BossID.None)
		{
			if (Economy_EV.BOSS_DROP_TABLE.ContainsKey(this.BossID))
			{
				int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
				int bossSoulsDropAmount = Souls_EV.GetBossSoulsDropAmount(this.BossID, this.m_previousBossNGLevel);
				int num = Mathf.Clamp(Souls_EV.GetBossSoulsDropAmount(this.BossID, newGamePlusLevel) - bossSoulsDropAmount, 0, int.MaxValue);
				SoulDrop.FakeSoulCounter_STATIC = num;
				ItemDropManager.DropItem(ItemDropType.Soul, num, this.m_dropPosition, true, true, true);
				float num2 = (float)newGamePlusLevel * 0.75f;
				if (this.IsVariantBossChest(this.BossID))
				{
					num2 += 1.25f;
				}
				int goldAmount = Economy_EV.BOSS_DROP_TABLE[this.BossID].GoldAmount;
				ItemDropManager.DropGold(goldAmount + Mathf.RoundToInt((float)goldAmount * num2), this.m_dropPosition, true, true, true);
				int num3 = Economy_EV.BOSS_DROP_TABLE[this.BossID].OreCount;
				num3 += Mathf.RoundToInt((float)num3 * num2);
				ItemDropManager.DropItem(ItemDropType.EquipmentOre, num3, this.m_dropPosition, true, true, true);
				int num4 = Economy_EV.BOSS_DROP_TABLE[this.BossID].RuneOreCount;
				num4 += Mathf.RoundToInt((float)num4 * num2);
				ItemDropManager.DropItem(ItemDropType.RuneOre, num4, this.m_dropPosition, true, true, true);
			}
			this.m_chestArgs.Initialize(this, SpecialItemType.Gold, 0, this.m_specialItemDropsList);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ChestOpened, this, this.m_chestArgs);
			return;
		}
		Debug.LogFormat("<color=red>[{0}] Chest is of Type, Boss, but its BossID is set to None. You must specify a Boss ID</color>", new object[]
		{
			this
		});
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x000DCE50 File Offset: 0x000DB050
	private bool IsVariantBossChest(BossID bossID)
	{
		if (bossID <= BossID.Forest_Boss)
		{
			if (bossID == BossID.Castle_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.CastleBossUp) > 0;
			}
			if (bossID == BossID.Bridge_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.BridgeBossUp) > 0;
			}
			if (bossID == BossID.Forest_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.ForestBossUp) > 0;
			}
		}
		else
		{
			if (bossID == BossID.Tower_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.TowerBossUp) > 0;
			}
			if (bossID == BossID.Cave_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.CaveBossUp) > 0;
			}
			if (bossID == BossID.Final_Boss)
			{
				return BurdenManager.GetBurdenLevel(BurdenType.FinalBossUp) > 0;
			}
		}
		return false;
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x000DCEDC File Offset: 0x000DB0DC
	private void DropRegularRewardFromBlackChest(SpecialItemType itemDropType, ISpecialItemDrop specialItemDrop, int level)
	{
		int oreDropAmount = this.GetOreDropAmount(ItemDropType.EquipmentOre, level);
		ItemDropManager.DropItem(ItemDropType.EquipmentOre, oreDropAmount, this.m_dropPosition, true, true, true);
		oreDropAmount = this.GetOreDropAmount(ItemDropType.RuneOre, level);
		ItemDropManager.DropItem(ItemDropType.RuneOre, oreDropAmount, this.m_dropPosition, true, true, true);
		int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		int totalBlackChestSoulsCollected = Souls_EV.GetTotalBlackChestSoulsCollected();
		EquipmentCategoryType finalGearCategoryType = EquipmentType_RL.GetFinalGearCategoryType(itemDropType);
		SaveManager.ModeSaveData.SetHighestNGBlackChestOpened(finalGearCategoryType, newGamePlusLevel, false, false);
		int num = Mathf.Clamp(Souls_EV.GetTotalBlackChestSoulsCollected() - totalBlackChestSoulsCollected, 0, int.MaxValue);
		if (num > 0)
		{
			SoulDrop.FakeSoulCounter_STATIC = num;
			ItemDropManager.DropItem(ItemDropType.Soul, num, this.m_dropPosition, true, true, true);
		}
		this.m_specialItemDropsList.Clear();
		if (specialItemDrop != null)
		{
			this.m_specialItemDropsList.Add(specialItemDrop);
			ItemDropManager.DropSpecialItem(specialItemDrop, true);
		}
		this.m_chestArgs.Initialize(this, itemDropType, oreDropAmount, this.m_specialItemDropsList);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.ChestOpened, this, this.m_chestArgs);
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x000DCFBC File Offset: 0x000DB1BC
	private ISpecialItemDrop CalculateSpecialItemDropObj(SpecialItemType specialItemType)
	{
		bool flag = true;
		ISpecialItemDrop specialItemDrop = null;
		if (specialItemType > SpecialItemType.Challenge)
		{
			if (specialItemType <= SpecialItemType.FinalGear_Helm)
			{
				if (specialItemType != SpecialItemType.FinalGear_Weapon && specialItemType != SpecialItemType.FinalGear_Helm)
				{
					goto IL_A2;
				}
			}
			else if (specialItemType != SpecialItemType.FinalGear_Chest && specialItemType != SpecialItemType.FinalGear_Cape && specialItemType != SpecialItemType.FinalGear_Trinket)
			{
				goto IL_A2;
			}
			specialItemDrop = SpecialItemDropUtility.GetFinalBlueprintDrop(EquipmentType_RL.GetFinalGearCategoryType(specialItemType));
			goto IL_A4;
		}
		if (specialItemType <= SpecialItemType.Rune)
		{
			if (specialItemType == SpecialItemType.Blueprint)
			{
				specialItemDrop = SpecialItemDropUtility.GetBlueprintDrop(this.Level, ChestType_RL.GetChestRarity(this.ChestType));
				goto IL_A4;
			}
			if (specialItemType == SpecialItemType.Rune)
			{
				specialItemDrop = SpecialItemDropUtility.GetRuneDrop(this.Level);
				goto IL_A4;
			}
		}
		else
		{
			if (specialItemType == SpecialItemType.Relic)
			{
				specialItemDrop = SpecialItemDropUtility.GetRandomRelicDrop();
				goto IL_A4;
			}
			if (specialItemType == SpecialItemType.Challenge)
			{
				specialItemDrop = SpecialItemDropUtility.GetChallengeDrop();
				goto IL_A4;
			}
		}
		IL_A2:
		flag = false;
		IL_A4:
		if (flag && specialItemDrop == null)
		{
			Debug.Log("Tried to drop special item of type: " + specialItemType.ToString() + " but failed. No available drops found.");
		}
		return specialItemDrop;
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x000DD098 File Offset: 0x000DB298
	private SpecialItemType GetSpecialItemTypeToDrop()
	{
		if (this.SpecialItemOverride != SpecialItemType.None)
		{
			return this.SpecialItemOverride;
		}
		if (this.ChestType == ChestType.Black && this.SpecialItemOverride == SpecialItemType.None)
		{
			throw new Exception("Black Chests MUST have a SpecialItemOverride assigned to it.");
		}
		if (this.ChestType == ChestType.Gold)
		{
			return SpecialItemType.Challenge;
		}
		Vector2[] chestItemTypeOdds = Economy_EV.GetChestItemTypeOdds(this.ChestType);
		int randomOdds = CDGHelper.GetRandomOdds(this.GetDropOdds(chestItemTypeOdds));
		return (SpecialItemType)chestItemTypeOdds[randomOdds].x;
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x000DD104 File Offset: 0x000DB304
	private List<float> GetDropOdds(Vector2[] dropTypeArray)
	{
		ChestObj.m_dropOddsHelper_STATIC.Clear();
		int num = dropTypeArray.Length;
		for (int i = 0; i < num; i++)
		{
			ChestObj.m_dropOddsHelper_STATIC.Add(dropTypeArray[i].y);
		}
		return ChestObj.m_dropOddsHelper_STATIC;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x000DD148 File Offset: 0x000DB348
	public void Initialise(ChestType chestType, int level, int gold, BossID bossID = BossID.None)
	{
		this.ChestType = chestType;
		this.Level = level;
		this.Gold = gold;
		this.BossID = bossID;
		if (this.Interactable)
		{
			this.Interactable.SetIsInteractableActive(!this.IsLocked);
			this.Interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnChestInteractedWith));
		}
		else
		{
			Debug.LogFormat("<color=red>[{0}] m_interactable field is null</color>", new object[]
			{
				this
			});
		}
		this.IsInitialised = true;
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x000DD1CC File Offset: 0x000DB3CC
	public void SetChestLockState(ChestLockState lockState)
	{
		this.LockState = lockState;
		switch (lockState)
		{
		case ChestLockState.Unlocked:
			this.m_animator.SetBool("Locked", false);
			this.m_animator.SetBool("FairyFailed", false);
			return;
		case ChestLockState.Locked:
			this.m_animator.SetBool("Locked", true);
			this.m_animator.SetBool("FairyFailed", false);
			return;
		case ChestLockState.Failed:
			this.m_animator.SetBool("Locked", false);
			this.m_animator.SetBool("FairyFailed", true);
			this.m_ambientAudioEvent.Stop();
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.FreeFairyChest).Level > 0)
			{
				this.Interactable.SetIsInteractableActive(true);
				return;
			}
			this.Interactable.SetIsInteractableActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x000DD298 File Offset: 0x000DB498
	public void SetOpacity(float opacity)
	{
		if (opacity < 0f || opacity > 1f)
		{
			throw new ArgumentOutOfRangeException("opacity", string.Format("<color=red>opacity must have a value between 0 and 1.</color>", Array.Empty<object>()));
		}
		this.m_mesh.GetPropertyBlock(this.m_matPropertyBlock);
		this.m_matPropertyBlock.SetFloat("_Opacity", opacity);
		this.m_mesh.SetPropertyBlock(this.m_matPropertyBlock);
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x0001CC6C File Offset: 0x0001AE6C
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x0001CC75 File Offset: 0x0001AE75
	private void OnDisable()
	{
		if (this.m_particleSystem && this.m_currentParticleSystemOpacity != 1f)
		{
			this.SetSparkleParticleOpacity(1f);
			this.m_particleSystem = null;
		}
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x0001CCAA File Offset: 0x0001AEAA
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") && this.Interactable.IsInteractableActive)
		{
			this.m_proximityAudioEvent.Play();
		}
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x000DD304 File Offset: 0x000DB504
	public void ResetValues()
	{
		this.SetChestLockState(ChestLockState.Unlocked);
		this.m_animator.SetBool("AlreadyOpened", false);
		this.m_animator.SetBool("Open", false);
		this.m_animator.SetBool("Locked", false);
		this.m_animator.SetBool("FairyFailed", false);
		this.m_animator.SetBool("Empty", false);
		this.m_animator.Update(0f);
		this.m_animator.Update(0f);
		this.Interactable.SetIsInteractableActive(true);
		this.m_hitboxController.GetCollider(HitboxType.Terrain).enabled = true;
		this.IsOpen = false;
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002A53 RID: 10835
	[SerializeField]
	private StudioEventEmitter m_openedAudioEvent;

	// Token: 0x04002A54 RID: 10836
	[SerializeField]
	private StudioEventEmitter m_ambientAudioEvent;

	// Token: 0x04002A55 RID: 10837
	[SerializeField]
	private StudioEventEmitter m_proximityAudioEvent;

	// Token: 0x04002A56 RID: 10838
	[SerializeField]
	private HitboxControllerLite m_hitboxController;

	// Token: 0x04002A57 RID: 10839
	private Animator m_animator;

	// Token: 0x04002A58 RID: 10840
	private SkinnedMeshRenderer m_mesh;

	// Token: 0x04002A59 RID: 10841
	private List<ISpecialItemDrop> m_specialItemDropsList = new List<ISpecialItemDrop>();

	// Token: 0x04002A5A RID: 10842
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002A5B RID: 10843
	private WaitUntil m_disableInputYield;

	// Token: 0x04002A5C RID: 10844
	private ChestOpenedEventArgs m_chestArgs;

	// Token: 0x04002A5D RID: 10845
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04002A5E RID: 10846
	private ParticleSystem m_particleSystem;

	// Token: 0x04002A5F RID: 10847
	private float m_currentParticleSystemOpacity = 1f;

	// Token: 0x04002A60 RID: 10848
	private int m_previousBossNGLevel = -1;

	// Token: 0x04002A61 RID: 10849
	private Vector3 m_dropPosition;

	// Token: 0x04002A6F RID: 10863
	private Relay m_onOpenedRelay = new Relay();

	// Token: 0x04002A70 RID: 10864
	private static List<float> m_dropOddsHelper_STATIC = new List<float>();
}
