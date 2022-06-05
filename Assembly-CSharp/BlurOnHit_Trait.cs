using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class BlurOnHit_Trait : BaseTrait
{
	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06001F95 RID: 8085 RVA: 0x00064F87 File Offset: 0x00063187
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurOnHit;
		}
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x00064F8E File Offset: 0x0006318E
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter));
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x00064FBB File Offset: 0x000631BB
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		this.m_waitYield = new WaitRL_Yield(0f, false);
		PlayerController playerController = PlayerManager.GetPlayerController();
		float y = playerController.Midpoint.y - playerController.transform.position.y;
		this.m_traitMask.transform.SetParent(playerController.transform);
		this.m_traitMask.transform.localPosition = new Vector3(0f, y, 0f);
		float num = 10f;
		this.m_traitMask.transform.localScale = new Vector3(num, num, 1f);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		this.m_blurGameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x00064FCC File Offset: 0x000631CC
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x00065038 File Offset: 0x00063238
	private void OnPlayerHit(MonoBehaviour sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController && playerController.IsPerfectBlocking)
		{
			return;
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.BlurCoroutine());
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x0006506F File Offset: 0x0006326F
	private IEnumerator BlurCoroutine()
	{
		this.m_blurGameObject.SetActive(true);
		CameraController.ForegroundPostProcessing.TintBaselineAmount = 1f;
		if (this.m_blurTween)
		{
			this.m_blurTween.StopTweenWithConditionChecks(false, CameraController.ForegroundPostProcessing, null);
		}
		this.m_waitYield.CreateNew(2.5f, false);
		yield return this.m_waitYield;
		this.m_blurTween = TweenManager.TweenTo(CameraController.ForegroundPostProcessing, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"TintBaselineAmount",
			0f
		});
		yield return this.m_blurTween.TweenCoroutine;
		CameraController.ForegroundPostProcessing.TintBaselineAmount = 0f;
		this.m_blurTween = null;
		this.m_blurGameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x00065080 File Offset: 0x00063280
	public override void DisableOnCutscene()
	{
		if (this.m_blurTween)
		{
			this.m_blurTween.StopTweenWithConditionChecks(false, CameraController.ForegroundPostProcessing, null);
		}
		base.StopAllCoroutines();
		this.m_blurGameObject.SetActive(false);
		CameraController.ForegroundPostProcessing.TintBaselineAmount = 0f;
		base.DisableOnCutscene();
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x000650D4 File Offset: 0x000632D4
	public override void DisableOnDeath()
	{
		if (this.m_blurTween)
		{
			this.m_blurTween.StopTweenWithConditionChecks(false, CameraController.ForegroundPostProcessing, null);
		}
		base.StopAllCoroutines();
		this.m_blurGameObject.SetActive(false);
		CameraController.ForegroundPostProcessing.TintBaselineAmount = 0f;
		base.DisableOnDeath();
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x00065128 File Offset: 0x00063328
	private void OnBiomeEnter(object sender, EventArgs args)
	{
		if (this.m_blurTween)
		{
			this.m_blurTween.StopTweenWithConditionChecks(false, CameraController.ForegroundPostProcessing, null);
		}
		base.StopAllCoroutines();
		this.m_blurGameObject.SetActive(false);
		CameraController.ForegroundPostProcessing.TintBaselineAmount = 0f;
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x00065178 File Offset: 0x00063378
	private void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
		if (!GameUtility.IsApplicationQuitting)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter));
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHit, this.m_onPlayerHit);
		}
	}

	// Token: 0x04001C33 RID: 7219
	[SerializeField]
	private GameObject m_blurGameObject;

	// Token: 0x04001C34 RID: 7220
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001C35 RID: 7221
	private Tween m_blurTween;

	// Token: 0x04001C36 RID: 7222
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
