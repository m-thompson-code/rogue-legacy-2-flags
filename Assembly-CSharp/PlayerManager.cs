using System;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
public class PlayerManager : MonoBehaviour
{
	// Token: 0x17001D47 RID: 7495
	// (get) Token: 0x06005704 RID: 22276 RVA: 0x0002F5EC File Offset: 0x0002D7EC
	public static bool IsDisposed
	{
		get
		{
			return PlayerManager.m_isDisposed;
		}
	}

	// Token: 0x17001D48 RID: 7496
	// (get) Token: 0x06005705 RID: 22277 RVA: 0x00149E54 File Offset: 0x00148054
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

	// Token: 0x17001D49 RID: 7497
	// (get) Token: 0x06005706 RID: 22278 RVA: 0x0002F5F3 File Offset: 0x0002D7F3
	public static bool IsInstantiated
	{
		get
		{
			return PlayerManager.m_isInitialized && PlayerManager.m_playerManager && PlayerManager.m_playerManager.m_playerController;
		}
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x0002F619 File Offset: 0x0002D819
	public static GameObject GetPlayer()
	{
		return PlayerManager.Instance.InternalGetPlayer();
	}

	// Token: 0x06005708 RID: 22280 RVA: 0x0002F625 File Offset: 0x0002D825
	public static PlayerController GetPlayerController()
	{
		return PlayerManager.Instance.InternalGetPlayerController();
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x0002F631 File Offset: 0x0002D831
	public static BaseRoom GetCurrentPlayerRoom()
	{
		return PlayerManager.GetPlayerController().CurrentlyInRoom;
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x0002F63D File Offset: 0x0002D83D
	private GameObject InternalGetPlayer()
	{
		return this.m_player;
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x0002F645 File Offset: 0x0002D845
	private PlayerController InternalGetPlayerController()
	{
		return this.m_playerController;
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x0002F64D File Offset: 0x0002D84D
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x0002F655 File Offset: 0x0002D855
	public void Initialize()
	{
		PlayerManager.m_playerManager = this;
		this.InitializePlayer();
		PlayerManager.m_isInitialized = true;
		PlayerManager.m_isDisposed = false;
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x00149EAC File Offset: 0x001480AC
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

	// Token: 0x0600570F RID: 22287 RVA: 0x0002F66F File Offset: 0x0002D86F
	private void OnDestroy()
	{
		PlayerManager.m_isDisposed = true;
		PlayerManager.m_isInitialized = false;
		UnityEngine.Object.Destroy(this.m_player);
		PlayerManager.m_playerManager = null;
	}

	// Token: 0x0400405E RID: 16478
	private const string PLAYERMANAGER_NAME = "PlayerManager";

	// Token: 0x0400405F RID: 16479
	private const string RESOURCE_PATH = "Prefabs/Managers/PlayerManager";

	// Token: 0x04004060 RID: 16480
	[SerializeField]
	private GameObject m_playerPrefab;

	// Token: 0x04004061 RID: 16481
	private GameObject m_player;

	// Token: 0x04004062 RID: 16482
	private PlayerController m_playerController;

	// Token: 0x04004063 RID: 16483
	private static bool m_isDisposed;

	// Token: 0x04004064 RID: 16484
	private static PlayerManager m_playerManager;

	// Token: 0x04004065 RID: 16485
	private static bool m_isInitialized;
}
