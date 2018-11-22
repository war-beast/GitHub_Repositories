using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace GitHubRepos.Fragments
{
    public class SettingsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static SettingsFragment NewInstance()
        {
            var frag2 = new SettingsFragment { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.settingsFragment, null);
        }
    }
}