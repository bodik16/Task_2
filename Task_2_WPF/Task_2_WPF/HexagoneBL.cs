using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Task2.Classes;

namespace Task2.BL
{
    public class HexagoneBL
    {
        public static void SerializeList(List<Hexagone> lines, string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Hexagone>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, lines);
            }
        }

        public static List<Hexagone> DeserializeList(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Hexagone>));
            List<Hexagone> lin = null;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                lin = (List<Hexagone>)formatter.Deserialize(fs);
            }

            if (lin == null)
            {
                throw new ApplicationException(string.Format("cannot deserialize file {0}", path));
            }

            return lin;
        }
    }
}
