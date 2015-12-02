using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace osuBMParser
{
    internal class OsuFileParser
    {

        private enum OsuFileSection
        {
            NULL,
            FORMAT,
            GENERAL,
            EDITOR,
            METADATA,
            DIFFICULTY,
            EVENTS,
            TIMINGPOINTS,
            COLOURS,
            HITOBJECTS
        }

        #region fields
        private Beatmap beatmap;
        private string path;
        #endregion

        #region constructors
        public OsuFileParser(string path, Beatmap beatmap)
        {
            this.path = path;
            this.beatmap = beatmap;
        }
        #endregion

        #region methods
        public void parse()
        {

            //Read in file. Exceptions here are to be handled by the devs who use this library.
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (IOException)
            {
                throw;
            }

            //First line is always file format version
            OsuFileSection currentSection = OsuFileSection.FORMAT;

            foreach (string line in lines)
            {
                //Skip line if empty
                if (!string.IsNullOrWhiteSpace(line))
                {
                    //Test for new section, otherwise, parse normally.
                    OsuFileSection sectionTest = testNewSection(line);
                    if (sectionTest != OsuFileSection.NULL)
                    {
                        currentSection = sectionTest;
                    }
                    else
                    {
                        parseLine(currentSection, line);
                    }
                }
            }

            //TODO:
            //Calculate time values for HitSliderSegments. First do some research on brezier curves for this :p

            Debug.WriteLine("osuBMParser: Finished beatmap parsing");

        }

        private OsuFileSection testNewSection(string data)
        {
            OsuFileSection sectionEnum;
            return Enum.TryParse(data.Substring(1, data.Length - 2), true, out sectionEnum) ? sectionEnum : OsuFileSection.NULL;

        }

        //Send line data to the right parse method
        private void parseLine(OsuFileSection section, string data)
        {
            switch (section)
            {
                case OsuFileSection.FORMAT:
                    beatmap.formatVersion = data;
                    break;
                case OsuFileSection.GENERAL:
                case OsuFileSection.EDITOR:
                case OsuFileSection.METADATA:
                case OsuFileSection.DIFFICULTY:
                    normalParse(data);
                    break;
                case OsuFileSection.TIMINGPOINTS:
                    timingPointParse(data);
                    break;
                case OsuFileSection.COLOURS:
                    colourParse(data);
                    break;
                case OsuFileSection.HITOBJECTS:
                    hitObjectParse(data);
                    break;
            }
        }

        #region parseMethods
        private void normalParse(string data)
        {
            string[] tokens = data.Split(':');

            switch (tokens[0].ToLower().Trim())
            {
                //Different parsing method (list)
                case "bookmarks":
                    beatmap.bookmarks.AddRange(Array.ConvertAll(tokens[1].Split(','), int.Parse));
                    break;
                //Different parsing method (list)
                case "tags":
                    if (tokens[1] != null) beatmap.tags.AddRange(tokens[1].Split(' '));
                    break;
                default:
                    //Use reflection to set property values
                    PropertyInfo property = beatmap.GetType().GetProperty(tokens[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        //Convert.ChangeType() does not do string to boolean conversion
                        if (property.PropertyType == typeof(Boolean))
                        {
                            property.SetValue(beatmap, toBool(tokens[1].Trim()));
                        }
                        else
                        {
                            property.SetValue(beatmap, Convert.ChangeType(tokens[1].Trim(), property.PropertyType));
                        }
                    }
                    else
                    {
                        Debug.WriteLine("osuBMParser: Undefined property: " + tokens[0]);
                    }
                    break;
            }

        }


        private void timingPointParse(string data)
        {
            //throw new NotImplementedException();

            string[] tokens = data.Split(',');

            tokens = getArrayWithSize(tokens, 8);

            beatmap.timingPoints.Add(new TimingPoint(
                toInt(tokens[0]),
                toFloat(tokens[1]),
                toInt(tokens[2]),
                toInt(tokens[3]),
                toInt(tokens[4]),
                toInt(tokens[5]),
                toBool(tokens[6]),
                toBool(tokens[7])));

        }

        private void colourParse(string data)
        {
            if (data.Trim() != "")
            {
                string[] tokens = data.Split(':')[1].Split(',');
                beatmap.colours.Add(new ComboColour(byte.Parse(tokens[0]), byte.Parse(tokens[1]), byte.Parse(tokens[2])));
            }
        }

        private void hitObjectParse(string data)
        {

            string[] tokens = data.Split(',');

            //These are at the same indexes for all types of HitObjects
            Vector2 position = new Vector2(toFloat(tokens[0]), toFloat(tokens[1]));
            int time = toInt(tokens[2]);
            int hitSound = toInt(tokens[4]);

            switch (toInt(tokens[3]))
            {
                case 1:
                case 5:

                    tokens = getArrayWithSize(tokens, 6);

                    beatmap.hitObjects.Add(new HitCircle(
                    position,
                    time,
                    hitSound,
                    getAdditionsAsIntArray(tokens[5]),
                    toInt(tokens[3]) == 5 ? true : false));
                    break;

                case 2:
                case 6:

                    tokens = getArrayWithSize(tokens, 11);

                    //Gets a list with all HitSliderSegments and SliderType as strings
                    List<HitSliderSegment> hitSliderSegments = new List<HitSliderSegment>();
                    string[] hitSliderSegmentPositions = tokens[5].Split('|');

                    //First element is SliderType
                    HitSlider.SliderType sliderType = HitSlider.parseSliderType(hitSliderSegmentPositions[0]);

                    //Loop to get all the HitSliderSegment objects (skip first element (SliderType))
                    foreach (string hitSliderSegmentPosition in hitSliderSegmentPositions.Skip(1))
                    {
                        if (!string.IsNullOrWhiteSpace(hitSliderSegmentPosition))
                        {
                            string[] pos = hitSliderSegmentPosition.Split(':');
                            if (pos.Length == 2) hitSliderSegments.Add(new HitSliderSegment(new Vector2(toFloat(pos[0]), toFloat(pos[1]))));
                        }
                    }

                    beatmap.hitObjects.Add(new HitSlider(
                    position,
                    time,
                    hitSound,
                    sliderType,
                    hitSliderSegments.ToArray(),
                    toInt(tokens[6]),
                    toFloat(tokens[7]),
                    toInt(tokens[8]),
                    getAdditionsAsIntArray(tokens[9]),
                    getAdditionsAsIntArray(tokens[10]),
                    toInt(tokens[3]) == 6 ? true : false));
                    break;

                case 8:
                case 12:

                    tokens = getArrayWithSize(tokens, 7);

                    beatmap.hitObjects.Add(new HitSpinner(
                    position,
                    time,
                    hitSound,
                    toInt(tokens[5]),
                    getAdditionsAsIntArray(tokens[6]),
                    toInt(tokens[3]) == 12 ? true : false));
                    break;

                default:
                    Debug.Write("osuBMParser: Invalid HitObject line at timestamp: " + tokens[2] + " | Type = " + tokens[3]);
                    break;

            }
        }

        private int[] getAdditionsAsIntArray(string additionToken)
        {

            int[] additions = new int[0];
            try
            {
                additions = Array.ConvertAll(additionToken.Split(':'), int.Parse);
            }
            catch { }
            return additions;

        }
        #endregion

        private string[] getArrayWithSize(string[] data, int size)
        {
            if (data.Length > size)
            {
                return new List<string>(data).GetRange(0, size - 1).ToArray();
            }
            else
            {
                return new List<string>(data).Concat(Enumerable.Repeat("0", size - data.Length)).ToArray();
            }

        }

        private int toInt(string data)
        {
            int result;
            return int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : 0;
        }

        private float toFloat(string data)
        {
            float result;
            return float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? result : 0f;
        }

        private bool toBool(string data)
        {
            return (data.Trim() == "1" || data.Trim().ToLower() == "true");
        }
        #endregion

    }
}

/*
Number of arguments per line:

    - Timingpoint: 8
    - Circle: 6
    - Slider: 11
    - Spinner: 7
    
*/
