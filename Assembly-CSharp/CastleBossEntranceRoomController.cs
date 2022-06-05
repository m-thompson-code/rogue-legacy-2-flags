using System;
using System.Collections;
using Cinemachine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public class CastleBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x170011B4 RID: 4532
	// (get) Token: 0x06002F73 RID: 12147 RVA: 0x000A2370 File Offset: 0x000A0570
	public IRelayLink DoorUnlockedRelay
	{
		get
		{
			return this.m_doorUnlockedRelay.link;
		}
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x000A237D File Offset: 0x000A057D
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
		this.m_onTorchHit = new Action<TorchesRoomPropController>(this.OnTorchHit);
	}

	// Token: 0x06002F75 RID: 12149 RVA: 0x000A23C0 File Offset: 0x000A05C0
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		this.m_leftTorchProp = this.m_torch1.PropInstance.gameObject.GetComponent<TorchesRoomPropController>();
		this.m_leftTorchProp.OnHitRelay.AddListener(this.m_onTorchHit, false);
		this.m_rightTorchProp = this.m_torch2.PropInstance.gameObject.GetComponent<TorchesRoomPropController>();
		this.m_rightTorchProp.OnHitRelay.AddListener(this.m_onTorchHit, false);
		if (TraitManager.IsTraitActive(TraitType.YouAreLarge))
		{
			this.m_leftTorchProp.transform.position += new Vector3(0f, 0f, 0f);
			this.m_rightTorchProp.transform.position += new Vector3(0f, 0f, 0f);
		}
		else if (TraitManager.IsTraitActive(TraitType.YouAreSmall))
		{
			this.m_leftTorchProp.transform.position += new Vector3(0f, 0f, 0f);
			this.m_rightTorchProp.transform.position += new Vector3(0f, 0f, 0f);
		}
		if (!base.IsRoomComplete)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(InsightType.CastleBoss_DoorOpened) >= InsightState.ResolvedButNotViewed)
			{
				this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
				this.m_leftTorchProp.OnHitRelay.RemoveListener(new Action<TorchesRoomPropController>(this.OnTorchHit));
				this.m_rightTorchProp.OnHitRelay.RemoveListener(new Action<TorchesRoomPropController>(this.OnTorchHit));
				return;
			}
			this.SetBossTunnelState(BossTunnelState.Closed, true);
		}
	}

	// Token: 0x06002F76 RID: 12150 RVA: 0x000A2570 File Offset: 0x000A0770
	protected override void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerExitRoom(sender, eventArgs);
		if (this.m_leftTorchProp)
		{
			this.m_leftTorchProp.OnHitRelay.RemoveListener(new Action<TorchesRoomPropController>(this.OnTorchHit));
		}
		if (this.m_rightTorchProp)
		{
			this.m_rightTorchProp.OnHitRelay.RemoveListener(new Action<TorchesRoomPropController>(this.OnTorchHit));
		}
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x000A25DC File Offset: 0x000A07DC
	private void OnTorchHit(TorchesRoomPropController torch)
	{
		if (!base.IsRoomComplete && this.m_leftTorchProp && this.m_rightTorchProp && this.m_leftTorchProp.IsFlameOn && this.m_rightTorchProp.IsFlameOn)
		{
			this.m_leftTorchProp.KeepFlameOn = true;
			this.m_rightTorchProp.KeepFlameOn = true;
			base.StartCoroutine(this.OpenDoorCoroutine());
		}
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x000A264A File Offset: 0x000A084A
	private IEnumerator OpenDoorCoroutine()
	{
		this.m_doorUnlockedRelay.Dispatch();
		RewiredMapController.SetCurrentMapEnabled(false);
		CinemachineVirtualCameraManager roomVcam = base.Room.CinemachineCamera;
		CinemachineBlendDefinition storedBlend = CameraController.CinemachineBrain.m_DefaultBlend;
		CinemachineBlendDefinition defaultBlend = default(CinemachineBlendDefinition);
		defaultBlend.m_Time = 1f;
		defaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
		CameraController.CinemachineBrain.m_DefaultBlend = defaultBlend;
		roomVcam.SetIsActiveCamera(false);
		this.m_doorVcam.gameObject.SetActive(true);
		this.m_doorVcam.SetIsActiveCamera(true);
		this.m_doorVcam.SetFollowTarget(base.BossTunnelSpawner.transform);
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		this.SetBossTunnelState(BossTunnelState.PartlyOpen, false);
		this.m_waitYield.CreateNew(1.5f, false);
		yield return this.m_waitYield;
		this.m_doorVcam.SetIsActiveCamera(false);
		this.m_doorVcam.gameObject.SetActive(false);
		roomVcam.SetIsActiveCamera(true);
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		CameraController.CinemachineBrain.m_DefaultBlend = storedBlend;
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.CastleBoss_DoorOpened) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.CastleBoss_DoorOpened, InsightState.ResolvedButNotViewed, false);
			this.m_insightEventArgs.Initialize(InsightType.CastleBoss_DoorOpened, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x040025D5 RID: 9685
	[SerializeField]
	private PropSpawnController m_torch1;

	// Token: 0x040025D6 RID: 9686
	[SerializeField]
	private PropSpawnController m_torch2;

	// Token: 0x040025D7 RID: 9687
	[SerializeField]
	private CinemachineVirtualCameraManager m_doorVcam;

	// Token: 0x040025D8 RID: 9688
	private TorchesRoomPropController m_leftTorchProp;

	// Token: 0x040025D9 RID: 9689
	private TorchesRoomPropController m_rightTorchProp;

	// Token: 0x040025DA RID: 9690
	private Relay m_doorUnlockedRelay = new Relay();

	// Token: 0x040025DB RID: 9691
	private Vector3 m_storedTorchPos1;

	// Token: 0x040025DC RID: 9692
	private Vector3 m_storedTorchPos2;

	// Token: 0x040025DD RID: 9693
	private WaitRL_Yield m_waitYield;

	// Token: 0x040025DE RID: 9694
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x040025DF RID: 9695
	private Action<TorchesRoomPropController> m_onTorchHit;
}
