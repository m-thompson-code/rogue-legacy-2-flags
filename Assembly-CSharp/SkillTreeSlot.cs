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

// Token: 0x02000BA1 RID: 2977
public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x17001DF0 RID: 7664
	// (get) Token: 0x06005987 RID: 22919 RVA: 0x00030CB0 File Offset: 0x0002EEB0
	// (set) Token: 0x06005988 RID: 22920 RVA: 0x00030CB8 File Offset: 0x0002EEB8
	public bool HasAnimated { get; private set; }

	// Token: 0x17001DF1 RID: 7665
	// (get) Token: 0x06005989 RID: 22921 RVA: 0x00030CC1 File Offset: 0x0002EEC1
	public Vector3 StoredScale
	{
		get
		{
			return this.m_storedScale;
		}
	}

	// Token: 0x17001DF2 RID: 7666
	// (get) Token: 0x0600598A RID: 22922 RVA: 0x00030CC9 File Offset: 0x0002EEC9
	public bool HasAnimParam
	{
		get
		{
			return !string.IsNullOrEmpty(this.m_bgAnimatorParam);
		}
	}

	// Token: 0x17001DF3 RID: 7667
	// (get) Token: 0x0600598B RID: 22923 RVA: 0x00030CD9 File Offset: 0x0002EED9
	// (set) Token: 0x0600598C RID: 22924 RVA: 0x00030CE1 File Offset: 0x0002EEE1
	public int SlotIndex { get; private set; }

	// Token: 0x17001DF4 RID: 7668
	// (get) Token: 0x0600598D RID: 22925 RVA: 0x00030CEA File Offset: 0x0002EEEA
	// (set) Token: 0x0600598E RID: 22926 RVA: 0x00030CF2 File Offset: 0x0002EEF2
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

	// Token: 0x17001DF5 RID: 7669
	// (get) Token: 0x0600598F RID: 22927 RVA: 0x00030D0F File Offset: 0x0002EF0F
	// (set) Token: 0x06005990 RID: 22928 RVA: 0x00030D17 File Offset: 0x0002EF17
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

	// Token: 0x17001DF6 RID: 7670
	// (get) Token: 0x06005991 RID: 22929 RVA: 0x00030D20 File Offset: 0x0002EF20
	// (set) Token: 0x06005992 RID: 22930 RVA: 0x00030D28 File Offset: 0x0002EF28
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

	// Token: 0x17001DF7 RID: 7671
	// (get) Token: 0x06005993 RID: 22931 RVA: 0x00030D31 File Offset: 0x0002EF31
	// (set) Token: 0x06005994 RID: 22932 RVA: 0x00030D39 File Offset: 0x0002EF39
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

	// Token: 0x17001DF8 RID: 7672
	// (get) Token: 0x06005995 RID: 22933 RVA: 0x00030D42 File Offset: 0x0002EF42
	public bool HasData
	{
		get
		{
			return this.m_skillTreeType > SkillTreeType.None;
		}
	}

	// Token: 0x17001DF9 RID: 7673
	// (get) Token: 0x06005996 RID: 22934 RVA: 0x00030D4D File Offset: 0x0002EF4D
	// (set) Token: 0x06005997 RID: 22935 RVA: 0x00030D55 File Offset: 0x0002EF55
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

	// Token: 0x17001DFA RID: 7674
	// (get) Token: 0x06005998 RID: 22936 RVA: 0x00030D5E File Offset: 0x0002EF5E
	// (set) Token: 0x06005999 RID: 22937 RVA: 0x00030D66 File Offset: 0x0002EF66
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

	// Token: 0x17001DFB RID: 7675
	// (get) Token: 0x0600599A RID: 22938 RVA: 0x00030D6F File Offset: 0x0002EF6F
	// (set) Token: 0x0600599B RID: 22939 RVA: 0x00030D77 File Offset: 0x0002EF77
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

	// Token: 0x17001DFC RID: 7676
	// (get) Token: 0x0600599C RID: 22940 RVA: 0x00030D80 File Offset: 0x0002EF80
	// (set) Token: 0x0600599D RID: 22941 RVA: 0x00030D88 File Offset: 0x0002EF88
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

	// Token: 0x17001DFD RID: 7677
	// (get) Token: 0x0600599E RID: 22942 RVA: 0x00030D91 File Offset: 0x0002EF91
	// (set) Token: 0x0600599F RID: 22943 RVA: 0x00030D99 File Offset: 0x0002EF99
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

	// Token: 0x17001DFE RID: 7678
	// (get) Token: 0x060059A0 RID: 22944 RVA: 0x00030DB6 File Offset: 0x0002EFB6
	// (set) Token: 0x060059A1 RID: 22945 RVA: 0x00030DBE File Offset: 0x0002EFBE
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

	// Token: 0x17001DFF RID: 7679
	// (get) Token: 0x060059A2 RID: 22946 RVA: 0x00030DDB File Offset: 0x0002EFDB
	// (set) Token: 0x060059A3 RID: 22947 RVA: 0x00030DE3 File Offset: 0x0002EFE3
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

	// Token: 0x17001E00 RID: 7680
	// (get) Token: 0x060059A4 RID: 22948 RVA: 0x00030E00 File Offset: 0x0002F000
	// (set) Token: 0x060059A5 RID: 22949 RVA: 0x00030E08 File Offset: 0x0002F008
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

	// Token: 0x17001E01 RID: 7681
	// (get) Token: 0x060059A6 RID: 22950 RVA: 0x00030E25 File Offset: 0x0002F025
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

	// Token: 0x17001E02 RID: 7682
	// (get) Token: 0x060059A7 RID: 22951 RVA: 0x00030E47 File Offset: 0x0002F047
	// (set) Token: 0x060059A8 RID: 22952 RVA: 0x00030E4F File Offset: 0x0002F04F
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

	// Token: 0x17001E03 RID: 7683
	// (get) Token: 0x060059A9 RID: 22953 RVA: 0x00030E58 File Offset: 0x0002F058
	// (set) Token: 0x060059AA RID: 22954 RVA: 0x00030E60 File Offset: 0x0002F060
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

	// Token: 0x17001E04 RID: 7684
	// (get) Token: 0x060059AB RID: 22955 RVA: 0x00030E69 File Offset: 0x0002F069
	// (set) Token: 0x060059AC RID: 22956 RVA: 0x00030E71 File Offset: 0x0002F071
	public bool IsInitialized { get; private set; }

	// Token: 0x060059AD RID: 22957 RVA: 0x00153628 File Offset: 0x00151828
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

	// Token: 0x060059AE RID: 22958 RVA: 0x001536C4 File Offset: 0x001518C4
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

	// Token: 0x060059AF RID: 22959 RVA: 0x0015377C File Offset: 0x0015197C
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

	// Token: 0x060059B0 RID: 22960 RVA: 0x00030E7A File Offset: 0x0002F07A
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

	// Token: 0x060059B1 RID: 22961 RVA: 0x00030E89 File Offset: 0x0002F089
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && !SkillTreeWindowController.CastleViewEnabled)
		{
			this.PurchaseSkillUpgrade(1);
		}
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x00030EA1 File Offset: 0x0002F0A1
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

	// Token: 0x060059B3 RID: 22963 RVA: 0x00030EBA File Offset: 0x0002F0BA
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

	// Token: 0x060059B4 RID: 22964 RVA: 0x0015399C File Offset: 0x00151B9C
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

	// Token: 0x060059B5 RID: 22965 RVA: 0x00153AD8 File Offset: 0x00151CD8
	public void OnDeselect(BaseEventData eventData)
	{
		this.m_selected = false;
		this.m_highlightGO.SetActive(false);
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = 0f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x00030EE5 File Offset: 0x0002F0E5
	private void OnDisable()
	{
		this.m_selected = false;
		this.m_highlightGO.SetActive(false);
		this.m_alreadyUnlockedEventPlayed = false;
	}

	// Token: 0x060059B7 RID: 22967 RVA: 0x00030F01 File Offset: 0x0002F101
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

	// Token: 0x060059B8 RID: 22968 RVA: 0x00153B1C File Offset: 0x00151D1C
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

	// Token: 0x060059B9 RID: 22969 RVA: 0x00153CD0 File Offset: 0x00151ED0
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

	// Token: 0x060059BA RID: 22970 RVA: 0x00153D48 File Offset: 0x00151F48
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

	// Token: 0x060059BB RID: 22971 RVA: 0x00153DBC File Offset: 0x00151FBC
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

	// Token: 0x060059BC RID: 22972 RVA: 0x00153E88 File Offset: 0x00152088
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

	// Token: 0x040043E7 RID: 17383
	private const string CAVE_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_digDeep";

	// Token: 0x040043E8 RID: 17384
	private const string FOUNDATION_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildFoundation";

	// Token: 0x040043E9 RID: 17385
	private const string TOWER_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_buildTower";

	// Token: 0x040043EA RID: 17386
	private const string WOODEN_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_wooden_structure";

	// Token: 0x040043EB RID: 17387
	private const string FOREST_APPEAR_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_tree_grow";

	// Token: 0x040043EC RID: 17388
	private const string FOREST_GROW_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_garden_expand";

	// Token: 0x040043ED RID: 17389
	private const string FLAGPOLE_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x040043EE RID: 17390
	private const string WEATHERVANE_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x040043EF RID: 17391
	private const string BALCONY_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_balcony_small";

	// Token: 0x040043F0 RID: 17392
	private const string TIRESWING_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_flagpole";

	// Token: 0x040043F1 RID: 17393
	private const string CONSTRUCTION_END_SFX = "event:/SFX/UpgradingCastle/sfx_upgrade_castle_constructionEnd";

	// Token: 0x040043F2 RID: 17394
	private const string VERY_SMALL_CAMERASHAKE_FX = "CameraShakeVerySmall_Effect";

	// Token: 0x040043F3 RID: 17395
	private const float CAVE_CAMERASHAKE_DURATION = 0.6f;

	// Token: 0x040043F4 RID: 17396
	private const float TOWER_CAMERASHAKE_DURATION = 1f;

	// Token: 0x040043F5 RID: 17397
	private const string CAVE_APPEAR_FX = "SkillTreeCastle_CaveAppear_Effect";

	// Token: 0x040043F6 RID: 17398
	private const string HORIZONTAL_TOWER_APPEAR_FX = "SkillTreeCastle_HorizontalDust_Effect";

	// Token: 0x040043F7 RID: 17399
	private const string VERTICAL_TOWER_APPEAR_FX = "SkillTreeCastle_VerticalDust_Effect";

	// Token: 0x040043F8 RID: 17400
	[SerializeField]
	private SkillTreeType m_skillTreeType;

	// Token: 0x040043F9 RID: 17401
	[Header("Animations")]
	[SerializeField]
	private string m_bgAnimatorParam;

	// Token: 0x040043FA RID: 17402
	[SerializeField]
	private SkillTreeSlot.SkillTreeAnimType m_animType;

	// Token: 0x040043FB RID: 17403
	[SerializeField]
	private Vector2 m_animEffectOffset;

	// Token: 0x040043FC RID: 17404
	[Header("Icons")]
	[SerializeField]
	private CanvasGroup m_iconCanvasGroup;

	// Token: 0x040043FD RID: 17405
	[SerializeField]
	private Image m_iconImage;

	// Token: 0x040043FE RID: 17406
	[SerializeField]
	private Image m_iconFrame;

	// Token: 0x040043FF RID: 17407
	[SerializeField]
	private GameObject m_highlightGO;

	// Token: 0x04004400 RID: 17408
	[SerializeField]
	private Image m_canBeUpgradedImage;

	// Token: 0x04004401 RID: 17409
	[SerializeField]
	private TMP_Text m_levelText;

	// Token: 0x04004402 RID: 17410
	[Header("Navigation Nodes")]
	[SerializeField]
	private SkillTreeSlot m_leftNode;

	// Token: 0x04004403 RID: 17411
	[SerializeField]
	private SkillTreeSlot m_rightNode;

	// Token: 0x04004404 RID: 17412
	[SerializeField]
	private SkillTreeSlot m_topNode;

	// Token: 0x04004405 RID: 17413
	[SerializeField]
	private SkillTreeSlot m_bottomNode;

	// Token: 0x04004406 RID: 17414
	[SerializeField]
	private Image m_newIndicator;

	// Token: 0x04004407 RID: 17415
	[SerializeField]
	private GameObject m_levelLockGO;

	// Token: 0x04004408 RID: 17416
	[SerializeField]
	private TMP_Text m_levelLockText;

	// Token: 0x04004409 RID: 17417
	[SerializeField]
	private bool m_disableTopNavNode;

	// Token: 0x0400440A RID: 17418
	[SerializeField]
	private bool m_disableBottomNavNode;

	// Token: 0x0400440B RID: 17419
	[SerializeField]
	private bool m_disableLeftNavNode;

	// Token: 0x0400440C RID: 17420
	[SerializeField]
	private bool m_disableRightNavNode;

	// Token: 0x0400440D RID: 17421
	[Header("Unlockable Nodes")]
	[SerializeField]
	[ReadOnly]
	private List<SkillTreeSlot> m_unlockSlotList;

	// Token: 0x0400440E RID: 17422
	[SerializeField]
	[ReadOnly]
	private List<bool> m_unlockSlotDisableList;

	// Token: 0x0400440F RID: 17423
	[Header("Debug")]
	[SerializeField]
	private Sprite m_hasNoLinkedDataSprite;

	// Token: 0x04004410 RID: 17424
	[SerializeField]
	private Sprite m_goldFrameSprite;

	// Token: 0x04004411 RID: 17425
	private Button m_button;

	// Token: 0x04004412 RID: 17426
	private static HighlightedSkillChangedEventArgs m_highlightedSkillChangedArgs = new HighlightedSkillChangedEventArgs(SkillTreeType.None);

	// Token: 0x04004413 RID: 17427
	private static GoldChangedEventArgs m_goldChangedArgs = new GoldChangedEventArgs(0, 0);

	// Token: 0x04004414 RID: 17428
	private SkillTreeData m_skillTreeData;

	// Token: 0x04004415 RID: 17429
	private Animator m_castleAnimator;

	// Token: 0x04004416 RID: 17430
	private Vector3 m_storedScale;

	// Token: 0x04004417 RID: 17431
	private bool m_alreadyUnlockedEventPlayed;

	// Token: 0x04004418 RID: 17432
	private bool m_selected;

	// Token: 0x04004419 RID: 17433
	public Relay UpgradedRelay = new Relay();

	// Token: 0x0400441A RID: 17434
	public Relay UpgradeFailedRelay = new Relay();

	// Token: 0x0400441B RID: 17435
	public Relay FullyUpgradedRelay = new Relay();

	// Token: 0x0400441C RID: 17436
	public Relay SelectedRelay = new Relay();

	// Token: 0x0400441E RID: 17438
	public UnityEvent OnAlreadyUnlockedEvent;

	// Token: 0x02000BA2 RID: 2978
	public enum NodeDirection
	{
		// Token: 0x04004422 RID: 17442
		Up,
		// Token: 0x04004423 RID: 17443
		Down,
		// Token: 0x04004424 RID: 17444
		Left,
		// Token: 0x04004425 RID: 17445
		Right
	}

	// Token: 0x02000BA3 RID: 2979
	public enum SkillTreeAnimType
	{
		// Token: 0x04004427 RID: 17447
		None,
		// Token: 0x04004428 RID: 17448
		CastleWingHorizontal_Foundation = 100,
		// Token: 0x04004429 RID: 17449
		CastleWingHorizontal_Tower = 110,
		// Token: 0x0400442A RID: 17450
		CastleWingHorizontal_Wooden = 120,
		// Token: 0x0400442B RID: 17451
		CastleWingVertical_Foundation = 200,
		// Token: 0x0400442C RID: 17452
		CastleWingVertical_Tower = 210,
		// Token: 0x0400442D RID: 17453
		CastleWingVertical_Wooden = 220,
		// Token: 0x0400442E RID: 17454
		Cave = 300,
		// Token: 0x0400442F RID: 17455
		Forest_Foundation = 400,
		// Token: 0x04004430 RID: 17456
		Forest_Grow = 410,
		// Token: 0x04004431 RID: 17457
		FlagPole = 450,
		// Token: 0x04004432 RID: 17458
		WeatherVane = 460,
		// Token: 0x04004433 RID: 17459
		BalconyOrSign = 470,
		// Token: 0x04004434 RID: 17460
		TireSwing = 480,
		// Token: 0x04004435 RID: 17461
		CameraShakeOnly = 500
	}
}
