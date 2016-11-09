using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TMDBApi
{
    public class Api
    {
	    private const string tmdbApiKey = "5e58d4e6624fddc921ee9e1086dd323f";
	    private const string baseTmdbUrl = "https://api.themoviedb.org";
	    private static readonly string version3 = string.Format("{0}/3", baseTmdbUrl);
		private readonly string discover = string.Format("{0}/discover/movie", version3);


		public List<Result> GetMoviesReleasedOnDate(int yearsBack = 5, DateTime? initialDate = null, int take = 10)
	    {
		    var dateToUse = initialDate ?? DateTime.Now;

		    var returnObj = new List<Result>();

		    var baseUrl =
			    string.Format(
				    "{0}?api_key={1}&language=en-US&sort_by=popularity.desc&include_adult=false&include_video=false&page=1",
				    discover, tmdbApiKey);

		    for (int i = 0; i < yearsBack; i++)
		    {

				var date = dateToUse.AddYears(i * -1).ToString("yyyy-MM-dd");

				var url = string.Format("{0}&primary_release_date.gte={1}&primary_release_date.lte={1}", baseUrl,
				    date);
			    var response = MakeCall<RootObject>(url);

				returnObj.AddRange(response.Data.results);

			    var totalPages = response.Data.total_pages;

			    if (totalPages <= 1) continue;

			    for(int j = 2; j <= totalPages; j++)
			    {
				    string nextUrl = url.Replace("page=1", "page=" + j);
				    var newResponse = MakeCall<RootObject>(nextUrl);
				    returnObj.AddRange(newResponse.Data.results);
			    }
		    }

		    return returnObj.OrderByDescending(o => o.popularity).Take(take).ToList();

	    }

	    private IRestResponse<T> MakeCall<T>(string url) where T: class, new()
	    {
		     var client = new RestClient(url);
				var request = new RestRequest(Method.GET);
				request.AddParameter("undefined", "{}", ParameterType.RequestBody);
				IRestResponse<T> response = client.Execute<T>(request);
		    return response;
	    }
    }
}
