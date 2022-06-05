using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class ExtendHitbox : MonoBehaviour
{
	// Token: 0x06001560 RID: 5472 RVA: 0x00042493 File Offset: 0x00040693
	private IEnumerator Start()
	{
		IHitboxController hbController = this.GetRoot(false).GetComponentInChildren<IHitboxController>();
		while (!hbController.IsInitialized)
		{
			yield return null;
		}
		BoxCollider2D boxCollider2D = hbController.GetCollider(this.m_hitboxTypeToAlter) as BoxCollider2D;
		if (boxCollider2D != null)
		{
			Vector3 localEulerAngles = boxCollider2D.transform.localEulerAngles;
			boxCollider2D.transform.localEulerAngles = Vector3.zero;
			Vector2 size = boxCollider2D.size;
			size.x += this.m_deltaWidth;
			size.y += this.m_deltaHeight;
			boxCollider2D.size = size;
			boxCollider2D.transform.localEulerAngles = localEulerAngles;
		}
		yield break;
	}

	// Token: 0x040014A2 RID: 5282
	[SerializeField]
	private HitboxType m_hitboxTypeToAlter;

	// Token: 0x040014A3 RID: 5283
	[SerializeField]
	private float m_deltaWidth;

	// Token: 0x040014A4 RID: 5284
	[SerializeField]
	private float m_deltaHeight;
}
