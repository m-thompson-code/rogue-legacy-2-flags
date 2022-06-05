using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000810 RID: 2064
public class ScenePrefab_Loader : MonoBehaviour
{
	// Token: 0x0600443F RID: 17471 RVA: 0x000F174C File Offset: 0x000EF94C
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

	// Token: 0x04003A4A RID: 14922
	[SerializeField]
	private string m_prefabPath;

	// Token: 0x04003A4B RID: 14923
	private static Dictionary<string, GameObject> m_scenePrefabDict = new Dictionary<string, GameObject>();
}
