using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CCC RID: 3276
public class OnTriggerEventController : MonoBehaviour
{
	// Token: 0x06005D84 RID: 23940 RVA: 0x0015E67C File Offset: 0x0015C87C
	private bool GetDoesCollisionTriggerEvent(Collider2D collision)
	{
		bool result = true;
		IRootObj componentInParent = collision.gameObject.GetComponentInParent<IRootObj>();
		if (componentInParent != null)
		{
			if (this.m_triggerByCharacterOnly && !componentInParent.gameObject.CompareTag("Player") && !componentInParent.gameObject.CompareTag("Enemy"))
			{
				result = false;
			}
		}
		else if (this.m_triggerByCharacterOnly)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x0003373F File Offset: 0x0003193F
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.GetDoesCollisionTriggerEvent(collision) && this.m_triggerEnterEvent != null)
		{
			this.m_triggerEnterEvent.Invoke();
		}
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x0003375D File Offset: 0x0003195D
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (this.GetDoesCollisionTriggerEvent(collision) && this.m_triggerExitEvent != null)
		{
			this.m_triggerExitEvent.Invoke();
		}
	}

	// Token: 0x04004CE3 RID: 19683
	[SerializeField]
	private UnityEvent m_triggerEnterEvent;

	// Token: 0x04004CE4 RID: 19684
	[SerializeField]
	private UnityEvent m_triggerExitEvent;

	// Token: 0x04004CE5 RID: 19685
	[SerializeField]
	private bool m_triggerByCharacterOnly = true;
}
