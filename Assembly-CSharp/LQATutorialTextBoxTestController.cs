using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class LQATutorialTextBoxTestController : MonoBehaviour
{
	// Token: 0x06003006 RID: 12294 RVA: 0x000A470B File Offset: 0x000A290B
	private void OnEnable()
	{
		if (this.m_textBoxArray == null)
		{
			this.Initialize();
		}
		base.StartCoroutine(this.DisplayAllTextBoxes());
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x000A4728 File Offset: 0x000A2928
	private void Initialize()
	{
		Room component = base.GetComponent<Room>();
		this.m_textBoxArray = component.GetComponentsInChildren<TutorialTextBoxController>(true);
		foreach (TutorialTextBoxController tutorialTextBoxController in this.m_textBoxArray)
		{
			tutorialTextBoxController.StickToCamera = false;
			tutorialTextBoxController.MustHaveConditions = ConditionFlag.None;
			tutorialTextBoxController.MustNotHaveConditions = ConditionFlag.None;
			IRootObj componentInParent = tutorialTextBoxController.GetComponentInParent<IRootObj>();
			if (!componentInParent.IsNativeNull() && componentInParent.gameObject != component.gameObject)
			{
				Interactable component2 = componentInParent.gameObject.GetComponent<Interactable>();
				if (component2)
				{
					component2.SetIsInteractableActive(false);
				}
			}
		}
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000A47B5 File Offset: 0x000A29B5
	private IEnumerator DisplayAllTextBoxes()
	{
		yield return null;
		TutorialTextBoxController[] textBoxArray = this.m_textBoxArray;
		for (int i = 0; i < textBoxArray.Length; i++)
		{
			textBoxArray[i].DisplayText();
		}
		yield break;
	}

	// Token: 0x04002641 RID: 9793
	private TutorialTextBoxController[] m_textBoxArray;
}
