using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200086C RID: 2156
	public class VertexJitter : MonoBehaviour
	{
		// Token: 0x06004740 RID: 18240 RVA: 0x0010068A File Offset: 0x000FE88A
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x00100698 File Offset: 0x000FE898
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x001006B0 File Offset: 0x000FE8B0
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001006C8 File Offset: 0x000FE8C8
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x001006D7 File Offset: 0x000FE8D7
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x001006EE File Offset: 0x000FE8EE
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			int loopCount = 0;
			this.hasTextChanged = true;
			VertexJitter.VertexAnim[] vertexAnim = new VertexJitter.VertexAnim[1024];
			for (int i = 0; i < 1024; i++)
			{
				vertexAnim[i].angleRange = UnityEngine.Random.Range(10f, 25f);
				vertexAnim[i].speed = UnityEngine.Random.Range(1f, 3f);
			}
			TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			for (;;)
			{
				if (this.hasTextChanged)
				{
					cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
					this.hasTextChanged = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					for (int j = 0; j < characterCount; j++)
					{
						if (textInfo.characterInfo[j].isVisible)
						{
							VertexJitter.VertexAnim vertexAnim2 = vertexAnim[j];
							int materialReferenceIndex = textInfo.characterInfo[j].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[j].vertexIndex;
							Vector3[] vertices = cachedMeshInfo[materialReferenceIndex].vertices;
							Vector3 b = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
							vertexAnim2.angle = Mathf.SmoothStep(-vertexAnim2.angleRange, vertexAnim2.angleRange, Mathf.PingPong((float)loopCount / 25f * vertexAnim2.speed, 1f));
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f) * this.CurveScale, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-5f, 5f) * this.AngleMultiplier), Vector3.one);
							vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b;
							vertices2[vertexIndex + 1] += b;
							vertices2[vertexIndex + 2] += b;
							vertices2[vertexIndex + 3] += b;
							vertexAnim[j] = vertexAnim2;
						}
					}
					for (int k = 0; k < textInfo.meshInfo.Length; k++)
					{
						textInfo.meshInfo[k].mesh.vertices = textInfo.meshInfo[k].vertices;
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[k].mesh, k);
					}
					loopCount++;
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04003C51 RID: 15441
		public float AngleMultiplier = 1f;

		// Token: 0x04003C52 RID: 15442
		public float SpeedMultiplier = 1f;

		// Token: 0x04003C53 RID: 15443
		public float CurveScale = 1f;

		// Token: 0x04003C54 RID: 15444
		private TMP_Text m_TextComponent;

		// Token: 0x04003C55 RID: 15445
		private bool hasTextChanged;

		// Token: 0x02000E84 RID: 3716
		private struct VertexAnim
		{
			// Token: 0x04005848 RID: 22600
			public float angleRange;

			// Token: 0x04005849 RID: 22601
			public float angle;

			// Token: 0x0400584A RID: 22602
			public float speed;
		}
	}
}
