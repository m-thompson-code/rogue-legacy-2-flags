using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200085F RID: 2143
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		// Token: 0x06004700 RID: 18176 RVA: 0x000FE434 File Offset: 0x000FC634
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

		// Token: 0x06004701 RID: 18177 RVA: 0x000FE502 File Offset: 0x000FC702
		private void Update()
		{
			if (!this.isStatic)
			{
				this.m_text.SetText("The count is <#0080ff>{0}</color>", (float)(this.count % 1000));
				this.count++;
			}
		}

		// Token: 0x04003C0A RID: 15370
		public TMP_ExampleScript_01.objectType ObjectType;

		// Token: 0x04003C0B RID: 15371
		public bool isStatic;

		// Token: 0x04003C0C RID: 15372
		private TMP_Text m_text;

		// Token: 0x04003C0D RID: 15373
		private const string k_label = "The count is <#0080ff>{0}</color>";

		// Token: 0x04003C0E RID: 15374
		private int count;

		// Token: 0x02000E7A RID: 3706
		public enum objectType
		{
			// Token: 0x0400580B RID: 22539
			TextMeshPro,
			// Token: 0x0400580C RID: 22540
			TextMeshProUGUI
		}
	}
}
