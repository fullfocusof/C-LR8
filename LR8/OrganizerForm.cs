using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR8
{
    public partial class OrganizerForm : Form
    {
        string FileName;

        public OrganizerForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string tab = "listBox" + (orgTabControl.SelectedIndex + 1);
            ListBox curBox = (ListBox)Controls.Find(tab, true)[0];
            if(textBoxInput.Text != string.Empty)
            {
                curBox.Items.Add(textBoxInput.Text);
            }          
            textBoxInput.Text = string.Empty;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            string tab = "listBox" + (orgTabControl.SelectedIndex + 1);
            ListBox curBox = (ListBox)Controls.Find(tab, true)[0];
            textBoxInput.Text = (string)curBox.SelectedItem;
        }

        private void OrganizerForm_Load(object sender, EventArgs e)
        {
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);
            toolStripStatusLabel.Text = DateTime.Now.ToString() + " " + DateTime.Now.DayOfWeek;
        }

        private void SaveToFile(string FileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    for (int i = 1; i <= orgTabControl.Controls.Count; i++)
                    {
                        ListBox curBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                        sw.WriteLine(curBox.Items.Count.ToString());
                        for (int j = 0; j < curBox.Items.Count; j++)
                        {
                            sw.WriteLine(curBox.Items[j]);
                        }
                        curBox.Items.Clear();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении файла", "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void LoadFromFile(string FileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    for (int i = 1; i <= orgTabControl.Controls.Count; i++)
                    {
                        ListBox curBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                        int cnt = int.Parse(sr.ReadLine());
                        for (int j = 0; j < cnt; j++)
                        {
                            curBox.Items.Add(sr.ReadLine());
                        }
                    }
                }
            }
            catch (Exception)
            {
                for (int i = 1; i <= orgTabControl.Controls.Count; i++)
                {
                    ListBox curBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                    curBox.Items.Clear();
                }
                MessageBox.Show("Файл не найден\nСоздание файла успешно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SaveToFile(FileName);
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            string tab = "listBox" + (orgTabControl.SelectedIndex + 1);
            ListBox curBox = (ListBox)Controls.Find(tab, true)[0];
            try
            {
                curBox.Items[curBox.SelectedIndex] = textBoxInput.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите заметку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            string tab = "listBox" + (orgTabControl.SelectedIndex + 1);
            ListBox curBox = (ListBox)Controls.Find(tab, true)[0];
            curBox.Items.Clear();
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                buttonAdd_Click(sender, e);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = DateTime.Now.ToString() + " " + DateTime.Now.DayOfWeek;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string tab = "listBox" + (orgTabControl.SelectedIndex + 1);
            ListBox curBox = (ListBox)Controls.Find(tab, true)[0];
            try
            {
                int temp = curBox.SelectedIndex;
                curBox.Items.RemoveAt(curBox.SelectedIndex);
                if (temp != 0 && temp == curBox.Items.Count)
                {
                    curBox.SelectedIndex = temp - 1;
                }
                else if(temp == 0 && temp != curBox.Items.Count)
                {
                    curBox.SelectedIndex = temp;
                }              
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите заметку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OrganizerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveToFile(FileName);
        }
    }
}