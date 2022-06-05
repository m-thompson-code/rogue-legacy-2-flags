using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public abstract class Hazard : MonoBehaviour, IHazard, IRootObj, IDamageObj, IGenericPoolObj, IPlayHitEffect, IRoomConsumer, ILevelConsumer
{
	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x0600286E RID: 10350 RVA: 0x00086070 File Offset: 0x00084270
	public string RelicDamageTypeString
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x0600286F RID: 10351 RVA: 0x00086073 File Offset: 0x00084273
	// (set) Token: 0x06002870 RID: 10352 RVA: 0x0008607B File Offset: 0x0008427B
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06002871 RID: 10353 RVA: 0x00086084 File Offset: 0x00084284
	// (set) Token: 0x06002872 RID: 10354 RVA: 0x0008608C File Offset: 0x0008428C
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x06002873 RID: 10355 RVA: 0x00086095 File Offset: 0x00084295
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x06002874 RID: 10356 RVA: 0x0008609D File Offset: 0x0008429D
	public StatusEffectType[] StatusEffectTypes
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x06002875 RID: 10357 RVA: 0x000860A0 File Offset: 0x000842A0
	public float[] StatusEffectDurations
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06002876 RID: 10358 RVA: 0x000860A3 File Offset: 0x000842A3
	public bool IsDotDamage
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06002877 RID: 10359 RVA: 0x000860A6 File Offset: 0x000842A6
	public virtual bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06002878 RID: 10360 RVA: 0x000860A9 File Offset: 0x000842A9
	public virtual bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06002879 RID: 10361 RVA: 0x000860AC File Offset: 0x000842AC
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000FF0 RID: 4080
	// (get) Token: 0x0600287A RID: 10362 RVA: 0x000860AF File Offset: 0x000842AF
	public float ActualCritChance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x0600287B RID: 10363 RVA: 0x000860B6 File Offset: 0x000842B6
	public float ActualCritDamage
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x0600287C RID: 10364 RVA: 0x000860BD File Offset: 0x000842BD
	public virtual float ActualDamage
	{
		get
		{
			return Hazard_EV.GetDamageAmount(this.Room);
		}
	}

	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x0600287D RID: 10365 RVA: 0x000860CA File Offset: 0x000842CA
	public float ActualKnockbackStrength
	{
		get
		{
			return this.BaseKnockbackStrength;
		}
	}

	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x0600287E RID: 10366 RVA: 0x000860D2 File Offset: 0x000842D2
	public float ActualStunStrength
	{
		get
		{
			return this.BaseStunStrength;
		}
	}

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x0600287F RID: 10367 RVA: 0x000860DA File Offset: 0x000842DA
	public virtual float BaseDamage
	{
		get
		{
			return this.ActualDamage;
		}
	}

	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x06002880 RID: 10368 RVA: 0x000860E2 File Offset: 0x000842E2
	// (set) Token: 0x06002881 RID: 10369 RVA: 0x000860EA File Offset: 0x000842EA
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

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x06002882 RID: 10370 RVA: 0x000860F3 File Offset: 0x000842F3
	// (set) Token: 0x06002883 RID: 10371 RVA: 0x000860FB File Offset: 0x000842FB
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

	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x06002884 RID: 10372 RVA: 0x00086104 File Offset: 0x00084304
	public Vector2 ExternalKnockbackMod
	{
		get
		{
			return this.m_knockbackMod;
		}
	}

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x06002885 RID: 10373 RVA: 0x0008610C File Offset: 0x0008430C
	// (set) Token: 0x06002886 RID: 10374 RVA: 0x00086114 File Offset: 0x00084314
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

	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x06002887 RID: 10375 RVA: 0x0008611D File Offset: 0x0008431D
	public float KnockbackStrength
	{
		get
		{
			return this.m_knockbackStrength;
		}
	}

	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x06002888 RID: 10376 RVA: 0x00086125 File Offset: 0x00084325
	public StrikeType StrikeType
	{
		get
		{
			return StrikeType.Blunt;
		}
	}

	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x06002889 RID: 10377 RVA: 0x00086129 File Offset: 0x00084329
	// (set) Token: 0x0600288A RID: 10378 RVA: 0x00086131 File Offset: 0x00084331
	public BaseRoom Room { get; private set; }

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x0600288B RID: 10379 RVA: 0x0008613A File Offset: 0x0008433A
	public int Level
	{
		get
		{
			return this.level;
		}
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x00086142 File Offset: 0x00084342
	public void SetLevel(int value)
	{
		this.level = value;
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x0008614B File Offset: 0x0008434B
	protected virtual void Awake()
	{
		this.m_animator = base.GetComponentInChildren<Animator>();
		this.IsAwakeCalled = true;
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x00086160 File Offset: 0x00084360
	public virtual void Initialize(HazardArgs hazardArgs)
	{
		this.InitialState = hazardArgs.InitialState;
	}

	// Token: 0x0600288F RID: 10383
	public abstract void ResetHazard();

	// Token: 0x06002890 RID: 10384 RVA: 0x0008616E File Offset: 0x0008436E
	public void ResetValues()
	{
		this.ResetHazard();
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x00086176 File Offset: 0x00084376
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.SetLevel(room.Level);
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x0008618C File Offset: 0x0008438C
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

	// Token: 0x06002893 RID: 10387 RVA: 0x0008620A File Offset: 0x0008440A
	protected virtual void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x0008623B File Offset: 0x0008443B
	GameObject IDamageObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x00086243 File Offset: 0x00084443
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x0008624B File Offset: 0x0008444B
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400217C RID: 8572
	[SerializeField]
	private Vector2 m_knockbackMod = new Vector2(1f, 1f);

	// Token: 0x0400217D RID: 8573
	[SerializeField]
	private float m_knockbackStrength = 99f;

	// Token: 0x0400217E RID: 8574
	[SerializeField]
	protected GameObject m_pivot;

	// Token: 0x0400217F RID: 8575
	private float m_baseStunStrength;

	// Token: 0x04002180 RID: 8576
	protected StateID m_initialState;

	// Token: 0x04002181 RID: 8577
	private Animator m_animator;

	// Token: 0x04002185 RID: 8581
	private int level;
}
