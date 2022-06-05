using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Rooms
{
	// Token: 0x02000E3A RID: 3642
	public class RoomMetaData : ScriptableObject
	{
		// Token: 0x170020E7 RID: 8423
		// (get) Token: 0x0600668D RID: 26253 RVA: 0x000386CB File Offset: 0x000368CB
		// (set) Token: 0x0600668E RID: 26254 RVA: 0x000386D3 File Offset: 0x000368D3
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

		// Token: 0x170020E8 RID: 8424
		// (get) Token: 0x0600668F RID: 26255 RVA: 0x000386DC File Offset: 0x000368DC
		// (set) Token: 0x06006690 RID: 26256 RVA: 0x000386E4 File Offset: 0x000368E4
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

		// Token: 0x170020E9 RID: 8425
		// (get) Token: 0x06006691 RID: 26257 RVA: 0x000386ED File Offset: 0x000368ED
		// (set) Token: 0x06006692 RID: 26258 RVA: 0x000386F5 File Offset: 0x000368F5
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

		// Token: 0x170020EA RID: 8426
		// (get) Token: 0x06006693 RID: 26259 RVA: 0x000386FE File Offset: 0x000368FE
		// (set) Token: 0x06006694 RID: 26260 RVA: 0x00038706 File Offset: 0x00036906
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

		// Token: 0x170020EB RID: 8427
		// (get) Token: 0x06006695 RID: 26261 RVA: 0x0003870F File Offset: 0x0003690F
		// (set) Token: 0x06006696 RID: 26262 RVA: 0x00038717 File Offset: 0x00036917
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

		// Token: 0x06006697 RID: 26263 RVA: 0x00038720 File Offset: 0x00036920
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

		// Token: 0x06006698 RID: 26264 RVA: 0x0003872F File Offset: 0x0003692F
		public Room GetPrefab(bool throwOnNull = true)
		{
			if (!this.m_cachedPrefab && !string.IsNullOrEmpty(this.RoomPath))
			{
				this.m_cachedPrefab = CDGResources.Load<GameObject>(this.RoomPath, "rooms", throwOnNull).GetComponent<Room>();
			}
			return this.m_cachedPrefab;
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x0003876D File Offset: 0x0003696D
		public RoomContentMetaData GetContent()
		{
			if (this.m_cachedContent == null && !string.IsNullOrEmpty(this.ContentPath))
			{
				this.m_cachedContent = CDGResources.Load<RoomContentMetaData>(this.ContentPath, "", true);
			}
			return this.m_cachedContent;
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x000387A7 File Offset: 0x000369A7
		public void ClearCachedPrefab()
		{
			this.m_cachedPrefab = null;
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x000387B0 File Offset: 0x000369B0
		public void ClearCachedContentData()
		{
			this.m_cachedContent = null;
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x000387B9 File Offset: 0x000369B9
		public void UpdateRoomPath(string path)
		{
			this.RoomPath = path;
		}

		// Token: 0x170020EC RID: 8428
		// (get) Token: 0x0600669D RID: 26269 RVA: 0x000387C2 File Offset: 0x000369C2
		// (set) Token: 0x0600669E RID: 26270 RVA: 0x000387CA File Offset: 0x000369CA
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

		// Token: 0x170020ED RID: 8429
		// (get) Token: 0x0600669F RID: 26271 RVA: 0x000387D3 File Offset: 0x000369D3
		// (set) Token: 0x060066A0 RID: 26272 RVA: 0x000387DB File Offset: 0x000369DB
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

		// Token: 0x170020EE RID: 8430
		// (get) Token: 0x060066A1 RID: 26273 RVA: 0x000387E4 File Offset: 0x000369E4
		// (set) Token: 0x060066A2 RID: 26274 RVA: 0x000387EC File Offset: 0x000369EC
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

		// Token: 0x170020EF RID: 8431
		// (get) Token: 0x060066A3 RID: 26275 RVA: 0x000387F5 File Offset: 0x000369F5
		public bool IsSpecialRoom
		{
			get
			{
				return this.SpecialRoomType > SpecialRoomType.None;
			}
		}

		// Token: 0x170020F0 RID: 8432
		// (get) Token: 0x060066A4 RID: 26276 RVA: 0x00038800 File Offset: 0x00036A00
		// (set) Token: 0x060066A5 RID: 26277 RVA: 0x00038808 File Offset: 0x00036A08
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

		// Token: 0x170020F1 RID: 8433
		// (get) Token: 0x060066A6 RID: 26278 RVA: 0x00038811 File Offset: 0x00036A11
		public string ContentPath
		{
			get
			{
				return this.m_contentPath;
			}
		}

		// Token: 0x170020F2 RID: 8434
		// (get) Token: 0x060066A7 RID: 26279 RVA: 0x00038819 File Offset: 0x00036A19
		// (set) Token: 0x060066A8 RID: 26280 RVA: 0x00038821 File Offset: 0x00036A21
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

		// Token: 0x170020F3 RID: 8435
		// (get) Token: 0x060066A9 RID: 26281 RVA: 0x0003882A File Offset: 0x00036A2A
		// (set) Token: 0x060066AA RID: 26282 RVA: 0x00038832 File Offset: 0x00036A32
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

		// Token: 0x060066AB RID: 26283 RVA: 0x0003883B File Offset: 0x00036A3B
		public RoomMetaData(Room room)
		{
			this.UpdateData(room);
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x0017B480 File Offset: 0x00179680
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

		// Token: 0x060066AD RID: 26285 RVA: 0x0017B630 File Offset: 0x00179830
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

		// Token: 0x060066AE RID: 26286 RVA: 0x0017B660 File Offset: 0x00179860
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

		// Token: 0x060066AF RID: 26287 RVA: 0x0017B6D4 File Offset: 0x001798D4
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

		// Token: 0x060066B0 RID: 26288 RVA: 0x00038851 File Offset: 0x00036A51
		public bool GetHasDoorsOnSide(RoomSide side, bool isMirrored)
		{
			return this.GetDoorsOnSide(side, isMirrored).Count<DoorLocation>() > 0;
		}

		// Token: 0x04005340 RID: 21312
		[SerializeField]
		[ReadOnly]
		private bool m_canFlip;

		// Token: 0x04005341 RID: 21313
		[SerializeField]
		[ReadOnly]
		private bool m_canMerge;

		// Token: 0x04005342 RID: 21314
		[SerializeField]
		[ReadOnly]
		private DoorLocation[] m_doorLocations;

		// Token: 0x04005343 RID: 21315
		[SerializeField]
		[ReadOnly]
		private RoomID m_id;

		// Token: 0x04005344 RID: 21316
		[SerializeField]
		private string m_roomPath;

		// Token: 0x04005345 RID: 21317
		[SerializeField]
		[ReadOnly]
		private string m_contentPath;

		// Token: 0x04005346 RID: 21318
		[SerializeField]
		[ReadOnly]
		private Vector2Int m_size;

		// Token: 0x04005347 RID: 21319
		[SerializeField]
		[ReadOnly]
		private int[] m_tunnelExits;

		// Token: 0x04005348 RID: 21320
		[SerializeField]
		[ReadOnly]
		private SpecialRoomType m_specialRoomType;

		// Token: 0x04005349 RID: 21321
		[SerializeField]
		[ReadOnly]
		private bool m_showIconOnMap = true;

		// Token: 0x0400534A RID: 21322
		[SerializeField]
		[ReadOnly]
		private bool m_isEasy;

		// Token: 0x0400534B RID: 21323
		[SerializeField]
		private BiomeType m_biomeOverride;

		// Token: 0x0400534C RID: 21324
		private Room m_cachedPrefab;

		// Token: 0x0400534D RID: 21325
		private RoomContentMetaData m_cachedContent;
	}
}
