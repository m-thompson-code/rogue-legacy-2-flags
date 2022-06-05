using System;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public class MergeRoomChunkManager : MonoBehaviour
{
	// Token: 0x170015C7 RID: 5575
	// (get) Token: 0x06003B02 RID: 15106 RVA: 0x00020622 File Offset: 0x0001E822
	public IRelayLink<Vector2> PlayerEnteredChunkRelay
	{
		get
		{
			return this.m_playerEnteredChunkRelay.link;
		}
	}

	// Token: 0x170015C8 RID: 5576
	// (get) Token: 0x06003B03 RID: 15107 RVA: 0x0002062F File Offset: 0x0001E82F
	// (set) Token: 0x06003B04 RID: 15108 RVA: 0x00020637 File Offset: 0x0001E837
	public bool IsInitialized { get; private set; }

	// Token: 0x170015C9 RID: 5577
	// (get) Token: 0x06003B05 RID: 15109 RVA: 0x00020640 File Offset: 0x0001E840
	// (set) Token: 0x06003B06 RID: 15110 RVA: 0x00020648 File Offset: 0x0001E848
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

	// Token: 0x06003B07 RID: 15111 RVA: 0x00020651 File Offset: 0x0001E851
	private void Awake()
	{
		this.m_onEnterMergeRoom = new Action<object, EventArgs>(this.OnEnterMergeRoom);
		this.m_onExitMergeRoom = new Action<object, EventArgs>(this.OnExitMergeRoom);
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x00020677 File Offset: 0x0001E877
	private void OnDestroy()
	{
		if (this.MergeRoom)
		{
			this.MergeRoom.PlayerEnterRelay.RemoveListener(this.m_onEnterMergeRoom);
			this.MergeRoom.PlayerExitRelay.RemoveListener(this.m_onExitMergeRoom);
		}
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x000206B4 File Offset: 0x0001E8B4
	private void OnEnterMergeRoom(object sender, EventArgs args)
	{
		this.m_runManager = true;
		this.m_currentChunkIndex = -1;
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x000206C4 File Offset: 0x0001E8C4
	private void OnExitMergeRoom(object sender, EventArgs args)
	{
		this.m_runManager = false;
		this.m_currentChunkIndex = -1;
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x000F2AD4 File Offset: 0x000F0CD4
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

	// Token: 0x06003B0C RID: 15116 RVA: 0x000206D4 File Offset: 0x0001E8D4
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

	// Token: 0x06003B0D RID: 15117 RVA: 0x000F2BA8 File Offset: 0x000F0DA8
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

	// Token: 0x06003B0E RID: 15118 RVA: 0x000F2CE0 File Offset: 0x000F0EE0
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

	// Token: 0x04002EFA RID: 12026
	private MergeRoom m_mergeRoom;

	// Token: 0x04002EFB RID: 12027
	private Relay<Vector2> m_playerEnteredChunkRelay = new Relay<Vector2>();

	// Token: 0x04002EFC RID: 12028
	private Vector2[] m_mergeRoomChunks;

	// Token: 0x04002EFD RID: 12029
	private PlayerController m_playerController;

	// Token: 0x04002EFE RID: 12030
	private int m_currentChunkIndex;

	// Token: 0x04002EFF RID: 12031
	private Action<object, EventArgs> m_onEnterMergeRoom;

	// Token: 0x04002F00 RID: 12032
	private Action<object, EventArgs> m_onExitMergeRoom;

	// Token: 0x04002F01 RID: 12033
	private bool m_runManager;
}
