using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B9C RID: 2972
public class TextureSizeProfile : ScriptableObject
{
	// Token: 0x17001DE2 RID: 7650
	// (get) Token: 0x06005964 RID: 22884 RVA: 0x00030B15 File Offset: 0x0002ED15
	public List<string> TextureNameList
	{
		get
		{
			return this.m_textureNameList;
		}
	}

	// Token: 0x17001DE3 RID: 7651
	// (get) Token: 0x06005965 RID: 22885 RVA: 0x00030B1D File Offset: 0x0002ED1D
	public List<int> TextureSizeList
	{
		get
		{
			return this.m_textureSizeList;
		}
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x00152C0C File Offset: 0x00150E0C
	public void AddProfileEntry(string texturePath, int maxTextureSize)
	{
		int num = this.m_textureNameList.IndexOf(texturePath);
		if (num == -1)
		{
			this.m_textureNameList.Add(texturePath);
			this.m_textureSizeList.Add(maxTextureSize);
		}
		else
		{
			Debug.Log("<color=red>WARNING: Texture: '" + texturePath + "' already found in TextureSize profile.</color>");
			this.m_textureSizeList[num] = maxTextureSize;
		}
		this.m_tableIsDirty = true;
	}

	// Token: 0x06005967 RID: 22887 RVA: 0x00152C70 File Offset: 0x00150E70
	public int GetEntryTextureSize(string texturePath)
	{
		this.RefreshTextureTable();
		int result;
		if (this.m_textureTable.TryGetValue(texturePath, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x00152C98 File Offset: 0x00150E98
	private void RefreshTextureTable()
	{
		if (this.m_tableIsDirty || this.m_textureTable.Count != this.m_textureNameList.Count)
		{
			this.m_textureTable.Clear();
			for (int i = 0; i < this.m_textureNameList.Count; i++)
			{
				this.m_textureTable.Add(this.m_textureNameList[i], this.m_textureSizeList[i]);
			}
			this.m_tableIsDirty = false;
		}
	}

	// Token: 0x040043C9 RID: 17353
	[SerializeField]
	private List<string> m_textureNameList = new List<string>();

	// Token: 0x040043CA RID: 17354
	[SerializeField]
	private List<int> m_textureSizeList = new List<int>();

	// Token: 0x040043CB RID: 17355
	private Dictionary<string, int> m_textureTable = new Dictionary<string, int>();

	// Token: 0x040043CC RID: 17356
	private bool m_tableIsDirty = true;
}
