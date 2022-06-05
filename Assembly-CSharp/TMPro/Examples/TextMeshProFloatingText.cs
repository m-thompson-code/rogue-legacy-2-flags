using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000869 RID: 2153
	public class TextMeshProFloatingText : MonoBehaviour
	{
		// Token: 0x06004734 RID: 18228 RVA: 0x00100248 File Offset: 0x000FE448
		private void Awake()
		{
			this.m_transform = base.transform;
			this.m_floatingText = new GameObject(base.name + " floating text");
			this.m_cameraTransform = Camera.main.transform;
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x00100284 File Offset: 0x000FE484
		private void Start()
		{
			if (this.SpawnType == 0)
			{
				this.m_textMeshPro = this.m_floatingText.AddComponent<TextMeshPro>();
				this.m_textMeshPro.rectTransform.sizeDelta = new Vector2(3f, 3f);
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMeshPro.fontSize = 24f;
				this.m_textMeshPro.enableKerning = false;
				this.m_textMeshPro.text = string.Empty;
				base.StartCoroutine(this.DisplayTextMeshProFloatingText());
				return;
			}
			if (this.SpawnType == 1)
			{
				this.m_floatingText_Transform = this.m_floatingText.transform;
				this.m_floatingText_Transform.position = this.m_transform.position + new Vector3(0f, 15f, 0f);
				this.m_textMesh = this.m_floatingText.AddComponent<TextMesh>();
				this.m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
				this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				this.m_textMesh.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
				this.m_textMesh.anchor = TextAnchor.LowerCenter;
				this.m_textMesh.fontSize = 24;
				base.StartCoroutine(this.DisplayTextMeshFloatingText());
				return;
			}
			int spawnType = this.SpawnType;
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x00100494 File Offset: 0x000FE694
		public IEnumerator DisplayTextMeshProFloatingText()
		{
			float CountDuration = 2f;
			float starting_Count = UnityEngine.Random.Range(5f, 20f);
			float current_Count = starting_Count;
			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMeshPro.color;
			float alpha = 255f;
			float fadeDuration = 3f / starting_Count * CountDuration;
			while (current_Count > 0f)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;
				if (current_Count <= 3f)
				{
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255f, 0f, 255f);
				}
				int num = (int)current_Count;
				this.m_textMeshPro.text = num.ToString();
				this.m_textMeshPro.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);
				this.m_floatingText_Transform.position += new Vector3(0f, starting_Count * Time.deltaTime, 0f);
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 vector = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			this.m_floatingText_Transform.position = start_pos;
			base.StartCoroutine(this.DisplayTextMeshProFloatingText());
			yield break;
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x001004A3 File Offset: 0x000FE6A3
		public IEnumerator DisplayTextMeshFloatingText()
		{
			float CountDuration = 2f;
			float starting_Count = UnityEngine.Random.Range(5f, 20f);
			float current_Count = starting_Count;
			Vector3 start_pos = this.m_floatingText_Transform.position;
			Color32 start_color = this.m_textMesh.color;
			float alpha = 255f;
			float fadeDuration = 3f / starting_Count * CountDuration;
			while (current_Count > 0f)
			{
				current_Count -= Time.deltaTime / CountDuration * starting_Count;
				if (current_Count <= 3f)
				{
					alpha = Mathf.Clamp(alpha - Time.deltaTime / fadeDuration * 255f, 0f, 255f);
				}
				int num = (int)current_Count;
				this.m_textMesh.text = num.ToString();
				this.m_textMesh.color = new Color32(start_color.r, start_color.g, start_color.b, (byte)alpha);
				this.m_floatingText_Transform.position += new Vector3(0f, starting_Count * Time.deltaTime, 0f);
				if (!this.lastPOS.Compare(this.m_cameraTransform.position, 1000) || !this.lastRotation.Compare(this.m_cameraTransform.rotation, 1000))
				{
					this.lastPOS = this.m_cameraTransform.position;
					this.lastRotation = this.m_cameraTransform.rotation;
					this.m_floatingText_Transform.rotation = this.lastRotation;
					Vector3 vector = this.m_transform.position - this.lastPOS;
					this.m_transform.forward = new Vector3(vector.x, 0f, vector.z);
				}
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			this.m_floatingText_Transform.position = start_pos;
			base.StartCoroutine(this.DisplayTextMeshFloatingText());
			yield break;
		}

		// Token: 0x04003C42 RID: 15426
		public Font TheFont;

		// Token: 0x04003C43 RID: 15427
		private GameObject m_floatingText;

		// Token: 0x04003C44 RID: 15428
		private TextMeshPro m_textMeshPro;

		// Token: 0x04003C45 RID: 15429
		private TextMesh m_textMesh;

		// Token: 0x04003C46 RID: 15430
		private Transform m_transform;

		// Token: 0x04003C47 RID: 15431
		private Transform m_floatingText_Transform;

		// Token: 0x04003C48 RID: 15432
		private Transform m_cameraTransform;

		// Token: 0x04003C49 RID: 15433
		private Vector3 lastPOS = Vector3.zero;

		// Token: 0x04003C4A RID: 15434
		private Quaternion lastRotation = Quaternion.identity;

		// Token: 0x04003C4B RID: 15435
		public int SpawnType;
	}
}
