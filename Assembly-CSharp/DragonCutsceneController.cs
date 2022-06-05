using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class DragonCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x1700115F RID: 4447
	// (get) Token: 0x06002DDC RID: 11740 RVA: 0x0009A8B4 File Offset: 0x00098AB4
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x17001160 RID: 4448
	// (get) Token: 0x06002DDD RID: 11741 RVA: 0x0009A8BC File Offset: 0x00098ABC
	// (set) Token: 0x06002DDE RID: 11742 RVA: 0x0009A8C4 File Offset: 0x00098AC4
	public BaseRoom Room { get; private set; }

	// Token: 0x17001161 RID: 4449
	// (get) Token: 0x06002DDF RID: 11743 RVA: 0x0009A8CD File Offset: 0x00098ACD
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x0009A8D5 File Offset: 0x00098AD5
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x0009A8E8 File Offset: 0x00098AE8
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x0009A90F File Offset: 0x00098B0F
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x0009A93B File Offset: 0x00098B3B
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x0009A94A File Offset: 0x00098B4A
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x0009A95F File Offset: 0x00098B5F
	private IEnumerator BossDoorAnimationCoroutine()
	{
		Animator dragonAnimator = this.m_dragonPropSpawner.PropInstance.GetComponent<Animator>();
		if (CutsceneManager.CutsceneSaveFlag == PlayerSaveFlag.CaveMiniboss_White_Defeated)
		{
			if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated))
			{
				dragonAnimator.SetTrigger("RightChainBreakInstant");
			}
		}
		else if (CutsceneManager.CutsceneSaveFlag == PlayerSaveFlag.CaveMiniboss_Black_Defeated && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated))
		{
			dragonAnimator.SetTrigger("LeftChainBreakInstant");
		}
		CutsceneManager.SetTraitsEnabled(false);
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.Room.CinemachineCamera.SetFollowTarget(this.m_cutsceneCameraGO.transform);
		playerController.StatusBarController.SetCanvasVisible(false);
		playerController.Visuals.SetActive(false);
		playerController.ControllerCorgi.enabled = false;
		playerController.HitboxController.DisableAllCollisions = true;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, this, null);
		while (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		bool flag = false;
		if (CutsceneManager.CutsceneSaveFlag == PlayerSaveFlag.CaveMiniboss_White_Defeated)
		{
			dragonAnimator.SetTrigger("LeftChainBreak");
			flag = true;
		}
		else if (CutsceneManager.CutsceneSaveFlag == PlayerSaveFlag.CaveMiniboss_Black_Defeated)
		{
			dragonAnimator.SetTrigger("RightChainBreak");
			flag = true;
		}
		if (flag)
		{
			this.m_waitYield.CreateNew(4f, false);
			yield return this.m_waitYield;
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated))
		{
			dragonAnimator.SetTrigger("CollarBreak");
			yield return null;
			this.m_waitYield.CreateNew(6f, false);
			yield return this.m_waitYield;
		}
		this.m_tunnelSpawner.Tunnel.ForceEnterTunnel(true, CutsceneManager.ExitRoomTunnel);
		RewiredMapController.SetIsInCutscene(false);
		CutsceneManager.ResetCutscene();
		yield break;
	}

	// Token: 0x040024A4 RID: 9380
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x040024A5 RID: 9381
	[SerializeField]
	private PropSpawnController m_dragonPropSpawner;

	// Token: 0x040024A6 RID: 9382
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x040024A7 RID: 9383
	private WaitRL_Yield m_waitYield;
}
