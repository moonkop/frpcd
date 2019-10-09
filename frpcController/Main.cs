using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace frpcController
{
    public partial class Main : Form
    {
        private IniHelper iniFileHelper = new IniHelper();
        private System.Timers.Timer checkTimer = new System.Timers.Timer();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            checkTimer.Elapsed += (s, e1) =>
            {
                updateAndCheck();
            };
        }
        private string getConfigFromUrl(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            var response = req.GetResponse() as HttpWebResponse;
            response.GetResponseStream();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
            {
                return reader.ReadToEnd();
            }
        }
        private string  readFromIni(string sector,string key)
        {
            StringBuilder sb = new StringBuilder(100);
            iniFileHelper.GetIniString(sector, key, "", sb, sb.Capacity);
            return sb.ToString();
        }
        private void updateAndCheck()
        {
            string url = this.readFromIni("config", "url");
            int checkInterval = int.Parse(this.readFromIni("config", "check_interval"));
             if (checkInterval != this.checkTimer.Interval)
            {
                checkTimer.Interval = checkInterval * 1000;
                checkTimer.Start();
            }

            string str = getConfigFromUrl(url);
            MessageBox.Show(str);
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            MessageBox.Show(obj["name"].ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {

            updateAndCheck();
 
        }
    }
}
