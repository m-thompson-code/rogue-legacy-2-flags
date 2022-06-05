using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000873 RID: 2163
public class LQATutorialTextBoxTestController : MonoBehaviour
{
	// Token: 0x06004298 RID: 17048 RVA: 0x00024D6F File Offset: 0x00022F6F
	private void OnEnable()
	{
		if (this.m_textBoxArray == null)
		{
			this.Initialize();
		}
		base.StartCoroutine(this.DisplayAllTextBoxes());
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x0010B3F4 File Offset: 0x001095F4
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

	// Token: 0x0600429A RID: 17050 RVA: 0x00024D8C File Offset: 0x00022F8C
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

	// Token: 0x0400340E RID: 13326
	private TutorialTextBoxController[] m_textBoxArray;
}
