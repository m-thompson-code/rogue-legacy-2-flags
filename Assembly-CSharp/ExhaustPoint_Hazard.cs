using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200071E RID: 1822
public class ExhaustPoint_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x060037B1 RID: 14257 RVA: 0x000E6A28 File Offset: 0x000E4C28
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		if (pointHazardArgs != null)
		{
			this.m_radius = pointHazardArgs.Radius;
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
		}
		this.m_exhaustPointWarningSprite.gameObject.transform.localScale = Vector3.one;
		float num = 4f;
		float num2 = this.m_radius * 2f * 1.75f;
		this.m_storedWarningScaleAmount = num2 / num;
		Vector3 localScale = new Vector3(this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount);
		this.m_exhaustPointWarningSprite.gameObject.transform.localScale = localScale;
		this.ResetHazard();
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x000E6AD4 File Offset: 0x000E4CD4
	private void FixedUpdate()
	{
		if (PlayerManager.IsInstantiated)
		{
			if (CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f)
			{
				if (!this.m_inRange)
				{
					this.m_exhaustPointWarningAnimator.SetBool("Aware", true);
					if (this.m_increaseExhaustCoroutine != null)
					{
						base.StopCoroutine(this.m_increaseExhaustCoroutine);
					}
					if (this.m_decreaseExhaustCoroutine != null)
					{
						base.StopCoroutine(this.m_decreaseExhaustCoroutine);
					}
					this.m_increaseExhaustCoroutine = base.StartCoroutine(this.IncreaseExhaustCoroutine());
				}
				this.m_inRange = true;
				return;
			}
			if (this.m_inRange)
			{
				this.m_exhaustPointWarningAnimator.SetBool("Aware", false);
				if (this.m_increaseExhaustCoroutine != null)
				{
					base.StopCoroutine(this.m_increaseExhaustCoroutine);
				}
				if (this.m_decreaseExhaustCoroutine != null)
				{
					base.StopCoroutine(this.m_decreaseExhaustCoroutine);
				}
				this.m_decreaseExhaustCoroutine = base.StartCoroutine(this.DecreaseExhaustCoroutine());
				this.m_inRange = false;
			}
		}
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0001E988 File Offset: 0x0001CB88
	private IEnumerator IncreaseExhaustCoroutine()
	{
		float intervalDuration = 0.01f;
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (playerController.CurrentExhaust < 20)
		{
			PlayerController playerController2 = playerController;
			int currentExhaust = playerController2.CurrentExhaust;
			playerController2.CurrentExhaust = currentExhaust + 1;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExhaustChange, null, null);
			float delay = Time.time + intervalDuration;
			while (Time.time < delay)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0001E990 File Offset: 0x0001CB90
	private IEnumerator DecreaseExhaustCoroutine()
	{
		float intervalDuration = 0.01f;
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (playerController.CurrentExhaust > 0)
		{
			PlayerController playerController2 = playerController;
			int currentExhaust = playerController2.CurrentExhaust;
			playerController2.CurrentExhaust = currentExhaust - 1;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExhaustChange, null, null);
			float delay = Time.time + intervalDuration;
			while (Time.time < delay)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x000E6BD4 File Offset: 0x000E4DD4
	public override void ResetHazard()
	{
		this.m_inRange = false;
		Vector3 localScale = new Vector3(this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount);
		this.m_exhaustPointWarningSprite.gameObject.transform.localScale = localScale;
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	private SpriteRenderer m_exhaustPointWarningSprite;

	// Token: 0x04002CD2 RID: 11474
	[SerializeField]
	private Animator m_exhaustPointWarningAnimator;

	// Token: 0x04002CD3 RID: 11475
	private float m_radius;

	// Token: 0x04002CD4 RID: 11476
	private float m_storedWarningScaleAmount;

	// Token: 0x04002CD5 RID: 11477
	private bool m_inRange;

	// Token: 0x04002CD6 RID: 11478
	private Coroutine m_increaseExhaustCoroutine;

	// Token: 0x04002CD7 RID: 11479
	private Coroutine m_decreaseExhaustCoroutine;
}
