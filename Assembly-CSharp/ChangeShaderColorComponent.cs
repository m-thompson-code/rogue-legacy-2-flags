using System;
using UnityEngine;

// Token: 0x02000C67 RID: 3175
public class ChangeShaderColorComponent : MonoBehaviour
{
	// Token: 0x06005B98 RID: 23448 RVA: 0x0015A79C File Offset: 0x0015899C
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

	// Token: 0x04004C0A RID: 19466
	private static MaterialPropertyBlock m_matBlock;

	// Token: 0x04004C0B RID: 19467
	[SerializeField]
	private Color m_color;

	// Token: 0x04004C0C RID: 19468
	[SerializeField]
	private ShaderID[] m_shaderIDsToChange;
}
