using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class ChakramProjectileLogic : BaseProjectileLogic, ITerrainOnEnterHitResponse, IHitResponse, IAudioEventEmitter
{
	// Token: 0x170015E2 RID: 5602
	// (get) Token: 0x06003B81 RID: 15233 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x000F3F90 File Offset: 0x000F2190
	private void OnEnable()
	{
		this.m_maxSpeed = base.SourceProjectile.ProjectileData.Speed;
		this.m_fireStartTime = Time.time;
		base.SourceProjectile.UpdateMovement();
		if (base.SourceProjectile.Velocity.x != 0f)
		{
			this.m_startingVelocity = base.SourceProjectile.Velocity.x;
			this.m_movingAlongYAxis = false;
		}
		else
		{
			this.m_startingVelocity = base.SourceProjectile.Velocity.y;
			this.m_movingAlongYAxis = true;
		}
		this.m_direction = Mathf.Sign(this.m_startingVelocity);
		this.m_pauseAtApexCounter = this.m_pauseAtApexDelay;
		this.m_isReturning = false;
		base.SourceProjectile.TurnSpeed = 0f;
		if (this.m_critEffectGO)
		{
			this.m_critEffectGO.SetActive(false);
		}
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x000F406C File Offset: 0x000F226C
	private void LateUpdate()
	{
		if (Time.time > this.m_fireStartTime + this.m_returnDelay)
		{
			if (!this.m_returnToPlayer)
			{
				base.SourceProjectile.Speed -= this.m_returnSpeed * Time.deltaTime;
				if (base.SourceProjectile.Speed < -this.m_maxSpeed)
				{
					base.SourceProjectile.Speed = -this.m_maxSpeed;
				}
			}
			else if (!this.m_isReturning)
			{
				base.SourceProjectile.Speed -= this.m_returnSpeed * Time.deltaTime;
				if (base.SourceProjectile.Speed < -this.m_maxSpeed)
				{
					base.SourceProjectile.Speed = -this.m_maxSpeed;
				}
			}
			else
			{
				base.SourceProjectile.Speed += this.m_returnSpeed * Time.deltaTime;
				if (base.SourceProjectile.Speed > this.m_maxSpeed)
				{
					base.SourceProjectile.Speed = this.m_maxSpeed;
				}
			}
			float num = this.m_movingAlongYAxis ? Mathf.Sign(base.SourceProjectile.Velocity.y) : Mathf.Sign(base.SourceProjectile.Velocity.x);
			if (this.m_direction != num && !this.m_isReturning)
			{
				this.m_isReturning = true;
				this.m_direction = num;
				if (this.m_flipOnReturn)
				{
					base.SourceProjectile.Flip();
				}
				if (this.m_returnToPlayer)
				{
					PlayerController playerController = PlayerManager.GetPlayerController();
					base.SourceProjectile.Heading = playerController.Midpoint - base.SourceProjectile.Midpoint;
					base.SourceProjectile.TurnSpeed = 200f;
				}
				if (this.m_grabbleByPlayer)
				{
					this.ChangeHitboxOnTerrainChange();
				}
				if (this.m_canCrit)
				{
					if (base.SourceProjectile.ActualCritChance < 100f)
					{
						base.SourceProjectile.ActualCritChance += 100f;
					}
					if (this.m_critEffectGO)
					{
						this.m_critEffectGO.SetActive(true);
					}
				}
				AudioManager.PlayOneShotAttached(this, this.m_changeDirectionAudioEvent, base.gameObject);
			}
			if (this.m_pauseAtApexCounter > 0f && this.m_isReturning)
			{
				this.m_pauseAtApexCounter -= Time.deltaTime;
				base.SourceProjectile.Speed = 0f;
			}
		}
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x000F42C8 File Offset: 0x000F24C8
	private void ChangeHitboxOnTerrainChange()
	{
		IHitboxController hitboxController = base.SourceProjectile.HitboxController;
		CollisionType collisionType = hitboxController.TerrainCollidesWithType;
		collisionType |= CollisionType.Player;
		collisionType |= CollisionType.Player_Dodging;
		hitboxController.ChangeCanCollideWith(HitboxType.Terrain, collisionType);
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x000F42FC File Offset: 0x000F24FC
	private void OnDisable()
	{
		if (this.m_grabbleByPlayer)
		{
			IHitboxController hitboxController = base.SourceProjectile.HitboxController;
			CollisionType collisionType = hitboxController.TerrainCollidesWithType;
			collisionType = (collisionType & ~CollisionType.Player & ~CollisionType.Player_Dodging);
			hitboxController.ChangeCanCollideWith(HitboxType.Terrain, collisionType);
		}
		if (this.m_critEffectGO)
		{
			this.m_critEffectGO.SetActive(false);
		}
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x00020B09 File Offset: 0x0001ED09
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		base.SourceProjectile.FlagForDestruction(null);
	}

	// Token: 0x04002F36 RID: 12086
	[SerializeField]
	private float m_returnDelay = 0.15f;

	// Token: 0x04002F37 RID: 12087
	[SerializeField]
	private float m_returnSpeed = 15f;

	// Token: 0x04002F38 RID: 12088
	[SerializeField]
	private float m_pauseAtApexDelay;

	// Token: 0x04002F39 RID: 12089
	[SerializeField]
	private bool m_returnToPlayer;

	// Token: 0x04002F3A RID: 12090
	[SerializeField]
	private bool m_grabbleByPlayer;

	// Token: 0x04002F3B RID: 12091
	[SerializeField]
	private string m_changeDirectionAudioEvent;

	// Token: 0x04002F3C RID: 12092
	[SerializeField]
	private bool m_canCrit = true;

	// Token: 0x04002F3D RID: 12093
	[SerializeField]
	private bool m_flipOnReturn;

	// Token: 0x04002F3E RID: 12094
	[SerializeField]
	private GameObject m_critEffectGO;

	// Token: 0x04002F3F RID: 12095
	private float m_fireStartTime;

	// Token: 0x04002F40 RID: 12096
	private float m_maxSpeed;

	// Token: 0x04002F41 RID: 12097
	private float m_startingVelocity;

	// Token: 0x04002F42 RID: 12098
	private float m_pauseAtApexCounter;

	// Token: 0x04002F43 RID: 12099
	private float m_direction;

	// Token: 0x04002F44 RID: 12100
	private bool m_isReturning;

	// Token: 0x04002F45 RID: 12101
	private bool m_movingAlongYAxis;
}
