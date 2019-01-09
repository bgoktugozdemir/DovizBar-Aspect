using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Castle.DynamicProxy;
using DovizBar.Aspects;
using HtmlAgilityPack;
using Newtonsoft.Json;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace DovizBar
{
    public partial class Form1 : Form
    {
        #region Variables

        /* THIS CLASS */
        public static Form1 form1;
        public static Form4 form4;
        private ProxyGenerator form4ProxyGenerator;

        /* DOUBLE */
        private double averageDolar = 0;
        private double averageEuro = 0;

        /* INT */
        private int updateCount = 1; //Thread -> 1, Normal -> 2
        private int position = 0;

        /* STRING */
        private string arrowUp = "▲";
        private string arrowDown = "▼";
        public string oldDolar;
        public string oldEuro;
        public static string currentDolar;
        public static string currentEuro;
        private string html = String.Empty;

        /* BOOL */
        private bool alert = false;
        private bool funMode = false;
        public static bool IsErrorMode = false;

        /* COLOR */
        private Color defaultCurrencyColor = Color.Gold;
        public Color upColor = Color.GreenYellow;
        public Color downColor = Color.Red;
        //private Color RefreshColor = Color.Yellow;
        private Color defaultTimeColor = Color.White;
        private Color dolarColor;
        private Color euroColor;
        public static Color CurrentDolarColor;
        public static Color CurrentEuroColor;
            
        /* THREAD */
        private Thread threadPull;

        /* SOUND */
        private SoundPlayer alertSound = new SoundPlayer(Properties.Resources.AlertSound_2);
        private SoundPlayer alertGoalSound = new SoundPlayer(Properties.Resources.AlertSound);

        /* SCREEN */
        private Screen scr;

        //private Uri url = new Uri(@"https://www.widgets.investing.com/single-currency-crosses?theme=darkTheme&currency=9");
        private Uri url = new Uri(
            @"https://www.widgets.investing.com/live-currency-cross-rates?theme=lightTheme&hideTitle=true&pairs=66,18
                                    ""%20width=""100%""%20height=""100%""%20frameborder=""0""%20allowtransparency=""true""%20marginwidth=""0""%20marginheight=""0"">
                                    </iframe><div%20class=""poweredBy""%20style=""font-family:%20Arial,%20Helvetica,%20sans-serif;");

        private HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        private WebClient client;

        #endregion

        public Form1()
        {
            form1 = this;
            form4 = new Form4();
            form4ProxyGenerator = new ProxyGenerator();
            form4 = form4ProxyGenerator.CreateClassProxy<Form4>(new Form4InterceptorAspect());

            Updater();
            if (CheckVersion())
            {
                Application.Exit();
                this.Close();
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = MinimumSize;

            txtErrorDolar.Visible = false;
            txtErrorEuro.Visible = false;

            rowDolar.Text = "";
            rowEuro.Text = "";
            dolarColor = txtDolar.ForeColor;
            euroColor = txtEuro.ForeColor;

            scr = Screen.FromPoint(this.Location);
            this.Location = new Point(scr.WorkingArea.Right - this.Width, scr.WorkingArea.Top);

            PrepareThread();
            threadPull.IsBackground = true;
            threadPull.Start();

            SetRefreshTime();
            timer1.Enabled = true;
            txtDolar.BackColor = Color.Transparent;
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            position++;
            ChangePosition();
        }

        private void PrepareThread()
        {
            threadPull = new Thread(PullData);
            ThreadPool.QueueUserWorkItem(new WaitCallback(PullData), threadPull);
        }

        public void ChangePosition()
        {
           PositionChanger();
        }

        private void PositionChanger()
        {
            if (position % 4 == 0)
                Location = new Point(scr.WorkingArea.Right - Width, scr.WorkingArea.Top);
            else if (position % 4 == 1)
                Location = new Point(scr.WorkingArea.Right - Width, scr.WorkingArea.Bottom - Height);
            else if (position % 4 == 2)
                Location = new Point(scr.WorkingArea.Left, scr.WorkingArea.Bottom - Height);
            else if (position % 4 == 3)
                Location = new Point(scr.WorkingArea.Left, scr.WorkingArea.Top);
        }

        public delegate void DelegateUpdateScreen();

        [CurrencyChangedAspect]
        private void UpdateScreen(string dolar, string percentDolar, string minDolar, string maxDolar,
                                  string euro, string percentEuro, string minEuro, string maxEuro,
                                  string dolarBuy, string dolarSell, string euroBuy, string euroSell)
        {
            txtDolar.Text = dolar;
            txtEuro.Text = euro;

            txtMinDolar.Text = minDolar;
            txtMaxDolar.Text = maxDolar;
            this.percentDolar.Text = percentDolar;

            txtMinEuro.Text = minEuro;
            txtMaxEuro.Text = maxEuro;
            this.percentEuro.Text = percentEuro;

            if (percentDolar[0] == '+')
            {
                CurrentDolarColor = upColor;
            }
            else if (percentDolar[0] == '-')
            {
                CurrentDolarColor = downColor;

            }

            if (percentEuro[0] == '+')
            {
                CurrentEuroColor = upColor;
            }
            else if (percentEuro[0] == '-')
            {
                CurrentEuroColor = downColor;
            }

            lblDolarBuy.Text = dolarBuy;
            lblDolarSell.Text = dolarSell;
            lblEuroBuy.Text = euroBuy;
            lblEuroSell.Text = euroSell;

            if (Convert.ToDouble(dolar.Replace('.', ',')) > 0) averageDolar += Convert.ToDouble(dolar.Replace('.', ','));
            if (Convert.ToDouble(euro.Replace('.', ',')) > 0) averageEuro += Convert.ToDouble(euro.Replace('.', ','));
            txtAverageDolar.Text = (averageDolar / updateCount).ToString(".0000");
            txtAverageEuro.Text = (averageEuro / updateCount).ToString(".0000");

            updateCount++;
            txtLastUpdate.Text = DateTime.Now.ToLongTimeString();

            this.percentDolar.ForeColor = CurrentDolarColor;
            this.percentEuro.ForeColor = CurrentEuroColor;
            timer2.Enabled = true;
        }

        [UpdaterAspect]
        private void UpdateDoviz()
        {
            try
            {
                client = new WebClient();
                client.Headers.Add("User-Agent: Other");
                html = client.DownloadString(url);
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);
                HtmlNodeCollection Currency =
                    document.DocumentNode.SelectNodes("//div");
                /*
                HtmlNodeCollection dovizler = document.DocumentNode.SelectNodes("//span[@class='menu-row2']");
                UpdateScreen(dovizler[1].InnerText, dovizler[2].InnerText);
                */
                if (Currency.Count < 33)
                {
                    return;
                }

                if (InvokeRequired)
                {
                    BeginInvoke(new DelegateUpdateScreen(() => UpdateScreen(
                        Currency[26].InnerText, Currency[31].InnerText, Currency[29].InnerText, Currency[28].InnerText,
                        Currency[16].InnerText, Currency[21].InnerText, Currency[19].InnerText, Currency[18].InnerText,
                        Currency[24].InnerText, Currency[25].InnerText, Currency[14].InnerText,
                        Currency[15].InnerText)));
                }
                else
                {
                    UpdateScreen(
                        Currency[26].InnerText, Currency[31].InnerText, Currency[29].InnerText, Currency[28].InnerText,
                        Currency[16].InnerText, Currency[21].InnerText, Currency[19].InnerText, Currency[18].InnerText,
                        Currency[24].InnerText, Currency[25].InnerText, Currency[14].InnerText, Currency[15].InnerText);
                }

                txtLastUpdate.ForeColor = defaultTimeColor;
            }
            catch(Exception e)
            {
                Form1.form1.ErrorMode();
                Debug.WriteLine("HATA MESAJI: " + this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " "+e.Message);
                if (Form1.form1.InvokeRequired)
                {
                    Form1.form1.BeginInvoke(new Form1.DelegateUpdateScreen(Form1.form1.ErrorMode));
                }
                else
                {
                    Form1.form1.ErrorMode();
                }
            }
        }

        [GoalAspect]
        private void CheckGoal()
        {
            var dolarGoal = Properties.Settings.Default.DolarGoal;
            var euroGoal = Properties.Settings.Default.EuroGoal;
            var isDolarUp = Properties.Settings.Default.DolarGoalBool;
            var isEuroUp = Properties.Settings.Default.EuroGoalBool;
            var currentDolar = Convert.ToDouble(txtDolar.Text.Replace('.', ','));
            var currentEuro = Convert.ToDouble(txtEuro.Text.Replace('.', ','));

            if (dolarGoal != 0)
            {
                if (dolarGoal > currentDolar)
                {
                    if (!isDolarUp)
                    {
                        Form3.ResetGoal("USD");
                        alertGoalSound.Play();
                        MessageBox.Show(
                            $"Dolar beklediğiniz seviyeye düştü. \n{DateTime.Now} => ({dolarGoal.ToString()} ▼ {currentDolar.ToString()})",
                            "▼ DOLAR DÜŞTÜ ▼");
                    }
                }
                else if (dolarGoal < currentDolar)
                {
                    if (isDolarUp)
                    {
                        Form3.ResetGoal("USD");
                        alertGoalSound.Play();
                        MessageBox.Show(
                            $"Dolar beklediğiniz düzeye ulaştı. \n{DateTime.Now} => ({dolarGoal.ToString()} ▲ {currentDolar.ToString()})",
                            "▲ DOLAR YÜKSELDİ ▲");
                    }
                }
            }

            if (euroGoal != 0)
            {
                if (euroGoal > currentEuro)
                {
                    if (!isEuroUp)
                    {
                        Form3.ResetGoal("EURO");
                        alertGoalSound.Play();
                        MessageBox.Show(
                            $"Euro beklediğiniz seviyeye düştü. \n{DateTime.Now} => ({euroGoal.ToString()} - {currentEuro.ToString()})",
                            "▼ EURO DÜŞTÜ ▼");
                    }
                }
                else if (euroGoal < currentEuro)
                {
                    if (isEuroUp)
                    {
                        Form3.ResetGoal("EURO");
                        alertGoalSound.Play();
                        MessageBox.Show(
                            $"Euro beklediğiniz düzeye ulaştı. \n{DateTime.Now} => ({euroGoal.ToString()} - {currentEuro.ToString()})",
                            "▲ EURO YÜKSELDİ ▲");
                    }
                }
            }
        }

        [PullDataAspect]
        private void PullData(object obj)
        {
            UpdateDoviz();

            if (oldDolar == "null" || oldEuro == "null")
            {
                PullData(obj);
                return;
            }

            currentDolar = txtDolar.Text;
            currentEuro = txtEuro.Text;

            if (InvokeRequired)
            {
                BeginInvoke(new DelegateUpdateScreen(RefreshRow));
            }
            else
            {
                RefreshRow();
            }

            CheckGoal();
        }

        private void UpdateOldCurrency()
        {
            oldDolar = Get_txtDolar_Text();
            oldEuro = Get_txtEuro_Text();
        }

        public void UpdateCurrencyWithInvoke()
        {

            if (InvokeRequired)
            {
                BeginInvoke(new DelegateUpdateScreen(UpdateOldCurrency));
            }
            else
            {
                UpdateOldCurrency();
            }

        }

        [RefreshAspect]
        private void RefreshRow() //ExceptionHandler
        {
            double _oldDolar = Convert.ToDouble(oldDolar);
            double _oldEuro = Convert.ToDouble(oldEuro);
            double _currentDolar = Convert.ToDouble(currentDolar);
            double _currentEuro = Convert.ToDouble(currentEuro);

            if (_oldDolar < _currentDolar)
            {
                Change_rowDolar_Text(arrowUp);
                Change_rowDolar_ForeColor(upColor);
                Change_txtDolar_ForeColor(upColor);
                PlayAlertSound();
            }
            else if (_oldDolar > _currentDolar)
            {
                Change_rowDolar_Text(arrowDown);
                Change_rowDolar_ForeColor(downColor);
                Change_txtDolar_ForeColor(downColor);
                PlayAlertSound();
            }
            else
            {
                Change_rowDolar_Text("");
            }

            if (_oldEuro < _currentEuro)
            {
                Change_rowEuro_Text(arrowUp);
                Change_rowEuro_ForeColor(upColor);
                Change_txtEuro_ForeColor(upColor);
                PlayAlertSound();
            }
            else if (_oldEuro > _currentEuro)
            {
                Change_rowEuro_Text(arrowDown);
                Change_rowEuro_ForeColor(downColor);
                Change_txtEuro_ForeColor(downColor);
                PlayAlertSound();
            }
            else
            {
                Change_rowEuro_Text("");
            }
        }

        private void PlayAlertSound()
        {
            if (alert)
            {
                alertSound.Play();
            }
        }

        private void DefaultColor()
        {
            Change_txtDolar_ForeColor(dolarColor);
            Change_txtEuro_ForeColor(euroColor);
        }

        private void txtClose_MouseHover(object sender, EventArgs e)
        {
            txtClose.ForeColor = Color.Red;
        }

        private void txtClose_MouseLeave(object sender, EventArgs e)
        {
            txtClose.ForeColor = Color.White;
        }

        private void txtClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtMinimize_MouseHover(object sender, EventArgs e)
        {
            txtMinimize.ForeColor = Color.DarkOrange;
        }

        private void txtMinimize_MouseLeave(object sender, EventArgs e)
        {
            txtMinimize.ForeColor = Color.White;
        }

        private void txtMinimize_Click(object sender, EventArgs e)
        {
            form4.Show();
            //this.Hide(); //Castle Proxy
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!IsErrorMode)
            {
                PrepareThread();
                threadPull.Start();
            }
            else
            {
                timer3.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DefaultColor();
            timer2.Enabled = false;
        }

        [ErrorModeAspect]
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
        }

        [LastUpdateAspect]
        private void txtLastUpdate_Click(object sender, EventArgs e)
        {
            if (!IsErrorMode)
            {
                string notes = String.Empty;
                string notesTarget =
                    "https://raw.githubusercontent.com/bgoktugozdemir/VersionChecker/master/DovizBar/Notes.txt";
                using (WebClient client = new WebClient())
                {
                    notes = client.DownloadString(notesTarget);
                }

                MessageBox.Show(notes, $"v{Program.Version}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckVersion()
        {
            string parser = string.Empty;
            string versionTarget =
                "https://raw.githubusercontent.com/bgoktugozdemir/VersionChecker/master/version.json";

            try
            {
                using (WebClient client = new WebClient())
                {
                    parser = client.DownloadString(versionTarget);
                }

                var json = JsonConvert.DeserializeObject<JsonDoviz>(parser);
                Version CurrentVersion = Version.Parse(json.version);

                if (CurrentVersion > Program.Version)
                {
                    if (json.imperativeUpdate)
                    {
                        MessageBox.Show(
                            $"Yeni sürüm mevcut! Bu güncellemeyi indirmeniz gerekmekte. \n({Program.Version} => {CurrentVersion})",
                            "Zorunlu Güncelleme Mevcut", MessageBoxButtons.OK);
                        NewDownloader();
                        return true;
                    }
                    else
                    {
                        if (MessageBox.Show(
                                $"Yeni sürüm mevcut! Güncellemek ister misiniz? \n({Program.Version} => {CurrentVersion})",
                                "Güncelleme Mevcut", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            NewDownloader();
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (System.Net.WebException e)
            {
                MessageBox.Show(
                    "İnternet bağlantısı bulunamadı ya da Sistem başka bir uygulama tarafından kullanılıyor!\n" +
                    e.ToString(), "Bağlantı Yok", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void NewDownloader()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/bgoktugozdemir/VersionChecker/raw/master/DovizBar/DownloadFiles/DovizBar.exe", path + @"/temp.exe");

                Process setupProcess = Process.Start(path + @"/temp.exe");
                Thread thread = new Thread(() => setupProcess.Start());
                thread.Start();
            }
        }

        private void Updater()
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\DovizBar.exe";
            var pathTemp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\temp.exe";
            if (AppDomain.CurrentDomain.FriendlyName == "temp.exe")
            {
                File.Delete(path);
                File.Copy(pathTemp, path);
                MessageBox.Show("Güncelleme Başarıyla Tamamlandı!");
                Process process = Process.Start(path);
                Environment.Exit(0);
            }
            else if (AppDomain.CurrentDomain.FriendlyName == "DovizBar.exe")
            {
                if (File.Exists(pathTemp))
                    File.Delete(pathTemp);
            }
            else
            {
                NewDownloader();
            }
        }

        private void alertPrefer_Click(object sender, EventArgs e)
        {
            if (alert)
            {
                alertPrefer.ForeColor = Color.Red;
            }
            else
            {
                alertPrefer.ForeColor = Color.LawnGreen;
            }
            alert = !alert;
        }

        [ChangePositionAspect]
        private void btnExtend_Click(object sender, EventArgs e)
        {
            if (btnExtend.Text == "↓")
            {
                btnExtend.Text = "↑";
                this.Height = this.MaximumSize.Height;
                ChangePosition();
            }
            else if (btnExtend.Text == "↑")
            {
                btnExtend.Text = "↓";
                this.Height = this.MinimumSize.Height;
            }
        }

        private void btnSetUpdateTime_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();
        }

        public void SetRefreshTime()
        {
            timer1.Interval = Properties.Settings.Default.RefreshTime;
        }

        [ChangePositionAspect]
        private void lblLeft_Click(object sender, EventArgs e)
        {
            if (Width != this.MaximumSize.Width)
            {
                this.Width = this.MaximumSize.Width;
                lblLeft.Text = ">";
            }
            else
            {
                this.Width = this.MinimumSize.Width;
                lblLeft.Text = "<";
            }
        }

        public static void KeyControl(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtCompareDolar_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyControl(sender, e);
            if (e.KeyChar == (char)13)
            {
                if (txtDolar.Text != "null")
                {
                    if (rbGeneral.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * Convert.ToDouble(txtDolar.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareEuro.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * (Convert.ToDouble(txtDolar.Text.Replace('.', ',')) / Convert.ToDouble(txtEuro.Text.Replace('.', ',')))).ToString("0.####");
                    }
                    else if (rbBuy.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * Convert.ToDouble(lblDolarBuy.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareEuro.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * (Convert.ToDouble(lblDolarBuy.Text.Replace('.', ',')) / Convert.ToDouble(lblEuroBuy.Text.Replace('.', ',')))).ToString("0.####");
                    }
                    else if (rbSell.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * Convert.ToDouble(lblDolarSell.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareEuro.Text = (Convert.ToDouble(txtCompareDolar.Text.Replace('.', ',')) * (Convert.ToDouble(lblDolarSell.Text.Replace('.', ',')) / Convert.ToDouble(lblEuroSell.Text.Replace('.', ',')))).ToString("0.####");
                    }
                }
            }
        }

        private void txtCompareEuro_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyControl(sender, e);
            if (e.KeyChar == (char)13)
            {
                if (txtEuro.Text != "null")
                {
                    if (rbGeneral.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * Convert.ToDouble(txtEuro.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareDolar.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * (Convert.ToDouble(txtEuro.Text.Replace('.', ',')) / Convert.ToDouble(txtDolar.Text.Replace('.', ',')))).ToString("0.####");
                    }
                    else if (rbBuy.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * Convert.ToDouble(lblEuroBuy.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareDolar.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * (Convert.ToDouble(lblEuroBuy.Text.Replace('.', ',')) / Convert.ToDouble(lblDolarBuy.Text.Replace('.', ',')))).ToString("0.####");
                    }
                    else if (rbSell.Checked)
                    {
                        txtCompareTl.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * Convert.ToDouble(lblEuroSell.Text.Replace('.', ','))).ToString("0.####");
                        txtCompareDolar.Text = (Convert.ToDouble(txtCompareEuro.Text.Replace('.', ',')) * (Convert.ToDouble(lblEuroSell.Text.Replace('.', ',')) / Convert.ToDouble(lblDolarSell.Text.Replace('.', ',')))).ToString("0.####");
                    }
                }
            }
        }

        private void txtCompareTl_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyControl(sender, e);
            if (e.KeyChar == (char)13)
            {
                if (txtDolar.Text != "null" && txtEuro.Text != "null")
                {
                    txtCompareDolar.Text = (Convert.ToDouble(txtCompareTl.Text.Replace('.', ',')) / Convert.ToDouble(txtDolar.Text.Replace('.', ','))).ToString("0.####");
                    txtCompareEuro.Text = (Convert.ToDouble(txtCompareTl.Text.Replace('.', ',')) / Convert.ToDouble(txtEuro.Text.Replace('.', ','))).ToString("0.####");
                }
            }
        }

        private void txtDolar_Click(object sender, EventArgs e)
        {
            if (!IsErrorMode)
            {
                Form3 form3 = new Form3("USD", txtDolar.Text);
                form3.Show();
            }
        }

        private void txtEuro_Click(object sender, EventArgs e)
        {
            if (!IsErrorMode)
            {
                Form3 form3 = new Form3("EURO", txtEuro.Text);
                form3.Show();
            }
        }

        public void ErrorMode()
        {
            IsErrorMode = true;

            txtDolar.Visible = false;
            txtEuro.Visible = false;
            txtErrorDolar.Visible = true;
            txtErrorEuro.Visible = true;

            Change_txtLastUpdate_Color(downColor);
            threadPull.Abort();
            Change_txtLastUpdate_Text(DateTime.Now.ToLongTimeString());
        }

        public void RunningMode()
        {
            IsErrorMode = false;

            txtErrorDolar.Visible = false;
            txtErrorEuro.Visible = false;
            txtDolar.Visible = true;
            txtEuro.Visible = true;
        }


        /* METHODS IN METHOD */
        public void Change_rowDolar_Text(string text)
        {
            rowDolar.Text = text;
        }
        public void Change_rowDolar_ForeColor(Color color)
        {
            rowDolar.ForeColor = color;
        }

        public string Get_txtDolar_Text()
        {
            return txtDolar.Text;
        }
        public Color Get_txtDolar_Color()
        {
            return txtDolar.ForeColor;
        }

        public void Change_txtDolar_Text(string text)
        {
            txtDolar.Text = text;
        }
        public void Change_txtDolar_ForeColor(Color color)
        {
            txtDolar.ForeColor = color;
        }

        public void Change_rowEuro_Text(string text)
        {
            rowEuro.Text = text;
        }
        public void Change_rowEuro_ForeColor(Color color)
        {
            rowEuro.ForeColor = color;
        }

        public string Get_txtEuro_Text()
        {
            return txtEuro.Text;
        }
        public Color Get_txtEuro_Color()
        {
            return txtEuro.ForeColor;
        }

        public void Change_txtEuro_Text(string text)
        {
            txtEuro.Text = text;
        }
        public void Change_txtEuro_ForeColor(Color color)
        {
            txtEuro.ForeColor = color;
        }

        public void Change_txtLastUpdate_Text(string text)
        {
            txtLastUpdate.Text = text;
        }
        public void Change_txtLastUpdate_Color(Color color)
        {
            txtLastUpdate.ForeColor = color;
        }

        private void txtErrorDolar_Click(object sender, EventArgs e)
        {
            RunningMode();
        }
        private void txtErrorEuro_Click(object sender, EventArgs e)
        {
            RunningMode();
        }
    }
}