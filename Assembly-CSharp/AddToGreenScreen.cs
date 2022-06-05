using System;
using UnityEngine;

// Token: 0x0200078C RID: 1932
public class AddToGreenScreen : MonoBehaviour
{
	// Token: 0x0600415F RID: 16735 RVA: 0x000E93B0 File Offset: 0x000E75B0
	private void Awake()
	{
		this.m_storedLayer = base.gameObject.layer;
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x000E93C3 File Offset: 0x000E75C3
	public void ResetLayers()
	{
		base.gameObject.SetLayerRecursively(this.m_storedLayer, false);
	}

	// Token: 0x04003923 RID: 14627
	private int m_storedLayer;
}
