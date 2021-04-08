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
			bool patch1_done = false;
			bool patch2_done = false;

			List<CodeInstruction> list = instructions.ToList<CodeInstruction>();

			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].opcode == OpCodes.Ldc_R4 && object.Equals(list[i].operand, 100f) && !patch1_done)
				{

					list[i] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DecreasedFallDamage_Patch), "GetMaxFallDamage", null, null));
					patch1_done = true;
#if DEBUG
					DecreasedFallDamage.Logger.LogWarning("[DEBUG] Patching " + list[i].ToString());
#endif
				}

				else if (
					list[i].opcode == OpCodes.Ldloc_0 &&
					list[i + 1].opcode == OpCodes.Ldc_R4 &&
					object.Equals(list[i + 1].operand, 4f) &&
					!patch2_done)
				{
					list[i + 1] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DecreasedFallDamage_Patch), "GetMinFallHeight", null, null));
					patch2_done = true;
#if DEBUG
					DecreasedFallDamage.Logger.LogWarning("[DEBUG] Patching " + list[i + 1].ToString());
#endif
				}
			}
			return list;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E8 File Offset: 0x000002E8
		private static float GetMaxFallDamage()
		{
			if(Configuration.skillMode)
            {
				float jumpSkill = Player.m_localPlayer.GetSkills().GetSkill(Skills.SkillType.Jump).m_level;
				float newFallDamage = Configuration.fallDamage - jumpSkill;

				if(newFallDamage < 0)
                {
					newFallDamage = 0;
                }

				DecreasedFallDamage.Logger.LogInfo("Player fall damage reduced by " + jumpSkill.ToString() + "%" + Environment.NewLine);
				return newFallDamage;
			}
			DecreasedFallDamage.Logger.LogInfo("Player fall damage reduced by " + (100 - Configuration.fallDamage) + "%" + Environment.NewLine);
			return Configuration.fallDamage;
		}

		private static float GetMinFallHeight()
		{
			return Configuration.minFallHeight;
		}

	}
}

