using System;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
public class PlayerManager : MonoBehaviour
{
	// Token: 0x1700155F RID: 5471
	// (get) Token: 0x06003E12 RID: 15890 RVA: 0x000D9670 File Offset: 0x000D7870
	public static bool IsDisposed
	{
		get
		{
			return PlayerManager.m_isDisposed;
		}
	}

	// Token: 0x17001560 RID: 5472
	// (get) Token: 0x06003E13 RID: 15891 RVA: 0x000D9678 File Offset: 0x000D7878
	private static PlayerManager Instance
	{
		get
		{
			if (PlayerManager.m_isDisposed)
			{
				Debug.Log("ERROR: PlayerManager Instance is being fetched after PlayerManager has been destroyed. STACK TRACE:" + Environment.StackTrace);
			}
			if (!PlayerManager.m_isInitialized)
			{
				Debug.Log("ERROR: PlayerManager has not been initialized.  Instantiate a new instance of PlayerManager.");
			}
			if (!PlayerManager.m_playerManager)
			{
				PlayerManager.m_playerManager = CDGHelper.FindStaticInstance<PlayerManager>(false);
			}
			return PlayerManager.m_playerManager;
		}
	}

	// Token: 0x17001561 RID: 5473
	// (get) Token: 0x06003E14 RID: 15892 RVA: 0x000D96CD File Offset: 0x000D78CD
	public static bool IsInstantiated
	{
		get
		{
			return PlayerManager.m_isInitialized && PlayerManager.m_playerManager && PlayerManager.m_playerManager.m_playerController;
		}
	}

	// Token: 0x06003E15 RID: 15893 RVA: 0x000D96F3 File Offset: 0x000D78F3
	public static GameObject GetPlayer()
	{
		return PlayerManager.Instance.InternalGetPlayer();
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x000D96FF File Offset: 0x000D78FF
	public static PlayerController GetPlayerController()
	{
		return PlayerManager.Instance.InternalGetPlayerController();
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x000D970B File Offset: 0x000D790B
	public static BaseRoom GetCurrentPlayerRoom()
	{
		return PlayerManager.GetPlayerController().CurrentlyInRoom;
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x000D9717 File Offset: 0x000D7917
	private GameObject InternalGetPlayer()
	{
		return this.m_player;
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x000D971F File Offset: 0x000D791F
	private PlayerController InternalGetPlayerController()
	{
		return this.m_playerController;
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x000D9727 File Offset: 0x000D7927
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x000D972F File Offset: 0x000D792F
	public void Initialize()
	{
		PlayerManager.m_playerManager = this;
		this.InitializePlayer();
		PlayerManager.m_isInitialized = true;
		PlayerManager.m_isDisposed = false;
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x000D974C File Offset: 0x000D794C
	private void InitializePlayer()
	{
		this.m_player = GameObject.FindGameObjectWithTag("Player");
		if (!this.m_player)
		{
			this.m_player = UnityEngine.Object.Instantiate<GameObject>(this.m_playerPrefab);
			this.m_player.name = "Player";
		}
		UnityEngine.Object.DontDestroyOnLoad(this.m_player);
		this.m_playerController = this.m_player.GetComponent<PlayerController>();
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x000D97B3 File Offset: 0x000D79B3
	private void OnDestroy()
	{
		PlayerManager.m_isDisposed = true;
		PlayerManager.m_isInitialized = false;
		UnityEngine.Object.Destroy(this.m_player);
		PlayerManager.m_playerManager = null;
	}

	// Token: 0x04002E41 RID: 11841
	private const string PLAYERMANAGER_NAME = "PlayerManager";

	// Token: 0x04002E42 RID: 11842
	private const string RESOURCE_PATH = "Prefabs/Managers/PlayerManager";

	// Token: 0x04002E43 RID: 11843
	[SerializeField]
	private GameObject m_playerPrefab;

	// Token: 0x04002E44 RID: 11844
	private GameObject m_player;

	// Token: 0x04002E45 RID: 11845
	private PlayerController m_playerController;

	// Token: 0x04002E46 RID: 11846
	private static bool m_isDisposed;

	// Token: 0x04002E47 RID: 11847
	private static PlayerManager m_playerManager;

	// Token: 0x04002E48 RID: 11848
	private static bool m_isInitialized;
}
