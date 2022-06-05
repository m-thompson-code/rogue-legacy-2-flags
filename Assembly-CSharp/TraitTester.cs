using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class TraitTester : MonoBehaviour
{
	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x060020AA RID: 8362 RVA: 0x00066DDE File Offset: 0x00064FDE
	// (set) Token: 0x060020AB RID: 8363 RVA: 0x00066DE6 File Offset: 0x00064FE6
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

	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x060020AC RID: 8364 RVA: 0x00066DEF File Offset: 0x00064FEF
	// (set) Token: 0x060020AD RID: 8365 RVA: 0x00066DF7 File Offset: 0x00064FF7
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

	// Token: 0x060020AE RID: 8366 RVA: 0x00066E00 File Offset: 0x00065000
	private void OnEnable()
	{
		if (!this.m_traitsTriggered)
		{
			base.StartCoroutine(this.TestCoroutine());
		}
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x00066E17 File Offset: 0x00065017
	private void Start()
	{
		if (!this.m_traitsTriggered)
		{
			base.StartCoroutine(this.TestCoroutine());
		}
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x00066E2E File Offset: 0x0006502E
	private IEnumerator TestCoroutine()
	{
		this.m_traitsTriggered = true;
		yield return new WaitForSeconds(1f);
		SaveManager.PlayerSaveData.CurrentCharacter.TraitOne = this.m_trait1Test;
		SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo = this.m_trait2Test;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.TraitsChanged, this, new TraitChangedEventArgs(this.m_trait1Test, this.m_trait2Test));
		yield break;
	}

	// Token: 0x04001C67 RID: 7271
	[SerializeField]
	private TraitType m_trait1Test;

	// Token: 0x04001C68 RID: 7272
	[SerializeField]
	private TraitType m_trait2Test;

	// Token: 0x04001C69 RID: 7273
	private bool m_traitsTriggered;
}
