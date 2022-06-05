using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class OnDownstrikeEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060026D3 RID: 9939 RVA: 0x0008123B File Offset: 0x0007F43B
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060026D4 RID: 9940 RVA: 0x0008123E File Offset: 0x0007F43E
	public override Vector3 Midpoint
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x0008124B File Offset: 0x0007F44B
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerBounce = new Action<Projectile_RL, GameObject>(this.OnPlayerBounce);
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x00081265 File Offset: 0x0007F465
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.OnSuccessfulDownstrikeRelay.AddListener(this.m_onPlayerBounce, false);
		}
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x0008128A File Offset: 0x0007F48A
	private void OnDisable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.OnSuccessfulDownstrikeRelay.RemoveListener(this.m_onPlayerBounce);
		}
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x000812AE File Offset: 0x0007F4AE
	private void OnPlayerBounce(Projectile_RL downstrikeProj, GameObject rootCollidedObj)
	{
		if (rootCollidedObj == this.m_rootObj)
		{
			this.TriggerBounceEvent(downstrikeProj);
		}
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x000812C8 File Offset: 0x0007F4C8
	private void TriggerBounceEvent(Projectile_RL downstrikeProj)
	{
		Vector3 vector = downstrikeProj.Midpoint;
		if (downstrikeProj.HitboxController.LastCollidedWith)
		{
			vector = downstrikeProj.HitboxController.LastCollidedWith.ClosestPoint(downstrikeProj.Midpoint);
		}
		GameObject gameObject = downstrikeProj.gameObject;
		foreach (EffectTriggerEntry effectTriggerEntry in base.TriggerArray)
		{
			EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(gameObject, effectTriggerEntry.TriggerDirection);
			EffectTriggerDirection effectDirectionFromObject2 = EffectTrigger.GetEffectDirectionFromObject(this.m_rootObj, effectTriggerEntry.TriggerDirection);
			effectTriggerEntry.OffsetType = EffectOffsetType.Midpoint;
			EffectTargetType deriveFacing = effectTriggerEntry.DeriveFacing;
			if (deriveFacing != EffectTargetType.None)
			{
				if (deriveFacing != EffectTargetType.Self)
				{
					if (deriveFacing == EffectTargetType.Other)
					{
						EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, vector, vector, effectDirectionFromObject, null);
					}
				}
				else
				{
					EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, vector, vector, effectDirectionFromObject2, null);
				}
			}
			else
			{
				EffectTrigger.InvokeTrigger(effectTriggerEntry, this.m_rootObj, gameObject, vector, vector, EffectTriggerDirection.Anywhere, null);
			}
		}
	}

	// Token: 0x040020B2 RID: 8370
	private Action<Projectile_RL, GameObject> m_onPlayerBounce;
}
