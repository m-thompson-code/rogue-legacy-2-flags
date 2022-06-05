using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class CharacterSortController : MonoBehaviour
{
	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00037B68 File Offset: 0x00035D68
	// (set) Token: 0x060012C6 RID: 4806 RVA: 0x00037B70 File Offset: 0x00035D70
	public int SortIndex { get; private set; }

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00037B79 File Offset: 0x00035D79
	// (set) Token: 0x060012C8 RID: 4808 RVA: 0x00037B81 File Offset: 0x00035D81
	public BaseCharacterController CharacterController { get; private set; }

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060012C9 RID: 4809 RVA: 0x00037B8A File Offset: 0x00035D8A
	public int SubLayer
	{
		get
		{
			return this.m_cameraLayerController.SubLayer;
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060012CA RID: 4810 RVA: 0x00037B97 File Offset: 0x00035D97
	// (set) Token: 0x060012CB RID: 4811 RVA: 0x00037B9F File Offset: 0x00035D9F
	public List<CharacterSortController> SortList { get; private set; } = new List<CharacterSortController>(5);

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060012CC RID: 4812 RVA: 0x00037BA8 File Offset: 0x00035DA8
	// (set) Token: 0x060012CD RID: 4813 RVA: 0x00037BB0 File Offset: 0x00035DB0
	public bool IsInitialized { get; private set; }

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060012CE RID: 4814 RVA: 0x00037BB9 File Offset: 0x00035DB9
	// (set) Token: 0x060012CF RID: 4815 RVA: 0x00037BC1 File Offset: 0x00035DC1
	public bool IsPlayer { get; private set; }

	// Token: 0x060012D0 RID: 4816 RVA: 0x00037BCC File Offset: 0x00035DCC
	private void Awake()
	{
		this.CharacterController = base.GetComponentInParent<BaseCharacterController>();
		this.m_cameraLayerController = this.CharacterController.GetComponent<CameraLayerController>();
		this.IsPlayer = (this.CharacterController is PlayerController);
		this.m_collider = this.CharacterController.VisualBoundsObj.Collider;
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00037C32 File Offset: 0x00035E32
	private IEnumerator Start()
	{
		this.m_collider.gameObject.layer = 29;
		this.m_collider.isTrigger = true;
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		while (!this.CharacterController.IsInitialized)
		{
			yield return null;
		}
		this.ResetCharacterLayers();
		this.UpdateSortDepth();
		this.IsInitialized = true;
		yield break;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00037C44 File Offset: 0x00035E44
	private void OnEnable()
	{
		this.m_collider.enabled = false;
		this.m_collider.enabled = true;
		if (!this.SortList.Contains(this))
		{
			this.SortList.Add(this);
		}
		if (this.IsPlayer)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x00037C98 File Offset: 0x00035E98
	private void OnDisable()
	{
		this.SortList.Clear();
		this.SortIndex = 0;
		this.CharacterController.Visuals.transform.SetLocalPositionZ(0f);
		if (this.IsPlayer)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x00037CE5 File Offset: 0x00035EE5
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.SortList.Clear();
		if (!this.SortList.Contains(this))
		{
			this.SortList.Add(this);
		}
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00037D0C File Offset: 0x00035F0C
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (this.IsPlayer)
		{
			return;
		}
		CharacterSortController component = collider.GetComponent<CharacterSortController>();
		if (component && !this.SortList.Contains(component))
		{
			this.SortList.Add(component);
			this.UpdateSortList();
		}
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x00037D54 File Offset: 0x00035F54
	private void OnTriggerExit2D(Collider2D collider)
	{
		if (this.IsPlayer)
		{
			return;
		}
		CharacterSortController component = collider.GetComponent<CharacterSortController>();
		if (component && this.SortList.Contains(component))
		{
			this.SortList.Remove(component);
			if (component.SortIndex < this.SortIndex)
			{
				int sortIndex = this.SortIndex;
				this.SortIndex = sortIndex - 1;
			}
			if (this.SortList.Count <= 1)
			{
				this.SortIndex = 0;
			}
			this.UpdateSortList();
		}
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x00037DD0 File Offset: 0x00035FD0
	public void UpdateSortList()
	{
		this.SortList.Sort(new Comparison<CharacterSortController>(this.SortBySubLayer));
		for (int i = 0; i < this.SortList.Count; i++)
		{
			CharacterSortController characterSortController = this.SortList[i];
			if (!characterSortController.IsPlayer)
			{
				if (i > characterSortController.SortIndex)
				{
					characterSortController.SortIndex = i;
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < i; j++)
					{
						if (i == this.SortList[j].SortIndex)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						characterSortController.SortIndex = i + 1;
					}
				}
				characterSortController.UpdateSortDepth();
			}
		}
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x00037E6C File Offset: 0x0003606C
	public void UpdateSortDepth()
	{
		float num;
		if (this.m_depthOverride == -1f)
		{
			num = (float)this.SortIndex * 1f;
		}
		else
		{
			num = (float)this.SortIndex * this.m_depthOverride;
		}
		if (this.IsPlayer)
		{
			num += -0.5f;
		}
		else
		{
			num += 0f;
		}
		if (this.CharacterController.Visuals)
		{
			this.CharacterController.Visuals.transform.SetLocalPositionZ(num);
		}
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x00037EE8 File Offset: 0x000360E8
	private int SortBySubLayer(CharacterSortController a, CharacterSortController b)
	{
		if (a.SubLayer > b.SubLayer || a.IsPlayer)
		{
			return -1;
		}
		if (a.SubLayer < b.SubLayer || b.IsPlayer)
		{
			return 1;
		}
		if (a.SortIndex < b.SortIndex)
		{
			return -1;
		}
		if (a.SortIndex > b.SortIndex)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00037F48 File Offset: 0x00036148
	public void ResetCharacterLayers()
	{
		int layer = 29;
		for (int i = 0; i < this.CharacterController.RendererArray.Count; i++)
		{
			this.CharacterController.RendererArray[i].gameObject.layer = layer;
		}
	}

	// Token: 0x04001311 RID: 4881
	private const float DEPTH_MULTIPLIER = 1f;

	// Token: 0x04001312 RID: 4882
	private const float PLAYER_STARTING_DEPTH_OFFSET = -0.5f;

	// Token: 0x04001313 RID: 4883
	private const float ENEMY_STARTING_DEPTH_OFFSET = 0f;

	// Token: 0x04001314 RID: 4884
	[SerializeField]
	private float m_depthOverride = -1f;

	// Token: 0x0400131A RID: 4890
	private Collider2D m_collider;

	// Token: 0x0400131B RID: 4891
	private CameraLayerController m_cameraLayerController;

	// Token: 0x0400131C RID: 4892
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
