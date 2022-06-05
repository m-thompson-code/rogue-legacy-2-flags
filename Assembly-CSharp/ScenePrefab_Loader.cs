using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CE0 RID: 3296
public class ScenePrefab_Loader : MonoBehaviour
{
	// Token: 0x06005DF3 RID: 24051 RVA: 0x0015F2CC File Offset: 0x0015D4CC
	private void Awake()
	{
		GameObject gameObject;
		if (!ScenePrefab_Loader.m_scenePrefabDict.TryGetValue(this.m_prefabPath, out gameObject))
		{
			gameObject = CDGResources.Load<GameObject>(this.m_prefabPath, "", true);
			ScenePrefab_Loader.m_scenePrefabDict.Add(this.m_prefabPath, gameObject);
		}
		UnityEngine.Object.Instantiate<GameObject>(gameObject);
	}

	// Token: 0x04004D30 RID: 19760
	[SerializeField]
	private string m_prefabPath;

	// Token: 0x04004D31 RID: 19761
	private static Dictionary<string, GameObject> m_scenePrefabDict = new Dictionary<string, GameObject>();
}
