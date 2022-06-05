using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	// Token: 0x02000D5A RID: 3418
	public class Benchmark01_UGUI : MonoBehaviour
	{
		// Token: 0x0600618A RID: 24970 RVA: 0x00035BCC File Offset: 0x00033DCC
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<Text>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.alignment = TextAnchor.MiddleCenter;
			}
			int num;
			for (int i = 0; i <= 1000000; i = num + 1)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.text = "The <#0050FF>count is: </color>" + (i % 1000).ToString();
					if (i % 1000 == 999)
					{
						this.m_textMeshPro.fontSharedMaterial = ((this.m_textMeshPro.fontSharedMaterial == this.m_material01) ? (this.m_textMeshPro.fontSharedMaterial = this.m_material02) : (this.m_textMeshPro.fontSharedMaterial = this.m_material01));
					}
				}
				else if (this.BenchmarkType == 1)
				{
					this.m_textMesh.text = "The <color=#0050FF>count is: </color>" + (i % 1000).ToString();
				}
				yield return null;
				num = i;
			}
			yield return null;
			yield break;
		}

		// Token: 0x04004F48 RID: 20296
		public int BenchmarkType;

		// Token: 0x04004F49 RID: 20297
		public Canvas canvas;

		// Token: 0x04004F4A RID: 20298
		public TMP_FontAsset TMProFont;

		// Token: 0x04004F4B RID: 20299
		public Font TextMeshFont;

		// Token: 0x04004F4C RID: 20300
		private TextMeshProUGUI m_textMeshPro;

		// Token: 0x04004F4D RID: 20301
		private Text m_textMesh;

		// Token: 0x04004F4E RID: 20302
		private const string label01 = "The <#0050FF>count is: </color>";

		// Token: 0x04004F4F RID: 20303
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x04004F50 RID: 20304
		private Material m_material01;

		// Token: 0x04004F51 RID: 20305
		private Material m_material02;
	}
}
