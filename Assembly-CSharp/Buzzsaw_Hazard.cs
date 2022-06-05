using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class Buzzsaw_Hazard : SingleLine_Multi_Hazard, IBodyOnEnterHitResponse, IHitResponse
{
	// Token: 0x17000FCB RID: 4043
	// (get) Token: 0x0600281E RID: 10270 RVA: 0x00084E00 File Offset: 0x00083000
	public float SawPosition
	{
		get
		{
			float num = this.m_pivot.transform.localPosition.x - -1f * this.m_halfWidth;
			float num2 = 2f * this.m_halfWidth;
			return num / num2;
		}
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x00084E3E File Offset: 0x0008303E
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
		this.m_retractWaitYield = new WaitRL_Yield(2f, false);
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x00084E64 File Offset: 0x00083064
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

	// Token: 0x06002821 RID: 10273 RVA: 0x00084F21 File Offset: 0x00083121
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

	// Token: 0x06002822 RID: 10274 RVA: 0x00084F30 File Offset: 0x00083130
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

	// Token: 0x06002823 RID: 10275 RVA: 0x000850A0 File Offset: 0x000832A0
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

	// Token: 0x06002824 RID: 10276 RVA: 0x00085194 File Offset: 0x00083394
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

	// Token: 0x06002825 RID: 10277 RVA: 0x000851E2 File Offset: 0x000833E2
	private IEnumerator RetractBuzzsawCoroutine()
	{
		this.RetractBuzzsaw();
		this.m_retractWaitYield.Reset();
		yield return this.m_retractWaitYield;
		this.ExtractBuzzsaw();
		yield break;
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x000851F4 File Offset: 0x000833F4
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

	// Token: 0x06002827 RID: 10279 RVA: 0x0008526C File Offset: 0x0008346C
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

	// Token: 0x06002828 RID: 10280 RVA: 0x000852C6 File Offset: 0x000834C6
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_buzzSawEventInstance.isValid())
		{
			AudioManager.Stop(this.m_buzzSawEventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x000852E7 File Offset: 0x000834E7
	private void OnDestroy()
	{
		if (this.m_buzzSawEventInstance.isValid())
		{
			this.m_buzzSawEventInstance.release();
		}
	}

	// Token: 0x0400214F RID: 8527
	private const string POSITION_PARAMETER_NAME = "sawPosition";

	// Token: 0x04002150 RID: 8528
	private const string ACTIVE_PARAMETER_NAME = "sawActive";

	// Token: 0x04002151 RID: 8529
	[SerializeField]
	private GameObject m_buzzSawSprite;

	// Token: 0x04002152 RID: 8530
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x04002153 RID: 8531
	[SerializeField]
	private GameObject m_resonantParticleEffect;

	// Token: 0x04002154 RID: 8532
	private bool m_movingRight;

	// Token: 0x04002155 RID: 8533
	private float m_buzzsawHalfWidth;

	// Token: 0x04002156 RID: 8534
	private float m_buzzsawHeight;

	// Token: 0x04002157 RID: 8535
	private IHitboxController m_hbController;

	// Token: 0x04002158 RID: 8536
	private WaitRL_Yield m_retractWaitYield;

	// Token: 0x04002159 RID: 8537
	private Coroutine m_retractBuzzsawCoroutine;

	// Token: 0x0400215A RID: 8538
	private float m_movementStartTime;

	// Token: 0x0400215B RID: 8539
	private EventInstance m_buzzSawEventInstance;
}
