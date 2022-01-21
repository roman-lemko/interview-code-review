using System;
using System.Net.Http;

namespace LegacyUserMgmt
{
    public static class UserRatingService
    {
        public static int GetUserRating(User user, bool useCache, bool saveToDb = true)
        {
            if (user.Type == UserType.SuperUser)
            {
                // admin or moderator
                return user.Id == "3aa857aa-77bf-4f8e-8545-3fffca0aa20d" ? 9999 : 1000;
            }
            else
            {
                // First, try get from cache if flag is true
                if (useCache)
                {
                    var cacheSingleton = UserRatingCache.GetInstance();
                    if (cacheSingleton.FindById(user.Id) != null)
                    {
                        var entry = cacheSingleton.FindById(user.Id);
                        return entry.Rating;
                    }
                }
                else if (user.LastModifiedDate > new DateTime(2015, 7, 20))
                {
                    // For users active after 07.20.2015 calculate based on data from db 
                    var sqlDbRepository = new SqlDbRepository("Server=AppServer2\\SQLSERVER;Database=appDb;User Id=admin;Password=1qazxsw2;");
                    var data = sqlDbRepository.GetUserDataAsync(user.Id).Result;
                    var value1 = data.PostsWritten;
                    var value2 = data.CommentsWritten / 30;
                    if (saveToDb)
                        sqlDbRepository.SaveUserRatingAsync(user.Id, (value1 + value2) / 2);
                    return (value1 + value2) / 2;
                }
                else
                {
                    // For old users get rating from legacy API
                    var client = new HttpClient();
                    var request = new HttpRequestMessage();
                    request.Headers.Add("Authorization", $"Basic {LegacyApiCredsProvider.GetBasicAuth()}");
                    var response = client.GetAsync("http://legacyapi.com/users/" + user.Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return int.Parse(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        // error
                        return -1;
                    }
                }
            }

            return -1;
        }
    }
}
