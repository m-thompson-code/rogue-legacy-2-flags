using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200069B RID: 1691
public class Breakable : MonoBehaviour, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse, ITerrainOnEnterHitResponse, ITerrainOnStayHitResponse, IEffectTriggerEvent_OnDeath, IEffectTriggerEvent_OnDamage, IRootObj, IPreOnDisable, IPlayHitEffect
{
	// Token: 0x170013C0 RID: 5056
	// (get) Token: 0x060033D4 RID: 13268 RVA: 0x0001C666 File Offset: 0x0001A866
	// (set) Token: 0x060033D5 RID: 13269 RVA: 0x0001C66E File Offset: 0x0001A86E
	public ItemDropType ItemDropTypeOverride { get; set; }

	// Token: 0x170013C1 RID: 5057
	// (get) Token: 0x060033D6 RID: 13270 RVA: 0x0001C677 File Offset: 0x0001A877
	// (set) Token: 0x060033D7 RID: 13271 RVA: 0x0001C67F File Offset: 0x0001A87F
	public bool IsDirectionalBreak { get; private set; }

	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x060033D8 RID: 13272 RVA: 0x0001C688 File Offset: 0x0001A888
	// (set) Token: 0x060033D9 RID: 13273 RVA: 0x0001C690 File Offset: 0x0001A890
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

	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x060033DA RID: 13274 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x060033DB RID: 13275 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x060033DC RID: 13276 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170013C6 RID: 5062
	// (get) Token: 0x060033DD RID: 13277 RVA: 0x0001C699 File Offset: 0x0001A899
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return this.m_spriteRenderer;
		}
	}

	// Token: 0x170013C7 RID: 5063
	// (get) Token: 0x060033DE RID: 13278 RVA: 0x000DBB00 File Offset: 0x000D9D00
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

	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x060033DF RID: 13279 RVA: 0x0001C6A1 File Offset: 0x0001A8A1
	// (set) Token: 0x060033E0 RID: 13280 RVA: 0x0001C6A9 File Offset: 0x0001A8A9
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

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x060033E1 RID: 13281 RVA: 0x000DBB3C File Offset: 0x000D9D3C
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

	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x060033E2 RID: 13282 RVA: 0x0001C6B2 File Offset: 0x0001A8B2
	public IRelayLink<IPreOnDisable> PreOnDisableRelay
	{
		get
		{
			return this.m_onDisableRelay.link;
		}
	}

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x060033E3 RID: 13283 RVA: 0x0001C6BF File Offset: 0x0001A8BF
	public IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay
	{
		get
		{
			return this.m_onDamageEffectTriggerRelay.link;
		}
	}

	// Token: 0x170013CC RID: 5068
	// (get) Token: 0x060033E4 RID: 13284 RVA: 0x0001C6CC File Offset: 0x0001A8CC
	public IRelayLink<GameObject> OnDeathEffectTriggerRelay
	{
		get
		{
			return this.m_onDeathEffectTriggerEventRelay.link;
		}
	}

	// Token: 0x170013CD RID: 5069
	// (get) Token: 0x060033E5 RID: 13285 RVA: 0x0001C6D9 File Offset: 0x0001A8D9
	public UnityEvent DestroyedEvent
	{
		get
		{
			return this.m_destroyedEvent;
		}
	}

	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x060033E6 RID: 13286 RVA: 0x0001C6E1 File Offset: 0x0001A8E1
	public virtual bool IsBroken
	{
		get
		{
			return this.CurrentHP <= 0;
		}
	}

	// Token: 0x170013CF RID: 5071
	// (get) Token: 0x060033E7 RID: 13287 RVA: 0x0001C6EF File Offset: 0x0001A8EF
	// (set) Token: 0x060033E8 RID: 13288 RVA: 0x0001C6F7 File Offset: 0x0001A8F7
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

	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x060033E9 RID: 13289 RVA: 0x0001C700 File Offset: 0x0001A900
	// (set) Token: 0x060033EA RID: 13290 RVA: 0x0001C708 File Offset: 0x0001A908
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

	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x060033EB RID: 13291 RVA: 0x0001C717 File Offset: 0x0001A917
	public int DefaultHP
	{
		get
		{
			return this.m_baseScaledHP;
		}
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x000DBB70 File Offset: 0x000D9D70
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

	// Token: 0x060033ED RID: 13293 RVA: 0x0001C71F File Offset: 0x0001A91F
	private void OnDisable()
	{
		this.ItemDropTypeOverride = ItemDropType.None;
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x0001C728 File Offset: 0x0001A928
	public void BodyOnEnterHitResponse(IHitboxController hbController)
	{
		this.TriggerCollision(hbController.DamageObj);
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x0001C736 File Offset: 0x0001A936
	public void BodyOnStayHitResponse(IHitboxController hbController)
	{
		this.BodyOnEnterHitResponse(hbController);
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x0001C73F File Offset: 0x0001A93F
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

	// Token: 0x060033F1 RID: 13297 RVA: 0x000DBC18 File Offset: 0x000D9E18
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

	// Token: 0x060033F2 RID: 13298 RVA: 0x000DBCAC File Offset: 0x000D9EAC
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

	// Token: 0x060033F3 RID: 13299 RVA: 0x000DBD40 File Offset: 0x000D9F40
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

	// Token: 0x060033F4 RID: 13300 RVA: 0x000DBDBC File Offset: 0x000D9FBC
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

	// Token: 0x060033F5 RID: 13301 RVA: 0x000DBE64 File Offset: 0x000DA064
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

	// Token: 0x060033F6 RID: 13302 RVA: 0x0001C77F File Offset: 0x0001A97F
	private void SetHitBoxControllerIsActive(bool isActive)
	{
		if (this.m_hitboxController == null)
		{
			this.m_hitboxController = base.GetComponentInChildren<IHitboxController>();
		}
		this.m_hitboxController.DisableAllCollisions = !isActive;
	}

	// Token: 0x060033F7 RID: 13303 RVA: 0x000DBEB4 File Offset: 0x000DA0B4
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

	// Token: 0x060033F8 RID: 13304 RVA: 0x000DC070 File Offset: 0x000DA270
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

	// Token: 0x060033F9 RID: 13305 RVA: 0x0001C7A4 File Offset: 0x0001A9A4
	public void TerrainOnEnterHitResponse(IHitboxController hbController)
	{
		if (!this.m_collideWithWeaponHitboxOnly)
		{
			this.TriggerCollision(hbController.DamageObj);
		}
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x0001C7BA File Offset: 0x0001A9BA
	public void TerrainOnStayHitResponse(IHitboxController hbController)
	{
		this.TerrainOnEnterHitResponse(hbController);
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x000DC0B8 File Offset: 0x000DA2B8
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

	// Token: 0x060033FC RID: 13308 RVA: 0x0001C7C3 File Offset: 0x0001A9C3
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

	// Token: 0x060033FD RID: 13309 RVA: 0x0001C7FE File Offset: 0x0001A9FE
	private void InitializeHitEventArgs(IDamageObj damageObj)
	{
		if (this.m_hitEventArgs == null)
		{
			this.m_hitEventArgs = new BreakableHitEventArgs(this, damageObj);
			return;
		}
		this.m_hitEventArgs.Initialize(this, damageObj);
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x000DC1F4 File Offset: 0x000DA3F4
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

	// Token: 0x06003401 RID: 13313 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IPreOnDisable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002A17 RID: 10775
	[SerializeField]
	private bool m_collideWithWeaponHitboxOnly;

	// Token: 0x04002A18 RID: 10776
	[SerializeField]
	private bool m_doNotDisableOnDeath;

	// Token: 0x04002A19 RID: 10777
	[SerializeField]
	private bool m_spawnItemOnDestroy = true;

	// Token: 0x04002A1A RID: 10778
	[SerializeField]
	private Sprite m_brokenStateSprite;

	// Token: 0x04002A1B RID: 10779
	[SerializeField]
	private int m_baseScaledHP = 1;

	// Token: 0x04002A1C RID: 10780
	[SerializeField]
	private bool m_displayDamageText;

	// Token: 0x04002A1D RID: 10781
	[SerializeField]
	private UnityEvent m_destroyedEvent;

	// Token: 0x04002A1E RID: 10782
	[SerializeField]
	private UnityEvent m_hitEvent;

	// Token: 0x04002A1F RID: 10783
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_currentHP = -1;

	// Token: 0x04002A20 RID: 10784
	private const string DEFAULT_STATE_NAME = "Idle";

	// Token: 0x04002A21 RID: 10785
	private SpriteRenderer m_spriteRenderer;

	// Token: 0x04002A22 RID: 10786
	private Sprite m_initialSprite;

	// Token: 0x04002A23 RID: 10787
	protected BlinkPulseEffect m_hitEffect;

	// Token: 0x04002A24 RID: 10788
	protected IHitboxController m_hitboxController;

	// Token: 0x04002A25 RID: 10789
	private Animator m_animator;

	// Token: 0x04002A26 RID: 10790
	private bool m_attackerIsOnRight;

	// Token: 0x04002A27 RID: 10791
	private BreakableHitEventArgs m_hitEventArgs;

	// Token: 0x04002A28 RID: 10792
	private Prop m_prop;

	// Token: 0x04002A29 RID: 10793
	private BreakableEventArgs m_breakableDestroyedEventArgs;

	// Token: 0x04002A2C RID: 10796
	private Relay<IPreOnDisable> m_onDisableRelay = new Relay<IPreOnDisable>();

	// Token: 0x04002A2D RID: 10797
	private Relay<GameObject> m_onDeathEffectTriggerEventRelay = new Relay<GameObject>();

	// Token: 0x04002A2E RID: 10798
	private Relay<GameObject, float, bool> m_onDamageEffectTriggerRelay = new Relay<GameObject, float, bool>();

	// Token: 0x04002A2F RID: 10799
	private static List<Vector2> m_dropTypeHelper_STATIC = new List<Vector2>();

	// Token: 0x04002A30 RID: 10800
	private static List<float> m_dropOddsHelper_STATIC = new List<float>();
}
