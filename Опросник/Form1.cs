using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Опросник
{
    public partial class Form1 : Form
    {
        string filename = "Pers.txt";
        string filequest = "Question.txt";
        string filequest2 = "Question2.txt";
        string filequest3 = "Question3.txt";
        string filequest4 = "Question4.txt";
        string filequest5 = "Question5.txt";
        string fileotv = "Otvet.txt";
        string fileclass = "Class.txt";
        public Persona persona;
        public Persons persons;
        public BindingSource source;
        public BindingSource persource;
        public Class clas;
        public Classes classes;
        public int id = 1;//id persona
        public int Nomer = 1;//nomer testa
        public bool pas = false;//ребенок или учитель
        public int t = 0;//счетчик для ответов да и нет
        public string[] str = new string[100];//массив с вопросами
        public string[] otv = new string[89];//массив ответов
        public string[,] res = new string[9, 3];//масив результатов
        public int[,] array = new int[9, 12];//масив правильных ответов
        public string[,] table = new string[100, 102];//таблица для вывода
        public int enter = 0; //переменная для определения можно ли закрывать форму нажатием на крестик(функция закрытия формы)


        //SqlConnection sqlConnection;


        public Form1()
        {
            InitializeComponent();
            persons = new Persons(filename);
            foreach (Persona p in persons)
            { id++; }
            persource = new BindingSource();
            persource.DataSource = persons;
            winlod();

            //winsql();
        }

        void winlod()
        {
            button6.Text = "Адаптированный подростковый вариант опросника Шмишека";
            // button6.Location = new Point(10, 10);
            button7.Text = "Шкала безнадежности Бека";
            button8.Text = "Шкала Астенического Состояния";
            button9.Text = "Тип мышления";
            button10.Text = "Самооценка психологической готовности к ЕГЭ";
            button1.Hide();
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
            button11.Hide();
            button12.Hide();
            button13.Hide();
            button14.Hide();
            button15.Hide();
            button16.Hide();
            button17.Hide();
            listBox1.Hide();
            listBox2.Hide();
            listBox3.Hide();
            dataGridView1.Hide();
            richTextBox1.Hide();

            /*var path = @"Data Source=DESKTOP-JH3BOL4\SQLEXPRESS;Initial Catalog=Qtr;Integrated Security=True";
            using (var connection = new SqlConnection(path))
            {
                connection.Open();

                var updateCommand = new SqlCommand("update Table_1 set ut = '6' where id =2", connection);
                updateCommand.ExecuteNonQuery();

                var commandRead = new SqlCommand();
                commandRead.Connection = connection;
                commandRead.CommandText = "select id,ut from Table_1";

                var reader = commandRead.ExecuteReader();
                while(reader.Read())
                {
                    label1.Text += reader["ut"];
                }

                connection.Close();
            }
            */
        }


        /* async void winsql()
         {
             string connectionstring = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\Прога для школы\Опросник\Опросник\Database.mdf; Integrated Security = True";

             sqlConnection = new SqlConnection(connectionstring);

             await sqlConnection.OpenAsync();
         }
         */
        /// ////////////////////////Тест "Адаптированный подростковый вариант опросника Шмишека"

        private void button6_Click(object sender, EventArgs e)//кнопка теста
        {
            button6.Hide(); //сокрытие кнопок других тестов
            button7.Hide(); //сокрытие кнопок других тестов
            button9.Hide(); //сокрытие кнопок других тестов
            button8.Hide(); //сокрытие кнопок других тестов
            button10.Hide(); //сокрытие кнопок других тестов
            Nomer = 1; //определитель номера теста
            Entrance addPers = new Entrance(); //открытие нового окна для введения данных опрашиваемого
            addPers.Tag = this;
            addPers.Show();
            button2.Show();//кнопка для начала теста
        }

        private void button2_Click(object sender, EventArgs e)//кнопка для начала теста
        {
            if (pas) //проверка результатов учителем
            {
                button2.Hide();//кнопка для начала теста
                button5.Show();
                listBox1.Show();//выборка для проверки тестов
                listBox2.Show();
                listBox3.Show();
                button1.Show();
            }
            else //прохождение теста
            {
                for (int i = 20; i < 89; i++)//очистка массива от лишних вопросов других тестов min кол вопросов 20 шт
                {
                    str[i] = null;
                }
                richTextBox1.Show(); //поле вывода вопросов
                if (File.Exists(filequest)) //чтение вопросов из файла
                {
                    StreamReader sr = new StreamReader(filequest);
                    string s0;
                    for (int i = 0; (s0 = sr.ReadLine())!= null; i++)//запись вопросов в массив
                    {
                        str[i] = s0;
                    }
                    sr.Close();
                    sr.Dispose();
                    richTextBox1.Text = str[0];
                }
                button2.Hide();
                button3.Show();
                button3.Text = "Да";
                button4.Show();
                button4.Text = "Нет";
                enter = 1;
                for (int i = 20; i < 89; i++)//очистка массива ответов
                {
                    otv[i] = null;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            source = new BindingSource();
            classes = new Classes(fileclass);
            List<Class> zw = classes.Where(z => z.School == (listBox1.SelectedItem).ToString()).ToList();
            source.DataSource = zw;
            listBox2.DataSource = source;
            listBox2.DisplayMember = "Klass";
        }                               

        private void button3_Click(object sender, EventArgs e)// кнопка ответов нв тест "ДА"
        {
            otv[t] = 1.ToString();//1==да
            if (str[t+1] != null)
            {
                richTextBox1.Text = str[t+1];
            }
            else
            {
                richTextBox1.Text = " ";
                richTextBox1.Hide();
                MessageBox.Show("Тест пройден");
                button6.Show();
                button7.Show();
                button8.Show();
                button9.Show();
                button10.Show();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button15.Hide();
                button16.Hide();
                button17.Hide();
                t = 0;
                Save();//Функция сохранения
                enter = 0;
            }
            t++;
        }

        private void button4_Click(object sender, EventArgs e)//no
        {
            otv[t] = 0.ToString();//0==нет
            if (str[t+1] != null)
            {
                richTextBox1.Text = str[t+1];
            }
            else
            {
                MessageBox.Show("Тест пройден");
                button6.Show();
                button7.Show();
                button8.Show();
                button9.Show();
                button10.Show();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button15.Hide();
                button16.Hide();
                button17.Hide();
                richTextBox1.Text = " ";
                richTextBox1.Hide();
                t = 0;
                Save();
                enter = 0;
            }
            t++;
        }

        void Save()
        {
            string a = id.ToString();
            for (int i = 0; i < 89 && otv[i] != null; i++)
            {
                if (otv[i] == 1.ToString())
                {
                    a += " ";
                    a += "да";
                }
                else if (otv[i] == 0.ToString())
                {
                    a += " ";
                    a += "нет";
                }
                else if (otv[i] == 2.ToString())
                {
                    a += " ";
                    a += "2";
                }
                else if (otv[i] == 3.ToString())
                {
                    a += " ";

                    a += "3";
                }
                else if (otv[i] == 4.ToString())
                {
                    a += " ";

                    a += "4";
                }
            }
            FileStream fs;
            if (!File.Exists(fileotv))
            {
                fs = File.Create(fileotv);
            }
            else
               fs = File.Open(fileotv, FileMode.Append);
               StreamWriter sw = new StreamWriter(fs);
               sw.WriteLine(a);
               sw.Close();
               sw.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)// Вывод таблицы с результатами
        {
            button1.Hide();//Скрытие кнопки перехода на главную страницу
            enter = 2;//Переменная которая отвечает за выход из таблицы нажатием на крестик
            dataGridView1.Show();//Показать таблицу dataGridView1
            this.Width = 1500;//Размер приложения по ширине 
            this.Height = 500;//Размер приложения по высоте
            dataGridView1.Width = 1500;//Размер таблицы по ширине 
            dataGridView1.Height = 500;//Размер таблицы по высоте
            if (listBox3.SelectedIndex + 1 == 1)//Условие какой тест отображать 
            {
                int sum = this.dataGridView1.Columns.Count;//Количество колонок в таблице
                for (int i = 0; i < sum; i++)//Цикл по колонкам
                { this.dataGridView1.Columns.RemoveAt(0); }//Удаление колонок в таблице
                loadtablepers();//выборка нужных людей и загрузка их в таблицу
                loadtabel1();//загрузка ответов для анализа test 1
                Result1();//добовление к людям в таблице их ответы и результаты test 1
                Sortyrovka();//сортировка по убыванию
                Vivod1();//вывод test 1
                Array.Clear(table, 0, table.Length);//очистка массива результатов
            }
            else if (listBox3.SelectedIndex + 1 == 2)//Условие какой тест отображать
            {
                int sum = this.dataGridView1.Columns.Count;//Количество колонок в таблице
                for (int i = 0; i < sum; i++)//Цикл по колонкам
                { this.dataGridView1.Columns.RemoveAt(0); }//Удаление колонок в таблице
                loadtablepers();//выборка нужных людей и загрузка их в таблицу 
                loadtabel2();//загрузка ответов для анализа test 2
                Result2();//добовление к людям в таблице их ответы и результаты test 2
                Vivod2();//вывод test 2
                Array.Clear(table, 0, table.Length);//очистка массива результатов
            }
            else if (listBox3.SelectedIndex + 1 == 3)//Условие какой тест отображать
            {
                int sum = this.dataGridView1.Columns.Count;//Количество колонок в таблице
                for (int i = 0; i < sum; i++)//Цикл по колонкам
                { this.dataGridView1.Columns.RemoveAt(0); }//Удаление колонок в таблице
                loadtablepers();//выборка нужных людей и загрузка их в таблицу 
                loadtabel3();//загрузка ответов для анализа test 3
                Result3();//добовление к людям в таблице их ответы и результаты test 3
                Vivod3();//вывод test 3
                Array.Clear(table, 0, table.Length);//очистка массива результатов
            }
            else if (listBox3.SelectedIndex + 1 == 4)//Условие какой тест отображать
            {
                int sum = this.dataGridView1.Columns.Count;//Количество колонок в таблице
                for (int i = 0; i < sum; i++)//Цикл по колонкам
                { this.dataGridView1.Columns.RemoveAt(0); }//Удаление колонок в таблице
                loadtablepers();//выборка нужных людей и загрузка их в таблицу 
                loadtabel4();//загрузка ответов для анализа test 3
                Result4();//добовление к людям в таблице их ответы и результаты test 3
                Vivod4();//вывод test 3 ultramar3@yandex.ru
                Array.Clear(table, 0, table.Length);//очистка массива результатов
            }
            else if (listBox3.SelectedIndex + 1 == 5)//Условие какой тест отображать
            {
                int sum = this.dataGridView1.Columns.Count;//Количество колонок в таблице
                for (int i = 0; i < sum; i++)//Цикл по колонкам
                { this.dataGridView1.Columns.RemoveAt(0); }//Удаление колонок в таблице
                loadtablepers();//выборка нужных людей и загрузка их в таблицу 
                loadtabel5();//загрузка ответов для анализа test 3
                Result5();//добовление к людям в таблице их ответы и результаты test 3
                Vivod5();//вывод test 3 ultramar3@yandex.ru
                Array.Clear(table, 0, table.Length);//очистка массива результатов
            }
            listBox1.Hide();//Скрываем поля с выборами
            listBox2.Hide();//Скрываем поля с выборами
            listBox3.Hide();//Скрываем поля с выборами
            button5.Hide();//Скрываем кнопку для перехода к результатам
        }

        void loadtablepers()
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {
                    string[] ar = s0.Split(' ');
                    Class per = listBox2.SelectedItem as Class;
                    if ((listBox1.SelectedItem).ToString() == ar[3] && per.Klass == ar[4] && (listBox3.SelectedIndex + 1).ToString() == ar[1])//добавить отбор по тесту
                    {
                        for (int j = 0; j < 6; j++)//увеличить на 1
                        {
                            if (j < 1)
                                table[i, j] = ar[j];
                            else if (j > 1)
                                table[i, j - 1] = ar[j];//не отображение номера теста
                        }
                    }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }//выборка нужных людей и загрузка их в таблицу

        void loadtabel1()
        { 
            if (File.Exists(fileotv))
            {
                StreamReader sr = new StreamReader(fileotv);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {                    
                    string[] ar = s0.Split(' ');
                    if (table[i, 0] == ar[0])
                        for (int j = 1; j < 88; j++)
                        {
                            if (ar[j] == "да")
                                table[i, j + 4] = 1.ToString();
                            else
                                table[i, j + 4] = 0.ToString();
                        }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }//загрузка ответов для анализа test 1

        void Result1()
        {
            res[0, 2] = "Гипертимный";
            res[1, 2] = "Циклотимный";
            res[2, 2] = "Лабильный";
            res[3, 2] = "Сензитивный";
            res[4, 2] = "Педантичный";
            res[5, 2] = "Шизоидный";
            res[6, 2] = "Эпилептоидный";
            res[7, 2] = "Истероидный";
            res[8, 2] = "Неустойчивый";
                        
            array = new int[,]
            {
                { 1, 11, 23 , 45, 55, 67, 77, 0, 0, 0, 0, 0 },
                { 6, 18, 28, 43, 50, 62, 72, 76, 0, 0, 0, 0 },
                { 3, 10, 13, 21, 32, 35, 47, 54, 57, 69, 75, 79 },
                { 16, 27, 38, 49, 60, 71, 82, 0, 0, 0, 0, 0 },
                { 4, 14, 17, 26, 39, 48, 58, 61, 70, 80, 83, 0 },
                { 51, 68, 74, 81, 87, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 12, 20, 24, 34, 37, 52, 56, 59, 64, 78, 0 },
                { 7, 15, 19, 22, 29, 41, 44, 63, 66, 73, 85, 88 },
                { 8, 25, 30, 31, 42, 84, 86, 0, 0, 0, 0, 0 }
            };

            for (int t=1;table[t,1]!=null;t++)
            {
                res[0, 1] = 0.ToString();
                res[1, 1] = 0.ToString();
                res[2, 1] = 0.ToString();
                res[3, 1] = 0.ToString();
                res[4, 1] = 0.ToString();
                res[5, 1] = 0.ToString();
                res[6, 1] = 0.ToString();
                res[7, 1] = 0.ToString();
                res[8, 1] = 0.ToString();
                if (Convert.ToInt32(table[t,9])==0)
                    res[3, 1]= (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 40]) == 0)
                    res[4, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 44]) == 0)
                    res[5, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 57]) == 0)
                    res[5, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 69]) == 0)
                    res[5, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 50]) == 0)
                    res[6, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                if (Convert.ToInt32(table[t, 13]) == 0)
                    res[8, 1] = (Convert.ToInt32(res[3, 1] + 1)).ToString();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (array[i, j] != 0)
                            if (Convert.ToInt32(table[t,(array[i,j])+4])==1)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 1).ToString();
                    }
                }
                table[t, 93] = (Convert.ToInt32(res[0, 1])*3).ToString();
                table[t, 94] = (Convert.ToInt32(res[1, 1])*3).ToString();
                table[t, 95] = (Convert.ToInt32(res[2, 1])*2).ToString();
                table[t, 96] = (Convert.ToInt32(res[3, 1])*3).ToString();
                table[t, 97] = (Convert.ToInt32(res[4, 1])*2).ToString();
                table[t, 98]= (Convert.ToInt32(res[5, 1])*3).ToString();
                table[t, 99] = (Convert.ToInt32(res[6, 1])*2).ToString();
                table[t, 100] = (Convert.ToInt32(res[7, 1])*2).ToString();
                table[t, 101] = (Convert.ToInt32(res[8, 1])*3).ToString();
            }


        }//добовление к людям в таблице их ответы и результаты test1

        void Sortyrovka()
        {
            for (int per = 1; table[per, 1] != null; per++)
            {
                string[,] sort = new string[9, 3];
                for (int i = 0; i < 9; i++)
                    sort[i, 2] = res[i, 2];
                for (int i = 0; i < 9; i++)
                    sort[i, 1] = table[per, i + 93];
                for(int p=0;p<8;p++)
                for (int i = 0; i < 8; i++)
                {
                    if (Convert.ToInt32(sort[i, 1]) < Convert.ToInt32(sort[i + 1, 1]))
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            string m = sort[i, j];
                            sort[i, j] = sort[i + 1, j];
                            sort[i + 1, j] = m;
                        }
                    }
                }
                for (int i=0;i<9;i++)
                {
                    table[per,i+93] = sort[i, 2]+" (" +sort[i, 1]+")";
                }
            }
        }

        void Vivod1()
        {            
            for (int j = 1; table[j, 2] != null; j++)
            {
                string[] Stroka = { table[j, 1], table[j, 2], table[j, 3], table[j, 4], table[j, 93], table[j, 94], table[j, 95], table[j, 96], table[j, 97], table[j, 98], table[j, 99], table[j, 100], table[j, 101], };

                addGridParam(Stroka, dataGridView1);
            }
        }//вывод test 1

        void addGridParam(string[] N, DataGridView Grid )
        {
            int w = 0;
            while (N.Length > Grid.ColumnCount)
            {                
                Grid.Columns.Add("", "");
                Grid.Columns[w].Width = 110;
                w++;
            }
            Grid.Rows.Add(N);
        }

        //////////////////////////////// Конец теста №1
       
        //////////////////////////////// Тест №2


        private void button7_Click(object sender, EventArgs e)//Тест №2
        {
            button6.Hide();
            button7.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            Nomer = 2;
            Entrance addPers = new Entrance();
            addPers.Tag = this;
            addPers.Show();
            button11.Location = new Point(300, 250);
            button11.Text = "Начать";

        }

        private void button11_Click(object sender, EventArgs e)//отображение вопросов
        {
            for (int i = 20; i < 89; i++)
            {
                str[i] = null;
            }
            richTextBox1.Show();
            if (File.Exists(filequest2))
            {
                StreamReader sr = new StreamReader(filequest2);
                string s0;
                for (int i = 0; i < 20; i++)
                {
                    s0 = sr.ReadLine();
                    str[i] = s0;
                }
                sr.Close();
                sr.Dispose();
                richTextBox1.Text = str[0];
            }
            button11.Hide();
            button3.Show();
            button3.Text = "Верно";
            button4.Show();
            button4.Text = "Неверно";
            enter = 1;
            for (int i=20;i<89;i++)
            {
                otv[i] = null;
            }
        }

        void loadtabel2()//загрузка ответов для анализа test 2
        {
            if (File.Exists(fileotv))
            {
                StreamReader sr = new StreamReader(fileotv);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {
                    string[] ar = s0.Split(' ');
                    if (table[i, 0] == ar[0])
                        for (int j = 1; j < 21; j++)
                        {
                            if (ar[j] == "да")
                                table[i, j + 4] = 1.ToString();
                            else
                                table[i, j + 4] = 0.ToString();
                        }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }
       
        void Result2()
        {
            int[] array1 = new int[] { 2, 4, 7, 9, 11, 12, 14, 16, 17, 18, 20 };//верные
            int[] array2 = new int[] { 1, 3, 5, 6, 8, 10, 13, 15, 19 };//неверные
            int ball = 0;//число баллов
            for (int t = 1; table[t, 1] != null; t++)//цикл по людям 
            {
                for (int i = 0; i < 11; i++)
                {
                    if (Convert.ToInt32(table[t, (array1[i]) + 4]) == 1)
                    {
                        ball++;
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (Convert.ToInt32(table[t, (array2[i]) + 4]) == 0)
                    {
                        ball++;
                    }
                }
                if (ball <= 3)
                    table[t, 26] = "Безнадёжность не выявлена (" + ball + ")";
                else if (ball > 3 && ball < 9)
                    table[t, 26] = "Безнадежность лёгкая (" + ball + ")";
                else if (ball > 8 && ball < 15)
                    table[t, 26] = "Безнадежность умеренная (" + ball + ")";
                else if (ball > 14 && ball < 21)
                    table[t, 26] = "Безнадежность тяжёлая (" + ball + ")";
            }
        }

        void Vivod2()
        {
            for (int j = 1; table[j, 2] != null; j++)
            {
                string[] Stroka = { table[j, 1], table[j, 2], table[j, 3], table[j, 4], table[j, 26] };

                addGridParam(Stroka, dataGridView1);
            }
        }

        //////////////////////////////// Конец теста №2
        //////////////////////////////// Тест №3

        private void button8_Click(object sender, EventArgs e)//Тест №3
        {
            button6.Hide();
            button7.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            Nomer = 3;
            Entrance addPers = new Entrance();
            addPers.Tag = this;
            addPers.Show();
            button12.Location = new Point(300, 250);
            button12.Text = "Начать";
        }

        private void button12_Click(object sender, EventArgs e)//отображение вопросов
        {
            for (int i = 20; i < 89; i++)
            {
                str[i] = null;
            }
            richTextBox1.Show();
            if (File.Exists(filequest3))
            {
                StreamReader sr = new StreamReader(filequest3);
                string s0;
                for (int i = 0; i < 30; i++)
                {
                    s0 = sr.ReadLine();
                    str[i] = s0;
                }
                sr.Close();
                sr.Dispose();
                richTextBox1.Text = str[0];
            }
            button12.Hide();
            button3.Show();
            button3.Text = "Нет, неверно ";
            button4.Show();
            button4.Text = "Пожалуй, это так ";
            button15.Show();
            button15.Text = "Верно";
            button16.Show();
            button16.Text = "Совершенно верно";
            enter = 1;
            for (int i = 20; i < 89; i++)
            {
                otv[i] = null;
            }
        }

        private void button15_Click(object sender, EventArgs e)/////Дополнительные
        {
            otv[t] = 2.ToString();//1==да
            if (str[t+1] != null)
            {
                richTextBox1.Text = str[t+1];
            }
            else
            {
                MessageBox.Show("Тест пройден");
                button6.Show();
                button7.Show();
                button8.Show();
                button9.Show();
                button10.Show();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button15.Hide();
                button16.Hide();
                button17.Hide();
                richTextBox1.Text = " ";
                richTextBox1.Hide();
                t = 0;
                Save();
                enter = 0;
            }
            t++;
        }

        private void button16_Click(object sender, EventArgs e)/////Дополнительные
        {
            otv[t] = 3.ToString();//1==да
            if (str[t+1] != null)
            {
                richTextBox1.Text = str[t+1];
            }
            else
            {
                MessageBox.Show("Тест пройден");
                button6.Show();
                button7.Show();
                button8.Show();
                button9.Show();
                button10.Show();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button15.Hide();
                button16.Hide();
                button17.Hide();
                richTextBox1.Text = " ";
                richTextBox1.Hide();
                t = 0;
                Save();
                enter = 0;
            }
            t++;
        }

        void loadtabel3()//загрузка ответов для анализа test 3
        {
            if (File.Exists(fileotv))
            {
                StreamReader sr = new StreamReader(fileotv);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {
                    string[] ar = s0.Split(' ');
                    if (table[i, 0] == ar[0])
                        for (int j = 1; j < 31; j++)
                        {
                            if (ar[j] == "да")
                                table[i, j + 4] = 1.ToString();
                            else if (ar[j] == "нет")
                                table[i, j + 4] = 2.ToString();
                            else if (ar[j] == "2")
                                table[i, j + 4] = 3.ToString();
                            else if (ar[j] == "3")
                                table[i, j + 4] = 4.ToString();
                        }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }

        void Result3()//добовление к людям в таблице их ответы и результаты test 3
        {
            int result = 0;

            for (int t = 1; table[t, 1] != null; t++)
            {
                for (int i = 1; i < 31; i++)
                    result += Convert.ToInt32(table[t, i + 4]);

                if (result > 28 && result <= 50)
                    table[t, 36] = "Отсутствие астении (" + result + ")";
                else if (result > 50 && result <= 75)
                    table[t, 36] = "Слабая астения (" + result + ")";
                else if (result > 75 && result <= 100)
                    table[t, 36] = "Умеренная астения (" + result + ")";
                else if (result > 100 && result <= 120)
                    table[t, 36] = "Выраженная астения (" + result + ")";
            }
        }

        void Vivod3()//вывод test 3
        {
            for (int j = 1; table[j, 2] != null; j++)
            {
                string[] Stroka = { table[j, 1], table[j, 2], table[j, 3], table[j, 4], table[j, 36] };

                addGridParam(Stroka, dataGridView1);
            }
        }
        //ultramar3@yandex.ru/////////// Конец теста №3
        //////////////////////////////// Тест №4

        private void button9_Click(object sender, EventArgs e)//Тест №4
        {
            button6.Hide();
            button7.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            Nomer = 4;
            Entrance addPers = new Entrance();
            addPers.Tag = this;
            addPers.Show();
            button13.Location = new Point(300, 250);
            button13.Text = "Начать";
        }

        private void button13_Click(object sender, EventArgs e)//отображение вопросов
        {
            for (int i = 20; i < 89; i++)
            {
                str[i] = null;
            }
            richTextBox1.Show();
            if (File.Exists(filequest4))
            {
                StreamReader sr = new StreamReader(filequest4);
                string s0;
                for (int i = 0; i < 40; i++)
                {
                    s0 = sr.ReadLine();
                    str[i] = s0;
                }
                sr.Close();
                sr.Dispose();
                richTextBox1.Text = str[0];
            }
            button13.Hide();
            button3.Show();
            button3.Text = "Да";
            button4.Show();
            button4.Text = "Нет";
            enter = 1;
            for (int i = 20; i < 89; i++)
            {
                otv[i] = null;
            }
        }

        void loadtabel4()//загрузка ответов для анализа test 4
        {
            if (File.Exists(fileotv))
            {
                StreamReader sr = new StreamReader(fileotv);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {
                    string[] ar = s0.Split(' ');
                    if (table[i, 0] == ar[0])
                        for (int j = 1; j < 41; j++)
                        {
                            if (ar[j] == "да")
                                table[i, j + 4] = 1.ToString();
                            else 
                                table[i, j + 4] = 0.ToString();                            
                        }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }

        void Result4()//добовление к людям в таблице их ответы и результаты test 4
        {            
            res[0, 2] = "Предметно-действенное";
            res[1, 2] = "Абстрактно-символическое";
            res[2, 2] = "Словесно-логическое";
            res[3, 2] = "Наглядно-образное ";
            res[4, 2] = "Креативность (творческое)";

            res[0, 1] = 0.ToString();
            res[1, 1] = 0.ToString();
            res[2, 1] = 0.ToString();
            res[3, 1] = 0.ToString();
            res[4, 1] = 0.ToString();

            array = new int[,]
            {
                { 1, 6, 11, 16, 21, 26, 31, 36},
                { 2, 7, 12, 17, 22, 27, 32, 37},
                { 3, 8, 13, 18, 23, 28, 33, 38 },
                { 4, 9, 14, 19, 24, 29, 34, 39 },
                { 5, 10, 15, 20, 25, 30, 35, 40 }
            };
            for (int t = 1; table[t, 1] != null; t++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (array[i, j] != 0)
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 1)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 1).ToString();
                    }
                }
                table[t, 46] = res[0, 2] + res[0, 1];
                table[t, 47] = res[1, 2] + res[1, 1];
                table[t, 48] = res[2, 2] + res[2, 1];
                table[t, 49] = res[3, 2] + res[3, 1];
                table[t, 50] = res[4, 2] + res[4, 1];
            }
        }

        void Vivod4()//вывод test 4
        {
            for (int j = 1; table[j, 2] != null; j++)
            {
                string[] Stroka = { table[j, 1], table[j, 2], table[j, 3], table[j, 4], table[t+1, 46], table[t + 1, 47], table[t + 1, 48], table[t + 1, 49], table[t + 1, 50] };

                addGridParam(Stroka, dataGridView1);
            }
        }
        //ultramar3@yandex.ru/////////// Конец теста №4
        //////////////////////////////// Тест №5

        private void button10_Click(object sender, EventArgs e)
        {
            button6.Hide();
            button7.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            Nomer = 5;
            Entrance addPers = new Entrance();
            addPers.Tag = this;
            addPers.Show();
            button14.Location = new Point(300, 250);
            button14.Text = "Начать";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            for (int i = 20; i < 89; i++)
            {
                str[i] = null;
            }
            richTextBox1.Show();
            if (File.Exists(filequest5))
            {
                StreamReader sr = new StreamReader(filequest5);
                string s0;
                for (int i = 0; i < 15; i++)
                {
                    s0 = sr.ReadLine();
                    str[i] = s0;
                }
                sr.Close();
                sr.Dispose();
                richTextBox1.Text = str[0];
            }
            button14.Hide();
            button3.Show();
            button3.Text = "Полностью не согласен";
            button4.Show();
            button4.Text = "Скорее не согласен, чем согласен";
            button4.Width = 100;
            button15.Show();
            button17.Text = "Затрудняюсь ответить";
            button16.Show();
            button15.Text = "Скорее согласен, чем не согласен";
            button15.Width = 100;
            button17.Show();
            button16.Text = "Абсолютно согласен";
            button16.Location = new Point(619, 165);
            enter = 1;
            for (int i = 20; i < 89; i++)
            {
                otv[i] = null;
            }
        }

        void loadtabel5()//загрузка ответов для анализа test 5
        {
            if (File.Exists(fileotv))
            {
                StreamReader sr = new StreamReader(fileotv);
                string s0;
                for (int i = 1; (s0 = sr.ReadLine()) != null; i++)
                {
                    string[] ar = s0.Split(' ');
                    if (table[i, 0] == ar[0])
                        for (int j = 1; j < 16; j++)
                        {
                            if (ar[j] == "да")
                                table[i, j + 4] = 1.ToString();
                            else if (ar[j] == "нет")
                                table[i, j + 4] = 2.ToString();
                            else if (ar[j] == "2")
                                table[i, j + 4] = 4.ToString();
                            else if (ar[j] == "3")
                                table[i, j + 4] = 5.ToString();
                            else if (ar[j] == "4")
                                table[i, j + 4] = 3.ToString();
                        }
                    else { i--; }
                }
                sr.Close();
                sr.Dispose();
            }
        }

        private void button17_Click(object sender, EventArgs e)/////Дополнительные
        {
            otv[t] = 4.ToString();//1==да
            if (str[t+1] != null)
            {
                richTextBox1.Text = str[t+1];
            }
            else
            {
                MessageBox.Show("Тест пройден");
                button6.Show();
                button7.Show();
                button8.Show();
                button9.Show();
                button10.Show();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button17.Hide();
                button15.Hide();
                button16.Hide();
                richTextBox1.Text = " ";
                richTextBox1.Hide();
                t = 0;
                Save();
                enter = 0;
            }
            t++;
        }

        void Result5()//добовление к людям в таблице их ответы и результаты test 5
        {
            res[0, 2] = "Осведомленность и умелость в процедурных вопросах сдачи ЕГЭ";
            res[1, 2] = "Способность к самоорганизации и самоконтролю";
            res[2, 2] = "Экзаменационная тревожность";

            res[0, 1] = 0.ToString();
            res[1, 1] = 0.ToString();
            res[2, 1] = 0.ToString();
            array = new int[,]
            {
                { 1, 7, 10, 13},
                { 2, 5, 11, 14},
                { 3, 12, 0, 0 }
            };

            int[,] array1 = new int[,]
            {   { 4, 0, 0 },
                { 8, 0, 0},
                { 6, 9, 15,}
            };

            for (int t = 1; table[t, 1] != null; t++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (array[i, j] != 0)
                        {
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 1)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 1).ToString();
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 2)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 2).ToString();
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 3)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 3).ToString();
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 4)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 4).ToString();
                            if (Convert.ToInt32(table[t, (array[i, j]) + 4]) == 5)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 5).ToString();
                        }
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (array1[i, j] != 0)
                        {
                            if (Convert.ToInt32(table[t, (array1[i, j]) + 4]) == 1)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 5).ToString();
                            if (Convert.ToInt32(table[t, (array1[i, j]) + 4]) == 2)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 4).ToString();
                            if (Convert.ToInt32(table[t, (array1[i, j]) + 4]) == 3)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 3).ToString();
                            if (Convert.ToInt32(table[t, (array1[i, j]) + 4]) == 4)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 2).ToString();
                            if (Convert.ToInt32(table[t, (array1[i, j]) + 4]) == 5)
                                res[i, 1] = (Convert.ToInt32(res[i, 1]) + 1).ToString();
                        }
                    }
                }

                table[t, 21] = res[0, 2] + res[0, 1];
                table[t, 22] = res[1, 2] + res[1, 1];
                table[t, 23] = res[2, 2] + res[2, 1];
            }

        }

        void Vivod5()//вывод test 4\5
        {
            for (int j = 1; table[j, 2] != null; j++)
            {
                string[] Stroka = { table[j, 1], table[j, 2], table[j, 3], table[j, 4], table[t + 1, 21], table[t + 1, 22], table[t + 1, 23] };

                addGridParam(Stroka, dataGridView1);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//форму нельзя закрыть крестиком
        {
            if (enter == 1)
                e.Cancel = true;
            else if (enter == 2)
            {
                e.Cancel = true;
                listBox1.Show();
                listBox2.Show();
                listBox3.Show();
                button5.Show();
                dataGridView1.Hide();
                enter = 0;
                this.Width = 800;
                this.Height = 400;
                button1.Show();
            }
            else
                e.Cancel = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button6.Show();
            button7.Show();
            button8.Show();
            button9.Show();
            button10.Show();
            listBox1.Hide();
            listBox2.Hide();
            listBox3.Hide();
            button5.Hide();
            button1.Hide();
        }
    }
    
}
