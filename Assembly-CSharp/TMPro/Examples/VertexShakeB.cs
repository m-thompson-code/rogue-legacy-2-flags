using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200086E RID: 2158
	public class VertexShakeB : MonoBehaviour
	{
		// Token: 0x0600474E RID: 18254 RVA: 0x001007CF File Offset: 0x000FE9CF
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x001007DD File Offset: 0x000FE9DD
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x001007F5 File Offset: 0x000FE9F5
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x0010080D File Offset: 0x000FEA0D
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0010081C File Offset: 0x000FEA1C
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x00100835 File Offset: 0x000FEA35
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			Vector3[][] copyOfVertices = new Vector3[0][];
			this.hasTextChanged = true;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					if (copyOfVertices.Length < textInfo.meshInfo.Length)
					{
						copyOfVertices = new Vector3[textInfo.meshInfo.Length][];
					}
					for (int i = 0; i < textInfo.meshInfo.Length; i++)
					{
						int num = textInfo.meshInfo[i].vertices.Length;
						copyOfVertices[i] = new Vector3[num];
					}
					this.hasTextChanged = false;
				}
				if (textInfo.characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int lineCount = textInfo.lineCount;
					for (int j = 0; j < lineCount; j++)
					{
						int firstCharacterIndex = textInfo.lineInfo[j].firstCharacterIndex;
						int lastCharacterIndex = textInfo.lineInfo[j].lastCharacterIndex;
						Vector3 b = (textInfo.characterInfo[firstCharacterIndex].bottomLeft + textInfo.characterInfo[lastCharacterIndex].topRight) / 2f;
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f));
						for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
						{
							if (textInfo.characterInfo[k].isVisible)
							{
								int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
								int vertexIndex = textInfo.characterInfo[k].vertexIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								Vector3 b2 = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
								copyOfVertices[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b2;
								float d = UnityEngine.Random.Range(0.95f, 1.05f);
								Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, Quaternion.identity, Vector3.one * d);
								copyOfVertices[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 1]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 2]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 3]);
								copyOfVertices[materialReferenceIndex][vertexIndex] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] += b2;
								copyOfVertices[materialReferenceIndex][vertexIndex] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] -= b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] -= b;
								matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one);
								copyOfVertices[materialReferenceIndex][vertexIndex] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 1]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 2]);
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = matrix4x.MultiplyPoint3x4(copyOfVertices[materialReferenceIndex][vertexIndex + 3]);
								copyOfVertices[materialReferenceIndex][vertexIndex] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] += b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] += b;
							}
						}
					}
					for (int l = 0; l < textInfo.meshInfo.Length; l++)
					{
						textInfo.meshInfo[l].mesh.vertices = copyOfVertices[l];
						this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04003C5C RID: 15452
		public float AngleMultiplier = 1f;

		// Token: 0x04003C5D RID: 15453
		public float SpeedMultiplier = 1f;

		// Token: 0x04003C5E RID: 15454
		public float CurveScale = 1f;

		// Token: 0x04003C5F RID: 15455
		private TMP_Text m_TextComponent;

		// Token: 0x04003C60 RID: 15456
		private bool hasTextChanged;
	}
}
