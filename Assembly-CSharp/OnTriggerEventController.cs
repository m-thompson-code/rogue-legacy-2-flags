using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000804 RID: 2052
public class OnTriggerEventController : MonoBehaviour
{
	// Token: 0x060043FB RID: 17403 RVA: 0x000F09C0 File Offset: 0x000EEBC0
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

	// Token: 0x060043FC RID: 17404 RVA: 0x000F0A19 File Offset: 0x000EEC19
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.GetDoesCollisionTriggerEvent(collision) && this.m_triggerEnterEvent != null)
		{
			this.m_triggerEnterEvent.Invoke();
		}
	}

	// Token: 0x060043FD RID: 17405 RVA: 0x000F0A37 File Offset: 0x000EEC37
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (this.GetDoesCollisionTriggerEvent(collision) && this.m_triggerExitEvent != null)
		{
			this.m_triggerExitEvent.Invoke();
		}
	}

	// Token: 0x04003A17 RID: 14871
	[SerializeField]
	private UnityEvent m_triggerEnterEvent;

	// Token: 0x04003A18 RID: 14872
	[SerializeField]
	private UnityEvent m_triggerExitEvent;

	// Token: 0x04003A19 RID: 14873
	[SerializeField]
	private bool m_triggerByCharacterOnly = true;
}
