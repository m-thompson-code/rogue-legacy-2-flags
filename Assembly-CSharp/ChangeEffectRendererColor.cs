using System;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class ChangeEffectRendererColor : MonoBehaviour
{
	// Token: 0x06001B1F RID: 6943 RVA: 0x000943EC File Offset: 0x000925EC
	public void ChangeColor(Color color)
	{
		SpriteRenderer[] renderersToChange = this.m_renderersToChange;
		for (int i = 0; i < renderersToChange.Length; i++)
		{
			renderersToChange[i].color = color;
		}
	}

	// Token: 0x0400193E RID: 6462
	[SerializeField]
	private SpriteRenderer[] m_renderersToChange;
}
