using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000893 RID: 2195
	public class RoomRequirementsController
	{
		// Token: 0x060047F8 RID: 18424 RVA: 0x00102C0C File Offset: 0x00100E0C
		public RoomRequirementsController(BiomeController biomeController)
		{
			this.CreateSpecialRoomPool(biomeController);
			int num = biomeController.TargetRoomCountsByRoomType[RoomType.Standard];
			int num2 = biomeController.TargetRoomCountsByRoomType[RoomType.Fairy];
			int num3 = biomeController.TargetRoomCountsByRoomType[RoomType.Trap];
			int num4 = biomeController.TargetRoomCountsByRoomType[RoomType.Bonus];
			int num5 = biomeController.TargetRoomCountsByRoomType[RoomType.Mandatory];
			int num6 = num + num2 + num3 + num4 + num5;
			int count = this.m_specialRoomPool.Count;
			if (count > 0)
			{
				this.m_interval = num6 / count;
				if (num6 % count != 0)
				{
					this.m_interval++;
				}
				if (this.m_interval == 0)
				{
					this.m_interval = 1;
				}
			}
			if (BiomeCreation_EV.FORCE_EASY_ROOMS_AT_BIOME_START.ContainsKey(biomeController.Biome))
			{
				this.m_requiredEasyRoomCount = BiomeCreation_EV.FORCE_EASY_ROOMS_AT_BIOME_START[biomeController.Biome];
			}
			this.m_biomeController = biomeController;
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x00102D08 File Offset: 0x00100F08
		public void AddToBacklog(RoomTypeEntry entry)
		{
			if (this.m_lastEntryRemovedFromBacklog == entry)
			{
				this.m_backlog.Insert(0, entry);
				return;
			}
			this.m_backlog.Add(entry);
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x00102D30 File Offset: 0x00100F30
		private void CreateSpecialRoomPool(BiomeController biomeController)
		{
			int num = biomeController.TargetRoomCountsByRoomType[RoomType.Bonus];
			int num2 = biomeController.TargetRoomCountsByRoomType[RoomType.Fairy];
			int num3 = biomeController.TargetRoomCountsByRoomType[RoomType.Trap];
			int num4 = biomeController.TargetRoomCountsByRoomType[RoomType.Mandatory];
			int num5 = (num + num2 + num3 + num4) / 2;
			List<RoomTypeEntry> list = new List<RoomTypeEntry>();
			for (int i = 0; i < num; i++)
			{
				list.Add(new RoomTypeEntry(RoomType.Bonus));
			}
			for (int j = 0; j < num2; j++)
			{
				list.Add(new RoomTypeEntry(RoomType.Fairy));
			}
			for (int k = 0; k < num3; k++)
			{
				list.Add(new RoomTypeEntry(RoomType.Trap));
			}
			MandatoryRoomEntry[] mandatoryRooms = RoomLibrary.GetSetCollection(biomeController.Biome).MandatoryRooms;
			List<RoomTypeEntry> list2 = new List<RoomTypeEntry>();
			for (int l = 0; l < mandatoryRooms.Length; l++)
			{
				bool flag = false;
				if (mandatoryRooms[l].ReplacementCriteria != ConditionFlag.None)
				{
					flag = ConditionFlag_RL.IsConditionFulfilled(mandatoryRooms[l].ReplacementCriteria);
				}
				RoomMetaData roomMetaData = mandatoryRooms[l].RoomMetaData;
				if (flag && mandatoryRooms[l].ReplacementRoom != null)
				{
					roomMetaData = mandatoryRooms[l].ReplacementRoom;
				}
				RoomTypeEntry item = new RoomTypeEntry(mandatoryRooms[l].RoomType, RoomSide.Any, roomMetaData, false);
				if (mandatoryRooms[l].RoomMetaData.SpecialRoomType == SpecialRoomType.BossEntrance || mandatoryRooms[l].RoomMetaData.SpecialRoomType == SpecialRoomType.Heirloom || mandatoryRooms[l].RoomMetaData.SpecialRoomType == SpecialRoomType.HeirloomEntrance)
				{
					list2.Add(item);
				}
				else
				{
					list.Add(item);
				}
			}
			RoomUtility.ShuffleList<RoomTypeEntry>(list);
			if (list.Count > num5)
			{
				for (int m = list.Count - 1; m >= num5; m--)
				{
					list2.Add(list[m]);
					list.RemoveAt(m);
				}
			}
			RoomUtility.ShuffleList<RoomTypeEntry>(list2);
			this.m_specialRoomPool.AddRange(list);
			this.m_specialRoomPool.AddRange(list2);
			if (BiomeCreation_EV.REQUEST_EASY_FAIRY_ROOMS_IN_BIOME.ContainsKey(biomeController.Biome))
			{
				int num6 = BiomeCreation_EV.REQUEST_EASY_FAIRY_ROOMS_IN_BIOME[biomeController.Biome];
				int num7 = 0;
				for (int n = 0; n < this.m_specialRoomPool.Count; n++)
				{
					if (this.m_specialRoomPool[n].RoomType == RoomType.Fairy)
					{
						this.m_specialRoomPool[n].MustBeEasy = true;
						num7++;
					}
					if (num7 == num6)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x00102FA0 File Offset: 0x001011A0
		public RoomTypeEntry GetRequirements(int roomNumber)
		{
			RoomTypeEntry roomTypeEntry;
			if (this.m_backlog.Count > 0)
			{
				roomTypeEntry = this.m_backlog[0];
				this.m_backlog.RemoveAt(0);
				this.m_lastEntryRemovedFromBacklog = roomTypeEntry;
			}
			else
			{
				if (this.m_specialRoomPool.Count <= 0 || ((this.m_interval <= 0 || roomNumber % this.m_interval != 0) && this.m_biomeController.GridPointManager.StandardRoomCount != this.m_biomeController.TargetRoomCountsByRoomType[RoomType.Standard]))
				{
					bool mustBeEasy = false;
					if (this.m_easyRoomsPlaced < this.m_requiredEasyRoomCount)
					{
						mustBeEasy = true;
						this.m_easyRoomsPlaced++;
					}
					return new RoomTypeEntry(RoomType.Standard, RoomSide.Any, null, mustBeEasy);
				}
				roomTypeEntry = this.m_specialRoomPool[0];
				this.m_specialRoomPool.RemoveAt(0);
			}
			RoomLibrary.GetSetCollection(this.m_biomeController.Biome).SetMaxRoomSizeOverride(Vector2Int.zero);
			if (roomTypeEntry.RoomMetaData != null)
			{
				Vector2Int maxRoomSize = RoomLibrary.GetSetCollection(this.m_biomeController.Biome).MaxRoomSize;
				if (maxRoomSize.x < roomTypeEntry.RoomMetaData.Size.x || maxRoomSize.y < roomTypeEntry.RoomMetaData.Size.y)
				{
					int x = maxRoomSize.x;
					if (x < roomTypeEntry.RoomMetaData.Size.x)
					{
						x = roomTypeEntry.RoomMetaData.Size.x;
					}
					int y = maxRoomSize.y;
					if (y < roomTypeEntry.RoomMetaData.Size.y)
					{
						y = roomTypeEntry.RoomMetaData.Size.y;
					}
					Vector2Int maxRoomSizeOverride = new Vector2Int(x, y);
					RoomLibrary.GetSetCollection(this.m_biomeController.Biome).SetMaxRoomSizeOverride(maxRoomSizeOverride);
				}
			}
			return roomTypeEntry;
		}

		// Token: 0x04003CC4 RID: 15556
		protected RoomTypeEntry m_lastEntryRemovedFromBacklog;

		// Token: 0x04003CC5 RID: 15557
		protected List<RoomTypeEntry> m_backlog = new List<RoomTypeEntry>();

		// Token: 0x04003CC6 RID: 15558
		protected List<RoomTypeEntry> m_specialRoomPool = new List<RoomTypeEntry>();

		// Token: 0x04003CC7 RID: 15559
		private BiomeController m_biomeController;

		// Token: 0x04003CC8 RID: 15560
		private int m_interval = -1;

		// Token: 0x04003CC9 RID: 15561
		private int m_requiredEasyRoomCount = -1;

		// Token: 0x04003CCA RID: 15562
		private int m_easyRoomsPlaced;
	}
}
