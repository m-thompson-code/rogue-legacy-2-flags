using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007EF RID: 2031
public class SegmentedBreakablePlatformParent : SpecialPlatform, IRoomConsumer
{
	// Token: 0x170016D0 RID: 5840
	// (get) Token: 0x06003E92 RID: 16018 RVA: 0x000229DE File Offset: 0x00020BDE
	// (set) Token: 0x06003E93 RID: 16019 RVA: 0x000229E6 File Offset: 0x00020BE6
	public BaseRoom Room { get; private set; }

	// Token: 0x06003E94 RID: 16020 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Awake()
	{
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x000229EF File Offset: 0x00020BEF
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

	// Token: 0x06003E96 RID: 16022 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void SetState(StateID state)
	{
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x000229FE File Offset: 0x00020BFE
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
	}

	// Token: 0x0400311F RID: 12575
	[SerializeField]
	private GameObject m_platformPrefab;

	// Token: 0x04003120 RID: 12576
	private GameObject[] m_segments;
}
