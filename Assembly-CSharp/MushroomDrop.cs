using System;

// Token: 0x02000476 RID: 1142
public class MushroomDrop : HealthDrop
{
	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0008A6AD File Offset: 0x000888AD
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.MushroomDrop;
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x0008A6B1 File Offset: 0x000888B1
	protected override void GainHealth(float hpGain = 0f)
	{
		base.GainHealth(hpGain);
		if (!PlayerManager.GetPlayerController().IsMushroomBig)
		{
			PlayerManager.GetPlayerController().SetMushroomBig(true, true);
		}
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x0008A6D2 File Offset: 0x000888D2
	public override void OnSpawnCollectCollisionCheck()
	{
		base.OnSpawnCollectCollisionCheck();
		this.m_moveRight = (base.CorgiController.Velocity.x > 0f);
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x0008A6F8 File Offset: 0x000888F8
	protected override void Update()
	{
		base.Update();
		if (base.CorgiController && base.CorgiController.State.IsGrounded && !base.Magnetized)
		{
			if ((base.CorgiController.State.IsCollidingLeft && !this.m_moveRight) || (base.CorgiController.State.IsCollidingRight && this.m_moveRight))
			{
				this.m_moveRight = !this.m_moveRight;
			}
			if (this.m_moveRight && base.CorgiController.Velocity.x != 5f)
			{
				base.CorgiController.SetHorizontalForce(5f);
				return;
			}
			if (!this.m_moveRight && base.CorgiController.Velocity.x != -5f)
			{
				base.CorgiController.SetHorizontalForce(-5f);
			}
		}
	}

	// Token: 0x0400224C RID: 8780
	private const float MOVEMENT_SPEED = 5f;

	// Token: 0x0400224D RID: 8781
	private bool m_moveRight;
}
