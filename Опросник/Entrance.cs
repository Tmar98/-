using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Опросник
{
    public partial class Entrance : Form
    {
        public Persona persona;
        public Persons persons;
        public BindingSource sourse;
        public Class clas;
        public Classes classes;
        string fileclass = "Class.txt";
        public int enter = 0;
        public Entrance()
        {
            InitializeComponent();  
        }        

        
         private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
         {
         sourse = new BindingSource();
         classes = new Classes(fileclass);
         List<Class> zw = classes.Where(z => z.School == (comboBox1.SelectedItem).ToString()).ToList();
         sourse.DataSource = zw;
         comboBox2.DataSource = sourse;
         comboBox2.DisplayMember = "Klass";
         }
        

        private void button1_Click(object sender, EventArgs e)
        {
            Class per = comboBox2.SelectedItem as Class;
            if (textBox1.Text == "1")
            {
                enter = 1;
                Form1 form1 = this.Tag as Form1;             
                form1.pas = true;
                
                form1.button2.Show();
                this.Close();
            }
           else
           if (comboBox1.SelectedItem==null)
                MessageBox.Show("Выберите корпус");
            else if (textBox1.Text != "" && (comboBox1.SelectedItem).ToString() != null && per.Klass != null)
            {
               enter = 1;
               Form1 form1 = this.Tag as Form1;
               form1.pas = false;
               DateTime localtime = DateTime.Now;
               Persona ps = new Persona(form1.id, form1.Nomer, textBox1.Text, (comboBox1.SelectedItem).ToString(), per.Klass, localtime);
               form1.persource.Add(ps);
               (form1.persource.DataSource as Persons).AddNew(ps);
                if (form1.Nomer == 1)
                    form1.button2.Show();
                else if (form1.Nomer == 2)
                    form1.button11.Show();
                else if (form1.Nomer == 3)
                    form1.button12.Show();
                else if (form1.Nomer == 4)
                    form1.button13.Show();
                else 
                    form1.button14.Show();
               this.Close();
                               
            }
            else { MessageBox.Show("Введите ФИО");  }

        }
        private void Entrance_FormClosing(object sender, FormClosingEventArgs e)//форму нельзя закрыть крестиком
        {
            if (enter==0)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void Entrance_Load(object sender, EventArgs e)
        {

        }
    }
}
