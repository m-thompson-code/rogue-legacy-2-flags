using System;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public abstract class BaseEffectTrigger : MonoBehaviour
{
	// Token: 0x17001437 RID: 5175
	// (get) Token: 0x06003563 RID: 13667 RVA: 0x0001D488 File Offset: 0x0001B688
	// (set) Token: 0x06003564 RID: 13668 RVA: 0x0001D490 File Offset: 0x0001B690
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

	// Token: 0x17001438 RID: 5176
	// (get) Token: 0x06003565 RID: 13669
	public abstract bool RequiresCollider { get; }

	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x06003566 RID: 13670 RVA: 0x0001D499 File Offset: 0x0001B699
	// (set) Token: 0x06003567 RID: 13671 RVA: 0x0001D4A1 File Offset: 0x0001B6A1
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

	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x06003568 RID: 13672
	public abstract Vector3 Midpoint { get; }

	// Token: 0x06003569 RID: 13673 RVA: 0x0001D4AA File Offset: 0x0001B6AA
	protected virtual void Awake()
	{
		this.m_rootObj = this.GetRoot(false);
	}

	// Token: 0x04002B4E RID: 11086
	[SerializeField]
	private CollisionType m_canCollideWith = CollisionType.Player | CollisionType.PlayerProjectile | CollisionType.Player_Dodging;

	// Token: 0x04002B4F RID: 11087
	[SerializeField]
	private EffectTriggerEntry[] m_effectList = new EffectTriggerEntry[0];

	// Token: 0x04002B50 RID: 11088
	protected GameObject m_rootObj;
}
