using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace zia51
{
    class A51
    {
        private   Register R1 = new Register(19);
        private   Register R2 = new Register(22);
        private   Register R3 = new Register(23);
        private   int  majR1=11,majR2=11,majR3=11;
        private   BitArray key;
        private   BitArray crypt;
        public A51()
        {
            R1.ForXor = new int[] {8,11,17};
            R2.ForXor = new int[] {4,15,16,17};
            R3.ForXor = new int[] {15,16,17,18};
        }
        public A51(string arr):this() //0011100011000110011000110011100111001110101110011001100011101001
        {
            if (arr.Length == 64)
            {
                key = new BitArray(64);
                for (int i = 0; i < 64; i++)
                    key[i] = Convert.ToBoolean(Convert.ToInt16(arr[i] - '0'));
                this.Load();
            }
        }
        public BitArray Key
        {
            get { return this.key; }
            set { this.key = value; }
        }
        public BitArray Crypt(BitArray arr)
        {
            crypt = new BitArray(arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                bool maj = (R1.arr[majR1] & R2.arr[majR2]) ^ (R1.arr[majR1] & R3.arr[majR3]) ^ (R2.arr[majR2] & R3.arr[majR3]);
                if (R1.arr[majR1] == maj)
                {
                    R1.shift();
                    R1.XorFirst();
                }
                if (R2.arr[majR2] == maj)
                {
                    R2.shift();
                    R2.XorFirst();
                }
                if (R3.arr[majR3] == maj)
                {
                    R3.shift();
                    R3.XorFirst();
                }
                crypt[i] = R1.arr[R1.arr.Length - 1] ^ R2.arr[R2.arr.Length - 1] ^ R3.arr[R3.arr.Length - 1] ^ arr[i];
            }           
            return crypt;
        }

        public void Load()
        {
            for (int i = 0; i < 64; i++)
            {
                if (i < 19)                
                    R1.arr[i] = Convert.ToBoolean(key[i]);                
                else
                {
                    if (i < 41)
                        R2.arr[i - 19] = Convert.ToBoolean(key[i]);
                    else
                        R3.arr[i - 41] = Convert.ToBoolean(key[i]);
                }
            }
        }
    }
}
