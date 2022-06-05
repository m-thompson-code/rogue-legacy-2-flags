using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000815 RID: 2069
public class SharedWorldObjects_Loader : MonoBehaviour
{
	// Token: 0x170016F8 RID: 5880
	// (get) Token: 0x0600444F RID: 17487 RVA: 0x000F1997 File Offset: 0x000EFB97
	// (set) Token: 0x06004450 RID: 17488 RVA: 0x000F199E File Offset: 0x000EFB9E
	public static bool IsInitialized { get; private set; }

	// Token: 0x06004451 RID: 17489 RVA: 0x000F19A8 File Offset: 0x000EFBA8
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

	// Token: 0x06004452 RID: 17490 RVA: 0x000F1A3C File Offset: 0x000EFC3C
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

	// Token: 0x06004453 RID: 17491 RVA: 0x000F1AD5 File Offset: 0x000EFCD5
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

	// Token: 0x06004454 RID: 17492 RVA: 0x000F1AE4 File Offset: 0x000EFCE4
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (this.m_destroyImmediatelyOnLoadScenes.Contains(scene.name))
		{
			SharedWorldObjectPoolManager.DestroyAllPools();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x06004455 RID: 17493 RVA: 0x000F1B0A File Offset: 0x000EFD0A
	private void OnSceneUnloaded(Scene scene)
	{
		if (this.m_destroyOnUnloadScenes.Contains(scene.name))
		{
			SharedWorldObjectPoolManager.DestroyAllPools();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06004456 RID: 17494 RVA: 0x000F1B30 File Offset: 0x000EFD30
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

	// Token: 0x04003A53 RID: 14931
	[SerializeField]
	private string m_sharedWorldObjects;

	// Token: 0x04003A54 RID: 14932
	[SerializeField]
	private List<string> m_destroyImmediatelyOnLoadScenes;

	// Token: 0x04003A55 RID: 14933
	[SerializeField]
	private List<string> m_destroyOnUnloadScenes;

	// Token: 0x04003A56 RID: 14934
	private static SharedWorldObjects_Loader m_instance;

	// Token: 0x04003A57 RID: 14935
	private GameObject m_rootGameObject;
}
