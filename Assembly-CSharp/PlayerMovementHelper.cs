using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class PlayerMovementHelper
{
	// Token: 0x06001B60 RID: 7008 RVA: 0x00057AF9 File Offset: 0x00055CF9
	public static void StopAllMovementInput()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.ConditionState = CharacterStates.CharacterConditions.ControlledMovement;
			playerController.SetVelocity(0f, 0f, false);
			playerController.CharacterMove.ResetHorizontalMovement();
		}
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x00057B29 File Offset: 0x00055D29
	public static void ResumeAllMovementInput()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x00057B3D File Offset: 0x00055D3D
	private static IEnumerator MovePlayerHorizontal(float speed, float duration)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.DisableAbilitiesForXSeconds(0f);
			float totalDuration = Time.time + duration;
			while (Time.time <= totalDuration)
			{
				playerController.CharacterMove.SetHorizontalMove(speed);
				yield return null;
			}
			playerController.CharacterMove.SetHorizontalMove(0f);
			playerController = null;
		}
		yield break;
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x00057B53 File Offset: 0x00055D53
	public static IEnumerator MovePlayerLeft(float duration)
	{
		yield return PlayerMovementHelper.MovePlayerHorizontal(-1f, duration);
		yield break;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x00057B62 File Offset: 0x00055D62
	public static IEnumerator MovePlayerRight(float duration)
	{
		yield return PlayerMovementHelper.MovePlayerHorizontal(1f, duration);
		yield break;
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x00057B71 File Offset: 0x00055D71
	public static IEnumerator MoveTo(Vector2 position, bool checkXAxisOnly)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.DisableAbilitiesForXSeconds(0f);
			Vector2 startingDirectionVector = position - playerController.transform.localPosition;
			bool moveCharacter = true;
			while (moveCharacter)
			{
				Vector2 vector = position - playerController.transform.localPosition;
				vector.x = Mathf.Clamp(vector.x, -1f, 1f);
				vector.y = Mathf.Clamp(vector.y, -1f, 1f);
				if (Mathf.Abs(vector.x) >= 0.1f)
				{
					yield return PlayerMovementHelper.MovePlayerHorizontal(vector.x, 0f);
				}
				else
				{
					moveCharacter = false;
				}
				Vector2 vector2 = position - playerController.transform.localPosition;
				if ((checkXAxisOnly && startingDirectionVector.x == 0f) || (!checkXAxisOnly && startingDirectionVector == Vector2.zero) || (vector2.x > 0f && startingDirectionVector.x < 0f) || (vector2.x < 0f && startingDirectionVector.x > 0f) || (!checkXAxisOnly && vector2.y > 0f && startingDirectionVector.y < 0f) || (!checkXAxisOnly && vector2.y < 0f && startingDirectionVector.y > 0f))
				{
					moveCharacter = false;
				}
			}
			if (checkXAxisOnly)
			{
				playerController.transform.position = new Vector3(position.x, playerController.transform.position.y, playerController.transform.position.z);
			}
			else
			{
				playerController.transform.position = new Vector3(position.x, position.y, playerController.transform.position.z);
			}
			playerController = null;
			startingDirectionVector = default(Vector2);
		}
		yield break;
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x00057B87 File Offset: 0x00055D87
	public static IEnumerator MoveTo_Old(Vector2 position, bool xAxisOnly)
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.DisableAbilitiesForXSeconds(0f);
			float num = CDGHelper.DistanceBetweenPts(position, playerController.transform.position);
			if (xAxisOnly)
			{
				num = Mathf.Abs(playerController.transform.position.x - position.x);
			}
			while (num > 0.1f)
			{
				if (playerController.transform.position.x < position.x)
				{
					yield return PlayerMovementHelper.MovePlayerRight(0f);
				}
				else
				{
					yield return PlayerMovementHelper.MovePlayerLeft(0f);
				}
				if (!xAxisOnly)
				{
					num = CDGHelper.DistanceBetweenPts(position, playerController.transform.position);
				}
				else
				{
					num = Mathf.Abs(playerController.transform.position.x - position.x);
				}
			}
			playerController = null;
		}
		yield break;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x00057B9D File Offset: 0x00055D9D
	public static IEnumerator JumpPlayer(float duration)
	{
		if (PlayerManager.IsInstantiated)
		{
			float timeStart = Time.time;
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.DisableAbilitiesForXSeconds(0f);
			playerController.CharacterJump.JumpStart();
			while (Time.time < timeStart + duration)
			{
				yield return null;
			}
			playerController.CharacterJump.JumpStop();
			playerController = null;
		}
		yield break;
	}
}
