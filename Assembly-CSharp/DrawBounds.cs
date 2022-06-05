using System;
using UnityEngine;

// Token: 0x02000CBA RID: 3258
public class DrawBounds : MonoBehaviour
{
	// Token: 0x06005D3E RID: 23870 RVA: 0x0015AABC File Offset: 0x00158CBC
	private void OnDrawGizmosSelected()
	{
		if (this.m_spriteRenderer != null)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawCube(this.m_spriteRenderer.bounds.center, this.m_spriteRenderer.bounds.size);
		}
	}

	// Token: 0x04004CAA RID: 19626
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;
}
