using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class PortraitCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x17001183 RID: 4483
	// (get) Token: 0x06002E92 RID: 11922 RVA: 0x0009E59C File Offset: 0x0009C79C
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x17001184 RID: 4484
	// (get) Token: 0x06002E93 RID: 11923 RVA: 0x0009E5A4 File Offset: 0x0009C7A4
	// (set) Token: 0x06002E94 RID: 11924 RVA: 0x0009E5AC File Offset: 0x0009C7AC
	public BaseRoom Room { get; private set; }

	// Token: 0x17001185 RID: 4485
	// (get) Token: 0x06002E95 RID: 11925 RVA: 0x0009E5B5 File Offset: 0x0009C7B5
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x0009E5BD File Offset: 0x0009C7BD
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x0009E5D0 File Offset: 0x0009C7D0
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x0009E5F7 File Offset: 0x0009C7F7
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x0009E623 File Offset: 0x0009C823
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x0009E632 File Offset: 0x0009C832
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x0009E647 File Offset: 0x0009C847
	private IEnumerator BossDoorAnimationCoroutine()
	{
		CutsceneManager.SetTraitsEnabled(false);
		yield return null;
		BossEntranceRoomController bossEntranceController = this.Room.gameObject.GetComponent<BossEntranceRoomController>();
		PortraitTunnelComponent portraitTunnel;
		if (bossEntranceController)
		{
			portraitTunnel = bossEntranceController.BossTunnelSpawner.Tunnel.gameObject.GetComponent<PortraitTunnelComponent>();
		}
		else
		{
			portraitTunnel = this.m_tunnelSpawner.Tunnel.GameObject.GetComponent<PortraitTunnelComponent>();
		}
		portraitTunnel.SetBossDefeated(CutsceneManager.CutsceneSaveFlag, false);
		if (bossEntranceController)
		{
			bossEntranceController.SetBossTunnelState(BossTunnelState.Closed, true);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.Room.CinemachineCamera.SetFollowTarget(this.m_cutsceneCameraGO.transform);
		playerController.StatusBarController.SetCanvasVisible(false);
		playerController.Visuals.SetActive(false);
		playerController.ControllerCorgi.enabled = false;
		playerController.HitboxController.DisableAllCollisions = true;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, this, null);
		PlayerSaveFlag cutsceneSaveFlag = CutsceneManager.CutsceneSaveFlag;
		if (cutsceneSaveFlag <= PlayerSaveFlag.BridgeBoss_Defeated)
		{
			if (cutsceneSaveFlag == PlayerSaveFlag.CastleBoss_Defeated)
			{
				goto IL_18F;
			}
			if (cutsceneSaveFlag == PlayerSaveFlag.ForestBoss_Defeated)
			{
				goto IL_1A2;
			}
			if (cutsceneSaveFlag != PlayerSaveFlag.BridgeBoss_Defeated)
			{
				goto IL_1C8;
			}
			goto IL_18F;
		}
		else
		{
			switch (cutsceneSaveFlag)
			{
			case PlayerSaveFlag.StudyBoss_Defeated:
				goto IL_1A2;
			case PlayerSaveFlag.StudyMiniboss_SwordKnight_Defeated:
				break;
			case PlayerSaveFlag.StudyMiniboss_SpearKnight_Defeated:
				goto IL_18F;
			default:
				if (cutsceneSaveFlag != PlayerSaveFlag.TowerBoss_Defeated && cutsceneSaveFlag != PlayerSaveFlag.CaveBoss_Defeated)
				{
					goto IL_1C8;
				}
				break;
			}
			AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_right", base.gameObject);
		}
		IL_1FA:
		while (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		yield return portraitTunnel.BossDefeatedCoroutine(CutsceneManager.CutsceneSaveFlag);
		if (portraitTunnel.IsPortraitComplete)
		{
			if (bossEntranceController)
			{
				this.m_waitYield.CreateNew(1f, false);
				yield return this.m_waitYield;
				AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_doorOpen", base.gameObject);
				bossEntranceController.SetBossTunnelState(BossTunnelState.PartlyOpen, false);
				this.m_waitYield.CreateNew(2f, false);
				yield return this.m_waitYield;
			}
		}
		else
		{
			this.m_waitYield.CreateNew(2f, false);
			yield return this.m_waitYield;
		}
		this.m_tunnelSpawner.Tunnel.ForceEnterTunnel(true, CutsceneManager.ExitRoomTunnel);
		RewiredMapController.SetIsInCutscene(false);
		CutsceneManager.ResetCutscene();
		yield break;
		IL_18F:
		AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_left", base.gameObject);
		goto IL_1FA;
		IL_1A2:
		AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_center", base.gameObject);
		goto IL_1FA;
		IL_1C8:
		Debug.LogWarning(string.Format("<color=red>The PlayerSaveFlag {0} is not recognized by PortraitCutsceneController. Please add a report to Pivotal.</color>", CutsceneManager.CutsceneSaveFlag));
		goto IL_1FA;
	}

	// Token: 0x04002527 RID: 9511
	private const string DOOR_UNLOCK_LEFT_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_left";

	// Token: 0x04002528 RID: 9512
	private const string DOOR_UNLOCK_CENTER_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_center";

	// Token: 0x04002529 RID: 9513
	private const string DOOR_UNLOCK_RIGHT_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_right";

	// Token: 0x0400252A RID: 9514
	private const string DOOR_OPEN_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_doorOpen";

	// Token: 0x0400252B RID: 9515
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x0400252C RID: 9516
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x0400252D RID: 9517
	private WaitRL_Yield m_waitYield;
}
