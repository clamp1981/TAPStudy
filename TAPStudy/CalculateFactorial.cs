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
				Thread.Sleep(50);
			}
			Console.WriteLine($"CalculateFactorial : {output}");

			return output;
		}

		public BigInteger Calculate(int input, IProgress<int> progress)
		{
			BigInteger output = 1;
			for (int i = 0; i < input; i++)
			{
				output *= (i + 1);
				if (progress != null)
				{
					int percent = (int)( (float)(i + 1) / (float)input) * 100;					
					progress.Report(percent);

				}
				Thread.Sleep(50);
				

			}
			Console.WriteLine($"CalculateFactorial : {output}");

			return output;
		}

		public BigInteger Calculate(int input, int index, IProgress<ProgressEventArgs> progress)
		{
			BigInteger output = 1;
			for (int i = 0; i < input; i++)
			{
				output *= (i + 1);
                if (progress != null)
                {
                    int percent = (int)(((float)((float)(i + 1) / (float)input)) * 100);
                    Console.WriteLine($"Calculate percent : {percent}");
                    progress.Report(new ProgressEventArgs(index, percent));

                }
                Thread.Sleep(50);


			}
			Console.WriteLine($"CalculateFactorial : {output}");

			return output;
		}


		



	}
}
