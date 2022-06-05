using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using RLAudio;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003FB RID: 1019
public class ChestObj : MonoBehaviour, IRoomConsumer, IRootObj, IGenericPoolObj, ITerrainOnEnterHitResponse, IHitResponse
{
	// Token: 0x17000F35 RID: 3893
	// (get) Token: 0x060025CE RID: 9678 RVA: 0x0007CCF5 File Offset: 0x0007AEF5
	// (set) Token: 0x060025CF RID: 9679 RVA: 0x0007CCFD File Offset: 0x0007AEFD
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x060025D0 RID: 9680 RVA: 0x0007CD06 File Offset: 0x0007AF06
	// (set) Token: 0x060025D1 RID: 9681 RVA: 0x0007CD0E File Offset: 0x0007AF0E
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x060025D2 RID: 9682 RVA: 0x0007CD17 File Offset: 0x0007AF17
	// (set) Token: 0x060025D3 RID: 9683 RVA: 0x0007CD1F File Offset: 0x0007AF1F
	public SpecialItemType SpecialItemOverride { get; set; }

	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x060025D4 RID: 9684 RVA: 0x0007CD28 File Offset: 0x0007AF28
	// (set) Token: 0x060025D5 RID: 9685 RVA: 0x0007CD30 File Offset: 0x0007AF30
	public BossID BossID { get; private set; }

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x060025D6 RID: 9686 RVA: 0x0007CD39 File Offset: 0x0007AF39
	// (set) Token: 0x060025D7 RID: 9687 RVA: 0x0007CD41 File Offset: 0x0007AF41
	public int Gold { get; private set; }

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x060025D8 RID: 9688 RVA: 0x0007CD4A File Offset: 0x0007AF4A
	// (set) Token: 0x060025D9 RID: 9689 RVA: 0x0007CD52 File Offset: 0x0007AF52
	public bool IsInitialised { get; private set; }

	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x060025DA RID: 9690 RVA: 0x0007CD5B File Offset: 0x0007AF5B
	// (set) Token: 0x060025DB RID: 9691 RVA: 0x0007CD63 File Offset: 0x0007AF63
	public ChestLockState LockState { get; private set; }

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x060025DC RID: 9692 RVA: 0x0007CD6C File Offset: 0x0007AF6C
	public bool IsLocked
	{
		get
		{
			return this.LockState == ChestLockState.Locked || this.LockState == ChestLockState.Failed;
		}
	}

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x060025DD RID: 9693 RVA: 0x0007CD82 File Offset: 0x0007AF82
	// (set) Token: 0x060025DE RID: 9694 RVA: 0x0007CD8A File Offset: 0x0007AF8A
	public int Level { get; private set; }

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060025DF RID: 9695 RVA: 0x0007CD93 File Offset: 0x0007AF93
	// (set) Token: 0x060025E0 RID: 9696 RVA: 0x0007CD9B File Offset: 0x0007AF9B
	public ChestType ChestType { get; private set; }

	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x060025E1 RID: 9697 RVA: 0x0007CDA4 File Offset: 0x0007AFA4
	// (set) Token: 0x060025E2 RID: 9698 RVA: 0x0007CDAC File Offset: 0x0007AFAC
	public bool IsOpen { get; private set; }

	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x060025E3 RID: 9699 RVA: 0x0007CDB5 File Offset: 0x0007AFB5
	// (set) Token: 0x060025E4 RID: 9700 RVA: 0x0007CDBD File Offset: 0x0007AFBD
	public BaseRoom Room { get; private set; }

	// Token: 0x17000F41 RID: 3905
	// (get) Token: 0x060025E5 RID: 9701 RVA: 0x0007CDC6 File Offset: 0x0007AFC6
	// (set) Token: 0x060025E6 RID: 9702 RVA: 0x0007CDCE File Offset: 0x0007AFCE
	public Interactable Interactable { get; private set; }

