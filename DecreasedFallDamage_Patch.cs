using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace Decreased_Fall_Damage
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(Character))]
	public static class DecreasedFallDamage_Patch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[HarmonyPatch("UpdateGroundContact")]
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> UpdateGroundContact(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].opcode == OpCodes.Ldc_R4 && object.Equals(list[i].operand, 100f))
				{
#if DEBUG
					DecreasedFallDamage.Logger.LogWarning("[DEBUG] Patching...");
#endif
					list[i] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DecreasedFallDamage_Patch), "GetMaxFallDamage", null, null));
				}

				else if (
					list[i].opcode == OpCodes.Ldloc_0 &&
					list[i + 1].opcode == OpCodes.Ldc_R4 &&
					object.Equals(list[i + 1].operand, 4f))
				{
#if DEBUG
					DecreasedFallDamage.Logger.LogWarning("[DEBUG] Patching Next...");
#endif
					list[i + 1] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DecreasedFallDamage_Patch), "GetMinFallHeight", null, null));

				}
			}
			return list;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E8 File Offset: 0x000002E8
		private static float GetMaxFallDamage()
		{
			return (float)Configuration.fallDamage;
		}

		private static float GetMinFallHeight()
		{
			return (float)Configuration.minFallHeight;
		}

	}
}

