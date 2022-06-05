using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D7C RID: 3452
	public class TextMeshSpawner : MonoBehaviour
	{
		// Token: 0x06006210 RID: 25104 RVA: 0x00002FCA File Offset: 0x000011CA
		private void Awake()
		{
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x0016E054 File Offset: 0x0016C254
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

		// Token: 0x0400501B RID: 20507
		public int SpawnType;

		// Token: 0x0400501C RID: 20508
		public int NumberOfNPC = 12;

		// Token: 0x0400501D RID: 20509
		public Font TheFont;

		// Token: 0x0400501E RID: 20510
		private TextMeshProFloatingText floatingText_Script;
	}
}
