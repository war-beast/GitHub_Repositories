using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using GitHubRepos.Adapters;
using GitHubRepos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubRepos.Fragments
{
    public class HomeFragemnt : Fragment
    {
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager layoutManager;
        private List<GitRepository> repositories;
        private RepositoryListAdapter adapter;

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


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout.homeFragment, container, false);
            recyclerView = fragmentView.FindViewById<RecyclerView>(Resource.Id.repoListView);
            layoutManager = new LinearLayoutManager(recyclerView.Context);
            recyclerView.SetLayoutManager(layoutManager);

            repositories = Task.Run(async () => {
                var apiReader = new GitApiReader();
                var items = new List<GitRepository>();
                try { 
                    items = await apiReader.GetRepositories();
                }
                catch(Exception ex)
                {
                    string msg = ex.Message;
                }
                return items;
            }).Result;
            adapter = new RepositoryListAdapter(Activity.BaseContext, repositories);
            recyclerView.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;

            return fragmentView;
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            int id = e;
        }
    }
}