using System;

namespace Pokedex.Api.Exceptions
{
    [Serializable]
    public class PokemonDetailsRetrievalException : Exception
    {
        public PokemonDetailsRetrievalException()
        {
        }

        public PokemonDetailsRetrievalException(string message) : base(message)
        {
        }

        public PokemonDetailsRetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
