using Microsoft.Extensions.Configuration;
using Movie.Demo.Repository.IInterface;
using Movie.Demo.Utility.Enumeration;
using System;
using System.Net.Http;

namespace Movie.Demo.Repository.Implimentation
{
    public class MovieRepository : IMovieRepository
    {
        private HttpClient _httpClient = null;
        private readonly IConfiguration _IConfiguration = null;
        private readonly string _ApiKey;

        public MovieRepository(IConfiguration iConfiguration, HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._IConfiguration = iConfiguration;
            this._ApiKey = this._IConfiguration.GetSection(URL.Uri.ToString()).GetSection(URL.ApiKey.ToString()).Value;
        }
        public string GetMovies(string langCode)
        {

            string movieUrl = this._IConfiguration.GetSection(URL.Uri.ToString()).GetSection(URL.MovieUrl.ToString()).Value;

            Uri movieUri = new Uri(movieUrl + "popular?api_key=" + this._ApiKey + "&language=" + langCode);

            HttpResponseMessage response = _httpClient.GetAsync(movieUri).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
        public string SearchMovies(string langCode, string query, int? pageNumber)
        {
            string movieUrl = this._IConfiguration.GetSection(URL.Uri.ToString()).GetSection(URL.SearchUrl.ToString()).Value;
            movieUrl += "?api_key=" + this._ApiKey + "&language=" + langCode + "&query=" + query;
            if (pageNumber.HasValue && pageNumber.Value > 0)
            {
                movieUrl += "&page=" + pageNumber;
            }

            Uri movieUri = new Uri(movieUrl);

            HttpResponseMessage response = _httpClient.GetAsync(movieUri).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
        public string GetMovieDetails(string langCode, int id)
        {
            string movieUrl = this._IConfiguration.GetSection(URL.Uri.ToString()).GetSection(URL.MovieUrl.ToString()).Value;

            Uri movieUri = new Uri(movieUrl + id + "?api_key=" + this._ApiKey + "&language=" + langCode);

            HttpResponseMessage response = _httpClient.GetAsync(movieUri).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
