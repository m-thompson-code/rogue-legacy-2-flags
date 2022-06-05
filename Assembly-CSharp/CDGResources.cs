using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000C61 RID: 3169
public static class CDGResources
{
	// Token: 0x17001E5B RID: 7771
	// (get) Token: 0x06005B75 RID: 23413 RVA: 0x0003225A File Offset: 0x0003045A
	public static bool IsLoading
	{
		get
		{
			return CDGResources.m_isLoading;
		}
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x0015A150 File Offset: 0x00158350
	public static void Init(bool loadSynchronous)
	{
		if (CDGResources.m_hasInitialized)
		{
			return;
		}
		CDGResources.m_hasInitialized = true;
		UnityEngine.Debug.Log("CDGResources backend: Asset Bundles");
		string path = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
		string[] allAssetBundles = AssetBundle.LoadFromFile(Path.Combine(path, "AssetBundles")).LoadAsset<AssetBundleManifest>("assetbundlemanifest").GetAllAssetBundles();
		CDGResources.m_bundles = new Dictionary<string, AssetBundle>(allAssetBundles.Length);
		if (loadSynchronous)
		{
			foreach (string text in allAssetBundles)
			{
				CDGResources.m_bundles[text] = AssetBundle.LoadFromFile(Path.Combine(path, text));
			}
			return;
		}
		new GameObject("Async Loader").AddComponent<CDGResources.CDGResources_AsyncLoader>().LoadBundlesAsync(allAssetBundles);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x0015A1FC File Offset: 0x001583FC
	public static T Load<T>(string path, string bundleName = "", bool throwOnNull = true) where T : UnityEngine.Object
	{
		string text = (typeof(T) == typeof(GameObject)) ? ".prefab" : ".asset";
		string text2 = "Assets/Content/" + path + text;
		text2 = text2.ToLower();
		if (bundleName == "")
		{
			bundleName = ((text == ".asset") ? "scriptable_objects" : "prefabs");
		}
		T t = CDGResources.m_bundles[bundleName].LoadAsset<T>(text2);
		if (t == null && throwOnNull)
		{
			throw new FileNotFoundException("Could not locate asset: " + text2);
		}
		return t;
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x0015A2A0 File Offset: 0x001584A0
	public static CDGAsyncLoadRequest<T> LoadAsync<T>(string path, string bundleName = "") where T : UnityEngine.Object
	{
		string text = (typeof(T) == typeof(GameObject)) ? ".prefab" : ".asset";
		string text2 = "Assets/Content/" + path + text;
		text2 = text2.ToLower();
		if (bundleName == "")
		{
			bundleName = ((text == ".asset") ? "scriptable_objects" : "prefabs");
		}
		AssetBundleRequest request = CDGResources.m_bundles[bundleName].LoadAssetAsync<T>(text2);
		return new CDGAsyncLoadRequest<T>(false, default(T), request);
	}

	// Token: 0x04004BF4 RID: 19444
	private static bool m_hasInitialized;

	// Token: 0x04004BF5 RID: 19445
	private static bool m_isLoading;

	// Token: 0x04004BF6 RID: 19446
	private static Dictionary<string, AssetBundle> m_bundles;

	// Token: 0x04004BF7 RID: 19447
	public const string ROOT_DIRECTORY = "Assets/Content/";

	// Token: 0x04004BF8 RID: 19448
	private const string BUNDLE_SUBDIRECTORY_NAME = "AssetBundles";

	// Token: 0x02000C62 RID: 3170
	public class CDGResources_AsyncLoader : MonoBehaviour
	{
		// Token: 0x06005B79 RID: 23417 RVA: 0x00032261 File Offset: 0x00030461
		public void LoadBundlesAsync(string[] bundleNames)
		{
			QualitySettings.asyncUploadTimeSlice = 8;
			QualitySettings.asyncUploadBufferSize = 64;
			QualitySettings.asyncUploadPersistentBuffer = true;
			base.StartCoroutine(this.LoadBundlesCoroutine(bundleNames));
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x00032284 File Offset: 0x00030484
		private IEnumerator LoadBundlesCoroutine(string[] bundleNames)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			CDGResources.m_isLoading = true;
			string bundleRootPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
			foreach (string path in bundleNames)
			{
				CDGResources.CDGResources_AsyncLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CDGResources.CDGResources_AsyncLoader.<>c__DisplayClass3_0();
				CS$<>8__locals1.bundleFilePath = Path.Combine(bundleRootPath, path);
				if (this.m_loadFromMemory)
				{
					CS$<>8__locals1.fileBytes = null;
					Task task = Task.Run(delegate()
					{
						CS$<>8__locals1.fileBytes = File.ReadAllBytes(CS$<>8__locals1.bundleFilePath);
					});
					while (!task.IsCompleted)
					{
						yield return null;
					}
					AssetBundle.LoadFromMemoryAsync(CS$<>8__locals1.fileBytes).completed += this.OnBundleLoaded;
					task = null;
				}
				else
				{
					AssetBundle.LoadFromFileAsync(CS$<>8__locals1.bundleFilePath).completed += this.OnBundleLoaded;
				}
				CS$<>8__locals1 = null;
			}
			string[] array = null;
			while (this.m_numBundlesLoaded < bundleNames.Length)
			{
				yield return null;
			}
			sw.Stop();
			UnityEngine.Debug.Log("Async loading bundles into memory took: " + sw.Elapsed.TotalSeconds.ToString());
			CDGResources.m_isLoading = false;
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x0015A334 File Offset: 0x00158534
		private void OnBundleLoaded(AsyncOperation op)
		{
			AssetBundle assetBundle = (op as AssetBundleCreateRequest).assetBundle;
			CDGResources.m_bundles[assetBundle.name] = assetBundle;
			this.m_numBundlesLoaded++;
		}

		// Token: 0x04004BF9 RID: 19449
		private int m_numBundlesLoaded;

		// Token: 0x04004BFA RID: 19450
		private bool m_loadFromMemory;
	}
}
