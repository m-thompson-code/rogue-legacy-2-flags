using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000739 RID: 1849
public class RaycastTurret_Hazard : Hazard, IHasProjectileNameArray, IMultiHazardConsumer
{
	// Token: 0x1700151B RID: 5403
	// (get) Token: 0x0600388A RID: 14474 RVA: 0x0001F0E9 File Offset: 0x0001D2E9
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileName
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x1700151C RID: 5404
	// (get) Token: 0x0600388B RID: 14475 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected virtual Vector2 FireOffset
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700151D RID: 5405
	// (get) Token: 0x0600388C RID: 14476 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool AllTurretsFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700151E RID: 5406
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float InitializationDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700151F RID: 5407
	// (get) Token: 0x0600388E RID: 14478 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float FireDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17001520 RID: 5408
	// (get) Token: 0x0600388F RID: 14479 RVA: 0x0000611B File Offset: 0x0000431B
	protected virtual float DelayBetweenShots
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17001521 RID: 5409
	// (get) Token: 0x06003890 RID: 14480 RVA: 0x00006E96 File Offset: 0x00005096
	protected virtual float DetectedRange
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x17001522 RID: 5410
	// (get) Token: 0x06003891 RID: 14481 RVA: 0x0001F10E File Offset: 0x0001D30E
	// (set) Token: 0x06003892 RID: 14482 RVA: 0x0001F116 File Offset: 0x0001D316
	public Multi_Hazard MultiHazard { get; set; }

	// Token: 0x17001523 RID: 5411
	// (get) Token: 0x06003893 RID: 14483 RVA: 0x000E8998 File Offset: 0x000E6B98
	private bool IsShotWithinRange
	{
		get
		{
			BaseCharacterController playerController = PlayerManager.GetPlayerController();
			float num = base.transform.localEulerAngles.z;
			num = (float)((int)CDGHelper.WrapAngleDegrees(num, true));
			Vector2 vector = playerController.Midpoint;
			vector = CDGHelper.RotatedPoint(vector, base.transform.position, 90f - num);
			Rect rect = new Rect(base.transform.position.x - 1f, base.transform.position.y, 2f, this.DetectedRange);
			return rect.Contains(vector);
		}
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x0001F11F File Offset: 0x0001D31F
	protected override void Awake()
	{
		base.Awake();
		this.m_triggerShotMask = 256;
		this.m_initialDelay = new WaitForSeconds(this.FireDelay);
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0001F143 File Offset: 0x0001D343
	private void OnEnable()
	{
		this.m_onEnterTime = Time.time;
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x0001F150 File Offset: 0x0001D350
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_shotDelayTimer = 0f;
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x000E8A30 File Offset: 0x000E6C30
	private void FixedUpdate()
	{
		if (Time.time < this.m_onEnterTime + this.InitializationDelay)
		{
			return;
		}
		if (this.m_shotDelayTimer <= 0f)
		{
			if (PlayerManager.IsInstantiated)
			{
				if (this.m_needsReloading)
				{
					base.Animator.SetTrigger("Reload");
					this.m_needsReloading = false;
				}
				PlayerController playerController = PlayerManager.GetPlayerController();
				float num = 0.5f;
				float z = base.transform.localEulerAngles.z;
				Vector3 v = base.transform.position;
				v.y += num;
				v = CDGHelper.RotatedPoint(v, base.transform.position, z - 90f);
				v.z = base.transform.position.z;
				if (this.IsShotWithinRange)
				{
					Vector2 direction = CDGHelper.VectorBetweenPts(v, playerController.Midpoint);
					float magnitude = direction.magnitude;
					direction.Normalize();
					if (!Physics2D.Raycast(v, direction, magnitude, this.m_triggerShotMask))
					{
						if (this.AllTurretsFire)
						{
							if (!this.MultiHazard || this.MultiHazard.Hazards == null)
							{
								return;
							}
							using (List<IHazard>.Enumerator enumerator = this.MultiHazard.Hazards.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									IHazard hazard = enumerator.Current;
									(hazard as RaycastTurret_Hazard).FireRaycastArrow();
								}
								return;
							}
						}
						this.FireRaycastArrow();
						return;
					}
				}
			}
		}
		else
		{
			this.m_shotDelayTimer -= Time.deltaTime;
		}
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x0001F163 File Offset: 0x0001D363
	public void FireRaycastArrow()
	{
		if (this.m_initialDelayCoroutine == null)
		{
			this.m_initialDelayCoroutine = base.StartCoroutine(this.FireAfterInitialDelay());
		}
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x0001F17F File Offset: 0x0001D37F
	private IEnumerator FireAfterInitialDelay()
	{
		base.Animator.SetTrigger("Tell");
		yield return this.m_initialDelay;
		base.Animator.SetTrigger("Fire");
		this.m_needsReloading = true;
		ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, this.FireOffset, false, base.transform.localEulerAngles.z, 1f, false, true, true, true);
		this.m_shotDelayTimer = this.DelayBetweenShots;
		this.m_initialDelayCoroutine = null;
		yield break;
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x0001F18E File Offset: 0x0001D38E
	public override void ResetHazard()
	{
		this.m_initialDelayCoroutine = null;
		this.m_needsReloading = false;
	}

	// Token: 0x04002D5F RID: 11615
	[SerializeField]
	private string m_projectileName;

	// Token: 0x04002D60 RID: 11616
	private const int SHOT_TRIGGER_WIDTH = 2;

	// Token: 0x04002D61 RID: 11617
	private float m_shotDelayTimer;

	// Token: 0x04002D62 RID: 11618
	private int m_triggerShotMask;

	// Token: 0x04002D63 RID: 11619
	private Coroutine m_initialDelayCoroutine;

	// Token: 0x04002D64 RID: 11620
	private WaitForSeconds m_initialDelay;

	// Token: 0x04002D65 RID: 11621
	private float m_onEnterTime;

	// Token: 0x04002D66 RID: 11622
	private bool m_needsReloading;

	// Token: 0x04002D67 RID: 11623
	[NonSerialized]
	private string[] m_projectileNameArray;
}
