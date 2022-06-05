using System;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class ChangeEffectRendererColor : MonoBehaviour
{
	// Token: 0x060012A1 RID: 4769 RVA: 0x00036D54 File Offset: 0x00034F54
	public void ChangeColor(Color color)
	{
		SpriteRenderer[] renderersToChange = this.m_renderersToChange;
		for (int i = 0; i < renderersToChange.Length; i++)
		{
			renderersToChange[i].color = color;
		}
	}

	// Token: 0x04001308 RID: 4872
	[SerializeField]
	private SpriteRenderer[] m_renderersToChange;
}
