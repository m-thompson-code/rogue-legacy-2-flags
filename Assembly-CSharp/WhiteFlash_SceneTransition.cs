using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class WhiteFlash_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
{
	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06002895 RID: 10389 RVA: 0x00003E42 File Offset: 0x00002042
	public override TransitionID ID
	{
		get
		{
			return TransitionID.WhiteFlash;
		}
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x00016C35 File Offset: 0x00014E35
	protected override void Awake()
	{
		base.Awake();
		this.m_canvasGroup.alpha = 0f;
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x00016C68 File Offset: 0x00014E68
	public IEnumerator TransitionIn()
	{
		this.m_canvasGroup.alpha = 0f;
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/Jonah/sfx_jonah_postDeath_transition_flash", CameraController.GameCamera.transform.position);
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.05f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x00016C77 File Offset: 0x00014E77
	public IEnumerator TransitionOut()
	{
		this.m_canvasGroup.alpha = 1f;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x00016C86 File Offset: 0x00014E86
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x0400239D RID: 9117
	[SerializeField]
	private CanvasGroup m_canvasGroup;
}
