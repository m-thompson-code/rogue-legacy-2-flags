using System;
using System.Collections;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x0200032C RID: 812
public class IsSkillTreeCastleSectionBuiltEventController : MonoBehaviour
{
	// Token: 0x0600199A RID: 6554 RVA: 0x0000CEF5 File Offset: 0x0000B0F5
	private void Awake()
	{
		this.m_onWindowOpened = new UnityAction(this.OnWindowOpened);
		this.m_onWindowClosed = new UnityAction(this.OnWindowClosed);
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000905D8 File Offset: 0x0008E7D8
	private void Start()
	{
		this.m_skillTreeWindow = (WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController);
		this.m_skillTreeWindow.WindowOpenedEvent.RemoveListener(this.m_onWindowOpened);
		this.m_skillTreeWindow.WindowOpenedEvent.AddListener(this.m_onWindowOpened);
		this.m_skillTreeWindow.WindowClosedEvent.RemoveListener(this.m_onWindowClosed);
		this.m_skillTreeWindow.WindowClosedEvent.AddListener(this.m_onWindowClosed);
		this.m_checkCoroutine = base.StartCoroutine(this.Check());
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x0000CF1B File Offset: 0x0000B11B
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

	// Token: 0x0600199D RID: 6557 RVA: 0x0000CF2A File Offset: 0x0000B12A
	private void OnWindowOpened()
	{
		if (this.m_checkCoroutine == null)
		{
			this.m_checkCoroutine = base.StartCoroutine(this.Check());
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x0000CF46 File Offset: 0x0000B146
	private void OnSectionBuilt()
	{
		if (this.m_sectionBuiltEvent != null)
		{
			this.m_sectionBuiltEvent.Invoke();
		}
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x0000CF5B File Offset: 0x0000B15B
	private void OnWindowClosed()
	{
		base.StopAllCoroutines();
		this.m_checkCoroutine = null;
		if (this.m_isBuilt && this.m_windowClosedEvent != null)
		{
			this.m_windowClosedEvent.Invoke();
		}
	}

	// Token: 0x04001847 RID: 6215
	[SerializeField]
	private string m_parameter;

	// Token: 0x04001848 RID: 6216
	[SerializeField]
	[FormerlySerializedAs("m_windowOpenedEvent")]
	private UnityEvent m_sectionBuiltEvent;

	// Token: 0x04001849 RID: 6217
	[SerializeField]
	private UnityEvent m_windowClosedEvent;

	// Token: 0x0400184A RID: 6218
	private WaitUntil m_waitForSkillTreeToOpen;

	// Token: 0x0400184B RID: 6219
	private WaitUntil m_waitUntilParameterIsSet;

	// Token: 0x0400184C RID: 6220
	private bool m_isBuilt;

	// Token: 0x0400184D RID: 6221
	private Coroutine m_checkCoroutine;

	// Token: 0x0400184E RID: 6222
	private SkillTreeWindowController m_skillTreeWindow;

	// Token: 0x0400184F RID: 6223
	private UnityAction m_onWindowOpened;

	// Token: 0x04001850 RID: 6224
	private UnityAction m_onWindowClosed;
}
