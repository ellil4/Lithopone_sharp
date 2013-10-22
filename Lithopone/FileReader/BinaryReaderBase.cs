using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lithopone.Memory;
using System.Windows.Forms;

namespace Lithopone.FileReader
{
    public class BinaryReaderBase
    {
        public enum BLOCKTYPE { TEST, CONFIG, RESOURCE };

        public FileStream mFs;
        public BinaryReader mBr;

        public void Begin(String path)
        {
            try
            {
                mFs = new FileStream(path, FileMode.Open, FileAccess.Read);
                mBr = new BinaryReader(mFs);
            }
            catch (Exception)
            {
                MessageBox.Show("源文件不存在");
                System.Environment.Exit(0);
            }

            Begin2();
        }

        public virtual void Begin2()
        { }

        public StBlockDscription GetBlockInfo(BLOCKTYPE type)
        {
            int posToSet = -1;

            switch (type)
            {
                case BLOCKTYPE.TEST:
                    posToSet = 8;
                    break;
                case BLOCKTYPE.CONFIG:
                    posToSet = 16;
                    break;
                case BLOCKTYPE.RESOURCE:
                    posToSet = 24;
                    break;
            }

            mFs.Position = posToSet;

            StBlockDscription retval = new StBlockDscription();

            retval.Begin = ReadInt();
            retval.Length = ReadInt();

            return retval;
        }

        public int ReadInt()
        {
            int retval = Int16.MaxValue;

            byte[] bArr = mBr.ReadBytes(4);
            Array.Reverse(bArr);
            retval = BitConverter.ToInt32(bArr, 0);

            return retval;
        }

        public long ReadLong()
        {
            long retval = Int64.MaxValue;

            byte[] bArr = mBr.ReadBytes(8);
            Array.Reverse(bArr);
            retval = BitConverter.ToInt64(bArr, 0);

            return retval;
        }

        public byte ReadByte()
        {
            return mBr.ReadByte();
        }

        public void End()
        {
            if (mBr != null)
            {
                mBr.Close();
                mBr = null;
            }

            if (mFs != null)
            {
                mFs.Close();
                mFs = null;
            }
        }

        ~BinaryReaderBase()
        {
            End();
        }
    }
}
