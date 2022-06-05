using System;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
public class ElevatorPlatform : SpecialPlatform
{
	// Token: 0x06003E2C RID: 15916 RVA: 0x000FA904 File Offset: 0x000F8B04
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

	// Token: 0x06003E2D RID: 15917 RVA: 0x000FA964 File Offset: 0x000F8B64
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

	// Token: 0x040030DB RID: 12507
	private float m_startingPosY;

	// Token: 0x040030DC RID: 12508
	private bool m_startMovingDown;
}
