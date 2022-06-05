using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x02000634 RID: 1588
[Serializable]
public class CompiledScene_ScriptableObject : ScriptableObject
{
	// Token: 0x1700143F RID: 5183
	// (get) Token: 0x0600396F RID: 14703 RVA: 0x000C3FDC File Offset: 0x000C21DC
	public string SceneID
	{
		get
		{
			return this.m_sceneID;
		}
	}

	// Token: 0x17001440 RID: 5184
	// (get) Token: 0x06003970 RID: 14704 RVA: 0x000C3FE4 File Offset: 0x000C21E4
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

	// Token: 0x17001441 RID: 5185
	// (get) Token: 0x06003971 RID: 14705 RVA: 0x000C40D6 File Offset: 0x000C22D6
	// (set) Token: 0x06003972 RID: 14706 RVA: 0x000C40DE File Offset: 0x000C22DE
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

	// Token: 0x04002C40 RID: 11328
	[ReadOnly]
	[SerializeField]
	private string m_sceneID;

	// Token: 0x04002C41 RID: 11329
	[ReadOnly]
	[SerializeField]
	private List<RoomMetaData> m_roomMetaData;

	// Token: 0x04002C42 RID: 11330
	private string m_debugColour = string.Format("#{0:X2}{1:X2}{2:X2}", 85, 85, 85);

	// Token: 0x04002C43 RID: 11331
	private List<Room> m_roomList;

	// Token: 0x04002C44 RID: 11332
	private bool m_roomListIsDirty;
}
