﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientAbstractions
{
    public interface IJokeClient
    {
        Task<string> GetJoke();
    }
}
