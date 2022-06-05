using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000A61 RID: 2657
[Serializable]
public class CompiledScene_ScriptableObject : ScriptableObject
{
	// Token: 0x17001BA6 RID: 7078
	// (get) Token: 0x0600504E RID: 20558 RVA: 0x0002BD0E File Offset: 0x00029F0E
	public string SceneID
	{
		get
		{
			return this.m_sceneID;
		}
	}

	// Token: 0x17001BA7 RID: 7079
	// (get) Token: 0x0600504F RID: 20559 RVA: 0x00132654 File Offset: 0x00130854
	public List<Room> RoomList
	{
		get
		{
			if (this.m_roomList == null || this.m_roomList.Count == 0 || this.m_roomListIsDirty)
			{
				if (this.RoomMetaData != null)
				{
					this.m_roomList = new List<Room>();
					this.m_roomListIsDirty = false;
					for (int i = 0; i < this.RoomMetaData.Count; i++)
					{
						Room prefab = this.RoomMetaData[i].GetPrefab(false);
						if (prefab != null)
						{
							this.m_roomList.Add(prefab);
						}
						else
						{
							Debug.LogFormat("{0}: Room GameObject in Room GameObject List at index ({1}) in Compiled Scene SO ({2}) is null", new object[]
							{
								Time.frameCount,
								i,
								base.name
							});
						}
					}
				}
				else
				{
					this.m_roomList = new List<Room>();
					Debug.LogFormat("{0}: RoomMetaData list is null in Compiled Scene SO ({1})", new object[]
					{
						Time.frameCount,
						base.name
					});
				}
			}
			return this.m_roomList;
		}
	}

	// Token: 0x17001BA8 RID: 7080
	// (get) Token: 0x06005050 RID: 20560 RVA: 0x0002BD16 File Offset: 0x00029F16
	// (set) Token: 0x06005051 RID: 20561 RVA: 0x0002BD1E File Offset: 0x00029F1E
	public List<RoomMetaData> RoomMetaData
	{
		get
		{
			return this.m_roomMetaData;
		}
		set
		{
			this.m_roomMetaData = value;
		}
	}

	// Token: 0x04003CD2 RID: 15570
	[ReadOnly]
	[SerializeField]
	private string m_sceneID;

	// Token: 0x04003CD3 RID: 15571
	[ReadOnly]
	[SerializeField]
	private List<RoomMetaData> m_roomMetaData;

	// Token: 0x04003CD4 RID: 15572
	private string m_debugColour = string.Format("#{0:X2}{1:X2}{2:X2}", 85, 85, 85);

	// Token: 0x04003CD5 RID: 15573
	private List<Room> m_roomList;

	// Token: 0x04003CD6 RID: 15574
	private bool m_roomListIsDirty;
}
