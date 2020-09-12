using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Movie.Demo.Repository.IInterface;
using System;
using System.Collections.Generic;
using System.IO;

namespace Movie.Demo.API.Controllers
{
    [Route("api/movie")]
    public class MovieController : ControllerBase
    {
        private IConfiguration _IConfiguration = null;
        private IHostingEnvironment _hostingEnvironment = null;
        private IMovieRepository _IMovieRepository = null;
        public MovieController(IMovieRepository iMovieRepository, IConfiguration iConfiguration, IHostingEnvironment hostingEnvironment)
        {
            this._IMovieRepository = iMovieRepository;
            this._IConfiguration = iConfiguration;
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { Directory.GetCurrentDirectory(), _IConfiguration.GetSection("Uri").GetSection("MovieUrl").Value };
        }

        [HttpGet]
        [Route("MovieList/{langCode}")]
        public IActionResult MovieList(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
            {
                return BadRequest();
            }
            try
            {
                var result = _IMovieRepository.GetMovies(langCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("SearchMovies/{langCode}/{query}/{page?}")]
        public IActionResult SearchMovies(string langCode, string query, int? page)
        {
            if (string.IsNullOrEmpty(langCode) || string.IsNullOrEmpty(query))
            {
                return BadRequest();
            }
            try
            {
                var result = _IMovieRepository.SearchMovies(langCode, query, page);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("MovieDetails/{langCode}/{id}")]
        public IActionResult MovieDetails(string langCode, int id)
        {
            if (string.IsNullOrEmpty(langCode) || id == 0)
            {
                return BadRequest();
            }
            try
            {
                var result = _IMovieRepository.GetMovieDetails(langCode, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}