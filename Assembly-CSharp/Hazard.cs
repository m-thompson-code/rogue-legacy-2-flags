using System;
using UnityEngine;

// Token: 0x02000724 RID: 1828
public abstract class Hazard : MonoBehaviour, IHazard, IRootObj, IDamageObj, IGenericPoolObj, IPlayHitEffect, IRoomConsumer, ILevelConsumer
{
	// Token: 0x170014E5 RID: 5349
	// (get) Token: 0x060037D6 RID: 14294 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014E6 RID: 5350
	// (get) Token: 0x060037D7 RID: 14295 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
	// (set) Token: 0x060037D8 RID: 14296 RVA: 0x0001EAC1 File Offset: 0x0001CCC1
	public bool IsFreePoolObj { get; set; }

	// Token: 0x170014E7 RID: 5351
	// (get) Token: 0x060037D9 RID: 14297 RVA: 0x0001EACA File Offset: 0x0001CCCA
	// (set) Token: 0x060037DA RID: 14298 RVA: 0x0001EAD2 File Offset: 0x0001CCD2
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x170014E8 RID: 5352
	// (get) Token: 0x060037DB RID: 14299 RVA: 0x0001EADB File Offset: 0x0001CCDB
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x170014E9 RID: 5353
	// (get) Token: 0x060037DC RID: 14300 RVA: 0x0000F49B File Offset: 0x0000D69B
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014EA RID: 5354
	// (get) Token: 0x060037DD RID: 14301 RVA: 0x0000F49B File Offset: 0x0000D69B
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014EB RID: 5355
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170014EC RID: 5356
	// (get) Token: 0x060037DF RID: 14303 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public virtual bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170014ED RID: 5357
	// (get) Token: 0x060037E0 RID: 14304 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170014EE RID: 5358
	// (get) Token: 0x060037E1 RID: 14305 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170014EF RID: 5359
	// (get) Token: 0x060037E2 RID: 14306 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170014F0 RID: 5360
	// (get) Token: 0x060037E3 RID: 14307 RVA: 0x00003CCB File Offset: 0x00001ECB
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170014F1 RID: 5361
	// (get) Token: 0x060037E4 RID: 14308 RVA: 0x0001EAE3 File Offset: 0x0001CCE3
	public virtual float ActualDamage
	{
		get
		{
			return Hazard_EV.GetDamageAmount(this.Room);
		}
	}

	// Token: 0x170014F2 RID: 5362
	// (get) Token: 0x060037E5 RID: 14309 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x170014F3 RID: 5363
	// (get) Token: 0x060037E6 RID: 14310 RVA: 0x0001EAF8 File Offset: 0x0001CCF8
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x170014F4 RID: 5364
	// (get) Token: 0x060037E7 RID: 14311 RVA: 0x0001EB00 File Offset: 0x0001CD00
	public virtual float BaseDamage
	{
		get
		{
			return this.ActualDamage;
		}
	}

	// Token: 0x170014F5 RID: 5365
	// (get) Token: 0x060037E8 RID: 14312 RVA: 0x0001EB08 File Offset: 0x0001CD08
	// (set) Token: 0x060037E9 RID: 14313 RVA: 0x0001EB10 File Offset: 0x0001CD10
	public float BaseStunStrength
	{
		get
		{
			return this.m_baseStunStrength;
		}
		set
		{
			this.m_baseStunStrength = value;
		}
	}

	// Token: 0x170014F6 RID: 5366
	// (get) Token: 0x060037EA RID: 14314 RVA: 0x0001EB19 File Offset: 0x0001CD19
	// (set) Token: 0x060037EB RID: 14315 RVA: 0x0001EB21 File Offset: 0x0001CD21
	public float BaseKnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
		set
		{
			this.m_knockbackStrength = value;
		}
	}

	// Token: 0x170014F7 RID: 5367
	// (get) Token: 0x060037EC RID: 14316 RVA: 0x0001EB2A File Offset: 0x0001CD2A
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return this.m_knockbackMod;
		}
	}

	// Token: 0x170014F8 RID: 5368
	// (get) Token: 0x060037ED RID: 14317 RVA: 0x0001EB32 File Offset: 0x0001CD32
	// (set) Token: 0x060037EE RID: 14318 RVA: 0x0001EB3A File Offset: 0x0001CD3A
	public StateID InitialState
	{
		get
		{
			return this.m_initialState;
		}
		protected set
		{
			this.m_initialState = value;
		}
	}

	// Token: 0x170014F9 RID: 5369
	// (get) Token: 0x060037EF RID: 14319 RVA: 0x0001EB19 File Offset: 0x0001CD19
	public float KnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
	}

	// Token: 0x170014FA RID: 5370
	// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000046FA File Offset: 0x000028FA
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x170014FB RID: 5371
	// (get) Token: 0x060037F1 RID: 14321 RVA: 0x0001EB43 File Offset: 0x0001CD43
	// (set) Token: 0x060037F2 RID: 14322 RVA: 0x0001EB4B File Offset: 0x0001CD4B
	public BaseRoom Room { get; private set; }

	// Token: 0x170014FC RID: 5372
	// (get) Token: 0x060037F3 RID: 14323 RVA: 0x0001EB54 File Offset: 0x0001CD54
	public int Level
	{
		get
		{
			return this.level;
		}
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x0001EB5C File Offset: 0x0001CD5C
	public void SetLevel(int value)
	{
		this.level = value;
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x0001EB65 File Offset: 0x0001CD65
	protected virtual void Awake()
	{
		this.m_animator = base.GetComponentInChildren<Animator>();
		this.IsAwakeCalled = true;
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x0001E5AA File Offset: 0x0001C7AA
	public virtual void Initialize(HazardArgs hazardArgs)
	{
		this.InitialState = hazardArgs.InitialState;
	}

	// Token: 0x060037F7 RID: 14327
	public abstract void ResetHazard();

	// Token: 0x060037F8 RID: 14328 RVA: 0x0001E5DF File Offset: 0x0001C7DF
	public void ResetValues()
	{
		this.ResetHazard();
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x0001EB7A File Offset: 0x0001CD7A
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.SetLevel(room.Level);
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x000E70C4 File Offset: 0x000E52C4
	public virtual void SetIsCulled(bool culled)
	{
		if (!this)
		{
			return;
		}
		if (!culled)
		{
			if (this.m_animator)
			{
				this.m_animator.enabled = true;
			}
			if (this.m_pivot)
			{
				this.m_pivot.SetActive(true);
				return;
			}
		}
		else
		{
			if (this.m_animator)
			{
				this.m_animator.enabled = false;
			}
			if (this.m_pivot)
			{
				this.m_pivot.SetActive(false);
			}
		}
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x0001BE85 File Offset: 0x0001A085
	protected virtual void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CEA RID: 11498
	[SerializeField]
	private Vector2 m_knockbackMod = new Vector2(1f, 1f);

	// Token: 0x04002CEB RID: 11499
	[SerializeField]
	private float m_knockbackStrength = 99f;

	// Token: 0x04002CEC RID: 11500
	[SerializeField]
	protected GameObject m_pivot;

	// Token: 0x04002CED RID: 11501
	private float m_baseStunStrength;

	// Token: 0x04002CEE RID: 11502
	protected StateID m_initialState;

	// Token: 0x04002CEF RID: 11503
	private Animator m_animator;

	// Token: 0x04002CF3 RID: 11507
	private int level;
}
