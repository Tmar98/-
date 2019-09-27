using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Опросник
{
    public class Class
    {
        public string School { get; set; }
        public string Klass { get; set; }

        public Class( string S, string K)
        {
             School = S; Klass = K;

        }
    }
    public class Classes : List<Class>
    {
        string fileName;
        public Classes() { }
        public Classes(string _fileName)
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
                        Class ps = new Class( ar[0], ar[1]);
                        this.Add(ps);
                    }
                }
                sr.Close();
                sr.Dispose();
            }
        }
    }
}
