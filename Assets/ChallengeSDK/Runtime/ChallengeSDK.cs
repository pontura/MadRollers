using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Catoff
{
    public class ChallengeSDK
    {
        private readonly string apiBaseUrl;
        private readonly string apiKey;

        public ChallengeSDK(string baseUrl, string apiKey)
        {
            apiBaseUrl = baseUrl;
            this.apiKey = apiKey;
        }

        public async Task<string> CreateChallenge(CreateChallengeDto challenge)
        {
            string url = $"{apiBaseUrl}/challenge";
            return await SendPostRequest(url, challenge);
        }

        public async Task<string> CreatePlayer(int challengeID, string userAddress)
        {
            string url = $"{apiBaseUrl}/player";
            var requestBody = new { ChallengeID = challengeID, UserAddress = userAddress };
            return await SendPostRequest(url, requestBody);
        }

        public async Task<string> UpdatePlayerScore(int challengeID, int updatedScore, string userAddress)
        {
            string url = $"{apiBaseUrl}/player/updatePlayerScore";
            var requestBody = new { ChallengeID = challengeID, UpdatedScore = updatedScore, UserAddress = userAddress };
            return await SendPostRequest(url, requestBody);
        }

        public async Task<string> GetLeaderboard(int challengeID)
        {
            string url = $"{apiBaseUrl}/challenge/leaderboard/{challengeID}";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("x-api-key", apiKey);
                var operation = request.SendWebRequest();

                while (!operation.isDone) await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError($"Error fetching leaderboard: {request.error}");
                    return null;
                }
            }
        }

        private async Task<string> SendPostRequest<T>(string url, T requestBody)
        {
            string jsonData = JsonConvert.SerializeObject(requestBody);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("x-api-key", apiKey);

                var operation = request.SendWebRequest();
                while (!operation.isDone) await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError($"Error: {request.error}");
                    return null;
                }
            }
        }
    }
}