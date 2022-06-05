using System;
using TMPro;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class PurchaseBoxDialogueController : MonoBehaviour
{
	// Token: 0x06002391 RID: 9105 RVA: 0x000738C8 File Offset: 0x00071AC8
	private void Awake()
	{
		if (this.m_purchaseBoxGoldGO)
		{
			this.m_storedGoldTextScale = this.m_purchaseBoxGoldGO.transform.localScale;
		}
		if (this.m_purchaseBoxOreTextGO)
		{
			this.m_storedOreTextScale = this.m_purchaseBoxOreTextGO.transform.localScale;
		}
		this.m_onPurchaseDialogueUpdated = new Action<MonoBehaviour, EventArgs>(this.OnPurchaseDialogueUpdated);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x0007393F File Offset: 0x00071B3F
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.UpdatePurchaseBoxDialogue, this.m_onPurchaseDialogueUpdated);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x0007395B File Offset: 0x00071B5B
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.UpdatePurchaseBoxDialogue, this.m_onPurchaseDialogueUpdated);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x00073978 File Offset: 0x00071B78
	private void OnPurchaseDialogueUpdated(object sender, EventArgs args)
	{
		PurchaseBoxDialogueEventArgs purchaseBoxDialogueEventArgs = args as PurchaseBoxDialogueEventArgs;
		string text = null;
		switch (purchaseBoxDialogueEventArgs.DialogueType)
		{
		case PurchaseBoxDialogueType.Welcome:
			if (this.m_welcomeDialogue != null && this.m_welcomeDialogue.Length != 0)
			{
				text = this.m_welcomeDialogue[UnityEngine.Random.Range(0, this.m_welcomeDialogue.Length)];
				this.m_flavourDescriptionTimer = Time.unscaledTime;
			}
			break;
		case PurchaseBoxDialogueType.PurchaseSuccessful:
			text = this.m_purchaseSuccessfulDialogue;
			this.m_flavourDescriptionTimer = 0f;
			break;
		case PurchaseBoxDialogueType.PurchaseFailed_NoMoney:
			if (this.m_purchaseBoxGoldGO)
			{
				this.m_purchaseBoxGoldGO.transform.localScale = this.m_storedGoldTextScale + new Vector3(0.25f, 0.25f, 0.25f);
				TweenManager.TweenTo_UnscaledTime(this.m_purchaseBoxGoldGO.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
				{
					"localScale.x",
					this.m_storedGoldTextScale.x,
					"localScale.y",
					this.m_storedGoldTextScale.y,
					"localScale.z",
					this.m_storedGoldTextScale.z
				});
			}
			text = this.m_purchaseFailedNoMoneyDialogue;
			this.m_flavourDescriptionTimer = 0f;
			break;
		case PurchaseBoxDialogueType.PurchaseFailed_NoOre:
			if (this.m_purchaseBoxOreTextGO)
			{
				this.m_purchaseBoxOreTextGO.transform.localScale = this.m_storedOreTextScale + new Vector3(0.25f, 0.25f, 0.25f);
				TweenManager.TweenTo_UnscaledTime(this.m_purchaseBoxOreTextGO.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
				{
					"localScale.x",
					this.m_storedOreTextScale.x,
					"localScale.y",
					this.m_storedOreTextScale.y,
					"localScale.z",
					this.m_storedOreTextScale.z
				});
			}
			text = this.m_purchaseFailedNoOreDialogue;
			this.m_flavourDescriptionTimer = 0f;
			break;
		case PurchaseBoxDialogueType.GearDescription:
			if (this.m_gearDescriptionDialogue != null && this.m_gearDescriptionDialogue.Length != 0 && Time.unscaledTime > this.m_flavourDescriptionTimer + 5f)
			{
				text = this.m_gearDescriptionDialogue[UnityEngine.Random.Range(0, this.m_gearDescriptionDialogue.Length)];
				this.m_flavourDescriptionTimer = Time.unscaledTime;
			}
			break;
		case PurchaseBoxDialogueType.GearNotFound:
			text = this.m_gearNotFoundDialogue;
			this.m_flavourDescriptionTimer = 0f;
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (this.m_usesLocIDs)
			{
				this.m_purchaseDialogueLocID = text;
				text = LocalizationManager.GetString(this.m_purchaseDialogueLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			this.m_purchaseBoxDialogueText.text = text;
		}
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x00073C40 File Offset: 0x00071E40
	private void RefreshText(object sender, EventArgs args)
	{
		if (this.m_usesLocIDs)
		{
			string @string = LocalizationManager.GetString(this.m_purchaseDialogueLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_purchaseBoxDialogueText.text = @string;
		}
	}

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	private TMP_Text m_purchaseBoxDialogueText;

	// Token: 0x04001E48 RID: 7752
	[SerializeField]
	private GameObject m_purchaseBoxGoldGO;

	// Token: 0x04001E49 RID: 7753
	[SerializeField]
	private GameObject m_purchaseBoxOreTextGO;

	// Token: 0x04001E4A RID: 7754
	[SerializeField]
	private bool m_usesLocIDs = true;

	// Token: 0x04001E4B RID: 7755
	[SerializeField]
	private string[] m_welcomeDialogue;

	// Token: 0x04001E4C RID: 7756
	[SerializeField]
	private string m_purchaseSuccessfulDialogue;

	// Token: 0x04001E4D RID: 7757
	[SerializeField]
	private string m_purchaseFailedNoMoneyDialogue;

	// Token: 0x04001E4E RID: 7758
	[SerializeField]
	private string m_purchaseFailedNoOreDialogue;

	// Token: 0x04001E4F RID: 7759
	[SerializeField]
	private string[] m_gearDescriptionDialogue;

	// Token: 0x04001E50 RID: 7760
	[SerializeField]
	private string m_gearNotFoundDialogue;

	// Token: 0x04001E51 RID: 7761
	private float m_flavourDescriptionTimer;

	// Token: 0x04001E52 RID: 7762
	private Vector3 m_storedGoldTextScale;

	// Token: 0x04001E53 RID: 7763
	private Vector3 m_storedOreTextScale;

	// Token: 0x04001E54 RID: 7764
	private string m_purchaseDialogueLocID;

	// Token: 0x04001E55 RID: 7765
	private Action<MonoBehaviour, EventArgs> m_onPurchaseDialogueUpdated;

	// Token: 0x04001E56 RID: 7766
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
