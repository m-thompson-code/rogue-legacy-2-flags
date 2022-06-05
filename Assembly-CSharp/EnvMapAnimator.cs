using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000CFB RID: 3323
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x06005EC6 RID: 24262 RVA: 0x000343F4 File Offset: 0x000325F4
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x00034413 File Offset: 0x00032613
	private IEnumerator Start()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		for (;;)
		{
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * this.RotationSpeeds.x, Time.time * this.RotationSpeeds.y, Time.time * this.RotationSpeeds.z), Vector3.one);
			this.m_material.SetMatrix("_EnvMatrix", matrix);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04004DD2 RID: 19922
	public Vector3 RotationSpeeds;

	// Token: 0x04004DD3 RID: 19923
	private TMP_Text m_textMeshPro;

	// Token: 0x04004DD4 RID: 19924
	private Material m_material;
}
