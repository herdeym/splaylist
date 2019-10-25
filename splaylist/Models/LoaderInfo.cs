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
            Done = 15
        }

        public Stage LoaderStage { get; internal set; }

        public string StatusString { get
            {
                switch (LoaderStage)
                {
                    case Stage.None:
                        return "Getting Ready...";
                    case Stage.Tracks:
                        return $"Loaded {Loaded} out of {Available} tracks";
                    case Stage.Artists:
                        return $"Loaded {Loaded} out of {Available} artists.";
                    case Stage.Analysis:
                        return $"Analysed {Loaded} of {Available} tracks";
                    case Stage.Albums:
                        return $"Loaded {Loaded} of {Available} albums";
                    case Stage.Done:
                    default:
                        return "Loading...";
                }

            } 
        }

    }
}
