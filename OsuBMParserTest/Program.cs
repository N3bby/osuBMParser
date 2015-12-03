using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using osuBMParser;

using System.Reflection;

namespace OsuBMParserTest
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();

            //Beatmap beatmap = new Beatmap("//192.168.0.79/razacx/home/razacx/osubeatmaps/6687 Hatsune Miku - World is Mine/Hatsune Miku - World is Mine (tom800510) [Pudding Lover!!].osu");
            Beatmap beatmap = new Beatmap("//192.168.0.79/razacx/home/razacx/osubeatmaps/83560 DJ S3RL - T-T-Techno (feat Jesskah)/DJ S3RL - T-T-Techno (feat. Jesskah) (nold_1702) [Technonationalism].osu");
            ((PropertyGrid)form.Controls.Find("propertyGrid1", true)[0]).SelectedObject = beatmap;

            Application.Run(form);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

    }
}
