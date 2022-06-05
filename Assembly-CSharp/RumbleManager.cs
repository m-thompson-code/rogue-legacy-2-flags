using System;
using Rewired;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
public class RumbleManager
{
	// Token: 0x17001572 RID: 5490
	// (get) Token: 0x06003E83 RID: 16003 RVA: 0x000DC48B File Offset: 0x000DA68B
	public static bool IsPaused
	{
		get
		{
			return RumbleManager.m_isPaused;
		}
	}

	// Token: 0x17001573 RID: 5491
	// (get) Token: 0x06003E84 RID: 16004 RVA: 0x000DC492 File Offset: 0x000DA692
	public static bool IsRumbling
	{
		get
		{
			return RumbleManager.IsLeftRumbling || RumbleManager.IsRightRumbling;
		}
	}

	// Token: 0x17001574 RID: 5492
	// (get) Token: 0x06003E85 RID: 16005 RVA: 0x000DC4A2 File Offset: 0x000DA6A2
	public static bool IsLeftRumbling
	{
		get
		{
			return Time.unscaledTime < RumbleManager.m_leftStartRumbleTimer;
		}
	}

	// Token: 0x17001575 RID: 5493
	// (get) Token: 0x06003E86 RID: 16006 RVA: 0x000DC4B0 File Offset: 0x000DA6B0
	public static bool IsRightRumbling
	{
		get
		{
			return Time.unscaledTime < RumbleManager.m_rightStartRumbleTimer;
		}
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x000DC4C0 File Offset: 0x000DA6C0
	public static void StartRumble(bool useLeftMotor, bool useRightMotor, float amount, float duration = 0f, bool stopAllRumbleFirst = true)
	{
		if (!ReInput.isReady)
		{
			return;
		}
		if (RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Joystick)
		{
			return;
		}
		if (SaveManager.ConfigData.DisableRumble)
		{
			return;
		}
		RumbleManager.m_isPaused = false;
		RumbleManager.m_leftMotorPauseAmount = 0f;
		RumbleManager.m_rightMotorPauseAmount = 0f;
		duration = Mathf.Max(0f, duration);
		if (duration > 0f)
		{
			if (useLeftMotor)
			{
				RumbleManager.m_leftStartRumbleTimer = Time.unscaledTime + duration;
			}
			if (useRightMotor)
			{
				RumbleManager.m_rightStartRumbleTimer = Time.unscaledTime + duration;
			}
		}
		else
		{
			if (useLeftMotor)
			{
				RumbleManager.m_leftStartRumbleTimer = Time.unscaledTime + 99999f;
			}
			if (useRightMotor)
			{
				RumbleManager.m_rightStartRumbleTimer = Time.unscaledTime + 99999f;
			}
		}
		foreach (Joystick joystick in Rewired_RL.Player.controllers.Joysticks)
		{
			if (joystick == RewiredOnStartupController.ActiveControllerUsed && joystick.supportsVibration && joystick.vibrationMotorCount > 0)
			{
				int motorIndex = (joystick.vibrationMotorCount > 1) ? 1 : 0;
				float leftMotorLevel;
				float rightMotorLevel;
				if (stopAllRumbleFirst)
				{
					leftMotorLevel = (useLeftMotor ? amount : 0f);
					rightMotorLevel = (useRightMotor ? amount : 0f);
				}
				else
				{
					leftMotorLevel = (useLeftMotor ? amount : joystick.GetVibration(0));
					rightMotorLevel = (useRightMotor ? amount : joystick.GetVibration(motorIndex));
				}
				joystick.SetVibration(leftMotorLevel, rightMotorLevel, duration, duration);
			}
		}
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x000DC610 File Offset: 0x000DA810
	public static void Start3DRumble(float amount, Vector2 emitterPos, Vector2 listenerPos, float duration = 0f, float distDeltaMod = 1f)
	{
		float num = 2f;
		float num2 = 10f * distDeltaMod;
		float magnitude = (emitterPos - new Vector2(listenerPos.x - num, listenerPos.y)).magnitude;
		float magnitude2 = (emitterPos - new Vector2(listenerPos.x + num, listenerPos.y)).magnitude;
		float num3 = 1f - Mathf.Min(magnitude, num2) / num2;
		if (emitterPos.x < listenerPos.x)
		{
			num3 = Mathf.Max(0.1f, num3);
		}
		float num4 = 1f - Mathf.Min(magnitude2, num2) / num2;
		if (emitterPos.x > listenerPos.x)
		{
			num4 = Mathf.Max(0.1f, num4);
		}
		RumbleManager.StartRumble(true, false, num3 * amount, duration, false);
		RumbleManager.StartRumble(false, true, num4 * amount, duration, false);
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x000DC6E8 File Offset: 0x000DA8E8
	public static void StopRumble(bool stopLeftMotor = true, bool stopRightMotor = true)
	{
		RumbleManager.m_isPaused = false;
		if (!ReInput.isReady)
		{
			return;
		}
		if (!RumbleManager.IsRumbling)
		{
			return;
		}
		foreach (Joystick joystick in Rewired_RL.Player.controllers.Joysticks)
		{
			if (joystick.supportsVibration && joystick.vibrationMotorCount > 0)
			{
				int motorIndex = (joystick.vibrationMotorCount > 1) ? 1 : 0;
				float leftMotorLevel = stopLeftMotor ? 0f : joystick.GetVibration(0);
				float rightMotorLevel = stopRightMotor ? 0f : joystick.GetVibration(motorIndex);
				joystick.SetVibration(leftMotorLevel, rightMotorLevel);
			}
		}
		if (stopLeftMotor)
		{
			RumbleManager.m_leftStartRumbleTimer = 0f;
		}
		if (stopRightMotor)
		{
			RumbleManager.m_rightStartRumbleTimer = 0f;
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x000DC7B4 File Offset: 0x000DA9B4
	public static void SetRumblePaused(bool paused)
	{
		if (RumbleManager.m_isPaused == paused)
		{
			return;
		}
		RumbleManager.m_isPaused = paused;
		if (paused)
		{
			if (RumbleManager.IsLeftRumbling)
			{
				RumbleManager.m_leftPausedDuration = RumbleManager.m_leftStartRumbleTimer - Time.unscaledTime;
				RumbleManager.m_leftMotorPauseAmount = RumbleManager.GetMotorAmount(false);
			}
			else
			{
				RumbleManager.m_leftPausedDuration = 0f;
				RumbleManager.m_leftMotorPauseAmount = 0f;
			}
			if (RumbleManager.IsRightRumbling)
			{
				RumbleManager.m_rightPausedDuration = RumbleManager.m_rightStartRumbleTimer - Time.unscaledTime;
				RumbleManager.m_rightMotorPauseAmount = RumbleManager.GetMotorAmount(true);
			}
			else
			{
				RumbleManager.m_rightPausedDuration = 0f;
				RumbleManager.m_rightMotorPauseAmount = 0f;
			}
			RumbleManager.StopRumble(true, true);
			return;
		}
		if (RumbleManager.m_leftPausedDuration > 0f)
		{
			RumbleManager.StartRumble(true, false, RumbleManager.m_leftMotorPauseAmount, RumbleManager.m_leftPausedDuration, false);
		}
		if (RumbleManager.m_rightMotorPauseAmount > 0f)
		{
			RumbleManager.StartRumble(false, true, RumbleManager.m_rightMotorPauseAmount, RumbleManager.m_rightPausedDuration, false);
		}
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x000DC888 File Offset: 0x000DAA88
	private static float GetMotorAmount(bool getRightMotor)
	{
		if (!ReInput.isReady)
		{
			return 0f;
		}
		float num = 0f;
		foreach (Joystick joystick in Rewired_RL.Player.controllers.Joysticks)
		{
			if (joystick.supportsVibration && joystick.vibrationMotorCount > 0)
			{
				int motorIndex = (joystick.vibrationMotorCount > 1) ? 1 : 0;
				float num2 = (!getRightMotor) ? joystick.GetVibration(0) : joystick.GetVibration(motorIndex);
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	// Token: 0x04002E85 RID: 11909
	private const float MOTOR_DISTANCE_DELTA = 2f;

	// Token: 0x04002E86 RID: 11910
	private const float MAX_DISTANCE = 10f;

	// Token: 0x04002E87 RID: 11911
	private static float m_leftStartRumbleTimer;

	// Token: 0x04002E88 RID: 11912
	private static float m_rightStartRumbleTimer;

	// Token: 0x04002E89 RID: 11913
	private static float m_leftPausedDuration;

	// Token: 0x04002E8A RID: 11914
	private static float m_rightPausedDuration;

	// Token: 0x04002E8B RID: 11915
	private static float m_leftMotorPauseAmount;

	// Token: 0x04002E8C RID: 11916
	private static float m_rightMotorPauseAmount;

	// Token: 0x04002E8D RID: 11917
	private static bool m_isPaused;
}
