using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000A3F RID: 2623
public class MaskedWindow_Prop : Prop
{
	// Token: 0x06004F1B RID: 20251 RVA: 0x0012F14C File Offset: 0x0012D34C
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

	// Token: 0x04003C19 RID: 15385
	[Tooltip("Calculated by: (sprite tiled size in units - stencil mask scale in units")]
	[SerializeField]
	private Vector2 m_referenceWindowBorderSize;

	// Token: 0x04003C1A RID: 15386
	[SerializeField]
	[FormerlySerializedAs("m_spriteMaskObject")]
	private GameObject m_maskObject;
}
