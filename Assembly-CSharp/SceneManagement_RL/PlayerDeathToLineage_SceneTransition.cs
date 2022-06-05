using System;
using System.Collections;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement_RL
{
	// Token: 0x02000E2C RID: 3628
	public class PlayerDeathToLineage_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020CF RID: 8399
		// (get) Token: 0x06006643 RID: 26179 RVA: 0x00004762 File Offset: 0x00002962
		public override TransitionID ID
		{
			get
			{
				return TransitionID.PlayerDeathToLineage;
			}
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x0017A9DC File Offset: 0x00178BDC
		private void OnEnable()
		{
			this.m_bgFadeCanvasGroup.alpha = 0f;
			this.m_deathPanelCanvasGroup.alpha = 1f;
			this.m_portraitCanvasGroup.alpha = 0f;
			this.m_playerModelCanvasGroup.alpha = 0f;
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x0017AA2C File Offset: 0x00178C2C
		public void SetData(RenderTexture portraitTexture, Vector3 playerAnchoredPos, string slainText, string tipText, string rankText, string rankCounterText)
		{
			if (this.m_portraitRawImage.texture == null)
			{
				RenderTexture renderTexture = new RenderTexture(portraitTexture);
				renderTexture.Create();
				this.m_portraitRawImage.texture = renderTexture;
			}
			if (this.m_playerModelRawImage == null)
			{
				this.m_playerModelRawImage = this.m_playerModelCanvasGroup.GetComponent<RawImage>();
				RectTransform component = this.m_playerModelCanvasGroup.GetComponent<RectTransform>();
				component.anchoredPosition = playerAnchoredPos;
				RenderTexture renderTexture2 = new RenderTexture((int)component.sizeDelta.x, (int)component.sizeDelta.y, 24, portraitTexture.format);
				renderTexture2.antiAliasing = Mathf.Clamp(QualitySettings.antiAliasing, 1, 10);
				renderTexture2.Create();
				this.m_playerModelRawImage.texture = renderTexture2;
				this.m_playerModelCamera.CopyFrom(CameraController.SoloCam.Camera);
				this.m_playerModelCamera.targetTexture = (this.m_playerModelRawImage.texture as RenderTexture);
				this.m_playerModelCamera.clearFlags = CameraClearFlags.Color;
				this.m_playerModelCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
				float num = (float)this.m_playerModelRawImage.texture.width / 60f;
				float num2 = (float)this.m_playerModelRawImage.texture.height / 60f;
				float num3 = 1f;
				float num4 = 1f;
				if (!Mathf.Approximately(1.7777778f, AspectRatioManager.CurrentScreenAspectRatio))
				{
					if (AspectRatioManager.CurrentScreenAspectRatio < 1.7777778f)
					{
						num4 = AspectRatioManager.CurrentScreenAspectRatio / 1.7777778f;
						if (AspectRatioManager.Disable_16_9_Aspect)
						{
							num3 = num4;
						}
					}
					else
					{
						num3 = 1.7777778f / AspectRatioManager.CurrentScreenAspectRatio;
					}
					if (!AspectRatioManager.Disable_16_9_Aspect || AspectRatioManager.CurrentScreenAspectRatio < 1.7777778f)
					{
						num *= num3;
						num2 *= num4;
					}
				}
				this.m_playerModelCamera.projectionMatrix = Matrix4x4.Ortho(-num * 0.5f, num * 0.5f, -num2 * 0.5f, num2 * 0.5f, 0.1f * num3, 5000f * num4);
			}
			this.m_portraitToRender = portraitTexture;
			this.m_portraitGORectTransform.anchoredPosition = Vector2.zero;
			this.m_slainText.text = slainText;
			this.m_tipText.text = tipText;
			this.m_rankText.text = rankText;
			this.m_rankCounterText.text = rankCounterText;
			if (SkillTreeLogicHelper.IsTotemUnlocked())
			{
				this.m_rankText.gameObject.SetActive(true);
				this.m_rankCounterText.gameObject.SetActive(true);
				return;
			}
			this.m_rankText.gameObject.SetActive(false);
			this.m_rankCounterText.gameObject.SetActive(false);
		}

		// Token: 0x06006646 RID: 26182 RVA: 0x00038537 File Offset: 0x00036737
		public IEnumerator TransitionIn()
		{
			Component playerController = PlayerManager.GetPlayerController();
			Graphics.CopyTexture(this.m_portraitToRender, this.m_portraitRawImage.texture);
			Vector3 position = this.m_playerModelCamera.transform.position;
			position = playerController.transform.position;
			position.z = this.m_playerModelCamera.transform.position.z;
			this.m_playerModelCamera.transform.position = position;
			this.m_playerModelCamera.Render();
			this.m_portraitCanvasGroup.alpha = 1f;
			this.m_playerModelCanvasGroup.alpha = 1f;
			(WindowManager.GetWindowController(WindowID.PlayerDeath) as PlayerDeathWindowController).DeactivatePortrait();
			yield return TweenManager.TweenTo_UnscaledTime(this.m_bgFadeCanvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			MapController.DeathMapCamera.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x00038546 File Offset: 0x00036746
		public IEnumerator TransitionOut()
		{
			this.m_bgFadeCanvasGroup.alpha = 1f;
			this.m_deathPanelCanvasGroup.alpha = 1f;
			this.m_portraitCanvasGroup.alpha = 1f;
			this.m_playerModelCanvasGroup.alpha = 1f;
			TweenManager.TweenTo_UnscaledTime(this.m_deathPanelCanvasGroup, 0.2f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			TweenManager.TweenTo_UnscaledTime(this.m_playerModelCanvasGroup, 0.2f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			float delay = Time.unscaledTime + 0.1f;
			while (Time.unscaledTime < delay)
			{
				yield return null;
			}
			float num = -222f;
			(WindowManager.GetWindowController(WindowID.Lineage) as LineageWindowController).RunOpenTransition(this.m_timeToFade);
			TweenManager.TweenBy_UnscaledTime(this.m_portraitGORectTransform, this.m_timeToFade, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"anchoredPosition.x",
				num
			});
			TweenManager.TweenTo_UnscaledTime(this.m_bgFadeCanvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_portraitCanvasGroup, this.m_timeToFade, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x00038555 File Offset: 0x00036755
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04005317 RID: 21271
		[SerializeField]
		private float m_timeToFade = 1f;

		// Token: 0x04005318 RID: 21272
		[SerializeField]
		private CanvasScaler m_canvasScaler;

		// Token: 0x04005319 RID: 21273
		[SerializeField]
		private CanvasGroup m_bgFadeCanvasGroup;

		// Token: 0x0400531A RID: 21274
		[SerializeField]
		private CanvasGroup m_portraitCanvasGroup;

		// Token: 0x0400531B RID: 21275
		[SerializeField]
		private RawImage m_portraitRawImage;

		// Token: 0x0400531C RID: 21276
		[SerializeField]
		private RectTransform m_portraitGORectTransform;

		// Token: 0x0400531D RID: 21277
		[SerializeField]
		private CanvasGroup m_playerModelCanvasGroup;

		// Token: 0x0400531E RID: 21278
		[SerializeField]
		private Camera m_playerModelCamera;

		// Token: 0x0400531F RID: 21279
		[SerializeField]
		private CanvasGroup m_deathPanelCanvasGroup;

		// Token: 0x04005320 RID: 21280
		[SerializeField]
		private TMP_Text m_slainText;

		// Token: 0x04005321 RID: 21281
		[SerializeField]
		private TMP_Text m_tipText;

		// Token: 0x04005322 RID: 21282
		[SerializeField]
		private TMP_Text m_rankText;

		// Token: 0x04005323 RID: 21283
		[SerializeField]
		private TMP_Text m_rankCounterText;

		// Token: 0x04005324 RID: 21284
		private RawImage m_playerModelRawImage;

		// Token: 0x04005325 RID: 21285
		private RenderTexture m_portraitToRender;

		// Token: 0x04005326 RID: 21286
		private Vector3[] m_worldCorners = new Vector3[4];
	}
}
