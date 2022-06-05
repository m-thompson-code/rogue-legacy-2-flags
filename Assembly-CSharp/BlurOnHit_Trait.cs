using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class BlurOnHit_Trait : BaseTrait
{
	// Token: 0x170011CC RID: 4556
	// (get) Token: 0x06002C3C RID: 11324 RVA: 0x0001890F File Offset: 0x00016B0F
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurOnHit;
		}
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x00018916 File Offset: 0x00016B16
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHit = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter));
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x00018943 File Offset: 0x00016B43
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

	// Token: 0x06002C3F RID: 11327 RVA: 0x000C53C4 File Offset: 0x000C35C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000C5430 File Offset: 0x000C3630
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

	// Token: 0x06002C41 RID: 11329 RVA: 0x00018952 File Offset: 0x00016B52
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

	// Token: 0x06002C42 RID: 11330 RVA: 0x000C5468 File Offset: 0x000C3668
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

	// Token: 0x06002C43 RID: 11331 RVA: 0x000C54BC File Offset: 0x000C36BC
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

	// Token: 0x06002C44 RID: 11332 RVA: 0x000C5510 File Offset: 0x000C3710
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

	// Token: 0x06002C45 RID: 11333 RVA: 0x000C5560 File Offset: 0x000C3760
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

	// Token: 0x04002557 RID: 9559
	[SerializeField]
	private GameObject m_blurGameObject;

	// Token: 0x04002558 RID: 9560
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002559 RID: 9561
	private Tween m_blurTween;

	// Token: 0x0400255A RID: 9562
	private Action<MonoBehaviour, EventArgs> m_onPlayerHit;
}
