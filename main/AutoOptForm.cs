using System.Windows.Forms;

namespace main
{
    public partial class AutoOptForm : Form
    {
        MainForm FMain;

        public AutoOptForm(MainForm _main)
        {
            FMain = _main;
            InitializeComponent();
            
        }

        private void AutoOptForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FMain.Enabled = true;
            FMain.refresh();
        }
    }
}
