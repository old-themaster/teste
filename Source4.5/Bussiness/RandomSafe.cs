using System;
using System.Security.Cryptography;

namespace Game.Bussiness
{
    public class RandomSafe : Random
    {
        public RandomSafe() : base(random_0())
        {

        }

        public override int Next(int max)
        {
            return Next(0, max);
        }

        public override int Next(int min, int max)
        {
            int num = base.Next(1, 50);
            int result = max - 1;
            for (int i = 0; i < num; i++)
            {
                result = base.Next(min, max);
            }
            return result;
        }

        public int NextSmallValue(int min, int max)
        {
            int num = Math.Abs(Next(min, max) - max);
            if (num > max)
            {
                num = max;
            }
            else if (num < min)
            {
                num = min;
            }
            return num;
        }

        private static int random_0()
        {
            byte[] array = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }
    }
}