using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuBMParser
{
    public class HitSlider : HitObject
    {

        public enum SliderType
        {
            NULL,
            BREZIER,
            CATMULL,
            LINEAR
        }

        #region fields
        public SliderType sliderType { get; set; }
        List<HitSliderSegment> hitSliderSegments { get; set; }
        public int repeat { get; set; }
        public float pixelLength { get; set; }
        public int edgeHitSound { get; set; }
        public List<int> edgeAddition { get; set; }
        #endregion

        #region constructors
        public HitSlider(Vector2 position, int time, int hitSound, SliderType sliderType, HitSliderSegment[] hitSliderSegments, int repeat, float pixelLength, int edgeHitSound, int[] edgeAddition, int[] addition, bool isNewCombo) : base(position, time, hitSound, addition, isNewCombo)
        {
            init();
            this.sliderType = sliderType;
            this.hitSliderSegments.AddRange(hitSliderSegments);
            this.repeat = repeat;
            this.pixelLength = pixelLength;
            this.edgeHitSound = edgeHitSound;
            this.edgeAddition.AddRange(edgeAddition);
        }
        #endregion

        #region methods
        private void init()
        {
            hitSliderSegments = new List<HitSliderSegment>();
            edgeAddition = new List<int>();
        }

        public static SliderType parseSliderType(string data)
        {
            switch (data.Trim().ToLower())
            {
                case "b":
                    return SliderType.BREZIER;
                case "c":
                    return SliderType.CATMULL;
                case "l":
                    return SliderType.LINEAR;
                default:
                    return SliderType.NULL;
            }
        }
        #endregion

    }
}
