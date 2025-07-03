using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System;

public static class Translator
{
    private const string API_URL = "https://libretranslate.de/translate";

    public static string LibreTranslate(string text, string targetLang = "en", string sourceLang = "ru")
    {
        if (string.IsNullOrEmpty(text) || text.Length < 1)
            return text;

        string jsonPayload = $"{{\"q\":\"{EscapeJsonString(text)}\",\"source\":\"{sourceLang}\",\"target\":\"{targetLang}\"}}";

        try
        {
            WebRequest request = WebRequest.Create(API_URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            request.ContentLength = byteArray.Length;
            
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string jsonResponse = reader.ReadToEnd();
                TranslationResult result = JsonUtility.FromJson<TranslationResult>(jsonResponse);
                return result.translatedText;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Translation failed: {e.Message}");
            return text;
        }
    }
    public static string TranslateGoogle(string text, string targetLang, string sourceLang = "auto")
    {
        string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";

        using (WebClient client = new WebClient())
        {
            client.Encoding = Encoding.UTF8;
            string result = client.DownloadString(url);

            // Парсинг ответа: [[["Переведенный текст","Оригинальный текст",,,]],null,"ru"]
            int start = result.IndexOf("\"") + 1;
            int end = result.IndexOf("\"", start);
            return result.Substring(start, end - start);
        }
    }

    private static string EscapeJsonString(string input)
    {
        return input.Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r");
    }

    [System.Serializable]
    private class TranslationResult
    {
        public string translatedText;
    }
}