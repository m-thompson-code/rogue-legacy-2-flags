using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D5D RID: 3421
	public class Benchmark03 : MonoBehaviour
	{
		// Token: 0x06006194 RID: 24980 RVA: 0x00002FCA File Offset: 0x000011CA
		private void Awake()
		{
		}

		// Token: 0x06006195 RID: 24981 RVA: 0x0016A79C File Offset: 0x0016899C
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					TextMeshPro textMeshPro = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMeshPro>();
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "@";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				else
				{
					TextMesh textMesh = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 0f, 0f)
						}
					}.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.TheFont.material;
					textMesh.font = this.TheFont;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
				}
			}
		}

		// Token: 0x04004F59 RID: 20313
		public int SpawnType;

		// Token: 0x04004F5A RID: 20314
		public int NumberOfNPC = 12;

		// Token: 0x04004F5B RID: 20315
		public Font TheFont;
	}
}
