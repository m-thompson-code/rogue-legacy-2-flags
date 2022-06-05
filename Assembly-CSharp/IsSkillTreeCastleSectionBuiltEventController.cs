using System;
using System.Collections;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020001BD RID: 445
public class IsSkillTreeCastleSectionBuiltEventController : MonoBehaviour
{
	// Token: 0x06001151 RID: 4433 RVA: 0x00032343 File Offset: 0x00030543
	private void Awake()
	{
		this.m_onWindowOpened = new UnityAction(this.OnWindowOpened);
		this.m_onWindowClosed = new UnityAction(this.OnWindowClosed);
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x0003236C File Offset: 0x0003056C
	private void Start()
	{
		this.m_skillTreeWindow = (WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController);
		this.m_skillTreeWindow.WindowOpenedEvent.RemoveListener(this.m_onWindowOpened);
		this.m_skillTreeWindow.WindowOpenedEvent.AddListener(this.m_onWindowOpened);
		this.m_skillTreeWindow.WindowClosedEvent.RemoveListener(this.m_onWindowClosed);
		this.m_skillTreeWindow.WindowClosedEvent.AddListener(this.m_onWindowClosed);
		this.m_checkCoroutine = base.StartCoroutine(this.Check());
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x000323F4 File Offset: 0x000305F4
	private IEnumerator Check()
	{
		if (this.m_waitForSkillTreeToOpen == null)
		{
			this.m_waitForSkillTreeToOpen = new WaitUntil(() => WindowManager.GetIsWindowOpen(WindowID.SkillTree));
			this.m_waitUntilParameterIsSet = new WaitUntil(() => this.m_skillTreeWindow.CastleAnimator.GetBool(this.m_parameter));
		}
		yield return this.m_waitForSkillTreeToOpen;
		if (!this.m_isBuilt)
		{
			yield return this.m_waitUntilParameterIsSet;
			this.m_isBuilt = true;
		}
		this.OnSectionBuilt();
		this.m_checkCoroutine = null;
		yield break;
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x00032403 File Offset: 0x00030603
	private void OnWindowOpened()
	{
		if (this.m_checkCoroutine == null)
		{
			this.m_checkCoroutine = base.StartCoroutine(this.Check());
		}
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x0003241F File Offset: 0x0003061F
	private void OnSectionBuilt()
	{
		if (this.m_sectionBuiltEvent != null)
		{
			this.m_sectionBuiltEvent.Invoke();
		}
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x00032434 File Offset: 0x00030634
	private void OnWindowClosed()
	{
		base.StopAllCoroutines();
		this.m_checkCoroutine = null;
		if (this.m_isBuilt && this.m_windowClosedEvent != null)
		{
			this.m_windowClosedEvent.Invoke();
		}
	}

	// Token: 0x0400123E RID: 4670
	[SerializeField]
	private string m_parameter;

	// Token: 0x0400123F RID: 4671
	[SerializeField]
	[FormerlySerializedAs("m_windowOpenedEvent")]
	private UnityEvent m_sectionBuiltEvent;

	// Token: 0x04001240 RID: 4672
	[SerializeField]
	private UnityEvent m_windowClosedEvent;

	// Token: 0x04001241 RID: 4673
	private WaitUntil m_waitForSkillTreeToOpen;

	// Token: 0x04001242 RID: 4674
	private WaitUntil m_waitUntilParameterIsSet;

	// Token: 0x04001243 RID: 4675
	private bool m_isBuilt;

	// Token: 0x04001244 RID: 4676
	private Coroutine m_checkCoroutine;

	// Token: 0x04001245 RID: 4677
	private SkillTreeWindowController m_skillTreeWindow;

	// Token: 0x04001246 RID: 4678
	private UnityAction m_onWindowOpened;

	// Token: 0x04001247 RID: 4679
	private UnityAction m_onWindowClosed;
}
