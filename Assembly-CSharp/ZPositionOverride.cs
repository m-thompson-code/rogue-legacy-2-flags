using System;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class ZPositionOverride : MonoBehaviour
{
	// Token: 0x06003C5A RID: 15450 RVA: 0x000D0899 File Offset: 0x000CEA99
	public void SetZPosition()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.m_zPosition);
	}

	// Token: 0x04002D6F RID: 11631
	[SerializeField]
	private float m_zPosition;
}
