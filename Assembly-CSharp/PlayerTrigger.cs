using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class PlayerTrigger : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06001D21 RID: 7457 RVA: 0x00060038 File Offset: 0x0005E238
	// (remove) Token: 0x06001D22 RID: 7458 RVA: 0x00060070 File Offset: 0x0005E270
	public event EventHandler<EventArgs> PlayerEnter;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06001D23 RID: 7459 RVA: 0x000600A8 File Offset: 0x0005E2A8
	// (remove) Token: 0x06001D24 RID: 7460 RVA: 0x000600E0 File Offset: 0x0005E2E0
	public event EventHandler<EventArgs> PlayerExit;

	// Token: 0x17000CD0 RID: 3280
	// (get) Token: 0x06001D25 RID: 7461 RVA: 0x00060115 File Offset: 0x0005E315
	public bool IsTriggerActive
	{
		get
		{
			return this.m_collider != null && this.m_collider.enabled;
		}
	}

	// Token: 0x17000CD1 RID: 3281
	// (get) Token: 0x06001D26 RID: 7462 RVA: 0x00060132 File Offset: 0x0005E332
	// (set) Token: 0x06001D27 RID: 7463 RVA: 0x0006013A File Offset: 0x0005E33A
	public bool IsPlayerInTrigger { get; private set; }

	// Token: 0x06001D28 RID: 7464 RVA: 0x00060143 File Offset: 0x0005E343
	private IEnumerator Start()
	{
		if (this.m_requireButtonPress)
		{
			if (this.m_pressButtonIndicator == null)
			{
				Debug.LogFormat("<color=yellow>{0}: UI Window Trigger on ({1}) requires a Button Press, but you haven't set a Press Button Indicator</color>", new object[]
				{
					Time.frameCount,
					base.name
				});
			}
			Debug.LogFormat("{0}: ({1}) is waiting for Rewired...", new object[]
			{
				Time.frameCount,
				this
			});
			yield return new WaitUntil(() => ReInput.isReady);
			Debug.LogFormat("{0}: ({1}) finished waiting for Rewired", new object[]
			{
				Time.frameCount,
				this
			});
			ReInput.players.GetPlayer(0).AddInputEventDelegate(new Action<InputActionEventData>(this.OnInteractButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
		yield break;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x00060152 File Offset: 0x0005E352
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0006015A File Offset: 0x0005E35A
	private void OnDestroy()
	{
		if (ReInput.isReady)
		{
			ReInput.players.GetPlayer(0).RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnInteractButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x00060188 File Offset: 0x0005E388
	public static PlayerTrigger CreateInstance(Vector2 size, Vector2 position, Transform parent = null)
	{
		GameObject gameObject = new GameObject("Exit");
		gameObject.transform.SetParent(parent);
		gameObject.transform.position = position;
		gameObject.layer = LayerMask.NameToLayer("Terrain_Hitbox");
		PlayerTrigger playerTrigger = gameObject.AddComponent<PlayerTrigger>();
		gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
		playerTrigger.m_collider = gameObject.AddComponent<BoxCollider2D>();
		playerTrigger.m_collider.isTrigger = true;
		(playerTrigger.m_collider as BoxCollider2D).size = size;
		return playerTrigger;
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x00060208 File Offset: 0x0005E408
	public void SetTriggerActive(bool isActive)
	{
		if (isActive)
		{
			this.Reset();
		}
		if (this.m_collider == null)
		{
			this.m_collider = base.GetComponent<Collider2D>();
			if (this.m_collider == null)
			{
				Debug.LogFormat("<color=red>{0}: No Collider found on GameObject.</color>", new object[]
				{
					Time.frameCount
				});
				return;
			}
		}
		this.m_collider.enabled = isActive;
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x00060270 File Offset: 0x0005E470
	public void Reset()
	{
		this.IsPlayerInTrigger = false;
		if (this.m_pressButtonIndicator != null)
		{
			this.m_pressButtonIndicator.SetActive(false);
		}
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x00060294 File Offset: 0x0005E494
	private void OnInteractButtonDown(InputActionEventData inputActionEventData)
	{
		if (this.m_requireButtonPress && this.IsPlayerInTrigger && !PlayerManager.GetPlayerController().CharacterCorgi.IsFrozen)
		{
			if (this.m_printDebug)
			{
				Debug.LogFormat("{0}: Player pressed interact Button on ({1})", new object[]
				{
					Time.frameCount,
					base.name
				});
			}
			if (this.PlayerEnter != null)
			{
				this.PlayerEnter(this, EventArgs.Empty);
			}
		}
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x00060309 File Offset: 0x0005E509
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") || collision.CompareTag("Player_Dodging"))
		{
			this.PlayerEnterTrigger();
		}
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0006032C File Offset: 0x0005E52C
	private void PlayerEnterTrigger()
	{
		if (this.m_printDebug)
		{
			Debug.LogFormat("<color=green>{0}: Player Entered PlayerTrigger</color>", new object[]
			{
				Time.frameCount
			});
		}
		this.IsPlayerInTrigger = true;
		if (this.m_requireButtonPress && this.m_pressButtonIndicator != null)
		{
			this.m_pressButtonIndicator.SetActive(true);
			return;
		}
		if (this.PlayerEnter != null)
		{
			this.PlayerEnter(this, EventArgs.Empty);
		}
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x000603A4 File Offset: 0x0005E5A4
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (this.m_printDebug)
		{
			Debug.LogFormat("<color=purple>{0}: Player Exited PlayerTrigger</color>", new object[]
			{
				Time.frameCount
			});
		}
		if (collision.CompareTag("Player") || collision.CompareTag("Player_Dodging"))
		{
			this.IsPlayerInTrigger = false;
			if (this.m_requireButtonPress && this.m_pressButtonIndicator != null)
			{
				this.m_pressButtonIndicator.SetActive(false);
				return;
			}
			if (this.PlayerExit != null)
			{
				this.PlayerExit(this, EventArgs.Empty);
			}
		}
	}

	// Token: 0x04001B28 RID: 6952
	[SerializeField]
	private bool m_requireButtonPress;

	// Token: 0x04001B29 RID: 6953
	[SerializeField]
	private GameObject m_pressButtonIndicator;

	// Token: 0x04001B2A RID: 6954
	[SerializeField]
	private bool m_printDebug;

	// Token: 0x04001B2B RID: 6955
	private Collider2D m_collider;
}
