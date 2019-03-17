﻿using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace up_mobile.Helpers
{
    public class Permissions
    {
        public static async Task<bool> RequestStoragePermission(Page page)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await page.DisplayAlert("Storage permission", "Uni-Park will now request storage permission to store login info", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                await page.DisplayAlert("Storage permission denied", "Storage permission is required, please try again", "OK");
            }
            return false;
        }
    }
}
