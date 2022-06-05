using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003F2 RID: 1010
public class Breakable : MonoBehaviour, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, ITerrainOnEnterHitResponse, ITerrainOnStayHitResponse, IEffectTriggerEvent_OnDeath, IEffectTriggerEvent_OnDamage, IRootObj, IPreOnDisable, IPlayHitEffect
{
	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x06002566 RID: 9574 RVA: 0x0007BB37 File Offset: 0x00079D37
	// (set) Token: 0x06002567 RID: 9575 RVA: 0x0007BB3F File Offset: 0x00079D3F
	public ItemDropType ItemDropTypeOverride { get; set; }

	// Token: 0x17000F12 RID: 3858
	// (get) Token: 0x06002568 RID: 9576 RVA: 0x0007BB48 File Offset: 0x00079D48
	// (set) Token: 0x06002569 RID: 9577 RVA: 0x0007BB50 File Offset: 0x00079D50
	public bool IsDirectionalBreak { get; private set; }

	// Token: 0x17000F13 RID: 3859
	// (get) Token: 0x0600256A RID: 9578 RVA: 0x0007BB59 File Offset: 0x00079D59
	// (set) Token: 0x0600256B RID: 9579 RVA: 0x0007BB61 File Offset: 0x00079D61
	public bool AttackerIsOnRight
	{
		get
		{
			return this.m_attackerIsOnRight;
		}
		set
		{
			this.m_attackerIsOnRight = value;
		}
	}

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x0600256C RID: 9580 RVA: 0x0007BB6A File Offset: 0x00079D6A
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x0600256D RID: 9581 RVA: 0x0007BB6D File Offset: 0x00079D6D
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x0600256E RID: 9582 RVA: 0x0007BB70 File Offset: 0x00079D70
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x0600256F RID: 9583 RVA: 0x0007BB73 File Offset: 0x00079D73
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return this.m_spriteRenderer;
		}
	}

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x06002570 RID: 9584 RVA: 0x0007BB7C File Offset: 0x00079D7C
	public Vector3 Midpoint
	{
		get
		{
			if (this.m_spriteRenderer)
			{
				return this.m_spriteRenderer.bounds.center;
			}
			return base.transform.position;
		}
	}

	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x06002571 RID: 9585 RVA: 0x0007BBB5 File Offset: 0x00079DB5
	// (set) Token: 0x06002572 RID: 9586 RVA: 0x0007BBBD File Offset: 0x00079DBD
	public Animator AnimatorComponent
	{
		get
		{
			return this.m_animator;
		}
		private set
		{
			this.m_animator = value;
		}
	}

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x06002573 RID: 9587 RVA: 0x0007BBC8 File Offset: 0x00079DC8
	public int CurrentAnimatorHashState
	{
		get
		{
			if (this.AnimatorComponent != null)
			{
				return this.AnimatorComponent.GetCurrentAnimatorStateInfo(0).shortNameHash;
			}
			return -1;
		}
	}

	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x06002574 RID: 9588 RVA: 0x0007BBF9 File Offset: 0x00079DF9
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_onDisableRelay.link;
		}
	}

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x06002575 RID: 9589 RVA: 0x0007BC06 File Offset: 0x00079E06
	public IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay
	{
		get
		{
			return this.m_onDamageEffectTriggerRelay.link;
		}
	}

	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x06002576 RID: 9590 RVA: 0x0007BC13 File Offset: 0x00079E13
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerEventRelay.link;
		}
	}

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x06002577 RID: 9591 RVA: 0x0007BC20 File Offset: 0x00079E20
	public UnityEvent DestroyedEvent
	{
		get
		{
			return this.m_destroyedEvent;
		}
	}

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x06002578 RID: 9592 RVA: 0x0007BC28 File Offset: 0x00079E28
	public virtual bool IsBroken
	{
		get
		{
			return this.CurrentHP <= 0;
		}
	}

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x06002579 RID: 9593 RVA: 0x0007BC36 File Offset: 0x00079E36
	// (set) Token: 0x0600257A RID: 9594 RVA: 0x0007BC3E File Offset: 0x00079E3E
	public bool DoNotDisableOnDeath
	{
		get
		{
			return this.m_doNotDisableOnDeath;
		}
		private set
		{
			this.m_doNotDisableOnDeath = value;
		}
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x0600257B RID: 9595 RVA: 0x0007BC47 File Offset: 0x00079E47
	// (set) Token: 0x0600257C RID: 9596 RVA: 0x0007BC4F File Offset: 0x00079E4F
	public int CurrentHP
	{
		get
		{
			return this.m_currentHP;
		}
		protected set
		{
			this.m_currentHP = Mathf.Max(0, value);
		}
	}

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x0600257D RID: 9597 RVA: 0x0007BC5E File Offset: 0x00079E5E
	public int DefaultHP
	{
		get
		{
			return this.m_baseScaledHP;
		}
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x0007BC68 File Offset: 0x00079E68
	protected virtual void Awake()
	{
		this.AnimatorComponent = base.GetComponent<Animator>();
		this.m_spriteRenderer = base.GetComponentInChildren<SpriteRenderer>();
		if (this.m_spriteRenderer)
		{
			this.m_initialSprite = this.m_spriteRenderer.sprite;
		}
		this.m_hitEffect = base.GetComponentInChildren<BlinkPulseEffect>();
		this.m_hitboxController = base.GetComponentInChildren<IHitboxController>();
		base.tag = "Breakable";
		this.m_breakableDestroyedEventArgs = new BreakableEventArgs(this);
		if (this.AnimatorComponent)
		{
			this.IsDirectionalBreak = global::AnimatorUtility.HasParameter(this.AnimatorComponent, "OpenLeft");
		}
		this.m_prop = base.GetComponent<Prop>();
		this.ForceBrokenState(false);
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x0007BD10 File Offset: 0x00079F10
	private void OnDisable()
	{
		this.ItemDropTypeOverride = ItemDropType.None;
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x0007BD19 File Offset: 0x00079F19
	public void BodyOnEnterHitResponse(IHitboxController hbController)
	{
		this.TriggerCollision(hbController.DamageObj);
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x0007BD27 File Offset: 0x00079F27
	public void BodyOnStayHitResponse(IHitboxController hbController)
	{
		this.BodyOnEnterHitResponse(hbController);
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x0007BD30 File Offset: 0x00079F30
	protected virtual void Break(IDamageObj damageObj)
	{
		this.BroadcastDestroyedEvents(damageObj);
		this.m_onDisableRelay.Dispatch(this);
		this.BreakDecos(damageObj);
		if (this.m_spawnItemOnDestroy || this.ItemDropTypeOverride != ItemDropType.None)
		{
			this.DropReward();
		}
		this.SetBrokenVisuals(true);
		this.SetHitBoxControllerIsActive(false);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x0007BD70 File Offset: 0x00079F70
	private void BreakDecos(IDamageObj damageObj)
	{
		if (this.m_prop)
		{
			for (int i = 0; i < this.m_prop.DecoControllers.Length; i++)
			{
				DecoController decoController = this.m_prop.DecoControllers[i];
				for (int j = 0; j < decoController.DecoLocations.Length; j++)
				{
					DecoLocation decoLocation = decoController.DecoLocations[j];
					if (decoLocation.DecoInstance)
					{
						Breakable[] componentsInChildren = decoLocation.DecoInstance.GetComponentsInChildren<Breakable>();
						for (int k = 0; k < componentsInChildren.Length; k++)
						{
							componentsInChildren[k].TriggerCollision(damageObj);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x0007BE04 File Offset: 0x0007A004
	private void SetBrokenVisuals(bool isBroken)
	{
		bool flag = this.m_spriteRenderer;
		if (flag)
		{
			this.m_spriteRenderer.enabled = true;
		}
		if (!isBroken)
		{
			if (flag && this.m_spriteRenderer.sprite != this.m_initialSprite)
			{
				this.m_spriteRenderer.sprite = this.m_initialSprite;
				return;
			}
		}
		else if (this.DoNotDisableOnDeath)
		{
			if (flag && this.m_brokenStateSprite)
			{
				this.m_spriteRenderer.sprite = this.m_brokenStateSprite;
				return;
			}
		}
		else
		{
			this.DisableDecos();
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x0007BE98 File Offset: 0x0007A098
	private void DisableDecos()
	{
		if (this.m_prop)
		{
			for (int i = 0; i < this.m_prop.DecoControllers.Length; i++)
			{
				DecoController decoController = this.m_prop.DecoControllers[i];
				for (int j = 0; j < decoController.DecoLocations.Length; j++)
				{
					DecoLocation decoLocation = decoController.DecoLocations[j];
					if (decoLocation.DecoInstance)
					{
						decoLocation.DecoInstance.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x0007BF14 File Offset: 0x0007A114
	public virtual void ForceBrokenState(bool isBroken)
	{
		if (isBroken)
		{
			this.CurrentHP = 0;
			if (this.IsDirectionalBreak)
			{
				if (this.AttackerIsOnRight)
				{
					this.AnimatorComponent.SetTrigger("OpenLeft");
					this.AnimatorComponent.Update(2f);
					this.AnimatorComponent.Update(2f);
				}
				else
				{
					this.AnimatorComponent.SetTrigger("OpenRight");
					this.AnimatorComponent.Update(2f);
					this.AnimatorComponent.Update(2f);
				}
			}
		}
		else
		{
			this.CurrentHP = this.DefaultHP;
		}
		this.SetBrokenVisuals(isBroken);
		this.SetHitBoxControllerIsActive(!isBroken);
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x0007BFBC File Offset: 0x0007A1BC
	public void SetAnimatorState(int stateHash)
	{
		if (this.AnimatorComponent == null || stateHash == -1)
		{
			return;
		}
		if (stateHash != -1)
		{
			this.AnimatorComponent.Play(stateHash, 0, 1f);
			return;
		}
		this.AnimatorComponent.Play("Idle", 0, 0f);
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x0007C009 File Offset: 0x0007A209
	private void SetHitBoxControllerIsActive(bool isActive)
	{
		if (this.m_hitboxController == null)
		{
			this.m_hitboxController = base.GetComponentInChildren<IHitboxController>();
		}
		this.m_hitboxController.DisableAllCollisions = !isActive;
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x0007C030 File Offset: 0x0007A230
	private void DropReward()
	{
		ItemDropType itemDropType;
		if (this.ItemDropTypeOverride != ItemDropType.None)
		{
			itemDropType = this.ItemDropTypeOverride;
		}
		else
		{
			if (ChallengeManager.IsInChallenge || !this.m_prop.Room.AllowItemDrops)
			{
				return;
			}
			Breakable.m_dropTypeHelper_STATIC.Clear();
			Breakable.m_dropTypeHelper_STATIC.AddRange(Economy_EV.BREAKABLE_ITEM_DROP_TYPE_ODDS);
			float num = (float)SaveManager.PlayerSaveData.GetRelic(RelicType.MeatChanceUp).Level * 0.01f;
			if (num > 0f)
			{
				for (int i = 0; i < Breakable.m_dropTypeHelper_STATIC.Count; i++)
				{
					Vector2 vector = Breakable.m_dropTypeHelper_STATIC[i];
					if (vector.x == 50f)
					{
						vector.y += num;
						Breakable.m_dropTypeHelper_STATIC[i] = vector;
					}
				}
			}
			int randomOdds = CDGHelper.GetRandomOdds(this.GetDropOdds(Breakable.m_dropTypeHelper_STATIC));
			itemDropType = (ItemDropType)Breakable.m_dropTypeHelper_STATIC[randomOdds].x;
		}
		bool largeSpurt = TraitManager.IsTraitActive(TraitType.ItemsGoFlying);
		if (itemDropType == ItemDropType.None)
		{
			float currentStatGain = SkillTreeManager.GetSkillTreeObj(SkillTreeType.Equipment_Ore_Find_Up).CurrentStatGain;
			float currentStatGain2 = SkillTreeManager.GetSkillTreeObj(SkillTreeType.Rune_Ore_Find_Up).CurrentStatGain;
			float y = 1f - (currentStatGain + currentStatGain2);
			Breakable.m_dropTypeHelper_STATIC.Clear();
			Breakable.m_dropTypeHelper_STATIC.Add(new Vector2(70f, currentStatGain));
			Breakable.m_dropTypeHelper_STATIC.Add(new Vector2(80f, currentStatGain2));
			Breakable.m_dropTypeHelper_STATIC.Add(new Vector2(0f, y));
			int randomOdds2 = CDGHelper.GetRandomOdds(this.GetDropOdds(Breakable.m_dropTypeHelper_STATIC));
			itemDropType = (ItemDropType)Breakable.m_dropTypeHelper_STATIC[randomOdds2].x;
		}
		if (itemDropType != ItemDropType.None)
		{
			int itemDropValue = Economy_EV.GetItemDropValue(itemDropType, false);
			Vector3 midpoint = this.Midpoint;
			ItemDropManager.DropItem(itemDropType, itemDropValue, midpoint, largeSpurt, false, false);
		}
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x0007C1EC File Offset: 0x0007A3EC
	private List<float> GetDropOdds(List<Vector2> dropTypeList)
	{
		Breakable.m_dropOddsHelper_STATIC.Clear();
		int count = dropTypeList.Count;
		for (int i = 0; i < count; i++)
		{
			Breakable.m_dropOddsHelper_STATIC.Add(dropTypeList[i].y);
		}
		return Breakable.m_dropOddsHelper_STATIC;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x0007C231 File Offset: 0x0007A431
	public void TerrainOnEnterHitResponse(IHitboxController hbController)
	{
		if (!this.m_collideWithWeaponHitboxOnly)
		{
			this.TriggerCollision(hbController.DamageObj);
		}
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x0007C247 File Offset: 0x0007A447
	public void TerrainOnStayHitResponse(IHitboxController hbController)
	{
		this.TerrainOnEnterHitResponse(hbController);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x0007C250 File Offset: 0x0007A450
	protected virtual void TriggerCollision(IDamageObj damageObj)
	{
		if (this.IsBroken)
		{
			return;
		}
		int num = Mathf.Clamp(Mathf.FloorToInt(damageObj.ActualDamage), 1, int.MaxValue);
		if (this.m_displayDamageText)
		{
			Bounds bounds = this.m_hitboxController.GetCollider(HitboxType.Body).bounds;
			Vector2 absPos = new Vector2(bounds.center.x, bounds.max.y);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.EnemyHit, (-num).ToString(), absPos, null, TextAlignmentOptions.Center);
		}
		this.CurrentHP--;
		if (this.CurrentHP <= 0)
		{
			if (this.m_hitEffect)
			{
				this.m_hitEffect.StartSingleBlinkEffect();
			}
			GameObject gameObject = damageObj.gameObject;
			Projectile_RL projectile_RL = damageObj as Projectile_RL;
			if (projectile_RL && projectile_RL.UseOwnerCollisionPoint)
			{
				gameObject = projectile_RL.Owner.gameObject;
			}
			if (gameObject.transform.position.x < base.transform.position.x)
			{
				this.AttackerIsOnRight = false;
			}
			else
			{
				this.AttackerIsOnRight = true;
			}
			this.Break(damageObj);
			return;
		}
		if (this.m_hitEffect)
		{
			this.m_hitEffect.StartSingleBlinkEffect();
		}
		this.BroadcastHitEvents(damageObj);
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x0007C38B File Offset: 0x0007A58B
	private void BroadcastDestroyedEvents(IDamageObj damageObj)
	{
		if (this.m_destroyedEvent != null)
		{
			this.m_destroyedEvent.Invoke();
		}
		this.m_onDeathEffectTriggerEventRelay.Dispatch(damageObj.gameObject);
		this.InitializeHitEventArgs(damageObj);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BreakableDestroyed, this, this.m_hitEventArgs);
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x0007C3C6 File Offset: 0x0007A5C6
	private void InitializeHitEventArgs(IDamageObj damageObj)
	{
		if (this.m_hitEventArgs == null)
		{
			this.m_hitEventArgs = new BreakableHitEventArgs(this, damageObj);
			return;
		}
		this.m_hitEventArgs.Initialize(this, damageObj);
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x0007C3EC File Offset: 0x0007A5EC
	protected void BroadcastHitEvents(IDamageObj damageObj)
	{
		this.m_onDamageEffectTriggerRelay.Dispatch(damageObj.gameObject, damageObj.ActualDamage, false);
		if (this.m_hitEvent != null)
		{
			this.m_hitEvent.Invoke();
		}
		this.InitializeHitEventArgs(damageObj);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BreakableHit, this, this.m_hitEventArgs);
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x0007C48D File Offset: 0x0007A68D
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x0007C495 File Offset: 0x0007A695
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001F76 RID: 8054
	[SerializeField]
	private bool m_collideWithWeaponHitboxOnly;

	// Token: 0x04001F77 RID: 8055
	[SerializeField]
	private bool m_doNotDisableOnDeath;

	// Token: 0x04001F78 RID: 8056
	[SerializeField]
	private bool m_spawnItemOnDestroy = true;

	// Token: 0x04001F79 RID: 8057
	[SerializeField]
	private Sprite m_brokenStateSprite;

	// Token: 0x04001F7A RID: 8058
	[SerializeField]
	private int m_baseScaledHP = 1;

	// Token: 0x04001F7B RID: 8059
	[SerializeField]
	private bool m_displayDamageText;

	// Token: 0x04001F7C RID: 8060
	[SerializeField]
	private UnityEvent m_destroyedEvent;

	// Token: 0x04001F7D RID: 8061
	[SerializeField]
	private UnityEvent m_hitEvent;

	// Token: 0x04001F7E RID: 8062
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_currentHP = -1;

	// Token: 0x04001F7F RID: 8063
	private const string DEFAULT_STATE_NAME = "Idle";

	// Token: 0x04001F80 RID: 8064
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04001F81 RID: 8065
	private Sprite m_initialSprite;

	// Token: 0x04001F82 RID: 8066
	protected BlinkPulseEffect m_hitEffect;

	// Token: 0x04001F83 RID: 8067
	protected IHitboxController m_hitboxController;

	// Token: 0x04001F84 RID: 8068
	private Animator m_animator;

	// Token: 0x04001F85 RID: 8069
	private bool m_attackerIsOnRight;

	// Token: 0x04001F86 RID: 8070
	private BreakableHitEventArgs m_hitEventArgs;

	// Token: 0x04001F87 RID: 8071
	private Prop m_prop;

	// Token: 0x04001F88 RID: 8072
	private BreakableEventArgs m_breakableDestroyedEventArgs;

	// Token: 0x04001F8B RID: 8075
	private Relay<IPreOnDisable> m_onDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x04001F8C RID: 8076
	private Relay<GameObject> m_onDeathEffectTriggerEventRelay = new Relay<GameObject>();

	// Token: 0x04001F8D RID: 8077
	private Relay<GameObject, float, bool> m_onDamageEffectTriggerRelay = new Relay<GameObject, float, bool>();

	// Token: 0x04001F8E RID: 8078
	private static List<Vector2> m_dropTypeHelper_STATIC = new List<Vector2>();

	// Token: 0x04001F8F RID: 8079
	private static List<float> m_dropOddsHelper_STATIC = new List<float>();
}
