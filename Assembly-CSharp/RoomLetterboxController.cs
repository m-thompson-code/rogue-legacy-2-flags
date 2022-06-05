using System;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public class RoomLetterboxController : MonoBehaviour
{
	// Token: 0x060026CE RID: 9934 RVA: 0x00015B9F File Offset: 0x00013D9F
	private void Awake()
	{
		this.m_letterBoxArray = new GameObject[4];
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.Initialize();
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x00015BC5 File Offset: 0x00013DC5
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x00015BD3 File Offset: 0x00013DD3
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x000B7138 File Offset: 0x000B5338
	private void Initialize()
	{
		for (int i = 0; i < this.m_letterBoxArray.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_letterboxPrefab);
			gameObject.transform.SetParent(base.transform);
			this.m_letterBoxArray[i] = gameObject;
		}
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x000B7180 File Offset: 0x000B5380
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

	// Token: 0x060026D3 RID: 9939 RVA: 0x000B7210 File Offset: 0x000B5410
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

	// Token: 0x04002184 RID: 8580
	[SerializeField]
	private GameObject m_letterboxPrefab;

	// Token: 0x04002185 RID: 8581
	private GameObject[] m_letterBoxArray;

	// Token: 0x04002186 RID: 8582
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x020004B6 RID: 1206
	private enum LetterboxSide
	{
		// Token: 0x04002188 RID: 8584
		Left,
		// Token: 0x04002189 RID: 8585
		Right,
		// Token: 0x0400218A RID: 8586
		Top,
		// Token: 0x0400218B RID: 8587
		Bottom
	}
}
