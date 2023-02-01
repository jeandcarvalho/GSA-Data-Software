using GSA.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GSA.Forms
{
    public partial class FormTag : Form
    {
        Aquisition current_aquisition_state = new Aquisition();
        List<Aquisition> aquisitionList = new List<Aquisition>();
        string filePath;
        bool isRecording = false;

        [DllImport("user32.dll")]
        static extern IntPtr PostMessage(IntPtr hWnd, UInt32 msg, int wParam, int lParam);

        public FormTag()
        {
            InitializeComponent();
            ResetButton(btnDriver_0);
            ResetButton(btnRoadCondition_0);
            ResetButton(btnRoadType_0);
            ResetButton(btnWeather_0);
            ResetButton(btnTb0);
            ResetButton(btnVC0);
            ResetButton(btnVT0);
            ResetButton(btnPoD0);
            this.filePath = "";
        }
        private void ResetButton(Button btn)
        {
            btn.BackColor = SystemColors.Control;
            btn.ForeColor = Color.Black;
        }
        private void SetAquisition()
        {
            Aquisition current_state = new Aquisition();
            current_state.CopyValues(current_aquisition_state);
            int aquisitionSize = aquisitionList.Count;
            if (aquisitionSize > 0)
            {
                aquisitionList[aquisitionSize-1].finish_time = current_state.finish_time;
                listView1.Items[aquisitionSize - 1] = new ListViewItem(aquisitionList[aquisitionSize - 1].ListViewString());

            }
            this.aquisitionList.Add(current_state);
            ListViewItem i = new ListViewItem(current_state.ListViewString());
            listView1.Items.Add(i);
            listView1.Items[aquisitionSize].EnsureVisible();
            

        }
      

      

        private void buttonClicked(object sender, EventArgs e)
        {
            
            Control control = ((Button)sender).Parent;
            String btnName = ((Button)sender).Text;
            var property_name = control.Text;
            property_name = property_name.ToLower();
            property_name = property_name.Replace(" ", "_");
            if (property_name == "curves" && btnName == "")
            {
                btnName = ((Button)sender).Tag.ToString();
            }
            current_aquisition_state[property_name] = btnName;
            foreach (Control c in control.Controls)
            {
                c.BackColor = Color.Transparent;
                c.ForeColor = Color.Black;
            }
            Control click = (Control)sender;
            click.BackColor = SystemColors.Control;
            click.ForeColor = Color.Black;

        }

       




        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (this.textBox5.Text != "") {
                    if (StartCanape() && !isRecording)
                    {
                    var btn = (Button)sender;
                    btn.Enabled = false;
                    isRecording = true;
                    SetAquisition();
                    this.timer1.Start();
                    
                    }
            }
            else
            {
               var result =  MessageBox.Show("É necessário escolher o caminho do arquivo de log");
            }

        }

       

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.aquisitionList.Count < 1) { 
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Log File| *.log";
            sfd.FileName = "aquisition";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.textBox5.Text = sfd.FileName;
                this.filePath = sfd.FileName; 
            }
            }
        }
       private bool StartCanape()
        {
            return true;
            const UInt32 WM_KEYDOWN = 0x0100;
            const int VK_F9 = 0x78;
            Process p = Process.GetProcessesByName("CANape64").FirstOrDefault();
            if (p != null) {
                PostMessage(p.MainWindowHandle, WM_KEYDOWN, VK_F9, 0);
                return true;
            }
            else
            {
                MessageBox.Show("O software CANape não foi inicializado");
                return false;
            }

        }
        private bool StopCanape()
        {
            return true;
            const UInt32 WM_KEYDOWN = 0x0100;
            const int VK_ESCAPE = 0x1B;
            Process p = Process.GetProcessesByName("CANape64").FirstOrDefault();
            if (p != null)
            {
                PostMessage(p.MainWindowHandle, WM_KEYDOWN, VK_ESCAPE, 0);
                return true;
            }
            else
            {
                MessageBox.Show("O software CANape não foi inicializado");
                return false;
            }

        }
        private void saveFile()
        {
            var data = aquisitionList.Select(x => (string)x);
            try
            {
                File.WriteAllLines(this.filePath, data);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            int aquisitionSize = aquisitionList.Count;
            if (aquisitionSize > 0)
            {
                aquisitionList[aquisitionSize - 1].finish_time = DateTime.Now;
                listView1.Items[aquisitionSize - 1] = new ListViewItem(aquisitionList[aquisitionSize - 1].ListViewString());
                timer1.Stop();
                if (!StopCanape())
                {
                    MessageBox.Show("Erro ao encerrar o CANape!");
                }
                btn_startrecord.Enabled = true;
                saveFile();
            }
            else
            {
                var result = MessageBox.Show("Nenhuma aquisição obtida pois não foi inserido nenhum dado");

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetAquisition();
        }
    }
}
