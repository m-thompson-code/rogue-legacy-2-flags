using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class ScreenDistortion_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition, IAudioEventEmitter
{
	// Token: 0x06001D5E RID: 7518 RVA: 0x00060A92 File Offset: 0x0005EC92
	protected override void Awake()
	{
		base.Awake();
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x00060AAA File Offset: 0x0005ECAA
	public void SetDistortInPosition(GameObject obj)
	{
		this.m_distortInPosObj = obj;
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x00060AB3 File Offset: 0x0005ECB3
	public void SetDistortOutPosition(GameObject obj)
	{
		this.m_distortOutPosObj = obj;
	}

	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x06001D61 RID: 7521 RVA: 0x00060ABC File Offset: 0x0005ECBC
	public override TransitionID ID
	{
		get
		{
			return TransitionID.ScreenDistortion;
		}
	}

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x06001D62 RID: 7522 RVA: 0x00060ABF File Offset: 0x0005ECBF
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

	// Token: 0x06001D63 RID: 7523 RVA: 0x00060AE5 File Offset: 0x0005ECE5
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00060AED File Offset: 0x0005ECED
	public IEnumerator TransitionIn()
	{
		if (this.m_distortInPosObj == null)
		{
			this.m_distortInPosObj = PlayerManager.GetPlayerController().gameObject;
		}
		AudioManager.PlayOneShot(this, this.m_transitionInAudioEventPath, this.m_distortInPosObj.transform.position);
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(this.m_distortInPosObj.transform.position);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = true;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_warpEffect, this.m_distortDuration, new EaseDelegate(Ease.Back.EaseIn), new object[]
		{
			"DistortionAmount",
			1
		}).TweenCoroutine;
		this.m_warpEffect.DistortionAmount = 1f;
		yield return null;
		yield break;
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x00060AFC File Offset: 0x0005ECFC
	public IEnumerator TransitionOut()
	{
		if (this.m_distortOutPosObj == null)
		{
			this.m_distortOutPosObj = PlayerManager.GetPlayerController().gameObject;
		}
		AudioManager.PlayOneShot(this, this.m_transitionOutAudioEventPath, this.m_distortInPosObj.transform.position);
		Vector3 vector = CameraController.GameCamera.WorldToViewportPoint(this.m_distortOutPosObj.transform.position);
		this.m_warpEffect.WarpCenterX = vector.x;
		this.m_warpEffect.WarpCenterY = vector.y;
		this.m_warpEffect.DistortionAmount = 1f;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_warpEffect, this.m_distortDuration, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"DistortionAmount",
			0
		}).TweenCoroutine;
		this.m_warpEffect.DistortionAmount = 0f;
		this.m_warpEffect.enabled = false;
		yield break;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x00060B0B File Offset: 0x0005ED0B
	private void OnDisable()
	{
		this.m_distortInPosObj = null;
		this.m_distortOutPosObj = null;
	}

	// Token: 0x04001B56 RID: 6998
	[SerializeField]
	private float m_distortDuration = 1f;

	// Token: 0x04001B57 RID: 6999
	[SerializeField]
	[EventRef]
	private string m_transitionInAudioEventPath;

	// Token: 0x04001B58 RID: 7000
	[SerializeField]
	[EventRef]
	private string m_transitionOutAudioEventPath;

	// Token: 0x04001B59 RID: 7001
	private GameObject m_distortInPosObj;

	// Token: 0x04001B5A RID: 7002
	private GameObject m_distortOutPosObj;

	// Token: 0x04001B5B RID: 7003
	private HeirloomWarp_Effect m_warpEffect;

	// Token: 0x04001B5C RID: 7004
	private string m_description = string.Empty;
}
