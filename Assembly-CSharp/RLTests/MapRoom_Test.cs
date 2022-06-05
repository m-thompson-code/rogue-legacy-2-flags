using System;
using Rooms;
using UnityEngine;

namespace RLTests
{
	// Token: 0x02000883 RID: 2179
	public class MapRoom_Test : MonoBehaviour
	{
		// Token: 0x060047AD RID: 18349 RVA: 0x00101FC0 File Offset: 0x001001C0
		private void Start()
		{
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x00101FC2 File Offset: 0x001001C2
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.RunTest();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.RunTest2();
			}
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x00101FE8 File Offset: 0x001001E8
		private void RunTest2()
		{
			Debug.Log(GridController.GetWorldPositionFromRoomCoordinates(this.m_roomCoords, this.m_roomSize));
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x00102008 File Offset: 0x00100208
		private void RunTest()
		{
			MapController.CreateMapRoomEntryForGridPoint(new GridPointManager(this.m_roomNumber, this.m_gridCoords, this.m_biome, this.m_roomType, this.m_roomMetaData, this.m_isMirrored, this.m_isTunnelDestination)).gameObject.SetActive(true);
		}

		// Token: 0x04003C9C RID: 15516
		[SerializeField]
		private Vector2Int m_gridCoords;

		// Token: 0x04003C9D RID: 15517
		[SerializeField]
		private Vector2Int m_roomSize = Vector2Int.one;

		// Token: 0x04003C9E RID: 15518
		[SerializeField]
		private RoomType m_roomType;

		// Token: 0x04003C9F RID: 15519
		[SerializeField]
		private BiomeType m_biome = BiomeType.Castle;

		// Token: 0x04003CA0 RID: 15520
		[SerializeField]
		private RoomMetaData m_roomMetaData;

		// Token: 0x04003CA1 RID: 15521
		[SerializeField]
		private int m_roomNumber;

		// Token: 0x04003CA2 RID: 15522
		[SerializeField]
		private bool m_isMirrored;

		// Token: 0x04003CA3 RID: 15523
		[SerializeField]
		private bool m_isTunnelDestination;

		// Token: 0x04003CA4 RID: 15524
		[SerializeField]
		private Vector2 m_roomCoords;
	}
}
