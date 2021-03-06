using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004EE RID: 1262
public class BridgeBossEntranceRoomController : BossEntranceRoomController, IAudioEventEmitter
{
	// Token: 0x170011B1 RID: 4529
	// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000A2133 File Offset: 0x000A0333
	public string Description
	{
		get
		{
			if (this.m_description == string.Empty)
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x06002F64 RID: 12132 RVA: 0x000A2159 File Offset: 0x000A0359
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06002F65 RID: 12133 RVA: 0x000A2174 File Offset: 0x000A0374
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		bool flag = SaveManager.PlayerSaveData.GetFlag(this.m_bossBeatenFlag) && BossEntranceRoomController.RunDoorCrumbleAnimation;
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (flag)
		{
			base.StartCoroutine(this.GateOpenAnimCoroutine());
			return;
		}
		if (base.IsRoomComplete)
		{
			Vector3 position = this.m_gateEndingPosGO.transform.position;
			position.z = this.m_gateSpawner.PropInstance.gameObject.transform.position.z;
			this.m_gateSpawner.PropInstance.gameObject.transform.position = position;
		}
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x000A220D File Offset: 0x000A040D
	private IEnumerator GateOpenAnimCoroutine()
	{
		RewiredMapController.SetIsInCutscene(true);
		while (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_waitYield.CreateNew(2f, false);
		yield return this.m_waitYield;
		float num = CameraController.GameCamera.orthographicSize * (float)Screen.width / (float)Screen.height;
		float num2 = CameraController.GameCamera.transform.localPosition.x + num;
		float x = base.Room.Bounds.max.x;
		float transitionAmount = x - num2;
		this.m_cameraMovementObj.transform.position = PlayerManager.GetPlayerController().transform.position;
		base.Room.CinemachineCamera.SetFollowTarget(this.m_cameraMovementObj);
		yield return TweenManager.TweenBy(this.m_cameraMovementObj, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			transitionAmount
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		EffectManager.PlayEffect(base.gameObject, null, "CameraShakeSmall_Effect", Vector3.zero, 3f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Interactables/sfx_env_gate_openSequence", this.m_gateSpawner.PropInstance.gameObject);
		yield return TweenManager.TweenTo(this.m_gateSpawner.PropInstance.gameObject.transform, 3f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.x",
			this.m_gateEndingPosGO.transform.position.x,
			"position.y",
			this.m_gateEndingPosGO.transform.position.y
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		yield return TweenManager.TweenBy(this.m_cameraMovementObj, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			-transitionAmount
		}).TweenCoroutine;
		base.Room.CinemachineCamera.SetFollowTarget(PlayerManager.GetPlayerController().FollowTargetGO.transform);
		RewiredMapController.SetCurrentMapEnabled(true);
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.BridgeBoss_GateRaised) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.BridgeBoss_GateRaised, InsightState.ResolvedButNotViewed, false);
			InsightObjectiveCompleteHUDEventArgs eventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.BridgeBoss_GateRaised, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, eventArgs);
		}
		RewiredMapController.SetIsInCutscene(false);
		yield break;
	}

	// Token: 0x040025C6 RID: 9670
	[SerializeField]
	private PropSpawnController m_gateSpawner;

	// Token: 0x040025C7 RID: 9671
	[SerializeField]
	private GameObject m_gateEndingPosGO;

	// Token: 0x040025C8 RID: 9672
	[SerializeField]
	private Transform m_cameraMovementObj;

	// Token: 0x040025C9 RID: 9673
	private Vector3 m_gateStartingPos;

	// Token: 0x040025CA RID: 9674
	private WaitRL_Yield m_waitYield;

	// Token: 0x040025CB RID: 9675
	private string m_description = string.Empty;

	// Token: 0x040025CC RID: 9676
	private const string GATE_OPENING_SFX_AUDIO_PATH = "event:/SFX/Interactables/sfx_env_gate_openSequence";
}
