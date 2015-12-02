using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuBMParser
{
    public class TimingPoint
    {

        #region fields
        public int offset { get; set; }
        public float msPerBeat { get; set; }
        public int meter { get; set; }
        public int sampleType { get; set; }
        public int sampleSet { get; set; }
        public int volume { get; set; }
        public bool inherited { get; set; }
        public bool kiaiMode { get; set; }
        #endregion

        #region constructors
        public TimingPoint() { }

        public TimingPoint(int offset, float msPerBeat, int meter, int sampleType, int sampleSet, int volume, bool inherited, bool kiaiMode)
        {
            this.offset = offset;
            this.msPerBeat = msPerBeat;
            this.meter = meter;
            this.sampleType = sampleType;
            this.sampleSet = sampleSet;
            this.volume = volume;
            this.inherited = inherited;
            this.kiaiMode = kiaiMode;
        }
        #endregion

    }
}
