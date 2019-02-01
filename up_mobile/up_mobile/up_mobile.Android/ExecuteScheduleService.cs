using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace up_mobile.Droid
{
    [Service]
    public class ExecuteScheduleService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    BackgroundTasks.TaskScheduler.ExecuteSchedule(_cts.Token).Wait();
                }
                catch (System.OperationCanceledException)
                {
                    var message = new BackgroundTasks.Messages.CancelExecuteScheduleMessage();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<BackgroundTasks.Messages.CancelExecuteScheduleMessage>(message, "CancelExecuteScheduleMessage");
                    });

                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        var message = new BackgroundTasks.Messages.CancelExecuteScheduleMessage();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send<BackgroundTasks.Messages.CancelExecuteScheduleMessage>(message, "CancelExecuteScheduleMessage");
                        });
                    }
                }
            }, _cts.Token);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }

            base.OnDestroy();
        }
    }
}