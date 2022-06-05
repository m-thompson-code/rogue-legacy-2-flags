using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public class SegmentedBreakablePlatformParent : SpecialPlatform, IRoomConsumer
{
	// Token: 0x1700114D RID: 4429
	// (get) Token: 0x06002D92 RID: 11666 RVA: 0x0009A1B4 File Offset: 0x000983B4
	// (set) Token: 0x06002D93 RID: 11667 RVA: 0x0009A1BC File Offset: 0x000983BC
	public BaseRoom Room { get; private set; }

	// Token: 0x06002D94 RID: 11668 RVA: 0x0009A1C5 File Offset: 0x000983C5
	protected override void Awake()
	{
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x0009A1C7 File Offset: 0x000983C7
	protected override IEnumerator Start()
	{
		int num = (int)base.Width / 2;
		this.m_segments = new GameObject[num];
		float num2 = base.transform.position.x - base.Width / 2f;
		for (int i = 0; i < num; i++)
		{
			int num3 = (i == num / 2 && base.Width % 2f != 0f) ? 3 : 2;
			float x = num2 + (float)num3 / 2f;
			Vector2 v = new Vector2(x, base.transform.position.y);
			this.m_segments[i] = UnityEngine.Object.Instantiate<GameObject>(this.m_platformPrefab, v, Quaternion.identity, base.transform);
			SegmentedBreakablePlatform component = this.m_segments[i].GetComponent<SegmentedBreakablePlatform>();
			component.SetRoom(this.Room);
			component.Width = (float)num3;
			num2 += (float)num3;
		}
		yield break;
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x0009A1D6 File Offset: 0x000983D6
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x0009A1D8 File Offset: 0x000983D8
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x04002488 RID: 9352
	[SerializeField]
	private GameObject m_platformPrefab;

	// Token: 0x04002489 RID: 9353
	private GameObject[] m_segments;
}
