using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lithopone.Memory;

namespace Lithopone.FileReader
{

    public class PackageReader : BinaryReaderBase
    {
        public void ExtractFile(string path, BLOCKTYPE type)
        {
            FileStream fso = new FileStream(path, FileMode.Create, FileAccess.Write);
            StBlockDscription dscp = GetBlockInfo(type);
            mFs.Position = dscp.Begin;

            int red = 0;
            int thisRed = 0;
            int buflen = 1024;
            int readingPlan = 0;
            int left = 0;

            readingPlan = buflen < dscp.Length ? buflen : dscp.Length;

            byte[] buf = new byte[1024];

            bool doRead = true;
            while (doRead)
            {
                //counting
                thisRed = mFs.Read(buf, 0, readingPlan);
                fso.Write(buf, 0, thisRed);
                red += thisRed;

                //make plan
                left = dscp.Length - red;
                if (left >= buflen)
                {
                    readingPlan = buflen;
                }
                else
                {
                    readingPlan = left;
                }

                //if stop
                if (red == dscp.Length)
                    doRead = false;
            }

            fso.Close();
        }

        public StPFileInfo GetPFileInfo()
        {
            StPFileInfo retval = new StPFileInfo();

            mFs.Position = 0;

            retval.Version = ReadInt();
            retval.Mode = ReadByte();
            retval.ResourceStatus = ReadByte();

            return retval;
        }
    }
}
