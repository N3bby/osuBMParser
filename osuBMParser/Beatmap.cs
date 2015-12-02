using System;
using System.Collections.Generic;

namespace osuBMParser
{
    public class Beatmap
    {

        //Test

        #region fields
        #region extra
        public string formatVersion { get; set; }
        #endregion

        #region general        
        public string audioFileName { get; set; }
        public int audioLeadIn { get; set; }
        public int previewTime { get; set; }
        public bool countdown { get; set; }
        public string sampleSet { get; set; }
        public float stackLeniency { get; set; }
        public int mode { get; set; }
        public bool letterBoxInBreaks { get; set; }
        public bool widescreenStoryboard { get; set; }
        #endregion

        #region editor
        public List<int> bookmarks { get; set; }
        public float distanceSpacing { get; set; }
        public int beatDivisor { get; set; }
        public int gridSize { get; set; }
        public int timelineZoom { get; set; }
        #endregion

        #region metadata
        public string title { get; set; }
        public string artist { get; set; }
        public string creator { get; set; }
        public string version { get; set; }
        public string source { get; set; }
        public List<string> tags { get; set; }        
        public int beatmapID { get; set; }
        public int beatmapSetID { get; set; }
        #endregion

        #region difficulty
        public float hpDrainRate { get; set; }
        public float circleSize { get; set; }
        public float overallDifficulty { get; set; }
        public float approachRate { get; set; }
        public float sliderMultiplier { get; set; }
        public float sliderTickRate { get; set; }
        #endregion

        #region events
        // I'll do this later :p
        #endregion

        #region timingPoints
        public List<TimingPoint> timingPoints { get; set; }        
        #endregion

        #region colours
        public List<ComboColour> colours { get; set; }      
        #endregion

        #region hitObjects 
        public List<HitObject> hitObjects { get; set; }
        #endregion
        #endregion

        #region constructors
        private Beatmap()
        {
            init();
        }

        public Beatmap(Beatmap beatmap)
        {
            //Needs to be done...
        }

        public Beatmap(string path) : this()
        {
            System.Diagnostics.Debug.WriteLine("Parsing: " + path);
            OsuFileParser parser = new OsuFileParser(path, this);
            parser.parse();
        }
        #endregion

        #region methods
        public void init()
        {
            bookmarks = new List<int>();
            tags = new List<string>();
            timingPoints = new List<TimingPoint>();
            colours = new List<ComboColour>();
            hitObjects = new List<HitObject>();
        }
        #endregion

    }
}
