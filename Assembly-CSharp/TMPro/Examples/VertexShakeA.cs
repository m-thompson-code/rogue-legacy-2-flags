using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D82 RID: 3458
	public class VertexShakeA : MonoBehaviour
	{
		// Token: 0x0600622A RID: 25130 RVA: 0x0003623A File Offset: 0x0003443A
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x00036248 File Offset: 0x00034448
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600622C RID: 25132 RVA: 0x00036260 File Offset: 0x00034460
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x00036278 File Offset: 0x00034478
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x00036287 File Offset: 0x00034487
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (this.m_TextComponent)
			{
				this.hasTextChanged = true;
			}
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x000362A0 File Offset: 0x000344A0
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
						Quaternion q = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-0.25f, 0.25f) * this.RotationMultiplier);
						for (int k = firstCharacterIndex; k <= lastCharacterIndex; k++)
						{
							if (textInfo.characterInfo[k].isVisible)
							{
								int materialReferenceIndex = textInfo.characterInfo[k].materialReferenceIndex;
								int vertexIndex = textInfo.characterInfo[k].vertexIndex;
								Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
								copyOfVertices[materialReferenceIndex][vertexIndex] = vertices[vertexIndex] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 1] = vertices[vertexIndex + 1] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 2] = vertices[vertexIndex + 2] - b;
								copyOfVertices[materialReferenceIndex][vertexIndex + 3] = vertices[vertexIndex + 3] - b;
								float d = UnityEngine.Random.Range(0.995f - 0.001f * this.ScaleMultiplier, 1.005f + 0.001f * this.ScaleMultiplier);
								Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.one, q, Vector3.one * d);
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

		// Token: 0x04005034 RID: 20532
		public float AngleMultiplier = 1f;

		// Token: 0x04005035 RID: 20533
		public float SpeedMultiplier = 1f;

		// Token: 0x04005036 RID: 20534
		public float ScaleMultiplier = 1f;

		// Token: 0x04005037 RID: 20535
		public float RotationMultiplier = 1f;

		// Token: 0x04005038 RID: 20536
		private TMP_Text m_TextComponent;

		// Token: 0x04005039 RID: 20537
		private bool hasTextChanged;
	}
}
