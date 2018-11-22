using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GitHubRepos.Models;

namespace GitHubRepos.Adapters
{
    public class RepositoryListAdapter : RecyclerView.Adapter
    {
        private IList<GitRepository> Repositories { get; set; }
        private Context _context;

        public override int ItemCount => Repositories.Count;
        public event EventHandler<int> ItemClick;

        private void OnClick(EventArgs args, int position)
        {
            int rowNumber = position + 1;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public RepositoryListAdapter(Context context, IList<GitRepository> repositories)
        {
            Repositories = repositories;
            _context = context;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < Repositories.Count)
            {
                Resources res = _context.Resources;
                var vh = holder as GitRepoViewHolder;
                if (Repositories.Count == 0)
                    return;
                vh.repoNameView.Text = Repositories[position].name;
                vh.ownerNameView.Text = Repositories[position].owner.login;
                vh.repoDescView.Text = Repositories[position].description;
                vh.repoUrlView.Text = Repositories[position].url;
                vh.ownerAvatarView.SetImageURI(Android.Net.Uri.Parse(Repositories[position].owner.avatar_url));
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gitRepoItemView, parent, false);
            GitRepoViewHolder vh = new GitRepoViewHolder(itemView);
            return vh;
        }

        private void SetImage(string filePath, ImageView image)
        {
            int height = image.Height;
            int width = image.Width;

            //Bitmap bmp = Bitmap.CreateBitmap(;
            //image.SetImageBitmap(bmp);
            //bmp.Dispose();
        }
    }
}