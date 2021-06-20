using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdeptusMechanicus
{
    public class DarkEldar_Pain_Tracker : MapComponent
    {
        public DarkEldar_Pain_Tracker(Map map) : base(map)
        {
            darkEldar = new List<Pawn>();
            painMaker = new List<Pawn>();
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            checkTick++;
            if (checkTick >= checkInterval)
            {
                checkTick = 0;
                for (int i = 0; i < painMaker.Count; i++)
                {
                    Pawn p = painMaker[i];
                    if (p == null)
                    {
                        painMaker.RemoveAt(i);
                        i--;
                        continue;
                    }
                    if (p.Dead)
                    {
                        painMaker.RemoveAt(i);
                        i--;
                        continue;
                    }
                    if (p.Map == null)
                    {
                        if (p.MapHeld != null)
                        {
                            if (p.MapHeld != map)
                            {
                                painMaker.RemoveAt(i);
                                i--;
                                continue;
                            }
                        }
                        else
                        {
                            painMaker.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    if (p.Map != this.map)
                    {
                        if (p.MapHeld != null)
                        {
                            if (p.MapHeld != map)
                            {
                                painMaker.RemoveAt(i);
                                i--;
                                continue;
                            }
                        }
                        else
                        {
                            painMaker.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                }
                totalPain = TotalPain;
            }
        }

        public float TotalPain
        {
            get
            {
                float num = 0f;
                for (int i = 0; i < painMaker.Count; i++)
                {
                    num += painMaker[i].health.hediffSet.PainTotal;
                }
                return num;
            }
        }

        public List<Pawn> darkEldar;
        public List<Pawn> painMaker;
        public float totalPain;
        public int checkTick = 0;
        private const int checkInterval = 150;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref checkTick, "checkTick", 0);
            Scribe_Values.Look(ref totalPain, "totalPain", 0f);
        }
    }
}
