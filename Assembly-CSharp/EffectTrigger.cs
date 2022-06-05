using System;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public static class EffectTrigger
{
	// Token: 0x060026B6 RID: 9910 RVA: 0x000801B4 File Offset: 0x0007E3B4
	public static BaseEffect InvokeTrigger(EffectTriggerEntry entry, GameObject selfObj, GameObject otherObj, Vector3 selfObjMidpos, Vector3 otherObjMidpos, EffectTriggerDirection otherDirection, Animator animator = null)
	{
		if (!EffectManager.IsInitialized)
		{
			return null;
		}
		Vector3 vector = entry.PositionOffset;
		float d = 1f;
		EffectTargetType effectTargetType = entry.DeriveScale;
		if (effectTargetType != EffectTargetType.Self)
		{
			if (effectTargetType == EffectTargetType.Other)
			{
				d = Mathf.Abs(otherObj.transform.lossyScale.x) / entry.BaseScaleToDerive;
			}
		}
		else
		{
			d = Mathf.Abs(selfObj.transform.lossyScale.x) / entry.BaseScaleToDerive;
		}
		vector *= d;
		bool flag = false;
		float duration = (entry.DurationType == EffectDurationType.CustomDuration) ? entry.Duration : ((float)entry.DurationType);
		BaseEffect baseEffect = null;
		effectTargetType = entry.FollowTargetType;
		if (effectTargetType != EffectTargetType.None)
		{
			if (effectTargetType != EffectTargetType.Self)
			{
				if (effectTargetType == EffectTargetType.Other)
				{
					IPivotObj component = otherObj.GetComponent<IPivotObj>();
					GameObject gameObject = (component != null) ? component.Pivot : otherObj;
					if (entry.OffsetType == EffectOffsetType.Midpoint)
					{
						gameObject = otherObj;
					}
					if (gameObject.transform.lossyScale.x < 0f)
					{
						vector.x = -vector.x;
					}
					if (entry.OffsetType == EffectOffsetType.Midpoint)
					{
						vector += otherObjMidpos;
					}
					else
					{
						vector += gameObject.transform.position;
					}
					vector.z = gameObject.transform.position.z;
					if (!animator)
					{
						animator = otherObj.GetComponentInChildren<Animator>();
					}
					baseEffect = EffectManager.PlayEffect(gameObject, animator, entry.EffectTypeName, vector, duration, entry.EffectStopType, otherDirection);
					Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
					gameObject.transform.localEulerAngles = Vector3.zero;
					baseEffect.transform.SetParent(gameObject.transform, true);
					if (gameObject.transform.localScale.x < 0f)
					{
						baseEffect.transform.SetLocalScaleX(baseEffect.transform.localScale.x * -1f);
					}
					gameObject.transform.localEulerAngles = localEulerAngles;
					IPreOnDisable component2 = otherObj.GetComponent<IPreOnDisable>();
					if (component2 != null)
					{
						baseEffect.AddDetachListener(component2);
					}
				}
			}
			else
			{
				IPivotObj component3 = selfObj.GetComponent<IPivotObj>();
				GameObject gameObject2 = (component3 != null) ? component3.Pivot : selfObj;
				if (entry.OffsetType == EffectOffsetType.Midpoint)
				{
					gameObject2 = selfObj;
				}
				if (gameObject2.transform.lossyScale.x < 0f)
				{
					vector.x = -vector.x;
				}
				if (entry.OffsetType == EffectOffsetType.Midpoint)
				{
					vector += selfObjMidpos;
				}
				else
				{
					vector += gameObject2.transform.position;
				}
				vector.z = gameObject2.transform.position.z;
				if (!animator)
				{
					animator = selfObj.GetComponentInChildren<Animator>();
				}
				baseEffect = EffectManager.PlayEffect(gameObject2, animator, entry.EffectTypeName, vector, duration, entry.EffectStopType, otherDirection);
				Vector3 localEulerAngles2 = gameObject2.transform.localEulerAngles;
				gameObject2.transform.localEulerAngles = Vector3.zero;
				baseEffect.transform.SetParent(gameObject2.transform, true);
				if (gameObject2.transform.localScale.x < 0f)
				{
					baseEffect.transform.SetLocalScaleX(baseEffect.transform.localScale.x * -1f);
				}
				gameObject2.transform.localEulerAngles = localEulerAngles2;
				IPreOnDisable component4 = selfObj.GetComponent<IPreOnDisable>();
				if (component4 != null)
				{
					baseEffect.AddDetachListener(component4);
				}
			}
		}
		else
		{
			BaseCharacterController component5 = selfObj.GetComponent<BaseCharacterController>();
			if ((component5 && !component5.IsFacingRight) || (!component5 && selfObj.transform.localScale.x < 0f))
			{
				vector.x = -vector.x;
				flag = true;
			}
			if (entry.OffsetType == EffectOffsetType.Midpoint)
			{
				vector += selfObjMidpos;
			}
			else
			{
				vector += selfObj.transform.position;
			}
			if (!animator)
			{
				animator = selfObj.GetComponentInChildren<Animator>();
			}
			baseEffect = EffectManager.PlayEffect(selfObj, animator, entry.EffectTypeName, vector, duration, entry.EffectStopType, otherDirection);
			Vector3 vector2 = selfObjMidpos;
			Vector3 b = otherObjMidpos;
			if (entry.OffsetType == EffectOffsetType.Pivot)
			{
				vector2 = selfObj.transform.position;
				b = otherObj.transform.position;
			}
			Vector3 a = Vector3.Lerp(vector2, b, entry.StartingPosLerp);
			baseEffect.transform.Translate(a - vector2);
		}
		Vector3 localScale = baseEffect.transform.localScale * d;
		localScale.z = baseEffect.transform.localScale.z;
		baseEffect.transform.localScale = localScale;
		Vector3 vector3 = baseEffect.transform.localEulerAngles;
		effectTargetType = entry.DeriveRotation;
		if (effectTargetType != EffectTargetType.Self)
		{
			if (effectTargetType == EffectTargetType.Other)
			{
				if (entry.DeriveRotationFromHeading)
				{
					BaseCharacterController component6 = otherObj.GetComponent<BaseCharacterController>();
					vector3.z = CDGHelper.VectorToAngle(component6.Heading);
				}
				else
				{
					Vector3 eulerAngles = otherObj.transform.eulerAngles;
					if (otherObj.transform.lossyScale.x < 0f && entry.FollowTargetType == EffectTargetType.None)
					{
						eulerAngles.z -= 180f;
					}
					if (entry.OffsetType == EffectOffsetType.Pivot)
					{
						IPivotObj component7 = otherObj.GetComponent<IPivotObj>();
						if (component7 != null)
						{
							eulerAngles = component7.Pivot.transform.eulerAngles;
						}
					}
					vector3 += eulerAngles;
				}
			}
		}
		else if (entry.DeriveRotationFromHeading)
		{
			IHeading component8 = selfObj.GetComponent<IHeading>();
			if (!component8.IsNativeNull())
			{
				vector3.z = CDGHelper.VectorToAngle(component8.Heading);
			}
		}
		else
		{
			Vector3 eulerAngles2 = selfObj.transform.eulerAngles;
			if (selfObj.transform.lossyScale.x < 0f && entry.FollowTargetType == EffectTargetType.None)
			{
				eulerAngles2.z -= 180f;
			}
			if (entry.OffsetType == EffectOffsetType.Pivot)
			{
				IPivotObj component9 = selfObj.GetComponent<IPivotObj>();
				if (component9 != null)
				{
					eulerAngles2 = component9.Pivot.transform.eulerAngles;
				}
			}
			vector3 += eulerAngles2;
		}
		baseEffect.transform.localEulerAngles = vector3;
		if (entry.DeriveRotation != EffectTargetType.None && entry.FollowTargetType == EffectTargetType.None)
		{
			Vector3 position = baseEffect.transform.position;
			Vector3 b2 = entry.PositionOffset * d;
			if (flag)
			{
				b2.x *= -1f;
			}
			Vector3 v = baseEffect.transform.position - b2;
			Vector3 position2 = CDGHelper.RotatedPoint(position, v, vector3.z);
			position2.z = position.z;
			baseEffect.transform.position = position2;
		}
		float num = 0f;
		bool flag2 = false;
		if ((otherDirection & EffectTriggerDirection.MovingLeft) != EffectTriggerDirection.None)
		{
			num += entry.LeftRotationZ;
			flag2 = entry.LeftFlip;
		}
		else if ((otherDirection & EffectTriggerDirection.MovingRight) != EffectTriggerDirection.None)
		{
			num += entry.RightRotationZ;
			flag2 = entry.RightFlip;
		}
		if ((otherDirection & EffectTriggerDirection.MovingDown) != EffectTriggerDirection.None)
		{
			num += entry.DownRotationZ;
			if (!flag2)
			{
				flag2 = entry.DownFlip;
			}
		}
		else if ((otherDirection & EffectTriggerDirection.MovingUp) != EffectTriggerDirection.None)
		{
			num += entry.UpRotationZ;
			if (!flag2)
			{
				flag2 = entry.UpFlip;
			}
		}
		if (flag2)
		{
			baseEffect.transform.localScale = Vector3.Scale(baseEffect.transform.localScale, new Vector3(-1f, 1f, 1f));
		}
		baseEffect.transform.localEulerAngles += new Vector3(0f, 0f, num);
		baseEffect.AnimatorLayer = entry.AnimatorLayer;
		baseEffect.DisableDestroyOnRoomChange = !entry.DestroyOnRoomChange;
		return baseEffect;
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x00080938 File Offset: 0x0007EB38
	public static EffectTriggerDirection GetEffectDirectionFromObject(GameObject obj, EffectTriggerDirection directionsToCheck)
	{
		EffectTriggerDirection effectTriggerDirection = EffectTriggerDirection.None;
		Vector2 vector = Vector2.zero;
		IEffectVelocity component = obj.GetComponent<IEffectVelocity>();
		if (component != null)
		{
			vector = component.EffectVelocity;
		}
		if (vector.x < 0f && (directionsToCheck & EffectTriggerDirection.MovingLeft) != EffectTriggerDirection.None)
		{
			effectTriggerDirection |= EffectTriggerDirection.MovingLeft;
		}
		else if (vector.x > 0f && (directionsToCheck & EffectTriggerDirection.MovingRight) != EffectTriggerDirection.None)
		{
			effectTriggerDirection |= EffectTriggerDirection.MovingRight;
		}
		if (vector.y < 0f && (directionsToCheck & EffectTriggerDirection.MovingDown) != EffectTriggerDirection.None)
		{
			effectTriggerDirection |= EffectTriggerDirection.MovingDown;
		}
		else if (vector.y > 0f && (directionsToCheck & EffectTriggerDirection.MovingUp) != EffectTriggerDirection.None)
		{
			effectTriggerDirection |= EffectTriggerDirection.MovingUp;
		}
		if (vector == Vector2.zero)
		{
			bool flag = obj.transform.localScale.x > 0f;
			BaseCharacterController component2 = obj.GetComponent<BaseCharacterController>();
			if (component2)
			{
				flag = component2.IsFacingRight;
			}
			if (flag)
			{
				effectTriggerDirection |= EffectTriggerDirection.MovingRight;
			}
			else
			{
				effectTriggerDirection |= EffectTriggerDirection.MovingLeft;
			}
		}
		return effectTriggerDirection;
	}
}
