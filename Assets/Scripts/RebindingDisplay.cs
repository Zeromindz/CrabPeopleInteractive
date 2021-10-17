using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingDisplay : MonoBehaviour
{
	[SerializeField] private InputActionReference accelerationAction = null;
	[SerializeField] private PlayerController playerController = null;
	[SerializeField] private GameObject bindingDisplayText = null;
	[SerializeField] private GameObject startRebingdingObject = null;
	[SerializeField] private GameObject waitingForInputObject = null;

	private InputActionRebindingExtensions.RebindingOperation RebindingOperation;

	public void StartRebinding()
	{
		startRebingdingObject.SetActive(false);
		waitingForInputObject.SetActive(true);

	//	playerController.PlayerInput.SwitchCurrentActionMap("Menu");

		RebindingOperation = accelerationAction.action.PerformInteractiveRebinding()
			.WithControlsExcluding("Mouse")
			.OnMatchWaitForAnother(0.1f)
			.OnComplete(operation => RebindComplete())
			.Start();
	}

	private void RebindComplete()
	{
		//playerController.PlayerInput.SwitchCurrentActionMap("Player");
		//int bindingIndex = accelerationAction.action.GetBindingIndexForControl(accelerationAction.action.controls[0]);

		bindingDisplayText.GetComponent<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(
			accelerationAction.action.bindings[0].effectivePath,
			InputControlPath.HumanReadableStringOptions.OmitDevice);

		RebindingOperation.Dispose();

		startRebingdingObject.SetActive(true);
		waitingForInputObject.SetActive(false);


	}
}
