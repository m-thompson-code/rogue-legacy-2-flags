using System;
using Rooms;
using UnityEngine;

namespace RLTests
{
	// Token: 0x02000DA3 RID: 3491
	public class MapRoom_Test : MonoBehaviour
	{
		// Token: 0x060062AB RID: 25259 RVA: 0x00002FCA File Offset: 0x000011CA
		private void Start()
		{
		}

		// Token: 0x060062AC RID: 25260 RVA: 0x0003658B File Offset: 0x0003478B
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

		// Token: 0x060062AD RID: 25261 RVA: 0x000365B1 File Offset: 0x000347B1
		private void RunTest2()
		{
			Debug.Log(GridController.GetWorldPositionFromRoomCoordinates(this.m_roomCoords, this.m_roomSize));
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x00170974 File Offset: 0x0016EB74
		private void RunTest()
		{
			MapController.CreateMapRoomEntryForGridPoint(new GridPointManager(this.m_roomNumber, this.m_gridCoords, this.m_biome, this.m_roomType, this.m_roomMetaData, this.m_isMirrored, this.m_isTunnelDestination)).gameObject.SetActive(true);
		}

		// Token: 0x0400509E RID: 20638
		[SerializeField]
		private Vector2Int m_gridCoords;

		// Token: 0x0400509F RID: 20639
		[SerializeField]
		private Vector2Int m_roomSize = Vector2Int.one;

		// Token: 0x040050A0 RID: 20640
		[SerializeField]
		private RoomType m_roomType;

		// Token: 0x040050A1 RID: 20641
		[SerializeField]
		private BiomeType m_biome = BiomeType.Castle;

		// Token: 0x040050A2 RID: 20642
		[SerializeField]
		private RoomMetaData m_roomMetaData;

		// Token: 0x040050A3 RID: 20643
		[SerializeField]
		private int m_roomNumber;

		// Token: 0x040050A4 RID: 20644
		[SerializeField]
		private bool m_isMirrored;

		// Token: 0x040050A5 RID: 20645
		[SerializeField]
		private bool m_isTunnelDestination;

		// Token: 0x040050A6 RID: 20646
		[SerializeField]
		private Vector2 m_roomCoords;
	}
}
