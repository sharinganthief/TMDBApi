using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDBApi.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			Api api =new Api();
			var results = api.GetMoviesReleasedOnDate(5, DateTime.Now.AddDays(-1));

			var rand = new Random();
			for (int i = 0; i < 10; i++)
			{
				var curr = results[rand.Next(0, results.Count)];
				results.Remove(curr);
				Process.Start(string.Format("https://www.themoviedb.org/movie/{0}", curr.id));
			}
			

			//foreach (Result result in results)
			//{
			//	System.Console.WriteLine("Name - {0}; Date - {1}", result.title, result.release_date);
			//}

			System.Console.ReadLine();
		}
	}
}
