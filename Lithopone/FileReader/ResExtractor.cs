using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lithopone.Memory;
using System.IO;

namespace Lithopone.FileReader
{
    public class ResExtractor : BinaryReaderBase
    {
        public static int BUFFERLEN = 1024;

        public StBlockDscription mBlockDscp;
        public int mResCount = 0;

        override public void Begin2()
        {
            mBlockDscp = GetBlockInfo(BLOCKTYPE.RESOURCE);
            
            mFs.Position = mBlockDscp.Begin;
            mResCount = ReadInt();
        }

        public long GetIndexBeg(int index)
        {
            long retval = -1;
            retval = index * 24 + mBlockDscp.Begin + 4;
            return retval;
        }

        public StResourceDscp GetResourceDscp(int index)
        {
            StResourceDscp retval = new StResourceDscp();

            mFs.Position = GetIndexBeg(index);

            retval.beg = ReadLong();
            retval.len = ReadLong();
            retval.type = ReadLong();

            return retval;
        }

        public static String GetExtendName(StResourceDscp resDscp)
        {
            String ret = "";

            if (resDscp.type == 0)
            {
                ret = ".jpg";
            }
            else if (resDscp.type == 1)
            {
                ret = ".png";
            }

            return ret;
            
        }

        public int GetResourceCount()
        {
            mFs.Position = mBlockDscp.Begin;
            int retval = ReadInt();
            return retval;
        }

        //extract a whole file into memory
        public byte[] ExtractRes2Mem(int index)
        {
            byte[] retval = null;
            StResourceDscp dscp = GetResourceDscp(index);
            mFs.Position = getResourceDataBegpos(index);
            retval = mBr.ReadBytes((int)dscp.len);
            return retval;
        }

        //get resource data begin pos of a whole picture file (not just its data part)
        private long getResourceDataBegpos(int index)
        {
            return GetResourceDscp(index).beg + mBlockDscp.Begin + 4 + 24 * mResCount;
        }

        //extract file to hard disk (currently not in use)
        public void ExtractRes(String destFolder, int rangeStart, int count)
        {
            String path = destFolder;

            if (rangeStart >= 0 && count > 0 && count <= mResCount)
            {
                int rangeEnd = rangeStart + count;

                StResourceDscp resDscp = null;

                byte[] buffer = new byte[BUFFERLEN];

                for (int i = rangeStart; i < rangeEnd; i++)
                {
                    resDscp = GetResourceDscp(i);

                    FileStream fso = 
                        new FileStream(path + i.ToString() + GetExtendName(resDscp), 
                        FileMode.Create, FileAccess.Write);

                    mFs.Position = getResourceDataBegpos(i);

                    if (resDscp.len < BUFFERLEN)
                    {
                        mBr.Read(buffer, (int)resDscp.beg, (int)resDscp.len);
                        fso.Write(buffer, 0, (int)resDscp.len);
                    }
                    else
                    {
                        int time = (int)(resDscp.len / BUFFERLEN + 1);
                        int lastLen = (int)(resDscp.len % BUFFERLEN);
                        int red = 0;

                        for (int j = 0; j < time; j++)
                        {
                            if (j != time - 1)
                            {
                                red = mBr.Read(buffer, 0, BUFFERLEN);
                                fso.Write(buffer, 0, BUFFERLEN);
                            }
                            else 
                            {
                                red = mBr.Read(buffer, 0, lastLen);
                                fso.Write(buffer, 0, lastLen);
                            }

                        }
                    }

                    fso.Close();
                }
            }
        }
    }
}
