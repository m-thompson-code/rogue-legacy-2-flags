using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000397 RID: 919
public class ExtendHitbox : MonoBehaviour
{
	// Token: 0x06001EA5 RID: 7845 RVA: 0x000100AE File Offset: 0x0000E2AE
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

	// Token: 0x04001B5F RID: 7007
	[SerializeField]
	private HitboxType m_hitboxTypeToAlter;

	// Token: 0x04001B60 RID: 7008
	[SerializeField]
	private float m_deltaWidth;

	// Token: 0x04001B61 RID: 7009
	[SerializeField]
	private float m_deltaHeight;
}
