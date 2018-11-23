using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GitHubRepos.Adapters;
using GitHubRepos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubRepos.Fragments
{
    public class HomeFragemnt : Fragment
    {
        private static RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private List<GitRepository> repositories;
        private RepositoryListAdapter adapter;
        private static ProgressBar repositoriesLoadingProgress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static HomeFragemnt NewInstance() 
        {
            var fragment = new HomeFragemnt { Arguments = new Bundle() };
            return fragment;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            string serializedRepository = JsonConvert.SerializeObject(repositories);
            outState.PutString("repositories", serializedRepository);

            // always call the base implementation!
            base.OnSaveInstanceState(outState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (repositories == null && savedInstanceState != null)
            {
                string savedString = savedInstanceState.GetString("repositories", "{}");
                repositories = JsonConvert.DeserializeObject<List<GitRepository>>(savedString);
            }

            var fragmentView = inflater.Inflate(Resource.Layout.homeFragment, container, false);
            recyclerView = fragmentView.FindViewById<RecyclerView>(Resource.Id.repoListView);
            repositoriesLoadingProgress = fragmentView.FindViewById<ProgressBar>(Resource.Id.repositoriesLoadingProgress);

            if(repositories == null)
                StartLoadTask();
            else
                FillRecyclerView();

            layoutManager = new LinearLayoutManager(recyclerView.Context);
            recyclerView.SetLayoutManager(layoutManager);

            return fragmentView;
        }

        private void StartLoadTask()
        {
            var loadTask = Task.Run(async () =>
            {
                var apiReader = new GitApiReader();
                var items = new List<GitRepository>();
                items = await apiReader.GetRepositories();
                if ((int)items.Count == 0)
                {
                    Resources res = Activity.BaseContext.Resources;
                    Activity.RunOnUiThread(() =>
                    {
                        Toast alert = Toast.MakeText(Activity.BaseContext, res.GetString(Resource.String.api_loading_error), ToastLength.Long);
                        alert.SetGravity(GravityFlags.Center, 0, 0);
                        alert.Show();
                    });
                }
                return items;
            }).ContinueWith(EndLoadRepositories);
        }

        private void EndLoadRepositories(Task<List<GitRepository>> obj)
        {
            if (!obj.IsFaulted)
            {
                repositories = obj.Result;
            }

            FillRecyclerView();
        }

        private void FillRecyclerView()
        {
            Activity.RunOnUiThread(() =>
            {
                adapter = new RepositoryListAdapter(Activity.BaseContext, repositories);
                recyclerView.SetAdapter(adapter);
                adapter.ItemClick += Adapter_ItemClick;
                repositoriesLoadingProgress.Visibility = ViewStates.Gone;
            });
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            int id = e;
            Fragment itemFragment = SummaryFragment.NewInstance(repositories[e]);
            FragmentTransaction transaction = Activity.SupportFragmentManager.BeginTransaction();
            //FragmentTransaction transaction = _fManager.BeginTransaction();
            transaction.Replace(Resource.Id.content_frame, itemFragment).AddToBackStack("summary").Commit();
        }
    }
}