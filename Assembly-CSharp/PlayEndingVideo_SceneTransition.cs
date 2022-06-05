using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020004E1 RID: 1249
public class PlayEndingVideo_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
{
	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06002858 RID: 10328 RVA: 0x00016A64 File Offset: 0x00014C64
	public override TransitionID ID
	{
		get
		{
			return TransitionID.PlayEndingVideo;
		}
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000BDE10 File Offset: 0x000BC010
	protected override void Awake()
	{
		base.Awake();
		this.m_videoPlayer = base.GetComponent<VideoPlayer>();
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		this.m_audioInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_endingCinematic", base.transform);
		this.m_videoPlayer_ErrorReceived = new VideoPlayer.ErrorEventHandler(this.VideoPlayer_ErrorReceived);
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x00016A68 File Offset: 0x00014C68
	private void OnDestroy()
	{
		if (this.m_audioInstance.isValid())
		{
			this.m_audioInstance.release();
		}
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x00016A83 File Offset: 0x00014C83
	public IEnumerator TransitionIn()
	{
		AudioManager.StopAllAudio(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.m_canvasGroup.alpha = 1f;
		this.m_videoPlayer.targetCamera = CameraController.UICamera;
		this.m_videoPlayer.errorReceived += this.m_videoPlayer_ErrorReceived;
		this.m_videoPreparationFailed = false;
		this.m_videoPlayer.Prepare();
		while (!this.m_videoPlayer.isPrepared && !this.m_videoPreparationFailed)
		{
			yield return null;
		}
		if (this.m_videoPreparationFailed)
		{
			yield break;
		}
		this.m_videoPlayer.Play();
		while (!this.m_videoPlayer.isPlaying)
		{
			yield return null;
		}
		if (this.m_audioInstance.isValid())
		{
			AudioManager.Play(null, this.m_audioInstance);
			if (this.m_syncCoroutine != null)
			{
				base.StopCoroutine(this.m_syncCoroutine);
			}
			this.m_syncCoroutine = base.StartCoroutine(this.SyncAudioCoroutine());
		}
		TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		RumbleManager.StartRumble(true, true, 0.1f, 0f, true);
		while (this.m_videoPlayer.time < 2.5999999046325684)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.4f, 0f, true);
		while (this.m_videoPlayer.time < 4.0)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.1f, 0f, true);
		while (this.m_videoPlayer.time < 6.0)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0f, 0f, true);
		while (this.m_videoPlayer.time < 6.5)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.2f, 0.2f, true);
		while (this.m_videoPlayer.time < 7.349999904632568)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.4f, 0.2f, true);
		while (this.m_videoPlayer.time < 7.800000190734863)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.4f, 0f, true);
		while (this.m_videoPlayer.time < 10.300000190734863)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.1f, 0f, true);
		while (this.m_videoPlayer.time < 16.700000762939453)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0f, 0f, true);
		while (this.m_videoPlayer.time < 24.5)
		{
			yield return null;
		}
		RumbleManager.StartRumble(true, true, 0.4f, 0f, true);
		while (this.m_videoPlayer.time < 28.0)
		{
			yield return null;
		}
		RumbleManager.StopRumble(true, true);
		while (this.m_videoPlayer.time < 31.0)
		{
			yield return null;
		}
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x00016A92 File Offset: 0x00014C92
	public IEnumerator TransitionOut()
	{
		while (this.m_videoPlayer.isPlaying && !this.m_videoPreparationFailed)
		{
			yield return null;
		}
		if (this.m_syncCoroutine != null)
		{
			base.StopCoroutine(this.m_syncCoroutine);
		}
		this.m_videoPlayer.Stop();
		this.m_videoPlayer.targetCamera = null;
		this.m_canvasGroup.alpha = 1f;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x00016AA1 File Offset: 0x00014CA1
	private IEnumerator SyncAudioCoroutine()
	{
		float syncTimer = Time.unscaledTime;
		for (;;)
		{
			if (Time.unscaledTime >= syncTimer)
			{
				syncTimer = Time.unscaledTime + 5f;
				if (!this.m_audioInstance.isValid() || !this.m_videoPlayer || !this.m_videoPlayer.isPlaying)
				{
					break;
				}
				float num = 0.3f;
				this.m_audioInstance.setTimelinePosition(Mathf.RoundToInt((float)((this.m_videoPlayer.time + (double)num) * 1000.0)));
			}
			else
			{
				yield return null;
			}
		}
		this.m_syncCoroutine = null;
		yield break;
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x00016AB0 File Offset: 0x00014CB0
	private void VideoPlayer_ErrorReceived(VideoPlayer source, string message)
	{
		source.errorReceived -= this.m_videoPlayer_ErrorReceived;
		this.m_videoPreparationFailed = true;
		Debug.Log("Failed to play video. Error: " + message);
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x00016AD5 File Offset: 0x00014CD5
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x0400237B RID: 9083
	[SerializeField]
	private float m_timeToFade = 1f;

	// Token: 0x0400237C RID: 9084
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x0400237D RID: 9085
	private VideoPlayer m_videoPlayer;

	// Token: 0x0400237E RID: 9086
	private EventInstance m_audioInstance;

	// Token: 0x0400237F RID: 9087
	private Coroutine m_syncCoroutine;

	// Token: 0x04002380 RID: 9088
	private VideoPlayer.ErrorEventHandler m_videoPlayer_ErrorReceived;

	// Token: 0x04002381 RID: 9089
	private bool m_videoPreparationFailed;
}
