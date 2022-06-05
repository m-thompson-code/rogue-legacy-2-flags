using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D68 RID: 3432
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		// Token: 0x060061B9 RID: 25017 RVA: 0x0016BA64 File Offset: 0x00169C64
		private void Awake()
		{
			if (this.ObjectType == TMP_ExampleScript_01.objectType.TextMeshPro)
			{
				this.m_text = (base.GetComponent<TextMeshPro>() ?? base.gameObject.AddComponent<TextMeshPro>());
			}
			else
			{
				this.m_text = (base.GetComponent<TextMeshProUGUI>() ?? base.gameObject.AddComponent<TextMeshProUGUI>());
			}
			this.m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			this.m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			this.m_text.fontSize = 120f;
			this.m_text.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = this.m_text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			this.m_text.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x00035D4F File Offset: 0x00033F4F
		private void Update()
		{
			if (!this.isStatic)
			{
				this.m_text.SetText("The count is <#0080ff>{0}</color>", (float)(this.count % 1000));
				this.count++;
			}
		}

		// Token: 0x04004FA0 RID: 20384
		public TMP_ExampleScript_01.objectType ObjectType;

		// Token: 0x04004FA1 RID: 20385
		public bool isStatic;

		// Token: 0x04004FA2 RID: 20386
		private TMP_Text m_text;

		// Token: 0x04004FA3 RID: 20387
		private const string k_label = "The count is <#0080ff>{0}</color>";

		// Token: 0x04004FA4 RID: 20388
		private int count;

		// Token: 0x02000D69 RID: 3433
		public enum objectType
		{
			// Token: 0x04004FA6 RID: 20390
			TextMeshPro,
			// Token: 0x04004FA7 RID: 20391
			TextMeshProUGUI
		}
	}
}
