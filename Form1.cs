using System;
using System.Text;
using System.Windows.Forms;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            var frameworkVersions = DotNetRuntimeChecker.GetDotNetFrameworkVersions();
            sb.AppendLine("[.NET Framework]");
            if (frameworkVersions.Count > 0)
            {
                foreach (var v in frameworkVersions)
                {
                    sb.AppendLine("  " + v);
                }
            }
            else
            {
                sb.AppendLine("  インストールされていません");
            }

            sb.AppendLine();

            var coreRuntimes = DotNetRuntimeChecker.GetDotNetCoreRuntimes();
            sb.AppendLine("[.NET (Core / 5+)]");
            if (coreRuntimes.Count > 0)
            {
                foreach (var v in coreRuntimes)
                {
                    sb.AppendLine("  " + v);
                }
            }
            else
            {
                sb.AppendLine("  インストールされていません");
            }

            txtResult.Text = sb.ToString();
        }
    }
}
