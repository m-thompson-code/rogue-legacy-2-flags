using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000855 RID: 2133
	public class Benchmark01 : MonoBehaviour
	{
		// Token: 0x060046E1 RID: 18145 RVA: 0x000FD396 File Offset: 0x000FB596
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
				this.m_textMeshPro.autoSizeTextContainer = true;
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_textMeshPro.enableWordWrapping = false;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Drop Shadow");
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<TextMesh>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				else
				{
					this.m_textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.anchor = TextAnchor.MiddleCenter;
			}
			int num;
			for (int i = 0; i <= 1000000; i = num + 1)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0}", (float)(i % 1000));
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

		// Token: 0x04003BBD RID: 15293
		public int BenchmarkType;

		// Token: 0x04003BBE RID: 15294
		public TMP_FontAsset TMProFont;

		// Token: 0x04003BBF RID: 15295
		public Font TextMeshFont;

		// Token: 0x04003BC0 RID: 15296
		private TextMeshPro m_textMeshPro;

		// Token: 0x04003BC1 RID: 15297
		private TextContainer m_textContainer;

		// Token: 0x04003BC2 RID: 15298
		private TextMesh m_textMesh;

		// Token: 0x04003BC3 RID: 15299
		private const string label01 = "The <#0050FF>count is: </color>{0}";

		// Token: 0x04003BC4 RID: 15300
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x04003BC5 RID: 15301
		private Material m_material01;

		// Token: 0x04003BC6 RID: 15302
		private Material m_material02;
	}
}
