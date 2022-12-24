using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace zia51
{
    class Register
    {
        public int n { get; set; }
        public BitArray arr { get; set; }

        private int[] forXor;

        public int[] ForXor
        {
            get
            {
                return this.forXor;
            }
            set
            {
                this.forXor = value;
            }
        }
        public Register(int n)
        {
            arr = new BitArray(n);
        }       
        public void shift()
        {
            arr.LeftShift(1);
        }
        public void XorFirst()
        {
            bool x =Convert.ToBoolean(arr[ForXor[0]]);            
            for (int j = 1; j < ForXor.Length; j++)
            {
                x ^= arr[ForXor[j]];
            }            
            arr[0] = x;
        }
    }
}
