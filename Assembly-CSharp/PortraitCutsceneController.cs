using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000821 RID: 2081
public class PortraitCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x1700173C RID: 5948
	// (get) Token: 0x06004034 RID: 16436 RVA: 0x000236E5 File Offset: 0x000218E5
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x1700173D RID: 5949
	// (get) Token: 0x06004035 RID: 16437 RVA: 0x000236ED File Offset: 0x000218ED
	// (set) Token: 0x06004036 RID: 16438 RVA: 0x000236F5 File Offset: 0x000218F5
	public BaseRoom Room { get; private set; }

	// Token: 0x1700173E RID: 5950
	// (get) Token: 0x06004037 RID: 16439 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x000236FE File Offset: 0x000218FE
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x00023711 File Offset: 0x00021911
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x00023738 File Offset: 0x00021938
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x00023764 File Offset: 0x00021964
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x00023773 File Offset: 0x00021973
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00023788 File Offset: 0x00021988
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

	// Token: 0x0400323D RID: 12861
	private const string DOOR_UNLOCK_LEFT_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_left";

	// Token: 0x0400323E RID: 12862
	private const string DOOR_UNLOCK_CENTER_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_center";

	// Token: 0x0400323F RID: 12863
	private const string DOOR_UNLOCK_RIGHT_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_right";

	// Token: 0x04003240 RID: 12864
	private const string DOOR_OPEN_AUDIO_PATH = "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_doorOpen";

	// Token: 0x04003241 RID: 12865
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x04003242 RID: 12866
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x04003243 RID: 12867
	private WaitRL_Yield m_waitYield;
}
