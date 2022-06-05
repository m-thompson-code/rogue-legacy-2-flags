using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000866 RID: 2150
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		// Token: 0x06004726 RID: 18214 RVA: 0x000FFF6C File Offset: 0x000FE16C
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

		// Token: 0x06004727 RID: 18215 RVA: 0x00100044 File Offset: 0x000FE244
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

		// Token: 0x04003C37 RID: 15415
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions AnchorPosition = TMPro_InstructionOverlay.FpsCounterAnchorPositions.BottomLeft;

		// Token: 0x04003C38 RID: 15416
		private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		// Token: 0x04003C39 RID: 15417
		private TextMeshPro m_TextMeshPro;

		// Token: 0x04003C3A RID: 15418
		private TextContainer m_textContainer;

		// Token: 0x04003C3B RID: 15419
		private Transform m_frameCounter_transform;

		// Token: 0x04003C3C RID: 15420
		private Camera m_camera;

		// Token: 0x02000E7D RID: 3709
		public enum FpsCounterAnchorPositions
		{
			// Token: 0x04005818 RID: 22552
			TopLeft,
			// Token: 0x04005819 RID: 22553
			BottomLeft,
			// Token: 0x0400581A RID: 22554
			TopRight,
			// Token: 0x0400581B RID: 22555
			BottomRight
		}
	}
}
