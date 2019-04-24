using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientAbstractions;

namespace DadJokeClient
{
    public class DadJokeClient : IJokeClient
    {
        public HttpClient Client { get; set; }
        public DadJokeClient(HttpClient client)
        {
            this.Client = client; //Bad bad bad
            this.Client.DefaultRequestHeaders.Add("Accept", "application/json");
            this.Client.DefaultRequestHeaders.Add("User-Agent", "DevTalks Client");
        }
        
        public async Task<string> GetJoke()
        {
            var result = await this.Client.GetAsync("https://icanhazdadjoke.com/");
            var joke = await result.Content.ReadAsJsonAsync<DadJoke>();
            return joke.joke;
        }

        public class DadJoke
        {
            public string id { get; set; }
            public string joke { get; set; }
            public int status { get; set; }
        }
    }
}
