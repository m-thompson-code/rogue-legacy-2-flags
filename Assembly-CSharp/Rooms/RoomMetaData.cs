using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Rooms
{
	// Token: 0x020008D0 RID: 2256
	public class RoomMetaData : ScriptableObject
	{
		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x0010A7FC File Offset: 0x001089FC
		// (set) Token: 0x060049F3 RID: 18931 RVA: 0x0010A804 File Offset: 0x00108A04
		public bool CanFlip
		{
			get
			{
				return this.m_canFlip;
			}
			private set
			{
				this.m_canFlip = value;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x0010A80D File Offset: 0x00108A0D
		// (set) Token: 0x060049F5 RID: 18933 RVA: 0x0010A815 File Offset: 0x00108A15
		public bool CanMerge
		{
			get
			{
				return this.m_canMerge;
			}
			private set
			{
				this.m_canMerge = value;
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x0010A81E File Offset: 0x00108A1E
		// (set) Token: 0x060049F7 RID: 18935 RVA: 0x0010A826 File Offset: 0x00108A26
		public DoorLocation[] DoorLocations
		{
			get
			{
				return this.m_doorLocations;
			}
			private set
			{
				this.m_doorLocations = value;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x0010A82F File Offset: 0x00108A2F
		// (set) Token: 0x060049F9 RID: 18937 RVA: 0x0010A837 File Offset: 0x00108A37
		public string RoomPath
		{
			get
			{
				return this.m_roomPath;
			}
			private set
			{
				this.m_roomPath = value;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x0010A840 File Offset: 0x00108A40
		// (set) Token: 0x060049FB RID: 18939 RVA: 0x0010A848 File Offset: 0x00108A48
		public bool IsEasy
		{
			get
			{
				return this.m_isEasy;
			}
			private set
			{
				this.m_isEasy = value;
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x0010A851 File Offset: 0x00108A51
		public IEnumerator LoadPrefabAsync()
		{
			if (!this.m_cachedPrefab && !string.IsNullOrEmpty(this.RoomPath))
			{
				CDGAsyncLoadRequest<GameObject> req = CDGResources.LoadAsync<GameObject>(this.RoomPath, "rooms");
				while (!req.IsDone)
				{
					yield return null;
				}
				this.m_cachedPrefab = req.Asset.GetComponent<Room>();
				req = default(CDGAsyncLoadRequest<GameObject>);
			}
			yield break;
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x0010A860 File Offset: 0x00108A60
		public Room GetPrefab(bool throwOnNull = true)
		{
			if (!this.m_cachedPrefab && !string.IsNullOrEmpty(this.RoomPath))
			{
				this.m_cachedPrefab = CDGResources.Load<GameObject>(this.RoomPath, "rooms", throwOnNull).GetComponent<Room>();
			}
			return this.m_cachedPrefab;
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0010A89E File Offset: 0x00108A9E
		public RoomContentMetaData GetContent()
		{
			if (this.m_cachedContent == null && !string.IsNullOrEmpty(this.ContentPath))
			{
				this.m_cachedContent = CDGResources.Load<RoomContentMetaData>(this.ContentPath, "", true);
			}
			return this.m_cachedContent;
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0010A8D8 File Offset: 0x00108AD8
		public void ClearCachedPrefab()
		{
			this.m_cachedPrefab = null;
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x0010A8E1 File Offset: 0x00108AE1
		public void ClearCachedContentData()
		{
			this.m_cachedContent = null;
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0010A8EA File Offset: 0x00108AEA
		public void UpdateRoomPath(string path)
		{
			this.RoomPath = path;
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x0010A8F3 File Offset: 0x00108AF3
		// (set) Token: 0x06004A03 RID: 18947 RVA: 0x0010A8FB File Offset: 0x00108AFB
		public RoomID ID
		{
			get
			{
				return this.m_id;
			}
			private set
			{
				this.m_id = value;
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x0010A904 File Offset: 0x00108B04
		// (set) Token: 0x06004A05 RID: 18949 RVA: 0x0010A90C File Offset: 0x00108B0C
		public Vector2Int Size
		{
			get
			{
				return this.m_size;
			}
			private set
			{
				this.m_size = value;
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06004A06 RID: 18950 RVA: 0x0010A915 File Offset: 0x00108B15
		// (set) Token: 0x06004A07 RID: 18951 RVA: 0x0010A91D File Offset: 0x00108B1D
		public int[] ExitTunnels
		{
			get
			{
				return this.m_tunnelExits;
			}
			private set
			{
				this.m_tunnelExits = value;
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x0010A926 File Offset: 0x00108B26
		public bool IsSpecialRoom
		{
			get
			{
				return this.SpecialRoomType > SpecialRoomType.None;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x06004A09 RID: 18953 RVA: 0x0010A931 File Offset: 0x00108B31
		// (set) Token: 0x06004A0A RID: 18954 RVA: 0x0010A939 File Offset: 0x00108B39
		public SpecialRoomType SpecialRoomType
		{
			get
			{
				return this.m_specialRoomType;
			}
			private set
			{
				this.m_specialRoomType = value;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x06004A0B RID: 18955 RVA: 0x0010A942 File Offset: 0x00108B42
		public string ContentPath
		{
			get
			{
				return this.m_contentPath;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x06004A0C RID: 18956 RVA: 0x0010A94A File Offset: 0x00108B4A
		// (set) Token: 0x06004A0D RID: 18957 RVA: 0x0010A952 File Offset: 0x00108B52
		public bool ShowIconOnMap
		{
			get
			{
				return this.m_showIconOnMap;
			}
			private set
			{
				this.m_showIconOnMap = value;
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x0010A95B File Offset: 0x00108B5B
		// (set) Token: 0x06004A0F RID: 18959 RVA: 0x0010A963 File Offset: 0x00108B63
		public BiomeType BiomeOverride
		{
			get
			{
				return this.m_biomeOverride;
			}
			private set
			{
				this.m_biomeOverride = value;
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x0010A96C File Offset: 0x00108B6C
		public RoomMetaData(Room room)
		{
			this.UpdateData(room);
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0010A984 File Offset: 0x00108B84
		public void UpdateData(Room room)
		{
			this.CanFlip = room.CanFlip;
			this.CanMerge = room.CanMerge;
			Door[] array = (from door in room.GetComponentsInChildren<Door>(true)
			where !door.DisabledFromLevelEditor
			select door).ToArray<Door>();
			this.DoorLocations = new DoorLocation[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.DoorLocations[i] = new DoorLocation(array[i].Side, array[i].Number);
			}
			this.ID = room.RoomID;
			this.Size = room.Size;
			BaseSpecialRoomController component = room.GetComponent<BaseSpecialRoomController>();
			if (component != null)
			{
				this.SpecialRoomType = component.SpecialRoomType;
			}
			else
			{
				this.SpecialRoomType = SpecialRoomType.None;
			}
			if (room.GetComponent<HeirloomBiomeOverrideController>())
			{
				this.SpecialRoomType = SpecialRoomType.Heirloom;
			}
			TunnelSpawnController componentInChildren = room.GetComponentInChildren<TunnelSpawnController>();
			if (componentInChildren != null && componentInChildren.Category == TunnelCategory.Boss)
			{
				this.SpecialRoomType = SpecialRoomType.BossEntrance;
			}
			bool showIconOnMap = true;
			MapRoomEntryIconOverrideController component2 = room.GetComponent<MapRoomEntryIconOverrideController>();
			if (component2 != null)
			{
				showIconOnMap = component2.ShowRoomIconOnMap;
			}
			this.ShowIconOnMap = showIconOnMap;
			TunnelSpawnController[] array2 = (from spawnController in room.GetComponentsInChildren<TunnelSpawnController>(true)
			where spawnController.Direction == TunnelDirection.Exit
			select spawnController).ToArray<TunnelSpawnController>();
			this.ExitTunnels = new int[array2.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				this.ExitTunnels[j] = array2[j].Index;
			}
			this.BiomeOverride = room.AppearanceOverride;
			this.IsEasy = room.IsEasy;
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x0010AB34 File Offset: 0x00108D34
		public bool GetHasExitTunnel(int index)
		{
			for (int i = 0; i < this.ExitTunnels.Length; i++)
			{
				if (this.ExitTunnels[i] == index)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0010AB64 File Offset: 0x00108D64
		public bool GetHasDoor(DoorLocation doorLocation, bool isMirrored = false)
		{
			if (isMirrored)
			{
				DoorLocation mirrorDoorLocation = RoomUtility.GetMirrorDoorLocation(this.Size, doorLocation);
				for (int i = 0; i < this.DoorLocations.Length; i++)
				{
					if (this.DoorLocations[i] == mirrorDoorLocation)
					{
						return true;
					}
				}
				return false;
			}
			for (int j = 0; j < this.DoorLocations.Length; j++)
			{
				if (this.DoorLocations[j] == doorLocation)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0010ABD8 File Offset: 0x00108DD8
		public IEnumerable<DoorLocation> GetDoorsOnSide(RoomSide side, bool isMirrored = false)
		{
			if (this.DoorLocations == null)
			{
				Debugger.Break();
			}
			if (isMirrored)
			{
				side = RoomUtility.GetMirrorSide(side);
			}
			return from location in this.DoorLocations
			where location.RoomSide == side
			select location;
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0010AC2A File Offset: 0x00108E2A
		public bool GetHasDoorsOnSide(RoomSide side, bool isMirrored)
		{
			return this.GetDoorsOnSide(side, isMirrored).Count<DoorLocation>() > 0;
		}

		// Token: 0x04003E36 RID: 15926
		[SerializeField]
		[ReadOnly]
		private bool m_canFlip;

		// Token: 0x04003E37 RID: 15927
		[SerializeField]
		[ReadOnly]
		private bool m_canMerge;

		// Token: 0x04003E38 RID: 15928
		[SerializeField]
		[ReadOnly]
		private DoorLocation[] m_doorLocations;

		// Token: 0x04003E39 RID: 15929
		[SerializeField]
		[ReadOnly]
		private RoomID m_id;

		// Token: 0x04003E3A RID: 15930
		[SerializeField]
		private string m_roomPath;

		// Token: 0x04003E3B RID: 15931
		[SerializeField]
		[ReadOnly]
		private string m_contentPath;

		// Token: 0x04003E3C RID: 15932
		[SerializeField]
		[ReadOnly]
		private Vector2Int m_size;

		// Token: 0x04003E3D RID: 15933
		[SerializeField]
		[ReadOnly]
		private int[] m_tunnelExits;

		// Token: 0x04003E3E RID: 15934
		[SerializeField]
		[ReadOnly]
		private SpecialRoomType m_specialRoomType;

		// Token: 0x04003E3F RID: 15935
		[SerializeField]
		[ReadOnly]
		private bool m_showIconOnMap = true;

		// Token: 0x04003E40 RID: 15936
		[SerializeField]
		[ReadOnly]
		private bool m_isEasy;

		// Token: 0x04003E41 RID: 15937
		[SerializeField]
		private BiomeType m_biomeOverride;

		// Token: 0x04003E42 RID: 15938
		private Room m_cachedPrefab;

		// Token: 0x04003E43 RID: 15939
		private RoomContentMetaData m_cachedContent;
	}
}
