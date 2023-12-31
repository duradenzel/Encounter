﻿using EncounterModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using EncounterInterfaces;
using Microsoft.Extensions.Caching.Memory;



namespace EncounterDAL
{
    public class MonsterApi : IMonsterApiService
    {
        
        private readonly string baseUrl = "https://api.open5e.com/monsters/?limit=500";
        private readonly IMemoryCache memoryCache;



        public MonsterApi(IMemoryCache memoryCache)
        {              
            this.memoryCache = memoryCache;
        }

        public async Task<List<Monster>> GetMonsters(int desiredXpValue, HttpClient _httpClient)
        {
         
            if (memoryCache.TryGetValue("AllMonsters", out List<Monster> allMonsters))
            {
                return SelectMonsters(allMonsters, desiredXpValue);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                ApiResponse<Monster> apiResponse = JsonConvert.DeserializeObject<ApiResponse<Monster>>(jsondata);
                if (apiResponse != null && apiResponse.Results != null)
                {
                    allMonsters = apiResponse.Results;

                   
                    memoryCache.Set("AllMonsters", allMonsters, TimeSpan.FromHours(24));

                    return SelectMonsters(allMonsters, desiredXpValue);
                }
            }

            return null;
        }

        public async Task<List<Monster>> GetMonsterList(HttpClient _httpClient)
        {

            if (memoryCache.TryGetValue("AllMonsters", out List<Monster> allMonsters))
            {
                return allMonsters;
            }

            HttpResponseMessage response = await _httpClient.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsondata = await response.Content.ReadAsStringAsync();
                ApiResponse<Monster> apiResponse = JsonConvert.DeserializeObject<ApiResponse<Monster>>(jsondata);
                if (apiResponse != null && apiResponse.Results != null)
                {
                    allMonsters = apiResponse.Results;
                    foreach (var monster in allMonsters)
                {
                    if (ExpMapper.CRToExpMap.TryGetValue(monster.CR, out int expValue) && expValue > 0)
                    {
                        monster.ExperiencePoints = expValue;
                    }
                }


                    memoryCache.Set("AllMonsters", allMonsters, TimeSpan.FromHours(24));

                    return allMonsters;
                }
            }

            return null;
        }



        private List<Monster> SelectMonsters(List<Monster> allMonsters, int desiredXpValue)
        {
            List<Monster> selectedMonsters = new List<Monster>();
            int currentXpValue = 0;
            Random random = new Random();

            while (currentXpValue < desiredXpValue)
            {
                allMonsters = allMonsters.OrderBy(x => random.Next()).ToList();

                foreach (var monster in allMonsters)
                {
                    if (ExpMapper.CRToExpMap.TryGetValue(monster.CR, out int expValue) && expValue > 0)
                    {
                        if (currentXpValue + expValue <= desiredXpValue)
                        {
                            monster.ExperiencePoints = expValue;
                            selectedMonsters.Add(monster);
                            currentXpValue += expValue;
                        }
                    }
                }
            }

            return selectedMonsters;
        }

    }

}