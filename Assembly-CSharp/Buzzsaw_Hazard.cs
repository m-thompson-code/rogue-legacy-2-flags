using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000715 RID: 1813
public class Buzzsaw_Hazard : SingleLine_Multi_Hazard, IBodyOnEnterHitResponse, IHitResponse
{
	// Token: 0x170014BA RID: 5306
	// (get) Token: 0x06003754 RID: 14164 RVA: 0x000E584C File Offset: 0x000E3A4C
	public float SawPosition
	{
		get
		{
			float num = this.m_pivot.transform.localPosition.x - -1f * this.m_halfWidth;
			float num2 = 2f * this.m_halfWidth;
			return num / num2;
		}
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x0001E710 File Offset: 0x0001C910
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_retractWaitYield = new WaitRL_Yield(2f, false);
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x000E588C File Offset: 0x000E3A8C
	public override void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		base.Initialize(pivot, width, hazardArgs);
		this.m_lineSprite.gameObject.transform.localPosition = this.m_startingLocalPos;
		this.m_lineSprite.size = new Vector2((float)width, this.m_lineSprite.size.y);
		if (!this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_hazard_sawTrap_on_loop", this.m_buzzSawSprite.transform);
		}
		base.StartCoroutine(this.SetHalfBoundsWidthCoroutine());
		if (this.m_buzzSawEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_buzzSawEventInstance);
			this.m_buzzSawEventInstance.setParameterByName("sawActive", 1f, false);
		}
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x0001E735 File Offset: 0x0001C935
	private IEnumerator SetHalfBoundsWidthCoroutine()
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_buzzsawHalfWidth = this.m_hbController.GetCollider(HitboxType.Body).bounds.size.x / 2f;
		this.m_buzzsawHeight = this.m_hbController.GetCollider(HitboxType.Body).bounds.size.y;
		yield break;
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x000E594C File Offset: 0x000E3B4C
	private void FixedUpdate()
	{
		Vector3 localPosition = this.m_pivot.transform.localPosition;
		float num = 0f;
		float num2 = 0f;
		PivotPoint pivotPoint = this.m_pivotPoint;
		if (pivotPoint != PivotPoint.Center)
		{
			if (pivotPoint != PivotPoint.Left)
			{
				if (pivotPoint == PivotPoint.Right)
				{
					num = 0f - this.m_halfWidth * 2f + this.m_buzzsawHalfWidth;
					num2 = 0f - this.m_buzzsawHalfWidth;
				}
			}
			else
			{
				num = 0f + this.m_buzzsawHalfWidth;
				num2 = 0f + this.m_halfWidth * 2f - this.m_buzzsawHalfWidth;
			}
		}
		else
		{
			num = -this.m_halfWidth + this.m_buzzsawHalfWidth;
			num2 = this.m_halfWidth - this.m_buzzsawHalfWidth;
		}
		float num3 = (Time.time - this.m_movementStartTime) / 10f;
		if (this.m_movingRight)
		{
			localPosition.x += 10f * Time.deltaTime;
			if (localPosition.x >= num2)
			{
				localPosition.x = num2;
				this.m_movingRight = !this.m_movingRight;
			}
		}
		else
		{
			localPosition.x -= 10f * Time.deltaTime;
			if (localPosition.x <= num)
			{
				localPosition.x = num;
				this.m_movingRight = !this.m_movingRight;
			}
		}
		this.m_pivot.transform.localPosition = localPosition;
		if (this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance.setParameterByName("sawPosition", this.SawPosition, false);
		}
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x000E5ABC File Offset: 0x000E3CBC
	public override void ResetHazard()
	{
		this.m_movementStartTime = Time.time;
		if (this != null && PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			Vector3 localPosition = this.m_pivot.transform.localPosition;
			if (PlayerManager.GetPlayer() == null)
			{
				return;
			}
			if (PlayerManager.GetPlayer().transform.position.x < base.transform.position.x)
			{
				this.m_movingRight = true;
			}
			else
			{
				this.m_movingRight = false;
			}
			this.m_pivot.transform.localPosition = this.m_startingLocalPos;
			if (this.m_hbController != null)
			{
				if (this.m_retractBuzzsawCoroutine != null)
				{
					base.StopCoroutine(this.m_retractBuzzsawCoroutine);
				}
				if (base.isActiveAndEnabled)
				{
					base.Animator.Play("Buzzsaw_Retracted_Animation");
					this.m_hbController.DisableAllCollisions = true;
					this.ExtractBuzzsaw();
				}
			}
		}
		base.ResetHazard();
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x000E5BB0 File Offset: 0x000E3DB0
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (!otherHBController.IsNativeNull() && !otherHBController.RootGameObject.GetComponent<DownstrikeProjectile_RL>())
		{
			return;
		}
		if (this.m_retractBuzzsawCoroutine != null)
		{
			base.StopCoroutine(this.m_retractBuzzsawCoroutine);
		}
		this.m_retractBuzzsawCoroutine = base.StartCoroutine(this.RetractBuzzsawCoroutine());
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0001E744 File Offset: 0x0001C944
	private IEnumerator RetractBuzzsawCoroutine()
	{
		this.RetractBuzzsaw();
		this.m_retractWaitYield.Reset();
		yield return this.m_retractWaitYield;
		this.ExtractBuzzsaw();
		yield break;
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x000E5C00 File Offset: 0x000E3E00
	private void RetractBuzzsaw()
	{
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_sawTrap_destroy", this.m_buzzSawSprite.transform.position);
		if (this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance.setParameterByName("sawActive", 0f, false);
		}
		this.m_hbController.DisableAllCollisions = true;
		base.Animator.SetBool("Retract", true);
		this.m_resonantParticleEffect.SetActive(false);
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x000E5C78 File Offset: 0x000E3E78
	private void ExtractBuzzsaw()
	{
		if (this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance.setParameterByName("sawActive", 1f, false);
		}
		this.m_hbController.DisableAllCollisions = false;
		base.Animator.SetBool("Retract", false);
		this.m_resonantParticleEffect.SetActive(true);
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x0001E753 File Offset: 0x0001C953
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_buzzSawEventInstance.isValid())
		{
			AudioManager.Stop(this.m_buzzSawEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x0001E774 File Offset: 0x0001C974
	private void OnDestroy()
	{
		if (this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance.release();
		}
	}

	// Token: 0x04002C97 RID: 11415
	private const string POSITION_PARAMETER_NAME = "sawPosition";

	// Token: 0x04002C98 RID: 11416
	private const string ACTIVE_PARAMETER_NAME = "sawActive";

	// Token: 0x04002C99 RID: 11417
	[SerializeField]
	private GameObject m_buzzSawSprite;

	// Token: 0x04002C9A RID: 11418
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x04002C9B RID: 11419
	[SerializeField]
	private GameObject m_resonantParticleEffect;

	// Token: 0x04002C9C RID: 11420
	private bool m_movingRight;

	// Token: 0x04002C9D RID: 11421
	private float m_buzzsawHalfWidth;

	// Token: 0x04002C9E RID: 11422
	private float m_buzzsawHeight;

	// Token: 0x04002C9F RID: 11423
	private IHitboxController m_hbController;

	// Token: 0x04002CA0 RID: 11424
	private WaitRL_Yield m_retractWaitYield;

	// Token: 0x04002CA1 RID: 11425
	private Coroutine m_retractBuzzsawCoroutine;

	// Token: 0x04002CA2 RID: 11426
	private float m_movementStartTime;

	// Token: 0x04002CA3 RID: 11427
	private EventInstance m_buzzSawEventInstance;
}
