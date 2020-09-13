namespace Movie.Demo.Utility.Enumeration
{
    public enum URL
    {
        Uri,
        MovieUrl,
        SearchUrl,
        ApiKey,
        SeqUrl
    }

    public enum Layer
    {
        MovieController,
        ExceptionMiddleware
    }

    public enum Method
    {
        MovieList,
        SearchMovies,
        MovieDetails,
        ExceptionHandler
    }
}
