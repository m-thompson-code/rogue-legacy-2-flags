using System;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
public class ChangeShaderColorComponent : MonoBehaviour
{
	// Token: 0x0600420F RID: 16911 RVA: 0x000EB65C File Offset: 0x000E985C
	private void Awake()
	{
		if (ChangeShaderColorComponent.m_matBlock == null)
		{
			ChangeShaderColorComponent.m_matBlock = new MaterialPropertyBlock();
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component)
		{
			component.GetPropertyBlock(ChangeShaderColorComponent.m_matBlock);
			foreach (ShaderID shaderID in this.m_shaderIDsToChange)
			{
				ChangeShaderColorComponent.m_matBlock.SetColor(ShaderID_RL.GetShaderID(shaderID), this.m_color);
			}
			component.SetPropertyBlock(ChangeShaderColorComponent.m_matBlock);
		}
	}

	// Token: 0x04003945 RID: 14661
	private static MaterialPropertyBlock m_matBlock;

	// Token: 0x04003946 RID: 14662
	[SerializeField]
	private Color m_color;

	// Token: 0x04003947 RID: 14663
	[SerializeField]
	private ShaderID[] m_shaderIDsToChange;
}
