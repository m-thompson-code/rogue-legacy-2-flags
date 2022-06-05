using System;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
public class BurstLogic : MonoBehaviour
{
	// Token: 0x17001433 RID: 5171
	// (get) Token: 0x0600354D RID: 13645 RVA: 0x0001D3E7 File Offset: 0x0001B5E7
	// (set) Token: 0x0600354E RID: 13646 RVA: 0x0001D3EF File Offset: 0x0001B5EF
	public Transform DestinationOverride { get; set; }

	// Token: 0x17001434 RID: 5172
	// (get) Token: 0x0600354F RID: 13647 RVA: 0x0001D3F8 File Offset: 0x0001B5F8
	// (set) Token: 0x06003550 RID: 13648 RVA: 0x0001D400 File Offset: 0x0001B600
	public IMidpointObj DestinationMidpointOverride { get; set; }

	// Token: 0x06003551 RID: 13649 RVA: 0x0001D409 File Offset: 0x0001B609
	public void ClearTrail()
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x000E04E4 File Offset: 0x000DE6E4
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

	// Token: 0x06003553 RID: 13651 RVA: 0x000E0544 File Offset: 0x000DE744
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

	// Token: 0x06003554 RID: 13652 RVA: 0x000E05A4 File Offset: 0x000DE7A4
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

	// Token: 0x04002B3C RID: 11068
	[SerializeField]
	private TrailRenderer m_trailRenderer;

	// Token: 0x04002B3D RID: 11069
	private float m_speed;

	// Token: 0x04002B3E RID: 11070
	private Vector2 m_heading;

	// Token: 0x04002B3F RID: 11071
	private float m_startTime;

	// Token: 0x04002B40 RID: 11072
	private float m_duration;

	// Token: 0x04002B41 RID: 11073
	private float m_internalSpeed;

	// Token: 0x04002B42 RID: 11074
	private Vector2 m_moveTowardsStartingPos;

	// Token: 0x04002B45 RID: 11077
	private bool m_movingAway;
}
