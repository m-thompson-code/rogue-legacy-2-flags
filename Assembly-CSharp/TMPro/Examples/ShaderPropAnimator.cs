using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000D63 RID: 3427
	public class ShaderPropAnimator : MonoBehaviour
	{
		// Token: 0x060061A1 RID: 24993 RVA: 0x00035C7D File Offset: 0x00033E7D
		private void Awake()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
			this.m_Material = this.m_Renderer.material;
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x00035C9C File Offset: 0x00033E9C
		private void Start()
		{
			base.StartCoroutine(this.AnimateProperties());
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x00035CAB File Offset: 0x00033EAB
		private IEnumerator AnimateProperties()
		{
			this.m_frame = UnityEngine.Random.Range(0f, 1f);
			for (;;)
			{
				float value = this.GlowCurve.Evaluate(this.m_frame);
				this.m_Material.SetFloat(ShaderUtilities.ID_GlowPower, value);
				this.m_frame += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x04004F8C RID: 20364
		private Renderer m_Renderer;

		// Token: 0x04004F8D RID: 20365
		private Material m_Material;

		// Token: 0x04004F8E RID: 20366
		public AnimationCurve GlowCurve;

		// Token: 0x04004F8F RID: 20367
		public float m_frame;
	}
}
