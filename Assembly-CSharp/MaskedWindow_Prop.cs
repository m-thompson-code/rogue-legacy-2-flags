using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200061C RID: 1564
public class MaskedWindow_Prop : Prop
{
	// Token: 0x06003877 RID: 14455 RVA: 0x000C0EBC File Offset: 0x000BF0BC
	protected override void ResizeSlicedOrTiledSpriteRenderer(SpriteRenderer spriteRenderer, Vector2 newSize)
	{
		float x = newSize.x - this.m_referenceWindowBorderSize.x;
		float y = newSize.y - this.m_referenceWindowBorderSize.y;
		this.m_maskObject.transform.localScale = new Vector3(x, y, this.m_maskObject.transform.localScale.z);
		Vector3 localPosition = this.m_maskObject.transform.localPosition;
		localPosition.y = newSize.y / 2f;
		this.m_maskObject.transform.localPosition = localPosition;
		base.ResizeSlicedOrTiledSpriteRenderer(spriteRenderer, newSize);
	}

	// Token: 0x04002BAC RID: 11180
	[Tooltip("Calculated by: (sprite tiled size in units - stencil mask scale in units")]
	[SerializeField]
	private Vector2 m_referenceWindowBorderSize;

	// Token: 0x04002BAD RID: 11181
	[SerializeField]
	[FormerlySerializedAs("m_spriteMaskObject")]
	private GameObject m_maskObject;
}
