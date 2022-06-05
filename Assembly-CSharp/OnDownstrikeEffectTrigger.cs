using System;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
public class OnDownstrikeEffectTrigger : BaseEffectTrigger
{
	// Token: 0x17001441 RID: 5185
	// (get) Token: 0x06003588 RID: 13704 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool RequiresCollider
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001442 RID: 5186
	// (get) Token: 0x06003589 RID: 13705 RVA: 0x0001D5F6 File Offset: 0x0001B7F6
	public override Vector3 Midpoint
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x0001D603 File Offset: 0x0001B803
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerBounce = new Action<Projectile_RL, GameObject>(this.OnPlayerBounce);
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x0001D61D File Offset: 0x0001B81D
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.OnSuccessfulDownstrikeRelay.AddListener(this.m_onPlayerBounce, false);
		}
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x0001D642 File Offset: 0x0001B842
	private void OnDisable()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.OnSuccessfulDownstrikeRelay.RemoveListener(this.m_onPlayerBounce);
		}
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x0001D666 File Offset: 0x0001B866
	private void OnPlayerBounce(Projectile_RL downstrikeProj, GameObject rootCollidedObj)
	{
		if (rootCollidedObj == this.m_rootObj)
		{
			this.TriggerBounceEvent(downstrikeProj);
		}
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x000E17F0 File Offset: 0x000DF9F0
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

	// Token: 0x04002B95 RID: 11157
	private Action<Projectile_RL, GameObject> m_onPlayerBounce;
}
