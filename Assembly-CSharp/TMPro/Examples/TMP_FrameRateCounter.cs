using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000860 RID: 2144
	public class TMP_FrameRateCounter : MonoBehaviour
	{
		// Token: 0x06004703 RID: 18179 RVA: 0x000FE540 File Offset: 0x000FC740
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_camera = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			this.m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			this.m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			this.m_frameCounter_transform = gameObject.transform;
			this.m_frameCounter_transform.SetParent(this.m_camera.transform);
			this.m_frameCounter_transform.localRotation = Quaternion.identity;
			this.m_TextMeshPro.enableWordWrapping = false;
			this.m_TextMeshPro.fontSize = 24f;
			this.m_TextMeshPro.isOverlay = true;
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.last_AnchorPosition = this.AnchorPosition;
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x000FE61A File Offset: 0x000FC81A
		private void Start()
		{
			this.m_LastInterval = Time.realtimeSinceStartup;
			this.m_Frames = 0;
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x000FE630 File Offset: 0x000FC830
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

		// Token: 0x06004706 RID: 18182 RVA: 0x000FE710 File Offset: 0x000FC910
		private void Set_FrameCounter_Position(TMP_FrameRateCounter.FpsCounterAnchorPositions anchor_position)
		{
			this.m_TextMeshPro.margin = new Vector4(1f, 1f, 1f, 1f);
			switch (anchor_position)
			{
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.TopLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(0f, 1f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				return;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.BottomLeft:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(0f, 0f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				return;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.TopRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(1f, 1f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				return;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.BottomRight:
				this.m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
				this.m_TextMeshPro.rectTransform.pivot = new Vector2(1f, 0f);
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				return;
			default:
				return;
			}
		}

		// Token: 0x04003C0F RID: 15375
		public float UpdateInterval = 5f;

		// Token: 0x04003C10 RID: 15376
		private float m_LastInterval;

		// Token: 0x04003C11 RID: 15377
		private int m_Frames;

		// Token: 0x04003C12 RID: 15378
		public TMP_FrameRateCounter.FpsCounterAnchorPositions AnchorPosition = TMP_FrameRateCounter.FpsCounterAnchorPositions.TopRight;

		// Token: 0x04003C13 RID: 15379
		private string htmlColorTag;

		// Token: 0x04003C14 RID: 15380
		private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

		// Token: 0x04003C15 RID: 15381
		private TextMeshPro m_TextMeshPro;

		// Token: 0x04003C16 RID: 15382
		private Transform m_frameCounter_transform;

		// Token: 0x04003C17 RID: 15383
		private Camera m_camera;

		// Token: 0x04003C18 RID: 15384
		private TMP_FrameRateCounter.FpsCounterAnchorPositions last_AnchorPosition;

		// Token: 0x02000E7B RID: 3707
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x0400580E RID: 22542
			TopLeft,
			// Token: 0x0400580F RID: 22543
			BottomLeft,
			// Token: 0x04005810 RID: 22544
			TopRight,
			// Token: 0x04005811 RID: 22545
			BottomRight
		}
	}
}
