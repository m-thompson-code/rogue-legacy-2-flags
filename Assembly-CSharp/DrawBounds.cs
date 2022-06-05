using System;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
public class DrawBounds : MonoBehaviour
{
	// Token: 0x060043B5 RID: 17333 RVA: 0x000ECB58 File Offset: 0x000EAD58
	private void OnDrawGizmosSelected()
	{
		if (this.m_spriteRenderer != null)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawCube(this.m_spriteRenderer.bounds.center, this.m_spriteRenderer.bounds.size);
		}
	}

	// Token: 0x040039E5 RID: 14821
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;
}
