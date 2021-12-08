﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Outposts;
using RimWorld;
using Verse;

namespace VOE
{
    public class Outpost_Town : Outpost
    {
        public override void Produce()
        {
            var newPawns = new List<Pawn>();
            foreach (var pawn in AllPawns)
                if (Rand.Chance(pawn.skills.GetSkill(SkillDefOf.Social).Level / 100f))
                {
                    var newPawn = PawnGenerator.GeneratePawn(pawn.kindDef, pawn.Faction);
                    newPawns.Add(newPawn);
                    AddPawn(newPawn);
                }

            if (newPawns.Any())
                Find.LetterStack.ReceiveLetter("Outposts.Letters.Recruit.Label".Translate(Name),
                    "Outposts.Letters.Recruit.Desc".Translate(Name) + ":\n" + newPawns.Join(pawn => pawn.NameFullColored, "\n   - "), LetterDefOf.PositiveEvent,
                    new LookTargets(Gen.YieldSingle(this)));
        }

        public static string CanSpawnOnWith(int tile, List<Pawn> pawns) => Find.WorldObjects.Settlements.Count(s => Find.WorldGrid.ApproxDistanceInTiles(s.Tile, tile) < 10) < 3
            ? "Outposts.NearbySettlements".Translate(3, 10)
            : null;

        public static string RequirementsString(int tile, List<Pawn> pawns) =>
            Requirement("Outposts.NearbySettlements".Translate(3, 10),
                Find.WorldObjects.Settlements.Count(s => Find.WorldGrid.ApproxDistanceInTiles(s.Tile, tile) < 10) >= 3);
    }
}