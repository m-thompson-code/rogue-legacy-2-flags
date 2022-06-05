using System;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class Lute_Ability : AimedAbilityFast_RL, IAttack, IAbility
{
	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06001825 RID: 6181 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06001826 RID: 6182 RVA: 0x0000C00F File Offset: 0x0000A20F
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06001827 RID: 6183 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06001828 RID: 6184 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x06001829 RID: 6185 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x0600182A RID: 6186 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x0600182B RID: 6187 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x0600182C RID: 6188 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x0600182D RID: 6189 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x0600182E RID: 6190 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x0008D778 File Offset: 0x0008B978
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		if (this.m_endAimIndicatorPivot)
		{
			this.m_endAimIndicatorPivot.transform.GetChild(0).transform.localPosition = new Vector3(this.m_aimLineLength + 0.5f, 0f, 0f);
		}
		base.Initialize(abilityController, castAbilityType);
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x0008D7D0 File Offset: 0x0008B9D0
	protected override void FireProjectile()
	{
		Vector2 vector = CDGHelper.AngleToVector(-this.m_aimAngle);
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			vector.x = -vector.x;
		}
		float aimAngle = this.m_aimAngle;
		CDGHelper.RotatedPoint(new Vector2(this.ProjectileOffset.x, 0f), Vector2.zero, aimAngle).y += this.ProjectileOffset.y;
		base.FireProjectile();
		if (this.m_firedProjectile.transform.localScale.x < 0f)
		{
			this.m_firedProjectile.Flip();
		}
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0000C283 File Offset: 0x0000A483
	protected override void Update()
	{
		base.Update();
		if (this.m_isAiming && this.m_endAimIndicatorPivot)
		{
			this.m_endAimIndicatorPivot.transform.SetLocalEulerZ(this.m_unmoddedAngle);
		}
	}

	// Token: 0x04001784 RID: 6020
	[SerializeField]
	private GameObject m_endAimIndicatorPivot;
}
