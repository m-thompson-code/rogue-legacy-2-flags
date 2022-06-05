using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public class TraitTester : MonoBehaviour
{
	// Token: 0x17001263 RID: 4707
	// (get) Token: 0x06002E49 RID: 11849 RVA: 0x00019539 File Offset: 0x00017739
	// (set) Token: 0x06002E4A RID: 11850 RVA: 0x00019541 File Offset: 0x00017741
	public TraitType Trait1
	{
		get
		{
			return this.m_trait1Test;
		}
		set
		{
			this.m_trait1Test = value;
		}
	}

	// Token: 0x17001264 RID: 4708
	// (get) Token: 0x06002E4B RID: 11851 RVA: 0x0001954A File Offset: 0x0001774A
	// (set) Token: 0x06002E4C RID: 11852 RVA: 0x00019552 File Offset: 0x00017752
	public TraitType Trait2
	{
		get
		{
			return this.m_trait2Test;
		}
		set
		{
			this.m_trait2Test = value;
		}
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x0001955B File Offset: 0x0001775B
	private void OnEnable()
	{
		if (!this.m_traitsTriggered)
		{
			base.StartCoroutine(this.TestCoroutine());
		}
	}

	// Token: 0x06002E4E RID: 11854 RVA: 0x0001955B File Offset: 0x0001775B
	private void Start()
	{
		if (!this.m_traitsTriggered)
		{
			base.StartCoroutine(this.TestCoroutine());
		}
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x00019572 File Offset: 0x00017772
	private IEnumerator TestCoroutine()
	{
		this.m_traitsTriggered = true;
		yield return new WaitForSeconds(1f);
		SaveManager.PlayerSaveData.CurrentCharacter.TraitOne = this.m_trait1Test;
		SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo = this.m_trait2Test;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, this, new TraitChangedEventArgs(this.m_trait1Test, this.m_trait2Test));
		yield break;
	}

	// Token: 0x04002606 RID: 9734
	[SerializeField]
	private TraitType m_trait1Test;

	// Token: 0x04002607 RID: 9735
	[SerializeField]
	private TraitType m_trait2Test;

	// Token: 0x04002608 RID: 9736
	private bool m_traitsTriggered;
}
