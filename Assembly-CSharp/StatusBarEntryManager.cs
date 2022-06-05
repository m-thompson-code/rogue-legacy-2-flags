using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003E4 RID: 996
public class StatusBarEntryManager : MonoBehaviour
{
	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x060024BC RID: 9404 RVA: 0x0007A519 File Offset: 0x00078719
	// (set) Token: 0x060024BD RID: 9405 RVA: 0x0007A520 File Offset: 0x00078720
	public static bool IsInitialized { get; private set; }

	// Token: 0x060024BE RID: 9406 RVA: 0x0007A528 File Offset: 0x00078728
	private void Awake()
	{
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x0007A541 File Offset: 0x00078741
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		StatusBarEntryManager.DisableAllStatusBarEntries();
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x0007A548 File Offset: 0x00078748
	private void OnDestroy()
	{
		StatusBarEntryManager.IsInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		this.m_statusBarPool.DestroyPool();
		this.m_statusBarPool = null;
		StatusBarEntryManager.m_statusBarManager = null;
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x0007A579 File Offset: 0x00078779
	private void Initialize()
	{
		this.m_statusBarPool = new GenericPool_RL<StatusBarEntry>();
		this.m_statusBarPool.Initialize(this.m_statusBarPrefab, 20, false, true);
		StatusBarEntryManager.IsInitialized = true;
	}

	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x060024C2 RID: 9410 RVA: 0x0007A5A1 File Offset: 0x000787A1
	private static StatusBarEntryManager Instance
	{
		get
		{
			if (StatusBarEntryManager.m_statusBarManager == null)
			{
				StatusBarEntryManager.m_statusBarManager = CDGHelper.FindStaticInstance<StatusBarEntryManager>(false);
			}
			return StatusBarEntryManager.m_statusBarManager;
		}
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x0007A5C0 File Offset: 0x000787C0
	public static StatusBarEntry GetFreeStatusBarEntry()
	{
		if (!StatusBarEntryManager.IsInitialized)
		{
			StatusBarEntryManager.Instance.Initialize();
		}
		return StatusBarEntryManager.Instance.m_statusBarPool.GetFreeObj();
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x0007A5E2 File Offset: 0x000787E2
	public static void DisableAllStatusBarEntries()
	{
		if (StatusBarEntryManager.IsInitialized)
		{
			StatusBarEntryManager.Instance.m_statusBarPool.DisableAll();
		}
	}

	// Token: 0x04001F26 RID: 7974
	private const int POOL_SIZE = 20;

	// Token: 0x04001F27 RID: 7975
	[SerializeField]
	private StatusBarEntry m_statusBarPrefab;

	// Token: 0x04001F28 RID: 7976
	private GenericPool_RL<StatusBarEntry> m_statusBarPool;

	// Token: 0x04001F29 RID: 7977
	private static StatusBarEntryManager m_statusBarManager;
}
