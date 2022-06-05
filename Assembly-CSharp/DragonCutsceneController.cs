using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020007FE RID: 2046
public class DragonCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x170016F0 RID: 5872
	// (get) Token: 0x06003F06 RID: 16134 RVA: 0x00022E00 File Offset: 0x00021000
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x170016F1 RID: 5873
	// (get) Token: 0x06003F07 RID: 16135 RVA: 0x00022E08 File Offset: 0x00021008
	// (set) Token: 0x06003F08 RID: 16136 RVA: 0x00022E10 File Offset: 0x00021010
	public BaseRoom Room { get; private set; }

	// Token: 0x170016F2 RID: 5874
	// (get) Token: 0x06003F09 RID: 16137 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x00022E19 File Offset: 0x00021019
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x00022E2C File Offset: 0x0002102C
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x00022E53 File Offset: 0x00021053
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x00022E7F File Offset: 0x0002107F
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x00022E8E File Offset: 0x0002108E
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x00022EA3 File Offset: 0x000210A3
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

	// Token: 0x04003153 RID: 12627
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x04003154 RID: 12628
	[SerializeField]
	private PropSpawnController m_dragonPropSpawner;

	// Token: 0x04003155 RID: 12629
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x04003156 RID: 12630
	private WaitRL_Yield m_waitYield;
}
