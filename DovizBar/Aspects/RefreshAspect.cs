using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class RefreshAspect:OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Form1.form1.ErrorMode();
            MessageBox.Show("Sunucuya erişilemedi!", "Sunucuya Erişilemedi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
