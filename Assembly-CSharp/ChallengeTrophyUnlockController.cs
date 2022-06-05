using System;
using UnityEngine;

// Token: 0x020008F8 RID: 2296
public class ChallengeTrophyUnlockController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170018C1 RID: 6337
	// (get) Token: 0x060045BF RID: 17855 RVA: 0x000264A7 File Offset: 0x000246A7
	// (set) Token: 0x060045C0 RID: 17856 RVA: 0x000264AF File Offset: 0x000246AF
	public BaseRoom Room { get; private set; }

	// Token: 0x060045C1 RID: 17857 RVA: 0x000264B8 File Offset: 0x000246B8
	private void Awake()
	{
		this.m_propSpawner = base.GetComponent<PropSpawnController>();
	}

	// Token: 0x060045C2 RID: 17858 RVA: 0x000264C6 File Offset: 0x000246C6
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060045C3 RID: 17859 RVA: 0x000264ED File Offset: 0x000246ED
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060045C4 RID: 17860 RVA: 0x0011191C File Offset: 0x0010FB1C
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

	// Token: 0x040035DB RID: 13787
	[SerializeField]
	private ChallengeType m_challengeToCheckUnlock;

	// Token: 0x040035DD RID: 13789
	private PropSpawnController m_propSpawner;
}
