using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D70 RID: 3440
	public class TMP_UiFrameRateCounter : MonoBehaviour
	{
		// Token: 0x060061DA RID: 25050 RVA: 0x0016CFD4 File Offset: 0x0016B1D4
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

		// Token: 0x060061DB RID: 25051 RVA: 0x00035F21 File Offset: 0x00034121
		private void Start()
		{
			this.m_LastInterval = Time.realtimeSinceStartup;
			this.m_Frames = 0;
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x0016D094 File Offset: 0x0016B294
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

		// Token: 0x060061DD RID: 25053 RVA: 0x0016D174 File Offset: 0x0016B374
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

		// Token: 0x04004FCC RID: 20428
		public float UpdateInterval = 5f;

		// Token: 0x04004FCD RID: 20429
		private float m_LastInterval;

		// Token: 0x04004FCE RID: 20430
		private int m_Frames;

		// Token: 0x04004FCF RID: 20431
		public TMP_UiFrameRateCounter.FpsCounterAnchorPositions AnchorPosition = TMP_UiFrameRateCounter.FpsCounterAnchorPositions.TopRight;

		// Token: 0x04004FD0 RID: 20432
		private string htmlColorTag;

		// Token: 0x04004FD1 RID: 20433
		private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

		// Token: 0x04004FD2 RID: 20434
		private TextMeshProUGUI m_TextMeshPro;

		// Token: 0x04004FD3 RID: 20435
		private RectTransform m_frameCounter_transform;

		// Token: 0x04004FD4 RID: 20436
		private TMP_UiFrameRateCounter.FpsCounterAnchorPositions last_AnchorPosition;

		// Token: 0x02000D71 RID: 3441
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04004FD6 RID: 20438
			TopLeft,
			// Token: 0x04004FD7 RID: 20439
			BottomLeft,
			// Token: 0x04004FD8 RID: 20440
			TopRight,
			// Token: 0x04004FD9 RID: 20441
			BottomRight
		}
	}
}
