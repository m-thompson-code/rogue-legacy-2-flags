using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000CE6 RID: 3302
public class SharedWorldObjects_Loader : MonoBehaviour
{
	// Token: 0x17001F00 RID: 7936
	// (get) Token: 0x06005E09 RID: 24073 RVA: 0x00033C79 File Offset: 0x00031E79
	// (set) Token: 0x06005E0A RID: 24074 RVA: 0x00033C80 File Offset: 0x00031E80
	public static bool IsInitialized { get; private set; }

	// Token: 0x06005E0B RID: 24075 RVA: 0x0015F550 File Offset: 0x0015D750
	private void Awake()
	{
		if (SharedWorldObjects_Loader.m_instance == null)
		{
			SharedWorldObjects_Loader.m_instance = this;
			base.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
			SceneManager.sceneUnloaded += this.OnSceneUnloaded;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			base.StartCoroutine(this.LoadAsync());
			return;
		}
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x0015F5E4 File Offset: 0x0015D7E4
	private void LoadSync()
	{
		this.m_rootGameObject = new GameObject("SharedWorldObjects", new Type[]
		{
			typeof(SharedWorldObjects)
		});
		UnityEngine.Object.DontDestroyOnLoad(this.m_rootGameObject);
		GameObject gameObject = CDGResources.Load<GameObject>(this.m_sharedWorldObjects, "", true);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			ILoadable component = UnityEngine.Object.Instantiate<GameObject>(gameObject.transform.GetChild(i).gameObject, this.m_rootGameObject.transform).GetComponent<ILoadable>();
			if (component != null)
			{
				component.LoadSync();
			}
		}
		SharedWorldObjects_Loader.IsInitialized = true;
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x00033C88 File Offset: 0x00031E88
	private IEnumerator LoadAsync()
	{
		this.m_rootGameObject = new GameObject("SharedWorldObjects", new Type[]
		{
			typeof(SharedWorldObjects)
		});
		UnityEngine.Object.DontDestroyOnLoad(this.m_rootGameObject);
		CDGAsyncLoadRequest<GameObject> sharedWorldObjectsReq = CDGResources.LoadAsync<GameObject>(this.m_sharedWorldObjects, "");
		while (!sharedWorldObjectsReq.IsDone)
		{
			yield return null;
		}
		GameObject prefab = sharedWorldObjectsReq.Asset;
		int num;
		for (int i = 0; i < prefab.transform.childCount; i = num + 1)
		{
			ILoadable component = UnityEngine.Object.Instantiate<GameObject>(prefab.transform.GetChild(i).gameObject, this.m_rootGameObject.transform).GetComponent<ILoadable>();
			if (component != null)
			{
				yield return component.LoadAsync();
			}
			yield return null;
			num = i;
		}
		yield return null;
		SharedWorldObjects_Loader.IsInitialized = true;
		yield break;
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x00033C97 File Offset: 0x00031E97
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (this.m_destroyImmediatelyOnLoadScenes.Contains(scene.name))
		{
			SharedWorldObjectPoolManager.DestroyAllPools();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x00033CBD File Offset: 0x00031EBD
	private void OnSceneUnloaded(Scene scene)
	{
		if (this.m_destroyOnUnloadScenes.Contains(scene.name))
		{
			SharedWorldObjectPoolManager.DestroyAllPools();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x0015F680 File Offset: 0x0015D880
	private void OnDestroy()
	{
		if (SharedWorldObjects_Loader.m_instance == this)
		{
			UnityEngine.Object.Destroy(this.m_rootGameObject);
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SharedWorldObjects_Loader.IsInitialized = false;
			SharedWorldObjects_Loader.m_instance = null;
		}
	}

	// Token: 0x04004D40 RID: 19776
	[SerializeField]
	private string m_sharedWorldObjects;

	// Token: 0x04004D41 RID: 19777
	[SerializeField]
	private List<string> m_destroyImmediatelyOnLoadScenes;

	// Token: 0x04004D42 RID: 19778
	[SerializeField]
	private List<string> m_destroyOnUnloadScenes;

	// Token: 0x04004D43 RID: 19779
	private static SharedWorldObjects_Loader m_instance;

	// Token: 0x04004D44 RID: 19780
	private GameObject m_rootGameObject;
}
