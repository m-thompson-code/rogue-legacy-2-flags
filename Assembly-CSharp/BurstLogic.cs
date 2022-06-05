using System;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class BurstLogic : MonoBehaviour
{
	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x0600269E RID: 9886 RVA: 0x0007FD6A File Offset: 0x0007DF6A
	// (set) Token: 0x0600269F RID: 9887 RVA: 0x0007FD72 File Offset: 0x0007DF72
	public Transform DestinationOverride { get; set; }

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x060026A0 RID: 9888 RVA: 0x0007FD7B File Offset: 0x0007DF7B
	// (set) Token: 0x060026A1 RID: 9889 RVA: 0x0007FD83 File Offset: 0x0007DF83
	public IMidpointObj DestinationMidpointOverride { get; set; }

	// Token: 0x060026A2 RID: 9890 RVA: 0x0007FD8C File Offset: 0x0007DF8C
	public void ClearTrail()
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x060026A3 RID: 9891 RVA: 0x0007FDA8 File Offset: 0x0007DFA8
	public void Burst(float speed, float angle = -1f)
	{
		this.m_speed = speed;
		if (angle < 0f)
		{
			angle = (float)UnityEngine.Random.Range(0, 360);
		}
		this.m_heading = CDGHelper.AngleToVector(angle);
		this.m_duration = 0.25f;
		this.m_startTime = Time.time;
		this.m_movingAway = true;
		this.m_internalSpeed = 0f;
	}

	// Token: 0x060026A4 RID: 9892 RVA: 0x0007FE08 File Offset: 0x0007E008
	public void BurstTowards(float speed)
	{
		this.m_heading = Vector2.one;
		this.m_speed = speed;
		this.m_duration = 0.25f;
		this.m_startTime = Time.time;
		this.m_movingAway = false;
		this.m_internalSpeed = 0f;
		this.m_moveTowardsStartingPos = base.transform.position;
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x0007FE68 File Offset: 0x0007E068
	private void FixedUpdate()
	{
		if (this.m_movingAway)
		{
			Vector3 position = base.transform.position;
			position.x += this.m_heading.x * this.m_internalSpeed * Time.deltaTime;
			position.y += this.m_heading.y * this.m_internalSpeed * Time.deltaTime;
			base.transform.position = position;
			float num = (Time.time - this.m_startTime) / this.m_duration;
			this.m_internalSpeed = Mathf.Lerp(this.m_speed, 0f, num);
			if (num >= 1f)
			{
				this.m_movingAway = false;
				this.m_moveTowardsStartingPos = base.transform.position;
				this.m_startTime = Time.time;
				return;
			}
		}
		else
		{
			float num2 = (Time.time - this.m_startTime) / this.m_duration;
			Vector3 position2 = base.transform.position;
			if (this.DestinationOverride == null && this.DestinationMidpointOverride == null)
			{
				PlayerController playerController = PlayerManager.GetPlayerController();
				position2.x = Mathf.Lerp(this.m_moveTowardsStartingPos.x, playerController.Midpoint.x, num2);
				position2.y = Mathf.Lerp(this.m_moveTowardsStartingPos.y, playerController.Midpoint.y, num2);
			}
			else
			{
				Vector2 vector = (this.DestinationMidpointOverride != null) ? this.DestinationMidpointOverride.Midpoint : this.DestinationOverride.position;
				position2.x = Mathf.Lerp(this.m_moveTowardsStartingPos.x, vector.x, num2);
				position2.y = Mathf.Lerp(this.m_moveTowardsStartingPos.y, vector.y, num2);
			}
			base.transform.position = position2;
			if (num2 >= 1f)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
	}

	// Token: 0x0400205C RID: 8284
	[SerializeField]
	private TrailRenderer m_trailRenderer;

	// Token: 0x0400205D RID: 8285
	private float m_speed;

	// Token: 0x0400205E RID: 8286
	private Vector2 m_heading;

	// Token: 0x0400205F RID: 8287
	private float m_startTime;

	// Token: 0x04002060 RID: 8288
	private float m_duration;

	// Token: 0x04002061 RID: 8289
	private float m_internalSpeed;

	// Token: 0x04002062 RID: 8290
	private Vector2 m_moveTowardsStartingPos;

	// Token: 0x04002065 RID: 8293
	private bool m_movingAway;
}
