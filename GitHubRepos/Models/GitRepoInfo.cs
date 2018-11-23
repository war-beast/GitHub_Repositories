

using Newtonsoft.Json;

namespace GitHubRepos.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GitRepository
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string name { get; set; } = "";
        [JsonProperty]
        public string full_name { get; set; } = "";
        [JsonProperty]
        public Owner owner { get; set; } = new Owner();
        [JsonProperty]
        public string description { get; set; } = "";
        [JsonProperty]
        public string url { get; set; } = "";
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Owner
    {
        [JsonProperty]
        public string avatar_url { get; set; } = "";
        [JsonProperty]
        public string login { get; set; } = "";
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GitRepoSummary
    {
        [JsonProperty]
        public string language { get; set; }
        [JsonProperty]
        public int stargazers_count { get; set; }
        [JsonProperty]
        public int watchers_count { get; set; }
        [JsonProperty]
        public int forks_count { get; set; }
        [JsonProperty]
        public int open_issues_count { get; set; }
    }
}