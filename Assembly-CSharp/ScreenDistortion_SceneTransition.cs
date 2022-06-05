using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class ScreenDistortion_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition, IAudioEventEmitter
{
	// Token: 0x06002879 RID: 10361 RVA: 0x00016B4C File Offset: 0x00014D4C
	protected override void Awake()
	{
		base.Awake();
		this.m_warpEffect = CameraController.ForegroundPerspCam.GetComponent<HeirloomWarp_Effect>();
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x00016B64 File Offset: 0x00014D64
	public void SetDistortInPosition(GameObject obj)
	{
		this.m_distortInPosObj = obj;
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x00016B6D File Offset: 0x00014D6D
	public void SetDistortOutPosition(GameObject obj)
	{
		this.m_distortOutPosObj = obj;
	}

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x0600287C RID: 10364 RVA: 0x00004792 File Offset: 0x00002992
	public override TransitionID ID
	{
		get
		{
			return TransitionID.ScreenDistortion;
		}
	}

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x0600287D RID: 10365 RVA: 0x00016B76 File Offset: 0x00014D76
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

	// Token: 0x0600287E RID: 10366 RVA: 0x00016B9C File Offset: 0x00014D9C
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x00016BA4 File Offset: 0x00014DA4
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

	// Token: 0x06002880 RID: 10368 RVA: 0x00016BB3 File Offset: 0x00014DB3
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

	// Token: 0x06002881 RID: 10369 RVA: 0x00016BC2 File Offset: 0x00014DC2
	private void OnDisable()
	{
		this.m_distortInPosObj = null;
		this.m_distortOutPosObj = null;
	}

	// Token: 0x0400238E RID: 9102
	[SerializeField]
	private float m_distortDuration = 1f;

	// Token: 0x0400238F RID: 9103
	[SerializeField]
	[EventRef]
	private string m_transitionInAudioEventPath;

	// Token: 0x04002390 RID: 9104
	[SerializeField]
	[EventRef]
	private string m_transitionOutAudioEventPath;

	// Token: 0x04002391 RID: 9105
	private GameObject m_distortInPosObj;

	// Token: 0x04002392 RID: 9106
	private GameObject m_distortOutPosObj;

	// Token: 0x04002393 RID: 9107
	private HeirloomWarp_Effect m_warpEffect;

	// Token: 0x04002394 RID: 9108
	private string m_description = string.Empty;
}
