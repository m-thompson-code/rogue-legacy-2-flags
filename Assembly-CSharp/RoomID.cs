using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020007CD RID: 1997
[Serializable]
public struct RoomID : IEquatable<RoomID>
{
	// Token: 0x06003D7D RID: 15741 RVA: 0x00022091 File Offset: 0x00020291
	public RoomID(Scene scene, int number)
	{
		this = new RoomID(scene.name, number);
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x000220A1 File Offset: 0x000202A1
	public RoomID(string sceneName, int number)
	{
		this.m_sceneName = sceneName;
		this.m_number = number;
	}

	// Token: 0x17001690 RID: 5776
	// (get) Token: 0x06003D7F RID: 15743 RVA: 0x000220B1 File Offset: 0x000202B1
	// (set) Token: 0x06003D80 RID: 15744 RVA: 0x000220B9 File Offset: 0x000202B9
	public int Number
	{
		get
		{
			return this.m_number;
		}
		set
		{
			this.m_number = value;
		}
	}

	// Token: 0x17001691 RID: 5777
	// (get) Token: 0x06003D81 RID: 15745 RVA: 0x000220C2 File Offset: 0x000202C2
	// (set) Token: 0x06003D82 RID: 15746 RVA: 0x000220CA File Offset: 0x000202CA
	public string SceneName
	{
		get
		{
			return this.m_sceneName;
		}
		set
		{
			this.m_sceneName = value;
		}
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x000F8E00 File Offset: 0x000F7000
	public override string ToString()
	{
		string result = "NONE";
		if (!string.IsNullOrEmpty(this.SceneName))
		{
			result = string.Format("{0} {1}", this.SceneName, this.Number);
		}
		return result;
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x000220D3 File Offset: 0x000202D3
	public static bool operator ==(RoomID id1, RoomID id2)
	{
		return id1.SceneName == id2.SceneName && id1.Number == id2.Number;
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x000220FC File Offset: 0x000202FC
	public override bool Equals(object obj)
	{
		return obj is RoomID && this == (RoomID)obj;
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x00022119 File Offset: 0x00020319
	public static bool operator !=(RoomID id1, RoomID id2)
	{
		return !(id1 == id2);
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00022125 File Offset: 0x00020325
	public override int GetHashCode()
	{
		return this.SceneName.GetHashCode() * this.Number;
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x00022139 File Offset: 0x00020339
	public bool Equals(RoomID other)
	{
		return this == other;
	}

	// Token: 0x04003079 RID: 12409
	[SerializeField]
	private string m_sceneName;

	// Token: 0x0400307A RID: 12410
	[SerializeField]
	private int m_number;
}
