using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000865 RID: 2149
	public class TMP_UiFrameRateCounter : MonoBehaviour
	{
		// Token: 0x06004721 RID: 18209 RVA: 0x000FFB94 File Offset: 0x000FDD94
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			Application.targetFrameRate = 1000;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_frameCounter_transform = gameObject.AddComponent<RectTransform>();
			this.m_frameCounter_transform.SetParent(base.transform, false);
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
			this.m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			this.m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			this.m_TextMeshPro.enableWordWrapping = false;
			this.m_TextMeshPro.fontSize = 36f;
			this.m_TextMeshPro.isOverlay = true;
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.last_AnchorPosition = this.AnchorPosition;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x000FFC53 File Offset: 0x000FDE53
		private void Start()
		{
			this.m_LastInterval = Time.realtimeSinceStartup;
			this.m_Frames = 0;
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x000FFC68 File Offset: 0x000FDE68
		private void Update()
		{
			if (this.AnchorPosition != this.last_AnchorPosition)
			{
				this.Set_FrameCounter_Position(this.AnchorPosition);
			}
			this.last_AnchorPosition = this.AnchorPosition;
			this.m_Frames++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup > this.m_LastInterval + this.UpdateInterval)
			{
				float num = (float)this.m_Frames / (realtimeSinceStartup - this.m_LastInterval);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					this.htmlColorTag = "<color=yellow>";
				}
				else if (num < 10f)
				{
					this.htmlColorTag = "<color=red>";
				}
				else
				{
					this.htmlColorTag = "<color=green>";
				}
				this.m_TextMeshPro.SetText(this.htmlColorTag + "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS", num, arg);
				this.m_Frames = 0;
				this.m_LastInterval = realtimeSinceStartup;
			}
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x000FFD48 File Offset: 0x000FDF48
		private void Set_FrameCounter_Position(TMP_UiFrameRateCounter.FpsCounterAnchorPositions anchor_position)
		{
			switch (anchor_position)
			{
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
				this.m_frameCounter_transform.pivot = new Vector2(0f, 1f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.01f, 0.99f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.01f, 0.99f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(0f, 1f);
				return;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.BottomLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				this.m_frameCounter_transform.pivot = new Vector2(0f, 0f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.01f, 0.01f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.01f, 0.01f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(0f, 0f);
				return;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
				this.m_frameCounter_transform.pivot = new Vector2(1f, 1f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.99f, 0.99f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.99f, 0.99f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(1f, 1f);
				return;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.BottomRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
				this.m_frameCounter_transform.pivot = new Vector2(1f, 0f);
				this.m_frameCounter_transform.anchorMin = new Vector2(0.99f, 0.01f);
				this.m_frameCounter_transform.anchorMax = new Vector2(0.99f, 0.01f);
				this.m_frameCounter_transform.anchoredPosition = new Vector2(1f, 0f);
				return;
			default:
				return;
			}
		}

		// Token: 0x04003C2E RID: 15406
		public float UpdateInterval = 5f;

		// Token: 0x04003C2F RID: 15407
		private float m_LastInterval;

		// Token: 0x04003C30 RID: 15408
		private int m_Frames;

		// Token: 0x04003C31 RID: 15409
		public TMP_UiFrameRateCounter.FpsCounterAnchorPositions AnchorPosition = TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopRight;

		// Token: 0x04003C32 RID: 15410
		private string htmlColorTag;

		// Token: 0x04003C33 RID: 15411
		private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

		// Token: 0x04003C34 RID: 15412
		private TextMeshProUGUI m_TextMeshPro;

		// Token: 0x04003C35 RID: 15413
		private RectTransform m_frameCounter_transform;

		// Token: 0x04003C36 RID: 15414
		private TMP_UiFrameRateCounter.FpsCounterAnchorPositions last_AnchorPosition;

		// Token: 0x02000E7C RID: 3708
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04005813 RID: 22547
			TopLeft,
			// Token: 0x04005814 RID: 22548
			BottomLeft,
			// Token: 0x04005815 RID: 22549
			TopRight,
			// Token: 0x04005816 RID: 22550
			BottomRight
		}
	}
}