	// Token: 0x17000F42 RID: 3906
	// (get) Token: 0x060025E7 RID: 9703 RVA: 0x0007CDD7 File Offset: 0x0007AFD7
	// (set) Token: 0x060025E8 RID: 9704 RVA: 0x0007CDDF File Offset: 0x0007AFDF
	public int ChestIndex { get; private set; }

	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x060025E9 RID: 9705 RVA: 0x0007CDE8 File Offset: 0x0007AFE8
	public IRelayLink OnOpenedRelay
	{
		get
		{
			return this.m_onOpenedRelay.link;
		}
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x0007CDF8 File Offset: 0x0007AFF8
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

	// Token: 0x060025EB RID: 9707 RVA: 0x0007CE85 File Offset: 0x0007B085
	public void SetChestIndex(int index)
	{
		this.ChestIndex = index;
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x0007CE8E File Offset: 0x0007B08E
	private void OnDestroy()
	{
		this.Interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.OnChestInteractedWith));
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x0007CEAC File Offset: 0x0007B0AC
	private void OnChestInteractedWith(GameObject otherObj)
	{
		if (!this.IsOpen)
		{
			this.OpenChest();
		}
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x0007CEBC File Offset: 0x0007B0BC
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

	// Token: 0x060025EF RID: 9711 RVA: 0x0007CF3C File Offset: 0x0007B13C
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

	// Token: 0x060025F0 RID: 9712 RVA: 0x0007D04C File Offset: 0x0007B24C
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

	// Token: 0x060025F1 RID: 9713 RVA: 0x0007D12C File Offset: 0x0007B32C
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

	// Token: 0x060025F2 RID: 9714 RVA: 0x0007D13C File Offset: 0x0007B33C
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

	// Token: 0x060025F3 RID: 9715 RVA: 0x0007D280 File Offset: 0x0007B480
	private int GetOreDropAmount(ItemDropType oreDropType, int chestLevel)
	{
		float num = (float)((oreDropType == ItemDropType.RuneOre) ? 170 : 190);
		float num2 = (oreDropType == ItemDropType.RuneOre) ? 2.25f : 5.1f;
		return (int)(num + (float)chestLevel * num2);
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x0007D2B8 File Offset: 0x0007B4B8
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

	// Token: 0x060025F5 RID: 9717 RVA: 0x0007D428 File Offset: 0x0007B628
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

	// Token: 0x060025F6 RID: 9718 RVA: 0x0007D4B4 File Offset: 0x0007B6B4
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

	// Token: 0x060025F7 RID: 9719 RVA: 0x0007D594 File Offset: 0x0007B794
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

	// Token: 0x060025F8 RID: 9720 RVA: 0x0007D670 File Offset: 0x0007B870
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

	// Token: 0x060025F9 RID: 9721 RVA: 0x0007D6DC File Offset: 0x0007B8DC
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

	// Token: 0x060025FA RID: 9722 RVA: 0x0007D720 File Offset: 0x0007B920
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

	// Token: 0x060025FB RID: 9723 RVA: 0x0007D7A4 File Offset: 0x0007B9A4
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

	// Token: 0x060025FC RID: 9724 RVA: 0x0007D870 File Offset: 0x0007BA70
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

	// Token: 0x060025FD RID: 9725 RVA: 0x0007D8DA File Offset: 0x0007BADA
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x0007D8E3 File Offset: 0x0007BAE3
	private void OnDisable()
	{
		if (this.m_particleSystem && this.m_currentParticleSystemOpacity != 1f)
		{
			this.SetSparkleParticleOpacity(1f);
			this.m_particleSystem = null;
		}
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x0007D918 File Offset: 0x0007BB18
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController.RootGameObject.CompareTag("Player") && this.Interactable.IsInteractableActive)
		{
			this.m_proximityAudioEvent.Play();
		}
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x0007D944 File Offset: 0x0007BB44
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

	// Token: 0x06002603 RID: 9731 RVA: 0x0007DA2E File Offset: 0x0007BC2E
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x0007DA36 File Offset: 0x0007BC36
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001FAA RID: 8106
	[SerializeField]
	private StudioEventEmitter m_openedAudioEvent;

	// Token: 0x04001FAB RID: 8107
	[SerializeField]
	private StudioEventEmitter m_ambientAudioEvent;

	// Token: 0x04001FAC RID: 8108
	[SerializeField]
	private StudioEventEmitter m_proximityAudioEvent;

	// Token: 0x04001FAD RID: 8109
	[SerializeField]
	private HitboxControllerLite m_hitboxController;

	// Token: 0x04001FAE RID: 8110
	private Animator m_animator;

	// Token: 0x04001FAF RID: 8111
	private SkinnedMeshRenderer m_mesh;

	// Token: 0x04001FB0 RID: 8112
	private List<ISpecialItemDrop> m_specialItemDropsList = new List<ISpecialItemDrop>();

	// Token: 0x04001FB1 RID: 8113
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001FB2 RID: 8114
	private WaitUntil m_disableInputYield;

	// Token: 0x04001FB3 RID: 8115
	private ChestOpenedEventArgs m_chestArgs;

	// Token: 0x04001FB4 RID: 8116
	private MaterialPropertyBlock m_matPropertyBlock;

	// Token: 0x04001FB5 RID: 8117
	private ParticleSystem m_particleSystem;

	// Token: 0x04001FB6 RID: 8118
	private float m_currentParticleSystemOpacity = 1f;

	// Token: 0x04001FB7 RID: 8119
	private int m_previousBossNGLevel = -1;

	// Token: 0x04001FB8 RID: 8120
	private Vector3 m_dropPosition;

	// Token: 0x04001FC6 RID: 8134
	private Relay m_onOpenedRelay = new Relay();

	// Token: 0x04001FC7 RID: 8135
	private static List<float> m_dropOddsHelper_STATIC = new List<float>();
}
