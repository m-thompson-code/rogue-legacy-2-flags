using System;
using Rewired;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class RumbleManager
{
	// Token: 0x17001D5C RID: 7516
	// (get) Token: 0x0600577E RID: 22398 RVA: 0x0002FA4F File Offset: 0x0002DC4F
	public static bool IsPaused
	{
		get
		{
			return RumbleManager.m_isPaused;
		}
	}

	// Token: 0x17001D5D RID: 7517
	// (get) Token: 0x0600577F RID: 22399 RVA: 0x0002FA56 File Offset: 0x0002DC56
	public static bool IsRumbling
	{
		get
		{
			return RumbleManager.IsLeftRumbling || RumbleManager.IsRightRumbling;
		}
	}

	// Token: 0x17001D5E RID: 7518
	// (get) Token: 0x06005780 RID: 22400 RVA: 0x0002FA66 File Offset: 0x0002DC66
	public static bool IsLeftRumbling
	{
		get
		{
			return Time.unscaledTime < RumbleManager.m_leftStartRumbleTimer;
		}
	}

	// Token: 0x17001D5F RID: 7519
	// (get) Token: 0x06005781 RID: 22401 RVA: 0x0002FA74 File Offset: 0x0002DC74
	public static bool IsRightRumbling
	{
		get
		{
			return Time.unscaledTime < RumbleManager.m_rightStartRumbleTimer;
		}
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x0014C894 File Offset: 0x0014AA94
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

	// Token: 0x06005783 RID: 22403 RVA: 0x0014C9E4 File Offset: 0x0014ABE4
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

	// Token: 0x06005784 RID: 22404 RVA: 0x0014CABC File Offset: 0x0014ACBC
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

	// Token: 0x06005785 RID: 22405 RVA: 0x0014CB88 File Offset: 0x0014AD88
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

	// Token: 0x06005786 RID: 22406 RVA: 0x0014CC5C File Offset: 0x0014AE5C
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

	// Token: 0x040040A7 RID: 16551
	private const float MOTOR_DISTANCE_DELTA = 2f;

	// Token: 0x040040A8 RID: 16552
	private const float MAX_DISTANCE = 10f;

	// Token: 0x040040A9 RID: 16553
	private static float m_leftStartRumbleTimer;

	// Token: 0x040040AA RID: 16554
	private static float m_rightStartRumbleTimer;

	// Token: 0x040040AB RID: 16555
	private static float m_leftPausedDuration;

	// Token: 0x040040AC RID: 16556
	private static float m_rightPausedDuration;

	// Token: 0x040040AD RID: 16557
	private static float m_leftMotorPauseAmount;

	// Token: 0x040040AE RID: 16558
	private static float m_rightMotorPauseAmount;

	// Token: 0x040040AF RID: 16559
	private static bool m_isPaused;
}
