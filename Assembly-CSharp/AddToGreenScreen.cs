using System;
using UnityEngine;

// Token: 0x02000C4D RID: 3149
public class AddToGreenScreen : MonoBehaviour
{
	// Token: 0x06005ADC RID: 23260 RVA: 0x00031DD6 File Offset: 0x0002FFD6
	private void Awake()
	{
		this.m_storedLayer = base.gameObject.layer;
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x00031DE9 File Offset: 0x0002FFE9
	public void ResetLayers()
	{
		base.gameObject.SetLayerRecursively(this.m_storedLayer, false);
	}

	// Token: 0x04004BD3 RID: 19411
	private int m_storedLayer;
}
