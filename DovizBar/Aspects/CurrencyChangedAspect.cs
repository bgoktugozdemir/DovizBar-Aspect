using System;
using System.Drawing;
using PostSharp.Aspects;

namespace DovizBar.Aspects
{
    [Serializable]
    public class CurrencyChangedAspect : OnMethodBoundaryAspect
    {
        private Color _dolarColor, _euroColor;
        private string _dolar, _euro;
        public override void OnExit(MethodExecutionArgs args)
        {
            _dolar = ((Form1)args.Instance).Get_txtDolar_Text();
            _euro = ((Form1)args.Instance).Get_txtEuro_Text();

            _dolarColor = ((Form1)args.Instance).Get_txtDolar_Color();
            _euroColor = ((Form1)args.Instance).Get_txtEuro_Color();

            UpdateScreen();
        }

        private void UpdateScreen()
        {
            Form1.form4.Change_Currency_Color(_dolarColor, _euroColor);
            Form1.form4.Change_Currency_Text(_dolar, _euro);
        }
    }
}