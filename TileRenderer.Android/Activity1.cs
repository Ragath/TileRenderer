using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace TileRenderer.Android
{
    [Activity(Label = "TileRenderer.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Unspecified
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        const SystemUiFlags f =
            SystemUiFlags.LayoutStable
            | SystemUiFlags.LayoutHideNavigation
            | SystemUiFlags.LayoutFullscreen
            | SystemUiFlags.HideNavigation
            | SystemUiFlags.Fullscreen
            | SystemUiFlags.ImmersiveSticky;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Game1(game => new Microsoft.Xna.Framework.GraphicsDeviceManager(game)
            {
                SynchronizeWithVerticalRetrace = false,
                SupportedOrientations = Microsoft.Xna.Framework.DisplayOrientation.Default,
                IsFullScreen = true
            });
            SetContentView((View)g.Services.GetService(typeof(View)));

            HideNavbar();
            //HideSystemUI();
            Resumed += Activity1_Resumed;

            g.Run();
        }

        private void Activity1_Resumed(object sender, System.EventArgs e)
        {
            HideNavbar();
        }

        private void HideNavbar()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
                Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            else
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)f;
                if (ActionBar != null)
                    ActionBar.Hide();
            }
            Immersive = true;
        }
    }
}

