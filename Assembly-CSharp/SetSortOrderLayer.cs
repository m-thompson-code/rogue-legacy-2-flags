using System;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class SetSortOrderLayer : MonoBehaviour
{
	// Token: 0x06001D83 RID: 7555 RVA: 0x0006126C File Offset: 0x0005F46C
	private void Start()
	{
		this.m_meshRenderer.sortingLayerName = this.m_layerName;
	}

	// Token: 0x04001B73 RID: 7027
	[SerializeField]
	private MeshRenderer m_meshRenderer;

	// Token: 0x04001B74 RID: 7028
	[SerializeField]
	private string m_layerName = "";
}
