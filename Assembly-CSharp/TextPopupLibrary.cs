using System;
using UnityEngine;

// Token: 0x02000412 RID: 1042
[CreateAssetMenu(menuName = "Custom/Libraries/Text Popup Library")]
public class TextPopupLibrary : ScriptableObject
{
	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06002140 RID: 8512 RVA: 0x00011B2A File Offset: 0x0000FD2A
	public static TextPopupTypeTextPopupObjDictionary TextPopupDict
	{
		get
		{
			return TextPopupLibrary.Instance.m_textPopupDict;
		}
	}

	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06002141 RID: 8513 RVA: 0x00011B36 File Offset: 0x0000FD36
	public static TextPopupLibrary Instance
	{
		get
		{
			if (TextPopupLibrary.m_instance == null)
			{
				TextPopupLibrary.m_instance = CDGResources.Load<TextPopupLibrary>("Scriptable Objects/Libraries/TextPopupLibrary", "", true);
			}
			return TextPopupLibrary.m_instance;
		}
	}

	// Token: 0x04001E30 RID: 7728
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TextPopupLibrary";

	// Token: 0x04001E31 RID: 7729
	[SerializeField]
	private TextPopupTypeTextPopupObjDictionary m_textPopupDict;

	// Token: 0x04001E32 RID: 7730
	private static TextPopupLibrary m_instance;

	// Token: 0x02000413 RID: 1043
	[Serializable]
	public class TextPopupEntry
	{
		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x00011B5F File Offset: 0x0000FD5F
		// (set) Token: 0x06002145 RID: 8517 RVA: 0x00011B67 File Offset: 0x0000FD67
		public TextPopupObj TextPopupPrefab
		{
			get
			{
				return this.m_textPopupPrefab;
			}
			set
			{
				this.m_textPopupPrefab = value;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x00011B70 File Offset: 0x0000FD70
		// (set) Token: 0x06002147 RID: 8519 RVA: 0x00011B78 File Offset: 0x0000FD78
		public int PoolSize
		{
			get
			{
				return this.m_poolSize;
			}
			set
			{
				this.m_poolSize = value;
			}
		}

		// Token: 0x04001E33 RID: 7731
		[SerializeField]
		private TextPopupObj m_textPopupPrefab;

		// Token: 0x04001E34 RID: 7732
		[SerializeField]
		private int m_poolSize = 5;
	}
}
