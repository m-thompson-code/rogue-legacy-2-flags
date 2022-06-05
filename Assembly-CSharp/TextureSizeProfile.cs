using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public class TextureSizeProfile : ScriptableObject
{
	// Token: 0x170015EA RID: 5610
	// (get) Token: 0x06004027 RID: 16423 RVA: 0x000E3389 File Offset: 0x000E1589
	public List<string> TextureNameList
	{
		get
		{
			return this.m_textureNameList;
		}
	}

	// Token: 0x170015EB RID: 5611
	// (get) Token: 0x06004028 RID: 16424 RVA: 0x000E3391 File Offset: 0x000E1591
	public List<int> TextureSizeList
	{
		get
		{
			return this.m_textureSizeList;
		}
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x000E339C File Offset: 0x000E159C
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

	// Token: 0x0600402A RID: 16426 RVA: 0x000E3400 File Offset: 0x000E1600
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

	// Token: 0x0600402B RID: 16427 RVA: 0x000E3428 File Offset: 0x000E1628
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

	// Token: 0x04003177 RID: 12663
	[SerializeField]
	private List<string> m_textureNameList = new List<string>();

	// Token: 0x04003178 RID: 12664
	[SerializeField]
	private List<int> m_textureSizeList = new List<int>();

	// Token: 0x04003179 RID: 12665
	private Dictionary<string, int> m_textureTable = new Dictionary<string, int>();

	// Token: 0x0400317A RID: 12666
	private bool m_tableIsDirty = true;
}
