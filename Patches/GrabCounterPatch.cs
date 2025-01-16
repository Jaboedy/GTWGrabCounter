using HarmonyLib;
using Isto.GTW.Player;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace GrabCounter.Patches
{

	[HarmonyPatch(typeof(PlayerController))]
	internal class GrabCounterPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch("ActivateGrabLink")]
		static void PlayerController_OnActivateGrabPrefix(ref PlayerController __instance, ref bool __state)
		{
			Traverse instance = Traverse.Create(__instance);
			bool isGrabLinkActive = instance.Field("_isGrabLinkActive").GetValue<bool>();
			float grabCooldownTimeLeft = instance.Field("_grabCooldownTimeLeft").GetValue<float>();
			if (isGrabLinkActive || grabCooldownTimeLeft > 0f)
			{
				__state = false;
				return;
			}
			__state = true;
		}

		[HarmonyPostfix]
		[HarmonyPatch("ActivateGrabLink")]
		static void PlayerController_OnActivateGrabPostfix(ref PlayerController __instance, bool __state)
		{
			if (!__state || !__instance.PlayerCollisionHandler.IsGrounded)
			{
				return;
			}
			var grabCounterTextObj = GameObject.Find("txt_GrabCounter");
			var grabCounterText = grabCounterTextObj.GetComponent<TextMeshProUGUI>();
			int currentGrabs = int.Parse(grabCounterText.text.Split(':')[1].Trim());
			currentGrabs++;
			grabCounterText.text = $"Grabs: {currentGrabs}";
		}
	}
}
