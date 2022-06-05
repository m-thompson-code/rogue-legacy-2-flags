using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004D7 RID: 1239
public class HubtownDudCutsceneController : MonoBehaviour, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x1700116B RID: 4459
	// (get) Token: 0x06002E2E RID: 11822 RVA: 0x0009C016 File Offset: 0x0009A216
	public TunnelSpawnController TunnelSpawner
	{
		get
		{
			return this.m_tunnelSpawner;
		}
	}

	// Token: 0x1700116C RID: 4460
	// (get) Token: 0x06002E2F RID: 11823 RVA: 0x0009C01E File Offset: 0x0009A21E
	// (set) Token: 0x06002E30 RID: 11824 RVA: 0x0009C026 File Offset: 0x0009A226
	public BaseRoom Room { get; private set; }

	// Token: 0x1700116D RID: 4461
	// (get) Token: 0x06002E31 RID: 11825 RVA: 0x0009C02F File Offset: 0x0009A22F
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x0009C037 File Offset: 0x0009A237
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x0009C04A File Offset: 0x0009A24A
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x0009C071 File Offset: 0x0009A271
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x0009C0A0 File Offset: 0x0009A2A0
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			this.m_oceanBG = this.m_oceanProp.PropInstance.transform.FindDeep("BGElements");
			this.m_oceanFG = this.m_oceanProp.PropInstance.transform.FindDeep("FGElements");
			this.RunBossDoorAnimation();
		}
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x0009C0FA File Offset: 0x0009A2FA
	private void RunBossDoorAnimation()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.BossDoorAnimationCoroutine());
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x0009C10F File Offset: 0x0009A30F
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

	// Token: 0x040024DB RID: 9435
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;

	// Token: 0x040024DC RID: 9436
	[SerializeField]
	private PropSpawnController m_oceanProp;

	// Token: 0x040024DD RID: 9437
	[SerializeField]
	private GameObject m_cutsceneCameraGO;

	// Token: 0x040024DE RID: 9438
	private WaitRL_Yield m_waitYield;

	// Token: 0x040024DF RID: 9439
	private Transform m_oceanBG;

	// Token: 0x040024E0 RID: 9440
	private Transform m_oceanFG;
}
