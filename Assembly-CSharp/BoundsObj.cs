using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000698 RID: 1688
public class BoundsObj : MonoBehaviour
{
	// Token: 0x170013B8 RID: 5048
	// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000DB8C8 File Offset: 0x000D9AC8
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

	// Token: 0x170013B9 RID: 5049
	// (get) Token: 0x060033C4 RID: 13252 RVA: 0x000DB904 File Offset: 0x000D9B04
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

	// Token: 0x170013BA RID: 5050
	// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000DB954 File Offset: 0x000D9B54
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

	// Token: 0x170013BB RID: 5051
	// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000DB9A4 File Offset: 0x000D9BA4
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

	// Token: 0x170013BC RID: 5052
	// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000DB9F4 File Offset: 0x000D9BF4
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

	// Token: 0x170013BD RID: 5053
	// (get) Token: 0x060033C8 RID: 13256 RVA: 0x0001C618 File Offset: 0x0001A818
	// (set) Token: 0x060033C9 RID: 13257 RVA: 0x0001C620 File Offset: 0x0001A820
	public Bounds Bounds { get; private set; }

	// Token: 0x060033CA RID: 13258 RVA: 0x0001C629 File Offset: 0x0001A829
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

	// Token: 0x04002A0F RID: 10767
	[SerializeField]
	private bool m_drawBounds;

	// Token: 0x04002A10 RID: 10768
	private bool m_boundsSet;
}
