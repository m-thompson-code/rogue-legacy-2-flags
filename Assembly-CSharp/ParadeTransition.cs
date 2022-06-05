using System;
using System.Collections;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class ParadeTransition : MonoBehaviour, IRootObj
{
	// Token: 0x06003251 RID: 12881 RVA: 0x000AA895 File Offset: 0x000A8A95
	public void TransitionToParade()
	{
		base.StartCoroutine(this.TransitionToParadeCoroutine());
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x000AA8A4 File Offset: 0x000A8AA4
	private IEnumerator TransitionToParadeCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		PlayerManager.GetPlayerController().StopActiveAbilities(true);
		ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(TransitionID.FadeToBlackNoLoading) as ISceneLoadingTransition;
		transition.GameObject.SetActive(true);
		yield return null;
		yield return transition.TransitionIn();
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterParade, null, null);
		WindowManager.SetWindowIsOpen(WindowID.PlayerDeath, true);
		yield return null;
		yield return transition.TransitionOut();
		transition.GameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x000AA8B4 File Offset: 0x000A8AB4
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002787 RID: 10119
	[SerializeField]
	private GameObject m_playerEnterPositionObj;
}
