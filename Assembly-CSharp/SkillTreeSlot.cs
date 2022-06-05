using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020006F2 RID: 1778
public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x170015F8 RID: 5624
	// (get) Token: 0x0600404A RID: 16458 RVA: 0x000E3FD2 File Offset: 0x000E21D2
	// (set) Token: 0x0600404B RID: 16459 RVA: 0x000E3FDA File Offset: 0x000E21DA
	public bool HasAnimated { get; private set; }

	// Token: 0x170015F9 RID: 5625
	// (get) Token: 0x0600404C RID: 16460 RVA: 0x000E3FE3 File Offset: 0x000E21E3
	public Vector3 StoredScale
	{
		get
		{
			return this.m_storedScale;
		}
	}

	// Token: 0x170015FA RID: 5626
	// (get) Token: 0x0600404D RID: 16461 RVA: 0x000E3FEB File Offset: 0x000E21EB
	public bool HasAnimParam
	{
		get
		{
			return !string.IsNullOrEmpty(this.m_bgAnimatorParam);
		}
	}

	// Token: 0x170015FB RID: 5627
	// (get) Token: 0x0600404E RID: 16462 RVA: 0x000E3FFB File Offset: 0x000E21FB
	// (set) Token: 0x0600404F RID: 16463 RVA: 0x000E4003 File Offset: 0x000E2203
	public int SlotIndex { get; private set; }

	// Token: 0x170015FC RID: 5628
	// (get) Token: 0x06004050 RID: 16464 RVA: 0x000E400C File Offset: 0x000E220C
	// (set) Token: 0x06004051 RID: 16465 RVA: 0x000E4014 File Offset: 0x000E2214
	public SkillTreeType SkillTreeType
	{
		get
		{
			return this.m_skillTreeType;
		}
		set
		{
			bool flag = this.m_skillTreeType != value;
			this.m_skillTreeType = value;
			if (flag)
			{
				this.UpdateSkillTreeType();
			}
		}
	}

	// Token: 0x170015FD RID: 5629
	// (get) Token: 0x06004052 RID: 16466 RVA: 0x000E4031 File Offset: 0x000E2231
	// (set) Token: 0x06004053 RID: 16467 RVA: 0x000E4039 File Offset: 0x000E2239
	public string BGAnimatorParam
	{
		get
		{
			return this.m_bgAnimatorParam;
		}
		set
		{
			this.m_bgAnimatorParam = value;
		}
	}

	// Token: 0x170015FE RID: 5630
	// (get) Token: 0x06004054 RID: 16468 RVA: 0x000E4042 File Offset: 0x000E2242
	// (set) Token: 0x06004055 RID: 16469 RVA: 0x000E404A File Offset: 0x000E224A
	public SkillTreeSlot.SkillTreeAnimType AnimType
	{
		get
		{
			return this.m_animType;
		}
		set
		{
			this.m_animType = value;
		}
	}

	// Token: 0x170015FF RID: 5631
	// (get) Token: 0x06004056 RID: 16470 RVA: 0x000E4053 File Offset: 0x000E2253
	// (set) Token: 0x06004057 RID: 16471 RVA: 0x000E405B File Offset: 0x000E225B
	public Vector2 AnimEffectOffset
	{
		get
		{
			return this.m_animEffectOffset;
		}
		set
		{
			this.m_animEffectOffset = value;
		}
	}

	// Token: 0x17001600 RID: 5632
	// (get) Token: 0x06004058 RID: 16472 RVA: 0x000E4064 File Offset: 0x000E2264
	public bool HasData
	{
		get
		{
			return this.m_skillTreeType > SkillTreeType.None;
		}
	}

	// Token: 0x17001601 RID: 5633
	// (get) Token: 0x06004059 RID: 16473 RVA: 0x000E406F File Offset: 0x000E226F
	// (set) Token: 0x0600405A RID: 16474 RVA: 0x000E4077 File Offset: 0x000E2277
	public bool DisableTopNavigationNode
	{
		get
		{
			return this.m_disableTopNavNode;
		}
		set
		{
			this.m_disableTopNavNode = value;
		}
	}

	// Token: 0x17001602 RID: 5634
	// (get) Token: 0x0600405B RID: 16475 RVA: 0x000E4080 File Offset: 0x000E2280
	// (set) Token: 0x0600405C RID: 16476 RVA: 0x000E4088 File Offset: 0x000E2288
	public bool DisableBottomNavigationNode
	{
		get
		{
			return this.m_disableBottomNavNode;
		}
		set
		{
			this.m_disableBottomNavNode = value;
		}
	}

	// Token: 0x17001603 RID: 5635
	// (get) Token: 0x0600405D RID: 16477 RVA: 0x000E4091 File Offset: 0x000E2291
	// (set) Token: 0x0600405E RID: 16478 RVA: 0x000E4099 File Offset: 0x000E2299
	public bool DisableLeftNavigationNode
	{
		get
		{
			return this.m_disableLeftNavNode;
		}
		set
		{
			this.m_disableLeftNavNode = value;
		}
	}

	// Token: 0x17001604 RID: 5636
	// (get) Token: 0x0600405F RID: 16479 RVA: 0x000E40A2 File Offset: 0x000E22A2
	// (set) Token: 0x06004060 RID: 16480 RVA: 0x000E40AA File Offset: 0x000E22AA
	public bool DisableRightNavigationNode
	{
		get
		{
			return this.m_disableRightNavNode;
		}
		set
		{
			this.m_disableRightNavNode = value;
		}
	}

	// Token: 0x17001605 RID: 5637
	// (get) Token: 0x06004061 RID: 16481 RVA: 0x000E40B3 File Offset: 0x000E22B3
	// (set) Token: 0x06004062 RID: 16482 RVA: 0x000E40BB File Offset: 0x000E22BB
	public SkillTreeSlot TopNode
	{
		get
		{
			return this.m_topNode;
		}
		set
		{
			if (value == this)
			{
				throw new Exception("Cannot link navigation node to self.");
			}
			this.m_topNode = value;
		}
	}

	// Token: 0x17001606 RID: 5638
	// (get) Token: 0x06004063 RID: 16483 RVA: 0x000E40D8 File Offset: 0x000E22D8
	// (set) Token: 0x06004064 RID: 16484 RVA: 0x000E40E0 File Offset: 0x000E22E0
	public SkillTreeSlot BottomNode
	{
		get
		{
			return this.m_bottomNode;
		}
		set
		{
			if (value == this)
			{
				throw new Exception("Cannot link navigation node to self.");
			}
			this.m_bottomNode = value;
		}
	}

	// Token: 0x17001607 RID: 5639
	// (get) Token: 0x06004065 RID: 16485 RVA: 0x000E40FD File Offset: 0x000E22FD
	// (set) Token: 0x06004066 RID: 16486 RVA: 0x000E4105 File Offset: 0x000E2305
	public SkillTreeSlot LeftNode
	{
		get
		{
			return this.m_leftNode;
		}
		set
		{
			if (value == this)
			{
				throw new Exception("Cannot link navigation node to self.");
			}
			this.m_leftNode = value;
		}
	}

	// Token: 0x17001608 RID: 5640
	// (get) Token: 0x06004067 RID: 16487 RVA: 0x000E4122 File Offset: 0x000E2322
	// (set) Token: 0x06004068 RID: 16488 RVA: 0x000E412A File Offset: 0x000E232A
	public SkillTreeSlot RightNode
	{
		get
		{
			return this.m_rightNode;
		}
		set
		{
			if (value == this)
			{
				throw new Exception("Cannot link navigation node to self.");
			}
			this.m_rightNode = value;
		}
	}

	// Token: 0x17001609 RID: 5641
	// (get) Token: 0x06004069 RID: 16489 RVA: 0x000E4147 File Offset: 0x000E2347
	public Button Button
	{
		get
		{
			if (this.m_button == null)
			{
				this.m_button = base.GetComponent<Button>();
			}
			return this.m_button;
		}
	}

	// Token: 0x1700160A RID: 5642
	// (get) Token: 0x0600406A RID: 16490 RVA: 0x000E4169 File Offset: 0x000E2369
	// (set) Token: 0x0600406B RID: 16491 RVA: 0x000E4171 File Offset: 0x000E2371
	public List<SkillTreeSlot> UnlockSlotList
	{
		get
		{
			return this.m_unlockSlotList;
		}
		set
		{
			this.m_unlockSlotList = value;
		}
	}

	// Token: 0x1700160B RID: 5643
	// (get) Token: 0x0600406C RID: 16492 RVA: 0x000E417A File Offset: 0x000E237A
	// (set) Token: 0x0600406D RID: 16493 RVA: 0x000E4182 File Offset: 0x000E2382
	public List<bool> UnlockSlotDisableList
	{
		get
		{
			return this.m_unlockSlotDisableList;
		}
		set
		{
			this.m_unlockSlotDisableList = value;
		}
	}

	// Token: 0x1700160C RID: 5644
	// (get) Token: 0x0600406E RID: 16494 RVA: 0x000E418B File Offset: 0x000E238B
	// (set) Token: 0x0600406F RID: 16495 RVA: 0x000E4193 File Offset: 0x000E2393
	public bool IsInitialized { get; private set; }

	// Token: 0x06004070 RID: 16496 RVA: 0x000E419C File Offset: 0x000E239C
	public void Initialize(int slotIndex, Animator castleAnimator)
	{
		this.m_storedScale = base.transform.localScale;
		this.SlotIndex = slotIndex;
		this.m_button = base.GetComponent<Button>();
		this.m_highlightGO.SetActive(false);
		this.m_canBeUpgradedImage.gameObject.SetActive(false);
		if (this.m_skillTreeType != SkillTreeType.None)
		{
			this.m_skillTreeData = SkillTreeLibrary.GetSkillTreeData(this.m_skillTreeType);
			this.m_castleAnimator = castleAnimator;
		}
		Navigation navigation = this.m_button.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		this.m_button.navigation = navigation;
		this.RefreshSlotState(false);
		this.IsInitialized = true;
	}

	// Token: 0x06004071 RID: 16497 RVA: 0x000E4238 File Offset: 0x000E2438
	public void UpdateSkillTreeType()
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(this.SkillTreeType);
		if (Application.isPlaying)
		{
			if (skillTreeObj.IsLocked)
			{
				this.m_iconImage.sprite = IconLibrary.GetSkillTreeLockedIcon();
			}
			else if (skillTreeObj.IsSoulLocked)
			{
				this.m_iconImage.sprite = IconLibrary.GetSkillTreeSoulLockedIcon();
			}
			else
			{
				this.m_iconImage.sprite = IconLibrary.GetSkillTreeIcon(this.SkillTreeType);
			}
		}
		else
		{
			Sprite skillTreeIcon = IconLibrary.GetSkillTreeIcon(this.SkillTreeType);
			if (skillTreeIcon != IconLibrary.GetDefaultSprite())
			{
				this.m_iconImage.sprite = skillTreeIcon;
			}
		}
		base.name = "SkillTreeSlot_" + this.SkillTreeType.ToString();
	}

	// Token: 0x06004072 RID: 16498 RVA: 0x000E42F0 File Offset: 0x000E24F0
	public void PurchaseSkillUpgrade(int numLevels)
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(this.SkillTreeType);
		if (skillTreeObj == null || skillTreeObj.IsLocked || skillTreeObj.IsLevelLocked || skillTreeObj.IsSoulLocked)
		{
			base.StartCoroutine(this.ShakeAnimCoroutine());
			this.UpgradeFailedRelay.Dispatch();
			return;
		}
		int levelsToAdd = Mathf.Clamp(numLevels, 0, skillTreeObj.MaxLevel - skillTreeObj.Level);
		int num = skillTreeObj.GetNumLevelsPurchaseableWithGold(levelsToAdd, SaveManager.PlayerSaveData.GoldCollectedIncludingBank);
		if (num > 0)
		{
			num = Mathf.Max(1, num);
			for (int i = 0; i < num; i++)
			{
				if (skillTreeObj.Level < skillTreeObj.MaxLevel)
				{
					int goldCostWithLevelAppreciation = skillTreeObj.GoldCostWithLevelAppreciation;
					int goldCollectedIncludingBank = SaveManager.PlayerSaveData.GoldCollectedIncludingBank;
					SaveManager.PlayerSaveData.SubtractFromGoldIncludingBank(goldCostWithLevelAppreciation);
					SaveManager.PlayerSaveData.GoldSpent += goldCostWithLevelAppreciation;
					SaveManager.PlayerSaveData.GoldSpentOnSkills += goldCostWithLevelAppreciation;
					SkillTreeSlot.m_goldChangedArgs.Initialize(goldCollectedIncludingBank, goldCollectedIncludingBank + goldCostWithLevelAppreciation);
					Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, this, SkillTreeSlot.m_goldChangedArgs);
					bool flag = i >= num - 1;
					if (!SkillTreeManager.SetSkillObjLevel(this.SkillTreeType, 1, true, flag, false))
					{
						throw new Exception("Failed to upgrade skill: " + this.SkillTreeType.ToString() + " in skill tree. This should never happen.");
					}
					Debug.Log("Successfully leveled up " + skillTreeObj.SkillTreeType.ToString());
					if (flag)
					{
						if (skillTreeObj.Level < skillTreeObj.MaxLevel)
						{
							this.UpgradedRelay.Dispatch();
						}
						else
						{
							this.FullyUpgradedRelay.Dispatch();
						}
					}
					if ((WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController).HasAllSkills())
					{
						StoreAPIManager.GiveAchievement(AchievementType.AllSkills, StoreType.All);
					}
				}
				else
				{
					Debug.Log("Cannot level up " + skillTreeObj.SkillTreeType.ToString() + ".  Reason: Skill already max level.");
					base.StartCoroutine(this.ShakeAnimCoroutine());
					this.UpgradeFailedRelay.Dispatch();
				}
			}
			return;
		}
		base.StartCoroutine(this.ShakeAnimCoroutine());
		this.UpgradeFailedRelay.Dispatch();
	}

	// Token: 0x06004073 RID: 16499 RVA: 0x000E450D File Offset: 0x000E270D
	private IEnumerator ShakeAnimCoroutine()
	{
		base.transform.localScale = this.m_storedScale + new Vector3(0.1f, 0.1f, 0.1f);
		TweenManager.TweenTo_UnscaledTime(base.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			this.m_storedScale.x,
			"localScale.y",
			this.m_storedScale.y,
			"localScale.z",
			this.m_storedScale.z
		});
		int shakeCount = 0;
		float z = 5f;
		float shakeDelay = 0.05f;
		float shakeTime = Time.time + shakeDelay;
		Vector3 shakeEuler = base.transform.localEulerAngles;
		shakeEuler.z = z;
		base.transform.localEulerAngles = shakeEuler;
		while (shakeCount < 3)
		{
			if (Time.time >= shakeTime)
			{
				shakeEuler.z *= -1f;
				base.transform.localEulerAngles = shakeEuler;
				shakeTime = Time.time + shakeDelay;
				int num = shakeCount;
				shakeCount = num + 1;
			}
			yield return null;
		}
		shakeEuler.z = 0f;
		base.transform.localEulerAngles = shakeEuler;
		yield break;
	}

	// Token: 0x06004074 RID: 16500 RVA: 0x000E451C File Offset: 0x000E271C
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && !SkillTreeWindowController.CastleViewEnabled)
		{
			this.PurchaseSkillUpgrade(1);
		}
	}

	// Token: 0x06004075 RID: 16501 RVA: 0x000E4534 File Offset: 0x000E2734
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Mouse)
		{
			return;
		}
		if (!SkillTreeWindowController.CastleViewEnabled)
		{
			this.OnSelect(eventData);
		}
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x000E454D File Offset: 0x000E274D
	public void OnSelect(BaseEventData eventData)
	{
		if (SkillTreeWindowController.CastleViewEnabled)
		{
			return;
		}
		if (this.m_button && this.m_button.interactable)
		{
			this.Select(true);
		}
	}

	// Token: 0x06004077 RID: 16503 RVA: 0x000E4578 File Offset: 0x000E2778
	public void Select(bool dispatchSelectedEvent = true)
	{
		if (this.m_selected)
		{
			return;
		}
		if (this.m_button.interactable)
		{
			this.m_selected = true;
			this.m_button.Select();
			this.m_highlightGO.SetActive(true);
			base.transform.localScale = this.m_storedScale + new Vector3(0.1f, 0.1f, 0.1f);
			TweenManager.TweenTo_UnscaledTime(base.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
			{
				"localScale.x",
				this.m_storedScale.x,
				"localScale.y",
				this.m_storedScale.y,
				"localScale.z",
				this.m_storedScale.z
			});
			SkillTreeSlot.m_highlightedSkillChangedArgs.Initialize(this.SkillTreeType);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.SkillTree_HighlightedSkillChanged, this, SkillTreeSlot.m_highlightedSkillChangedArgs);
			if (dispatchSelectedEvent)
			{
				this.SelectedRelay.Dispatch();
				return;
			}
		}
		else
		{
			Debug.LogFormat("<color=red>{0}: You attempted to select a non interactable Skill Tree Slot ({1})</color>", new object[]
			{
				Time.frameCount,
				this.m_skillTreeType
			});
		}
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x000E46B4 File Offset: 0x000E28B4
	public void OnDeselect(BaseEventData eventData)
	{
		this.m_selected = false;
		this.m_highlightGO.SetActive(false);
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = 0f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x000E46F8 File Offset: 0x000E28F8
	private void OnDisable()
	{
		this.m_selected = false;
		this.m_highlightGO.SetActive(false);
		this.m_alreadyUnlockedEventPlayed = false;
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x000E4714 File Offset: 0x000E2914
	public IEnumerator AnimateBGImage()
	{
		this.HasAnimated = true;
		Vector3 effectPosition = base.transform.position + this.AnimEffectOffset;
		effectPosition.z = -10f;
		if (!string.IsNullOrEmpty(this.BGAnimatorParam))
		{
			SkillTreeSlot.SkillTreeAnimType animType = this.m_animType;
			if (animType <= SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Wooden)
			{
				if (animType <= SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Wooden)
				{
					if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Foundation)
					{
						if (animType == SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Tower)
						{
							goto IL_13D;
						}
						if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Wooden)
						{
							goto IL_213;
						}
						goto IL_157;
					}
				}
				else if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Foundation)
				{
					if (animType == SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Tower)
					{
						goto IL_13D;
					}
					if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Wooden)
					{
						goto IL_213;
					}
					goto IL_157;
				}
				AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildFoundation", default(Vector3));
				goto IL_213;
				IL_13D:
				AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildTower", default(Vector3));
				goto IL_213;
				IL_157:
				AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_wooden_structure", default(Vector3));
			}
			else if (animType <= SkillTreeSlot.SkillTreeAnimType.Forest_Grow)
			{
				if (animType != SkillTreeSlot.SkillTreeAnimType.Cave)
				{
					if (animType != SkillTreeSlot.SkillTreeAnimType.Forest_Foundation)
					{
						if (animType == SkillTreeSlot.SkillTreeAnimType.Forest_Grow)
						{
							AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_garden_expand", default(Vector3));
						}
					}
					else
					{
						AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_tree_grow", default(Vector3));
					}
				}
				else
				{
					AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_digDeep", default(Vector3));
				}
			}
			else if (animType <= SkillTreeSlot.SkillTreeAnimType.WeatherVane)
			{
				if (animType != SkillTreeSlot.SkillTreeAnimType.FlagPole)
				{
					if (animType == SkillTreeSlot.SkillTreeAnimType.WeatherVane)
					{
						AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole", default(Vector3));
					}
				}
				else
				{
					AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole", default(Vector3));
				}
			}
			else if (animType != SkillTreeSlot.SkillTreeAnimType.BalconyOrSign)
			{
				if (animType == SkillTreeSlot.SkillTreeAnimType.TireSwing)
				{
					AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole", default(Vector3));
				}
			}
			else
			{
				AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_balcony_small", default(Vector3));
			}
			IL_213:
			animType = this.m_animType;
			if (animType > SkillTreeSlot.SkillTreeAnimType.Cave)
			{
				if (animType <= SkillTreeSlot.SkillTreeAnimType.FlagPole)
				{
					if (animType == SkillTreeSlot.SkillTreeAnimType.Forest_Foundation)
					{
						goto IL_2EB;
					}
					if (animType != SkillTreeSlot.SkillTreeAnimType.Forest_Grow && animType != SkillTreeSlot.SkillTreeAnimType.FlagPole)
					{
						goto IL_3DD;
					}
				}
				else if (animType <= SkillTreeSlot.SkillTreeAnimType.BalconyOrSign)
				{
					if (animType != SkillTreeSlot.SkillTreeAnimType.WeatherVane && animType != SkillTreeSlot.SkillTreeAnimType.BalconyOrSign)
					{
						goto IL_3DD;
					}
				}
				else if (animType != SkillTreeSlot.SkillTreeAnimType.TireSwing && animType != SkillTreeSlot.SkillTreeAnimType.CameraShakeOnly)
				{
					goto IL_3DD;
				}
				EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "CameraShakeVerySmall_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				goto IL_3DD;
			}
			if (animType > SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Wooden)
			{
				if (animType <= SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Tower)
				{
					if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Foundation && animType != SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Tower)
					{
						goto IL_3DD;
					}
				}
				else if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingVertical_Wooden)
				{
					if (animType != SkillTreeSlot.SkillTreeAnimType.Cave)
					{
						goto IL_3DD;
					}
					EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "CameraShakeVerySmall_Effect", Vector3.zero, 0.6f, EffectStopType.Gracefully, EffectTriggerDirection.None);
					EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "SkillTreeCastle_CaveAppear_Effect", effectPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
					goto IL_3DD;
				}
				EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "CameraShakeVerySmall_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "SkillTreeCastle_VerticalDust_Effect", effectPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				goto IL_3DD;
			}
			if (animType != SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Foundation && animType != SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Tower && animType != SkillTreeSlot.SkillTreeAnimType.CastleWingHorizontal_Wooden)
			{
				goto IL_3DD;
			}
			IL_2EB:
			EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "CameraShakeVerySmall_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			EffectManager.PlayEffect(base.gameObject, this.m_castleAnimator, "SkillTreeCastle_HorizontalDust_Effect", effectPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			IL_3DD:
			this.m_castleAnimator.SetBool(this.BGAnimatorParam, false);
			this.m_castleAnimator.Update(1f);
			this.m_castleAnimator.SetBool(this.BGAnimatorParam, true);
			float delay = Time.time + 1.5f;
			while (Time.time < delay)
			{
				yield return null;
			}
			AudioManager.PlayOneShot(null, "event:/SFX/UpgradingCastle/sfx_upgrade_castle_constructionEnd", default(Vector3));
		}
		this.m_alreadyUnlockedEventPlayed = true;
		yield break;
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x000E4724 File Offset: 0x000E2924
	public void RefreshSlotState(bool updateAnimParams)
	{
		this.m_iconCanvasGroup.alpha = 1f;
		if (this.m_newIndicator.gameObject.activeSelf)
		{
			this.m_newIndicator.gameObject.SetActive(false);
		}
		if (this.m_levelLockGO.activeSelf)
		{
			this.m_levelLockGO.SetActive(false);
		}
		if (this.SkillTreeType == SkillTreeType.None)
		{
			return;
		}
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(this.SkillTreeType);
		if (skillTreeObj.Level == 0 && !skillTreeObj.IsLocked && !skillTreeObj.IsSoulLocked)
		{
			this.m_iconCanvasGroup.alpha = 0.5f;
		}
		if (this.HasAnimParam && updateAnimParams)
		{
			this.UpdateAnimatorParams();
		}
		int maxLevel = skillTreeObj.MaxLevel;
		int clampedLevel = skillTreeObj.ClampedLevel;
		this.m_levelText.text = string.Format("{0} / {1}", clampedLevel, maxLevel);
		if (!skillTreeObj.IsLocked && !skillTreeObj.IsSoulLocked)
		{
			this.m_levelText.gameObject.SetActive(true);
		}
		else
		{
			this.m_levelText.gameObject.SetActive(false);
		}
		if (clampedLevel >= maxLevel)
		{
			this.m_iconFrame.overrideSprite = this.m_goldFrameSprite;
		}
		else
		{
			this.m_iconFrame.overrideSprite = null;
		}
		if (skillTreeObj != null && !skillTreeObj.IsLocked && !skillTreeObj.IsLevelLocked && !skillTreeObj.IsSoulLocked && clampedLevel < maxLevel && SaveManager.PlayerSaveData.GoldCollectedIncludingBank >= skillTreeObj.GoldCostWithLevelAppreciation && !this.m_newIndicator.gameObject.activeSelf)
		{
			this.m_newIndicator.gameObject.SetActive(true);
		}
		if (skillTreeObj != null && skillTreeObj.IsLevelLocked)
		{
			this.m_levelLockText.text = skillTreeObj.UnlockLevel.ToString();
			this.m_levelLockGO.SetActive(true);
		}
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x000E48D8 File Offset: 0x000E2AD8
	public void UpdateAnimatorParams()
	{
		if (SkillTreeManager.GetSkillTreeObj(this.SkillTreeType).Level > 0)
		{
			this.m_castleAnimator.SetBool(this.BGAnimatorParam, true);
			this.m_castleAnimator.Update(2f);
			this.HasAnimated = true;
			if (!this.m_alreadyUnlockedEventPlayed)
			{
				this.m_alreadyUnlockedEventPlayed = true;
				this.OnAlreadyUnlockedEvent.Invoke();
				return;
			}
		}
		else
		{
			this.m_castleAnimator.SetBool(this.BGAnimatorParam, false);
		}
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x000E4950 File Offset: 0x000E2B50
	public void UnlockConnectedSkillSlots()
	{
		for (int i = 0; i < this.m_unlockSlotList.Count; i++)
		{
			SkillTreeSlot skillTreeSlot = this.m_unlockSlotList[i];
			bool flag = this.m_unlockSlotDisableList[i];
			if (skillTreeSlot && !flag && skillTreeSlot.HasData)
			{
				SkillUnlockState skillUnlockState = skillTreeSlot.m_skillTreeData.SkillUnlockState;
				if (skillUnlockState <= SkillUnlockState.Locked)
				{
					skillTreeSlot.gameObject.SetActive(true);
				}
			}
		}
		this.UpdateNavigationNodes();
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x000E49C4 File Offset: 0x000E2BC4
	public void UpdateNavigationNodes()
	{
		Navigation navigation = this.m_button.navigation;
		int recursionCount = 0;
		SkillTreeSlot skillTreeSlot = this.RecursiveNodeSearch(SkillTreeSlot.NodeDirection.Up, recursionCount);
		if (skillTreeSlot && !this.DisableTopNavigationNode)
		{
			navigation.selectOnUp = skillTreeSlot.Button;
		}
		recursionCount = 0;
		SkillTreeSlot skillTreeSlot2 = this.RecursiveNodeSearch(SkillTreeSlot.NodeDirection.Down, recursionCount);
		if (skillTreeSlot2 && !this.DisableBottomNavigationNode)
		{
			navigation.selectOnDown = skillTreeSlot2.Button;
		}
		recursionCount = 0;
		SkillTreeSlot skillTreeSlot3 = this.RecursiveNodeSearch(SkillTreeSlot.NodeDirection.Left, recursionCount);
		if (skillTreeSlot3 && !this.DisableLeftNavigationNode)
		{
			navigation.selectOnLeft = skillTreeSlot3.Button;
		}
		recursionCount = 0;
		SkillTreeSlot skillTreeSlot4 = this.RecursiveNodeSearch(SkillTreeSlot.NodeDirection.Right, recursionCount);
		if (skillTreeSlot4 && !this.DisableRightNavigationNode)
		{
			navigation.selectOnRight = skillTreeSlot4.Button;
		}
		this.m_button.navigation = navigation;
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x000E4A90 File Offset: 0x000E2C90
	public SkillTreeSlot RecursiveNodeSearch(SkillTreeSlot.NodeDirection direction, int recursionCount)
	{
		recursionCount++;
		if (recursionCount > 20)
		{
			throw new Exception("No: " + direction.ToString() + " " + this.m_skillTreeType.ToString());
		}
		SkillTreeSlot result = null;
		switch (direction)
		{
		case SkillTreeSlot.NodeDirection.Up:
			if (this.m_topNode)
			{
				result = ((this.m_topNode.isActiveAndEnabled && this.m_topNode.HasData) ? this.m_topNode : this.m_topNode.RecursiveNodeSearch(direction, recursionCount));
			}
			break;
		case SkillTreeSlot.NodeDirection.Down:
			if (this.m_bottomNode)
			{
				result = ((this.m_bottomNode.isActiveAndEnabled && this.m_bottomNode.HasData) ? this.m_bottomNode : this.m_bottomNode.RecursiveNodeSearch(direction, recursionCount));
			}
			break;
		case SkillTreeSlot.NodeDirection.Left:
			if (this.m_leftNode)
			{
				result = ((this.m_leftNode.isActiveAndEnabled && this.m_leftNode.HasData) ? this.m_leftNode : this.m_leftNode.RecursiveNodeSearch(direction, recursionCount));
			}
			break;
		case SkillTreeSlot.NodeDirection.Right:
			if (this.m_rightNode)
			{
				result = ((this.m_rightNode.isActiveAndEnabled && this.m_rightNode.HasData) ? this.m_rightNode : this.m_rightNode.RecursiveNodeSearch(direction, recursionCount));
			}
			break;
		}
		return result;
	}

	// Token: 0x0400318C RID: 12684
	private const string CAVE_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_digDeep";

	// Token: 0x0400318D RID: 12685
	private const string FOUNDATION_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildFoundation";

	// Token: 0x0400318E RID: 12686
	private const string TOWER_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildTower";

	// Token: 0x0400318F RID: 12687
	private const string WOODEN_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_wooden_structure";

	// Token: 0x04003190 RID: 12688
	private const string FOREST_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_tree_grow";

	// Token: 0x04003191 RID: 12689
	private const string FOREST_GROW_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_garden_expand";

	// Token: 0x04003192 RID: 12690
	private const string FLAGPOLE_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x04003193 RID: 12691
	private const string WEATHERVANE_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x04003194 RID: 12692
	private const string BALCONY_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_balcony_small";

	// Token: 0x04003195 RID: 12693
	private const string TIRESWING_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x04003196 RID: 12694
	private const string CONSTRUCTION_END_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_constructionEnd";

	// Token: 0x04003197 RID: 12695
	private const string VERY_SMALL_CAMERASHAKE_FX = "CameraShakeVerySmall_Effect";

	// Token: 0x04003198 RID: 12696
	private const float CAVE_CAMERASHAKE_DURATION = 0.6f;

	// Token: 0x04003199 RID: 12697
	private const float TOWER_CAMERASHAKE_DURATION = 1f;

	// Token: 0x0400319A RID: 12698
	private const string CAVE_APPEAR_FX = "SkillTreeCastle_CaveAppear_Effect";

	// Token: 0x0400319B RID: 12699
	private const string HORIZONTAL_TOWER_APPEAR_FX = "SkillTreeCastle_HorizontalDust_Effect";

	// Token: 0x0400319C RID: 12700
	private const string VERTICAL_TOWER_APPEAR_FX = "SkillTreeCastle_VerticalDust_Effect";

	// Token: 0x0400319D RID: 12701
	[SerializeField]
	private SkillTreeType m_skillTreeType;

	// Token: 0x0400319E RID: 12702
	[Header("Animations")]
	[SerializeField]
	private string m_bgAnimatorParam;

	// Token: 0x0400319F RID: 12703
	[SerializeField]
	private SkillTreeSlot.SkillTreeAnimType m_animType;

	// Token: 0x040031A0 RID: 12704
	[SerializeField]
	private Vector2 m_animEffectOffset;

	// Token: 0x040031A1 RID: 12705
	[Header("Icons")]
	[SerializeField]
	private CanvasGroup m_iconCanvasGroup;

	// Token: 0x040031A2 RID: 12706
	[SerializeField]
	private Image m_iconImage;

	// Token: 0x040031A3 RID: 12707
	[SerializeField]
	private Image m_iconFrame;

	// Token: 0x040031A4 RID: 12708
	[SerializeField]
	private GameObject m_highlightGO;

	// Token: 0x040031A5 RID: 12709
	[SerializeField]
	private Image m_canBeUpgradedImage;

	// Token: 0x040031A6 RID: 12710
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x040031A7 RID: 12711
	[Header("Navigation Nodes")]
	[SerializeField]
	private SkillTreeSlot m_leftNode;

	// Token: 0x040031A8 RID: 12712
	[SerializeField]
	private SkillTreeSlot m_rightNode;

	// Token: 0x040031A9 RID: 12713
	[SerializeField]
	private SkillTreeSlot m_topNode;

	// Token: 0x040031AA RID: 12714
	[SerializeField]
	private SkillTreeSlot m_bottomNode;

	// Token: 0x040031AB RID: 12715
	[SerializeField]
	private Image m_newIndicator;

	// Token: 0x040031AC RID: 12716
	[SerializeField]
	private GameObject m_levelLockGO;

	// Token: 0x040031AD RID: 12717
	[SerializeField]
	private TMP_Text m_levelLockText;

	// Token: 0x040031AE RID: 12718
	[SerializeField]
	private bool m_disableTopNavNode;

	// Token: 0x040031AF RID: 12719
	[SerializeField]
	private bool m_disableBottomNavNode;

	// Token: 0x040031B0 RID: 12720
	[SerializeField]
	private bool m_disableLeftNavNode;

	// Token: 0x040031B1 RID: 12721
	[SerializeField]
	private bool m_disableRightNavNode;

	// Token: 0x040031B2 RID: 12722
	[Header("Unlockable Nodes")]
	[SerializeField]
	[ReadOnly]
	private List<SkillTreeSlot> m_unlockSlotList;

	// Token: 0x040031B3 RID: 12723
	[SerializeField]
	[ReadOnly]
	private List<bool> m_unlockSlotDisableList;

	// Token: 0x040031B4 RID: 12724
	[Header("Debug")]
	[SerializeField]
	private Sprite m_hasNoLinkedDataSprite;

	// Token: 0x040031B5 RID: 12725
	[SerializeField]
	private Sprite m_goldFrameSprite;

	// Token: 0x040031B6 RID: 12726
	private Button m_button;

	// Token: 0x040031B7 RID: 12727
	private static HighlightedSkillChangedEventArgs m_highlightedSkillChangedArgs = new HighlightedSkillChangedEventArgs(SkillTreeType.None);

	// Token: 0x040031B8 RID: 12728
	private static GoldChangedEventArgs m_goldChangedArgs = new GoldChangedEventArgs(0, 0);

	// Token: 0x040031B9 RID: 12729
	private SkillTreeData m_skillTreeData;

	// Token: 0x040031BA RID: 12730
	private Animator m_castleAnimator;

	// Token: 0x040031BB RID: 12731
	private Vector3 m_storedScale;

	// Token: 0x040031BC RID: 12732
	private bool m_alreadyUnlockedEventPlayed;

	// Token: 0x040031BD RID: 12733
	private bool m_selected;

	// Token: 0x040031BE RID: 12734
	public Relay UpgradedRelay = new Relay();

	// Token: 0x040031BF RID: 12735
	public Relay UpgradeFailedRelay = new Relay();

	// Token: 0x040031C0 RID: 12736
	public Relay FullyUpgradedRelay = new Relay();

	// Token: 0x040031C1 RID: 12737
	public Relay SelectedRelay = new Relay();

	// Token: 0x040031C3 RID: 12739
	public UnityEvent OnAlreadyUnlockedEvent;

	// Token: 0x02000E23 RID: 3619
	public enum NodeDirection
	{
		// Token: 0x040056DF RID: 22239
		Up,
		// Token: 0x040056E0 RID: 22240
		Down,
		// Token: 0x040056E1 RID: 22241
		Left,
		// Token: 0x040056E2 RID: 22242
		Right
	}

	// Token: 0x02000E24 RID: 3620
	public enum SkillTreeAnimType
	{
		// Token: 0x040056E4 RID: 22244
		None,
		// Token: 0x040056E5 RID: 22245
		CastleWingHorizontal_Foundation = 100,
		// Token: 0x040056E6 RID: 22246
		CastleWingHorizontal_Tower = 110,
		// Token: 0x040056E7 RID: 22247
		CastleWingHorizontal_Wooden = 120,
		// Token: 0x040056E8 RID: 22248
		CastleWingVertical_Foundation = 200,
		// Token: 0x040056E9 RID: 22249
		CastleWingVertical_Tower = 210,
		// Token: 0x040056EA RID: 22250
		CastleWingVertical_Wooden = 220,
		// Token: 0x040056EB RID: 22251
		Cave = 300,
		// Token: 0x040056EC RID: 22252
		Forest_Foundation = 400,
		// Token: 0x040056ED RID: 22253
		Forest_Grow = 410,
		// Token: 0x040056EE RID: 22254
		FlagPole = 450,
		// Token: 0x040056EF RID: 22255
		WeatherVane = 460,
		// Token: 0x040056F0 RID: 22256
		BalconyOrSign = 470,
		// Token: 0x040056F1 RID: 22257
		TireSwing = 480,
		// Token: 0x040056F2 RID: 22258
		CameraShakeOnly = 500
	}
}
