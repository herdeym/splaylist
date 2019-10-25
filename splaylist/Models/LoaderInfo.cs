using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Models
{
    public class LoaderInfo
    {
        public int Loaded { get; internal set; }
        public int Available { get; internal set; }


        [Flags]
        public enum Stage
        {
            None = 0,
            Tracks = 1,
            Artists = 2,
            Analysis = 4,
            Albums = 8,
            All = Tracks & Artists & Analysis & Albums
        }

        public Stage LoaderStage { get; internal set; }

    }
}
