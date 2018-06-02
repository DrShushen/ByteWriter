using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ByteWriter
{
    class DataStructure
    {
        public Int32 DataId { get; set; }
        public double X { get; set; }
        public bool trueFalse { get; set; }
        public Color col { get; set; }

        public void BinaryWrite(BinaryWriter bw)
        {
            bw.Write(DataId);
            bw.Write(X);
            bw.Write(trueFalse);

            // Bytes. R, G, B.
            bw.Write(col.R);
            bw.Write(col.G);
            bw.Write(col.B);
        }

        public static DataStructure BinaryRead(BinaryReader br)
        {
            DataStructure result = new DataStructure();

            result.DataId = br.ReadInt32();
            result.X = br.ReadDouble();
            result.trueFalse = br.ReadBoolean();

            // Bytes. R, G, B.
            byte r = br.ReadByte();
            byte g = br.ReadByte();
            byte b = br.ReadByte();
            result.col = Color.FromArgb(r, g, b);  // Generate color from RGB.

            return result;
        }
    }

    // Extension.
    public static class BinaryReaderEndOfFileExtension
    {

        public static bool EndOfFile(this BinaryReader binaryReader)
        {
            var bs = binaryReader.BaseStream;
            return (bs.Position == bs.Length);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<DataStructure> dataStructures = 
                new List<DataStructure>()
                {
                    new DataStructure() { DataId = 7, X = 715.1241, trueFalse = true, col = Color.Blue },
                    new DataStructure() { DataId = -5, X = -0.912933, trueFalse = false, col = Color.Red }
                };

            string fileName = @"C:\Users\Evgany Saveliev\Desktop\test1.bin";
            using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (DataStructure ds in dataStructures)
                {
                    ds.BinaryWrite(bw);
                }
            }

            List<DataStructure> dataStructuresFromFile = new List<DataStructure>();
            if (File.Exists(fileName))
            {
                using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    while (!br.EndOfFile())
                    {
                        dataStructuresFromFile.Add(DataStructure.BinaryRead(br));
                    }
                }
            }

        }
    }
}
