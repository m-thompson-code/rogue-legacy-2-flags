using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x02000BA6 RID: 2982
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17001E09 RID: 7689
	// (get) Token: 0x060059CB RID: 22987 RVA: 0x00030F8B File Offset: 0x0002F18B
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

	// Token: 0x17001E0A RID: 7690
	// (get) Token: 0x060059CC RID: 22988 RVA: 0x00030FAF File Offset: 0x0002F1AF
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060059CD RID: 22989 RVA: 0x00030FBB File Offset: 0x0002F1BB
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060059CE RID: 22990 RVA: 0x0015462C File Offset: 0x0015282C
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

	// Token: 0x060059CF RID: 22991 RVA: 0x00154720 File Offset: 0x00152920
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

	// Token: 0x060059D0 RID: 22992 RVA: 0x00030FC3 File Offset: 0x0002F1C3
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

	// Token: 0x060059D1 RID: 22993 RVA: 0x00030FE7 File Offset: 0x0002F1E7
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04004441 RID: 17473
	protected static SteamManager s_instance;

	// Token: 0x04004442 RID: 17474
	protected static bool s_EverInitialized;

	// Token: 0x04004443 RID: 17475
	protected bool m_bInitialized;

	// Token: 0x04004444 RID: 17476
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
