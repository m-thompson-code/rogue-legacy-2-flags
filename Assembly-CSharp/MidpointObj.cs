using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class MidpointObj : MonoBehaviour, IMidpointObj
{
	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000912B8 File Offset: 0x0008F4B8
	public Vector3 Midpoint
	{
		get
		{
			if (this.m_usePositionOffset)
			{
				return base.gameObject.transform.position + this.m_positionOffset;
			}
			if (this.m_charController != null)
			{
				return this.m_charController.CollisionBounds.center;
			}
			if (this.m_collider != null)
			{
				return this.m_collider.bounds.center;
			}
			if (this.m_renderer != null)
			{
				return this.m_renderer.bounds.center;
			}
			return base.transform.position;
		}
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x00091360 File Offset: 0x0008F560
	private void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_charController = root.GetComponentInChildren<BaseCharacterController>();
		if (this.m_charController == null)
		{
			this.m_collider = root.GetComponentInChildren<Collider2D>();
			if (this.m_collider == null)
			{
				this.m_renderer = root.GetComponentInChildren<Renderer>();
			}
		}
	}

	// Token: 0x040022FD RID: 8957
	[SerializeField]
	private bool m_usePositionOffset;

	// Token: 0x040022FE RID: 8958
	[SerializeField]
	private Vector3 m_positionOffset;

	// Token: 0x040022FF RID: 8959
	private Renderer m_renderer;

	// Token: 0x04002300 RID: 8960
	private Collider2D m_collider;

	// Token: 0x04002301 RID: 8961
	private BaseCharacterController m_charController;
}
