using System;
using UnityEngine;

// Token: 0x02000254 RID: 596
[CreateAssetMenu(menuName = "Custom/Libraries/Text Popup Library")]
public class TextPopupLibrary : ScriptableObject
{
	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x0600178C RID: 6028 RVA: 0x000494F3 File Offset: 0x000476F3
	public static TextPopupTypeTextPopupObjDictionary TextPopupDict
	{
		get
		{
			return TextPopupLibrary.Instance.m_textPopupDict;
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x0600178D RID: 6029 RVA: 0x000494FF File Offset: 0x000476FF
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

	// Token: 0x04001716 RID: 5910
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/TextPopupLibrary";

	// Token: 0x04001717 RID: 5911
	[SerializeField]
	private TextPopupTypeTextPopupObjDictionary m_textPopupDict;

	// Token: 0x04001718 RID: 5912
	private static TextPopupLibrary m_instance;

	// Token: 0x02000B37 RID: 2871
	[Serializable]
	public class TextPopupEntry
	{
		// Token: 0x17001E64 RID: 7780
		// (get) Token: 0x06005C21 RID: 23585 RVA: 0x0015BEB7 File Offset: 0x0015A0B7
		// (set) Token: 0x06005C22 RID: 23586 RVA: 0x0015BEBF File Offset: 0x0015A0BF
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

		// Token: 0x17001E65 RID: 7781
		// (get) Token: 0x06005C23 RID: 23587 RVA: 0x0015BEC8 File Offset: 0x0015A0C8
		// (set) Token: 0x06005C24 RID: 23588 RVA: 0x0015BED0 File Offset: 0x0015A0D0
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

		// Token: 0x04004BAF RID: 19375
		[SerializeField]
		private TextPopupObj m_textPopupPrefab;

		// Token: 0x04004BB0 RID: 19376
		[SerializeField]
		private int m_poolSize = 5;
	}
}
