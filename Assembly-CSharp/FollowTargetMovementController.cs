using System;
using Rewired;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class FollowTargetMovementController : MonoBehaviour
{
	// Token: 0x06001EE2 RID: 7906 RVA: 0x00010359 File Offset: 0x0000E559
	private void Awake()
	{
		this.m_playerController = base.GetComponentInParent<global::PlayerController>();
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x00010367 File Offset: 0x0000E567
	public void Reset()
	{
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000A14F4 File Offset: 0x0009F6F4
	private void Update()
	{
		if (this.m_playerController && !this.m_playerController.IsInitialized)
		{
			return;
		}
		if (!ReInput.isReady)
		{
			return;
		}
		Controller lastActiveController = Rewired_RL.Player.controllers.GetLastActiveController();
		if (lastActiveController == null)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		this.m_localPosition = base.transform.localPosition;
		if (lastActiveController.type == ControllerType.Joystick)
		{
			if (!this.m_playerController.CastAbility.IsAiming)
			{
				num = Rewired_RL.Player.GetAxis("MoveHorizontalR");
				num2 = Rewired_RL.Player.GetAxis("MoveVerticalR");
			}
		}
		else if (Rewired_RL.Player.GetButton("FreeLook"))
		{
			num = Rewired_RL.Player.GetAxis("MoveHorizontal");
			num2 = Rewired_RL.Player.GetAxis("MoveVertical");
		}
		this.m_normalizedInput.x = num;
		this.m_normalizedInput.y = num2;
		this.m_normalizedInput.Normalize();
		this.HandleHorizontal(num, Mathf.Abs(this.m_normalizedInput.x));
		this.HandleVertical(num2, Mathf.Abs(this.m_normalizedInput.y));
		base.transform.localPosition = this.m_localPosition;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000A1630 File Offset: 0x0009F830
	private void HandleHorizontal(float horizontalInput, float maxMove)
	{
		float num = horizontalInput * 21f * Time.deltaTime;
		if (horizontalInput != 0f)
		{
			this.m_localPosition.x = Mathf.Clamp(this.m_localPosition.x + num, -5.25f * maxMove, 5.25f * maxMove);
			return;
		}
		if (horizontalInput == 0f && this.m_localPosition.x != 0f)
		{
			num = 21f * Time.deltaTime;
			if (this.m_localPosition.x > 0f)
			{
				this.m_localPosition.x = Mathf.Clamp(this.m_localPosition.x - num, 0f, 5.25f);
				return;
			}
			this.m_localPosition.x = Mathf.Clamp(this.m_localPosition.x + num, -5.25f, 0f);
		}
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000A1708 File Offset: 0x0009F908
	private void HandleVertical(float verticalInput, float maxMove)
	{
		float num = verticalInput * 21f * Time.deltaTime;
		if (verticalInput != 0f)
		{
			this.m_localPosition.y = Mathf.Clamp(this.m_localPosition.y + num, -5.25f * maxMove, 5.25f * maxMove);
			base.transform.localPosition = this.m_localPosition;
			return;
		}
		if (verticalInput == 0f && this.m_localPosition.y != 0f)
		{
			num = 21f * Time.deltaTime;
			if (this.m_localPosition.y > 0f)
			{
				this.m_localPosition.y = Mathf.Clamp(this.m_localPosition.y - num, 0f, 5.25f);
				return;
			}
			this.m_localPosition.y = Mathf.Clamp(this.m_localPosition.y + num, -5.25f, 0f);
		}
	}

	// Token: 0x04001B9A RID: 7066
	private const float MAX_MOVEMENT = 5.25f;

	// Token: 0x04001B9B RID: 7067
	private const float MOVEMENT_SPEED = 0.25f;

	// Token: 0x04001B9C RID: 7068
	private global::PlayerController m_playerController;

	// Token: 0x04001B9D RID: 7069
	private Vector2 m_normalizedInput;

	// Token: 0x04001B9E RID: 7070
	private Vector2 m_localPosition;
}
