﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace HelloRefit
{
    // See https://developer.github.com/v3/markdown/ 

    public class GitHubRenderRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("context")]
        public string Context { get; set; }
    }

    [Headers("User-Agent: HelloRefit")]
    public interface IGitHubApi
    {
        [Post("/markdown")]
        Task<string> RenderAsMarkdown([Body] GitHubRenderRequest request);
    }

    internal static class Program
    {
        private static readonly string baseUrl = "https://api.github.com";

        private static void Main(string[] args)
        {
            var md = "**Hello**, [Refit](https://github.com/paulcbetts/refit)!";
            var html = ConvertToHtml(md).Result;
            Console.WriteLine(html);

            Console.ReadKey();
        }

        private static async Task<string> ConvertToHtml(string markdown)
        {
            var api = RestService.For<IGitHubApi>(baseUrl);
            var request = new GitHubRenderRequest() { Text = markdown, Mode = "markdown" };

            return await api.RenderAsMarkdown(request);
        }
    }
}
