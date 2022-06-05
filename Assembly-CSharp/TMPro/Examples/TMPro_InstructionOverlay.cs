using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D72 RID: 3442
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		// Token: 0x060061DF RID: 25055 RVA: 0x0016D37C File Offset: 0x0016B57C
		private void Awake()
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_camera = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			this.m_frameCounter_transform = gameObject.transform;
			this.m_frameCounter_transform.parent = this.m_camera.transform;
			this.m_frameCounter_transform.localRotation = Quaternion.identity;
			this.m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			this.m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			this.m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			this.m_TextMeshPro.fontSize = 30f;
			this.m_TextMeshPro.isOverlay = true;
			this.m_textContainer = gameObject.GetComponent<TextContainer>();
			this.Set_FrameCounter_Position(this.AnchorPosition);
			this.m_TextMeshPro.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x0016D454 File Offset: 0x0016B654
		private void Set_FrameCounter_Position(TMPro_InstructionOverlay.FpsCounterAnchorPositions anchor_position)
		{
			switch (anchor_position)
			{
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.TopRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				return;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomRight:
				this.m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
				this.m_frameCounter_transform.position = this.m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				return;
			default:
				return;
			}
		}

		// Token: 0x04004FDA RID: 20442
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions AnchorPosition = TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft;

		// Token: 0x04004FDB RID: 20443
		private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		// Token: 0x04004FDC RID: 20444
		private TextMeshPro m_TextMeshPro;

		// Token: 0x04004FDD RID: 20445
		private TextContainer m_textContainer;

		// Token: 0x04004FDE RID: 20446
		private Transform m_frameCounter_transform;

		// Token: 0x04004FDF RID: 20447
		private Camera m_camera;

		// Token: 0x02000D73 RID: 3443
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04004FE1 RID: 20449
			TopLeft,
			// Token: 0x04004FE2 RID: 20450
			BottomLeft,
			// Token: 0x04004FE3 RID: 20451
			TopRight,
			// Token: 0x04004FE4 RID: 20452
			BottomRight
		}
	}
}
