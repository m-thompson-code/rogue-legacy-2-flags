using System;
using UnityEngine;

// Token: 0x02000788 RID: 1928
public class MidpointObj : MonoBehaviour, IMidpointObj
{
	// Token: 0x170015CA RID: 5578
	// (get) Token: 0x06003B10 RID: 15120 RVA: 0x000F2DE8 File Offset: 0x000F0FE8
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

	// Token: 0x06003B11 RID: 15121 RVA: 0x000F2E90 File Offset: 0x000F1090
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

	// Token: 0x04002F03 RID: 12035
	[SerializeField]
	private bool m_usePositionOffset;

	// Token: 0x04002F04 RID: 12036
	[SerializeField]
	private Vector3 m_positionOffset;

	// Token: 0x04002F05 RID: 12037
	private Renderer m_renderer;

	// Token: 0x04002F06 RID: 12038
	private Collider2D m_collider;

	// Token: 0x04002F07 RID: 12039
	private BaseCharacterController m_charController;
}
