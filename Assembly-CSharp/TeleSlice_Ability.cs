using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class TeleSlice_Ability : AimedAbilityFast_RL, ITalent, IAbility
{
	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00028076 File Offset: 0x00026276
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0002807D File Offset: 0x0002627D
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06000D36 RID: 3382 RVA: 0x00028084 File Offset: 0x00026284
	protected override float TellAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0002808B File Offset: 0x0002628B
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00028092 File Offset: 0x00026292
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00028099 File Offset: 0x00026299
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06000D3A RID: 3386 RVA: 0x000280A0 File Offset: 0x000262A0
	protected override float AttackAnimSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000280A7 File Offset: 0x000262A7
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000280AE File Offset: 0x000262AE
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x06000D3D RID: 3389 RVA: 0x000280B5 File Offset: 0x000262B5
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x000280BC File Offset: 0x000262BC
	protected override float GravityReduction
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06000D3F RID: 3391 RVA: 0x000280C3 File Offset: 0x000262C3
	protected override Vector2 BowPushbackAmount
	{
		get
		{
			return new Vector2(0f, 2.5f);
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06000D40 RID: 3392 RVA: 0x000280D4 File Offset: 0x000262D4
	protected override AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Tell;
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x000280D7 File Offset: 0x000262D7
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<MonoBehaviour, EventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000280F4 File Offset: 0x000262F4
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

	// Token: 0x06000D43 RID: 3395 RVA: 0x00028181 File Offset: 0x00026381
	protected override void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyHit, this.m_onEnemyHit);
		base.OnDestroy();
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x00028198 File Offset: 0x00026398
	private void OnEnemyHit(object sender, EventArgs args)
	{
		CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
		if (characterHitEventArgs.Victim.CurrentHealth <= 0f && this.m_firedProjectile && characterHitEventArgs.Attacker == this.m_firedProjectile)
		{
			base.EndCooldownTimer(true);
		}
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x000281E0 File Offset: 0x000263E0
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x000281F3 File Offset: 0x000263F3
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

	// Token: 0x06000D47 RID: 3399 RVA: 0x0002820C File Offset: 0x0002640C
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

	// Token: 0x06000D48 RID: 3400 RVA: 0x00028980 File Offset: 0x00026B80
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_chargeEmitter.Stop();
		if (abilityInterrupted)
		{
			this.m_cancelChargeEmitter.Play();
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x000289A2 File Offset: 0x00026BA2
	private IEnumerator DisableSlopeCheckCoroutine(PlayerController playerController)
	{
		playerController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
		yield return null;
		playerController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
		yield break;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x000289B1 File Offset: 0x00026BB1
	private IEnumerator PushbackGroundCheck()
	{
		yield return null;
		if (!this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.SetVelocity(this.PushbackAmount.x, this.PushbackAmount.y, false);
		}
		yield break;
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x000289C0 File Offset: 0x00026BC0
	protected override void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		base.UpdateArrowAim(true);
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x000289CC File Offset: 0x00026BCC
	protected override void UpdateAimLine()
	{
		Vector3 localEulerAngles = this.m_aimLine.transform.localEulerAngles;
		localEulerAngles.z = this.m_unmoddedAngle;
		this.m_aimLine.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x040010C4 RID: 4292
	[SerializeField]
	private GameObject m_endAimIndicator;

	// Token: 0x040010C5 RID: 4293
	[Header("Audio Event Emitters")]
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;

	// Token: 0x040010C6 RID: 4294
	[SerializeField]
	private StudioEventEmitter m_cancelChargeEmitter;

	// Token: 0x040010C7 RID: 4295
	private Vector2 m_storedSoftZone;

	// Token: 0x040010C8 RID: 4296
	private Action<MonoBehaviour, EventArgs> m_onEnemyHit;

	// Token: 0x040010C9 RID: 4297
	private List<RaycastHit2D> m_platformHitList = new List<RaycastHit2D>(5);
}
