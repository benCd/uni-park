using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using Xamarin.Forms;
using UIKit;

namespace up_mobile.iOS
{
    public class iOSExecuteSchedule
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("ExecuteScheduleTask", OnExperiation);

            try
            {
                await Background.TaskScheduler.ExecuteSchedule(_cts.Token);
            } catch(OperationCanceledException)
            {
                var message = new Background.Messages.CancelExecuteScheduleMessage();
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send<Background.Messages.CancelExecuteScheduleMessage>(message, "CancelExecuteScheduleMessage");
                });
                
            }
            finally
            {
                if(_cts.IsCancellationRequested)
                {
                    var message = new Background.Messages.CancelExecuteScheduleMessage();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<Background.Messages.CancelExecuteScheduleMessage>(message, "CancelExecuteScheduleMessage");
                    });
                }
            }
        }

        private void OnExperiation()
        {
            var message = new Background.Messages.CancelExecuteScheduleMessage();
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send<Background.Messages.CancelExecuteScheduleMessage>(message, "CancelExecuteScheduleMessage");
            });
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}