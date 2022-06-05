using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class WhiteFlash_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
{
	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x06001D68 RID: 7528 RVA: 0x00060B39 File Offset: 0x0005ED39
	public override TransitionID ID
	{
		get
		{
			return TransitionID.WhiteFlash;
		}
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x00060B3C File Offset: 0x0005ED3C
	protected override void Awake()
	{
		base.Awake();
		this.m_canvasGroup.alpha = 0f;
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"), false);
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x00060B6F File Offset: 0x0005ED6F
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

	// Token: 0x06001D6B RID: 7531 RVA: 0x00060B7E File Offset: 0x0005ED7E
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

	// Token: 0x06001D6C RID: 7532 RVA: 0x00060B8D File Offset: 0x0005ED8D
	public override IEnumerator Run()
	{
		yield break;
	}

	// Token: 0x04001B5D RID: 7005
	[SerializeField]
	private CanvasGroup m_canvasGroup;
}
