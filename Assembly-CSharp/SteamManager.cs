using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700160D RID: 5645
	// (get) Token: 0x06004082 RID: 16514 RVA: 0x000E4C48 File Offset: 0x000E2E48
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x1700160E RID: 5646
	// (get) Token: 0x06004083 RID: 16515 RVA: 0x000E4C6C File Offset: 0x000E2E6C
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x000E4C78 File Offset: 0x000E2E78
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x000E4C80 File Offset: 0x000E2E80
	protected virtual void Awake()
	{
		if (SteamManager.s_instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (base.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)1253920U))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.Log("<color=red>[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.</color>", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06004086 RID: 16518 RVA: 0x000E4D74 File Offset: 0x000E2F74
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x000E4DC2 File Offset: 0x000E2FC2
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x000E4DE6 File Offset: 0x000E2FE6
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040031C6 RID: 12742
	protected static SteamManager s_instance;

	// Token: 0x040031C7 RID: 12743
	protected static bool s_EverInitialized;

	// Token: 0x040031C8 RID: 12744
	protected bool m_bInitialized;

	// Token: 0x040031C9 RID: 12745
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
