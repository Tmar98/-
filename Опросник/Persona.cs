using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Опросник
{
    public class Persona
    {
        public int Id { get; set; }
        public int Nomertest { get; set; }
        public string FIO { get; set; }
        public string School { get; set; }
        public string Class { get; set; }
        public DateTime date { get; set; }
        //ultramar3 @yandex.ru
        public Persona(int id, int N,string F, string S, string C,DateTime D)
        {
            Id = id; Nomertest = N; FIO = F; School = S; Class = C; date = D;

        }
        public Persona()
        {
            Id = -1;
            FIO = string.Empty; School = string.Empty; Class = string.Empty;   
        }
    }

    public class Persons : List<Persona>
    {
        string fileName;
        public Persons() { }
        public Persons(string _fileName)
        {
            fileName = _fileName;
            if (!string.IsNullOrEmpty(_fileName))
            {
                GetListFromFile(fileName);
            }
        }
        public void GetListFromFile(string FileName)
        {
            fileName = FileName;
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(FileName);
                string s0;
                while ((s0 = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(s0))
                    {
                        string[] ar = s0.Split(' ');
                        Persona ps = new Persona(int.Parse(ar[0]), int.Parse(ar[1]), ar[2], ar[3],ar[4],Convert.ToDateTime(ar[5]));
                        this.Add(ps);
                    }
                }
                sr.Close();
                sr.Dispose();
            }
        }
        public void AddWrite(Persona ps)
        {
            this.Add(ps);
            FileStream fs;
            if (File.Exists(fileName))
            {
                fs = File.Create(fileName);
            }
            else fs = File.Open(fileName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} ", ps.Id, ps.Nomertest, ps.FIO, ps.School, ps.Class, ps.date));
            sw.Close();
            sw.Dispose();
        }
        //ultramar3@yandex.ru
        public void AddNew(Persona ps)
        {
            this.Add(ps);
            FileStream fs;
            if (!File.Exists(fileName))
            {
                fs = File.Create(fileName);
            }
            else
                fs = File.Open(fileName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} ", ps.Id, ps.Nomertest, ps.FIO, ps.School, ps.Class, ps.date));
            sw.Close();
            sw.Dispose();
        }



    }
}
