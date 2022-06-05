using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000685 RID: 1669
public class StatusBarEntryManager : MonoBehaviour
{
	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x060032FE RID: 13054 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
	// (set) Token: 0x060032FF RID: 13055 RVA: 0x0001BEBB File Offset: 0x0001A0BB
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003300 RID: 13056 RVA: 0x0001BEC3 File Offset: 0x0001A0C3
	private void Awake()
	{
		this.Initialize();
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x0001BEDC File Offset: 0x0001A0DC
	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		StatusBarEntryManager.DisableAllStatusBarEntries();
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x0001BEE3 File Offset: 0x0001A0E3
	private void OnDestroy()
	{
		StatusBarEntryManager.IsInitialized = false;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		this.m_statusBarPool.DestroyPool();
		this.m_statusBarPool = null;
		StatusBarEntryManager.m_statusBarManager = null;
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x0001BF14 File Offset: 0x0001A114
	private void Initialize()
	{
		this.m_statusBarPool = new GenericPool_RL<StatusBarEntry>();
		this.m_statusBarPool.Initialize(this.m_statusBarPrefab, 20, false, true);
		StatusBarEntryManager.IsInitialized = true;
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x06003304 RID: 13060 RVA: 0x0001BF3C File Offset: 0x0001A13C
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

	// Token: 0x06003305 RID: 13061 RVA: 0x0001BF5B File Offset: 0x0001A15B
	public static StatusBarEntry GetFreeStatusBarEntry()
	{
		if (!StatusBarEntryManager.IsInitialized)
		{
			StatusBarEntryManager.Instance.Initialize();
		}
		return StatusBarEntryManager.Instance.m_statusBarPool.GetFreeObj();
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x0001BF7D File Offset: 0x0001A17D
	public static void DisableAllStatusBarEntries()
	{
		if (StatusBarEntryManager.IsInitialized)
		{
			StatusBarEntryManager.Instance.m_statusBarPool.DisableAll();
		}
	}

	// Token: 0x040029A4 RID: 10660
	private const int POOL_SIZE = 20;

	// Token: 0x040029A5 RID: 10661
	[SerializeField]
	private StatusBarEntry m_statusBarPrefab;

	// Token: 0x040029A6 RID: 10662
	private GenericPool_RL<StatusBarEntry> m_statusBarPool;

	// Token: 0x040029A7 RID: 10663
	private static StatusBarEntryManager m_statusBarManager;
}
