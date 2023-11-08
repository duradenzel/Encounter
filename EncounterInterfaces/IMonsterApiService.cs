﻿using EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncounterInterfaces
{
    public interface IMonsterApiService
    {
        Task<List<Monster>> GetMonsters(int desiredXpValue);
        Task<List<Monster>> GetMonsterList();

    }
}
