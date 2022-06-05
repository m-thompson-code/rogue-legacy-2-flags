using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class LineageScreen_RoomBuilder : MonoBehaviour
{
	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x06001D33 RID: 7475 RVA: 0x0006043B File Offset: 0x0005E63B
	public List<GameObject> PortraitList
	{
		get
		{
			return this.m_portraitList;
		}
	}

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00060443 File Offset: 0x0005E643
	public List<Vector3> RoomPositionList
	{
		get
		{
			return this.m_roomPositionList;
		}
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x0006044C File Offset: 0x0005E64C
	private void Awake()
	{
		this.m_portraitList = new List<GameObject>();
		this.ConstructPortraitRooms(this.DEBUG_NUM_PORTRAITS);
		this.m_startingRoom.gameObject.SetActive(false);
		this.m_portraitRoom.gameObject.SetActive(false);
		this.m_portrait.gameObject.SetActive(false);
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x000604A4 File Offset: 0x0005E6A4
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

	// Token: 0x04001B2F RID: 6959
	public const int NUM_PORTRAIT_ROWS = 7;

	// Token: 0x04001B30 RID: 6960
	public const int NUM_PORTRAIT_COLUMNS = 3;

	// Token: 0x04001B31 RID: 6961
	public const int NUM_PORTRAITS_PER_ROOM = 21;

	// Token: 0x04001B32 RID: 6962
	private int DEBUG_NUM_PORTRAITS = 40;

	// Token: 0x04001B33 RID: 6963
	[SerializeField]
	private Room m_startingRoom;

	// Token: 0x04001B34 RID: 6964
	[SerializeField]
	private Room m_portraitRoom;

	// Token: 0x04001B35 RID: 6965
	[SerializeField]
	private GameObject m_portrait;

	// Token: 0x04001B36 RID: 6966
	private List<GameObject> m_portraitList;

	// Token: 0x04001B37 RID: 6967
	private List<Vector3> m_roomPositionList;

	// Token: 0x04001B38 RID: 6968
	private Vector3 m_portraitStartingPosOffset = new Vector3(3f, -2f, 0f);
}
