using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x0200085C RID: 2140
	public class ShaderPropAnimator : MonoBehaviour
	{
		// Token: 0x060046F4 RID: 18164 RVA: 0x000FE26C File Offset: 0x000FC46C
		private void Awake()
		{
			this.m_Renderer = base.GetComponent<Renderer>();
			this.m_Material = this.m_Renderer.material;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x000FE28B File Offset: 0x000FC48B
		private void Start()
		{
			base.StartCoroutine(this.AnimateProperties());
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x000FE29A File Offset: 0x000FC49A
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

		// Token: 0x04003BFF RID: 15359
		private Renderer m_Renderer;

		// Token: 0x04003C00 RID: 15360
		private Material m_Material;

		// Token: 0x04003C01 RID: 15361
		public AnimationCurve GlowCurve;

		// Token: 0x04003C02 RID: 15362
		public float m_frame;
	}
}
