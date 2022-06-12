using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TAPStudy
{
    public class CalculateFactorial
    {
		public BigInteger Calculate(int input)
		{
			BigInteger output = 1;
			for (int i = 0; i < input; i++)
			{
				output *= (i + 1);
				Thread.Sleep(100);
			}
			Console.WriteLine($"CalculateFactorial : {output}");

			return output;
		}
	}
}
