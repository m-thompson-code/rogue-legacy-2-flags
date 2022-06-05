using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020004B5 RID: 1205
[Serializable]
public struct RoomID : IEquatable<RoomID>
{
	// Token: 0x06002CD1 RID: 11473 RVA: 0x00097D3A File Offset: 0x00095F3A
	public RoomID(Scene scene, int number)
	{
		this = new RoomID(scene.name, number);
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x00097D4A File Offset: 0x00095F4A
	public RoomID(string sceneName, int number)
	{
		this.m_sceneName = sceneName;
		this.m_number = number;
	}

	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x00097D5A File Offset: 0x00095F5A
	// (set) Token: 0x06002CD4 RID: 11476 RVA: 0x00097D62 File Offset: 0x00095F62
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

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x00097D6B File Offset: 0x00095F6B
	// (set) Token: 0x06002CD6 RID: 11478 RVA: 0x00097D73 File Offset: 0x00095F73
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

	// Token: 0x06002CD7 RID: 11479 RVA: 0x00097D7C File Offset: 0x00095F7C
	public override string ToString()
	{
		string result = "NONE";
		if (!string.IsNullOrEmpty(this.SceneName))
		{
			result = string.Format("{0} {1}", this.SceneName, this.Number);
		}
		return result;
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x00097DB9 File Offset: 0x00095FB9
	public static bool operator ==(RoomID id1, RoomID id2)
	{
		return id1.SceneName == id2.SceneName && id1.Number == id2.Number;
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x00097DE2 File Offset: 0x00095FE2
	public override bool Equals(object obj)
	{
		return obj is RoomID && this == (RoomID)obj;
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x00097DFF File Offset: 0x00095FFF
	public static bool operator !=(RoomID id1, RoomID id2)
	{
		return !(id1 == id2);
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x00097E0B File Offset: 0x0009600B
	public override int GetHashCode()
	{
		return this.SceneName.GetHashCode() * this.Number;
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x00097E1F File Offset: 0x0009601F
	public bool Equals(RoomID other)
	{
		return this == other;
	}

	// Token: 0x04002416 RID: 9238
	[SerializeField]
	private string m_sceneName;

	// Token: 0x04002417 RID: 9239
	[SerializeField]
	private int m_number;
}
