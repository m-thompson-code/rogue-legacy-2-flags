using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class MergeRoomChunkManager : MonoBehaviour
{
	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06002ACE RID: 10958 RVA: 0x00090EC1 File Offset: 0x0008F0C1
	public IRelayLink<Vector2> PlayerEnteredChunkRelay
	{
		get
		{
			return this.m_playerEnteredChunkRelay.link;
		}
	}

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06002ACF RID: 10959 RVA: 0x00090ECE File Offset: 0x0008F0CE
	// (set) Token: 0x06002AD0 RID: 10960 RVA: 0x00090ED6 File Offset: 0x0008F0D6
	public bool IsInitialized { get; private set; }

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x00090EDF File Offset: 0x0008F0DF
	// (set) Token: 0x06002AD2 RID: 10962 RVA: 0x00090EE7 File Offset: 0x0008F0E7
	public MergeRoom MergeRoom
	{
		get
		{
			return this.m_mergeRoom;
		}
		private set
		{
			this.m_mergeRoom = value;
		}
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x00090EF0 File Offset: 0x0008F0F0
	private void Awake()
	{
		this.m_onEnterMergeRoom = new Action<object, EventArgs>(this.OnEnterMergeRoom);
		this.m_onExitMergeRoom = new Action<object, EventArgs>(this.OnExitMergeRoom);
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x00090F16 File Offset: 0x0008F116
	private void OnDestroy()
	{
		if (this.MergeRoom)
		{
			this.MergeRoom.PlayerEnterRelay.RemoveListener(this.m_onEnterMergeRoom);
			this.MergeRoom.PlayerExitRelay.RemoveListener(this.m_onExitMergeRoom);
		}
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x00090F53 File Offset: 0x0008F153
	private void OnEnterMergeRoom(object sender, EventArgs args)
	{
		this.m_runManager = true;
		this.m_currentChunkIndex = -1;
	}

	// Token: 0x06002AD6 RID: 10966 RVA: 0x00090F63 File Offset: 0x0008F163
	private void OnExitMergeRoom(object sender, EventArgs args)
	{
		this.m_runManager = false;
		this.m_currentChunkIndex = -1;
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x00090F74 File Offset: 0x0008F174
	public void Initialize(MergeRoom mergeRoom, List<Room> rooms)
	{
		this.MergeRoom = mergeRoom;
		this.MergeRoom.PlayerEnterRelay.AddListener(this.m_onEnterMergeRoom, false);
		this.MergeRoom.PlayerExitRelay.AddListener(this.m_onExitMergeRoom, false);
		List<Vector2> list = new List<Vector2>(rooms.Count);
		for (int i = 0; i < rooms.Count; i++)
		{
			Room room = rooms[i];
			list.Add(new Vector2(room.Bounds.min.x, room.Bounds.max.x));
		}
		list.Sort(new Comparison<Vector2>(this.SortChunks));
		this.m_mergeRoomChunks = list.ToArray();
		this.m_playerController = PlayerManager.GetPlayerController();
		this.m_currentChunkIndex = -1;
		this.IsInitialized = true;
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x00091045 File Offset: 0x0008F245
	private int SortChunks(Vector2 a, Vector2 b)
	{
		if (a.x < b.x)
		{
			return -1;
		}
		if (a.x > b.x)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x00091068 File Offset: 0x0008F268
	private void FixedUpdate()
	{
		if (this.IsInitialized && this.m_runManager)
		{
			int num = this.m_mergeRoomChunks.Length;
			if (num > 1)
			{
				if (this.m_currentChunkIndex < 0)
				{
					this.UpdateChunkIndex();
					return;
				}
				float x = this.m_playerController.CollisionBounds.min.x;
				float x2 = this.m_playerController.CollisionBounds.max.x;
				bool flag = this.m_currentChunkIndex > 0;
				bool flag2 = this.m_currentChunkIndex < num - 1;
				bool flag3 = false;
				if (flag)
				{
					Vector2 vector = this.m_mergeRoomChunks[this.m_currentChunkIndex - 1];
					if (x2 < vector.x)
					{
						this.UpdateChunkIndex();
						flag3 = true;
					}
					else if (x2 < vector.y)
					{
						this.m_playerEnteredChunkRelay.Dispatch(vector);
						this.m_currentChunkIndex--;
						flag3 = true;
					}
				}
				if (flag2 && !flag3)
				{
					Vector2 vector2 = this.m_mergeRoomChunks[this.m_currentChunkIndex + 1];
					if (x > vector2.y)
					{
						this.UpdateChunkIndex();
						return;
					}
					if (x >= vector2.x)
					{
						this.m_playerEnteredChunkRelay.Dispatch(vector2);
						this.m_currentChunkIndex++;
					}
				}
			}
		}
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x000911A0 File Offset: 0x0008F3A0
	private void UpdateChunkIndex()
	{
		int num = this.m_mergeRoomChunks.Length;
		float x = this.m_playerController.Midpoint.x;
		int currentChunkIndex = this.m_currentChunkIndex;
		bool flag = false;
		if (x < this.m_mergeRoomChunks[0].x)
		{
			this.m_currentChunkIndex = 0;
			flag = true;
		}
		else if (x > this.m_mergeRoomChunks[this.m_mergeRoomChunks.Length - 1].y)
		{
			this.m_currentChunkIndex = this.m_mergeRoomChunks.Length - 1;
			flag = true;
		}
		if (!flag)
		{
			for (int i = 0; i < num; i++)
			{
				if (i >= num - 1)
				{
					this.m_currentChunkIndex = i;
					break;
				}
				float x2 = this.m_mergeRoomChunks[i].x;
				float x3 = this.m_mergeRoomChunks[i + 1].x;
				if (x >= x2 && x < x3)
				{
					this.m_currentChunkIndex = i;
					break;
				}
			}
		}
		if (this.m_currentChunkIndex != currentChunkIndex)
		{
			this.m_playerEnteredChunkRelay.Dispatch(this.m_mergeRoomChunks[this.m_currentChunkIndex]);
		}
	}

	// Token: 0x040022F4 RID: 8948
	private MergeRoom m_mergeRoom;

	// Token: 0x040022F5 RID: 8949
	private Relay<Vector2> m_playerEnteredChunkRelay = new Relay<Vector2>();

	// Token: 0x040022F6 RID: 8950
	private Vector2[] m_mergeRoomChunks;

	// Token: 0x040022F7 RID: 8951
	private PlayerController m_playerController;

	// Token: 0x040022F8 RID: 8952
	private int m_currentChunkIndex;

	// Token: 0x040022F9 RID: 8953
	private Action<object, EventArgs> m_onEnterMergeRoom;

	// Token: 0x040022FA RID: 8954
	private Action<object, EventArgs> m_onExitMergeRoom;

	// Token: 0x040022FB RID: 8955
	private bool m_runManager;
}
