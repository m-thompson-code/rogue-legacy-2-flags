using System;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class RepeatHit_Check : MonoBehaviour
{
	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x06002688 RID: 9864 RVA: 0x000157ED File Offset: 0x000139ED
	// (set) Token: 0x06002689 RID: 9865 RVA: 0x000157F5 File Offset: 0x000139F5
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

	// Token: 0x0600268A RID: 9866 RVA: 0x000B68BC File Offset: 0x000B4ABC
	private void Awake()
	{
		this.m_hitObjectArray = new RepeatHit_Check.HitObjectContainer[10];
		for (int i = 0; i < 10; i++)
		{
			this.m_hitObjectArray[i] = new RepeatHit_Check.HitObjectContainer();
		}
	}

	// Token: 0x0600268B RID: 9867 RVA: 0x000B68F0 File Offset: 0x000B4AF0
	protected void Update()
	{
		RepeatHit_Check.HitObjectContainer[] hitObjectArray = this.m_hitObjectArray;
		for (int i = 0; i < hitObjectArray.Length; i++)
		{
			hitObjectArray[i].Update();
		}
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x000B691C File Offset: 0x000B4B1C
	protected void LateUpdate()
	{
		RepeatHit_Check.HitObjectContainer[] hitObjectArray = this.m_hitObjectArray;
		for (int i = 0; i < hitObjectArray.Length; i++)
		{
			hitObjectArray[i].JustAdded = false;
		}
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x000B6948 File Offset: 0x000B4B48
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

	// Token: 0x0600268E RID: 9870 RVA: 0x000B69CC File Offset: 0x000B4BCC
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

	// Token: 0x0400215E RID: 8542
	private const int REPEAT_HIT_ARRAY_SIZE = 10;

	// Token: 0x0400215F RID: 8543
	[SerializeField]
	private float m_repeatHitDuration = 0.5f;

	// Token: 0x04002160 RID: 8544
	protected RepeatHit_Check.HitObjectContainer[] m_hitObjectArray;

	// Token: 0x020004AB RID: 1195
	protected class HitObjectContainer
	{
		// Token: 0x06002690 RID: 9872 RVA: 0x00015811 File Offset: 0x00013A11
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

		// Token: 0x04002161 RID: 8545
		public GameObject ObjectHit;

		// Token: 0x04002162 RID: 8546
		public float HitDuration;

		// Token: 0x04002163 RID: 8547
		public bool JustAdded;
	}
}
