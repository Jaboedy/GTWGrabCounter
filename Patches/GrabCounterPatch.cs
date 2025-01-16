using HarmonyLib;
using Isto.GTW.LevelFeatures.LevelEventTriggers;
using Isto.Core;
using Isto.GTW.Player;
using TMPro;
using UnityEngine;

namespace GrabCounter.Patches
{
	[HarmonyPatch(typeof(PlayerController))]
	internal class GrabCounterPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch("ActivateGrabLink")]
		static void PlayerController_OnActivateGrabPrefix(ref PlayerController __instance)
		{
			Traverse instance = Traverse.Create(__instance);
			bool isGrabLinkActive = instance.Field("_isGrabLinkActive").GetValue<bool>();
			float grabCooldownTimeLeft = instance.Field("_grabCooldownTimeLeft").GetValue<float>();
			if (isGrabLinkActive || grabCooldownTimeLeft > 0f || !__instance.PlayerCollisionHandler.IsGrounded)
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
