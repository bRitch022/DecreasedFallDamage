using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Decreased_Fall_Damage
{
	// Token: 0x02000004 RID: 4
	[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
	[BepInProcess("valheim.exe")]
	public class DecreasedFallDamage : BaseUnityPlugin
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002404 File Offset: 0x00000604
		public void Awake()
		{
			BepInEx.Logging.Logger.Sources.Add(DecreasedFallDamage.Logger);
			Configuration.InitConfiguration();

			if (Configuration.Enable)
			{
				new Harmony("bRitch02.valheim.DecreasedFallDamage").PatchAll();
			}
			else
			{
				if (!Configuration.Enable)
				{
					DecreasedFallDamage.Logger.LogWarning("[DEBUG] Mod disable, patch skipped.");
				}
			}
		}

		// Token: 0x04000005 RID: 5
		public const string pluginGuid = "ritchmods.valheim.DecreasedFallDamage";

		// Token: 0x04000006 RID: 6
		public const string pluginName = "Decreased Fall Damage";

		// Token: 0x04000007 RID: 7
		public const string pluginVersion = "1.2.0";

		// Token: 0x04000008 RID: 8
		public new static ManualLogSource Logger = new ManualLogSource("Decreased Fall Damage");
	}
}
