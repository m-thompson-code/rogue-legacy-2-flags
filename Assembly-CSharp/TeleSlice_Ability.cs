using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class TeleSlice_Ability : AimedAbilityFast_RL, ITalent, IAbility
{
	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06001479 RID: 5241 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x0600147A RID: 5242 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x0600147B RID: 5243 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float TellAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x0600147C RID: 5244 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x0600147D RID: 5245 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x0600147E RID: 5246 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x0600147F RID: 5247 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float AttackAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06001480 RID: 5248 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06001481 RID: 5249 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06001482 RID: 5250 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06001483 RID: 5251 RVA: 0x00006764 File Offset: 0x00004964
	protected override float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06001484 RID: 5252 RVA: 0x0000A4EF File Offset: 0x000086EF
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 2.5f);
		}
	}

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06001485 RID: 5253 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Tell;
		}
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x0000A500 File Offset: 0x00008700
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x00087068 File Offset: 0x00085268
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		float num = 12f / (float)this.m_aimLine.positionCount;
		for (int i = 0; i < this.m_aimLine.positionCount; i++)
		{
			this.m_aimLine.SetPosition(i, new Vector3(num * (float)i, 0f, 0f));
		}
		this.m_endAimIndicator.transform.localPosition = new Vector3(12f, 0f, 0f);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyHit, this.m_onEnemyHit);
		base.Initialize(abilityController, castAbilityType);
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x0000A51A File Offset: 0x0000871A
	protected override void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHit);
		base.OnDestroy();
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x000870F8 File Offset: 0x000852F8
	private void OnEnemyHit(object sender, EventArgs args)
	{
		CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
		if (characterHitEventArgs.Victim.CurrentHealth <= 0f && this.m_firedProjectile && characterHitEventArgs.Attacker == this.m_firedProjectile)
		{
			base.EndCooldownTimer(true);
		}
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x0000A52E File Offset: 0x0000872E
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x0000A541 File Offset: 0x00008741
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState < this.StateToHoldAttackOn)
		{
			if (duration <= 0f)
			{
				yield return null;
			}
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				if (!Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
				{
					base.CancelChangeAnimCoroutine();
					this.m_animator.Play(this.AbilityTellIntroName.Replace("Tell", "Attack"));
					yield break;
				}
				yield return null;
			}
			this.m_animator.SetTrigger("Change_Ability_Anim");
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x00087140 File Offset: 0x00085340
	protected override void FireProjectile()
	{
		Vector2 vector = CDGHelper.AngleToVector(this.m_unmoddedAngle);
		Vector3 localPosition = this.m_abilityController.PlayerController.transform.localPosition;
		if (this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.SetLastStandingPosition(localPosition);
		}
		this.m_abilityController.PlayerController.StartCoroutine(this.DisableSlopeCheckCoroutine(this.m_abilityController.PlayerController));
		LayerMask layerMask = this.m_abilityController.PlayerController.ControllerCorgi.SavedPlatformMask & ~this.m_abilityController.PlayerController.ControllerCorgi.OneWayPlatformMask;
		ContactFilter2D contactFilter = default(ContactFilter2D);
		contactFilter.NoFilter();
		contactFilter.SetLayerMask(layerMask);
		this.m_platformHitList.Clear();
		Physics2D.Raycast(this.m_abilityController.PlayerController.Midpoint, vector, contactFilter, this.m_platformHitList, 12f);
		RaycastHit2D hit = default(RaycastHit2D);
		foreach (RaycastHit2D raycastHit2D in this.m_platformHitList)
		{
			if (raycastHit2D && !raycastHit2D.collider.CompareTag("OneWay"))
			{
				hit = raycastHit2D;
				break;
			}
		}
		LayerMask mask = 1073741824;
		RaycastHit2D hit2 = Physics2D.Raycast(this.m_abilityController.PlayerController.Midpoint, vector, 12f, mask);
		bool flag = false;
		if (hit2)
		{
			flag = hit2.collider.GetComponentInParent<Cloud>();
		}
		float x = this.m_abilityController.PlayerController.transform.localPosition.x;
		Vector3 localPosition2 = this.m_abilityController.PlayerController.transform.localPosition;
		if (hit)
		{
			Vector2 vector2 = -vector;
			Vector3 vector3 = this.m_abilityController.PlayerController.Midpoint - this.m_abilityController.PlayerController.transform.localPosition;
			if (vector.x >= 0f)
			{
				localPosition2.x = Mathf.Max(hit.point.x - vector3.x + vector2.x, localPosition2.x);
			}
			else
			{
				localPosition2.x = Mathf.Min(hit.point.x - vector3.x + vector2.x, localPosition2.x);
			}
			if (vector.y >= 0f)
			{
				localPosition2.y = Mathf.Max(hit.point.y - vector3.y + vector2.y, localPosition2.y);
			}
			else
			{
				localPosition2.y = Mathf.Min(hit.point.y - vector3.y + vector2.y, localPosition2.y);
			}
		}
		else if (flag)
		{
			Vector2 vector4 = -vector;
			Vector3 vector5 = this.m_abilityController.PlayerController.Midpoint - this.m_abilityController.PlayerController.transform.localPosition;
			localPosition2.x = hit2.point.x - vector5.x + vector4.x;
			localPosition2.y = hit2.point.y - vector5.y + vector4.y;
		}
		else
		{
			localPosition2.x += vector.x * 12f;
			localPosition2.y += vector.y * 12f;
		}
		float num = 0f;
		float num2 = 0f;
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (currentPlayerRoom.AppearanceBiomeType != BiomeType.Lineage)
		{
			Rect boundsRect = currentPlayerRoom.BoundsRect;
			if (localPosition2.x < boundsRect.xMin)
			{
				num = boundsRect.xMin - localPosition2.x;
			}
			else if (localPosition2.x > boundsRect.xMax)
			{
				num = boundsRect.xMax - localPosition2.x;
			}
			if (localPosition2.y < boundsRect.yMin)
			{
				num2 = boundsRect.yMin - localPosition2.y;
			}
			else if (localPosition2.y > boundsRect.yMax)
			{
				num2 = boundsRect.yMax - localPosition2.y;
			}
			localPosition2.x += num;
			localPosition2.y += num2;
		}
		if (num != 0f || num2 != 0f)
		{
			Vector2 vector6 = CDGHelper.AngleToVector(CDGHelper.AngleBetweenPts(this.m_abilityController.PlayerController.Midpoint, localPosition2));
			hit = Physics2D.Raycast(this.m_abilityController.PlayerController.Midpoint, vector6, 12f, layerMask);
			if (hit)
			{
				Vector2 vector7 = -vector6;
				Vector3 vector8 = this.m_abilityController.PlayerController.Midpoint - this.m_abilityController.PlayerController.transform.localPosition;
				localPosition2.x = hit.point.x - vector8.x + vector7.x;
				localPosition2.y = hit.point.y - vector8.y + vector7.y;
			}
		}
		this.m_abilityController.PlayerController.transform.localPosition = localPosition2;
		bool flag2 = false;
		if ((x > this.m_abilityController.PlayerController.transform.localPosition.x && this.m_abilityController.PlayerController.IsFacingRight) || (x < this.m_abilityController.PlayerController.transform.localPosition.x && !this.m_abilityController.PlayerController.IsFacingRight))
		{
			this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
			flag2 = true;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.SetRaysParameters();
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		base.StartCoroutine(this.PushbackGroundCheck());
		if (this.m_abilityController.PlayerController.InvincibilityTimer < 0.1f)
		{
			this.m_abilityController.PlayerController.CharacterHitResponse.SetInvincibleTime(0.1f, false, false);
		}
		if ((flag2 && this.m_abilityController.PlayerController.IsFacingRight) || (!flag2 && !this.m_abilityController.PlayerController.IsFacingRight))
		{
			this.m_aimAngle *= -1f;
		}
		CinemachineFramingTransposer componentInChildren = CameraController.CinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponentInChildren<CinemachineFramingTransposer>();
		this.m_storedSoftZone.x = componentInChildren.m_SoftZoneWidth;
		this.m_storedSoftZone.y = componentInChildren.m_SoftZoneHeight;
		componentInChildren.m_SoftZoneWidth = 1f;
		componentInChildren.m_SoftZoneHeight = 2f;
		TweenManager.TweenTo(componentInChildren, 0.25f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"m_SoftZoneWidth",
			this.m_storedSoftZone.x,
			"m_SoftZoneHeight",
			this.m_storedSoftZone.y
		});
		this.m_chargeEmitter.Stop();
		base.FireProjectile();
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x0000A557 File Offset: 0x00008757
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_chargeEmitter.Stop();
		if (abilityInterrupted)
		{
			this.m_cancelChargeEmitter.Play();
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0000A579 File Offset: 0x00008779
	private IEnumerator DisableSlopeCheckCoroutine(PlayerController playerController)
	{
		playerController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
		yield return null;
		playerController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
		yield break;
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x0000A588 File Offset: 0x00008788
	private IEnumerator PushbackGroundCheck()
	{
		yield return null;
		if (!this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.SetVelocity(this.PushbackAmount.x, this.PushbackAmount.y, false);
		}
		yield break;
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x0000A597 File Offset: 0x00008797
	protected override void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		base.UpdateArrowAim(true);
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000878B4 File Offset: 0x00085AB4
	protected override void UpdateAimLine()
	{
		Vector3 localEulerAngles = this.m_aimLine.transform.localEulerAngles;
		localEulerAngles.z = this.m_unmoddedAngle;
		this.m_aimLine.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x04001616 RID: 5654
	[SerializeField]
	private GameObject m_endAimIndicator;

	// Token: 0x04001617 RID: 5655
	[Header("Audio Event Emitters")]
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;

	// Token: 0x04001618 RID: 5656
	[SerializeField]
	private StudioEventEmitter m_cancelChargeEmitter;

	// Token: 0x04001619 RID: 5657
	private Vector2 m_storedSoftZone;

	// Token: 0x0400161A RID: 5658
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;

	// Token: 0x0400161B RID: 5659
	private List<RaycastHit2D> m_platformHitList = new List<RaycastHit2D>(5);
}
