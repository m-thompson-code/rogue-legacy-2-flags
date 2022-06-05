using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class LineageScreen_RoomBuilder : MonoBehaviour
{
	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06002806 RID: 10246 RVA: 0x000167DE File Offset: 0x000149DE
	public List<GameObject> PortraitList
	{
		get
		{
			return this.m_portraitList;
		}
	}

	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x06002807 RID: 10247 RVA: 0x000167E6 File Offset: 0x000149E6
	public List<Vector3> RoomPositionList
	{
		get
		{
			return this.m_roomPositionList;
		}
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000BC6E4 File Offset: 0x000BA8E4
	private void Awake()
	{
		this.m_portraitList = new List<GameObject>();
		this.ConstructPortraitRooms(this.DEBUG_NUM_PORTRAITS);
		this.m_startingRoom.gameObject.SetActive(false);
		this.m_portraitRoom.gameObject.SetActive(false);
		this.m_portrait.gameObject.SetActive(false);
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x000BC73C File Offset: 0x000BA93C
	private void ConstructPortraitRooms(int numPortraits)
	{
		float num = 21f;
		int num2 = Mathf.CeilToInt((float)numPortraits / num);
		Room room = UnityEngine.Object.Instantiate<Room>(this.m_startingRoom);
		room.Initialise(BiomeType.Castle, RoomType.Standard, -1);
		List<Room> list = new List<Room>();
		list.Add(room);
		this.m_roomPositionList = new List<Vector3>();
		for (int i = 0; i < num2; i++)
		{
			Room room2 = UnityEngine.Object.Instantiate<Room>(this.m_portraitRoom);
			room2.Initialise(BiomeType.Castle, RoomType.Boss, 1);
			Vector3 position = room.gameObject.transform.position;
			position.x -= (float)((i + 1) * room2.UnitDimensions.x);
			room2.gameObject.transform.position = position;
			this.m_roomPositionList.Add(position);
			list.Add(room2);
		}
		Vector3 vector = room.Coordinates;
		vector.y += (float)room.UnitDimensions.y;
		vector.z = 1f;
		vector += this.m_portraitStartingPosOffset;
		GameObject gameObject = new GameObject();
		gameObject.name = "Portrait Container";
		gameObject.transform.position = vector;
		float num3 = (float)room.UnitDimensions.x / 7f;
		float num4 = 4f;
		int num5 = 0;
		for (int j = 0; j < numPortraits; j++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_portrait);
			gameObject2.transform.SetParent(gameObject.transform, false);
			Vector3 position2 = vector;
			if (j % 3 == 0)
			{
				num5++;
			}
			position2.x -= num3 * (float)num5;
			position2.y -= num4 * ((float)j % 3f);
			gameObject2.transform.position = position2;
			this.m_portraitList.Add(gameObject2);
		}
		Vector3 position3 = gameObject.transform.position;
		position3.z = this.m_portrait.transform.position.z;
		gameObject.transform.position = position3;
	}

	// Token: 0x0400233B RID: 9019
	public const int NUM_PORTRAIT_ROWS = 7;

	// Token: 0x0400233C RID: 9020
	public const int NUM_PORTRAIT_COLUMNS = 3;

	// Token: 0x0400233D RID: 9021
	public const int NUM_PORTRAITS_PER_ROOM = 21;

	// Token: 0x0400233E RID: 9022
	private int DEBUG_NUM_PORTRAITS = 40;

	// Token: 0x0400233F RID: 9023
	[SerializeField]
	private Room m_startingRoom;

	// Token: 0x04002340 RID: 9024
	[SerializeField]
	private Room m_portraitRoom;

	// Token: 0x04002341 RID: 9025
	[SerializeField]
	private GameObject m_portrait;

	// Token: 0x04002342 RID: 9026
	private List<GameObject> m_portraitList;

	// Token: 0x04002343 RID: 9027
	private List<Vector3> m_roomPositionList;

	// Token: 0x04002344 RID: 9028
	private Vector3 m_portraitStartingPosOffset = new Vector3(3f, -2f, 0f);
}
