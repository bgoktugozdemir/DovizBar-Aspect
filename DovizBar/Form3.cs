using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static DovizBar.Properties.Settings;
using System.Windows.Forms;
using DovizBar.Aspects;

namespace DovizBar
{
    public partial class Form3 : Form
    {
        public static string Type;
        public static string tempValue;
        private double currentValue;
        public static Form3 form3;

        public Form3(string cType, string currentValue)
        {
            form3 = this;
            Type = cType;
            this.currentValue = Convert.ToDouble(currentValue.Replace('.',','));
            InitializeComponent();
        }

        [TargetAspect]
        private void Form3_Load(object sender, EventArgs e)
        {
            txtGoal.Text = tempValue;
        }

        private void txtGoal_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form1.KeyControl(sender, e);
        }

        [Form3CloseAspect]
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Type == "USD")
            {
                Default.DolarGoal = Convert.ToDouble(Convert.ToDouble(txtGoal.Text.Replace('.',',')).ToString("#.0000"));

                if (currentValue > Default.DolarGoal)
                    Default.DolarGoalBool = false;
                else if (currentValue < Default.DolarGoal)
                    Default.DolarGoalBool = true;
            }
            else if (Type == "EURO")
            {
                Default.EuroGoal = Convert.ToDouble(txtGoal.Text.ToString().Replace('.',','));

                if (currentValue > Default.EuroGoal)
                    Default.EuroGoalBool = false;
                else if (currentValue < Default.EuroGoal)
                    Default.EuroGoalBool = true;
            }
        }

        [Form3CloseAspect]
        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        [ResetGoalAspect]
        [Form3CloseAspect]
        private void btnNoGoal_Click(object sender, EventArgs e)
        {

        }

        public static void ResetGoal(string type)
        {
            if (type == "USD")
            {
                Default.DolarGoal = 0;
                Default.DolarGoalBool = false;
            }
            else if (type == "EURO")
            {
                Default.EuroGoal = 0;
                Default.EuroGoalBool = false;
            }
        }
    }
}
