using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class ExhaustPoint_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x0600285D RID: 10333 RVA: 0x00085AF8 File Offset: 0x00083CF8
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

	// Token: 0x0600285E RID: 10334 RVA: 0x00085BA4 File Offset: 0x00083DA4
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

	// Token: 0x0600285F RID: 10335 RVA: 0x00085CA2 File Offset: 0x00083EA2
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

	// Token: 0x06002860 RID: 10336 RVA: 0x00085CAA File Offset: 0x00083EAA
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

	// Token: 0x06002861 RID: 10337 RVA: 0x00085CB4 File Offset: 0x00083EB4
	public override void ResetHazard()
	{
		this.m_inRange = false;
		Vector3 localScale = new Vector3(this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount);
		this.m_exhaustPointWarningSprite.gameObject.transform.localScale = localScale;
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x00085CFF File Offset: 0x00083EFF
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002171 RID: 8561
	[SerializeField]
	private SpriteRenderer m_exhaustPointWarningSprite;

	// Token: 0x04002172 RID: 8562
	[SerializeField]
	private Animator m_exhaustPointWarningAnimator;

	// Token: 0x04002173 RID: 8563
	private float m_radius;

	// Token: 0x04002174 RID: 8564
	private float m_storedWarningScaleAmount;

	// Token: 0x04002175 RID: 8565
	private bool m_inRange;

	// Token: 0x04002176 RID: 8566
	private Coroutine m_increaseExhaustCoroutine;

	// Token: 0x04002177 RID: 8567
	private Coroutine m_decreaseExhaustCoroutine;
}
