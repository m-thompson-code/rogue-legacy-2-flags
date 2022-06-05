using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D86 RID: 3462
	public class VertexZoom : MonoBehaviour
	{
		// Token: 0x06006244 RID: 25156 RVA: 0x000363AF File Offset: 0x000345AF
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06006245 RID: 25157 RVA: 0x000363BD File Offset: 0x000345BD
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06006246 RID: 25158 RVA: 0x000363D5 File Offset: 0x000345D5
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x000363ED File Offset: 0x000345ED
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x000363FC File Offset: 0x000345FC
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x00036413 File Offset: 0x00034613
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
			List<float> modifiedCharScale = new List<float>();
			List<int> scaleSortingOrder = new List<int>();
			this.hasTextChanged = true;
			Comparison<int> <>9__0;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
					this.hasTextChanged = false;
				}
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					modifiedCharScale.Clear();
					scaleSortingOrder.Clear();
					for (int i = 0; i < characterCount; i++)
					{
						if (textInfo.characterInfo[i].isVisible)
						{
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							Vector3[] vertices = cachedMeshInfoVertexData[materialReferenceIndex].vertices;
							Vector3 b2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
							Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
							vertices2[vertexIndex] = vertices[vertexIndex] - b2;
							vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
							vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
							vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
							float num = UnityEngine.Random.Range(1f, 1.5f);
							modifiedCharScale.Add(num);
							scaleSortingOrder.Add(modifiedCharScale.Count - 1);
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * num);
							vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
							vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
							vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
							vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
							vertices2[vertexIndex] += b2;
							vertices2[vertexIndex + 1] += b2;
							vertices2[vertexIndex + 2] += b2;
							vertices2[vertexIndex + 3] += b2;
							Vector2[] uvs = cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
							Vector2[] uvs2 = textInfo.meshInfo[materialReferenceIndex].uvs0;
							uvs2[vertexIndex] = uvs[vertexIndex];
							uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
							uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
							uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
							Color32[] colors = cachedMeshInfoVertexData[materialReferenceIndex].colors32;
							Color32[] colors2 = textInfo.meshInfo[materialReferenceIndex].colors32;
							colors2[vertexIndex] = colors[vertexIndex];
							colors2[vertexIndex + 1] = colors[vertexIndex + 1];
							colors2[vertexIndex + 2] = colors[vertexIndex + 2];
							colors2[vertexIndex + 3] = colors[vertexIndex + 3];
						}
					}
					for (int j = 0; j < textInfo.meshInfo.Length; j++)
					{
						List<int> list = scaleSortingOrder;
						Comparison<int> comparison;
						if ((comparison = <>9__0) == null)
						{
							comparison = (<>9__0 = ((int a, int b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b])));
						}
						list.Sort(comparison);
						textInfo.meshInfo[j].SortGeometry(scaleSortingOrder);
						textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
						textInfo.meshInfo[j].mesh.uv = textInfo.meshInfo[j].uvs0;
						textInfo.meshInfo[j].mesh.colors32 = textInfo.meshInfo[j].colors32;
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04005049 RID: 20553
		public float AngleMultiplier = 1f;

		// Token: 0x0400504A RID: 20554
		public float SpeedMultiplier = 1f;

		// Token: 0x0400504B RID: 20555
		public float CurveScale = 1f;

		// Token: 0x0400504C RID: 20556
		private TMP_Text m_TextComponent;

		// Token: 0x0400504D RID: 20557
		private bool hasTextChanged;
	}
}
