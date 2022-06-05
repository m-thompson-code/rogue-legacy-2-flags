using System;
using Rewired;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class FollowTargetMovementController : MonoBehaviour
{
	// Token: 0x06001588 RID: 5512 RVA: 0x00042F17 File Offset: 0x00041117
	private void Awake()
	{
		this.m_playerController = base.GetComponentInParent<global::PlayerController>();
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00042F25 File Offset: 0x00041125
	public void Reset()
	{
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x00042F38 File Offset: 0x00041138
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

	// Token: 0x0600158B RID: 5515 RVA: 0x00043074 File Offset: 0x00041274
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

	// Token: 0x0600158C RID: 5516 RVA: 0x0004314C File Offset: 0x0004134C
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

	// Token: 0x040014CB RID: 5323
	private const float MAX_MOVEMENT = 5.25f;

	// Token: 0x040014CC RID: 5324
	private const float MOVEMENT_SPEED = 0.25f;

	// Token: 0x040014CD RID: 5325
	private global::PlayerController m_playerController;

	// Token: 0x040014CE RID: 5326
	private Vector2 m_normalizedInput;

	// Token: 0x040014CF RID: 5327
	private Vector2 m_localPosition;
}
