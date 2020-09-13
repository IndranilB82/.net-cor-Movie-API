using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movie.Demo.Repository.Implimentation;
using Movie.Demo.Test.Entity;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using FluentAssertions;

namespace Movie.Demo.Test
{
    [TestClass]
    public class MovieRepositoryTest
    {
        #region Member Variable
        private MovieRepository _movieRepository;
        Mock<IConfiguration> _IConfiguration = new Mock<IConfiguration>();
        Mock<HttpClient> _HttpClient = new Mock<HttpClient>();
        #endregion

        #region Constructor
        public MovieRepositoryTest()
        {
            _movieRepository = new MovieRepository( _IConfiguration.Object, _HttpClient.Object);
        }
        #endregion

        #region public Method

        #region SearchMovies

        #region Positive Test Case

        [TestMethod]
        public void SearchMoviesPositive()
        {
            try
            {
                //Arrange
                int result = 664345;

                //Act
                var output = _movieRepository.SearchMovies("en", "kung fu mulan", 1);
                Search search = JsonConvert.DeserializeObject<Search>(output);

                //Assert
                result.ShouldBeEquivalentTo(search.results[0].id);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Null response from SearchMovies method of MovieRepository", ex.Message);
            }

        }

        #endregion

        #region Negetive Test case

        [TestMethod]
        public void SearchMoviesNegative()
        {
            try
            {
                //Arrange
                bool result = true;

                //Act
                var output = _movieRepository.SearchMovies("en", "kung fu mulan", 1);
                Search search = JsonConvert.DeserializeObject<Search>(output);

                //Assert
                result.ShouldBeEquivalentTo(search.results[0].video);

            }
            catch (Exception ex)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
            }
        }


        #endregion

        #endregion

        #endregion
    }
}
