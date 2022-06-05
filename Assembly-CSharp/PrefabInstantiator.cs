using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CCE RID: 3278
public class PrefabInstantiator : MonoBehaviour
{
	// Token: 0x06005D8B RID: 23947 RVA: 0x0015E724 File Offset: 0x0015C924
	private void Awake()
	{
		foreach (GameObject original in this.m_prefabs)
		{
			Transform parent = null;
			if (this.m_parentToThisGameObject)
			{
				parent = base.transform;
			}
			UnityEngine.Object.Instantiate<GameObject>(original, parent);
		}
	}

	// Token: 0x04004CE7 RID: 19687
	[SerializeField]
	private List<GameObject> m_prefabs;

	// Token: 0x04004CE8 RID: 19688
	[SerializeField]
	private bool m_parentToThisGameObject = true;
}
