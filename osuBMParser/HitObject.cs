using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuBMParser
{
    public abstract class HitObject
    {

        #region fields
        public Vector2 position { get; set; }
        public int time { get; set; }
        public int hitSound { get; set; }
        public List<int> addition { get; set; }
        public bool isNewCombo { get; set; }
        #endregion

        #region constructors
        public HitObject(Vector2 position, int time, int hitSound, int[] addition, bool isNewCombo)
        {
            init();
            this.position = position;
            this.time = time;
            this.hitSound = hitSound;
            this.addition.AddRange(addition);
            this.isNewCombo = isNewCombo;
        }
        #endregion

        #region methods
        private void init()
        {
            position = new Vector2();
            addition = new List<int>();
        }
        #endregion

    }
}
