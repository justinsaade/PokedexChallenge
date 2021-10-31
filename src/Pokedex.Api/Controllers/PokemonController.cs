using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Models;
using System;
using System.Collections.Generic;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{pokemonName}")]
        public IEnumerable<PokemonViewModel> GetPokemon(string pokemonName)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("translated/{pokemonName}")]
        public IEnumerable<PokemonViewModel> GetPokemonWithTranslatedDetails(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
