using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class ElevatorPlatform : SpecialPlatform
{
	// Token: 0x06002D56 RID: 11606 RVA: 0x00099778 File Offset: 0x00097978
	public override void SetState(StateID state)
	{
		if (state != StateID.One)
		{
			if (state != StateID.Two)
			{
				if (state == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_startMovingDown = true;
					}
					else
					{
						this.m_startMovingDown = false;
					}
				}
			}
			else
			{
				this.m_startMovingDown = true;
			}
		}
		else
		{
			this.m_startMovingDown = false;
		}
		this.m_startingPosY = base.transform.localPosition.y;
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000997D8 File Offset: 0x000979D8
	private void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		float num = Mathf.Sin(Time.time * 2f) / 2f;
		if (this.m_startMovingDown)
		{
			localPosition.y = this.m_startingPosY - num * 3f;
		}
		else
		{
			localPosition.y = this.m_startingPosY + num * 3f;
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x0400245D RID: 9309
	private float m_startingPosY;

	// Token: 0x0400245E RID: 9310
	private bool m_startMovingDown;
}
