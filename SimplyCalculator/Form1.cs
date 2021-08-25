using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace SimplyCalculator
{
    public partial class Form : System.Windows.Forms.Form
    {
        Stack<string> listExp = new Stack<string>();
        bool flagDisplay = true;//是否清空畫面 T是 F否
        bool flagChangOperation = false;//是否更換運算子  T是 F否
        public Form()
        {
            InitializeComponent();
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text.ToString())
            {
                case "÷":
                case "✕":
                case "－":
                case "＋":
                    flagDisplay = true;
                    pickOperation(btn.Text.ToString());
                    break;
                case "=":
                case "⌫":
                case "C":
                case "CE":
                    pickFunction(btn.Text.ToString());
                    break;
                case ".":
                    pickNumber(btn.Text.ToString());
                    break;
                default:
                    if (flagDisplay)
                    {
                        flagDisplay = false;
                        this.txtDisplay.Text = "";
                    }
                    pickNumber(btn.Text.ToString());
                    break;
            }
        }

        private void pickOperation(string str)
        {
            if (this.txtDisplay.Text != "")
            {
                if (flagChangOperation)
                {
                    this.txtErr.Text = this.txtErr.Text.Length==0?"":this.txtErr.Text.Substring(0,txtErr.Text.Length-1);
                    listExp.Pop();
                }
                else
                {
                    listExp.Push(this.txtDisplay.Text);

                    if (this.txtErr.Text.Contains("="))
                    {
                        this.txtErr.Text = this.txtDisplay.Text;
                    }
                    else
                    {
                        this.txtErr.Text += this.txtDisplay.Text;
                    }

                    if (listExp.Count == 3)
                    {
                        string temp = getSum().ToString();
                        listExp.Clear();
                        listExp.Push(temp);
                        this.txtDisplay.Text = temp;
                    }
                    else
                    {
                        this.txtDisplay.Text = "";
                    }
                }
                flagChangOperation = true;
                switch (str)
                {
                    case "÷":
                        this.txtErr.Text += "/";
                        listExp.Push("/");
                        break;
                    case "✕":
                        this.txtErr.Text += "*";
                        listExp.Push("*");
                        break;
                    case "－":
                        this.txtErr.Text += "-";
                        listExp.Push("-");
                        break;
                    case "＋":
                        this.txtErr.Text += "+";
                        listExp.Push("+");
                        break;
                }
            }
        }

        private void pickNumber(string str)
        {
            string temp = this.txtDisplay.Text;
            Decimal b = 0;
            switch (str)
            {
                case "±":
                    try
                    {
                        b = Convert.ToDecimal(temp);
                        this.txtDisplay.Text = (b * (-1)).ToString();
                    }
                    catch { }
                    break;
                case ".":
                    if (!this.txtDisplay.Text.ToString().Contains("."))
                    {
                        this.txtDisplay.Text += str;
                    }
                    break;
                default:
                    flagChangOperation = false;
                    if (this.txtErr.Text.Contains("="))
                    {
                        this.txtErr.Text = "";
                    }
                    temp += str;
                    try
                    {
                        b = Convert.ToDecimal(temp);
                        if (str != "0")
                        {
                            this.txtDisplay.Text = b.ToString();
                        }
                        else
                        {
                            if ((this.txtDisplay.Text.Contains(".")) || (b != 0))
                            {
                                this.txtDisplay.Text += "0";
                            }
                            else
                            {
                                this.txtDisplay.Text = "0";
                            }
                        }
                    }
                    catch{}
                    break;
            }
        }

        private void pickFunction(string str)
        {
            switch (str)
            {
                case "=":
                    flagDisplay = true;
                    listExp.Push(this.txtDisplay.Text);
                    this.txtErr.Text += this.txtDisplay.Text; 
                    if (listExp.Count == 3)
                    {
                        this.txtDisplay.Text = getSum().ToString();
                        this.txtErr.Text = this.txtErr.Text + "=" +this.txtDisplay.Text;
                        listExp.Clear();
                    }
                    break;
                case "⌫":
                    if (this.txtErr.Text.Contains("="))
                    {
                        this.txtErr.Text = "";
                    }
                    this.txtDisplay.Text = this.txtDisplay.Text.Length <= 1 ? "0" : this.txtDisplay.Text.Substring(0, this.txtDisplay.Text.Length - 1);
                    break;
                case "C":
                    this.txtDisplay.Text = "0";
                    this.txtErr.Text = "";
                    this.listExp.Clear();
                    flagDisplay = true;
                    break;
                case "CE":
                    this.txtDisplay.Text = "0";
                    if (this.txtErr.Text.Contains("="))
                    {
                        this.txtErr.Text = "";
                    }
                    flagDisplay = true;
                    break;
            }
        }

        private Decimal getSum()
        {
            try
            {
                Decimal num2 = Convert.ToDecimal(this.listExp.Pop());
                string op = this.listExp.Pop();
                Decimal num1 = Convert.ToDecimal(this.listExp.Pop());
                switch (op)
                {
                    case "+":
                        return num1 + num2;
                    case "-":
                        return num1 - num2;
                    case "*":
                        return num1 * num2;
                    case "/":
                        return num1 / num2;
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }
    }
}
