using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GitHubRepos
{
    public static class AppSettings
    {
        private static int defaultCount = 10;
        private static int _repoListCount;
        public static int RepoListCount
        {
            get
            {
                ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                _repoListCount = sharedPreferences.GetInt("repoListCount", defaultCount);
                return _repoListCount;
            }
            set
            {
                if (_repoListCount != value)
                {
                    _repoListCount = value;
                    ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                    ISharedPreferencesEditor preferencesEditor = sharedPreferences.Edit();
                    preferencesEditor.PutInt("repoListCount", _repoListCount);
                    preferencesEditor.Commit();
                }
            }
        }
    }
}