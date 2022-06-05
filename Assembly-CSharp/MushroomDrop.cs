using System;

// Token: 0x0200076C RID: 1900
public class MushroomDrop : HealthDrop
{
	// Token: 0x1700157A RID: 5498
	// (get) Token: 0x060039E3 RID: 14819 RVA: 0x0001FCF3 File Offset: 0x0001DEF3
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.MushroomDrop;
		}
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x0001FCF7 File Offset: 0x0001DEF7
	protected override void GainHealth(float hpGain = 0f)
	{
		base.GainHealth(hpGain);
		if (!PlayerManager.GetPlayerController().IsMushroomBig)
		{
			PlayerManager.GetPlayerController().SetMushroomBig(true, true);
		}
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x0001FD18 File Offset: 0x0001DF18
	public override void OnSpawnCollectCollisionCheck()
	{
		base.OnSpawnCollectCollisionCheck();
		this.m_moveRight = (base.CorgiController.Velocity.x > 0f);
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x000EC37C File Offset: 0x000EA57C
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

	// Token: 0x04002E28 RID: 11816
	private const float MOVEMENT_SPEED = 5f;

	// Token: 0x04002E29 RID: 11817
	private bool m_moveRight;
}
