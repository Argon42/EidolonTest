using System.Text;
using System.Threading.Tasks;
using Eidolon.Analytic;
using Eidolon.Common.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class AnalyticService : IAnalyticService
    {
        private const string ContentTypeValue = "application/json";
        private const string ContentType = "Content-Type";
        private readonly string _uri;
        public string ServiceName => "Base";

        public AnalyticService(string uri)
        {
            _uri = uri;
        }

        public async Task<bool> TrySendEvents(string events)
        {
            UnityWebRequest request = UnityWebRequest.Post(_uri, "");
            request.uploadHandler = new UploadHandlerRaw(GetBytes(events));
            request.uploadHandler.contentType = ContentTypeValue;
            request.SetRequestHeader(ContentType, ContentTypeValue);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            await operation;
            long responseCode = operation.webRequest.responseCode;
            string text = operation.webRequest.downloadHandler?.text;
            string error = operation.webRequest.error;
            operation.webRequest.Dispose();
            if (responseCode == 200)
                return true;
            Debug.LogError(
                $"Request to {_uri} return response code {responseCode}. Error={error}. Text={text}");
            return false;
        }

        private byte[] GetBytes(string json) => Encoding.UTF8.GetBytes(json);
    }
}