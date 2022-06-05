﻿using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200086A RID: 2154
	public class TextMeshSpawner : MonoBehaviour
	{
		// Token: 0x06004739 RID: 18233 RVA: 0x001004D0 File Offset: 0x000FE6D0
		private void Awake()
		{
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x001004D4 File Offset: 0x000FE6D4
		private void Start()
		{
			for (int i = 0; i < this.NumberOfNPC; i++)
			{
				if (this.SpawnType == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "!";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					this.floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 0;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(UnityEngine.Random.Range(-95f, 95f), 0.5f, UnityEngine.Random.Range(-95f, 95f));
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = this.TheFont.material;
					textMesh.font = this.TheFont;
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "!";
					this.floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
					this.floatingText_Script.SpawnType = 1;
				}
			}
		}

		// Token: 0x04003C4C RID: 15436
		public int SpawnType;

		// Token: 0x04003C4D RID: 15437
		public int NumberOfNPC = 12;

		// Token: 0x04003C4E RID: 15438
		public Font TheFont;

		// Token: 0x04003C4F RID: 15439
		private TextMeshProFloatingText floatingText_Script;
	}
}
