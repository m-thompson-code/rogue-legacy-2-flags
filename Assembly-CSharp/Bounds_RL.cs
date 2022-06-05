using System;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class Bounds_RL : MonoBehaviour
{
	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06001ACE RID: 6862 RVA: 0x0000DDAD File Offset: 0x0000BFAD
	public Collider2D Collider
	{
		get
		{
			return this.m_boundsArray[this.m_activeBoundsIndex];
		}
	}

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x06001ACF RID: 6863 RVA: 0x0000DDBC File Offset: 0x0000BFBC
	public Bounds Bounds
	{
		get
		{
			return this.Collider.bounds;
		}
	}

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x0000DDC9 File Offset: 0x0000BFC9
	public Rigidbody2D Rigidbody
	{
		get
		{
			return this.m_rigidbody;
		}
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x00093434 File Offset: 0x00091634
	private void Awake()
	{
		Collider2D[] boundsArray = this.m_boundsArray;
		for (int i = 0; i < boundsArray.Length; i++)
		{
			boundsArray[i].gameObject.layer = 2;
		}
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x0000DDD1 File Offset: 0x0000BFD1
	public void ChangeBoundsIndex(int index)
	{
		this.m_activeBoundsIndex = index;
	}

	// Token: 0x040018F4 RID: 6388
	[SerializeField]
	private Collider2D[] m_boundsArray;

	// Token: 0x040018F5 RID: 6389
	[SerializeField]
	private Rigidbody2D m_rigidbody;

	// Token: 0x040018F6 RID: 6390
	private int m_activeBoundsIndex;
}
