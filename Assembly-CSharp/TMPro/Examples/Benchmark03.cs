using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000858 RID: 2136
	public class Benchmark03 : MonoBehaviour
	{
		// Token: 0x060046E7 RID: 18151 RVA: 0x000FD67E File Offset: 0x000FB87E
		private void Awake()
		{
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x000FD680 File Offset: 0x000FB880
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

		// Token: 0x04003BD4 RID: 15316
		public int SpawnType;

		// Token: 0x04003BD5 RID: 15317
		public int NumberOfNPC = 12;

		// Token: 0x04003BD6 RID: 15318
		public Font TheFont;
	}
}
