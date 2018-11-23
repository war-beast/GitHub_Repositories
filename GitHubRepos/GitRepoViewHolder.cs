using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace GitHubRepos
{
    public class GitRepoViewHolder : RecyclerView.ViewHolder
    {
        public TextView repoNameView { get; private set; }
        public TextView ownerNameView { get; private set; }
        public TextView repoDescView { get; private set; }
        public TextView repoUrlView { get; private set; }
        public ImageView ownerAvatarView { get; private set; }
        public LinearLayout cardLayout { get; private set; }

        public GitRepoViewHolder(View itemView, System.Action<int> listener) : base(itemView) {
            repoNameView = itemView.FindViewById<TextView>(Resource.Id.repoNameView);
            ownerNameView = itemView.FindViewById<TextView>(Resource.Id.ownerNameView);
            repoDescView = itemView.FindViewById<TextView>(Resource.Id.repoDescView);
            repoUrlView = itemView.FindViewById<TextView>(Resource.Id.repoUrlView);
            ownerAvatarView = itemView.FindViewById<ImageView>(Resource.Id.ownerAvatarView);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}