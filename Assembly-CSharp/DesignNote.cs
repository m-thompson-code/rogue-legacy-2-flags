using System;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public class DesignNote : MonoBehaviour
{
	// Token: 0x170016E7 RID: 5863
	// (get) Token: 0x060043AC RID: 17324 RVA: 0x000ECA6D File Offset: 0x000EAC6D
	// (set) Token: 0x060043AD RID: 17325 RVA: 0x000ECA75 File Offset: 0x000EAC75
	public string Text
	{
		get
		{
			return this.m_text;
		}
		set
		{
			this.m_text = value;
		}
	}

	// Token: 0x170016E8 RID: 5864
	// (get) Token: 0x060043AE RID: 17326 RVA: 0x000ECA7E File Offset: 0x000EAC7E
	// (set) Token: 0x060043AF RID: 17327 RVA: 0x000ECA86 File Offset: 0x000EAC86
	public Color TextColour
	{
		get
		{
			return this.m_textColour;
		}
		set
		{
			this.m_textColour = value;
		}
	}

	// Token: 0x040039E1 RID: 14817
	[SerializeField]
	private string m_text;

	// Token: 0x040039E2 RID: 14818
	[SerializeField]
	private Color m_textColour = Color.yellow;
}
