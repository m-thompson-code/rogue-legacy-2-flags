using System;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class RoomLetterboxController : MonoBehaviour
{
	// Token: 0x06001C0C RID: 7180 RVA: 0x0005A74C File Offset: 0x0005894C
	private void Awake()
	{
		this.m_letterBoxArray = new GameObject[4];
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.Initialize();
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x0005A772 File Offset: 0x00058972
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x0005A780 File Offset: 0x00058980
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x0005A790 File Offset: 0x00058990
	private void Initialize()
	{
		for (int i = 0; i < this.m_letterBoxArray.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_letterboxPrefab);
			gameObject.transform.SetParent(base.transform);
			this.m_letterBoxArray[i] = gameObject;
		}
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x0005A7D8 File Offset: 0x000589D8
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
		if (this.m_letterBoxArray[0] == null)
		{
			this.Initialize();
		}
		if (roomViaDoorEventArgs != null)
		{
			if (!roomViaDoorEventArgs.Room.DisableRoomLetterBoxing)
			{
				this.InitializeLetterbox(RoomLetterboxController.LetterboxSide.Left, roomViaDoorEventArgs.Room);
				this.InitializeLetterbox(RoomLetterboxController.LetterboxSide.Right, roomViaDoorEventArgs.Room);
				this.InitializeLetterbox(RoomLetterboxController.LetterboxSide.Top, roomViaDoorEventArgs.Room);
				this.InitializeLetterbox(RoomLetterboxController.LetterboxSide.Bottom, roomViaDoorEventArgs.Room);
				return;
			}
			GameObject[] letterBoxArray = this.m_letterBoxArray;
			for (int i = 0; i < letterBoxArray.Length; i++)
			{
				letterBoxArray[i].SetActive(false);
			}
		}
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x0005A868 File Offset: 0x00058A68
	private void InitializeLetterbox(RoomLetterboxController.LetterboxSide side, BaseRoom room)
	{
		GameObject gameObject = this.m_letterBoxArray[(int)side];
		gameObject.transform.SetParent(room.gameObject.transform);
		Bounds bounds = room.Collider2D.bounds;
		Vector3 localScale = default(Vector3);
		Vector3 position = default(Vector3);
		float num = 10f;
		if (side > RoomLetterboxController.LetterboxSide.Right)
		{
			if (side - RoomLetterboxController.LetterboxSide.Top <= 1)
			{
				localScale.x = (bounds.size.x + 2f) / 2f;
				localScale.y = num;
				position.x = bounds.center.x;
				position.y = ((side == RoomLetterboxController.LetterboxSide.Top) ? (bounds.max.y + num) : (bounds.min.y - num));
			}
		}
		else
		{
			localScale.x = num;
			localScale.y = (bounds.size.y + 2f) / 2f;
			position.x = ((side == RoomLetterboxController.LetterboxSide.Left) ? (bounds.min.x - num) : (bounds.max.x + num));
			position.y = bounds.center.y;
		}
		localScale.z = 1f;
		position.z = 1f;
		gameObject.transform.localScale = localScale;
		gameObject.transform.position = position;
		if (bounds.size.x == 32f)
		{
			gameObject.layer = 5;
		}
		else
		{
			gameObject.layer = 23;
		}
		gameObject.SetActive(true);
	}

	// Token: 0x04001989 RID: 6537
	[SerializeField]
	private GameObject m_letterboxPrefab;

	// Token: 0x0400198A RID: 6538
	private GameObject[] m_letterBoxArray;

	// Token: 0x0400198B RID: 6539
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x02000B6C RID: 2924
	private enum LetterboxSide
	{
		// Token: 0x04004C91 RID: 19601
		Left,
		// Token: 0x04004C92 RID: 19602
		Right,
		// Token: 0x04004C93 RID: 19603
		Top,
		// Token: 0x04004C94 RID: 19604
		Bottom
	}
}
