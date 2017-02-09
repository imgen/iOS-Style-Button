using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Crosswall;

namespace iOSStyleButton.Example
{
	[Activity(Label = "iOSStyleButton.Example", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/AppTheme")]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			IOSButton button = FindViewById<IOSButton>(Resource.Id.iosbutton_01);
			int count = 0;
			button.Click += delegate { 
				button.Text = string.Format("{0} clicks!", count++); 
			};
		}
	}
}

