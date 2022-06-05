using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000845 RID: 2117
public class BridgeBossEntranceRoomController : BossEntranceRoomController, IAudioEventEmitter
{
	// Token: 0x1700178E RID: 6030
	// (get) Token: 0x06004172 RID: 16754 RVA: 0x000243CD File Offset: 0x000225CD
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

	// Token: 0x06004173 RID: 16755 RVA: 0x000243F3 File Offset: 0x000225F3
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06004174 RID: 16756 RVA: 0x0010760C File Offset: 0x0010580C
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

	// Token: 0x06004175 RID: 16757 RVA: 0x0002440C File Offset: 0x0002260C
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

	// Token: 0x0400333D RID: 13117
	[SerializeField]
	private PropSpawnController m_gateSpawner;

	// Token: 0x0400333E RID: 13118
	[SerializeField]
	private GameObject m_gateEndingPosGO;

	// Token: 0x0400333F RID: 13119
	[SerializeField]
	private Transform m_cameraMovementObj;

	// Token: 0x04003340 RID: 13120
	private Vector3 m_gateStartingPos;

	// Token: 0x04003341 RID: 13121
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003342 RID: 13122
	private string m_description = string.Empty;

	// Token: 0x04003343 RID: 13123
	private const string GATE_OPENING_SFX_AUDIO_PATH = "event:/SFX/Interactables/sfx_env_gate_openSequence";
}
