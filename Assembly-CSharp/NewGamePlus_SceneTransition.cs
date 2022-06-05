using System;
using System.Collections;
using Cinemachine;
using FMODUnity;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002E1 RID: 737
public class NewGamePlus_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition, IAudioEventEmitter
{
	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00060968 File Offset: 0x0005EB68
	public override TransitionID ID
	{
		get
		{
			return TransitionID.NewGamePlus;
		}
	}

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x06001D50 RID: 7504 RVA: 0x0006096C File Offset: 0x0005EB6C
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x00060974 File Offset: 0x0005EB74
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0006097C File Offset: 0x0005EB7C
	public IEnumerator TransitionIn()
	{
		this.m_canvas.renderMode = RenderMode.WorldSpace;
		RewiredMapController.SetIsInCutscene(true);
		PlayerController playerController = PlayerManager.GetPlayerController();
		CameraController.SoloCam.gameObject.SetActive(true);
		CameraController.SoloCam.transform.SetPositionX(CameraController.GameCamera.transform.position.x);
		CameraController.SoloCam.transform.SetPositionY(CameraController.GameCamera.transform.position.y);
		CameraController.SoloCam.Camera.orthographicSize = CameraController.GameCamera.orthographicSize;
		CameraController.SoloCam.AddToCameraLayer(playerController.Visuals);
		this.m_bgCanvasGroup.alpha = 0f;
		this.m_bgImage.color = Color.black;
		this.m_bgCanvasGroup.gameObject.SetActive(true);
		this.m_bgCanvasGroup.transform.SetPositionX(CameraController.GameCamera.transform.position.x);
		this.m_bgCanvasGroup.transform.SetPositionY(CameraController.GameCamera.transform.position.y);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HidePlayerHUD, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.NGPlusChanged, null, null);
		playerController.Animator.SetBool("Victory", true);
		this.m_loopEventEmitter.Play();
		this.m_storedPixelResolution = CameraController.ForegroundPostProcessing.PixelResolution;
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		CameraController.ForegroundPostProcessing.Pixelation = true;
		CameraController.ForegroundPostProcessing.PixelBaselineAmount = 1f;
		CameraController.ForegroundPostProcessing.PixelResolution = new Vector2(1920f, 1080f);
		TweenManager.TweenTo(CameraController.ForegroundPostProcessing, 1f, new EaseDelegate(Ease.Quint.EaseOut), new object[]
		{
			"PixelResolution.x",
			64,
			"PixelResolution.y",
			36
		});
		CameraController.ForegroundPostProcessing.Distortion = true;
		CameraController.ForegroundPostProcessing.LensDistortion = 0f;
		TweenManager.TweenTo(CameraController.ForegroundPostProcessing, 1f, new EaseDelegate(Ease.Quint.EaseOut), new object[]
		{
			"LensDistortion",
			1
		});
		delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		playerController.Visuals.transform.SetLocalPositionY(-1f);
		Vector3 position = playerController.transform.position;
		Vector3 position2 = playerController.Visuals.transform.position;
		playerController.Pivot.transform.SetLocalPositionY(1f);
		TweenManager.TweenTo(playerController.Pivot.transform, 1.2f, new EaseDelegate(Ease.Back.EaseInSmall), new object[]
		{
			"localEulerAngles.z",
			720
		});
		TweenManager.TweenTo(playerController.Pivot.transform, 1f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"delay",
			0.2f,
			"localScale.x",
			0,
			"localScale.y",
			0
		});
		float d = playerController.BaseScaleToOffsetWith / playerController.transform.localScale.x;
		Vector2 vector = new Vector2(1.5f, 1.5f) * d;
		TweenManager.TweenBy(playerController.Pivot.transform, 1f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"delay",
			0.2f,
			"localPosition.x",
			vector.x,
			"localPosition.y",
			vector.y
		});
		BaseEffect wormholeEffect = EffectManager.PlayEffect(playerController.gameObject, null, "NewGamePlusCharge_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		CameraController.SoloCam.AddToCameraLayer(wormholeEffect.gameObject);
		yield return TweenManager.TweenTo(this.m_bgCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		delay = Time.time + 1.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		playerController.Visuals.SetActive(false);
		playerController.ControllerCorgi.GravityActive(false);
		delay = Time.time + 1.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_bgImage.color = Color.white;
		wormholeEffect.Stop(EffectStopType.Immediate);
		this.m_loopEventEmitter.Stop();
		AudioManager.PlayOneShot(this, "event:/Cut_Scenes/sfx_cutscene_ngPlusTeleport_end", default(Vector3));
		this.m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		yield break;
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0006098B File Offset: 0x0005EB8B
	public IEnumerator TransitionOut()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		float storedFallAscentMultiplier = playerController.FallMultiplierOverride;
		playerController.FallMultiplierOverride = 1f;
		playerController.Animator.SetBool("Victory", false);
		playerController.Visuals.SetActive(false);
		playerController.ControllerCorgi.GravityActive(false);
		GameObject gameObject = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("NGPlusSpawnPos");
		if (gameObject)
		{
			playerController.transform.localPosition = gameObject.transform.position;
			if (CameraController.CinemachineBrain.enabled)
			{
				CinemachineBrain.UpdateMethod updateMethod = CameraController.CinemachineBrain.m_UpdateMethod;
				CameraController.CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate;
				CameraController.CinemachineBrain.ManualUpdate();
				CameraController.CinemachineBrain.m_UpdateMethod = updateMethod;
			}
			CameraController.SoloCam.transform.SetPositionX(CameraController.GameCamera.transform.position.x);
			CameraController.SoloCam.transform.SetPositionY(CameraController.GameCamera.transform.position.y);
			CameraController.SoloCam.Camera.orthographicSize = CameraController.GameCamera.orthographicSize;
			this.m_bgCanvasGroup.transform.SetPositionX(CameraController.GameCamera.transform.position.x);
			this.m_bgCanvasGroup.transform.SetPositionY(CameraController.GameCamera.transform.position.y);
			yield return null;
			CameraController.ForegroundPostProcessing.Pixelation = true;
			CameraController.ForegroundPostProcessing.PixelBaselineAmount = 1f;
			CameraController.ForegroundPostProcessing.PixelResolution = new Vector2(64f, 36f);
		}
		TweenManager.TweenTo(this.m_bgCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo(CameraController.ForegroundPostProcessing, 1f, new EaseDelegate(Ease.Quint.EaseIn), new object[]
		{
			"LensDistortion",
			0
		});
		yield return TweenManager.TweenTo(CameraController.ForegroundPostProcessing, 1f, new EaseDelegate(Ease.Quint.EaseIn), new object[]
		{
			"PixelResolution.x",
			1920,
			"PixelResolution.y",
			1080
		}).TweenCoroutine;
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		CameraController.ForegroundPostProcessing.Distortion = false;
		CameraController.ForegroundPostProcessing.Pixelation = false;
		CameraController.ForegroundPostProcessing.PixelBaselineAmount = 0f;
		if (TraitManager.GetActiveTrait(TraitType.EnemiesCensored))
		{
			CameraController.ForegroundPostProcessing.Pixelation = true;
			CameraController.ForegroundPostProcessing.PixelResolution = this.m_storedPixelResolution;
		}
		float delay = Time.time + 0.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		EffectManager.PlayEffect(playerController.gameObject, null, "NewGamePlusPoof_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		playerController.Pivot.transform.localScale = new Vector3(1f, 1f, 1f);
		playerController.Pivot.transform.localPosition = Vector3.zero;
		playerController.Visuals.transform.SetLocalPositionY(0f);
		playerController.Visuals.SetActive(true);
		playerController.ControllerCorgi.GravityActive(true);
		AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_cutscene_ngPlusTeleport_playerAppear", playerController.gameObject);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayPlayerHUD, null, null);
		playerController.Animator.SetTrigger("Death5");
		while (!playerController.IsGrounded)
		{
			yield return null;
		}
		delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		playerController.SetVelocity(0f, 5f, false);
		playerController.Animator.Play("JumpBlendTree");
		delay = Time.time + 0.1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		while (!playerController.IsGrounded)
		{
			yield return null;
		}
		playerController.FallMultiplierOverride = storedFallAscentMultiplier;
		RewiredMapController.SetIsInCutscene(false);
		yield break;
	}

	// Token: 0x04001B4A RID: 6986
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x04001B4B RID: 6987
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04001B4C RID: 6988
	[SerializeField]
	private Image m_bgImage;

	// Token: 0x04001B4D RID: 6989
	[SerializeField]
	private StudioEventEmitter m_loopEventEmitter;

	// Token: 0x04001B4E RID: 6990
	private Vector2 m_storedPixelResolution;
}
