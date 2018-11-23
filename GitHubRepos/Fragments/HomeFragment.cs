using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GitHubRepos.Adapters;
using GitHubRepos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubRepos.Fragments
{
    public class HomeFragemnt : Fragment
    {
        private static RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private FragmentManager _fManager;
        private List<GitRepository> repositories;
        private RepositoryListAdapter adapter;
        private static ProgressBar repositoriesLoadingProgress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static HomeFragemnt NewInstance(FragmentManager fragmentManager) 
        {
            var fragment = new HomeFragemnt { Arguments = new Bundle(), _fManager = fragmentManager };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout.homeFragment, container, false);
            recyclerView = fragmentView.FindViewById<RecyclerView>(Resource.Id.repoListView);
            repositoriesLoadingProgress = fragmentView.FindViewById<ProgressBar>(Resource.Id.repositoriesLoadingProgress);

            var loadTask = Task.Run(async () => {
                var apiReader = new GitApiReader();
                var items = new List<GitRepository>();
                items = await apiReader.GetRepositories();
                if (items == null) {
                    Resources res = Activity.BaseContext.Resources;
                    Toast alert = Toast.MakeText(Activity.BaseContext, res.GetString(Resource.String.api_loading_error), ToastLength.Long);
                    alert.SetGravity(GravityFlags.Center, 0, 0);
                    alert.Show();
                }
                return items;
            }).ContinueWith(FillRecycler);

            layoutManager = new LinearLayoutManager(recyclerView.Context);
            recyclerView.SetLayoutManager(layoutManager);

            return fragmentView;
        }

        private void FillRecycler(Task<List<GitRepository>> obj)
        {
            Activity.RunOnUiThread(() =>
            {
                repositories = obj.Result;
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
            FragmentTransaction transaction = _fManager.BeginTransaction();
            transaction.Replace(Resource.Id.content_frame, itemFragment).AddToBackStack("summary").Commit();
        }
    }
}