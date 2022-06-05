using System;
using TMPro;
using UnityEngine;

// Token: 0x0200065C RID: 1628
public class PurchaseBoxDialogueController : MonoBehaviour
{
	// Token: 0x060031AF RID: 12719 RVA: 0x000D3BC4 File Offset: 0x000D1DC4
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

	// Token: 0x060031B0 RID: 12720 RVA: 0x0001B46B File Offset: 0x0001966B
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.UpdatePurchaseBoxDialogue, this.m_onPurchaseDialogueUpdated);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x0001B487 File Offset: 0x00019687
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.UpdatePurchaseBoxDialogue, this.m_onPurchaseDialogueUpdated);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x000D3C3C File Offset: 0x000D1E3C
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

	// Token: 0x060031B3 RID: 12723 RVA: 0x000D3F04 File Offset: 0x000D2104
	private void RefreshText(object sender, EventArgs args)
	{
		if (this.m_usesLocIDs)
		{
			string @string = LocalizationManager.GetString(this.m_purchaseDialogueLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_purchaseBoxDialogueText.text = @string;
		}
	}

	// Token: 0x04002880 RID: 10368
	[SerializeField]
	private TMP_Text m_purchaseBoxDialogueText;

	// Token: 0x04002881 RID: 10369
	[SerializeField]
	private GameObject m_purchaseBoxGoldGO;

	// Token: 0x04002882 RID: 10370
	[SerializeField]
	private GameObject m_purchaseBoxOreTextGO;

	// Token: 0x04002883 RID: 10371
	[SerializeField]
	private bool m_usesLocIDs = true;

	// Token: 0x04002884 RID: 10372
	[SerializeField]
	private string[] m_welcomeDialogue;

	// Token: 0x04002885 RID: 10373
	[SerializeField]
	private string m_purchaseSuccessfulDialogue;

	// Token: 0x04002886 RID: 10374
	[SerializeField]
	private string m_purchaseFailedNoMoneyDialogue;

	// Token: 0x04002887 RID: 10375
	[SerializeField]
	private string m_purchaseFailedNoOreDialogue;

	// Token: 0x04002888 RID: 10376
	[SerializeField]
	private string[] m_gearDescriptionDialogue;

	// Token: 0x04002889 RID: 10377
	[SerializeField]
	private string m_gearNotFoundDialogue;

	// Token: 0x0400288A RID: 10378
	private float m_flavourDescriptionTimer;

	// Token: 0x0400288B RID: 10379
	private Vector3 m_storedGoldTextScale;

	// Token: 0x0400288C RID: 10380
	private Vector3 m_storedOreTextScale;

	// Token: 0x0400288D RID: 10381
	private string m_purchaseDialogueLocID;

	// Token: 0x0400288E RID: 10382
	private Action<MonoBehaviour, EventArgs> m_onPurchaseDialogueUpdated;

	// Token: 0x0400288F RID: 10383
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
