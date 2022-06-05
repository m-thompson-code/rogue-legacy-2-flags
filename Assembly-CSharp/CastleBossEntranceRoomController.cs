using System;
using System.Collections;
using Cinemachine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class CastleBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x17001797 RID: 6039
	// (get) Token: 0x06004194 RID: 16788 RVA: 0x000244FD File Offset: 0x000226FD
	public IRelayLink DoorUnlockedRelay
	{
		get
		{
			return this.m_doorUnlockedRelay.link;
		}
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x0002450A File Offset: 0x0002270A
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
		this.m_onTorchHit = new Action<TorchesRoomPropController>(this.OnTorchHit);
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x00107C84 File Offset: 0x00105E84
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

	// Token: 0x06004197 RID: 16791 RVA: 0x00107E34 File Offset: 0x00106034
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

	// Token: 0x06004198 RID: 16792 RVA: 0x00107EA0 File Offset: 0x001060A0
	private void OnTorchHit(TorchesRoomPropController torch)
	{
		if (!base.IsRoomComplete && this.m_leftTorchProp && this.m_rightTorchProp && this.m_leftTorchProp.IsFlameOn && this.m_rightTorchProp.IsFlameOn)
		{
			this.m_leftTorchProp.KeepFlameOn = true;
			this.m_rightTorchProp.KeepFlameOn = true;
			base.StartCoroutine(this.OpenDoorCoroutine());
		}
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0002454A File Offset: 0x0002274A
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

	// Token: 0x04003357 RID: 13143
	[SerializeField]
	private PropSpawnController m_torch1;

	// Token: 0x04003358 RID: 13144
	[SerializeField]
	private PropSpawnController m_torch2;

	// Token: 0x04003359 RID: 13145
	[SerializeField]
	private CinemachineVirtualCameraManager m_doorVcam;

	// Token: 0x0400335A RID: 13146
	private TorchesRoomPropController m_leftTorchProp;

	// Token: 0x0400335B RID: 13147
	private TorchesRoomPropController m_rightTorchProp;

	// Token: 0x0400335C RID: 13148
	private Relay m_doorUnlockedRelay = new Relay();

	// Token: 0x0400335D RID: 13149
	private Vector3 m_storedTorchPos1;

	// Token: 0x0400335E RID: 13150
	private Vector3 m_storedTorchPos2;

	// Token: 0x0400335F RID: 13151
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003360 RID: 13152
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs;

	// Token: 0x04003361 RID: 13153
	private Action<TorchesRoomPropController> m_onTorchHit;
}
