using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class Bounds_RL : MonoBehaviour
{
	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06001258 RID: 4696 RVA: 0x00035B42 File Offset: 0x00033D42
	public Collider2D Collider
	{
		get
		{
			return this.m_boundsArray[this.m_activeBoundsIndex];
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06001259 RID: 4697 RVA: 0x00035B51 File Offset: 0x00033D51
	public Bounds Bounds
	{
		get
		{
			return this.Collider.bounds;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x0600125A RID: 4698 RVA: 0x00035B5E File Offset: 0x00033D5E
	public Rigidbody2D Rigidbody
	{
		get
		{
			return this.m_rigidbody;
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x00035B68 File Offset: 0x00033D68
	private void Awake()
	{
		Collider2D[] boundsArray = this.m_boundsArray;
		for (int i = 0; i < boundsArray.Length; i++)
		{
			boundsArray[i].gameObject.layer = 2;
		}
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00035B98 File Offset: 0x00033D98
	public void ChangeBoundsIndex(int index)
	{
		this.m_activeBoundsIndex = index;
	}

	// Token: 0x040012CA RID: 4810
	[SerializeField]
	private Collider2D[] m_boundsArray;

	// Token: 0x040012CB RID: 4811
	[SerializeField]
	private Rigidbody2D m_rigidbody;

	// Token: 0x040012CC RID: 4812
	private int m_activeBoundsIndex;
}
