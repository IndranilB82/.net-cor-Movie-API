using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Demo.Repository.IInterface
{
    public interface IMovieRepository
    {
        string GetMovies(string langCode);
        string SearchMovies(string langCode, string query, int? pageNumber);
        string GetMovieDetails(string langCode, int id);

    }
}
