using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace Decreased_Fall_Damage
{
	// Token: 0x02000003 RID: 3
	public static class Configuration
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002100 File Offset: 0x00000300
		public static bool Enable
		{
			get
			{
				return Configuration._enable.Value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000211C File Offset: 0x0000031C
		public static int fallDamage
		{
			get
			{
				return Configuration._fallDamage.Value;
			}
		}

		public static int minFallHeight
		{
			get
			{
				return Configuration._minFallHeight.Value;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002154 File Offset: 0x00000354
		public static void InitConfiguration()
		{
			Configuration.InitValues();
			Configuration.CheckConfiguration();

			Configuration._configFile.Save();
#if DEBUG
			DecreasedFallDamage.Logger.LogInfo("Successfully loaded configuration");
#endif
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002224 File Offset: 0x00000424
		private static void InitValues()
		{
			Configuration._configFile = new ConfigFile(Path.Combine(Paths.ConfigPath,"ritchmods.valheim.decreasedfalldamage.cfg"), true);
			Configuration._enable = Configuration._configFile.Bind<bool>("General", "enable", true, "Description : Enable / disable the mod" + Environment.NewLine + "Values : true; false");
			Configuration._fallDamage = Configuration._configFile.Bind<int>("General", "fallDamage", 1, "Description : Fall damage rate. Default : 1" + Environment.NewLine + "Values : 0 - 100");
			Configuration._minFallHeight = Configuration._configFile.Bind<int>("General", "minFallheight", 4, "Description : Height at which fall damage occurs. Default : 1" + Environment.NewLine + "Values : 1 - 100");
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022D8 File Offset: 0x000004D8
		private static void CheckConfiguration()
		{
			if (Configuration._enable.Value && !Configuration._enable.Value)
			{
#if DEBUG
				DecreasedFallDamage.Logger.LogWarning("Config \"enable\" was reset to default : true (before " + Configuration._enable.Value.ToString() + ")");
#endif
				Configuration._enable.Value = true;
			}

			if (Configuration._fallDamage.Value < 0 || Configuration._fallDamage.Value > 100)
			{
#if DEBUG
				DecreasedFallDamage.Logger.LogWarning("Config \"fallDamage\" was reset to default : 0 (before " + Configuration._fallDamage.Value.ToString() + ")");
#endif
				Configuration._fallDamage.Value = 0;
			}

			if (Configuration._minFallHeight.Value < 1 || Configuration._minFallHeight.Value > 100)
			{
#if DEBUG
				DecreasedFallDamage.Logger.LogWarning("Config \"fallDamage\" was reset to default : 0 (before " + Configuration._fallDamage.Value.ToString() + ")");
#endif
				Configuration._fallDamage.Value = 4;
			}
		}

		// Token: 0x04000001 RID: 1
		private static ConfigFile _configFile;

		// Token: 0x04000002 RID: 2
		private static ConfigEntry<bool> _enable;

		// Token: 0x04000003 RID: 3
		private static ConfigEntry<int> _fallDamage;

		private static ConfigEntry<int> _minFallHeight;
	}
}

