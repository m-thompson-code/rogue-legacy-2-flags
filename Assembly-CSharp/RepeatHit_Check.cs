using System;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class RepeatHit_Check : MonoBehaviour
{
	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x00059C19 File Offset: 0x00057E19
	// (set) Token: 0x06001BD5 RID: 7125 RVA: 0x00059C21 File Offset: 0x00057E21
	public float RepeatHitDuration
	{
		get
		{
			return this.m_repeatHitDuration;
		}
		set
		{
			this.m_repeatHitDuration = value;
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00059C2C File Offset: 0x00057E2C
	private void Awake()
	{
		this.m_hitObjectArray = new RepeatHit_Check.HitObjectContainer[10];
		for (int i = 0; i < 10; i++)
		{
			this.m_hitObjectArray[i] = new RepeatHit_Check.HitObjectContainer();
		}
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x00059C60 File Offset: 0x00057E60
	protected void Update()
	{
		RepeatHit_Check.HitObjectContainer[] hitObjectArray = this.m_hitObjectArray;
		for (int i = 0; i < hitObjectArray.Length; i++)
		{
			hitObjectArray[i].Update();
		}
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00059C8C File Offset: 0x00057E8C
	protected void LateUpdate()
	{
		RepeatHit_Check.HitObjectContainer[] hitObjectArray = this.m_hitObjectArray;
		for (int i = 0; i < hitObjectArray.Length; i++)
		{
			hitObjectArray[i].JustAdded = false;
		}
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x00059CB8 File Offset: 0x00057EB8
	public bool AllowCollision(GameObject objCollided)
	{
		objCollided = objCollided.transform.root.gameObject;
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			RepeatHit_Check.HitObjectContainer hitObjectContainer = this.m_hitObjectArray[i];
			if (hitObjectContainer.ObjectHit == null)
			{
				num = i;
			}
			if (hitObjectContainer.ObjectHit == objCollided && !hitObjectContainer.JustAdded)
			{
				return false;
			}
		}
		RepeatHit_Check.HitObjectContainer hitObjectContainer2 = this.m_hitObjectArray[num];
		hitObjectContainer2.ObjectHit = objCollided;
		hitObjectContainer2.JustAdded = true;
		hitObjectContainer2.HitDuration = this.RepeatHitDuration;
		return true;
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x00059D3C File Offset: 0x00057F3C
	public void ClearCollisionChecks()
	{
		if (this.m_hitObjectArray != null)
		{
			foreach (RepeatHit_Check.HitObjectContainer hitObjectContainer in this.m_hitObjectArray)
			{
				hitObjectContainer.JustAdded = false;
				hitObjectContainer.HitDuration = 0f;
				hitObjectContainer.ObjectHit = null;
			}
		}
	}

	// Token: 0x04001971 RID: 6513
	private const int REPEAT_HIT_ARRAY_SIZE = 10;

	// Token: 0x04001972 RID: 6514
	[SerializeField]
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x04001973 RID: 6515
	protected RepeatHit_Check.HitObjectContainer[] m_hitObjectArray;

	// Token: 0x02000B68 RID: 2920
	protected class HitObjectContainer
	{
		// Token: 0x06005D21 RID: 23841 RVA: 0x0015F7CF File Offset: 0x0015D9CF
		public void Update()
		{
			if (this.ObjectHit == null)
			{
				return;
			}
			this.HitDuration -= Time.deltaTime;
			if (this.HitDuration <= 0f)
			{
				this.ObjectHit = null;
			}
		}

		// Token: 0x04004C82 RID: 19586
		public GameObject ObjectHit;

		// Token: 0x04004C83 RID: 19587
		public float HitDuration;

		// Token: 0x04004C84 RID: 19588
		public bool JustAdded;
	}
}
