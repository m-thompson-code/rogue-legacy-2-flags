using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public static class CDGResources
{
	// Token: 0x1700165F RID: 5727
	// (get) Token: 0x060041F8 RID: 16888 RVA: 0x000EB1DA File Offset: 0x000E93DA
	public static bool IsLoading
	{
		get
		{
			return CDGResources.m_isLoading;
		}
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x000EB1E4 File Offset: 0x000E93E4
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

	// Token: 0x060041FA RID: 16890 RVA: 0x000EB290 File Offset: 0x000E9490
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

	// Token: 0x060041FB RID: 16891 RVA: 0x000EB334 File Offset: 0x000E9534
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

	// Token: 0x0400393D RID: 14653
	private static bool m_hasInitialized;

	// Token: 0x0400393E RID: 14654
	private static bool m_isLoading;

	// Token: 0x0400393F RID: 14655
	private static Dictionary<string, AssetBundle> m_bundles;

	// Token: 0x04003940 RID: 14656
	public const string ROOT_DIRECTORY = "Assets/Content/";

	// Token: 0x04003941 RID: 14657
	private const string BUNDLE_SUBDIRECTORY_NAME = "AssetBundles";

	// Token: 0x02000E37 RID: 3639
	public class CDGResources_AsyncLoader : MonoBehaviour
	{
		// Token: 0x06006BC9 RID: 27593 RVA: 0x00192B00 File Offset: 0x00190D00
		public void LoadBundlesAsync(string[] bundleNames)
		{
			QualitySettings.asyncUploadTimeSlice = 8;
			QualitySettings.asyncUploadBufferSize = 64;
			QualitySettings.asyncUploadPersistentBuffer = true;
			base.StartCoroutine(this.LoadBundlesCoroutine(bundleNames));
		}

		// Token: 0x06006BCA RID: 27594 RVA: 0x00192B23 File Offset: 0x00190D23
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

		// Token: 0x06006BCB RID: 27595 RVA: 0x00192B3C File Offset: 0x00190D3C
		private void OnBundleLoaded(AsyncOperation op)
		{
			AssetBundle assetBundle = (op as AssetBundleCreateRequest).assetBundle;
			CDGResources.m_bundles[assetBundle.name] = assetBundle;
			this.m_numBundlesLoaded++;
		}

		// Token: 0x04005739 RID: 22329
		private int m_numBundlesLoaded;

		// Token: 0x0400573A RID: 22330
		private bool m_loadFromMemory;
	}
}
