using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GitHubRepos.Adapters;
using GitHubRepos.Common;
using GitHubRepos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubRepos.Fragments
{
    public class SummaryFragment : Fragment
    {
        View fragmentView;
        private GitRepository repository;
        TextView repoNameViewSum;
        TextView ownerNameViewSum;
        TextView repoDescViewSum;
        TextView repoUrlViewSum;
        ImageView ownerAvatarViewSum;
        TextView repoLanguage;
        TextView repoWatchers;
        TextView repoStargazers;
        TextView repoForkCount;
        TextView repoOpenIssues;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static SummaryFragment NewInstance(GitRepository repoInfo)
        {
            var fragment = new SummaryFragment { Arguments = new Bundle(), repository = repoInfo };
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var summary = Task.Run(async () => {
                var apiReader = new GitApiReader();
                var info = await apiReader.GitRepoSummary(repository.full_name);
                if(info == null)
                {
                    Resources res = Activity.BaseContext.Resources;
                    Toast alert = Toast.MakeText(Activity.BaseContext, res.GetString(Resource.String.api_loading_error), ToastLength.Long);
                    alert.SetGravity(GravityFlags.Center, 0, 0);
                    alert.Show();
                }
                return info;
            }).Result;
            
            fragmentView = inflater.Inflate(Resource.Layout.summaryFragment, container, false);
            repoNameViewSum = fragmentView.FindViewById<TextView>(Resource.Id.repoNameViewSum);
            ownerNameViewSum = fragmentView.FindViewById<TextView>(Resource.Id.ownerNameViewSum);
            repoDescViewSum = fragmentView.FindViewById<TextView>(Resource.Id.repoDescViewSum);
            repoUrlViewSum = fragmentView.FindViewById<TextView>(Resource.Id.repoUrlViewSum);
            ownerAvatarViewSum = fragmentView.FindViewById<ImageView>(Resource.Id.ownerAvatarViewSum);
            repoLanguage = fragmentView.FindViewById<TextView>(Resource.Id.repoLanguage);
            repoStargazers = fragmentView.FindViewById<TextView>(Resource.Id.repoStargazers);
            repoWatchers = fragmentView.FindViewById<TextView>(Resource.Id.repoWatchers);
            repoForkCount = fragmentView.FindViewById<TextView>(Resource.Id.repoForkCount);
            repoOpenIssues = fragmentView.FindViewById<TextView>(Resource.Id.repoOpenIssues);

            repoNameViewSum.Text = repository.name;
            ownerNameViewSum.Text = repository.owner.login;
            repoDescViewSum.Text = repository.description;
            repoUrlViewSum.Text = repository.url;
            using (var avatarLoadr = new ImageLoader())
            {
                var imageBitmap = avatarLoadr.GetImageBitmapFromUrl(repository.owner.avatar_url);
                if (imageBitmap != null)
                    ownerAvatarViewSum.SetImageBitmap(imageBitmap);
            }
            repoLanguage.Text = summary.language;
            repoStargazers.Text = summary.stargazers_count.ToString();
            repoWatchers.Text = summary.watchers_count.ToString();
            repoForkCount.Text = summary.forks_count.ToString();
            repoOpenIssues.Text = summary.open_issues_count.ToString();

            return fragmentView;
        }
    }
}