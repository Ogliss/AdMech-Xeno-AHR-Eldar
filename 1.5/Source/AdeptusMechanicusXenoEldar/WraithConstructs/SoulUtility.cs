using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AdeptusMechanicus
{
    public static class SoulUtility
    {
        public static Soul StorePawnSoulIn(Pawn pawn)
        {
        //    Log.Message($"Making Soul for {pawn.Name}");
            Pawn_StoryTracker story = pawn.story;
            Pawn_RelationsTracker relations = pawn.relations;
            Soul soul = new Soul
            {
                originalPawn = pawn,
                ownerName = pawn.Name.ToStringFull,
                name = pawn.Name,
                title = story?.title,
                skills = new List<SkillRecord>(pawn.skills.skills),
                childhood = (story?.Childhood),
                adulthood = (story?.Adulthood),
                traits = (story?.traits?.allTraits),
                relations = (relations?.DirectRelations),
                relatedPawns = (relations?.RelatedPawns?.ToHashSet<Pawn>()),
                pawnMarketValue = pawn.MarketValue,
                records = pawn.records.records,
                faction = pawn.Faction,
                isPrisoner = pawn.IsPrisoner,
                isSlave = pawn.IsSlave
            };
            using (HashSet<Pawn>.Enumerator enumerator2 = soul.relatedPawns.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    Pawn otherPawn = enumerator2.Current;
                    using (IEnumerator<PawnRelationDef> enumerator3 = pawn.GetRelations(otherPawn).GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            PawnRelationDef rel2 = enumerator3.Current;
                            bool flag = (from r in soul.relations
                                         where r.def == rel2 && r.otherPawn == otherPawn
                                         select r).Count<DirectPawnRelation>() == 0;
                            if (flag && !rel2.implied)
                            {
                                soul.relations.Add(new DirectPawnRelation(rel2, otherPawn, 0));
                            }
                        }
                    }
                    soul.relatedPawns.Add(otherPawn);
                }
            }
        //    Log.Message($"setting Soul.priorities for {pawn.Name}");
            soul.priorities = new Dictionary<WorkTypeDef, int>();
            if (pawn.workSettings != null && pawn.workSettings.priorities != null)
            {
                foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
                {
                    soul.priorities[workTypeDef] = pawn.workSettings.GetPriority(workTypeDef);
                }
            }
        //    Log.Message($"setting ideologyActive for {pawn.Name}");
            bool ideologyActive = ModsConfig.IdeologyActive;
            if (ideologyActive)
            {
                if (pawn.Ideo is Ideo ideo && pawn.ideo is Pawn_IdeoTracker tracker)
                {
                    soul.ideo = ideo;
                    soul.certainty = tracker.Certainty;
                    Precept_Role role = ideo.GetRole(pawn);
                    if (role is Precept_RoleMulti precept_RoleMulti)
                    {
                        soul.precept_RoleMulti = precept_RoleMulti;
                        soul.precept_RoleSingle = null;
                    }
                    else
                    {
                        if (role is Precept_RoleSingle precept_RoleSingle)
                        {
                            soul.precept_RoleMulti = null;
                            soul.precept_RoleSingle = precept_RoleSingle;
                        }
                    }
                }
                if (story != null && story.favoriteColor != null)
                {
                    soul.favColor = new Color?(story.favoriteColor.Value);
                }
            }
            return soul;
        }

        public static Pawn applySoulData(SoulStone stone, Pawn pawn)
        {
            Soul soul = stone.soul;
            pawn.skills.skills.Clear();
            if (soul.skills != null)
            {
                foreach (SkillRecord skillRecord in soul.skills)
                {
                    SkillRecord skillRecord2 = new SkillRecord(pawn, skillRecord.def);
                    skillRecord2.passion = skillRecord.passion;
                    skillRecord2.levelInt = skillRecord.levelInt;
                    skillRecord2.xpSinceLastLevel = skillRecord.xpSinceLastLevel;
                    skillRecord2.xpSinceMidnight = skillRecord.xpSinceMidnight;
                    pawn.skills.skills.Add(skillRecord2);
                }
            }
            if (soul.childhood != null)  pawn.story.Childhood = soul.childhood;
            if (soul.adulthood != null) pawn.story.Adulthood = soul.adulthood;
            pawn.story.traits.allTraits = soul.traits;
            if (soul.relatedPawns != null)
            {
                foreach (Pawn pawn2 in soul.relatedPawns)
                {
                    if (pawn2 != null)
                    {
                        foreach (DirectPawnRelation directPawnRelation in pawn2.relations.DirectRelations)
                        {
                            Name name = soul.name;
                            Pawn otherPawn = directPawnRelation.otherPawn;
                            if (name == (otherPawn?.Name)) directPawnRelation.otherPawn = pawn;
                        }
                    }
                }
                foreach (Pawn pawn3 in soul.relatedPawns)
                {
                    if (pawn3 != null)
                    {
                        foreach (DirectPawnRelation directPawnRelation2 in soul.relations)
                        {
                            foreach (DirectPawnRelation directPawnRelation3 in pawn3.relations.DirectRelations)
                            {
                                if (directPawnRelation2.def == directPawnRelation3.def)
                                {
                                    Pawn otherPawn2 = directPawnRelation3.otherPawn;
                                    if ((otherPawn2?.Name) == pawn.Name)
                                    {
                                        directPawnRelation3.otherPawn = pawn;
                                    }
                                }
                            }
                        }
                    }
                }
                using (List<DirectPawnRelation>.Enumerator enumerator7 = soul.relations.GetEnumerator())
                {
                    while (enumerator7.MoveNext())
                    {
                        DirectPawnRelation rel = enumerator7.Current;
                        if (rel.otherPawn != null)
                        {
                            DirectPawnRelation directPawnRelation4 = (from r in rel.otherPawn.relations.DirectRelations
                                                                      where r.def == rel.def && r.otherPawn.Name == pawn.Name
                                                                      select r).FirstOrDefault<DirectPawnRelation>();
                            if (directPawnRelation4 != null)
                            {
                                directPawnRelation4.otherPawn = pawn;
                            }
                        }
                        pawn.relations.AddDirectRelation(rel.def, rel.otherPawn);
                    }
                }
            }
            pawn.Name = soul.name;
            pawn.workSettings.pawn ??= pawn;
            /*
            Traverse traverse = Traverse.Create(pawn.workSettings).Field("pawn");
            if (traverse.GetValue() == null)
            {
                traverse.SetValue(pawn);
            }
            */
            pawn.workSettings.priorities ??= new DefMap<WorkTypeDef, int>();
            /*
            Traverse traverse2 = Traverse.Create(pawn.workSettings).Field("priorities");
            if (traverse2.GetValue() == null)
            {
                traverse2.SetValue(new DefMap<WorkTypeDef, int>());
            }
            */
            if (soul.priorities != null)
            {
                foreach (WorkTypeDef workTypeDef in soul.priorities.Keys)
                {
                    pawn.workSettings.SetPriority(workTypeDef, soul.priorities[workTypeDef]);
                }
            }
            if (pawn.records == null)
            {
                pawn.records = new Pawn_RecordsTracker(pawn);
            }
            if (soul.records != null)
            {
                pawn.records.records = soul.records;
            //    Traverse.Create(pawn.records).Field("records").SetValue(soul.records);
            }
            if (soul.faction != null && pawn.Faction != soul.faction)
            {
                pawn.SetFaction(soul.faction, null);
            }
            if (soul.isPrisoner)
            {
                if (pawn.guest.Released)
                {
                    pawn.guest.Released = false;
                    pawn.guest.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
                    GenGuest.RemoveHealthyPrisonerReleasedThoughts(pawn);
                }
                if (!pawn.IsPrisonerOfColony)
                {
                    pawn.guest.SetGuestStatus(soul.faction, GuestStatus.Prisoner);
                }
            }
            else
            {
                if (!soul.isPrisoner && pawn.IsPrisoner)
                {
                    GenGuest.PrisonerRelease(pawn);
                }
            }
            if (ModsConfig.IdeologyActive)
            {
                if (soul.precept_RoleMulti != null)
                {
                    if (soul.precept_RoleMulti.chosenPawns == null)
                    {
                        soul.precept_RoleMulti.chosenPawns = new List<IdeoRoleInstance>();
                    }
                    soul.precept_RoleMulti.chosenPawns.Add(new IdeoRoleInstance(soul.precept_RoleMulti)
                    {
                        pawn = pawn
                    });
                    soul.precept_RoleMulti.FillOrUpdateAbilities();
                }
                if (soul.precept_RoleSingle != null)
                {
                    soul.precept_RoleSingle.chosenPawn = new IdeoRoleInstance(soul.precept_RoleMulti)
                    {
                        pawn = pawn
                    };
                    soul.precept_RoleSingle.FillOrUpdateAbilities();
                }
                if (soul.ideo != null)
                {
                    pawn.ideo.SetIdeo(soul.ideo);
                    pawn.ideo.certaintyInt = soul.certainty;
                //    Traverse.Create(pawn.ideo).Field("certainty").SetValue(soul.certainty);
                }
                if (soul.favColor != null)
                {
                    pawn.story.favoriteColor = new Color?(soul.favColor.Value);
                }
                if (soul.isSlave)
                {
                    pawn.guest.SetGuestStatus(soul.faction, GuestStatus.Slave);
                }
                else
                {
                    if (pawn.IsSlave && !soul.isSlave)
                    {
                        InteractionWorker_RecruitAttempt.DoRecruit(null, pawn, true);
                    }
                }
            }
            return pawn;
        }
    }
}
