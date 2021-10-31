﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Services;
using Pokedex.Api.ViewModels;
using System.Threading.Tasks;

namespace Pokedex.Api.Controllers
{
    /*
     * TO:DO Improve the swagger documentation for each endpoint.
     */
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> logger;
        private readonly IPokedexService pokedexService;

        public PokemonController(
            ILogger<PokemonController> logger,
            IPokedexService pokedexService)
        {
            this.logger = logger;
            this.pokedexService = pokedexService;
        }

        [HttpGet("{name}")]
        public async Task<PokemonViewModel> GetPokemon(string name)
        {
            return await pokedexService.GetPokemonDetails(name);
        }

        [HttpGet]
        [Route("translated/{name}")]
        public async Task<PokemonViewModel> GetPokemonWithTranslatedDetails(string name)
        {
            return await pokedexService.GetPokemonDetails(name, translate: true);
        }
    }
}
