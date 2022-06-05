using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class ChakramProjectileLogic : BaseProjectileLogic, ITerrainOnEnterHitResponse, IHitResponse, IAudioEventEmitter
{
	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06002B32 RID: 11058 RVA: 0x00092542 File Offset: 0x00090742
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x0009254C File Offset: 0x0009074C
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

	// Token: 0x06002B34 RID: 11060 RVA: 0x00092628 File Offset: 0x00090828
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

	// Token: 0x06002B35 RID: 11061 RVA: 0x00092884 File Offset: 0x00090A84
	private void ChangeHitboxOnTerrainChange()
	{
		IHitboxController hitboxController = base.SourceProjectile.HitboxController;
		CollisionType collisionType = hitboxController.TerrainCollidesWithType;
		collisionType |= CollisionType.Player;
		collisionType |= CollisionType.Player_Dodging;
		hitboxController.ChangeCanCollideWith(HitboxType.Terrain, collisionType);
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000928B8 File Offset: 0x00090AB8
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

	// Token: 0x06002B37 RID: 11063 RVA: 0x0009290A File Offset: 0x00090B0A
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		base.SourceProjectile.FlagForDestruction(null);
	}

	// Token: 0x0400231F RID: 8991
	[SerializeField]
	private float m_returnDelay = 0.15f;

	// Token: 0x04002320 RID: 8992
	[SerializeField]
	private float m_returnSpeed = 15f;

	// Token: 0x04002321 RID: 8993
	[SerializeField]
	private float m_pauseAtApexDelay;

	// Token: 0x04002322 RID: 8994
	[SerializeField]
	private bool m_returnToPlayer;

	// Token: 0x04002323 RID: 8995
	[SerializeField]
	private bool m_grabbleByPlayer;

	// Token: 0x04002324 RID: 8996
	[SerializeField]
	private string m_changeDirectionAudioEvent;

	// Token: 0x04002325 RID: 8997
	[SerializeField]
	private bool m_canCrit = true;

	// Token: 0x04002326 RID: 8998
	[SerializeField]
	private bool m_flipOnReturn;

	// Token: 0x04002327 RID: 8999
	[SerializeField]
	private GameObject m_critEffectGO;

	// Token: 0x04002328 RID: 9000
	private float m_fireStartTime;

	// Token: 0x04002329 RID: 9001
	private float m_maxSpeed;

	// Token: 0x0400232A RID: 9002
	private float m_startingVelocity;

	// Token: 0x0400232B RID: 9003
	private float m_pauseAtApexCounter;

	// Token: 0x0400232C RID: 9004
	private float m_direction;

	// Token: 0x0400232D RID: 9005
	private bool m_isReturning;

	// Token: 0x0400232E RID: 9006
	private bool m_movingAlongYAxis;
}
