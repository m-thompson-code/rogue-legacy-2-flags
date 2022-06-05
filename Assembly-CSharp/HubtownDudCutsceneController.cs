using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000811 RID: 2065
public class HubtownDudCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x17001716 RID: 5910
	// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x000232E0 File Offset: 0x000214E0
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x17001717 RID: 5911
	// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x000232E8 File Offset: 0x000214E8
	// (set) Token: 0x06003FA8 RID: 16296 RVA: 0x000232F0 File Offset: 0x000214F0
	public BaseRoom Room { get; private set; }

	// Token: 0x17001718 RID: 5912
	// (get) Token: 0x06003FA9 RID: 16297 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x000232F9 File Offset: 0x000214F9
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x0002330C File Offset: 0x0002150C
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x00023333 File Offset: 0x00021533
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x000FEE04 File Offset: 0x000FD004
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.m_oceanBG = this.m_oceanProp.PropInstance.transform.FindDeep("BGElements");
			this.m_oceanFG = this.m_oceanProp.PropInstance.transform.FindDeep("FGElements");
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x0002335F File Offset: 0x0002155F
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x06003FAF RID: 16303 RVA: 0x00023374 File Offset: 0x00021574
	private IEnumerator BossDoorAnimationCoroutine()
	{
		BaseEffect cameraShake = EffectManager.PlayEffect(base.gameObject, null, "CameraShakeSmall_Effect", Vector2.zero, 999f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		CutsceneManager.SetTraitsEnabled(false);
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.Room.CinemachineCamera.SetFollowTarget(this.m_cutsceneCameraGO.transform);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_ending_risingWaters", this.m_cutsceneCameraGO.transform.position);
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
		TweenManager.TweenBy(this.m_oceanBG, 4f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			4
		});
		TweenManager.TweenBy(this.m_oceanFG, 4f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			4
		});
		this.m_waitYield.CreateNew(4.1f, false);
		yield return this.m_waitYield;
		this.m_tunnelSpawner.Tunnel.ForceEnterTunnel(true, CutsceneManager.ExitRoomTunnel);
		RewiredMapController.SetIsInCutscene(false);
		CutsceneManager.ResetCutscene();
		cameraShake.Stop(EffectStopType.Immediate);
		yield break;
	}

	// Token: 0x040031BD RID: 12733
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x040031BE RID: 12734
	[SerializeField]
	private PropSpawnController m_oceanProp;

	// Token: 0x040031BF RID: 12735
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x040031C0 RID: 12736
	private WaitRL_Yield m_waitYield;

	// Token: 0x040031C1 RID: 12737
	private Transform m_oceanBG;

	// Token: 0x040031C2 RID: 12738
	private Transform m_oceanFG;
}
