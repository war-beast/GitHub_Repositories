using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GitHubRepos.Common
{
    public class ImageLoader : IDisposable
    {
        Bitmap imageBitmap = null;

        public ImageLoader()
        {

        }

        public void Dispose()
        {
            imageBitmap.Dispose();
        }

        public Bitmap GetImageBitmapFromUrl(string url)
        {
            byte[] imageBytes = null;

            using (var webClient = new WebClient())
            {
                try
                {
                    imageBytes = webClient.DownloadData(url);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }


    }
}