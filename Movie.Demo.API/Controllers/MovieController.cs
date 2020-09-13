using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Movie.Demo.API.Helpers;
using Movie.Demo.API.Loggers;
using Movie.Demo.Repository.IInterface;
using Movie.Demo.Utility.Enumeration;
using Serilog;
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
        private ILogger _Logger = Log.ForContext<MovieController>();
        private LogEntity _LogEntity = null;
        public MovieController(IMovieRepository iMovieRepository, IConfiguration iConfiguration, IHostingEnvironment hostingEnvironment)
        {
            this._IMovieRepository = iMovieRepository;
            this._IConfiguration = iConfiguration;
            this._hostingEnvironment = hostingEnvironment;
            _LogEntity = new LogEntity();
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
            _LogEntity.LangCode = langCode;
            try
            {
                #region Log
                _Logger.Here().Information(Helper.LogContextControllerInfo(_LogEntity));
                #endregion

                var result = _IMovieRepository.GetMovies(langCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                #region Log
                _Logger.Error(Helper.LogContextErrorInfo(Layer.MovieController.ToString(), Method.MovieList.ToString(), _LogEntity, ex.Message, ex.InnerException, ex.StackTrace));
                #endregion

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

            _LogEntity.LangCode = langCode;
            _LogEntity.Query = query;
            _LogEntity.PageNumber = page;
            try
            {
                var result = _IMovieRepository.SearchMovies(langCode, query, page);

                return Ok(result);
            }
            catch (Exception ex)
            {
                #region Log
                _Logger.Error(Helper.LogContextErrorInfo(Layer.MovieController.ToString(), Method.SearchMovies.ToString(), _LogEntity, ex.Message, ex.InnerException, ex.StackTrace));
                #endregion

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
            _LogEntity.LangCode = langCode;
            _LogEntity.Id = id;
            try
            {
                var result = _IMovieRepository.GetMovieDetails(langCode, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                #region Log
                _Logger.Error(Helper.LogContextErrorInfo(Layer.MovieController.ToString(), Method.MovieDetails.ToString(), _LogEntity, ex.Message, ex.InnerException, ex.StackTrace));
                #endregion

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}