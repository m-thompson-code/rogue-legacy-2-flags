using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class SharedGameObjects_Loader : MonoBehaviour
{
	// Token: 0x0600444A RID: 17482 RVA: 0x000F1880 File Offset: 0x000EFA80
	private void Awake()
	{
		if (SharedGameObjects_Loader.m_instance == null)
		{
			SharedGameObjects_Loader.m_instance = this;
			base.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (SceneLoadingUtility.ActiveScene.name != SceneLoadingUtility.GetSceneName(SceneID.Splash))
			{
				CDGResources.Init(true);
				RuntimeManager.LoadBank("Master.strings", false);
				RuntimeManager.LoadBank("Master", false);
				RuntimeManager.LoadBank("Music", false);
				RuntimeManager.WaitForAllLoads();
				GameObject gameObject = new GameObject("SharedGameObjects_Game");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				GameObject gameObject2 = CDGResources.Load<GameObject>(this.m_sharedGameObjectsPath, "", true);
				for (int i = 0; i < gameObject2.transform.childCount; i++)
				{
					ILoadable component = UnityEngine.Object.Instantiate<GameObject>(gameObject2.transform.GetChild(i).gameObject, gameObject.transform).GetComponent<ILoadable>();
					if (component != null)
					{
						component.LoadSync();
					}
				}
				return;
			}
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x000F1976 File Offset: 0x000EFB76
	public IEnumerator Initialize()
	{
		CDGAsyncLoadRequest<GameObject> sharedGameObjectsReq = CDGResources.LoadAsync<GameObject>(this.m_sharedGameObjectsPath, "");
		while (!sharedGameObjectsReq.IsDone)
		{
			yield return null;
		}
		GameObject rootSharedGameObjects = new GameObject("SharedGameObjects_Game");
		UnityEngine.Object.DontDestroyOnLoad(rootSharedGameObjects);
		GameObject prefab = sharedGameObjectsReq.Asset;
		int num;
		for (int i = 0; i < prefab.transform.childCount; i = num + 1)
		{
			ILoadable component = UnityEngine.Object.Instantiate<GameObject>(prefab.transform.GetChild(i).gameObject, rootSharedGameObjects.transform).GetComponent<ILoadable>();
			if (component != null)
			{
				yield return component.LoadAsync();
			}
			yield return null;
			num = i;
		}
		yield break;
	}

	// Token: 0x04003A51 RID: 14929
	[SerializeField]
	private string m_sharedGameObjectsPath;

	// Token: 0x04003A52 RID: 14930
	private static SharedGameObjects_Loader m_instance;
}
