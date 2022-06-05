using System;
using System.Collections;
using Ferr;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class TallSpike_Hazard : Hazard, IFerr2DHazard, IHazard, IRootObj, IMirror
{
	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x0600293C RID: 10556 RVA: 0x0008858F File Offset: 0x0008678F
	// (set) Token: 0x0600293D RID: 10557 RVA: 0x00088597 File Offset: 0x00086797
	public Ferr2DT_PathTerrain Ferr2D { get; private set; }

	// Token: 0x0600293E RID: 10558 RVA: 0x000885A0 File Offset: 0x000867A0
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000885B4 File Offset: 0x000867B4
	private void Start()
	{
		if (base.transform.localEulerAngles.z != 0f || base.transform.localScale.x != 1f || base.transform.localScale.y != 1f)
		{
			BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
			string text = (componentInParent != null) ? componentInParent.gameObject.name : "NULL ROOM";
			Debug.LogFormat("<color=red>TallSpike Hazard {0} was not created correctly in room {1}.  Check transform values.</color>", new object[]
			{
				this.GetRoot(false).name,
				text
			});
		}
		if (Application.isPlaying)
		{
			CameraLayerController component = base.GetComponent<CameraLayerController>();
			Renderer component2 = base.GetComponent<Renderer>();
			component2.sortingLayerID = CameraLayerUtility.GetLayer(component.CameraLayer);
			component2.sortingOrder = CameraLayerUtility.GetSpriteOrderInLayer(component.CameraLayer, component.SubLayer);
		}
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x0008868C File Offset: 0x0008688C
	public override void Initialize(HazardArgs hazardArgs)
	{
		Ferr2DHazardArgs ferr2DHazardArgs = hazardArgs as Ferr2DHazardArgs;
		if (ferr2DHazardArgs == null)
		{
			return;
		}
		if (this.m_hbController == null)
		{
			throw new MissingComponentException("HitboxControllerLite");
		}
		this.Ferr2D = base.GetComponent<Ferr2DT_PathTerrain>();
		this.Ferr2D.transform.localPosition = ferr2DHazardArgs.LocalPosition;
		this.Ferr2D.ClearPoints();
		for (int i = 0; i < ferr2DHazardArgs.Points.Count; i++)
		{
			this.Ferr2D.AddPoint(ferr2DHazardArgs.Points[i], -1, PointType.Sharp);
		}
		this.Ferr2D.Build(true);
		if (Time.inFixedTimeStep)
		{
			base.Invoke("InitializeHitboxes", 0f);
			return;
		}
		this.InitializeHitboxes();
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x00088744 File Offset: 0x00086944
	private void InitializeHitboxes()
	{
		this.m_hbController.Initialize();
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x00088751 File Offset: 0x00086951
	public override void ResetHazard()
	{
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x00088753 File Offset: 0x00086953
	private void OnEnable()
	{
		if (this.m_flipColliders && !this.m_collidersFlipped)
		{
			base.StartCoroutine(this.MirrorTerrain());
		}
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x00088772 File Offset: 0x00086972
	public void Mirror()
	{
		if (!this.m_hbController.IsInitialized)
		{
			this.m_flipColliders = true;
		}
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x00088788 File Offset: 0x00086988
	private IEnumerator MirrorTerrain()
	{
		yield return new WaitUntil(() => this.m_hbController.IsInitialized);
		Collider2D collider = this.m_hbController.GetCollider(HitboxType.Terrain);
		Collider2D collider2 = this.m_hbController.GetCollider(HitboxType.Platform);
		Collider2D component = base.GetComponent<PolygonCollider2D>();
		if (!(collider2 is PolygonCollider2D))
		{
			collider2.offset = new Vector2(-collider2.offset.x, collider2.offset.y);
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		if (collider != null)
		{
			CDGHelper.CopyComponent<Collider2D>(collider2, collider.gameObject, false);
		}
		this.m_collidersFlipped = true;
		yield break;
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x0008879F File Offset: 0x0008699F
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040021FC RID: 8700
	private bool m_flipColliders;

	// Token: 0x040021FD RID: 8701
	private bool m_collidersFlipped;

	// Token: 0x040021FE RID: 8702
	private IHitboxController m_hbController;
}
