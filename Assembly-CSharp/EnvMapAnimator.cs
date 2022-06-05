using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000823 RID: 2083
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x060044F4 RID: 17652 RVA: 0x000F531A File Offset: 0x000F351A
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x000F5339 File Offset: 0x000F3539
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

	// Token: 0x04003ACA RID: 15050
	public Vector3 RotationSpeeds;

	// Token: 0x04003ACB RID: 15051
	private TMP_Text m_textMeshPro;

	// Token: 0x04003ACC RID: 15052
	private Material m_material;
}
