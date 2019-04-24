using System;
using System.Threading.Tasks;
using ClientAbstractions;

namespace MockJokeClient
{
    public class MockJokeClient : IJokeClient
    {
        public Task<string> GetJoke()
        {
            return Task.FromResult("Did you hear about the restaurant on the moon? Great food, no atmosphere.");
        }
    }
}
