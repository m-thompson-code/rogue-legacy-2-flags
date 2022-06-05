using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class BouncySpike_AIScript : BaseAIScript
{
	// Token: 0x06000290 RID: 656 RVA: 0x00013086 File Offset: 0x00011286
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[0];
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00013094 File Offset: 0x00011294
	private void PerformAwayFromPlayerCheck()
	{
		float num = (float)this.m_startingAngle;
		bool flag = false;
		float num2 = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, PlayerManager.GetPlayerController().Midpoint);
		if (num2 < 90f && num > 270f)
		{
			num = CDGHelper.WrapAngleDegrees(num, true);
		}
		else if (num2 > 270f && num < 90f)
		{
			num2 = -CDGHelper.WrapAngleDegrees(num2, true);
		}
		float num3 = num2 - num;
		if ((num3 < 45f && num3 > 0f) || (num3 > -45f && num3 < 0f))
		{
			flag = true;
		}
		int num4 = this.m_startingAngle;
		if (flag)
		{
			if (num4 > 0)
			{
				num4 -= 180;
			}
			else
			{
				num4 += 180;
			}
		}
		base.EnemyController.Orientation = 0.017453292f * (float)num4;
		base.SetVelocity(base.EnemyController.HeadingX * base.EnemyController.ActualSpeed, base.EnemyController.HeadingY * base.EnemyController.ActualSpeed, false);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00013198 File Offset: 0x00011398
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		int num = UnityEngine.Random.Range(0, 4);
		this.m_startingAngle = 45 + num * 90;
		base.EnemyController.OnResetPositionRelay.AddListener(new Action<object, EventArgs>(this.OnResetPosition), false);
		base.EnemyController.OnPositionedForSummoningRelay.AddListener(new Action<object, EventArgs>(this.OnPositionedForSummoning), false);
		if (PlayerManager.IsInstantiated)
		{
			this.PerformAwayFromPlayerCheck();
		}
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0001320A File Offset: 0x0001140A
	private void OnResetPosition(object sender, EventArgs args)
	{
		if (base.IsInitialized && PlayerManager.IsInstantiated)
		{
			this.PerformAwayFromPlayerCheck();
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00013221 File Offset: 0x00011421
	private void OnPositionedForSummoning(object sender, EventArgs args)
	{
		if (base.IsInitialized && PlayerManager.IsInstantiated)
		{
			this.PerformAwayFromPlayerCheck();
		}
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00013238 File Offset: 0x00011438
	public override IEnumerator Idle()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		for (;;)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00013248 File Offset: 0x00011448
	private void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.IsPaused)
		{
			if (base.EnemyController.Velocity != Vector2.zero)
			{
				base.EnemyController.SetVelocity(0f, 0f, false);
			}
			return;
		}
		float num = base.EnemyController.ActualSpeed * 20f;
		Vector3 localEulerAngles = base.EnemyController.Visuals.transform.localEulerAngles;
		if (this.m_startingAngle < 90 || this.m_startingAngle > 270)
		{
			localEulerAngles.z += num * Time.deltaTime;
		}
		else
		{
			localEulerAngles.z -= num * Time.deltaTime;
		}
		base.EnemyController.Visuals.transform.localEulerAngles = localEulerAngles;
		if (base.EnemyController.Velocity == Vector2.zero)
		{
			base.SetVelocity(base.EnemyController.HeadingX * base.EnemyController.ActualSpeed, base.EnemyController.HeadingY * base.EnemyController.ActualSpeed, false);
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0001335C File Offset: 0x0001155C
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.OnResetPositionRelay.RemoveListener(new Action<object, EventArgs>(this.OnResetPosition));
			base.EnemyController.OnPositionedForSummoningRelay.RemoveListener(new Action<object, EventArgs>(this.OnPositionedForSummoning));
		}
	}

	// Token: 0x0400068E RID: 1678
	private int m_startingAngle;
}
