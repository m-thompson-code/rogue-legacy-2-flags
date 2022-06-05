using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public abstract class BaseEffectTrigger : MonoBehaviour
{
	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x060026AE RID: 9902 RVA: 0x00080161 File Offset: 0x0007E361
	// (set) Token: 0x060026AF RID: 9903 RVA: 0x00080169 File Offset: 0x0007E369
	public EffectTriggerEntry[] TriggerArray
	{
		get
		{
			return this.m_effectList;
		}
		set
		{
			this.m_effectList = value;
		}
	}

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x060026B0 RID: 9904
	public abstract bool RequiresCollider { get; }

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x060026B1 RID: 9905 RVA: 0x00080172 File Offset: 0x0007E372
	// (set) Token: 0x060026B2 RID: 9906 RVA: 0x0008017A File Offset: 0x0007E37A
	public CollisionType CanCollideWith
	{
		get
		{
			return this.m_canCollideWith;
		}
		set
		{
			this.m_canCollideWith = value;
		}
	}

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x060026B3 RID: 9907
	public abstract Vector3 Midpoint { get; }

	// Token: 0x060026B4 RID: 9908 RVA: 0x00080183 File Offset: 0x0007E383
	protected virtual void Awake()
	{
		this.m_rootObj = this.GetRoot(false);
	}

	// Token: 0x0400206B RID: 8299
	[SerializeField]
	private CollisionType m_canCollideWith = CollisionType.Player | CollisionType.PlayerProjectile | CollisionType.Player_Dodging;

	// Token: 0x0400206C RID: 8300
	[SerializeField]
	private EffectTriggerEntry[] m_effectList = new EffectTriggerEntry[0];

	// Token: 0x0400206D RID: 8301
	protected GameObject m_rootObj;
}
