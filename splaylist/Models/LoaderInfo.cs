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


        // [Flags]   // no good for StatusString 
        public enum Stage
        {
            None = 0,
            Tracks = 1,
            Artists = 2,
            Analysis = 4,
            Albums = 8,
            Done = 15
        }

        private Stage _loaderStatus;

        public Stage LoaderStage { 
            get { return _loaderStatus; } 
            internal set
            {
                // clear count when changing stage
                Loaded = 0;
                Available = 0;
                _loaderStatus = value;
            }
        }

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


        // so count can be added without necessarily causing a null reference exception
        public int AddLoaded(int count)
        {
            return Loaded += count;
        }

    }
}
