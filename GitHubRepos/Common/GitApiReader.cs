using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GitHubRepos.Models;
using Newtonsoft.Json;

namespace GitHubRepos
{
    public class GitApiReader
    {
        private string repoListUrl = "https://api.github.com/repositories?page=1&per_page=";
        private string repoSummaryUrl = "https://api.github.com/repos/";
        private int repoItemsCount = 0;

        public GitApiReader()
        {
            repoItemsCount = AppSettings.RepoListCount;
        }

        public async Task<List<GitRepository>> GetRepositories()
        {
            return await Task.Run(() =>
            {
                var retList = new List<GitRepository>();
                string apiData = "";

                using (WebClient web = new WebClient())
                {
                    web.Encoding = UTF8Encoding.UTF8;
                    web.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.104");

                    string url = repoListUrl + repoItemsCount.ToString();
                    try
                    {
                        apiData = web.DownloadString(url);
                        var list = JsonConvert.DeserializeObject<List<GitRepository>>(apiData).Take(repoItemsCount);
                        retList.AddRange(list);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        throw (ex);
                    }
                }

                return retList;
            });
        }

        public async Task<GitRepoSummary> GitRepoSummary(string full_name)
        {
            return await Task.Run(() =>
            {
                var retVal = new GitRepoSummary();
                string apiData = "";

                using (WebClient web = new WebClient())
                {
                    web.Encoding = UTF8Encoding.UTF8;
                    web.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.104");

                    string url = repoSummaryUrl + full_name;
                    try
                    {
                        apiData = web.DownloadString(url);
                    }
                    catch (Exception)
                    {
                        apiData = "{}";
                    }
                }

                retVal = JsonConvert.DeserializeObject<GitRepoSummary>(apiData);
                return retVal;
            });
        }
    }
}