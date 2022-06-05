using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020002E2 RID: 738
public class PlayEndingVideo_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
{
	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x06001D55 RID: 7509 RVA: 0x000609A2 File Offset: 0x0005EBA2
	public override TransitionID ID
	{
		get
		{
			return TransitionID.PlayEndingVideo;
		}
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x000609A8 File Offset: 0x0005EBA8
	protected override void Awake()
	{
		base.Awake();
		this.m_videoPlayer = base.GetComponent<VideoPlayer>();
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
		this.m_audioInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_endingCinematic", base.transform);
		this.m_videoPlayer_ErrorReceived = new VideoPlayer.ErrorEventHandler(this.VideoPlayer_ErrorReceived);
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x00060A0A File Offset: 0x0005EC0A
	private void OnDestroy()
	{
		if (this.m_audioInstance.isValid())
		{
			this.m_audioInstance.release();
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x00060A25 File Offset: 0x0005EC25
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

	// Token: 0x06001D59 RID: 7513 RVA: 0x00060A34 File Offset: 0x0005EC34
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

	// Token: 0x06001D5A RID: 7514 RVA: 0x00060A43 File Offset: 0x0005EC43
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

	// Token: 0x06001D5B RID: 7515 RVA: 0x00060A52 File Offset: 0x0005EC52
	private void VideoPlayer_ErrorReceived(VideoPlayer source, string message)
	{
		source.errorReceived -= this.m_videoPlayer_ErrorReceived;
		this.m_videoPreparationFailed = true;
		Debug.Log("Failed to play video. Error: " + message);
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x00060A77 File Offset: 0x0005EC77
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x04001B4F RID: 6991
	[SerializeField]
	private float m_timeToFade = 1f;

	// Token: 0x04001B50 RID: 6992
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001B51 RID: 6993
	private VideoPlayer m_videoPlayer;

	// Token: 0x04001B52 RID: 6994
	private EventInstance m_audioInstance;

	// Token: 0x04001B53 RID: 6995
	private Coroutine m_syncCoroutine;

	// Token: 0x04001B54 RID: 6996
	private VideoPlayer.ErrorEventHandler m_videoPlayer_ErrorReceived;

	// Token: 0x04001B55 RID: 6997
	private bool m_videoPreparationFailed;
}
