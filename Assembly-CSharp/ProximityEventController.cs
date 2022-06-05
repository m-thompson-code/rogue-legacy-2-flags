using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001BE RID: 446
public class ProximityEventController : MonoBehaviour
{
	// Token: 0x06001159 RID: 4441 RVA: 0x0003247E File Offset: 0x0003067E
	private void Awake()
	{
		if (this.m_proximityRadius < 0f)
		{
			throw new ArgumentOutOfRangeException("m_proximityRadius", "Value must be greater than 0");
		}
		this.m_proximitySquareMagnitude = this.m_proximityRadius * this.m_proximityRadius;
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x000324B0 File Offset: 0x000306B0
	private void Update()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		this.CheckProximity();
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x000324C0 File Offset: 0x000306C0
	private void OnDisable()
	{
		if (this.m_hasEventBeenFired)
		{
			this.m_hasEventBeenFired = false;
			if (this.m_playerExitedRangeEvent != null)
			{
				this.m_playerExitedRangeEvent.Invoke();
			}
		}
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x000324E4 File Offset: 0x000306E4
	protected virtual void CheckProximity()
	{
		Vector2 a = PlayerManager.GetPlayerController().transform.position;
		Vector2 b = base.transform.position;
		Vector2 vector = a - b;
		if (this.m_hasEventBeenFired)
		{
			if (vector.sqrMagnitude > this.m_proximitySquareMagnitude)
			{
				this.m_hasEventBeenFired = false;
				if (this.m_playerExitedRangeEvent != null)
				{
					this.m_playerExitedRangeEvent.Invoke();
					return;
				}
			}
		}
		else if (vector.sqrMagnitude <= this.m_proximitySquareMagnitude)
		{
			this.m_hasEventBeenFired = true;
			if (this.m_playerEnteredRangeEvent != null)
			{
				this.m_playerEnteredRangeEvent.Invoke();
			}
		}
	}

	// Token: 0x04001248 RID: 4680
	[SerializeField]
	private float m_proximityRadius = 10f;

	// Token: 0x04001249 RID: 4681
	[SerializeField]
	private UnityEvent m_playerEnteredRangeEvent;

	// Token: 0x0400124A RID: 4682
	[SerializeField]
	private UnityEvent m_playerExitedRangeEvent;

	// Token: 0x0400124B RID: 4683
	private bool m_hasEventBeenFired;

	// Token: 0x0400124C RID: 4684
	private float m_proximitySquareMagnitude;
}
