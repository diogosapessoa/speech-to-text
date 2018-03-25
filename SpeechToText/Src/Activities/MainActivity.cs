using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Util;
using Android.Widget;

using SpeechToText.Src.Listeners;

namespace SpeechToText.Src.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        SpeechRecognizer Recognizer { get; set; }
        Intent SpeechIntent { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var BtnStartSpeech = FindViewById<Button>(Resource.Id.btn_start_speech);
            BtnStartSpeech.Click += BtnStartSpeech_Click;

            var recListener = new RecognitionListener();
            recListener.BeginSpeech += RecListener_BeginSpeech;
            recListener.EndSpeech += RecListener_EndSpeech;
            recListener.Error += RecListener_Error;
            recListener.Ready += RecListener_Ready;
            recListener.Recognized += RecListener_Recognized;

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            Recognizer.SetRecognitionListener(recListener);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, PackageName);
        }

        private void BtnStartSpeech_Click(object sender, System.EventArgs e) => Recognizer.StartListening(SpeechIntent);

        private void RecListener_Ready(object sender, Bundle e) => Log.Debug(nameof(MainActivity), nameof(RecListener_Ready));

        private void RecListener_BeginSpeech() => Log.Debug(nameof(MainActivity), nameof(RecListener_BeginSpeech));

        private void RecListener_EndSpeech() => Log.Debug(nameof(MainActivity), nameof(RecListener_EndSpeech));

        private void RecListener_Error(object sender, SpeechRecognizerError e) => Log.Debug(nameof(MainActivity), $"{nameof(RecListener_Error)}={e.ToString()}");

        private void RecListener_Recognized(object sender, string recognized) => Toast.MakeText(this, recognized, ToastLength.Long).Show();
    }
}