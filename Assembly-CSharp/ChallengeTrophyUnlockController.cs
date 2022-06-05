using System;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class ChallengeTrophyUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x1700123E RID: 4670
	// (get) Token: 0x060031B1 RID: 12721 RVA: 0x000A809F File Offset: 0x000A629F
	// (set) Token: 0x060031B2 RID: 12722 RVA: 0x000A80A7 File Offset: 0x000A62A7
	public BaseRoom Room { get; private set; }

	// Token: 0x060031B3 RID: 12723 RVA: 0x000A80B0 File Offset: 0x000A62B0
	private void Awake()
	{
		this.m_propSpawner = base.GetComponent<PropSpawnController>();
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x000A80BE File Offset: 0x000A62BE
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x000A80E5 File Offset: 0x000A62E5
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x000A8114 File Offset: 0x000A6314
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (this.m_propSpawner == null || this.m_propSpawner.PropInstance.IsNativeNull())
		{
			throw new Exception("Trophy Prop Spawner or its prop instance was null!");
		}
		GameObject gameObject = this.m_propSpawner.PropInstance.gameObject;
		ChallengeTrophyRank challengeTrophyRank = ChallengeManager.GetChallengeTrophyRank(this.m_challengeToCheckUnlock, true);
		if (challengeTrophyRank == ChallengeTrophyRank.None)
		{
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		SpriteRenderer componentInChildren = this.m_propSpawner.PropInstance.GetComponentInChildren<SpriteRenderer>();
		switch (challengeTrophyRank)
		{
		case ChallengeTrophyRank.Bronze:
			componentInChildren.sprite = IconLibrary.GetChallengeIcon(this.m_challengeToCheckUnlock, ChallengeLibrary.ChallengeIconEntryType.Bronze);
			return;
		case ChallengeTrophyRank.Silver:
			componentInChildren.sprite = IconLibrary.GetChallengeIcon(this.m_challengeToCheckUnlock, ChallengeLibrary.ChallengeIconEntryType.Silver);
			return;
		case ChallengeTrophyRank.Gold:
			componentInChildren.sprite = IconLibrary.GetChallengeIcon(this.m_challengeToCheckUnlock, ChallengeLibrary.ChallengeIconEntryType.Gold);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400271E RID: 10014
	[SerializeField]
	private ChallengeType m_challengeToCheckUnlock;

	// Token: 0x04002720 RID: 10016
	private PropSpawnController m_propSpawner;
}
