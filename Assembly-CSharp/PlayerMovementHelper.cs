using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class PlayerMovementHelper
{
	// Token: 0x060025D8 RID: 9688 RVA: 0x0001509E File Offset: 0x0001329E
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

	// Token: 0x060025D9 RID: 9689 RVA: 0x000150CE File Offset: 0x000132CE
	public static void ResumeAllMovementInput()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ConditionState = CharacterStates.CharacterConditions.Normal;
		}
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x000150E2 File Offset: 0x000132E2
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

	// Token: 0x060025DB RID: 9691 RVA: 0x000150F8 File Offset: 0x000132F8
	public static IEnumerator MovePlayerLeft(float duration)
	{
		yield return PlayerMovementHelper.MovePlayerHorizontal(-1f, duration);
		yield break;
	}

	// Token: 0x060025DC RID: 9692 RVA: 0x00015107 File Offset: 0x00013307
	public static IEnumerator MovePlayerRight(float duration)
	{
		yield return PlayerMovementHelper.MovePlayerHorizontal(1f, duration);
		yield break;
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x00015116 File Offset: 0x00013316
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

	// Token: 0x060025DE RID: 9694 RVA: 0x0001512C File Offset: 0x0001332C
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

	// Token: 0x060025DF RID: 9695 RVA: 0x00015142 File Offset: 0x00013342
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
