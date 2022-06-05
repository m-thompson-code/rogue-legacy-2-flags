using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200032F RID: 815
public class ProximityEventController : MonoBehaviour
{
	// Token: 0x060019AB RID: 6571 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
	private void Awake()
	{
		if (this.m_proximityRadius < 0f)
		{
			throw new ArgumentOutOfRangeException("m_proximityRadius", "Value must be greater than 0");
		}
		this.m_proximitySquareMagnitude = this.m_proximityRadius * this.m_proximityRadius;
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x0000CFFA File Offset: 0x0000B1FA
	private void Update()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		this.CheckProximity();
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x0000D00A File Offset: 0x0000B20A
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

	// Token: 0x060019AE RID: 6574 RVA: 0x00090734 File Offset: 0x0008E934
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

	// Token: 0x04001856 RID: 6230
	[SerializeField]
	private float m_proximityRadius = 10f;

	// Token: 0x04001857 RID: 6231
	[SerializeField]
	private UnityEvent m_playerEnteredRangeEvent;

	// Token: 0x04001858 RID: 6232
	[SerializeField]
	private UnityEvent m_playerExitedRangeEvent;

	// Token: 0x04001859 RID: 6233
	private bool m_hasEventBeenFired;

	// Token: 0x0400185A RID: 6234
	private float m_proximitySquareMagnitude;
}
