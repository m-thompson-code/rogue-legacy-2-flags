using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class BoundsObj : MonoBehaviour
{
	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x0600255D RID: 9565 RVA: 0x0007B9B4 File Offset: 0x00079BB4
	public float Top
	{
		get
		{
			if (this.m_boundsSet)
			{
				return this.Bounds.max.y;
			}
			return base.transform.position.y;
		}
	}

	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x0600255E RID: 9566 RVA: 0x0007B9F0 File Offset: 0x00079BF0
	public float Bottom
	{
		get
		{
			if (this.m_boundsSet)
			{
				return this.Bounds.center.y - this.Bounds.extents.y;
			}
			return base.transform.position.y;
		}
	}

	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x0600255F RID: 9567 RVA: 0x0007BA40 File Offset: 0x00079C40
	public float Left
	{
		get
		{
			if (this.m_boundsSet)
			{
				return this.Bounds.center.x - this.Bounds.extents.x;
			}
			return base.transform.position.x;
		}
	}

	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x06002560 RID: 9568 RVA: 0x0007BA90 File Offset: 0x00079C90
	public float Right
	{
		get
		{
			if (this.m_boundsSet)
			{
				return this.Bounds.center.x + this.Bounds.extents.x;
			}
			return base.transform.position.x;
		}
	}

	// Token: 0x17000F0F RID: 3855
	// (get) Token: 0x06002561 RID: 9569 RVA: 0x0007BAE0 File Offset: 0x00079CE0
	public Vector3 Center
	{
		get
		{
			if (this.m_boundsSet)
			{
				return this.Bounds.center;
			}
			return base.transform.position;
		}
	}

	// Token: 0x17000F10 RID: 3856
	// (get) Token: 0x06002562 RID: 9570 RVA: 0x0007BB0F File Offset: 0x00079D0F
	// (set) Token: 0x06002563 RID: 9571 RVA: 0x0007BB17 File Offset: 0x00079D17
	public Bounds Bounds { get; private set; }

	// Token: 0x06002564 RID: 9572 RVA: 0x0007BB20 File Offset: 0x00079D20
	private IEnumerator Start()
	{
		GameObject rootObj = this.GetRoot(false);
		IHitboxController hbController = rootObj.GetComponentInChildren<IHitboxController>();
		yield return new WaitUntil(() => hbController == null || hbController.IsInitialized);
		if (!this.m_boundsSet)
		{
			Collider2D componentInChildren = rootObj.GetComponentInChildren<Collider2D>();
			if (componentInChildren != null)
			{
				this.Bounds = componentInChildren.bounds;
				this.m_boundsSet = true;
			}
		}
		if (!this.m_boundsSet)
		{
			Renderer componentInChildren2 = rootObj.GetComponentInChildren<Renderer>();
			if (componentInChildren2 != null)
			{
				this.m_boundsSet = true;
				this.Bounds = componentInChildren2.bounds;
			}
		}
		yield break;
	}

	// Token: 0x04001F73 RID: 8051
	[SerializeField]
	private bool m_drawBounds;

	// Token: 0x04001F74 RID: 8052
	private bool m_boundsSet;
}
